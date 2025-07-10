using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using MonoGame2DShooterPrototype.Source.Core;

namespace MonoGame2DShooterPrototype.Panels
{
    public static class VideoSettingsPanel
    {
        public static Widget Build(GameSettings settings, Color textColor)
        {
            var panel = new VerticalStackPanel { Spacing = 12 };
            panel.Widgets.Add(new Label { Text = $"Fullscreen: {settings.VideoSettings.Fullscreen}", TextColor = textColor });
            panel.Widgets.Add(new Label { Text = $"Resolution: {settings.VideoSettings.Resolution}", TextColor = textColor });
            // Add controls for changing these values as needed
            return panel;
        }
    }
}
