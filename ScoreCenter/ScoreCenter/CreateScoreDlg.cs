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
        private Score m_parentScore = null;

        public CreateScoreDlg(Score parent)
        {
            InitializeComponent();

            m_parentScore = parent;
            if (m_parentScore != null)
            {
                rbnParent.Text = m_parentScore.Name;
                rbnParent.Checked = true;
            }
            else
            {
                rbnNone.Checked = true;
                rbnParent.Visible = false;
            }

            cbxType.DataSource = Enum.GetValues(typeof(Node));
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

        public Score NewScore
        {
            get
            {
                Score score = new Score();
                score.Id = Tools.GenerateId();
                score.Name = tbxName.Text;
                score.Type = (Node)cbxType.SelectedValue;
                if (rbnParent.Checked && m_parentScore != null)
                {
                    score.Parent = m_parentScore.Id;
                }

                return score;
            }
        }
    }
}
