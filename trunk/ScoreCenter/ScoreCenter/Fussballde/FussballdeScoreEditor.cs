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
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Plugin.ScoreCenter;
using MediaPortal.Plugin.ScoreCenter.Editor;
using MediaPortal.Plugin.ScoreCenter.Parser;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public partial class FussballdeScoreEditor : BaseScoreEditor
    {
        private int m_iconIndex = 0;

        public FussballdeScoreEditor()
        {
            InitializeComponent();
        }

        public override void LoadScore(BaseScore baseScore, ScoreCenter center)
        {
            FussballdeScore fsc = baseScore as FussballdeScore;
            m_center = center;
            m_iconIndex = 0;

            errorProvider1.Clear();

            tbxScoreId.Text = fsc.Id;
            tbxName.Text = fsc.Name;
            tbxUrl.Text = fsc.Url;
            tbxDetails.Text = fsc.Details;
            tbxLevels.Text = fsc.Levels;
            tbxHighlights.Text = fsc.Highlights;

            if (!String.IsNullOrEmpty(fsc.Image))
            {
                string iconPath = Path.Combine(Config.GetSubFolder(Config.Dir.Thumbs, "ScoreCenter"), fsc.Image + ".png");
                if (File.Exists(iconPath))
                    pictureBox1.Image = new Bitmap(iconPath);
                else
                    pictureBox1.Image = Properties.Resources.fussballde_logo;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.fussballde_logo;
            }
        }

        public override Type GetScoreType()
        {
            return typeof(FussballdeScore);
        }

        public override bool SaveScore(ref BaseScore baseScore)
        {
            if (!CheckData())
                return false;

            FussballdeScore score = baseScore as FussballdeScore;

            score.Name = tbxName.Text;
            score.Url = tbxUrl.Text;
            score.Details = tbxDetails.Text;
            score.Levels = tbxLevels.Text;
            score.Highlights = tbxHighlights.Text;
            return true;
        }

        public override bool CheckData()
        {
            bool res = CheckTextBox(tbxName, lblName, true);
            res &= CheckTextBox(tbxUrl, lblUrl, true);
            return res;
        }

        public override bool HasTest
        {
            get
            {
                return true;
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

        private void btnOpenUrl_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text.Length > 0)
            {
                string url = Tools.ParseUrl(FussballdeScoreParser.GetMainUrl(tbxUrl.Text), m_center.Parameters);
                Process.Start(url);
            }
        }

        private void btnIcon_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text.Length > 0)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

                ScoreCache cache = new ScoreCache(0);

                string url = Tools.ParseUrl(FussballdeScoreParser.GetMainUrl(tbxUrl.Text), m_center.Parameters);
                string home = cache.GetScore(url, "", false);
                string emblemUrl = FussballdeScoreParser.GetEmblemUrl(home, m_iconIndex++);

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

                    string relpath = Path.Combine("Football", tbxScoreId.Text);
                    string path = Path.Combine(Config.GetSubFolder(Config.Dir.Thumbs, "ScoreCenter"), relpath) + ".png";
                    icon.Save(path, ImageFormat.Png);
                    NotifySetIcon(relpath);
                }
                finally
                {
                    cache = null;
                    if (bmp != null) bmp.Dispose();
                    File.Delete(tmpfile);
                }
            }
        }
    }
}
