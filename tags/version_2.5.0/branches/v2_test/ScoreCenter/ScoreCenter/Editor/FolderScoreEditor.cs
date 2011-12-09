using System;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public partial class FolderScoreEditor : BaseScoreEditor
    {
        public FolderScoreEditor()
        {
            InitializeComponent();
        }

        public override void LoadScore(BaseScore baseScore, ScoreCenter center)
        {
            tbxScore.Text = baseScore.Name;
        }
        public override Type GetScoreType()
        {
            return typeof(FolderScore);
        }

        public override bool SaveScore(ref BaseScore baseScore)
        {
            if (!CheckData())
                return false;

            baseScore.Name = tbxScore.Text;
            return true;
        }

        public override void Clear()
        {
        }
        public override bool CheckData()
        {
            return CheckTextBox(tbxScore, lblName, true);
        }
    }
}
