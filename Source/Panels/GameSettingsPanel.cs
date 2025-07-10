using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using MonoGame2DShooterPrototype.Source.Core;

namespace MonoGame2DShooterPrototype.Panels
{
    public static class GameSettingsPanel
    {
        public static Widget Build(GameSettings settings, Color textColor)
        {
            var panel = new VerticalStackPanel { Spacing = 12 };
            panel.Widgets.Add(new Label { Text = $"Show FPS: {settings.GameSettingsSection.ShowFPS}", TextColor = textColor });
            panel.Widgets.Add(new Label { Text = $"Show Debug Info: {settings.GameSettingsSection.ShowDebugInfo}", TextColor = textColor });
            panel.Widgets.Add(new Label { Text = $"Controller Support: {settings.GameSettingsSection.ControllerSupport}", TextColor = textColor });
            // Add controls for changing these values as needed
            return panel;
        }
    }
}
