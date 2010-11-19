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
    /// <summary>
    /// Export Dialog.
    /// </summary>
    public partial class ExportDialog : Form
    {
        /// <summary>
        /// The source.
        /// </summary>
        private ScoreCenter m_center;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="center">The source to export.</param>
        public ExportDialog(ScoreCenter center)
        {
            InitializeComponent();

            m_center = center;
        }

        private void ExportDialog_Load(object sender, EventArgs e)
        {
            ScoreCenterConfig.BuildScoreList(treeView1, m_center, false);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportScores(string fileName)
        {
            List<Score> scores = new List<Score>();
            List<string> styles = new List<string>();

            // navigate through the nodes and export only the checked nodes
            foreach (ThreeStateTreeNode scoreNode in treeView1.GetCheckedNodes())
            {
                if (scoreNode.Checked)
                {
                    Score score = scoreNode.Tag as Score;
                    scores.Add(score);

                    if (score.Rules != null)
                    {
                        foreach (Rule r in score.Rules)
                        {
                            if (styles.Contains(r.Format) == false)
                                styles.Add(r.Format);
                        }
                    }
                }
            }

            ExchangeManager.Export(m_center, fileName, scores, styles);
        }

        private void ExportDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ExportScores(saveFileDialog1.FileName);
                }
            }
        }
    }
}
