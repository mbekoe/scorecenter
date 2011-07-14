using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Dialog to select a score.
    /// This used for example to test a score from a virtual score.
    /// </summary>
    public partial class TestScoreSelector : Form
    {
        public TestScoreSelector(IList<BaseScore> scores)
        {
            InitializeComponent();
            lbxTest.Items.AddRange(scores.ToArray());
            lbxTest.SelectedIndex = 0;
        }

        /// <summary>
        /// Get the score selected by the user.
        /// </summary>
        public BaseScore SelectedScore
        {
            get
            {
                return lbxTest.SelectedItem as BaseScore;
            }
        }
    }
}
