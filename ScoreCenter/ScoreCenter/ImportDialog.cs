#region

/* 
 *      Copyright (C) 2009-2013 Team MediaPortal
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
        }

        private void ImportDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                ImportOptions type = ImportOptions.None;
                if (ckxNewScore.Checked) type |= ImportOptions.New;
                if (ckxMergeExisting.Checked)
                {
                    if (ckxNames.Checked) type |= ImportOptions.Names;
                    if (ckxUrl.Checked) type |= ImportOptions.Parsing;
                    if (ckxRules.Checked) type |= ImportOptions.Rules;
                }

                ExchangeManager.Import(m_center, m_fileName, type);
            }
        }

        private void ckxMergeExisting_CheckedChanged(object sender, EventArgs e)
        {
            ckxNames.Enabled = ckxMergeExisting.Checked;
            ckxRules.Enabled = ckxMergeExisting.Checked;
            ckxUrl.Enabled = ckxMergeExisting.Checked;
        }
    }
}
