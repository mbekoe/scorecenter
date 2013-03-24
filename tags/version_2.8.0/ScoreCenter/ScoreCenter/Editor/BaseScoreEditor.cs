#region

/* 
 *      Copyright (C) 2009-2013 Team MediaPortal
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
            else
            {
                errorProvider1.SetError(control, String.Empty);
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
