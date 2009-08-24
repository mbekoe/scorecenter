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
            if (m_center.Setup != null)
            {
                if (!String.IsNullOrEmpty(m_center.Setup.Name))
                {
                    name = m_center.Setup.Name;
                }

                tbxBackdrop.Text = m_center.Setup.BackdropDir;
            }
             
            tbxName.Text = name;
            numericUpDown1.Value = m_center.Setup.CacheExpiration;

            tbxUrl.Text = m_center.Setup.UpdateUrl;
            ImportOptions option = ImportOptions.None;
            if (m_center.Setup.UpdateRule.Length > 0)
            {
                option = (ImportOptions)Enum.Parse(typeof(ImportOptions), m_center.Setup.UpdateRule, true);
            }

            ckxUpdateOnline.Checked = m_center.Setup.UpdateOnline;
            ckxNew.Checked = ((option & ImportOptions.New) == ImportOptions.New);
            ckxNames.Checked = ((option & ImportOptions.Names) == ImportOptions.Names);
            ckxRules.Checked = ((option & ImportOptions.Rules) == ImportOptions.Rules);
            ckxUrl.Checked = ((option & ImportOptions.Parsing) == ImportOptions.Parsing);
        }

        private void OptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (m_center.Setup == null)
                    m_center.Setup = new ScoreCenterSetup();

                m_center.Setup.Name = tbxName.Text;
                m_center.Setup.BackdropDir = tbxBackdrop.Text;
                m_center.Setup.UpdateUrl = tbxUrl.Text;

                ImportOptions options = ImportOptions.None;
                if (ckxNew.Checked) options |= ImportOptions.New;
                if (ckxMergeExisting.Checked)
                {
                    if (ckxNames.Checked) options |= ImportOptions.Names;
                    if (ckxRules.Checked) options |= ImportOptions.Rules;
                    if (ckxUrl.Checked) options |= ImportOptions.Parsing;
                }

                m_center.Setup.UpdateRule = options.ToString();
                m_center.Setup.UpdateOnline = ckxUpdateOnline.Checked;
            }
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxBackdrop.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ExchangeManager.OnlineUpdate(m_center);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
