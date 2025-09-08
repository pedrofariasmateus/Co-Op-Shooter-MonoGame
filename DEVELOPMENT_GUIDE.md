# Development Guide - MonoGame 2D Shooter Prototype

## 🚀 Getting Started

### Prerequisites

Before you begin development, ensure you have the following installed:

- **Visual Studio 2022** or **Visual Studio Code**
- **.NET 8.0 SDK** or later
- **MonoGame Templates** (optional but recommended)
- **Git** for version control

### Initial Setup

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd MonoGame2DShooterPrototype
   ```

2. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

3. **Build the Project**
   ```bash
   dotnet build
   ```

4. **Run the Game**
   ```bash
   dotnet run
   ```

### IDE Configuration

#### Visual Studio Code
The project includes pre-configured VS Code settings:

- **`.vscode/launch.json`**: Debug configuration
- **`.vscode/tasks.json`**: Build tasks
- **`.vscode/settings.json`**: Editor settings

#### Visual Studio 2022
Open the solution file: `MonoGame2DShooterPrototype.sln`

## 🏗️ Project Architecture

### Folder Structure Guidelines

```
Source/
├── Core/           # Core game systems and managers
├── Entities/       # Game objects (Player, Enemy, Bullet)
├── Managers/       # System managers (Entity, Bullet, Enemy, etc.)
├── Screens/        # Game screens (Menu, Settings, Game)
├── UI/            # User interface components
└── Utilities/     # Helper classes and utilities
```

### Naming Conventions

- **Classes**: PascalCase (`PlayerManager`, `GameScreen`)
- **Methods**: PascalCase (`Update`, `LoadContent`)
- **Properties**: PascalCase (`IsActive`, `Position`)
- **Fields**: camelCase with underscore prefix (`_spriteBatch`, `_gameState`)
- **Constants**: PascalCase (`GridCellSize`, `MaxEntities`)

### Code Organization

#### Manager Classes
All manager classes should follow this pattern:

```csharp
public class ExampleManager
{
    // Private fields
    private readonly SomeType _dependency;
    private readonly List<Entity> _entities;
    
    // Constructor with dependencies
    public ExampleManager(SomeType dependency)
    {
        _dependency = dependency;
        _entities = new List<Entity>();
    }
    
    // Public interface methods
    public void Update(GameTime gameTime) { }
    public void Draw(SpriteBatch spriteBatch) { }
    
    // Private helper methods
    private void ProcessEntities() { }
}
```

#### Entity Classes
All entities must implement `IEntity`:

```csharp
public class NewEntity : IEntity
{
    public bool IsActive { get; set; } = true;
    
    public void Update(GameTime gameTime, int viewportWidth, int viewportHeight)
    {
        // Update logic here
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Rendering logic here
    }
    
    // Entity-specific methods
    public Rectangle GetBounds()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
    }
}
```

## 🔧 Development Workflow

### Adding New Features

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/new-feature-name
   ```

2. **Implement Feature**
   - Follow existing code patterns
   - Add appropriate error handling
   - Include debug/test functionality where applicable

3. **Test Thoroughly**
   - Test in debug mode
   - Verify performance impact
   - Test edge cases

4. **Commit Changes**
   ```bash
   git add .
   git commit -m "Add: New feature description"
   ```

### Code Review Checklist

Before submitting code:

- [ ] Code follows naming conventions
- [ ] No memory leaks or excessive allocations
- [ ] Performance impact is acceptable
- [ ] Error handling is implemented
- [ ] Debug features are properly gated with `#if DEBUG`
- [ ] Comments explain complex logic
- [ ] No hardcoded values (use constants or settings)

## 🎮 Adding New Entity Types

### Step 1: Create Entity Class

```csharp
// Source/Entities/NewEntity.cs
public class NewEntity : IEntity
{
    public Vector2 Position { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Entity-specific properties
    public float Speed { get; set; } = 100f;
    public Texture2D Texture { get; set; }
    
    public NewEntity(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
    }
    
    public void Update(GameTime gameTime, int viewportWidth, int viewportHeight)
    {
        // Update logic
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
            spriteBatch.Draw(Texture, Position, Color.White);
    }
    
    public Rectangle GetBounds()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, 
                           Texture.Width, Texture.Height);
    }
}
```

### Step 2: Create Manager (if needed)

```csharp
// Source/Managers/NewEntityManager.cs
public class NewEntityManager
{
    private readonly Texture2D _texture;
    private readonly Queue<NewEntity> _entityPool;
    
    public NewEntityManager(Texture2D texture)
    {
        _texture = texture;
        _entityPool = new Queue<NewEntity>();
        
        // Pre-populate pool
        for (int i = 0; i < 50; i++)
        {
            _entityPool.Enqueue(new NewEntity(texture, Vector2.Zero));
        }
    }
    
    public NewEntity SpawnEntity(Vector2 position)
    {
        NewEntity entity;
        if (_entityPool.Count > 0)
        {
            entity = _entityPool.Dequeue();
            entity.Position = position;
            entity.IsActive = true;
        }
        else
        {
            entity = new NewEntity(_texture, position);
        }
        return entity;
    }
    
    public void ReturnToPool(NewEntity entity)
    {
        entity.IsActive = false;
        _entityPool.Enqueue(entity);
    }
}
```

### Step 3: Integrate with Game

```csharp
// In GameScreen.cs LoadContent method
newEntityTexture = CreateTexture(Color.Blue, 16, 16);
newEntityManager = new NewEntityManager(newEntityTexture);

// In GameScreen.cs Update method
var newEntity = newEntityManager.SpawnEntity(spawnPosition);
entityManager.Register(newEntity);
```

## 🎨 Adding UI Components

### Custom UI Components

```csharp
// Source/UI/NewUIComponent.cs
public class NewUIComponent
{
    private readonly SpriteFont _font;
    private readonly Texture2D _backgroundTexture;
    
    public NewUIComponent(SpriteFont font, GraphicsDevice graphicsDevice)
    {
        _font = font;
        _backgroundTexture = CreateBackgroundTexture(graphicsDevice);
    }
    
    public void Update(GameTime gameTime)
    {
        // Update UI logic
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        // Draw UI elements
        spriteBatch.Draw(_backgroundTexture, position, Color.White);
        spriteBatch.DrawString(_font, "UI Text", position + Vector2.One * 10, Color.Black);
    }
    
    private Texture2D CreateBackgroundTexture(GraphicsDevice graphicsDevice)
    {
        var texture = new Texture2D(graphicsDevice, 200, 50);
        var data = new Color[200 * 50];
        for (int i = 0; i < data.Length; i++)
            data[i] = Color.Gray;
        texture.SetData(data);
        return texture;
    }
}
```

### GeonBit.UI Components

```csharp
// For menu screens
private Panel CreateNewPanel()
{
    var panel = new Panel(new Vector2(400, 300), PanelSkin.Simple, Anchor.Center);
    
    var button = new Button("New Button", ButtonSkin.Default)
    {
        Size = new Vector2(150, 40),
        Anchor = Anchor.Auto
    };
    
    button.OnClick = (Entity btn) => HandleButtonClick();
    panel.AddChild(button);
    
    return panel;
}
```

## ⚡ Performance Guidelines

### Memory Management

1. **Use Object Pooling** for frequently created/destroyed objects
2. **Avoid Allocations** in Update loops
3. **Reuse Collections** instead of creating new ones
4. **Cache Expensive Calculations**

```csharp
// Good: Reuse Vector2
private Vector2 _cachedDirection = Vector2.Zero;

public void Update(GameTime gameTime)
{
    _cachedDirection.X = input.X;
    _cachedDirection.Y = input.Y;
    if (_cachedDirection != Vector2.Zero)
        _cachedDirection.Normalize();
}

// Bad: Create new Vector2 each frame
public void Update(GameTime gameTime)
{
    var direction = new Vector2(input.X, input.Y); // Allocation!
    if (direction != Vector2.Zero)
        direction.Normalize();
}
```

### Collision Detection

When adding new collision types:

1. **Use Spatial Grid** for broad-phase detection
2. **Implement GetBounds()** method for all collidable entities
3. **Add to CollisionManager** handling

```csharp
// Add new collision type
var newCollisions = CollisionManager.HandleCollisions<NewEntity, Player>(
    newEntityGrid, playerGrid, GridCellSize, "NewEntity-Player");

foreach (var collision in newCollisions)
{
    // Handle collision response
    var newEntity = collision.EntityA as NewEntity;
    var player = collision.EntityB as Player;
    
    // Process collision
}
```

## 🔧 Settings and Configuration

### Adding New Settings

1. **Update GameSettings.cs**

```csharp
public class GameplaySettings
{
    public bool ShowFPS { get; set; } = true;
    public bool ShowDebugInfo { get; set; } = false;
    public bool NewSetting { get; set; } = false; // Add new setting
}
```

2. **Update Settings Screen**

```csharp
// In SettingsScreen.cs
var newSettingCheckbox = new CheckBox("New Setting")
{
    Checked = _game.GameSettings.GameSettingsSection.NewSetting
};
newSettingCheckbox.OnValueChange = (Entity entity) =>
{
    _game.GameSettings.GameSettingsSection.NewSetting = newSettingCheckbox.Checked;
};
```

3. **Use Setting in Game**

```csharp
// In game logic
if (settings.GameSettingsSection.NewSetting)
{
    // Apply setting behavior
}
```

## 🐛 Debugging and Testing

### Debug Features

Add debug functionality with proper gating:

```csharp
#if DEBUG
private bool _debugMode = false;
private bool _lastDebugKey = false;

// In Update method
if (keyboardState.IsKeyDown(Keys.F1))
{
    if (!_lastDebugKey)
    {
        _debugMode = !_debugMode;
        _lastDebugKey = true;
    }
}
else
{
    _lastDebugKey = false;
}

// Debug rendering
if (_debugMode)
{
    DrawDebugInfo(spriteBatch);
}
#endif
```

### Performance Testing

```csharp
#if DEBUG
private void MeasurePerformance(Action operation, string operationName)
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    operation();
    stopwatch.Stop();
    
    if (stopwatch.ElapsedMilliseconds > 1) // Only log slow operations
    {
        Console.WriteLine($"{operationName}: {stopwatch.ElapsedMilliseconds}ms");
    }
}
#endif
```

### Test Scenarios

Create test scenarios for new features:

```csharp
#if DEBUG
private void SpawnTestScenario()
{
    // Spawn specific entity configurations for testing
    for (int i = 0; i < 10; i++)
    {
        var testEntity = entityManager.SpawnEntity(new Vector2(i * 50, 100));
        // Configure test entity
    }
}
#endif
```

## 📝 Documentation Standards

### Code Comments

```csharp
/// <summary>
/// Manages the lifecycle and behavior of bullet entities.
/// Implements object pooling for performance optimization.
/// </summary>
public class BulletManager
{
    /// <summary>
    /// Spawns a new bullet at the specified position with the given direction.
    /// Uses object pooling to minimize garbage collection.
    /// </summary>
    /// <param name="position">World position to spawn the bullet</param>
    /// <param name="direction">Normalized direction vector for bullet movement</param>
    /// <returns>Active bullet instance ready for use</returns>
    public Bullet SpawnBullet(Vector2 position, Vector2 direction)
    {
        // Implementation
    }
}
```

### Method Documentation

- **Purpose**: What the method does
- **Parameters**: What each parameter represents
- **Returns**: What the method returns
- **Side Effects**: Any state changes or external effects

## 🔄 Version Control

### Commit Message Format

```
Type: Brief description

Detailed explanation if needed

Examples:
- Add: New enemy type with special behavior
- Fix: Collision detection edge case with bullets
- Update: Performance optimization for entity manager
- Remove: Deprecated UI component
```

### Branch Naming

- `feature/feature-name` - New features
- `fix/bug-description` - Bug fixes
- `refactor/component-name` - Code refactoring
- `docs/documentation-update` - Documentation changes

## 🚀 Release Process

### Pre-Release Checklist

- [ ] All tests pass
- [ ] Performance benchmarks meet targets
- [ ] No debug code in release build
- [ ] Settings save/load correctly
- [ ] All screens function properly
- [ ] Memory usage is acceptable

### Build Configuration

```bash
# Debug build
dotnet build --configuration Debug

# Release build
dotnet build --configuration Release

# Publish for distribution
dotnet publish --configuration Release --self-contained true
```

## 🤝 Contributing Guidelines

### Code Quality

1. **Follow Existing Patterns**: Maintain consistency with existing code
2. **Performance First**: Consider performance impact of all changes
3. **Error Handling**: Implement appropriate error handling
4. **Testing**: Test thoroughly before submitting

### Pull Request Process

1. Create feature branch
2. Implement changes
3. Test thoroughly
4. Update documentation if needed
5. Submit pull request with clear description

---

This development guide provides the foundation for contributing to the MonoGame 2D Shooter Prototype. Follow these guidelines to maintain code quality and project consistency.