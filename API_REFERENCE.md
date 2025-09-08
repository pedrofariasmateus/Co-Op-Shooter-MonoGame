# API Reference - MonoGame 2D Shooter Prototype

## 📋 Table of Contents

1. [Core Interfaces](#core-interfaces)
2. [Entity Classes](#entity-classes)
3. [Manager Classes](#manager-classes)
4. [Screen Classes](#screen-classes)
5. [UI Components](#ui-components)
6. [Utility Classes](#utility-classes)
7. [Settings Classes](#settings-classes)

## 🔌 Core Interfaces

### IEntity

Base interface for all game entities.

```csharp
public interface IEntity
{
    bool IsActive { get; set; }
    void Update(GameTime gameTime, int viewportWidth, int viewportHeight);
    void Draw(SpriteBatch spriteBatch);
}
```

**Properties:**
- `IsActive`: Determines if the entity should be updated and rendered

**Methods:**
- `Update(GameTime, int, int)`: Called each frame to update entity state
- `Draw(SpriteBatch)`: Called each frame to render the entity

### IGameScreen

Interface for game screens (menu, settings, gameplay).

```csharp
public interface IGameScreen
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
```

**Methods:**
- `Update(GameTime)`: Update screen logic
- `Draw(SpriteBatch)`: Render screen content

## 🎯 Entity Classes

### Player

Represents a player-controlled character.

```csharp
public class Player : IEntity
{
    public Vector2 Position;
    public float Speed = 200f;
    public Texture2D Texture;
    public PlayerInputSettings InputSettings;
    public bool IsActive { get; set; } = true;
}
```

**Properties:**
- `Position`: Current world position
- `Speed`: Movement speed in pixels per second
- `Texture`: Visual representation
- `InputSettings`: Input configuration for this player

**Constructor:**
```csharp
Player(Texture2D texture, Vector2 startPosition, PlayerInputSettings inputSettings, 
       int screenWidth = 800, int screenHeight = 600)
```

### Enemy

Represents an AI-controlled enemy.

```csharp
public class Enemy : IEntity
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsActive { get; set; } = true;
    public int Health { get; set; } = 1;
}
```

**Properties:**
- `Texture`: Visual representation
- `Position`: Current world position
- `Velocity`: Movement vector in pixels per second
- `Health`: Hit points remaining

**Methods:**
- `GetBounds()`: Returns collision rectangle

**Constructor:**
```csharp
Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
```

### Bullet

Represents a projectile.

```csharp
public class Bullet : IEntity
{
    public Vector2 Position;
    public Vector2 Direction;
    public float Speed = 400f;
    public Texture2D Texture;
    public bool IsActive = true;
}
```

**Properties:**
- `Position`: Current world position
- `Direction`: Normalized movement direction
- `Speed`: Movement speed in pixels per second
- `Texture`: Visual representation

**Methods:**
- `GetBounds()`: Returns collision rectangle

**Constructor:**
```csharp
Bullet(Texture2D texture, Vector2 position, Vector2 direction, 
       int screenWidth, int screenHeight)
```

## 🎮 Manager Classes

### EntityManager

Manages the lifecycle of all game entities.

```csharp
public class EntityManager
{
    public BulletManager BulletManager { get; set; }
    public EnemyManager EnemyManager { get; set; }
}
```

**Methods:**
- `Register(IEntity entity)`: Add entity to management
- `Update(GameTime, int, int)`: Update all active entities
- `Draw(SpriteBatch)`: Render all active entities
- `GetEntities()`: Get list of all managed entities
- `GetActiveEntities()`: Get list of active entities only

### BulletManager

Manages bullet entities with object pooling.

```csharp
public class BulletManager
{
    public BulletManager(Texture2D bulletTexture, int screenWidth, int screenHeight, int poolSize)
}
```

**Methods:**
- `SpawnBullet(Vector2 position, Vector2 direction)`: Create new bullet
- `SpawnTestBullets(int count)`: Create multiple bullets for testing
- `ReturnToPool(Bullet bullet)`: Return bullet to pool for reuse

### EnemyManager

Manages enemy spawning and lifecycle.

```csharp
public class EnemyManager
{
    public EnemyManager(Texture2D enemyTexture, GraphicsDevice graphicsDevice)
}
```

**Methods:**
- `UpdateAndMaybeSpawn(GameTime)`: Update spawn timer and potentially spawn enemy
- `SpawnEnemy()`: Create single enemy
- `SpawnTestEnemies(int count)`: Create multiple enemies for testing
- `ReturnToPool(Enemy enemy)`: Return enemy to pool for reuse

### PlayerManager

Static utility for creating player instances.

```csharp
public static class PlayerManager
{
    public static List<IEntity> CreatePlayers(Texture2D playerTexture, GameSettings settings, 
                                            int viewportWidth, int viewportHeight)
}
```

**Methods:**
- `CreatePlayers(...)`: Create player instances based on settings

### CollisionManager

Handles collision detection between entity types.

```csharp
public static class CollisionManager
{
    public static List<CollisionEvent> HandleCollisions<T1, T2>(
        SpatialGrid<T1> gridA, SpatialGrid<T2> gridB, int cellSize, string eventType)
}
```

**Methods:**
- `HandleCollisions<T1, T2>(...)`: Detect collisions between two entity types

## 🖥️ Screen Classes

### MainMenuScreen

Main menu interface using GeonBit.UI.

```csharp
public class MainMenuScreen : IGameScreen
{
    public MainMenuScreen(PrototypeMonoGame game)
}
```

**Properties:**
- `MenuBackgroundColor`: Static background color for menu

**Methods:**
- `GetGeonBitPanel()`: Returns the UI panel for rendering

### SettingsScreen

Settings configuration interface.

```csharp
public class SettingsScreen : IGameScreen
{
    public SettingsScreen(PrototypeMonoGame game)
}
```

**Methods:**
- `GetGeonBitPanel()`: Returns the UI panel for rendering

### GameScreen

Main gameplay screen.

```csharp
public class GameScreen : IGameScreen
{
    public GameScreen(GraphicsDevice graphicsDevice, GameSettings settings)
}
```

**Methods:**
- `LoadContent(ContentManager content)`: Load game assets
- `Update(GameTime)`: Update game logic
- `Draw(SpriteBatch)`: Render game content

## 🎨 UI Components

### GameHUD

In-game heads-up display.

```csharp
public class GameHUD
{
    public GameHUD(GraphicsDevice graphicsDevice, SpriteFont font)
}
```

**Methods:**
- `Update(GameTime)`: Update HUD state
- `Draw(SpriteBatch, GameStateManager, int)`: Render HUD elements

### VisualEffects

Particle and visual effect system.

```csharp
public class VisualEffects
{
    public VisualEffects(GraphicsDevice graphicsDevice)
}
```

**Methods:**
- `CreateExplosion(Vector2 position, Color color)`: Create explosion effect
- `CreateBulletTrail(Vector2 start, Vector2 end, Color color)`: Create bullet trail
- `Update(GameTime)`: Update all effects
- `Draw(SpriteBatch)`: Render all effects

### GameBackground

Animated background system.

```csharp
public class GameBackground
{
    public GameBackground(GraphicsDevice graphicsDevice)
}
```

**Methods:**
- `Update(GameTime)`: Update background animation
- `Draw(SpriteBatch)`: Render background

### ScorePopupManager

Manages floating score notifications.

```csharp
public class ScorePopupManager
{
    public ScorePopupManager(SpriteFont font)
}
```

**Methods:**
- `AddScorePopup(Vector2 position, int score, Color color)`: Create score popup
- `Update(GameTime)`: Update all popups
- `Draw(SpriteBatch)`: Render all popups

## 🔧 Utility Classes

### SpatialGrid<T>

Generic spatial partitioning system for collision optimization.

```csharp
public class SpatialGrid<T>
{
    public SpatialGrid(int width, int height, int cellSize)
}
```

**Properties:**
- `Cols`: Number of grid columns
- `Rows`: Number of grid rows
- `CellSize`: Size of each grid cell in pixels

**Methods:**
- `Clear()`: Remove all items from grid
- `Add(T item, Rectangle bounds)`: Add item to appropriate grid cell
- `GetCell(int x, int y)`: Get items in specific cell

### GameStateManager

Manages gameplay state and progression.

```csharp
public class GameStateManager
{
    public GamePlayState CurrentState { get; private set; } = GamePlayState.Playing;
    public int CurrentWave { get; set; } = 1;
    public float WaveTimeRemaining { get; set; } = 0f;
    public int ComboCount { get; set; } = 0;
    public float ComboTimer { get; set; } = 0f;
    public float PlayerHealth { get; set; } = 100f;
}
```

**Methods:**
- `Update(GameTime, KeyboardState)`: Update game state
- `AddCombo()`: Increment combo counter
- `TakeDamage(float damage)`: Reduce player health
- `HealPlayer(float healing)`: Increase player health
- `ActivatePowerUp(string name, float duration)`: Activate power-up
- `StartNewWave(int wave, float duration)`: Begin new wave
- `RestartGame()`: Reset to initial state
- `GetScoreMultiplier()`: Calculate score multiplier from combo

## ⚙️ Settings Classes

### GameSettings

Root settings container.

```csharp
public class GameSettings
{
    public AudioSettings AudioSettings { get; set; }
    public VideoSettings VideoSettings { get; set; }
    public GameplaySettings GameSettingsSection { get; set; }
    public InputSettings InputSettings { get; set; }
}
```

**Methods:**
- `static Load()`: Load settings from file or create defaults
- `Save()`: Save current settings to file

### AudioSettings

Audio configuration.

```csharp
public class AudioSettings
{
    public int MasterVolume { get; set; } = 10;
    public int MusicVolume { get; set; } = 10;
    public int SfxVolume { get; set; } = 10;
}
```

### VideoSettings

Display configuration.

```csharp
public class VideoSettings
{
    public bool Fullscreen { get; set; } = false;
    public string Resolution { get; set; } = "800x600";
}
```

### GameplaySettings

Gameplay options.

```csharp
public class GameplaySettings
{
    public bool ShowFPS { get; set; } = true;
    public bool ShowDebugInfo { get; set; } = false;
    public bool ControllerSupport { get; set; } = true;
}
```

### InputSettings

Input configuration for both players.

```csharp
public class InputSettings
{
    public PlayerInputSettings Player1 { get; set; } = new PlayerInputSettings();
    public PlayerInputSettings Player2 { get; set; } = new PlayerInputSettings();
}
```

### PlayerInputSettings

Individual player input configuration.

```csharp
public class PlayerInputSettings
{
    public bool KeyboardEnabled { get; set; } = true;
    public bool ControllerEnabled { get; set; } = false;
    public int ControllerIndex { get; set; } = 0;
    public KeyboardKeybindings KeyboardKeybindings { get; set; }
    public ControllerKeybindings ControllerKeybindings { get; set; }
}
```

**Properties:**
- `KeyBindings`: Dictionary of action-key mappings for active input type

## 🎮 Core Game Class

### PrototypeMonoGame

Main game class inheriting from MonoGame's Game class.

```csharp
public class PrototypeMonoGame : Game
{
    public GameSettings GameSettings { get; private set; }
    public FontSystem FontSystem { get; }
    public FontSystem FontSystemBold { get; }
}
```

**Methods:**
- `SwitchToGame()`: Transition to gameplay screen
- `SwitchToSettings()`: Transition to settings screen
- `SwitchToMenu()`: Transition to main menu
- `QuitGame()`: Exit application

## 📊 Enumerations

### GamePlayState

Gameplay state enumeration.

```csharp
public enum GamePlayState
{
    Playing,    // Active gameplay
    Paused,     // Game paused
    GameOver    // Game ended
}
```

## 🔗 Event Classes

### CollisionEvent

Represents a collision between two entities.

```csharp
public class CollisionEvent
{
    public IEntity EntityA { get; }
    public IEntity EntityB { get; }
    public string EventType { get; }
}
```

## 🎯 Usage Examples

### Creating a New Entity

```csharp
// Create texture
var texture = new Texture2D(graphicsDevice, 32, 32);
var data = new Color[32 * 32];
for (int i = 0; i < data.Length; i++) data[i] = Color.Blue;
texture.SetData(data);

// Create entity
var entity = new CustomEntity(texture, new Vector2(100, 100));

// Register with manager
entityManager.Register(entity);
```

### Handling Collisions

```csharp
// Detect collisions
var collisions = CollisionManager.HandleCollisions<Bullet, Enemy>(
    bulletGrid, enemyGrid, 64, "Bullet-Enemy");

// Process collision events
foreach (var collision in collisions)
{
    var bullet = collision.EntityA as Bullet;
    var enemy = collision.EntityB as Enemy;
    
    bullet.IsActive = false;
    enemy.IsActive = false;
    
    // Add explosion effect
    visualEffects.CreateExplosion(enemy.Position, Color.Orange);
}
```

### Managing Settings

```csharp
// Load settings
var settings = GameSettings.Load();

// Modify settings
settings.AudioSettings.MasterVolume = 8;
settings.VideoSettings.Fullscreen = true;

// Save settings
settings.Save();
```

---

This API reference provides comprehensive documentation for all public classes, methods, and properties in the MonoGame 2D Shooter Prototype.