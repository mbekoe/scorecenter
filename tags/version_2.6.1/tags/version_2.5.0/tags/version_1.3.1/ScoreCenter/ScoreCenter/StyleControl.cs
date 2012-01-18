using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class StyleControl : UserControl
    {
        private string m_originalName;
        public EventHandler OnDelete;
        public StyleControl()
        {
            InitializeComponent();
        }

        public StyleControl(string name, long code)
        {
            InitializeComponent();

            m_originalName = name;
            tbxStyleName.Text = name;
            pnlStyle.BackColor = Color.FromArgb((int)code);
            tbxStyleName.ForeColor = pnlStyle.BackColor;
        }

        public string OriginalName
        {
            get { return m_originalName; }
        }

        public string StyleName
        {
            get { return tbxStyleName.Text; }
        }

        public int ColorCode
        {
            get { return pnlStyle.BackColor.ToArgb(); }
        }
        
        public void SetBackColor(Color bckColor)
        {
            tbxStyleName.BackColor = bckColor;
        }

        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pnlStyle.BackColor = colorDialog1.Color;
                tbxStyleName.ForeColor = colorDialog1.Color;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnDelete != null)
            {
                OnDelete(this, null);
            }
        }
    }
}
