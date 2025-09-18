# Digosaur

Project for the advanced graphics and interaction course DH2413 at KTH

## Project Structure

```
src/
├── Digosaur/                    # Godot project for track visualization
│   ├── Main.tscn               # Main scene file
│   ├── mouse_track.gd          # Mouse input handler for track creation
│   ├── tracks.tres             # Shader for track rendering
│   ├── Images/                 # Textures for the ground
│   │   ├── snow_albedo.png     # Snow albedo texture
│   │   └── snow_height.png     # Snow height map
│   ├── 3D models/              # 3D model assets
│   │   └── dogbone3.glb        # Dog bone 3D model
│   ├── icon.svg                # Project icon
│   └── project.godot           # Godot project configuration
├── RawImageVisualizer/         # C# application for Surface table communication
│   ├── App1.cs                 # Main application logic
│   ├── Server.cs               # Network server implementation
│   ├── ServerThread.cs         # Server threading logic
│   ├── SocketThreadManager.cs  # Socket management
│   ├── StaticNetworkUtilities.cs # Network utilities
│   └── RawImageVisualizer.csproj # C# project file
├── Surface_table_code/         # Microsoft Surface SDK files
│   ├── Surface/                # SDK source code
│   │   ├── Microsoft.Surface/  # Surface application services
│   │   └── Microsoft.Surface.Core/ # Core Surface functionality
│   └── v2.0/                   # SDK runtime files
└── SurfaceTrackLibrary.dll     # Native DLL for Surface table integration
```

## Quick Start

1. **Clone the repository** to your development machine
2. **Build the C# application** (for Surface table communication):
   ```bash
   cd src/RawImageVisualizer
   msbuild RawImageVisualizer.csproj
   ```
3. **Start Godot**: Open `src/Digosaur/` in Godot 4.4
4. **Run the project**: The project uses mouse input to create tracks on the ground
5. **For Surface table integration**: Run the RawImageVisualizer application on the Surface table to enable touch input

## Features

- **Real-time track rendering** using custom shaders
- **Mouse input support** for track creation
- **Surface table integration** via RawImageVisualizer application
- **3D visualization** with customizable ground textures
- **Particle system** for track effects

## GitHub Workflow Guidelines

### Branch Names

First issue number, then some description

**Example:**
79-Fix_my_broken_thing

### Commits

#### 1. Tag

**feat (for adding a new feature):**
feat: Add function LCIx #x
Details: The implementation of the LIC9xfunction (including helper function)

**fix (for fixing a bug):**
fix: Resolve null pointer exception #x
Details: Adds a null check

**doc (for documentation changes):**
doc: add documentation and comments for LICx #x
Details: The detailed comments and documentation explaining the function, its inputs, and outputs.

**refactor: (for code restructuring without changing behaviour):**
refactor: Rename variable to use lowercase #x

**test: (for adding a test)**
test: Add test for LIC14

#### 2. At the end of the message:

If done: Resolves issue #45
If part of the solution: Relates to issue #45

### Pull request:

**Title: [tag]: Description of what was added**
Example: fix: Changes the parameter types of LIC5 and LIC7

**Comment:**
[Eventual info explaining the reason for this pull request]
Closes issue #45

### Merges

When the time to merge [Press green merge button]

**Title: Merge pull request # from "branch"**
Example: Merge pull request #69 from wys/issue/60

**Comment:**
[Eventual info explaining the reason for this pull request]
Closes issue #45

### Other things to think about

**Small letters for tags:** fix, feat doc, etc.

**Do not merge your own branch:** Make somebody else merge yours
