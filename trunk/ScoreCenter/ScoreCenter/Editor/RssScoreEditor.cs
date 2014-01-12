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
using System.Diagnostics;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public partial class RssScoreEditor : BaseScoreEditor
    {
        public RssScoreEditor()
        {
            InitializeComponent();
        }

        public override bool HasTest
        {
            get
            {
                return true;
            }
        }

        public override Type GetScoreType()
        {
            return typeof(RssScore);
        }
        
        public override void LoadScore(BaseScore baseScore, ScoreCenter center)
        {
            m_center = center;
            RssScore score = baseScore as RssScore;
            if (score == null)
                throw new NullReferenceException("Not a RSS score!");

            tbxScoreId.Text = score.Id;
            tbxScore.Text = score.Name;
            tbxUrl.Text = score.Url;
            tbxEncoding.Text = score.Encoding;
        }

        public override bool SaveScore(ref BaseScore baseScore)
        {
            if (!CheckData())
                return false;

            RssScore score = baseScore as RssScore;

            score.Name = tbxScore.Text;
            score.Url = tbxUrl.Text;
            score.Encoding = tbxEncoding.Text;

            return true;
        }

        public override bool CheckData()
        {
            bool result = CheckTextBox(tbxScore, lblName, true);
            result &= CheckTextBox(tbxUrl, lblUrl, true);

            return result;
        }

        private void btnOpenUrl_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text.Length > 0)
            {
                string url = Tools.ParseUrl(tbxUrl.Text, m_center.Parameters);
                Process.Start(url);
            }
        }
    }
}
