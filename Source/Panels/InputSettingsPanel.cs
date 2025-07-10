using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using MonoGame2DShooterPrototype.Source.Core;
using System.Collections.Generic;

namespace MonoGame2DShooterPrototype.Panels
{
    public static class InputSettingsPanel
    {
        public static Widget Build(GameSettings settings, Color titleColor, Color buttonTextColor)
        {
            var mainPanel = new VerticalStackPanel { Spacing = 24, HorizontalAlignment = HorizontalAlignment.Center, Padding = new Myra.Graphics2D.Thickness(8, 8, 8, 8) };

            // Player 1
            var p1Label = new Label { Text = "Player 1", TextColor = titleColor, Padding = new Myra.Graphics2D.Thickness(0, 8, 0, 4) };
            mainPanel.Widgets.Add(p1Label);
            var p1Row = new HorizontalStackPanel { Spacing = 24 };
            // Player 1 Keyboard
            var p1KeyboardDict = new Dictionary<string, string>();
            var p1KeyboardProps = settings.InputSettings.Player1.KeyboardKeybindings.GetType().GetProperties();
            foreach (var prop in p1KeyboardProps)
            {
                p1KeyboardDict[prop.Name] = prop.GetValue(settings.InputSettings.Player1.KeyboardKeybindings)?.ToString() ?? "";
            }
            p1Row.Widgets.Add(KeybindingsTableWidget.Build("Keyboard", p1KeyboardDict, titleColor, buttonTextColor));
            // Player 1 Controller
            var p1ControllerDict = new Dictionary<string, string>();
            var p1ControllerProps = settings.InputSettings.Player1.ControllerKeybindings.GetType().GetProperties();
            foreach (var prop in p1ControllerProps)
            {
                p1ControllerDict[prop.Name] = prop.GetValue(settings.InputSettings.Player1.ControllerKeybindings)?.ToString() ?? "";
            }
            p1Row.Widgets.Add(KeybindingsTableWidget.Build("Controller", p1ControllerDict, titleColor, buttonTextColor));
            mainPanel.Widgets.Add(p1Row);

            // Player 2
            var p2Label = new Label { Text = "Player 2", TextColor = titleColor, Padding = new Myra.Graphics2D.Thickness(0, 16, 0, 4) };
            mainPanel.Widgets.Add(p2Label);
            var p2Row = new HorizontalStackPanel { Spacing = 24 };
            // Player 2 Keyboard
            var p2KeyboardDict = new Dictionary<string, string>();
            var p2KeyboardProps = settings.InputSettings.Player2.KeyboardKeybindings.GetType().GetProperties();
            foreach (var prop in p2KeyboardProps)
            {
                p2KeyboardDict[prop.Name] = prop.GetValue(settings.InputSettings.Player2.KeyboardKeybindings)?.ToString() ?? "";
            }
            p2Row.Widgets.Add(KeybindingsTableWidget.Build("Keyboard", p2KeyboardDict, titleColor, buttonTextColor));
            // Player 2 Controller
            var p2ControllerDict = new Dictionary<string, string>();
            var p2ControllerProps = settings.InputSettings.Player2.ControllerKeybindings.GetType().GetProperties();
            foreach (var prop in p2ControllerProps)
            {
                p2ControllerDict[prop.Name] = prop.GetValue(settings.InputSettings.Player2.ControllerKeybindings)?.ToString() ?? "";
            }
            p2Row.Widgets.Add(KeybindingsTableWidget.Build("Controller", p2ControllerDict, titleColor, buttonTextColor));
            mainPanel.Widgets.Add(p2Row);

            var scrollViewer = new ScrollViewer { Content = mainPanel, Width = 600, HorizontalAlignment = HorizontalAlignment.Center };
            return scrollViewer;
        }
    }
}
