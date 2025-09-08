using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame2DShooterPrototype.Source.Screens;
using FontStashSharp;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public class PrototypeMonoGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _rectTexture;

        private FontSystem _fontSystem; // FontStashSharp font system
        private FontSystem _fontSystemBold; // FontStashSharp bold font system

        private enum GameState { Menu, Settings, Playing, Quit }
        private GameState _currentState = GameState.Menu;
        private IGameScreen _currentScreen;
        private MainMenuScreen _menuScreen;
        private SettingsScreen _settingsScreen;
        private GameScreen _gameScreen;

        public GameSettings GameSettings { get; private set; }
        public FontSystem FontSystem => _fontSystem;
        public FontSystem FontSystemBold => _fontSystemBold;

        public PrototypeMonoGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true; // Show mouse cursor for GeonBit.UI
            GameSettings = GameSettings.Load();
            
            // Set window title
            Window.Title = "MonoGame 2D Shooter Prototype v1.0";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            _rectTexture.SetData(new[] { Color.White });
            // FontStashSharp: Load TTF fonts from Content folder
            _fontSystem = new FontSystem();
            _fontSystem.AddFont(File.ReadAllBytes(Path.Combine(Content.RootDirectory, "Arial.ttf")));
            _fontSystemBold = new FontSystem();
            _fontSystemBold.AddFont(File.ReadAllBytes(Path.Combine(Content.RootDirectory, "Arial-bold.ttf")));

            // Initialize GeonBit.UI
            GeonBit.UI.UserInterface.Initialize(Content, GeonBit.UI.BuiltinThemes.hd);

            _menuScreen = new MainMenuScreen(this);
            _settingsScreen = new SettingsScreen(this);
            _currentScreen = _menuScreen;
            // Add GeonBit main menu panel to UI
            GeonBit.UI.UserInterface.Active.Clear();
            GeonBit.UI.UserInterface.Active.AddEntity(_menuScreen.GetGeonBitPanel());
        }

        protected override void Update(GameTime gameTime)
        {
            // Update GeonBit UI for both menu and settings screens
            if (_currentScreen is MainMenuScreen || _currentScreen is SettingsScreen)
            {
                GeonBit.UI.UserInterface.Active.Update(gameTime);
            }
            else
            {
                _currentScreen?.Update(gameTime);
            }
            if (_currentState == GameState.Quit)
            {
                Exit();
                return;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Draw GeonBit.UI for both menu and settings screens
            if (_currentScreen is MainMenuScreen || _currentScreen is SettingsScreen)
            {
                GraphicsDevice.Clear(MainMenuScreen.MenuBackgroundColor);
                GeonBit.UI.UserInterface.Active.Draw(_spriteBatch);
            }
            else
            {
                GraphicsDevice.Clear(new Color(5, 10, 20)); // Dark space background
                _spriteBatch.Begin();
                _currentScreen?.Draw(_spriteBatch);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void SwitchToGame()
        {
            // Create and set up the game screen
            _gameScreen = new Screens.GameScreen(GraphicsDevice, GameSettings);
            _gameScreen.LoadContent(Content);
            _currentScreen = _gameScreen;
            // Clear GeonBit UI (not used in gameplay)
            GeonBit.UI.UserInterface.Active.Clear();
            _currentState = GameState.Playing;
        }
        public void SwitchToSettings()
        {
            // Reload settings from file before showing the screen
            GameSettings = GameSettings.Load();
            _settingsScreen = new SettingsScreen(this);
            _currentScreen = _settingsScreen;
            // Switch GeonBit UI to settings panel
            GeonBit.UI.UserInterface.Active.Clear();
            GeonBit.UI.UserInterface.Active.AddEntity(_settingsScreen.GetGeonBitPanel());
            _currentState = GameState.Settings;
        }
        public void SwitchToMenu()
        {
            _menuScreen = new MainMenuScreen(this);
            _currentScreen = _menuScreen;
            // Switch GeonBit UI to main menu
            GeonBit.UI.UserInterface.Active.Clear();
            GeonBit.UI.UserInterface.Active.AddEntity(_menuScreen.GetGeonBitPanel());
            _currentState = GameState.Menu;
        }
        public void QuitGame() => _currentState = GameState.Quit;
    }
}
