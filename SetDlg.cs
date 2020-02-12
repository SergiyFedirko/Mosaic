using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mosaic
{
    public partial class SetDlg : Form
    {
        public SetDlg()
        {
            InitializeComponent();
        }

        public int LengthSides = 3;

        private void SetDlg_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = LengthSides;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            LengthSides = (int)numericUpDown1.Value;
        }

        
    }
}
