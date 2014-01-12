#region

/* 
 *      Copyright (C) 2009-2014 Team MediaPortal
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
    public partial class CreateScoreDlg : Form
    {
        private string m_parentId = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent"></param>
        public CreateScoreDlg(BaseScore parent)
        {
            InitializeComponent();

            rbnNone.Checked = true;
            if (parent != null)
            {
                rbnParent.Text = parent.Name;
                rbnParent.Checked = parent.IsContainer();
                rbnParent.Enabled = rbnParent.Checked;
                m_parentId = parent.Id;
            }
            else
            {
                rbnParent.Visible = false;
            }

            cbxType.DataSource = ScoreFactory.Instance.GetScoreTypes();
            tbxName.Text = Properties.Resources.NewItem;
        }

        private void CreateScoreDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (tbxName.Text.Length == 0)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        public BaseScore NewScore
        {
            get
            {
                BaseScore score = ScoreFactory.Instance.CreateScore(cbxType.SelectedValue.ToString());
                score.Name = tbxName.Text;
                if (rbnParent.Checked && !String.IsNullOrEmpty(m_parentId))
                {
                    score.Parent = m_parentId;
                }

                return score;
            }
        }
    }
}
