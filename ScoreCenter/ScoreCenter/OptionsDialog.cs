#region Copyright (C) 2005-2011 Team MediaPortal

/* 
 *      Copyright (C) 2005-2011 Team MediaPortal
 *      http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// <param name="selectUpdate">True to select the update tab.</param>
        public OptionsDialog(ScoreCenter center, bool selectUpdate)
        {
            InitializeComponent();

            m_center = center;
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");

            List<string> allready = new List<string>();
            if (center.Parameters != null)
            {
                foreach (var p in center.Parameters)
                {
                    dt.Rows.Add(p.name, p.Value);
                    allready.Add(p.name);
                }
            }

            List<string> plist = m_center.GetParameters();
            foreach (string pp in plist)
            {
                if (!allready.Contains(pp))
                {
                    dt.Rows.Add(pp, "");
                }
            }

            dataGridView1.DataSource = dt;
            
            if (selectUpdate)
            {
                tbcOptions.SelectedTab = tpgUpdate;
            }
        }

        /// <summary>
        /// Gets a value indicating if a reload of the settings is required.
        /// </summary>
        public bool ReloadRequired
        {
            get { return m_reload; }
        }

        private static decimal CheckInt(int val, NumericUpDown numbox)
        {
            if (val < numbox.Minimum) return numbox.Minimum;
            if (val > numbox.Maximum) return numbox.Maximum;
            return val;
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
            numCacheExpiration.Value = m_center.Setup.CacheExpiration;
            numNotificationTime.Value = CheckInt(m_center.Setup.LiveNotifTime, numNotificationTime);
            numCheckDelay.Value = CheckInt(m_center.Setup.LiveCheckDelay, numCheckDelay);
            ckxPlaySound.Checked = m_center.Setup.LivePlaySound;
            ckxUseAltColor.Checked = m_center.Setup.UseAltColor;

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
            ckxOverwriteIcons.Checked = ((option & ImportOptions.OverwriteIcons) == ImportOptions.OverwriteIcons);

            ckxMergeExisting.Checked = ckxNames.Checked || ckxRules.Checked || ckxUrl.Checked || ckxOverwriteIcons.Checked;
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
            m_center.Setup.CacheExpiration = (int)numCacheExpiration.Value;
            m_center.Setup.LiveNotifTime = (int)numNotificationTime.Value;
            m_center.Setup.LiveCheckDelay = (int)numCheckDelay.Value;
            m_center.Setup.LivePlaySound = ckxPlaySound.Checked;
            m_center.Setup.UseAltColor = ckxUseAltColor.Checked;

            m_center.Setup.UpdateOnlineMode = Tools.ParseEnum<UpdateMode>(comboBox1.SelectedValue.ToString());
            m_center.Setup.UpdateUrl = tbxUrl.Text;

            ImportOptions options = ReadImportOptions();
            m_center.Setup.UpdateRule = options.ToString();

            List<ScoreParameter> plist = new List<ScoreParameter>();
            DataTable dt = dataGridView1.DataSource as DataTable;
            foreach (DataRow r in dt.Rows)
            {
                ScoreParameter p = new ScoreParameter();
                p.name = r["Name"] as string;
                p.Value = r["Value"] as string;
                if (!String.IsNullOrEmpty(p.name) && !String.IsNullOrEmpty(p.Value))
                {
                    plist.Add(p);
                }
            }

            m_center.Parameters = plist.ToArray();
        }

        private ImportOptions ReadImportOptions()
        {
            ImportOptions options = ImportOptions.None;
            if (ckxNew.Checked) options |= ImportOptions.New;
            if (ckxMergeExisting.Checked)
            {
                if (ckxNames.Checked) options |= ImportOptions.Names;
                if (ckxRules.Checked) options |= ImportOptions.Rules;
                if (ckxUrl.Checked) options |= ImportOptions.Parsing;
                if (ckxOverwriteIcons.Checked) options |= ImportOptions.OverwriteIcons;
            }
            return options;
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
            string strOptions = m_center.Setup.UpdateRule;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                ImportOptions options = ReadImportOptions();

                m_reload = true;
                m_center.Setup.UpdateOnlineMode = UpdateMode.Manually;
                m_center.Setup.UpdateRule = options.ToString();
                ExchangeManager.OnlineUpdate(m_center, true);
            }
            finally
            {
                m_center.Setup.UpdateOnlineMode = mode;
                m_center.Setup.UpdateRule = strOptions;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
