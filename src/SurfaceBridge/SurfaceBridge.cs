using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Surface.Core;

namespace SurfaceBridge
{
    public class SurfaceBridge
    {
        private TouchTarget touchTarget;
        private UdpClient udpClient;
        private IPEndPoint godotEndPoint;
        private bool isRunning = false;
        
        // Godot will listen on this port
        private const int GODOT_PORT = 12345;
        private const string GODOT_IP = "127.0.0.1"; // localhost for now
        
        public SurfaceBridge()
        {
            // Initialize UDP client to send data to Godot
            udpClient = new UdpClient();
            godotEndPoint = new IPEndPoint(IPAddress.Parse(GODOT_IP), GODOT_PORT);
            
            // Create a hidden window to receive touch events
            var form = new Form();
            form.WindowState = FormWindowState.Minimized;
            form.ShowInTaskbar = false;
            form.Visible = false;
            
            // Create TouchTarget for the window
            touchTarget = new TouchTarget(form.Handle, true);
            
            // Subscribe to touch events
            touchTarget.TouchDown += OnTouchDown;
            touchTarget.TouchMove += OnTouchMove;
            touchTarget.TouchUp += OnTouchUp;
            
            Console.WriteLine("Surface Bridge initialized. Waiting for touch events...");
            Console.WriteLine($"Sending data to Godot at {GODOT_IP}:{GODOT_PORT}");
        }
        
        public void Start()
        {
            isRunning = true;
            Application.Run();
        }
        
        public void Stop()
        {
            isRunning = false;
            touchTarget?.Dispose();
            udpClient?.Close();
        }
        
        private void OnTouchDown(object sender, TouchEventArgs e)
        {
            SendTouchData("DOWN", e.TouchPoint);
        }
        
        private void OnTouchMove(object sender, TouchEventArgs e)
        {
            SendTouchData("MOVE", e.TouchPoint);
        }
        
        private void OnTouchUp(object sender, TouchEventArgs e)
        {
            SendTouchData("UP", e.TouchPoint);
        }
        
        private void SendTouchData(string eventType, TouchPoint touchPoint)
        {
            if (!isRunning) return;
            
            try
            {
                // Create a simple message format: "EVENT_TYPE:X:Y:ID"
                string message = $"{eventType}:{touchPoint.X}:{touchPoint.Y}:{touchPoint.Id}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                
                udpClient.Send(data, data.Length, godotEndPoint);
                
                Console.WriteLine($"Sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data: {ex.Message}");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var bridge = new SurfaceBridge();
                
                Console.WriteLine("Press Ctrl+C to exit...");
                
                // Handle Ctrl+C gracefully
                Console.CancelKeyPress += (sender, e) => {
                    e.Cancel = true;
                    bridge.Stop();
                    Environment.Exit(0);
                };
                
                bridge.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Make sure you're running this on a Surface table with the Surface SDK installed.");
            }
        }
    }
}
