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

using MediaPortal.Plugin.ScoreCenter.Parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class FussballdeScore : BaseScore
    {
        public override bool IsVirtualFolder() { return true; }

        public override IList<BaseScore> GetVirtualScores(ScoreParameter[] parameters)
        {
            return FussballdeScoreParser.GetRealScores(this, parameters);
        }

        internal override BaseScore Clone(string id)
        {
            FussballdeScore copy = new FussballdeScore();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Name = this.Name;
            copy.Image = this.Image;
            copy.Parent = this.Parent;
            copy.LiveConfig = this.LiveConfig;
            copy.Url = this.Url;
            copy.Details = this.Details;
            copy.Levels = this.Levels;
            copy.Highlights = this.Highlights;
            if (this.Range != null)
            {
                copy.Range = this.Range.Clone();
            }

            return (BaseScore)copy;
        }

        internal override bool CanApplySettings()
        {
            return true;
        }

        internal override void ApplySettings(BaseScore score)
        {
            WorldFootballScore settings = score as WorldFootballScore;
            if (settings != null)
            {
                this.LiveConfig = settings.LiveConfig;
                this.Details = settings.Details;
                this.Levels = settings.Levels;
                this.Highlights = settings.Highlights;
            }
        }

        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\fussballde";
        }
    }
}