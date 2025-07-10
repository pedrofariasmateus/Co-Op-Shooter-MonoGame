using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using MonoGame2DShooterPrototype.Source.Core;

namespace MonoGame2DShooterPrototype.Panels
{
    public static class AudioSettingsPanel
    {
        public static Widget Build(GameSettings settings, Color textColor)
        {
            var panel = new VerticalStackPanel { Spacing = 12 };
            panel.Widgets.Add(new Label { Text = $"Master Volume: {settings.AudioSettings.MasterVolume}", TextColor = textColor });
            panel.Widgets.Add(new Label { Text = $"Music Volume: {settings.AudioSettings.MusicVolume}", TextColor = textColor });
            panel.Widgets.Add(new Label { Text = $"SFX Volume: {settings.AudioSettings.SfxVolume}", TextColor = textColor });
            // Add controls for changing these values as needed
            return panel;
        }
    }
}
