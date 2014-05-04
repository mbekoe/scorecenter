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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Plugin.ScoreCenter.Parser;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public partial class WorldFootballScoreEditor : BaseScoreEditor
    {
        public WorldFootballScoreEditor()
        {
            InitializeComponent();
        }

        public override Type GetScoreType()
        {
            return typeof(WorldFootballScore);
        }
        
        public override void LoadScore(BaseScore baseScore, ScoreCenter center)
        {
            m_center = center;
            WorldFootballScore score = baseScore as WorldFootballScore;
            if (score == null)
                throw new NullReferenceException("Not a WorldFootballScore score!");

            errorProvider1.Clear();

            // always clear
            tbxName.Clear();
            tbxCountry.Clear();
            tbxLeague.Clear();
            tbxSeason.Clear();
            tbxLevels.Clear();
            tbxHighlights.Clear();
            tbxDetails.Clear();
            ckxLiveEnabled.Checked = false;
            tbxLiveFilter.Clear();

            tbxName.Text = score.Name;
            tbxCountry.Text = score.Country;
            tbxLeague.Text = score.League;
            tbxSeason.Text = score.Season;
            numNbTeams.Value = score.NbTeams;
            numRounds.Value = score.Rounds;
            tbxLevels.Text = score.Levels;
            cbxKind.SelectedValue = score.Kind;
            tbxHighlights.Text = score.Highlights;
            tbxDetails.Text = score.Details;
            tbxScoreId.Text = score.Id;
            
            if (score.LiveConfig != null)
            {
                ckxLiveEnabled.Checked = score.LiveConfig.enabled;
                tbxLiveFilter.Text = score.LiveConfig.filter;
            }

            if (!String.IsNullOrEmpty(score.Image))
            {
                string iconPath = Path.Combine(Config.GetSubFolder(Config.Dir.Thumbs, "ScoreCenter"), score.Image + ".png");
                if (File.Exists(iconPath))
                    pictureBox1.Image = new Bitmap(iconPath);
                else
                    pictureBox1.Image = Properties.Resources.wfb_logo;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.wfb_logo;
            }
        }

        public override bool SaveScore(ref BaseScore baseScore)
        {
            if (!CheckData())
                return false;

            WorldFootballScore score = baseScore as WorldFootballScore;

            score.Name = tbxName.Text;
            score.Country = tbxCountry.Text;
            score.League = tbxLeague.Text;
            score.Season = tbxSeason.Text;
            score.NbTeams = (int)numNbTeams.Value;
            score.Rounds = (int)numRounds.Value;
            score.Levels = tbxLevels.Text;
            score.Highlights = Tools.TrimList(tbxHighlights.Text);
            score.Details = Tools.TrimList(tbxDetails.Text);
            tbxDetails.Text = score.Details;
            score.Kind = (WorldFootballKind)Enum.Parse(typeof(WorldFootballKind), cbxKind.SelectedValue.ToString());

            if (ckxLiveEnabled.Checked && tbxLiveFilter.Text.Length > 0)
            {
                score.LiveConfig = new LiveConfig();
                score.LiveConfig.enabled = true;
                score.LiveConfig.filter = Tools.TrimList(tbxLiveFilter.Text);
                tbxLiveFilter.Text = score.LiveConfig.filter;
            }
            else
            {
                score.LiveConfig = null;
            }

            if (String.IsNullOrEmpty(score.Image))
            {
                score.Image = String.Format("Football\\{0}", GetFullName());
            }

            return true;
        }

        public override bool HasTest
        {
            get
            {
                return true;
            }
        }

        public override bool CheckData()
        {
            bool result = CheckTextBox(tbxName, lblName, true);
            result &= CheckTextBox(tbxLeague, lblLeague, true);

            return result;
        }

        private void btnOpenUrl_Click(object sender, EventArgs e)
        {
            if (tbxLeague.Text.Length > 0)
            {
                string url = GetUrl();
                url = Tools.ParseUrl(url, m_center.Parameters);
                Process.Start(url);
            }
        }

        private string GetFullName()
        {
            string fullname = tbxLeague.Text;
            if (tbxCountry.Text.Length > 0)
                fullname = String.Format("{0}-{1}", tbxCountry.Text, tbxLeague.Text);
            
            return fullname;
        }
        private string GetUrl()
        {
            var kind = (WorldFootballKind)Enum.Parse(typeof(WorldFootballKind), cbxKind.SelectedValue.ToString());
            if (kind == WorldFootballKind.Team)
                return String.Format("{{@worldfootball}}teams/{0}/", GetFullName());

            return String.Format("{{@worldfootball}}wettbewerb/{0}/", GetFullName());
        }

        private void WorldFootballScoreEditor_Load(object sender, EventArgs e)
        {
            cbxKind.DataSource = EnumManager.ReadWorldFootballKind();
            cbxKind.ValueMember = "ID";
            cbxKind.DisplayMember = "NAME";
        }

        private void btnIcon_Click(object sender, EventArgs e)
        {
            if (tbxLeague.Text.Length > 0)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

                string url = Tools.ParseUrl(GetUrl(), m_center.Parameters);
                ScoreCache cache = new ScoreCache(0);
                string home = cache.GetScore(url, "", false);

                string emblemUrl = WorldFootballScoreParser.GetEmblemUrl(home);
                if (String.IsNullOrEmpty(emblemUrl))
                    return;

                string tmpfile = Path.GetTempFileName();
                File.Delete(tmpfile);
                tmpfile += ".png";
                Bitmap bmp = null;
                try
                {
                    cache.GetImage(emblemUrl, tmpfile);
                    bmp = new Bitmap(tmpfile);
                    Image icon = Tools.FixedBitmapSize(bmp, 48, 48, Color.White);
                    pictureBox1.Image = icon;

                    string path = Config.GetSubFolder(Config.Dir.Thumbs, "ScoreCenter");
                    path = Path.Combine(path, "Football", GetFullName() + ".png");
                    icon.Save(path, ImageFormat.Png);
                }
                finally
                {
                    cache = null;
                    if (bmp != null) bmp.Dispose();
                    File.Delete(tmpfile);
                }
            }
        }

        private void cbxDetailsHelper_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = cbxDetailsHelper.Text;
            string details = tbxDetails.Text;
            if ((details.Length == 0 || !details.Contains(",")) && details == selectedItem)
                return;
            if (details.Length > 0)
            {
                if (details.StartsWith(selectedItem + ",") || details.Contains("," + selectedItem + ",") || details.EndsWith("," + selectedItem))
                    return;
                tbxDetails.AppendText(",");
            }
            tbxDetails.AppendText(selectedItem);
        }
    }
}
