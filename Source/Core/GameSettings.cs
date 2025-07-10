using System;
using System.IO;
using System.Text.Json;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public class GameSettings
    {
        public AudioSettings AudioSettings { get; set; }
        public VideoSettings VideoSettings { get; set; }
        public GameplaySettings GameSettingsSection { get; set; }
        public InputSettings InputSettings { get; set; }

        private static readonly string SettingsFile = "settings.json";

        public static GameSettings Load()
        {
            GameSettings settings;
            if (!File.Exists(SettingsFile))
            {
                // Return default settings if file doesn't exist
                settings = new GameSettings
                {
                    AudioSettings = new AudioSettings(),
                    VideoSettings = new VideoSettings(),
                    GameSettingsSection = new GameplaySettings(),
                    InputSettings = new InputSettings()
                };
            }
            else
            {
                var json = File.ReadAllText(SettingsFile);
                settings = JsonSerializer.Deserialize<GameSettings>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (settings == null) settings = new GameSettings();
            }
            // Ensure all nested properties are non-null
            settings.AudioSettings ??= new AudioSettings();
            settings.VideoSettings ??= new VideoSettings();
            settings.GameSettingsSection ??= new GameplaySettings();
            settings.InputSettings ??= new InputSettings();
            return settings;
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
    }

    public class AudioSettings
    {
        public int MasterVolume { get; set; } = 10;
        public int MusicVolume { get; set; } = 10;
        public int SfxVolume { get; set; } = 10;
    }

    public class VideoSettings
    {
        public bool Fullscreen { get; set; } = false;
        public string Resolution { get; set; } = "800x600";
    }

    public class GameplaySettings
    {
        public bool ShowFPS { get; set; } = true;
        public bool ShowDebugInfo { get; set; } = false;
        public bool ControllerSupport { get; set; } = true;
    }

    public class InputSettings
    {
        public PlayerInputSettings Player1 { get; set; } = new PlayerInputSettings();
        public PlayerInputSettings Player2 { get; set; } = new PlayerInputSettings();

        public InputSettings()
        {
            Player1 = new PlayerInputSettings
            {
                KeyboardKeybindings = new KeyboardKeybindings
                {
                    MoveUp = "W",
                    MoveDown = "S",
                    MoveLeft = "A",
                    MoveRight = "D",
                    Shoot = "Space"
                },
                ControllerKeybindings = new ControllerKeybindings
                {
                    MoveUp = "DPadUp",
                    MoveDown = "DPadDown",
                    MoveLeft = "DPadLeft",
                    MoveRight = "DPadRight",
                    Shoot = "ButtonA"
                }
            };

            Player2 = new PlayerInputSettings
            {
                KeyboardKeybindings = new KeyboardKeybindings
                {
                    MoveUp = "Up",
                    MoveDown = "Down",
                    MoveLeft = "Left",
                    MoveRight = "Right",
                    Shoot = "Enter"
                },
                ControllerKeybindings = new ControllerKeybindings
                {
                    MoveUp = "DPadUp",
                    MoveDown = "DPadDown",
                    MoveLeft = "DPadLeft",
                    MoveRight = "DPadRight",
                    Shoot = "ButtonB"
                }
            };
        }
    }

    public class KeyboardKeybindings
    {
        public string MoveUp { get; set; }
        public string MoveDown { get; set; }
        public string MoveLeft { get; set; }
        public string MoveRight { get; set; }
        public string Shoot { get; set; }
    }

    public class ControllerKeybindings
    {
        public string MoveUp { get; set; }
        public string MoveDown { get; set; }
        public string MoveLeft { get; set; }
        public string MoveRight { get; set; }
        public string Shoot { get; set; }
    }

    public class PlayerInputSettings
    {
        public bool KeyboardEnabled { get; set; } = true;
        public bool ControllerEnabled { get; set; } = false;
        public int ControllerIndex { get; set; } = 0;
        public KeyboardKeybindings KeyboardKeybindings { get; set; } = new KeyboardKeybindings();
        public ControllerKeybindings ControllerKeybindings { get; set; } = new ControllerKeybindings();
    }
}
