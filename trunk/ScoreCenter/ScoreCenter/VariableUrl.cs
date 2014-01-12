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
    /// <summary>
    /// Class to handle variable url.
    /// </summary>
    public class VariableUrl
    {
        public const string PARAM = "{#}";

        public int Minimun { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }
        public int Default { get; set; }
        public string Template { get; set; }

        public VariableUrl(int value, int minimum, int maximum, string template)
        {
            Minimun = minimum;
            Maximum = maximum;
            Value = value;
            Default = value;
            Template = template;
        }

        public VariableUrl Clone()
        {
            VariableUrl range = new VariableUrl(Default, Minimun, Maximum, Template);
            range.Value = this.Value;
            range.Template = this.Template;
            return range;
        }

        public bool HasPrev() { return Value > Minimun; }
        public bool HasNext() { return Value < Maximum; }
        public void MovePrev() { Value--; }
        public void MoveNext() { Value++; }
        public void Reset() { Value = Default; }

        public string Apply(string url, string param)
        {
            return url.Replace(VariableUrl.PARAM, param);
        }
        public string ApplyDefault(string url)
        {
            return Apply(url, Default.ToString());
        }
        public string ApplyCurrent(string url)
        {
            return Apply(url, Value.ToString());
        }
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Template)) return Value.ToString();
            return String.Format(Template, Value);
        }
    }
}
