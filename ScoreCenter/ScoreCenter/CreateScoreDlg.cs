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

            if (parent != null)
            {
                rbnParent.Text = parent.Name;
                rbnParent.Checked = true;
                m_parentId = parent.Id;
            }
            else
            {
                rbnNone.Checked = true;
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
