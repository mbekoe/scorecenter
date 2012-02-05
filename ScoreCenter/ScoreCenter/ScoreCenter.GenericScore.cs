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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class GenericScore
    {
        public override string GetSource()
        {
            return this.Url;
        }
        
        public override IList<string> GetStyles()
        {
            if (this.Rules == null)
                return base.GetStyles();

            List<string> styles = new List<string>();
            foreach (Rule r in this.Rules)
            {
                if (styles.Contains(r.Format) == false)
                    styles.Add(r.Format);
            }

            return styles;
        }

        public static bool CheckParsingOption(ParsingOptions opt, ParsingOptions o)
        {
            return (opt & o) == o;
        }

        public ParsingOptions GetParseOption()
        {
            ParsingOptions opt = ParsingOptions.None;
            if (!String.IsNullOrEmpty(this.ParseOptions))
            {
                if (!String.IsNullOrEmpty(this.ParseOptions))
                    opt = (ParsingOptions)Enum.Parse(typeof(ParsingOptions), this.ParseOptions);
            }

            return opt;
        }

        public void SetParseOption(bool caption, bool theader, bool newLine, bool wordWrap, bool reverse, bool imgAlt)
        {
            ParsingOptions opt = ParsingOptions.None;

            if (caption) opt |= ParsingOptions.Caption;
            if (theader) opt |= ParsingOptions.UseTheader;
            if (newLine) opt |= ParsingOptions.NewLine;
            if (wordWrap) opt |= ParsingOptions.WordWrap;
            if (reverse) opt |= ParsingOptions.Reverse;
            if (imgAlt) opt |= ParsingOptions.ImgAlt;

            this.ParseOptions = opt.ToString();
        }

        internal override BaseScore Clone(string id)
        {
            GenericScore copy = new GenericScore();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Name = this.Name;
            copy.Url = this.Url;
            copy.XPath = this.XPath;
            copy.XPathCol = this.XPathCol;
            copy.Headers = this.Headers;
            copy.Sizes = this.Sizes;
            copy.Skip = this.Skip;
            copy.MaxLines = this.MaxLines;
            copy.Image = this.Image;
            copy.Element = this.Element;
            copy.Encoding = this.Encoding;
            copy.ParseOptions = this.ParseOptions;
            copy.Parent = this.Parent;
            copy.LiveConfig = this.LiveConfig;
            copy.Dictionary = this.Dictionary;

            if (this.Range != null) copy.Range = this.Range.Clone();

            return (BaseScore)copy;
        }

        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\score";
        }

        public override bool Merge(BaseScore newBaseScore, ImportOptions option)
        {
            GenericScore newScore = newBaseScore as GenericScore;
            if (newScore == null)
                return false;

            bool result = base.Merge(newBaseScore, option);

            if ((option & ImportOptions.Parsing) == ImportOptions.Parsing)
            {
                result |= (String.Compare(this.Url, newScore.Url, true) != 0)
                    || (String.Compare(this.XPath, newScore.XPath, true) != 0)
                    || (String.Compare(this.XPathCol, newScore.XPathCol, true) != 0)
                    || (this.Skip != newScore.Skip)
                    || (this.MaxLines != newScore.MaxLines)
                    || (String.Compare(this.Encoding, newScore.Encoding, true) != 0)
                    || (String.Compare(this.Element, newScore.Element, true) != 0);

                this.Url = newScore.Url;
                this.XPath = newScore.XPath;
                this.XPathCol = newScore.XPathCol;
                this.Skip = newScore.Skip;
                this.MaxLines = newScore.MaxLines;
                this.Sizes = newScore.Sizes;
                this.Element = newScore.Element;
                this.BetweenElts = newScore.BetweenElts;
                this.Encoding = newScore.Encoding;
                this.ParseOptions = newScore.ParseOptions;
                if (newScore.Headers.Length > 0 || this.Headers.Length == 0)
                {
                    this.Headers = newScore.Headers;
                    result = true;
                }
            }

            if ((option & ImportOptions.Rules) == ImportOptions.Rules)
            {
                if (newScore.Rules != null && newScore.Rules.Length > 0)
                {
                    this.Rules = newScore.Rules;
                    result = true;
                }
            }

            return result;
        }
        public string GetUrl()
        {
            if (this.Range == null)
                return this.Url;
            return this.Range.Apply(this.Url, this.Range.Value.ToString());
        }
        public override void ApplyRangeValue(bool setDefault)
        {
            if (this.Range == null)
                return;

            if (setDefault)
            {
                this.Url = this.Range.ApplyDefault(this.Url);
            }
            else
            {
                this.Url = this.Range.ApplyCurrent(this.Url);
            }
        }
    }
}
