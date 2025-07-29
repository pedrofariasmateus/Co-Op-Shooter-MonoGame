using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI.Entities;
using MonoGame2DShooterPrototype.Source.Core;

namespace MonoGame2DShooterPrototype.Source.Screens
{
    public class SettingsScreen : IGameScreen
    {
        private readonly Panel _mainPanel;
        private readonly PrototypeMonoGame _game;
        private GameSettings _settings;
        private string _selectedTab = "Video";
        private Panel _tabContentPanel;

        public SettingsScreen(PrototypeMonoGame game)
        {
            _game = game;
            _settings = game.GameSettings;
            _mainPanel = BuildSettingsUI();
        }

        private Panel BuildSettingsUI()
        {
            var panel = new Panel(new Vector2(700, 540), PanelSkin.Simple, Anchor.Center)
            {
                Padding = new Vector2(32, 32),
                FillColor = new Color(40, 40, 60)
            };
            // Title
            var title = new Header("Settings")
            {
                Anchor = Anchor.TopCenter,
                FillColor = new Color(220, 220, 255),
                Padding = new Vector2(0, 32),
                Scale = 1.6f
            };
            panel.AddChild(title);
            panel.AddChild(new Paragraph("") { Scale = 0.7f }); // spacing below title
            // Create a vertical container for tabs and content
            var tabsAndContentContainer = new Panel(new Vector2(600, 320), PanelSkin.None, Anchor.Center)
            {
                Padding = new Vector2(0, 0),
                Offset = new Vector2(0, 16)
            };
            var tabPanel = new Panel(new Vector2(600, 48), PanelSkin.None, Anchor.TopCenter)
            {
                Padding = new Vector2(0, 0)
            };
            var tabWidth = 140f;
            var tabSpacing = 10f;
            var videoTab = CreateTabButton("Video", tabWidth);
            var audioTab = CreateTabButton("Audio", tabWidth);
            var inputTab = CreateTabButton("Input", tabWidth);
            var gameTab = CreateTabButton("Game", tabWidth);
            videoTab.Anchor = Anchor.CenterLeft;
            audioTab.Anchor = Anchor.CenterLeft;
            inputTab.Anchor = Anchor.CenterLeft;
            gameTab.Anchor = Anchor.CenterLeft;
            videoTab.Offset = new Vector2(0, 0);
            audioTab.Offset = new Vector2(tabWidth + tabSpacing, 0);
            inputTab.Offset = new Vector2((tabWidth + tabSpacing) * 2, 0);
            gameTab.Offset = new Vector2((tabWidth + tabSpacing) * 3, 0);
            tabPanel.AddChild(videoTab);
            tabPanel.AddChild(audioTab);
            tabPanel.AddChild(inputTab);
            tabPanel.AddChild(gameTab);
            videoTab.OnClick = (Entity btn) => SelectTab("Video", videoTab, audioTab, inputTab, gameTab);
            audioTab.OnClick = (Entity btn) => SelectTab("Audio", videoTab, audioTab, inputTab, gameTab);
            inputTab.OnClick = (Entity btn) => SelectTab("Input", videoTab, audioTab, inputTab, gameTab);
            gameTab.OnClick = (Entity btn) => SelectTab("Game", videoTab, audioTab, inputTab, gameTab);
            // Tab content panel
            _tabContentPanel = new Panel(new Vector2(600, 260), PanelSkin.Simple, Anchor.Auto)
            {
                Padding = new Vector2(24, 18),
                FillColor = new Color(50, 54, 70)
            };
            // Add tabPanel and content panel to container
            tabsAndContentContainer.AddChild(tabPanel);
            tabsAndContentContainer.AddChild(_tabContentPanel);
            // Add container to main panel
            panel.AddChild(tabsAndContentContainer);

            // Action buttons + Back button (horizontal, centered at bottom)
            var buttonPanel = new Panel(new Vector2(640, 48), PanelSkin.None, Anchor.BottomCenter)
            {
                Padding = new Vector2(0, 24)
            };
            var saveButton = new Button("Save", ButtonSkin.Default) { Size = new Vector2(140, 38), Scale = 1.2f };
            var resetButton = new Button("Reset", ButtonSkin.Default) { Size = new Vector2(140, 38), Scale = 1.2f };
            var backButton = new Button("Back", ButtonSkin.Default) { Size = new Vector2(140, 38), Scale = 1.2f };
            saveButton.Anchor = Anchor.CenterLeft;
            resetButton.Anchor = Anchor.CenterLeft;
            backButton.Anchor = Anchor.CenterLeft;
            saveButton.Offset = new Vector2(0, 0);
            resetButton.Offset = new Vector2(160, 0);
            backButton.Offset = new Vector2(320, 0);
            backButton.FillColor = new Color(36, 44, 66);
            if (backButton.Children.Count > 0 && backButton.Children[0] is Label label)
            {
                label.FillColor = Color.White;
                label.Scale = 1.2f;
            }
            backButton.OnMouseEnter = (Entity btn) => backButton.FillColor = new Color(50, 70, 110);
            backButton.OnMouseLeave = (Entity btn) => backButton.FillColor = new Color(36, 44, 66);
            backButton.OnMouseDown = (Entity btn) => backButton.FillColor = new Color(70, 90, 130);
            backButton.OnClick = (Entity btn) => _game.SwitchToMenu();
            buttonPanel.AddChild(saveButton);
            buttonPanel.AddChild(resetButton);
            buttonPanel.AddChild(backButton);
            panel.AddChild(buttonPanel);

            // Select default tab
            SelectTab(_selectedTab, videoTab, audioTab, inputTab, gameTab);

            return panel;
        }

        private Button CreateTabButton(string text, float width)
        {
            var button = new Button(text, ButtonSkin.Default)
            {
                Size = new Vector2(width, 38),
                Anchor = Anchor.Auto,
                Scale = 1.2f
            };
            button.FillColor = new Color(36, 44, 66);
            if (button.Children.Count > 0 && button.Children[0] is Label label)
            {
                label.FillColor = Color.White;
                label.Scale = 1.2f;
            }
            button.OnMouseEnter = (Entity btn) => button.FillColor = new Color(50, 70, 110);
            button.OnMouseLeave = (Entity btn) => button.FillColor = new Color(36, 44, 66);
            button.OnMouseDown = (Entity btn) => button.FillColor = new Color(70, 90, 130);
            return button;
        }

        private void SelectTab(string tab, Button videoTab, Button audioTab, Button inputTab, Button gameTab)
        {
            _selectedTab = tab;
            var tabButtons = new[] { videoTab, audioTab, inputTab, gameTab };
            foreach (var btn in tabButtons)
            {
                btn.FillColor = new Color(36, 44, 66);
                if (btn.Children.Count > 0 && btn.Children[0] is Label label)
                {
                    label.FillColor = Color.White;
                }
            }
            Button selectedBtn = tab == "Video" ? videoTab : tab == "Audio" ? audioTab : tab == "Input" ? inputTab : gameTab;
            selectedBtn.FillColor = new Color(120, 160, 255);
            if (selectedBtn.Children.Count > 0 && selectedBtn.Children[0] is Label selectedLabel)
            {
                selectedLabel.FillColor = Color.White;
            }
            _tabContentPanel.ClearChildren();
            Entity tabContent = null;
            switch (tab)
            {
                case "Video": tabContent = BuildVideoSettingsPanel(); break;
                case "Audio": tabContent = BuildAudioSettingsPanel(); break;
                case "Input": tabContent = BuildInputSettingsPanel(); break;
                case "Game": tabContent = BuildGameSettingsPanel(); break;
            }
            if (tabContent != null) _tabContentPanel.AddChild(tabContent);
        }

        private Entity BuildVideoSettingsPanel()
        {
            var panel = new Panel(new Vector2(0, 0), PanelSkin.None, Anchor.Auto) { Padding = Vector2.Zero };
            panel.AddChild(new Header("Video Settings") { FillColor = Color.White, Scale = 1.2f, Padding = Vector2.Zero, Offset = new Vector2(0, -8) });

            // Grid container for settings (vertical stack)
            var grid = new Panel(new Vector2(540, 90), PanelSkin.None, Anchor.Auto)
            {
                Padding = new Vector2(0, 0)
            };

            // --- Fullscreen row ---
            var fullscreenRow = new Panel(new Vector2(540, 36), PanelSkin.None, Anchor.Auto)
            {
                Padding = new Vector2(0, 0)
            };
            var fullscreenLabel = new Paragraph("Fullscreen", Anchor.CenterLeft)
            {
                FillColor = Color.White,
                Scale = 1.1f,
                Size = new Vector2(200, 36)
            };
            var fullscreenCheckbox = new CheckBox("")
            {
                Checked = _settings.VideoSettings.Fullscreen,
                Anchor = Anchor.CenterRight,
                Scale = 1.25f, // slightly larger for better balance
                Size = new Vector2(40, 40),
                Offset = new Vector2(0, 4) // lower for better vertical alignment
            };
            fullscreenCheckbox.OnValueChange = (e) => _settings.VideoSettings.Fullscreen = fullscreenCheckbox.Checked;
            fullscreenRow.AddChild(fullscreenLabel);
            fullscreenRow.AddChild(fullscreenCheckbox);

            // --- Divider ---
            var divider = new Panel(new Vector2(500, 1), PanelSkin.None, Anchor.Auto)
            {
                FillColor = new Color(120, 120, 160), // lighter color for modern look
                Offset = new Vector2(0, 2) // less vertical space
            };

            // --- Resolution row ---
            var resRow = new Panel(new Vector2(540, 36), PanelSkin.None, Anchor.Auto)
            {
                Padding = new Vector2(0, 0)
            };
            var resLabel = new Paragraph("Resolution", Anchor.CenterLeft)
            {
                FillColor = Color.White,
                Scale = 1.1f,
                Size = new Vector2(200, 36)
            };
            var resolutionDropdown = new DropDown(new Vector2(200, 36), Anchor.CenterRight)
            {
                Scale = 1.1f,
                Offset = new Vector2(0, 2) // match vertical alignment with checkbox
            };
            var resolutions = new[] { "1920x1080", "1600x900", "1280x720" };
            foreach (var res in resolutions) resolutionDropdown.AddItem(res);
            int resIndex = System.Array.IndexOf(resolutions, _settings.VideoSettings.Resolution);
            resolutionDropdown.SelectedIndex = resIndex >= 0 ? resIndex : 0;
            resolutionDropdown.OnValueChange = (e) => _settings.VideoSettings.Resolution = resolutionDropdown.SelectedValue;
            resRow.AddChild(resLabel);
            resRow.AddChild(resolutionDropdown);

            // Add rows to grid
            grid.AddChild(fullscreenRow);
            grid.AddChild(divider);
            grid.AddChild(resRow);

            // Add grid to main panel
            panel.AddChild(grid);

            return panel;
        }

        private Entity BuildAudioSettingsPanel()
        {
            var panel = new Panel(new Vector2(0, 0), PanelSkin.None, Anchor.Auto) { Padding = Vector2.Zero };
            panel.AddChild(new Header("Audio Settings") { FillColor = Color.White, Scale = 1.2f, Padding = Vector2.Zero, Offset = new Vector2(0, -8) });
            // Music Volume row
            var musicRow = new Panel(new Vector2(540, 36), PanelSkin.None, Anchor.Auto)
            {
                Padding = new Vector2(16, 0)
            };
            var musicLabel = new Paragraph("Music Volume", Anchor.CenterLeft) { FillColor = Color.White, Scale = 1.1f };
            var musicSlider = new Slider(0, 100, new Vector2(140, 24), SliderSkin.Default, Anchor.Center) { Scale = 1.1f };
            musicSlider.Value = _settings.AudioSettings.MusicVolume;
            var musicValueLabel = new Paragraph($"{musicSlider.Value}%", Anchor.CenterRight) { FillColor = Color.White, Scale = 1.1f, Offset = new Vector2(24, 0) };
            musicSlider.OnValueChange = (e) => {
                _settings.AudioSettings.MusicVolume = musicSlider.Value;
                musicValueLabel.Text = $"{musicSlider.Value}%";
            };
            musicRow.AddChild(musicLabel);
            musicRow.AddChild(musicSlider);
            musicRow.AddChild(musicValueLabel);
            panel.AddChild(musicRow);
            // SFX Volume row
            var sfxRow = new Panel(new Vector2(540, 36), PanelSkin.None, Anchor.Auto)
            {
                Padding = new Vector2(16, 0)
            };
            var sfxLabel = new Paragraph("SFX Volume", Anchor.CenterLeft) { FillColor = Color.White, Scale = 1.1f };
            var sfxSlider = new Slider(0, 100, new Vector2(140, 24), SliderSkin.Default, Anchor.Center) { Scale = 1.1f };
            sfxSlider.Value = _settings.AudioSettings.SfxVolume;
            var sfxValueLabel = new Paragraph($"{sfxSlider.Value}%", Anchor.CenterRight) { FillColor = Color.White, Scale = 1.1f, Offset = new Vector2(24, 0) };
            sfxSlider.OnValueChange = (e) => {
                _settings.AudioSettings.SfxVolume = sfxSlider.Value;
                sfxValueLabel.Text = $"{sfxSlider.Value}%";
            };
            sfxRow.AddChild(sfxLabel);
            sfxRow.AddChild(sfxSlider);
            sfxRow.AddChild(sfxValueLabel);
            panel.AddChild(sfxRow);
            return panel;
        }

        private Entity BuildInputSettingsPanel()
        {
            var panel = new Panel(new Vector2(0, 0), PanelSkin.None, Anchor.Auto) { Padding = Vector2.Zero };
            panel.AddChild(new Header("Input Settings") { FillColor = Color.White, Scale = 1.2f, Padding = Vector2.Zero, Offset = new Vector2(0, -8) });
            panel.AddChild(new Paragraph("Player 1 Input Type:", Anchor.CenterLeft) { FillColor = Color.White, Scale = 1.1f });
            var p1InputDropdown = new DropDown(new Vector2(180, 36), Anchor.CenterLeft) { Scale = 1.1f };
            p1InputDropdown.AddItem("Keyboard");
            p1InputDropdown.AddItem("Controller");
            p1InputDropdown.SelectedIndex = _settings.InputSettings.Player1.ControllerEnabled ? 1 : 0;
            p1InputDropdown.OnValueChange = (e) => {
                _settings.InputSettings.Player1.ControllerEnabled = p1InputDropdown.SelectedIndex == 1;
                _settings.InputSettings.Player1.KeyboardEnabled = p1InputDropdown.SelectedIndex == 0;
            };
            panel.AddChild(p1InputDropdown);
            panel.AddChild(new Paragraph("Player 2 Input Type:", Anchor.CenterLeft) { FillColor = Color.White, Scale = 1.1f });
            var p2InputDropdown = new DropDown(new Vector2(180, 36), Anchor.CenterLeft) { Scale = 1.1f };
            p2InputDropdown.AddItem("Keyboard");
            p2InputDropdown.AddItem("Controller");
            p2InputDropdown.SelectedIndex = _settings.InputSettings.Player2.ControllerEnabled ? 1 : 0;
            p2InputDropdown.OnValueChange = (e) => {
                _settings.InputSettings.Player2.ControllerEnabled = p2InputDropdown.SelectedIndex == 1;
                _settings.InputSettings.Player2.KeyboardEnabled = p2InputDropdown.SelectedIndex == 0;
            };
            panel.AddChild(p2InputDropdown);

            // --- Player 1 Keybindings ---
            var p1Keybindings = _settings.InputSettings.Player1.KeyBindings; // Dictionary<string, string>
            panel.AddChild(new Paragraph("Player 1 Keybindings:", Anchor.CenterLeft) { FillColor = Color.White, Scale = 1.0f, Offset = new Vector2(0, 8) });
            var p1Grid = new Panel(new Vector2(400, 0), PanelSkin.None, Anchor.Auto) { Padding = new Vector2(0, 0) };
            foreach (var kvp in p1Keybindings)
            {
                var row = new Panel(new Vector2(400, 28), PanelSkin.None, Anchor.Auto) { Padding = new Vector2(0, 0) };
                row.AddChild(new Paragraph(kvp.Key, Anchor.CenterLeft) { Size = new Vector2(180, 28), FillColor = Color.White, Scale = 1.0f });
                row.AddChild(new Paragraph(kvp.Value, Anchor.Center) { Size = new Vector2(100, 28), FillColor = Color.LightGray, Scale = 1.0f });
                var editBtn = new Button("Edit", ButtonSkin.Default) { Size = new Vector2(60, 24), Scale = 0.9f, Anchor = Anchor.CenterRight };
                // editBtn.OnClick = ... (add your rebinding logic here)
                row.AddChild(editBtn);
                p1Grid.AddChild(row);
            }
            panel.AddChild(p1Grid);

            // --- Player 2 Keybindings ---
            var p2Keybindings = _settings.InputSettings.Player2.KeyBindings; // Dictionary<string, string>
            panel.AddChild(new Paragraph("Player 2 Keybindings:", Anchor.CenterLeft) { FillColor = Color.White, Scale = 1.0f, Offset = new Vector2(0, 8) });
            var p2Grid = new Panel(new Vector2(400, 0), PanelSkin.None, Anchor.Auto) { Padding = new Vector2(0, 0) };
            foreach (var kvp in p2Keybindings)
            {
                var row = new Panel(new Vector2(400, 28), PanelSkin.None, Anchor.Auto) { Padding = new Vector2(0, 0) };
                row.AddChild(new Paragraph(kvp.Key, Anchor.CenterLeft) { Size = new Vector2(180, 28), FillColor = Color.White, Scale = 1.0f });
                row.AddChild(new Paragraph(kvp.Value, Anchor.Center) { Size = new Vector2(100, 28), FillColor = Color.LightGray, Scale = 1.0f });
                var editBtn = new Button("Edit", ButtonSkin.Default) { Size = new Vector2(60, 24), Scale = 0.9f, Anchor = Anchor.CenterRight };
                // editBtn.OnClick = ... (add your rebinding logic here)
                row.AddChild(editBtn);
                p2Grid.AddChild(row);
            }
            panel.AddChild(p2Grid);

            return panel;
        }

        private Entity BuildGameSettingsPanel()
        {
            var panel = new Panel(new Vector2(0, 0), PanelSkin.None, Anchor.Auto) { Padding = Vector2.Zero };
            panel.AddChild(new Header("Game Settings") { FillColor = Color.White, Scale = 1.2f, Padding = Vector2.Zero, Offset = new Vector2(0, -8) });
            var showFpsCheckbox = new CheckBox("Show FPS") { Checked = _settings.GameSettingsSection.ShowFPS, Anchor = Anchor.CenterLeft, Scale = 1.1f };
            showFpsCheckbox.OnValueChange = (e) => _settings.GameSettingsSection.ShowFPS = showFpsCheckbox.Checked;
            panel.AddChild(showFpsCheckbox);
            var debugCheckbox = new CheckBox("Show Debug Info") { Checked = _settings.GameSettingsSection.ShowDebugInfo, Anchor = Anchor.CenterLeft, Scale = 1.1f };
            debugCheckbox.OnValueChange = (e) => _settings.GameSettingsSection.ShowDebugInfo = debugCheckbox.Checked;
            panel.AddChild(debugCheckbox);
            var controllerCheckbox = new CheckBox("Controller Support") { Checked = _settings.GameSettingsSection.ControllerSupport, Anchor = Anchor.CenterLeft, Scale = 1.1f };
            controllerCheckbox.OnValueChange = (e) => _settings.GameSettingsSection.ControllerSupport = controllerCheckbox.Checked;
            panel.AddChild(controllerCheckbox);
            return panel;
        }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { /* GeonBit.UI is drawn by main loop */ }
        public Panel GetGeonBitPanel() => _mainPanel;
    }
}
