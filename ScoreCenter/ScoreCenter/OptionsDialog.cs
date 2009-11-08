using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class OptionsDialog : Form
    {
        private ScoreCenter m_center;
        private bool m_reload; // false

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="center">The score center to use.</param>
        public OptionsDialog(ScoreCenter center)
        {
            InitializeComponent();

            m_center = center;
        }

        /// <summary>
        /// Gets a value indicating if a reload of the settings is required.
        /// </summary>
        public bool ReloadRequired
        {
            get { return m_reload; }
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

            DataTable dt = EnumManager.ReadOnlineUpdateMode();
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "NAME";
            comboBox1.ValueMember = "ID";
            comboBox1.SelectedValue = m_center.Setup.UpdateOnlineMode;

            ckxNew.Checked = ((option & ImportOptions.New) == ImportOptions.New);
            ckxNames.Checked = ((option & ImportOptions.Names) == ImportOptions.Names);
            ckxRules.Checked = ((option & ImportOptions.Rules) == ImportOptions.Rules);
            ckxUrl.Checked = ((option & ImportOptions.Parsing) == ImportOptions.Parsing);
        }

        private void OptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                SaveOptions();
            }
        }

        /// <summary>
        /// Save options to score center.
        /// </summary>
        private void SaveOptions()
        {
            if (m_center.Setup == null)
                m_center.Setup = new ScoreCenterSetup();

            m_center.Setup.Name = tbxName.Text;
            m_center.Setup.BackdropDir = tbxBackdrop.Text;

            m_center.Setup.UpdateOnlineMode = Tools.ParseEnum<UpdateMode>(comboBox1.SelectedValue.ToString());
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
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            // reselect current
            if (Directory.Exists(tbxBackdrop.Text))
            {
                folderBrowserDialog1.SelectedPath = tbxBackdrop.Text;
            }
            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxBackdrop.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            UpdateMode mode = m_center.Setup.UpdateOnlineMode;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                SaveOptions();

                m_reload = true;
                m_center.Setup.UpdateOnlineMode = UpdateMode.Manually;
                ExchangeManager.OnlineUpdate(m_center, true);
            }
            finally
            {
                m_center.Setup.UpdateOnlineMode = mode;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
