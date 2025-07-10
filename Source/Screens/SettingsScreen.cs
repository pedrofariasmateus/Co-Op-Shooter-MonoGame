using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using MonoGame2DShooterPrototype.Source.Core;
using MonoGame2DShooterPrototype.Panels;
using FontStashSharp;

namespace MonoGame2DShooterPrototype
{
    public class SettingsScreen : IGameScreen
    {
        private readonly Desktop _desktop;
        private readonly PrototypeMonoGame _game;
        private readonly GameSettings _settings;

        // Color palette (same as MainMenuScreen)
        private static readonly Color MenuBackgroundColor = new Color(30, 32, 48);
        private static readonly Color ButtonColor = new Color(36, 44, 66);
        private static readonly Color ButtonHoverColor = new Color(50, 70, 110);
        private static readonly Color ButtonPressedColor = new Color(70, 90, 130);
        private static readonly Color ButtonTextColor = Color.White;
        private static readonly Color TitleColor = new Color(220, 220, 255);
        private static readonly SolidBrush MenuBackgroundBrush = new SolidBrush(MenuBackgroundColor);
        private static readonly SolidBrush ButtonBrush = new SolidBrush(ButtonColor);
        private static readonly SolidBrush ButtonHoverBrush = new SolidBrush(ButtonHoverColor);
        private static readonly SolidBrush ButtonPressedBrush = new SolidBrush(ButtonPressedColor);
        private static readonly Color CardBackgroundColor = new Color(48, 52, 72); // lighter for more contrast
        private static readonly Color CardBorderColor = new Color(120, 140, 200); // brighter border
        private static readonly int CardPadding = 32; // more padding
        private string _selectedTab = "Video";
        private static readonly Color TabHighlightColor = new Color(120, 160, 255); // re-add for tab highlight

        public SettingsScreen(PrototypeMonoGame game)
        {
            _game = game;
            _settings = game.GameSettings;
            _desktop = BuildSettingsUI();
        }

        private Desktop BuildSettingsUI()
        {
            var rootPanel = new VerticalStackPanel {
                Spacing = 8,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Myra.Graphics2D.Thickness(0, 2, 0, 0)
            };
            var title = new Label
            {
                Text = "Settings",
                HorizontalAlignment = HorizontalAlignment.Center,
                TextColor = TitleColor,
                Padding = new Myra.Graphics2D.Thickness(0, 2, 0, 2),
                Font = _game.FontSystemBold.GetFont(40)
            };
            rootPanel.Widgets.Add(title);

            // Tab buttons with improved highlight and spacing
            var tabPanel = new HorizontalStackPanel { Spacing = 2, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top }; // Center for tab buttons
            var tabFont = _game.FontSystemBold.GetFont(22);
            var videoTab = CreateTabButton("Video", tabFont);
            var audioTab = CreateTabButton("Audio", tabFont);
            var gameTab = CreateTabButton("Game", tabFont);
            var inputTab = CreateTabButton("Input", tabFont);
            tabPanel.Widgets.Add(videoTab);
            tabPanel.Widgets.Add(audioTab);
            tabPanel.Widgets.Add(gameTab);
            tabPanel.Widgets.Add(inputTab);
            rootPanel.Widgets.Add(tabPanel);

            // Card drop shadow (fake with offset panel)
            var windowWidth = _game.GraphicsDevice.Viewport.Width;
            var minCardWidth = windowWidth > 40 ? windowWidth - 40 : 600;
            var cardShadow = new Panel {
                Background = new SolidBrush(new Color(20, 22, 32, 180)),
                Padding = new Myra.Graphics2D.Thickness(CardPadding),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Myra.Graphics2D.Thickness(20, 0, 20, 0),
                MinHeight = 400,
                MinWidth = minCardWidth,
                Left = 0, Top = 8 // offset for shadow effect
            };

            // Card panel for content with border, padding
            var cardGrid = new Grid {
                Padding = new Myra.Graphics2D.Thickness(CardPadding),
                Background = new SolidBrush(CardBackgroundColor),
                Border = new SolidBrush(CardBorderColor),
                BorderThickness = new Myra.Graphics2D.Thickness(3),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Myra.Graphics2D.Thickness(20, 0, 20, 0),
                MinHeight = 400,
                MinWidth = minCardWidth
            };
            cardGrid.RowsProportions.Add(new Proportion(ProportionType.Fill)); // Content
            cardGrid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // Buttons

            // Placeholder for tab content
            var contentPanel = new Panel { Id = "SettingsContentPanel", Padding = new Myra.Graphics2D.Thickness(24, 8, 24, 8) };
            Grid.SetRow(contentPanel, 0);
            cardGrid.Widgets.Add(contentPanel);

            // Save/Apply/Reset buttons at the bottom right, with extra spacing and top border
            var buttonRow = new HorizontalStackPanel {
                Spacing = 16,
                HorizontalAlignment = HorizontalAlignment.Right,
                Padding = new Myra.Graphics2D.Thickness(0, 18, 0, 0) // space above buttons
            };
            var saveButton = CreateActionButton("Save");
            var applyButton = CreateActionButton("Apply");
            var resetButton = CreateActionButton("Reset");
            buttonRow.Widgets.Add(saveButton);
            buttonRow.Widgets.Add(applyButton);
            buttonRow.Widgets.Add(resetButton);
            // Add a subtle top border for separation
            var buttonRowPanel = new Panel {
                Padding = new Myra.Graphics2D.Thickness(0, 0, 0, 0),
                Background = null // transparent
            };
            buttonRowPanel.Widgets.Add(new Panel {
                Height = 1,
                Background = new SolidBrush(new Color(120, 140, 200, 60)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            });
            buttonRowPanel.Widgets.Add(buttonRow);
            Grid.SetRow(buttonRowPanel, 1);
            cardGrid.Widgets.Add(buttonRowPanel);

            // Layer shadow and card
            var cardStack = new Panel { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top };
            cardStack.Widgets.Add(cardShadow);
            cardStack.Widgets.Add(cardGrid);
            rootPanel.Widgets.Add(cardStack);

            // Set up tab switching with improved highlight and section heading
            void SelectTab(string tab)
            {
                _selectedTab = tab;
                SetTabContent(contentPanel, tab);
                // Update tab button styles and highlight
                foreach (var btn in new[] { videoTab, audioTab, gameTab, inputTab })
                {
                    btn.Background = ButtonBrush;
                    btn.Border = null;
                    btn.BorderThickness = new Myra.Graphics2D.Thickness(0);
                    ((Label)btn.Content).TextColor = ButtonTextColor;
                    ((Label)btn.Content).Font = tabFont;
                }
                Button selectedBtn = tab == "Video" ? videoTab : tab == "Audio" ? audioTab : tab == "Game" ? gameTab : inputTab;
                selectedBtn.Background = new SolidBrush(TabHighlightColor);
                selectedBtn.Border = new SolidBrush(Color.White);
                selectedBtn.BorderThickness = new Myra.Graphics2D.Thickness(0, 0, 0, 4);
                ((Label)selectedBtn.Content).TextColor = Color.White;
                ((Label)selectedBtn.Content).Font = _game.FontSystemBold.GetFont(24); // slightly larger for selected
                // No section heading update
            }
            videoTab.Click += (s, a) => SelectTab("Video");
            audioTab.Click += (s, a) => SelectTab("Audio");
            gameTab.Click += (s, a) => SelectTab("Game");
            inputTab.Click += (s, a) => SelectTab("Input");
            // Default tab
            SelectTab("Video");

            // Back button in top right with icon and rounded corners
            var backButtonPanel = new Panel {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Myra.Graphics2D.Thickness(0, 12, 20, 0)
            };
            var backButton = new Button
            {
                Width = 120,
                Height = 40,
                Background = new SolidBrush(new Color(36, 44, 66)),
                OverBackground = new SolidBrush(new Color(50, 70, 110)),
                PressedBackground = new SolidBrush(new Color(70, 90, 130)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Border = new SolidBrush(Color.Black),
                BorderThickness = new Myra.Graphics2D.Thickness(2),
                Padding = new Myra.Graphics2D.Thickness(0, 0, 0, 0)
            };
            var backLabel = new Label {
                Text = "\u2190  Back", // Unicode left arrow
                TextColor = ButtonTextColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Font = _game.FontSystemBold.GetFont(20)
            };
            backButton.Content = backLabel;
            backButton.Click += (s, a) => _game.SwitchToMenu();
            backButtonPanel.Widgets.Add(backButton);
            // Use a Grid to overlay the back button in the top right
            var overlayGrid = new Grid();
            overlayGrid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            overlayGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));
            overlayGrid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));
            overlayGrid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            overlayGrid.Widgets.Add(backButtonPanel);
            Grid.SetRow(backButtonPanel, 0);
            Grid.SetColumn(backButtonPanel, 1);
            // Main content panel (rootPanel) in row 1, column 0-1
            overlayGrid.Widgets.Add(rootPanel);
            Grid.SetRow(rootPanel, 1);
            Grid.SetColumn(rootPanel, 0);
            Grid.SetColumnSpan(rootPanel, 2);
            var desktop = new Desktop { Root = overlayGrid };
            desktop.Background = MenuBackgroundBrush;
            return desktop;
        }

        private Button CreateTabButton(string text, DynamicSpriteFont font)
        {
            var button = new Button { Width = 120, Height = 38, Padding = new Myra.Graphics2D.Thickness(0, 2, 0, 2) };
            var label = new Label { Text = text, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextColor = ButtonTextColor, Font = font };
            button.Content = label;
            button.Background = ButtonBrush;
            button.OverBackground = ButtonHoverBrush;
            button.PressedBackground = ButtonPressedBrush;
            return button;
        }

        private Button CreateActionButton(string text)
        {
            var button = new Button { Width = 100, Height = 36, Border = new SolidBrush(Color.Black), BorderThickness = new Myra.Graphics2D.Thickness(2) };
            var label = new Label { Text = text, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextColor = ButtonTextColor, Font = _game.FontSystemBold.GetFont(18) };
            button.Content = label;
            button.Background = ButtonBrush;
            button.OverBackground = ButtonHoverBrush;
            button.PressedBackground = ButtonPressedBrush;
            return button;
        }

        private void SetTabContent(Panel contentPanel, string tab)
        {
            contentPanel.Widgets.Clear();
            Widget tabContent = null;
            switch (tab)
            {
                case "Video":
                    tabContent = VideoSettingsPanel.Build(_settings, ButtonTextColor); // Ensure no heading in panel
                    break;
                case "Audio":
                    tabContent = AudioSettingsPanel.Build(_settings, ButtonTextColor);
                    break;
                case "Game":
                    tabContent = GameSettingsPanel.Build(_settings, ButtonTextColor);
                    break;
                case "Input":
                    tabContent = InputSettingsPanel.Build(_settings, TitleColor, ButtonTextColor);
                    break;
            }
            // Only add tab content, do not add duplicate heading
            if (tabContent != null)
            {
                contentPanel.Widgets.Add(tabContent);
            }
            // No section heading update
        }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }
        public Desktop GetMyraDesktop() => _desktop;
    }
}
