using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
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

        private Desktop _myraDesktop;

        public GameSettings GameSettings { get; private set; }
        public FontSystem FontSystem => _fontSystem;
        public FontSystem FontSystemBold => _fontSystemBold;

        public PrototypeMonoGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameSettings = GameSettings.Load();
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

            MyraEnvironment.Game = this;
            _menuScreen = new MainMenuScreen(this);
            _settingsScreen = new SettingsScreen(this);
            _currentScreen = _menuScreen;
            _myraDesktop = _menuScreen.GetMyraDesktop();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_myraDesktop != _currentScreen?.GetMyraDesktop())
            {
                _myraDesktop = _currentScreen?.GetMyraDesktop();
            }
            _currentScreen?.Update(gameTime);
            if (_currentState == GameState.Quit)
            {
                Exit();
                return;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _currentScreen?.Draw(_spriteBatch);
            _spriteBatch.End();
            _myraDesktop?.Render();
            base.Draw(gameTime);
        }

        public void SwitchToGame() => _currentState = GameState.Playing;
        public void SwitchToSettings()
        {
            // Reload settings from file before showing the screen
            GameSettings = GameSettings.Load();
            _settingsScreen = new SettingsScreen(this);
            _currentScreen = _settingsScreen;
            _myraDesktop = _settingsScreen.GetMyraDesktop();
            _currentState = GameState.Settings;
        }
        public void SwitchToMenu()
        {
            _menuScreen = new MainMenuScreen(this);
            _currentScreen = _menuScreen;
            _myraDesktop = _menuScreen.GetMyraDesktop();
            _currentState = GameState.Menu;
        }
        public void QuitGame() => _currentState = GameState.Quit;
    }
}
