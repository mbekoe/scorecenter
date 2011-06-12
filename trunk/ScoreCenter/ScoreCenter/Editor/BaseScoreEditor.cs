using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public partial class BaseScoreEditor : UserControl
    {
        protected ScoreCenter m_center;

        public BaseScoreEditor()
        {
            InitializeComponent();
        }

        public virtual void LoadScore(BaseScore baseScore, ScoreCenter center)
        {
            throw new NotImplementedException();
        }
        
        public virtual bool SaveScore(ref BaseScore baseScore)
        {
            throw new NotImplementedException();
        }
        
        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual bool CheckData()
        {
            throw new NotImplementedException();
        }

        public virtual bool HasTest
        {
            get { return false; }
        }

        public Panel TestPanel
        {
            get;
            set;
        }

        public virtual Type GetScoreType()
        {
            throw new NotImplementedException();
        }

        public virtual void AlignColumn(Point pt, ContentAlignment alignement)
        {
            // do nothing
        }

        protected bool CheckTextBox(TextBox control, Label label, bool require)
        {
            if (control.Text.Length == 0 && require)
            {
                string message = String.Format(Properties.Resources.RequiredField, label.Text);
                errorProvider1.SetError(control, message);
            }

            return errorProvider1.GetError(control).Length == 0;
        }

        protected bool CheckNumber(TextBox control, Label label, bool require)
        {
            if (control.Text.Length == 0)
            {
                if (require)
                {
                    string message = String.Format(Properties.Resources.RequiredField, label.Text);
                    errorProvider1.SetError(control, message);
                }
            }
            else
            {
                int test;
                if (false == int.TryParse(control.Text, out test))
                {
                    string message = String.Format(Properties.Resources.BadNumberFormat, label.Text);
                    errorProvider1.SetError(control, message);
                }
            }

            return errorProvider1.GetError(control).Length == 0;
        }
    }
}
