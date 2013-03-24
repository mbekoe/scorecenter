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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class RssScore
    {
        public override string GetSource()
        {
            return this.Url;
        }
        
        internal override BaseScore Clone(string id)
        {
            RssScore copy = new RssScore();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Name = this.Name;
            copy.Url = this.Url;
            copy.Image = this.Image;
            copy.Parent = this.Parent;

            return (BaseScore)copy;
        }

        internal override bool CanApplySettings()
        {
            return false;
        }

        internal override void ApplySettings(BaseScore settings)
        {
            // nothing
        }
        
        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\rss";
        }
        
        public override bool IsLive()
        {
            return false;
        }

        public override bool CanLive()
        {
            return false;
        }

        public override bool Merge(BaseScore newBaseScore, ImportOptions option)
        {
            RssScore newScore = newBaseScore as RssScore;
            if (newScore == null)
                return false;

            bool result = base.Merge(newScore, option);

            if ((option & ImportOptions.Parsing) == ImportOptions.Parsing)
            {
                result |= (String.Compare(this.Url, newScore.Url, true) != 0)
                    || (String.Compare(this.Encoding, newScore.Encoding, true) != 0);

                this.Url = newScore.Url;
                this.Encoding = newScore.Encoding;
            }

            return result;
        }
    }
}
