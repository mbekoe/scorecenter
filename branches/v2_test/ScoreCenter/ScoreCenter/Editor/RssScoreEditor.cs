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

        public override void Clear()
        {
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
