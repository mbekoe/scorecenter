#region Copyright (C) 2005-2011 Team MediaPortal

/* 
 *      Copyright (C) 2005-2011 Team MediaPortal
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
