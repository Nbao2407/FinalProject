using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpler
{
    public class PlaceholderHelper
    {
        public static void SetPlaceholder(Control control, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                control.Text = placeholder;
                control.ForeColor = Color.Gray;
            }

            control.Enter += (s, e) =>
            {
                if (control.ForeColor == Color.Gray)
                {
                    control.Text = "";
                    control.ForeColor = Color.Black;
                }
            };

            control.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(control.Text))
                {
                    control.Text = placeholder;
                    control.ForeColor = Color.Gray;
                }
            };
        }

        public static void SetDataAsPlaceholder(Control control, string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                control.Text = data;
                control.ForeColor = Color.Gray;

                control.Enter += (s, e) =>
                {
                    if (control.ForeColor == Color.Gray)
                    {
                        control.Text = "";
                        control.ForeColor = Color.Black;
                    }
                };

                control.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(control.Text))
                    {
                        control.Text = data;
                        control.ForeColor = Color.Gray;
                    }
                };
            }
        }
    }
}
