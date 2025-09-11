using System;
using System.Threading;
using Microsoft.Surface.Core;

namespace SurfaceTrackLibrary
{
    public struct TouchBlob
    {
        public uint pixelCount;
        public uint minX;
        public uint minY;
        public uint maxX;
        public uint maxY;
        public bool isActive;

        public TouchBlob(uint x, uint y)
        {
            pixelCount = 0;
            minX = x;
            maxX = x;
            minY = y;
            maxY = y;
            isActive = false;
        }
    }

    public class SurfaceTrackLibrary
    {
        private const uint sensorWidth = 960;
        private const uint sensorHeight = 540;
        private const uint sensorCount = sensorWidth * sensorHeight;
        private const byte brightnessThreshold = 180;
        private const byte pixelCountThreshold = 100;
        private const byte NULL_BLOB = 0xff;

        private static TouchTarget touchTarget;
        private static ImageMetrics normalizedMetrics;
        private static byte[] normalizedImage;

        private static byte[] blobMembership = new byte[960 * 540];
        private static byte[] mergeTable = new byte[256];
        private static TouchBlob[] tmpBlobs = new TouchBlob[255];
        private static TouchBlob[] validBlobs = new TouchBlob[255];
        private static uint validBlobCount = 0;

        private static Mutex mutex = new Mutex();
        private static bool isInitialized = false;

        [RGiesecke.DllExport.DllExport]
        public static bool Initialize(IntPtr hwnd)
        {
            try
            {
                if (isInitialized) return true;

                // Create a target for surface input
                touchTarget = new TouchTarget(hwnd, EventThreadChoice.OnBackgroundThread);
                touchTarget.EnableInput();

                // Enable the normalized raw-image
                touchTarget.EnableImage(ImageType.Normalized);

                // Hook up callback for new frames
                touchTarget.FrameReceived += OnTouchTargetFrameReceived;

                isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SurfaceTrackLibrary initialization failed: " + ex.Message);
                return false;
            }
        }

        [RGiesecke.DllExport.DllExport]
        public static void Shutdown()
        {
            if (touchTarget != null)
            {
                touchTarget.Dispose();
                touchTarget = null;
            }
            isInitialized = false;
        }

        [RGiesecke.DllExport.DllExport]
        public static uint GetTouchCount()
        {
            if (!isInitialized) return 0;
            
            mutex.WaitOne();
            uint count = validBlobCount;
            mutex.ReleaseMutex();
            return count;
        }

        [RGiesecke.DllExport.DllExport]
        public static float GetTouchX(uint index)
        {
            if (!isInitialized || index >= validBlobCount) return 0.0f;
            
            mutex.WaitOne();
            float x = (float)((validBlobs[index].maxX + validBlobs[index].minX) / 2) / (float)sensorWidth;
            mutex.ReleaseMutex();
            return x;
        }

        [RGiesecke.DllExport.DllExport]
        public static float GetTouchY(uint index)
        {
            if (!isInitialized || index >= validBlobCount) return 0.0f;
            
            mutex.WaitOne();
            float y = (float)((validBlobs[index].maxY + validBlobs[index].minY) / 2) / (float)sensorHeight;
            mutex.ReleaseMutex();
            return y;
        }

        [RGiesecke.DllExport.DllExport]
        public static float GetTouchSizeX(uint index)
        {
            if (!isInitialized || index >= validBlobCount) return 0.0f;
            
            mutex.WaitOne();
            float sizeX = (float)(validBlobs[index].maxX - validBlobs[index].minX) / (float)sensorWidth;
            mutex.ReleaseMutex();
            return sizeX;
        }

        [RGiesecke.DllExport.DllExport]
        public static float GetTouchSizeY(uint index)
        {
            if (!isInitialized || index >= validBlobCount) return 0.0f;
            
            mutex.WaitOne();
            float sizeY = (float)(validBlobs[index].maxY - validBlobs[index].minY) / (float)sensorHeight;
            mutex.ReleaseMutex();
            return sizeY;
        }

        [RGiesecke.DllExport.DllExport]
        public static bool IsTouchActive(uint index)
        {
            if (!isInitialized || index >= validBlobCount) return false;
            
            mutex.WaitOne();
            bool active = validBlobs[index].isActive;
            mutex.ReleaseMutex();
            return active;
        }

        private static uint SensorIdx(uint x, uint y)
        {
            return x + y * sensorWidth;
        }

        private static void OnTouchTargetFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            if (!isInitialized) return;

            try
            {
                if (normalizedImage == null)
                {
                    // Get raw image data for the first time
                    e.TryGetRawImage(
                        ImageType.Normalized,
                        0, 0,
                        1920, 1080,
                        out normalizedImage,
                        out normalizedMetrics);
                }
                else
                {
                    // Update the raw image data
                    e.UpdateRawImage(
                        ImageType.Normalized,
                        normalizedImage,
                        0, 0,
                        1920, 1080);
                }

                ProcessImageData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing frame: " + ex.Message);
            }
        }

        private static void ProcessImageData()
        {
            uint tmpBlobCount = 0;
            
            // Reset blob membership
            for (uint i = 0; i < sensorCount; i++)
            {
                blobMembership[i] = NULL_BLOB;
            }

            // Process each pixel to find blobs
            for (uint y = 1; y < sensorHeight; y++)
            {
                for (uint x = 1; x < sensorWidth; x++)
                {
                    uint i = SensorIdx(x, y);
                    
                    // Skip if pixel is too bright (not a touch)
                    if (normalizedImage[i] >= brightnessThreshold)
                        continue;

                    byte downBlobId = blobMembership[SensorIdx(x, y - 1)];
                    byte backBlobId = blobMembership[SensorIdx(x - 1, y)];

                    // If either neighbor is part of a blob, join that blob
                    if (downBlobId != NULL_BLOB || backBlobId != NULL_BLOB)
                    {
                        byte blobId = Math.Min(downBlobId, backBlobId);
                        blobMembership[i] = blobId;
                        
                        // Update blob bounds
                        tmpBlobs[blobId].maxX = Math.Max(x, tmpBlobs[blobId].maxX);
                        tmpBlobs[blobId].minX = Math.Min(x, tmpBlobs[blobId].minX);
                        tmpBlobs[blobId].maxY = Math.Max(y, tmpBlobs[blobId].maxY);
                        tmpBlobs[blobId].minY = Math.Min(y, tmpBlobs[blobId].minY);
                        tmpBlobs[blobId].pixelCount += 1;

                        // Merge blobs if needed
                        if (downBlobId != backBlobId && downBlobId != NULL_BLOB && backBlobId != NULL_BLOB)
                        {
                            while (blobId != mergeTable[blobId])
                            {
                                blobId = mergeTable[blobId];
                            }
                            mergeTable[Math.Max(downBlobId, backBlobId)] = blobId;
                        }
                    }
                    else
                    {
                        // Start new blob
                        if (tmpBlobCount < 255)
                        {
                            blobMembership[i] = (byte)tmpBlobCount;
                            tmpBlobs[tmpBlobCount] = new TouchBlob(x, y);
                            mergeTable[tmpBlobCount] = (byte)tmpBlobCount;
                            tmpBlobCount++;
                        }
                    }
                }
            }

            // Merge overlapping blobs
            for (uint revBlobId = 0; revBlobId < tmpBlobCount; revBlobId++)
            {
                byte blobId = (byte)(tmpBlobCount - revBlobId - 1);
                byte mergeTo = mergeTable[blobId];
                if (blobId == mergeTo) continue;

                // Update blob membership
                for (uint x = tmpBlobs[blobId].minX; x <= tmpBlobs[blobId].maxX; x++)
                {
                    for (uint y = tmpBlobs[blobId].minY; y <= tmpBlobs[blobId].maxY; y++)
                    {
                        if (blobMembership[SensorIdx(x, y)] == blobId)
                        {
                            blobMembership[SensorIdx(x, y)] = mergeTo;
                        }
                    }
                }

                // Merge blob data
                tmpBlobs[mergeTo].maxX = Math.Max(tmpBlobs[mergeTo].maxX, tmpBlobs[blobId].maxX);
                tmpBlobs[mergeTo].maxY = Math.Max(tmpBlobs[mergeTo].maxY, tmpBlobs[blobId].maxY);
                tmpBlobs[mergeTo].minX = Math.Min(tmpBlobs[mergeTo].minX, tmpBlobs[blobId].minX);
                tmpBlobs[mergeTo].minY = Math.Min(tmpBlobs[mergeTo].minY, tmpBlobs[blobId].minY);
                tmpBlobs[mergeTo].pixelCount += tmpBlobs[blobId].pixelCount;
                tmpBlobs[blobId].pixelCount = 0;
            }

            // Update valid blobs
            mutex.WaitOne();
            validBlobCount = 0;
            for (int bi = 0; bi < tmpBlobCount; bi++)
            {
                if (tmpBlobs[bi].pixelCount == 0) continue;
                
                if (tmpBlobs[bi].pixelCount >= pixelCountThreshold)
                {
                    validBlobs[validBlobCount] = tmpBlobs[bi];
                    validBlobs[validBlobCount].isActive = true;
                    validBlobCount++;
                }
            }
            mutex.ReleaseMutex();
        }
    }
}
