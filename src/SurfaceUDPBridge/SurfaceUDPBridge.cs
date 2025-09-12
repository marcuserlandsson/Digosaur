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
        private static int touchIdCounter = 0;

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
                
                // Initialize Surface SDK
                touchTarget = new TouchTarget(form.Handle, EventThreadChoice.OnBackgroundThread);
                touchTarget.EnableInput();
                touchTarget.EnableImage(ImageType.Normalized);
                touchTarget.FrameReceived += OnTouchTargetFrameReceived;
                
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
        private static int sensorWidth = 1920;
        private static int sensorHeight = 1080;

        private static void OnTouchTargetFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            try
            {
                // Get raw image data for blob detection
                if (normalizedImage == null)
                {
                    // Get raw image data for a specific area
                    e.TryGetRawImage(
                        ImageType.Normalized,
                        0, 0,
                        sensorWidth, sensorHeight,
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
                        sensorWidth, sensorHeight);
                }

                if (normalizedImage != null)
                {
                    Console.WriteLine("Processing image: " + sensorWidth + "x" + sensorHeight + ", data length: " + normalizedImage.Length);
                    var blobs = ProcessImageData(normalizedImage, sensorWidth, sensorHeight);
                    
                    Console.WriteLine("Found " + blobs.Length + " blobs");
                    
                    // Send touch data via UDP
                    foreach (var blob in blobs)
                    {
                        SendTouchData("DOWN", blob.X, blob.Y, blob.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing frame: " + ex.Message);
            }
        }

        private static TouchBlob[] ProcessImageData(byte[] imageData, int width, int height)
        {
            var blobs = new System.Collections.Generic.List<TouchBlob>();
            
            // Check if image data is valid
            if (imageData == null || imageData.Length == 0)
            {
                Console.WriteLine("No image data available");
                return blobs.ToArray();
            }
            
            // Check if dimensions are valid
            if (width <= 0 || height <= 0 || imageData.Length < width * height)
            {
                Console.WriteLine("Invalid image dimensions: " + width + "x" + height + ", data length: " + imageData.Length);
                return blobs.ToArray();
            }
            
            var visited = new bool[width * height];
            
            // Simple blob detection - find bright pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixelIndex = y * width + x;
                    if (pixelIndex >= imageData.Length || pixelIndex >= visited.Length) continue;
                    if (visited[pixelIndex]) continue;
                    
                    if (imageData[pixelIndex] > 200) // Bright pixel threshold
                    {
                        var blob = new TouchBlob(x, y);
                        FloodFill(imageData, visited, x, y, width, height, blob);
                        
                        if (blob.PixelCount > 50) // Minimum blob size
                        {
                            blob.Id = ++touchIdCounter;
                            blobs.Add(blob);
                        }
                    }
                }
            }
            
            return blobs.ToArray();
        }

        private static void FloodFill(byte[] imageData, bool[] visited, int startX, int startY, int width, int height, TouchBlob blob)
        {
            var stack = new System.Collections.Generic.Stack<System.Drawing.Point>();
            stack.Push(new System.Drawing.Point(startX, startY));
            
            while (stack.Count > 0)
            {
                var point = stack.Pop();
                int x = point.X;
                int y = point.Y;
                
                // Bounds checking
                if (x < 0 || x >= width || y < 0 || y >= height) continue;
                
                int pixelIndex = y * width + x;
                if (pixelIndex >= imageData.Length || pixelIndex >= visited.Length) continue;
                
                if (visited[pixelIndex]) continue;
                if (imageData[pixelIndex] <= 200) continue;
                
                visited[pixelIndex] = true;
                blob.AddPixel(x, y);
                
                // Add neighbors
                stack.Push(new System.Drawing.Point(x + 1, y));
                stack.Push(new System.Drawing.Point(x - 1, y));
                stack.Push(new System.Drawing.Point(x, y + 1));
                stack.Push(new System.Drawing.Point(x, y - 1));
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
