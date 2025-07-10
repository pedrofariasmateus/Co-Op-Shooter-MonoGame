using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Myra;
using MonoGame2DShooterPrototype.Source.Core;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;

namespace MonoGame2DShooterPrototype.Source.Screens
{
    public class MainMenuScreen : IGameScreen
    {
        private readonly Desktop _desktop;
        private readonly PrototypeMonoGame _game;

        private static readonly Color MenuBackgroundColor = new Color(30, 32, 48); // dark blue-gray
        private static readonly Color ButtonColor = new Color(36, 44, 66); // dark blue, blends with background
        private static readonly Color ButtonHoverColor = new Color(50, 70, 110); // lighter dark blue for hover
        private static readonly Color ButtonPressedColor = new Color(70, 90, 130); // muted blue for pressed, fits palette
        private static readonly Color ButtonTextColor = Color.White;
        private static readonly Color TitleColor = new Color(220, 220, 255); // light blue
        private static readonly Myra.Graphics2D.Brushes.SolidBrush MenuBackgroundBrush = new Myra.Graphics2D.Brushes.SolidBrush(MenuBackgroundColor);
        private static readonly Myra.Graphics2D.Brushes.SolidBrush ButtonBrush = new Myra.Graphics2D.Brushes.SolidBrush(ButtonColor);
        private static readonly Myra.Graphics2D.Brushes.SolidBrush ButtonHoverBrush = new Myra.Graphics2D.Brushes.SolidBrush(ButtonHoverColor);
        private static readonly Myra.Graphics2D.Brushes.SolidBrush ButtonPressedBrush = new Myra.Graphics2D.Brushes.SolidBrush(ButtonPressedColor);

        public MainMenuScreen(PrototypeMonoGame game)
        {
            _game = game;
            _desktop = BuildMenu();
        }

        private Desktop BuildMenu()
        {
            // Root panel is a Grid with two rows: title (top), buttons (center)
            var grid = new Grid();
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // Title row
            grid.RowsProportions.Add(new Proportion(ProportionType.Fill)); // Buttons row (fills remaining space)
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));

            // Title at the top, centered horizontally
            var title = new Label
            {
                Text = "2D Shooter Prototype",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                TextColor = TitleColor,
                Font = _game.FontSystemBold.GetFont(50),
                Padding = new Myra.Graphics2D.Thickness(0, 32, 0, 32)
            };
            grid.Widgets.Add(title);
            Grid.SetRow(title, 0);
            Grid.SetColumn(title, 0);

            // Centered panel for buttons
            var buttonPanel = new VerticalStackPanel
            {
                Spacing = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            var playButton = CreateStyledButton("Play");
            var settingsButton = CreateStyledButton("Settings");
            var quitButton = CreateStyledButton("Quit");
            playButton.Click += (s, a) => _game.SwitchToGame();
            settingsButton.Click += (s, a) => _game.SwitchToSettings();
            quitButton.Click += (s, a) => _game.QuitGame();
            buttonPanel.Widgets.Add(playButton);
            buttonPanel.Widgets.Add(settingsButton);
            buttonPanel.Widgets.Add(quitButton);
            grid.Widgets.Add(buttonPanel);
            Grid.SetRow(buttonPanel, 1);
            Grid.SetColumn(buttonPanel, 0);

            var desktop = new Desktop { Root = grid };
            desktop.Background = MenuBackgroundBrush;
            return desktop;
        }

        private Button CreateStyledButton(string text)
        {
            var button = new Button { Width = 200, Height = 40 };
            var label = new Label { Text = text, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextColor = ButtonTextColor };
            button.Content = label;
            button.Background = ButtonBrush;
            button.OverBackground = ButtonHoverBrush;
            button.PressedBackground = ButtonPressedBrush;
            return button;
        }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }
        public Desktop GetMyraDesktop() => _desktop;
    }
}
