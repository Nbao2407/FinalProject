using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpler
{
    public class SearchHelper
    {
            public static void UpdateSearchSuggestions<T>(
                Panel resultPanel,
                IEnumerable<T> suggestionResults,
                Control txtSearch,
                int itemHeight,
                int maxPanelHeight,
                Func<T, string> displayFunc,
                Action<T> clickAction) where T : class
            {
                if (resultPanel == null || suggestionResults == null || txtSearch == null || displayFunc == null || clickAction == null)
                    return;

                resultPanel.SuspendLayout();
                resultPanel.Controls.Clear();
                resultPanel.BackColor = Color.White;
                resultPanel.AutoScroll = false; 
                resultPanel.BorderStyle = BorderStyle.FixedSingle;
            var resultsList = suggestionResults.ToList();

                if (resultsList.Any() && !string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    int totalContentHeight = resultsList.Count * itemHeight;
                    resultPanel.Height = Math.Min(totalContentHeight, maxPanelHeight);
                    resultPanel.AutoScroll = totalContentHeight > maxPanelHeight;

                    Color hoverBackColor = Color.FromArgb(230, 240, 255);
                    Color hoverForeColor = Color.FromArgb(0, 102, 204);
                    Color defaultForeColor = Color.FromArgb(51, 51, 51);
                    Color defaultBackColor = Color.White;
                    int currentTop = resultPanel.DisplayRectangle.Height - itemHeight; 
                    if (resultPanel.AutoScroll)
                    {
                        currentTop = totalContentHeight - itemHeight;
                    }

                    for (int i = resultsList.Count - 1; i >= 0; i--)
                    {
                        var item = resultsList[i];
                        Label lbl = new Label
                        {
                            Text = "  " + displayFunc(item),
                            Tag = item,
                            Font = new Font("Segoe UI", 9.5f),
                            ForeColor = defaultForeColor,
                            BackColor = defaultBackColor,
                            Size = new Size(resultPanel.ClientSize.Width, itemHeight),
                            Location = new Point(0, currentTop),
                            Padding = new Padding(12, 0, 12, 0),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Cursor = Cursors.Hand,
                            Margin = new Padding(0),
                        };

                        lbl.MouseEnter += (s, e) => { lbl.BackColor = hoverBackColor; lbl.ForeColor = hoverForeColor; };
                        lbl.MouseLeave += (s, e) => { lbl.BackColor = defaultBackColor; lbl.ForeColor = defaultForeColor; };
                        lbl.Click += (s, e) => { if (lbl.Tag is T selectedItem) { clickAction(selectedItem); resultPanel.Visible = false; } };

                        resultPanel.Controls.Add(lbl);

                        currentTop -= itemHeight;
                    }
                    resultPanel.Visible = true;

                    if (resultPanel.AutoScroll)
                    {
                        resultPanel.ScrollControlIntoView(resultPanel.Controls[0]);
                    }
                }
                else
                {
                    resultPanel.Visible = false;
                }
                resultPanel.ResumeLayout();
            }
    }
}
