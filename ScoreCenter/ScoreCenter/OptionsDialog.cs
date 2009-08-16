using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class OptionsDialog : Form
    {
        private ScoreCenter m_center;
        public OptionsDialog(ScoreCenter center)
        {
            InitializeComponent();

            m_center = center;
        }

        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            string name = ScoreCenterPlugin.DefaultPluginName;
            if (m_center.Setup != null && !String.IsNullOrEmpty(m_center.Setup.Name))
            {
                name = m_center.Setup.Name;
            }
            
            tbxName.Text = name;
            numericUpDown1.Value = m_center.Setup.CacheExpiration;
        }

        private void OptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (m_center.Setup == null)
                    m_center.Setup = new ScoreCenterSetup();

                m_center.Setup.Name = tbxName.Text;
            }
        }
    }
}
