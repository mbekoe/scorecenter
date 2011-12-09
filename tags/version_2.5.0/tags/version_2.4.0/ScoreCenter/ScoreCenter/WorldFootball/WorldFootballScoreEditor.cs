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
            numNbTeams.Value = 20;
            tbxLevels.Clear();
            tbxHighlights.Clear();
            tbxDetails.Clear();
            ckxLiveEnabled.Checked = false;

            tbxName.Text = score.Name;
            tbxCountry.Text = score.Country;
            tbxLeague.Text = score.League;
            tbxSeason.Text = score.Season;
            numNbTeams.Value = score.NbTeams;
            tbxLevels.Text = score.Levels;
            cbxKind.SelectedValue = score.Kind;
            tbxHighlights.Text = score.Highlights;
            tbxDetails.Text = score.Details;
            tbxScoreId.Text = score.Id;
            ckxLiveEnabled.Checked = score.LiveConfig != null && score.LiveConfig.enabled;

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
            score.Levels = tbxLevels.Text;
            score.Highlights = tbxHighlights.Text;
            score.Details = tbxDetails.Text;
            score.Kind = (WorldFootballKind)Enum.Parse(typeof(WorldFootballKind), cbxKind.SelectedValue.ToString());

            if (ckxLiveEnabled.Checked)
            {
                score.LiveConfig = new LiveConfig();
                score.LiveConfig.enabled = true;
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
        private string GetDefaultIconPath()
        {
            string path = Config.GetSubFolder(Config.Dir.Thumbs, "ScoreCenter");
            path = Path.Combine(Path.Combine(path, "Football"), GetFullName() + ".png");
            return path;
        }

        private void WorldFootballScoreEditor_Load(object sender, EventArgs e)
        {
            cbxKind.DataSource = EnumManager.ReadWorldFootballKind();
            cbxKind.ValueMember = "ID";
            cbxKind.DisplayMember = "NAME";
        }

        private static Image FixedSize(Bitmap imgPhoto, int width, int height, Color backColor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(backColor);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
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

                string path = GetDefaultIconPath();

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
                    Image icon = FixedSize(bmp, 48, 48, Color.White);
                    pictureBox1.Image = icon;

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
