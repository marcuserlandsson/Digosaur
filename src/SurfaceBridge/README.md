# Surface Table Bridge

This is a simple bridge application that captures touch events from a Samsung SUR40 Surface table and sends them to Godot via UDP.

## How it works

1. **Surface Bridge (C#)**: Captures touch events from the Surface table using the Microsoft Surface SDK
2. **UDP Communication**: Sends touch data to Godot in a simple format: `EVENT_TYPE:X:Y:ID`
3. **Godot Integration**: Receives UDP packets and converts them to track particles

## Setup

### Prerequisites

- Samsung SUR40 Surface table
- .NET Framework 4.8 (comes with Windows 7/10/11)
- Godot 4.x
- Surface SDK files (included in this repo)

### Building the Bridge

```bash
cd src/SurfaceBridge
dotnet build
```

### Running the System

1. **Start Godot first** - Run your Godot project from `src/Digosaur/` with the surface_track.gd script
2. **Start the Bridge** - Run the C# bridge application:
   ```bash
   cd src/SurfaceBridge
   dotnet run
   ```

## Touch Event Format

The bridge sends UDP messages in this format:

- `DOWN:X:Y:ID` - Touch started
- `MOVE:X:Y:ID` - Touch moved
- `UP:X:Y:ID` - Touch ended

Where:

- `X, Y` are screen coordinates from the Surface table
- `ID` is the unique touch point identifier

## Configuration

- **UDP Port**: 12345 (configurable in both applications)
- **IP Address**: 127.0.0.1 (localhost)

## Troubleshooting

- Make sure the Surface SDK is properly installed
- Check that both applications are using the same UDP port
- Verify the Surface table is connected and recognized by Windows
- Check console output for error messages

## Notes

- The bridge creates a hidden window to receive touch events
- Touch coordinates are in the Surface table's native resolution
- The system supports multiple simultaneous touch points
- Press Ctrl+C to exit the bridge application
