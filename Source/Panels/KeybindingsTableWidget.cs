using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame2DShooterPrototype.Panels
{
    public static class KeybindingsTableWidget
    {
        public static Widget Build(string header, Dictionary<string, string> keybindings, Color headerColor, Color textColor)
        {
            // Table border and background colors
            var borderColor = new Color(80, 90, 120);
            var headerBgColor = new Color(44, 48, 70);
            var rowBgColor = new Color(36, 40, 60);
            var altRowBgColor = new Color(30, 32, 48);

            var outerPanel = new Panel
            {
                Padding = new Myra.Graphics2D.Thickness(0),
                Background = new Myra.Graphics2D.Brushes.SolidBrush(borderColor)
            };

            var tableGrid = new Grid { ColumnSpacing = 0, RowSpacing = 0, Padding = new Myra.Graphics2D.Thickness(1) };
            tableGrid.ColumnsProportions.Add(new Proportion(ProportionType.Auto)); // Name
            tableGrid.ColumnsProportions.Add(new Proportion(ProportionType.Fill)); // Value

            int row = 0;
            foreach (var kvp in keybindings)
            {
                tableGrid.RowsProportions.Add(new Proportion(ProportionType.Auto));

                // Row background
                var bgPanel = new Panel
                {
                    Background = new Myra.Graphics2D.Brushes.SolidBrush(row % 2 == 0 ? rowBgColor : altRowBgColor),
                    Padding = new Myra.Graphics2D.Thickness(0, 0, 0, 0)
                };
                Grid.SetColumn(bgPanel, 0);
                Grid.SetRow(bgPanel, row);
                Grid.SetColumnSpan(bgPanel, 2);
                tableGrid.Widgets.Add(bgPanel);

                // Name label
                var nameLabel = new Label
                {
                    Text = kvp.Key + ":",
                    TextColor = textColor,
                    Padding = new Myra.Graphics2D.Thickness(8, 2, 8, 2),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(nameLabel, 0);
                Grid.SetRow(nameLabel, row);
                tableGrid.Widgets.Add(nameLabel);

                // Value label
                var valueLabel = new Label
                {
                    Text = kvp.Value,
                    TextColor = textColor,
                    Padding = new Myra.Graphics2D.Thickness(8, 2, 8, 2),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(valueLabel, 1);
                Grid.SetRow(valueLabel, row);
                tableGrid.Widgets.Add(valueLabel);

                row++;
            }

            outerPanel.Widgets.Add(tableGrid);
            return outerPanel;
        }
    }
}
