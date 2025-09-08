# MonoGame 2D Shooter Prototype

A 2D space shooter game prototype built with MonoGame Framework, featuring modern game development patterns and optimized performance.

## 🎮 Game Overview

This is a classic top-down 2D shooter where players control a spaceship to destroy incoming enemies. The game supports both single and multiplayer modes with customizable controls and settings.

### Key Features

- **Dual Player Support**: Two players can play simultaneously with separate controls
- **Optimized Performance**: Spatial grid collision detection and object pooling
- **Modern UI**: Clean menu system using GeonBit.UI
- **Customizable Settings**: Audio, video, gameplay, and input configuration
- **Visual Effects**: Particle effects, explosions, and bullet trails
- **Wave-based Gameplay**: Progressive difficulty with wave system
- **Combo System**: Score multipliers for consecutive hits

## 🛠️ Technical Architecture

### Core Technologies

- **Framework**: MonoGame 3.8.* (DesktopGL)
- **Target Platform**: .NET 8.0
- **UI Library**: 
  - GeonBit.UI 4.3.0.4 (All UI components)
- **Font Rendering**: FontStashSharp

### Project Structure

```
MonoGame2DShooterPrototype/
├── Source/
│   ├── Core/                    # Core game systems
│   │   ├── PrototypeMonoGame.cs # Main game class
│   │   ├── GameSettings.cs      # Settings management
│   │   ├── GameStateManager.cs  # Game state handling
│   │   └── PowerUpSystem.cs     # Power-up mechanics
│   ├── Entities/                # Game objects
│   │   ├── IEntity.cs          # Entity interface
│   │   ├── Player.cs           # Player character
│   │   ├── Enemy.cs            # Enemy entities
│   │   └── Bullet.cs           # Projectiles
│   ├── Managers/                # System managers
│   │   ├── EntityManager.cs    # Entity lifecycle
│   │   ├── BulletManager.cs    # Bullet pooling
│   │   ├── EnemyManager.cs     # Enemy spawning
│   │   ├── PlayerManager.cs    # Player management
│   │   └── CollisionManager.cs # Collision detection
│   ├── Screens/                 # Game screens
│   │   ├── IGameScreen.cs      # Screen interface
│   │   ├── MainMenuScreen.cs   # Main menu
│   │   ├── SettingsScreen.cs   # Settings menu
│   │   └── GameScreen.cs       # Gameplay screen
│   ├── UI/                      # UI components
│   │   ├── GameHUD.cs          # In-game HUD
│   │   ├── VisualEffects.cs    # Particle effects
│   │   ├── GameBackground.cs   # Background rendering
│   │   └── ScorePopupManager.cs # Score notifications
│   └── Utilities/               # Utility classes
│       └── SpatialGrid.cs      # Spatial partitioning
├── Content/                     # Game assets
│   ├── Fonts/                  # Font files
│   └── GeonBit.UI/             # UI themes and assets
└── Program.cs                   # Entry point
```

## 🎯 Core Systems

### 1. Entity System

The game uses a component-based entity system with the `IEntity` interface:

```csharp
public interface IEntity
{
    bool IsActive { get; set; }
    void Update(GameTime gameTime, int viewportWidth, int viewportHeight);
    void Draw(SpriteBatch spriteBatch);
}
```

**Key Entities:**
- **Player**: Controllable character with customizable input bindings
- **Enemy**: AI-controlled opponents with simple movement patterns
- **Bullet**: Projectiles with directional movement and collision

### 2. Performance Optimizations

#### Spatial Grid Collision Detection
- Uses a 64x64 pixel grid system for efficient collision detection
- Reduces collision checks from O(n²) to O(n) in most cases
- Separate grids for bullets and enemies

#### Object Pooling
- **Bullet Pool**: Pre-allocated bullets to avoid garbage collection
- **Enemy Pool**: Reusable enemy objects for consistent performance
- Pool size: 100 objects each (configurable)

### 3. Input System

Flexible input handling supporting both keyboard and controller:

```csharp
// Player 1 Default Controls (Keyboard)
W, A, S, D - Movement
Space - Shoot
Left Mouse - Directional shooting

// Player 2 Default Controls (Keyboard)
Arrow Keys - Movement
Enter - Shoot
Right Mouse - Directional shooting
```

### 4. Game State Management

Three main game states:
- **Playing**: Active gameplay
- **Paused**: Game paused (P key)
- **GameOver**: End state with restart option (R key)

### 5. Settings System

Persistent JSON-based configuration:

```json
{
  "AudioSettings": {
    "MasterVolume": 10,
    "MusicVolume": 10,
    "SfxVolume": 10
  },
  "VideoSettings": {
    "Fullscreen": false,
    "Resolution": "800x600"
  },
  "GameplaySettings": {
    "ShowFPS": true,
    "ShowDebugInfo": false,
    "ControllerSupport": true
  }
}
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- MonoGame development tools
- Visual Studio 2022 or VS Code (recommended)

### Building and Running

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MonoGame2DShooterPrototype
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the game**
   ```bash
   dotnet run
   ```

### Development Setup

The project includes VS Code configuration:
- **Launch configuration** for debugging
- **Build tasks** for compilation
- **Settings** for C# development

## 🎮 Gameplay Features

### Core Mechanics

- **Movement**: Smooth 8-directional movement with normalized vectors
- **Shooting**: Both keyboard and mouse-directed shooting
- **Collision**: Precise rectangle-based collision detection
- **Scoring**: Points awarded for enemy destruction with combo multipliers

### Visual Effects

- **Bullet Trails**: Dynamic trail effects for projectiles
- **Explosions**: Particle-based explosion effects
- **Background**: Animated starfield background
- **HUD**: Real-time display of health, score, wave info, and combo status

### Wave System

- Progressive difficulty with timed waves
- Enemy spawn rate increases with wave progression
- Wave completion triggers new challenges

## 🔧 Configuration

### Game Settings

Settings are automatically saved to `settings.json` and include:

- **Audio**: Master, music, and SFX volume controls
- **Video**: Resolution and fullscreen options
- **Gameplay**: FPS display, debug info, controller support
- **Input**: Customizable key bindings for both players

### Debug Features

Debug mode includes:
- **T Key**: Spawn 90 test bullets
- **E Key**: Spawn 100 test enemies
- **Performance Metrics**: Frame time monitoring
- **Debug Info**: Entity counts and system status

## 🏗️ Architecture Patterns

### Manager Pattern
Each major system has a dedicated manager class responsible for:
- Lifecycle management
- Resource allocation
- System-specific logic

### Object Pooling
Critical for performance in bullet-heavy gameplay:
- Pre-allocated object pools
- Efficient reuse without garbage collection
- Configurable pool sizes

### Spatial Partitioning
Optimizes collision detection:
- Grid-based spatial organization
- Reduced computational complexity
- Scalable to larger game worlds

### Screen Management
Clean separation of game states:
- Interface-based screen system
- Easy transitions between states
- Modular UI components

## 🎨 Asset Pipeline

### Content Management
- MonoGame Content Pipeline for asset processing
- TTF fonts for text rendering
- Procedurally generated textures for prototyping

### UI Theming
- GeonBit.UI theme system
- Consistent visual styling
- Customizable color schemes

## 🔍 Performance Considerations

### Optimizations Implemented

1. **Spatial Grid**: O(n) collision detection instead of O(n²)
2. **Object Pooling**: Eliminates garbage collection spikes
3. **Batch Processing**: Grouped entity updates and rendering
4. **Efficient Bounds Checking**: Quick viewport culling
5. **Minimal Allocations**: Reuse of Vector2 and Rectangle objects

### Performance Metrics

The game is designed to maintain 60 FPS with:
- 100+ active bullets
- 50+ active enemies
- Real-time collision detection
- Particle effects and visual feedback

## 🧪 Testing and Debug Features

### Debug Controls
- **T**: Spawn test bullets for performance testing
- **E**: Spawn test enemies for collision testing
- **P**: Pause/unpause gameplay
- **R**: Restart game (when game over)

### Performance Monitoring
- Frame time tracking
- Entity count display
- Memory usage indicators (debug mode)

## 🔮 Future Enhancements

### Planned Features
- **Audio System**: Sound effects and background music
- **Power-ups**: Temporary player enhancements
- **Multiple Enemy Types**: Varied AI behaviors and appearances
- **Boss Battles**: Large enemies with complex patterns
- **Weapon Upgrades**: Progressive player improvements
- **Persistent High Scores**: Local leaderboard system

### Technical Improvements
- **Content Pipeline**: Custom asset processing
- **Shader Effects**: Advanced visual effects
- **Networking**: Multiplayer support
- **Mobile Support**: Touch controls and optimization

## 📝 Contributing

### Code Style
- Follow C# naming conventions
- Use meaningful variable and method names
- Comment complex algorithms and business logic
- Maintain consistent indentation and formatting

### Architecture Guidelines
- Keep managers focused on single responsibilities
- Use interfaces for extensibility
- Implement proper error handling
- Write unit tests for core systems

## 📄 License

This project is a prototype for educational and demonstration purposes. Please refer to the license file for specific terms and conditions.

---

**Version**: 1.0  
**Last Updated**: 2024  
**MonoGame Version**: 3.8.*  
**Target Framework**: .NET 8.0