using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Surface.Core;

namespace SurfaceUDPBridge
{
    class Program
    {
        private static UdpClient udpClient;
        private static TouchTarget touchTarget;
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Surface UDP Bridge starting...");
            
            try
            {
                // Initialize UDP client
                udpClient = new UdpClient();
                
                // Create a hidden form for TouchTarget
                var form = new System.Windows.Forms.Form();
                form.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                form.ShowInTaskbar = false;
                form.Visible = false;
                form.Show(); // Show the form to get a valid handle
                
                Console.WriteLine("Form handle: " + form.Handle);
                
                // Initialize Surface SDK exactly like CheeseEater.cs
                init(form.Handle);
                
                isRunning = true;
                Console.WriteLine("Surface UDP Bridge ready! Press any key to stop...");
                
                // Keep running until key press
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Cleanup();
            }
        }

        private static byte[] normalizedImage = null;
        private static ImageMetrics normalizedMetrics = null;
        private static int imageWidth = 1920;   // Raw image dimensions
        private static int imageHeight = 1080;
        private static int sensorWidth = 960;   // Blob detection dimensions (downsampled)
        private static int sensorHeight = 540;
        private static int frameCount = 0;
        
        // Constants from CheeseEater.cs
        private const byte brightnessThreshhold = 180;
        private const byte pixelCountThreshhold = 100;

        // Data structures for blob detection (from CheeseEater.cs)
        private static byte[] blobMembership = new byte[960 * 540];  // Holds the index of the blob the sensor is part of
        private static byte[] mergeTable = new byte[256];
        private static Blob[] tmpBlobs = new Blob[255];
        private static Blob[] validBlobs = new Blob[255];
        private static uint validBlobCount = 0;
        private const byte NULL_BLOB = 0xff;

        // Helper method from CheeseEater.cs
        private static uint SensorIdx(uint x, uint y)
        {
            return x + y * (uint)sensorWidth;
        }

        // Exact copy of CheeseEater.cs init method
        public static void init(IntPtr hwnd)
        {
            try
            {
                Console.WriteLine("Initializing Surface SDK with handle: " + hwnd);
                
                // Create a target for surface input.
                touchTarget = new TouchTarget(hwnd, EventThreadChoice.OnBackgroundThread);
                Console.WriteLine("TouchTarget created successfully");
                
                touchTarget.EnableInput();
                Console.WriteLine("Input enabled successfully");

                // Enable the normalized raw-image.
                touchTarget.EnableImage(ImageType.Normalized);
                Console.WriteLine("Image enabled successfully");

                // Hook up a callback to get notified when there is a new frame available
                touchTarget.FrameReceived += OnTouchTargetFrameReceived;
                Console.WriteLine("FrameReceived callback registered successfully");
                
                Console.WriteLine("TouchTarget initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in init: " + ex.GetType().Name + ": " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                throw;
            }
        }

        // Exact copy of CheeseEater.cs blob detection
        private static Blob[] ProcessImageDataLikeCheeseEater(byte[] normalizedImage)
        {
            // Downsample the image (1920x1080 -> 960x540) by taking every 2nd pixel
            byte[] downsampled = new byte[sensorWidth * sensorHeight];
            for (int y = 0; y < sensorHeight; y++)
            {
                for (int x = 0; x < sensorWidth; x++)
                {
                    int sourceX = x * 2;  // 2x downsampling
                    int sourceY = y * 2;
                    int sourceIndex = sourceX + sourceY * imageWidth;
                    int targetIndex = x + y * sensorWidth;
                    
                    if (sourceIndex < normalizedImage.Length)
                    {
                        downsampled[targetIndex] = normalizedImage[sourceIndex];
                    }
                }
            }

            uint tmpBlobCount = 0;
            for (uint y = 1; y < sensorHeight; y++)
            {
                for (uint x = 1; x < sensorWidth; x++)
                {
                    uint i = SensorIdx(x, y);
                    blobMembership[i] = NULL_BLOB;
                    if (downsampled[i] < brightnessThreshhold)
                        continue;
                    byte downBlobId = blobMembership[SensorIdx(x, y - 1)];
                    byte backBlobId = blobMembership[SensorIdx(x - 1, y)];
                    // if either is not NULL_BLOB make the sensor a member of one
                    if (downBlobId != NULL_BLOB || backBlobId != NULL_BLOB)
                    {
                        byte blobId = Math.Min(downBlobId, backBlobId);
                        blobMembership[SensorIdx(x, y)] = blobId;
                        // update blob
                        tmpBlobs[blobId].maxX = Math.Max(x, tmpBlobs[blobId].maxX);
                        tmpBlobs[blobId].minX = Math.Min(x, tmpBlobs[blobId].minX);
                        tmpBlobs[blobId].maxY = Math.Max(y, tmpBlobs[blobId].maxY);
                        tmpBlobs[blobId].minY = Math.Min(y, tmpBlobs[blobId].minY);
                        tmpBlobs[blobId].pixelCount += 1;
                        // if neither is NULL_BLOB, map higher index to lower index
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
                        // start new blob
                        if (tmpBlobCount == 255)
                            continue;
                        blobMembership[SensorIdx(x, y)] = (byte)tmpBlobCount;
                        tmpBlobs[tmpBlobCount] = new Blob(x, y);
                        mergeTable[tmpBlobCount] = (byte)tmpBlobCount;
                        tmpBlobCount += 1;
                    }
                }
            }
            
            // iter backwards if things fuck up
            for (uint revBlobId = 0; revBlobId < tmpBlobCount; revBlobId++)
            {
                byte blobId = (byte)(tmpBlobCount - revBlobId - 1);
                byte mergeTo = mergeTable[blobId];
                if (blobId == mergeTo) continue;
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
                tmpBlobs[mergeTo].maxX = Math.Max(tmpBlobs[mergeTo].maxX, tmpBlobs[blobId].maxX);
                tmpBlobs[mergeTo].maxY = Math.Max(tmpBlobs[mergeTo].maxY, tmpBlobs[blobId].maxY);
                tmpBlobs[mergeTo].minX = Math.Min(tmpBlobs[mergeTo].minX, tmpBlobs[blobId].minX);
                tmpBlobs[mergeTo].minY = Math.Min(tmpBlobs[mergeTo].minY, tmpBlobs[blobId].minY);
                tmpBlobs[mergeTo].pixelCount += tmpBlobs[blobId].pixelCount;
                tmpBlobs[blobId].pixelCount = 0;
            }

            validBlobCount = 0;
            for (int bi = 0; bi < tmpBlobCount; bi++)
            {
                if (tmpBlobs[bi].pixelCount == 0) continue;
                
                if (tmpBlobs[bi].pixelCount >= pixelCountThreshhold)
                {
                    validBlobs[validBlobCount] = tmpBlobs[bi];
                    validBlobCount += 1;
                }
            }

            // Convert to array for return
            Blob[] result = new Blob[validBlobCount];
            for (int i = 0; i < validBlobCount; i++)
            {
                result[i] = validBlobs[i];
            }
            return result;
        }

        private static void OnTouchTargetFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            frameCount++;
            Console.WriteLine("Frame received! Processing frame #" + frameCount + "...");
            try
            {
                // Get raw image data for blob detection
                if (normalizedImage == null)
                {
                    // Get raw image data for a specific area
                    e.TryGetRawImage(
                        ImageType.Normalized,
                        0, 0,
                        1920, 1080,
                        out normalizedImage,
                        out normalizedMetrics);
                }
                else
                {
                    // Get the updated raw image data for the specified area
                    e.UpdateRawImage(
                        ImageType.Normalized,
                        normalizedImage,
                        0, 0,
                        1920, 1080);
                }

                if (normalizedImage != null)
                {
                    Console.WriteLine("Processing image: " + imageWidth + "x" + imageHeight + ", data length: " + normalizedImage.Length);
                    if (normalizedMetrics != null)
                    {
                        Console.WriteLine("Image metrics - Width: " + normalizedMetrics.Width + ", Height: " + normalizedMetrics.Height + ", BitsPerPixel: " + normalizedMetrics.BitsPerPixel);
                    }
                    
                    // Debug: Check some pixel values
                    int sampleCount = Math.Min(100, normalizedImage.Length);
                    int brightPixels = 0;
                    int maxValue = 0;
                    int minValue = 255;
                    for (int i = 0; i < sampleCount; i++)
                    {
                        if (normalizedImage[i] >= 180) brightPixels++;
                        if (normalizedImage[i] > maxValue) maxValue = normalizedImage[i];
                        if (normalizedImage[i] < minValue) minValue = normalizedImage[i];
                    }
                    Console.WriteLine("Sample bright pixels: " + brightPixels + "/" + sampleCount + ", min: " + minValue + ", max: " + maxValue);
                    
                    // Debug: Check if there are ANY bright pixels in the entire image
                    int totalBrightPixels = 0;
                    int totalMediumPixels = 0;
                    int totalLowPixels = 0;
                    for (int i = 0; i < normalizedImage.Length; i++)
                    {
                        if (normalizedImage[i] >= 180) totalBrightPixels++;
                        else if (normalizedImage[i] >= 100) totalMediumPixels++;
                        else if (normalizedImage[i] >= 50) totalLowPixels++;
                    }
                    Console.WriteLine("Bright(>=180): " + totalBrightPixels + ", Medium(>=100): " + totalMediumPixels + ", Low(>=50): " + totalLowPixels + "/" + normalizedImage.Length);
                    
                    // Process image data exactly like CheeseEater.cs
                    var blobs = ProcessImageDataLikeCheeseEater(normalizedImage);
                    
                    Console.WriteLine("Found " + blobs.Length + " blobs");
                    
                    // Send touch data via UDP
                    for (int i = 0; i < blobs.Length; i++)
                    {
                        var blob = blobs[i];
                        // Calculate center position from blob bounds
                        float centerX = (float)((blob.maxX + blob.minX) / 2) / (float)sensorWidth;
                        float centerY = (float)((blob.maxY + blob.minY) / 2) / (float)sensorHeight;
                        SendTouchData("DOWN", centerX, centerY, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing frame: " + ex.Message);
            }
        }


        private static void SendTouchData(string action, float x, float y, int id)
        {
            try
            {
                string message = action + ":" + x + ":" + y + ":" + id;
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                
                udpClient.Send(data, data.Length, "127.0.0.1", 12345);
                Console.WriteLine("Sent: " + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending UDP: " + ex.Message);
            }
        }

        private static void Cleanup()
        {
            isRunning = false;
            
            if (touchTarget != null)
            {
                touchTarget.Dispose();
                touchTarget = null;
            }
            
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
            }
            
            Console.WriteLine("Surface UDP Bridge stopped.");
        }
    }

    // Exact copy of CheeseEater.cs Blob struct
    public struct Blob
    {
        public uint pixelCount;
        public uint minX;
        public uint minY;
        public uint maxX;
        public uint maxY;

        /// <summary>
        /// New blob from initial pixel x and y
        /// </summary>
        public Blob(uint x, uint y)
        {
            pixelCount = 0;
            minX = x;
            maxX = x;
            minY = y;
            maxY = y;
        }
    }

    public class TouchBlob
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int PixelCount { get; set; }
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public TouchBlob(int x, int y)
        {
            X = x;
            Y = y;
            MinX = x;
            MinY = y;
            MaxX = x;
            MaxY = y;
            PixelCount = 0;
        }

        public void AddPixel(int x, int y)
        {
            PixelCount++;
            MinX = Math.Min(MinX, x);
            MinY = Math.Min(MinY, y);
            MaxX = Math.Max(MaxX, x);
            MaxY = Math.Max(MaxY, y);
            
            // Update center
            X = (MinX + MaxX) / 2.0f;
            Y = (MinY + MaxY) / 2.0f;
        }
    }
}
