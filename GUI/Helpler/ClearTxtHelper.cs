using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpler
{
    public class ClearTxtHelper
    {
        public void ClearTextBoxes(Control parent)
        {
            parent.Controls.OfType<TextBox>().ToList().ForEach(textBox =>
            {
                textBox.Text = string.Empty;
                textBox.ForeColor = Color.Gray;
            });
        }

    }
}
