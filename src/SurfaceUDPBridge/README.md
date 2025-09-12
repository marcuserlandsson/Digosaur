# Surface UDP Bridge

A simple UDP bridge that connects the Samsung SUR40 Surface table to Godot for touch detection.

## How It Works

1. **Detects touches** from the Surface table using the Surface SDK
2. **Processes raw image data** to find touch blobs
3. **Sends UDP messages** to Godot on port 12345
4. **Godot receives** the touch data and creates tracks

## UDP Message Format

- `DOWN:X:Y:ID` - Touch started at position (X,Y) with ID
- `MOVE:X:Y:ID` - Touch moved to position (X,Y) with ID
- `UP:ID` - Touch ended with ID

## Building

### On Windows 7 (Surface Table):

```cmd
"C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe" /target:exe /out:SurfaceUDPBridge.exe /reference:"C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.Surface.Core\v4.0_2.0.0.0__31bf3856ad364e35\Microsoft.Surface.Core.dll" /reference:"C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.Surface\v4.0_2.0.0.0__31bf3856ad364e35\Microsoft.Surface.dll" /reference:"System.Windows.Forms.dll" SurfaceUDPBridge.cs Properties\AssemblyInfo.cs
```

## Running

1. **Start the UDP bridge:**

   ```cmd
   SurfaceUDPBridge.exe
   ```

2. **Start Godot project** - The Godot project will listen on port 12345

3. **Touch the Surface table** - You should see tracks appear in Godot!

## Requirements

- Windows 7 (Surface table)
- .NET Framework 3.5 or 4.0
- Microsoft Surface SDK
- Godot 4.4

## Troubleshooting

- **No touches detected:** Check if Surface SDK is properly installed
- **UDP connection failed:** Make sure port 12345 is not blocked by firewall
- **Godot not receiving data:** Check console output for error messages
