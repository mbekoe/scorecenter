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
    public partial class ImportDialog : Form
    {
        private ScoreCenter m_center;
        private string m_fileName;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="file">The file to import.</param>
        /// <param name="center">The score center to import into.</param>
        public ImportDialog(string file, ScoreCenter center)
        {
            InitializeComponent();

            m_center = center;
            m_fileName = file;
            rbnNewOnly.Checked = true;
        }

        private void rbn_CheckedChanged(object sender, EventArgs e)
        {
            ckxNames.Enabled = rbnMerge.Enabled;
            ckxRules.Enabled = ckxNames.Enabled;
            ckxUrl.Enabled = ckxNames.Enabled;
        }

        private void ImportDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                ImportScores();
            }
        }

        private void ImportScores()
        {
            ScoreCenter imported = Tools.ReadSettings(m_fileName, true);
            if (imported.Scores == null)
                return;

            MergeType type = MergeType.None;
            if (ckxNames.Checked) type |= MergeType.Names;
            if (ckxUrl.Checked) type |= MergeType.Parsing;
            if (ckxRules.Checked) type |= MergeType.Rules;

            List<Score> toImport = new List<Score>();
            foreach (Score score in imported.Scores)
            {
                Score exist = m_center.FindScore(score.Id);
                if (exist == null)
                {
                    toImport.Add(score);
                }
                else
                {
                    exist.Merge(score, type);
                }
            }

            if (toImport.Count > 0)
            {
                Score[] list = new Score[m_center.Scores.Length + toImport.Count];
                m_center.Scores.CopyTo(list, 0);
                
                int i = m_center.Scores.Length;
                foreach (Score sc in toImport)
                {
                    list[i++] = sc;
                }

                m_center.Scores = list;
            }
        }
    }
}
