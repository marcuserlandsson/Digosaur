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

        private static void OnTouchTargetFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            try
            {
                // Get raw image data for blob detection
                if (e.TryGetRawImage(ImageType.Normalized))
                {
                    var imageData = e.GetRawImageData(ImageType.Normalized);
                    var blobs = ProcessImageData(imageData, e.GetRawImageWidth(ImageType.Normalized), e.GetRawImageHeight(ImageType.Normalized));
                    
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
            var visited = new bool[width * height];
            
            // Simple blob detection - find bright pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixelIndex = y * width + x;
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
                
                if (x < 0 || x >= width || y < 0 || y >= height) continue;
                if (visited[y * width + x]) continue;
                if (imageData[y * width + x] <= 200) continue;
                
                visited[y * width + x] = true;
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
