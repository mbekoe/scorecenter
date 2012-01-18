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
            List<BaseScore> scores = new List<BaseScore>();

            // navigate through the nodes and export only the checked nodes
            foreach (ThreeStateTreeNode scoreNode in treeView1.GetCheckedNodes())
            {
                if (scoreNode.Checked)
                {
                    BaseScore score = scoreNode.Tag as BaseScore;
                    scores.Add(score);
                }
            }

            ExchangeManager.Export(m_center, fileName, scores);
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
