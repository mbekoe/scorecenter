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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ScoreDifference
    {
        private const string C_SEPARATORS = " ;,.:?!'\"(){}[]+-/*|\\";
        
        public string Word;
        public string Separator;
        public bool IsNew = false;

        public ScoreDifference(string word, string sep)
        {
            this.Word = word;
            this.Separator = sep;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ScoreDifference other = obj as ScoreDifference;
            if (other == null)
                return false;

            return String.Equals(this.Word, other.Word)
                && String.Equals(this.Separator, other.Separator);
        }

        public override int GetHashCode()
        {
            return (this.Word + this.Separator).GetHashCode();
        }

        public static List<ScoreDifference> ListFromString(string score)
        {
            List<ScoreDifference> list = new List<ScoreDifference>();

            string curr = "";
            foreach (char c in score)
            {
                if (!C_SEPARATORS.Contains(c))
                {
                    curr += c;
                }
                else
                {
                    ScoreDifference d = new ScoreDifference(curr, c.ToString());
                    list.Add(d);
                    curr = "";
                }
            }
            list.Add(new ScoreDifference(curr, ""));

            return list;
        }

        public static string StringFromList(IEnumerable<ScoreDifference> list, string separator)
        {
            string res = "";
            foreach (ScoreDifference diff in list)
            {
                res += String.Format("{2}{0}{2}{1}", diff.Word, diff.Separator, diff.IsNew ? separator : "");
            }
            return res;
        }
    }
}
