using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI;
using GeonBit.UI.Entities;
using MonoGame2DShooterPrototype.Source.Core;

namespace MonoGame2DShooterPrototype.Source.Screens
{
    public class MainMenuScreen : IGameScreen
    {
        private readonly Panel _mainPanel;
        private readonly PrototypeMonoGame _game;

        public static readonly Color MenuBackgroundColor = new Color(30, 32, 48);
        private static readonly Color ButtonColor = new Color(36, 44, 66);
        private static readonly Color ButtonHoverColor = new Color(50, 70, 110);
        private static readonly Color ButtonPressedColor = new Color(70, 90, 130);
        private static readonly Color ButtonTextColor = Color.White;
        private static readonly Color TitleColor = new Color(220, 220, 255);

        public MainMenuScreen(PrototypeMonoGame game)
        {
            _game = game;
            _mainPanel = BuildMenu();
        }

        private Panel BuildMenu()
        {
            // Main panel (card)
            var panel = new Panel(new Vector2(600, 400), PanelSkin.Simple, Anchor.Center)
            {
                Padding = new Vector2(32, 32)
            };

            // Title at the top, centered
            var title = new Header("2D Shooter Prototype")
            {
                Anchor = Anchor.TopCenter,
                FillColor = TitleColor,
                Padding = new Vector2(0, 32)
            };
            panel.AddChild(title);

            // Button panel (centered)
            var buttonPanel = new Panel(new Vector2(200, 180), PanelSkin.None, Anchor.Center)
            {
                Anchor = Anchor.Center,
                Offset = Vector2.Zero,
                Padding = new Vector2(0, 0)
            };

            // Add vertically stacked buttons
            var playButton = CreateStyledButton("Play");
            var settingsButton = CreateStyledButton("Settings");
            var quitButton = CreateStyledButton("Quit");
            playButton.Anchor = Anchor.Auto;
            settingsButton.Anchor = Anchor.Auto;
            quitButton.Anchor = Anchor.Auto;
            playButton.OnClick = (Entity btn) => _game.SwitchToGame();
            settingsButton.OnClick = (Entity btn) => _game.SwitchToSettings();
            quitButton.OnClick = (Entity btn) => _game.QuitGame();
            buttonPanel.AddChild(playButton);
            buttonPanel.AddChild(settingsButton);
            buttonPanel.AddChild(quitButton);
            panel.AddChild(buttonPanel);

            return panel;
        }

        private Button CreateStyledButton(string text)
        {
            var button = new Button(text, ButtonSkin.Default)
            {
                Size = new Vector2(200, 40),
                Anchor = Anchor.Auto
            };
            button.FillColor = ButtonColor;
            // Set text color via label child
            if (button.Children.Count > 0 && button.Children[0] is Label label)
            {
                label.FillColor = ButtonTextColor;
            }
            button.OnMouseEnter = (Entity btn) => button.FillColor = ButtonHoverColor;
            button.OnMouseLeave = (Entity btn) => button.FillColor = ButtonColor;
            button.OnMouseDown = (Entity btn) => button.FillColor = ButtonPressedColor;
            // No OnMouseUp event in GeonBit.UI
            return button;
        }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the UI (background color should be set in Game.Draw)
            UserInterface.Active.Draw(spriteBatch);
        }
        // Removed GetMyraDesktop (obsolete)
        public Panel GetGeonBitPanel() => _mainPanel;
    }
}
