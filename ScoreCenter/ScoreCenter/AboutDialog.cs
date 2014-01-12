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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// About dialog box.
    /// </summary>
    public partial class AboutDialog : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutDialog(ScoreCenter center)
        {
            InitializeComponent();

            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = v.ToString(4);

            if (center.Scores.Items != null)
            {
                foreach (string tt in ScoreFactory.Instance.GetScoreTypes())
                {
                    AddCategory(tt, center.Scores.Items.Where(sc => sc.GetType().Name == tt + "Score").Count());
                }
                lvwSummary.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void AddCategory(string label, int nb)
        {
            ListViewItem item = new ListViewItem(new string[] { label, nb.ToString() });
            lvwSummary.Items.Add(item);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://code.google.com/p/scorecenter/");
        }
    }
}
