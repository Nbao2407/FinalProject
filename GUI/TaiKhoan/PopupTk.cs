﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.TaiKhoan
{
    public partial class PopupTk : Form
    {
        public PopupTk()
        {
            InitializeComponent();
        }

        private void PopupTk_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            using (var edit = new EditTk())
            {
                this.Close();
                edit.Deactivate += (s, e) => edit.TopMost = true;
                edit.StartPosition = FormStartPosition.CenterParent;
                edit.ShowDialog();
            }
        }

        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
