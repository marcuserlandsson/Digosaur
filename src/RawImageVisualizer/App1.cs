using System;
using Microsoft.Surface;
using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace RawImageVisualizer
{
    /// <summary>
    /// This is the main type for your application.
    /// </summary>
    public class App1 : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager graphics;
        private TouchTarget touchTarget;
        private bool applicationLoadCompleteSignalled;
        private SpriteBatch foregroundBatch;
        private string touchStatus = "No touch detected";
        
        // Touch coordinate storage for TCP communication
        public static int touchX = -1;
        public static int touchY = -1;
        public static int touchIntensity = 0;
        public static bool touchActive = false;
        
        // Multi-touch support
        public static List<TouchPoint> touchPoints = new List<TouchPoint>();
        
        public class TouchPoint
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Intensity { get; set; }
            public int Size { get; set; }
            
            public TouchPoint(int x, int y, int intensity, int size)
            {
                X = x;
                Y = y;
                Intensity = intensity;
                Size = size;
            }
        }

        // normalizedImageUpdated is accessed from differet threads. Mark it
        // volatile to make sure that every read gets the latest value.
        private volatile bool normalizedImageUpdated;

        // For Normalized Raw Image
        volatile public static byte[] normalizedImage;
        volatile public static byte[] normalizedImage_compressed=null;
        private long totalTime = 0;
        private int totalFrames = 0;
        volatile private bool compressing = false;
        private ImageMetrics normalizedMetrics;



        // Something to lock to deal with asynchronous frame updates
        private readonly object syncObject = new object();


        /// <summary>
        /// Default constructor.
        /// </summary>
        public App1()
        {
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: App1 constructor called");
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            SocketThreadManager.CreateServer("169.254.185.113", 666);
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: Server created");
            RawImageVisualizer.Program.sw.WriteLine("HELLO");
        }

        /// <summary>
        /// Allows the app to perform any initialization it needs to before starting to run.
        /// This is where it calls to loads amd Initializes SurfaceInput TouchTarget.  
        /// Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: Initialize() called");
            IsMouseVisible = true; // easier for debugging not to "lose" mouse
            SetWindowOnSurface();
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: Window set on surface");
            InitializeSurfaceInput();
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: Surface input initialized");

            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
            graphics.IsFullScreen = false; // Make window smaller so you can see console
            base.Initialize();
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            // Turn raw image back on again
            touchTarget.EnableImage(ImageType.Normalized);
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            // If the app isn't active, there's no need to keep the raw image enabled
            touchTarget.DisableImage(ImageType.Normalized);
        }

        /// <summary>
        /// Moves and sizes the window to cover the input surface.
        /// </summary>
        private void SetWindowOnSurface()
        {
            System.Diagnostics.Debug.Assert(Window != null && Window.Handle != IntPtr.Zero,
                "Window initialization must be complete before SetWindowOnSurface is called");
            if (Window == null || Window.Handle == IntPtr.Zero)
                return;

            // Get the window sized right.
            Program.InitializeWindow(Window);
            // Set the graphics device buffers.
            graphics.PreferredBackBufferWidth = Program.WindowSize.Width;
            graphics.PreferredBackBufferHeight = Program.WindowSize.Height;
            graphics.ApplyChanges();
            // Make sure the window is in the right location.
            Program.PositionWindow();
        }

        /// <summary>
        /// Initializes the surface input system. This should be called after any window
        /// initialization is done, and should only be called once.
        /// </summary>
        private void InitializeSurfaceInput()
        {
            System.Diagnostics.Debug.Assert(Window != null && Window.Handle != IntPtr.Zero,
                "Window initialization must be complete before InitializeSurfaceInput is called");
            if (Window == null || Window.Handle == IntPtr.Zero)
                return;
            System.Diagnostics.Debug.Assert(touchTarget == null,
                "Surface input already initialized");
            if (touchTarget != null)
                return;

            // Create a target for surface input.
            touchTarget = new TouchTarget(Window.Handle, EventThreadChoice.OnBackgroundThread);
            touchTarget.EnableInput();
            Console.WriteLine("DEBUG: TouchTarget created and input enabled");

            // Enable the normalized raw-image.
            touchTarget.EnableImage(ImageType.Normalized);
            Console.WriteLine("DEBUG: Normalized image enabled");

            // Hook up a callback to get notified when there is a new frame available
            touchTarget.FrameReceived += OnTouchTargetFrameReceived;
        }

        /// <summary>
        /// Handler for the FrameReceived event. 
        /// Here we get the rawimage data from FrameReceivedEventArgs object.
        /// </summary>
        /// <remarks>
        /// When a frame is received, this event handler gets the normalized image 
        /// from the TouchTarget. The image is copied into the touch sprite in the 
        /// Update method. The reason for this separation is that this event handler is 
        /// called from a background thread and Update is called from the main thread. It 
        /// is not safe to get the normalized image from the TouchTarget in the Update 
        /// method because TouchTarget and Update run on different threads. It is 
        /// possible for TouchTarget to change the image while Update is accessing it.
        /// 
        /// To address the threading issue, the raw image is retrieved here, on the same 
        /// thread that updates and uses it on the main thread. It is stored in a variable 
        /// that is available to both threads, and access to the variable is controlled 
        /// through lock statements so that only one thread can use the image at a time.
        /// </remarks>
        /// <param name="sender">TouchTarget that received the frame</param>
        /// <param name="e">Object containing information about the current frame</param>
        Stopwatch frameTimer = new Stopwatch();
        private void OnTouchTargetFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: OnTouchTargetFrameReceived called!");
            
            // Lock the syncObject object so normalizedImage isn't changed while the Update method is using it
            
            lock (syncObject)
            {
                if (normalizedImage == null)
                {
                    Console.WriteLine("iamm null");
                    // get rawimage data for a specific area
                    if (e.TryGetRawImage(
                            ImageType.Normalized,
                            0, 0,
                           1920,1080, //InteractiveSurface.PrimarySurfaceDevice.WorkingAreaWidth,
                           // InteractiveSurface.PrimarySurfaceDevice.WorkingAreaHeight,
                            out normalizedImage,
                            out normalizedMetrics))
                    {
                        // Image data initialized successfully
                    }
                }
                else
                {
                    // get the updated rawimage data for the specified area
                    e.UpdateRawImage(
                        ImageType.Normalized,
                        normalizedImage,
                        0, 0,
                        1920, 1080);// InteractiveSurface.PrimarySurfaceDevice.WorkingAreaWidth,
                        //InteractiveSurface.PrimarySurfaceDevice.WorkingAreaHeight);
                    
                    // Simple touch detection
                    DetectTouches();
                }
                frameTimer.Stop();
                if (!compressing)//ensures we don't create a backlog of threads
                {
                    compressing = true;
                    new Thread(delegate()
                    {
                        //Stopwatch stopwatch = Stopwatch.StartNew();
                        normalizedImage_compressed = StaticNetworkUtilities.compressFrame_assumeNoZero(normalizedImage);
                        compressing = false;
                        //stopwatch.Stop();
                        //Interlocked.Increment(ref totalFrames);
                        //Interlocked.Add(ref totalTime, stopwatch.ElapsedMilliseconds);
                        //RawImageVisualizer.Program.sw.WriteLine("Compression took: " + stopwatch.ElapsedMilliseconds+", average: " + ((double)totalTime)/(double)totalFrames);
                    }).Start();
                }
                else
                {
                    RawImageVisualizer.Program.sw.WriteLine("A frame was not compressed because the previous thread failed to complete in " + frameTimer.ElapsedMilliseconds + " ms");
                }
                frameTimer.Restart();

                normalizedImageUpdated = true;
            }
            
        }

        private void DetectTouches()
        {
            if (normalizedImage == null) 
            {
                RawImageVisualizer.Program.sw.WriteLine("DEBUG: normalizedImage is null");
                return;
            }
            
            // Quick check: if no pixels above threshold, skip expensive processing
            bool hasTouchPixels = false;
            for (int i = 0; i < normalizedImage.Length; i += 4) // Check every 4th pixel for speed
            {
                if (normalizedImage[i] > 50)
                {
                    hasTouchPixels = true;
                    break;
                }
            }
            
            if (!hasTouchPixels)
            {
                // No touch detected, clear previous touches
                touchPoints.Clear();
                touchX = -1;
                touchY = -1;
                touchIntensity = 0;
                touchActive = false;
                touchStatus = "No touch detected";
                return;
            }
            
            RawImageVisualizer.Program.sw.WriteLine("DEBUG: DetectTouches called, image length: " + normalizedImage.Length);
            
            const int TOUCH_THRESHOLD = 50;
            const int MIN_TOUCH_SIZE = 20; // Minimum pixels for a valid touch (increased)
            const int MAX_TOUCH_DISTANCE = 80; // Maximum distance between touches to merge
            const int MAX_TOUCHES = 5; // Maximum number of touches to detect
            
            int width = normalizedMetrics.Width;
            int height = normalizedMetrics.Height;
            
            // Clear previous touch points
            touchPoints.Clear();
            
            // Create a visited array to avoid processing the same blob twice
            bool[,] visited = new bool[width, height];
            
            // Find all touch blobs (optimized: skip every 2nd pixel for speed)
            for (int y = 0; y < height; y += 2)
            {
                for (int x = 0; x < width; x += 2)
                {
                    if (!visited[x, y] && normalizedImage[y * width + x] > TOUCH_THRESHOLD)
                    {
                        // Found a new touch blob, flood fill to find its boundaries
                        TouchPoint touch = FloodFillTouch(x, y, width, height, TOUCH_THRESHOLD, visited);
                        if (touch != null && touch.Size >= MIN_TOUCH_SIZE)
                        {
                            touchPoints.Add(touch);
                        }
                    }
                }
            }
            
            // Merge nearby touches (for hands that create multiple blobs)
            MergeNearbyTouches(MAX_TOUCH_DISTANCE);
            
            // Limit to maximum number of touches
            if (touchPoints.Count > MAX_TOUCHES)
            {
                // Keep only the largest touches
                touchPoints.Sort((a, b) => b.Size.CompareTo(a.Size));
                touchPoints = touchPoints.Take(MAX_TOUCHES).ToList();
            }
            
            // Update touch status
            if (touchPoints.Count > 0)
            {
                // For backward compatibility, use the first touch as primary
                TouchPoint primaryTouch = touchPoints[0];
                touchX = primaryTouch.X;
                touchY = primaryTouch.Y;
                touchIntensity = primaryTouch.Intensity;
                touchActive = true;
                
                touchStatus = $"Multi-touch detected! {touchPoints.Count} touches: ";
                foreach (var touch in touchPoints)
                {
                    touchStatus += $"({touch.X},{touch.Y}) ";
                }
                
                RawImageVisualizer.Program.sw.WriteLine(touchStatus);
                System.Diagnostics.Debug.WriteLine(touchStatus);
            }
            else
            {
                // No touch detected
                touchX = -1;
                touchY = -1;
                touchIntensity = 0;
                touchActive = false;
                touchStatus = "No touch detected";
            }
        }
        
        private TouchPoint FloodFillTouch(int startX, int startY, int width, int height, int threshold, bool[,] visited)
        {
            // Optimized flood fill - only check 4-connected neighbors (faster than 8-connected)
            long totalX = 0, totalY = 0, totalIntensity = 0;
            int pixelCount = 0;
            
            Queue<int> queueX = new Queue<int>();
            Queue<int> queueY = new Queue<int>();
            
            queueX.Enqueue(startX);
            queueY.Enqueue(startY);
            visited[startX, startY] = true;
            
            while (queueX.Count > 0 && pixelCount < 1000) // Limit to prevent infinite loops
            {
                int x = queueX.Dequeue();
                int y = queueY.Dequeue();
                
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    int intensity = normalizedImage[y * width + x];
                    if (intensity > threshold)
                    {
                        totalX += x * intensity;
                        totalY += y * intensity;
                        totalIntensity += intensity;
                        pixelCount++;
                        
                        // Check only 4-connected neighbors (faster than 8-connected)
                        if (x + 1 < width && !visited[x + 1, y])
                        {
                            visited[x + 1, y] = true;
                            queueX.Enqueue(x + 1);
                            queueY.Enqueue(y);
                        }
                        if (x - 1 >= 0 && !visited[x - 1, y])
                        {
                            visited[x - 1, y] = true;
                            queueX.Enqueue(x - 1);
                            queueY.Enqueue(y);
                        }
                        if (y + 1 < height && !visited[x, y + 1])
                        {
                            visited[x, y + 1] = true;
                            queueX.Enqueue(x);
                            queueY.Enqueue(y + 1);
                        }
                        if (y - 1 >= 0 && !visited[x, y - 1])
                        {
                            visited[x, y - 1] = true;
                            queueX.Enqueue(x);
                            queueY.Enqueue(y - 1);
                        }
                    }
                }
            }
            
            if (pixelCount > 0 && totalIntensity > 0)
            {
                int centerX = (int)(totalX / totalIntensity);
                int centerY = (int)(totalY / totalIntensity);
                int avgIntensity = (int)(totalIntensity / pixelCount);
                
                return new TouchPoint(centerX, centerY, avgIntensity, pixelCount);
            }
            
            return null;
        }
        
        private void MergeNearbyTouches(int maxDistance)
        {
            // Merge touches that are too close together (likely from the same hand)
            for (int i = touchPoints.Count - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    TouchPoint touch1 = touchPoints[i];
                    TouchPoint touch2 = touchPoints[j];
                    
                    // Calculate distance between touches
                    double distance = Math.Sqrt(Math.Pow(touch1.X - touch2.X, 2) + Math.Pow(touch1.Y - touch2.Y, 2));
                    
                    if (distance < maxDistance)
                    {
                        // Merge the two touches
                        int totalSize = touch1.Size + touch2.Size;
                        int mergedX = (touch1.X * touch1.Size + touch2.X * touch2.Size) / totalSize;
                        int mergedY = (touch1.Y * touch1.Size + touch2.Y * touch2.Size) / totalSize;
                        int mergedIntensity = (touch1.Intensity + touch2.Intensity) / 2;
                        
                        // Replace touch2 with merged touch, remove touch1
                        touchPoints[j] = new TouchPoint(mergedX, mergedY, mergedIntensity, totalSize);
                        touchPoints.RemoveAt(i);
                        break; // Move to next touch
                    }
                }
            }
        }

        void compressRawimage()
        {
            normalizedImage_compressed = StaticNetworkUtilities.compressFrame_assumeNoZero(normalizedImage);
        }
        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            foregroundBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the app to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Lock the syncObject object so the normalized image and metrics aren't updated while this method is using them
            lock (syncObject)
            {
                // Reset the flag - no graphics rendering needed
                normalizedImageUpdated = false;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the app should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!applicationLoadCompleteSignalled)
            {
                // Dismiss the loading screen now that we are starting to draw
                ApplicationServices.SignalApplicationLoadComplete();
                applicationLoadCompleteSignalled = true;
                
                // Make window completely transparent and unobtrusive
                System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // Remove border completely
                form.TopMost = false; // Don't keep it on top
            }

            // Simple black background
            graphics.GraphicsDevice.Clear(Color.Black);

            // Simple message - touch detection is printed to console
            // The window just shows that the app is running

            base.Draw(gameTime);
        }

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SocketThreadManager.Alive = false;
                Thread.Sleep(500);
                // Release  managed resources.
                if (foregroundBatch != null)
                {
                    foregroundBatch.Dispose();
                }
                if (touchTarget != null)
                {
                    touchTarget.Dispose();
                }

                IDisposable graphicsDispose = graphics as IDisposable;
                if (graphicsDispose != null)
                {
                    graphicsDispose.Dispose();
                }
            }

            // Release unmanaged Resources.

            base.Dispose(disposing);
        }

        #endregion
    }

}
