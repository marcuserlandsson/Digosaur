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

        public TouchBlob(uint x, uint y)
        {
            pixelCount = 0;
            minX = x;
            minY = y;
            maxX = x;
            maxY = y;
        }
    }

    public class SurfaceTrack
    {
        private static TouchTarget touchTarget;
        private static ImageMetrics normalizedMetrics;
        private static byte[] normalizedImage;
        private static TouchBlob[] validBlobs = new TouchBlob[255];
        private static uint validBlobCount = 0;
        private static Mutex mutex = new Mutex();
        private static bool isInitialized = false;

        public static bool Initialize(IntPtr hwnd)
        {
            try
            {
                if (isInitialized)
                    return true;

                touchTarget = new TouchTarget(hwnd, EventThreadChoice.OnBackgroundThread);
                touchTarget.EnableInput();
                touchTarget.EnableImage(ImageType.Normalized);
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

        public static uint GetBlobCount()
        {
            mutex.WaitOne();
            uint count = validBlobCount;
            mutex.ReleaseMutex();
            return count;
        }

        public static float GetBlobX(uint index)
        {
            if (index >= validBlobCount)
                return 0.0f;

            mutex.WaitOne();
            float x = (float)validBlobs[index].minX / normalizedMetrics.Width;
            mutex.ReleaseMutex();
            return x;
        }

        public static float GetBlobY(uint index)
        {
            if (index >= validBlobCount)
                return 0.0f;

            mutex.WaitOne();
            float y = (float)validBlobs[index].minY / normalizedMetrics.Height;
            mutex.ReleaseMutex();
            return y;
        }

        public static float GetBlobSizeX(uint index)
        {
            if (index >= validBlobCount)
                return 0.0f;

            mutex.WaitOne();
            float sizeX = (float)(validBlobs[index].maxX - validBlobs[index].minX) / normalizedMetrics.Width;
            mutex.ReleaseMutex();
            return sizeX;
        }

        public static float GetBlobSizeY(uint index)
        {
            if (index >= validBlobCount)
                return 0.0f;

            mutex.WaitOne();
            float sizeY = (float)(validBlobs[index].maxY - validBlobs[index].minY) / normalizedMetrics.Height;
            mutex.ReleaseMutex();
            return sizeY;
        }

        private static void OnTouchTargetFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            try
            {
                if (normalizedImage == null)
                {
                    // get rawimage data for a specific area
                    e.TryGetRawImage(
                        ImageType.Normalized,
                        0, 0,
                        1920, 1080,
                        out normalizedImage,
                        out normalizedMetrics);
                }
                else
                {
                    // get the updated rawimage data for the specified area
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
            TouchBlob[] tmpBlobs = new TouchBlob[255];

            // Simple blob detection - find bright pixels
            for (uint y = 0; y < normalizedMetrics.Height; y++)
            {
                for (uint x = 0; x < normalizedMetrics.Width; x++)
                {
                    uint pixelIndex = (y * normalizedMetrics.Width + x) * 4;
                    if (pixelIndex + 2 < normalizedImage.Length)
                    {
                        byte r = normalizedImage[pixelIndex + 2];
                        byte g = normalizedImage[pixelIndex + 1];
                        byte b = normalizedImage[pixelIndex];

                        // Simple threshold - if pixel is bright enough, consider it a touch
                        if (r > 200 && g > 200 && b > 200)
                        {
                            // Add to existing blob or create new one
                            bool addedToExisting = false;
                            for (uint i = 0; i < tmpBlobCount; i++)
                            {
                                if (Math.Abs((int)x - (int)tmpBlobs[i].minX) < 20 && 
                                    Math.Abs((int)y - (int)tmpBlobs[i].minY) < 20)
                                {
                                    tmpBlobs[i].pixelCount++;
                                    tmpBlobs[i].minX = (uint)Math.Min((int)tmpBlobs[i].minX, (int)x);
                                    tmpBlobs[i].minY = (uint)Math.Min((int)tmpBlobs[i].minY, (int)y);
                                    tmpBlobs[i].maxX = (uint)Math.Max((int)tmpBlobs[i].maxX, (int)x);
                                    tmpBlobs[i].maxY = (uint)Math.Max((int)tmpBlobs[i].maxY, (int)y);
                                    addedToExisting = true;
                                    break;
                                }
                            }

                            if (!addedToExisting && tmpBlobCount < 255)
                            {
                                tmpBlobs[tmpBlobCount] = new TouchBlob(x, y);
                                tmpBlobs[tmpBlobCount].pixelCount = 1;
                                tmpBlobCount++;
                            }
                        }
                    }
                }
            }

            // Update global blob data
            mutex.WaitOne();
            validBlobCount = tmpBlobCount;
            Array.Copy(tmpBlobs, validBlobs, tmpBlobCount);
            mutex.ReleaseMutex();
        }
    }
}