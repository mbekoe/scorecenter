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
    public partial class GenericScore
    {
        /// <summary>
        /// Creates a new GenericScore.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="xpath"></param>
        /// <param name="image"></param>
        /// <param name="element"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static GenericScore CreateNewScore(string parent, string id, string name, string xpath, string image, string element, int index)
        {
            GenericScore sc = new GenericScore();
            sc.Order = index;
            sc.enable = true;
            sc.Name = name;
            sc.Id = String.Format("{0}-{1}", parent, id);
            sc.Parent = parent;
            sc.XPath = xpath;
            sc.Image = image;
            sc.Element = element;
            sc.SetVirtual(true);
            sc.SetCanLive(false);

            return sc;
        }

        #region Defines abstract methods from BaseScore
        internal override BaseScore Clone(string id)
        {
            GenericScore copy = new GenericScore();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Name = this.Name;
            copy.Url = this.Url;
            copy.XPath = this.XPath;
            copy.XPathRow = this.XPathRow;
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

            if (this.Rules != null && this.Rules.Length > 0)
            {
                copy.Rules = new Rule[this.Rules.Length];
                for (int i = 0; i < this.Rules.Length; i++)
                {
                    copy.Rules[i] = this.Rules[i].Clone();
                }
            }

            if (this.Range != null) copy.Range = this.Range.Clone();

            return (BaseScore)copy;
        }

        internal override bool CanApplySettings()
        {
            return true;
        }

        internal override void ApplySettings(BaseScore score)
        {
            GenericScore settings = score as GenericScore;
            if (settings != null)
            {
                this.XPath = settings.XPath;
                this.XPathRow = settings.XPathRow;
                this.XPathCol = settings.XPathCol;
                this.Skip = settings.Skip;
                this.MaxLines = settings.MaxLines;
                this.Element = settings.Element;
                this.Encoding = settings.Encoding;
                this.ParseOptions = settings.ParseOptions;
            }
        }

        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\score";
        }

        #endregion

        /// <summary>
        /// Gets the source (web site) for this score.
        /// </summary>
        /// <returns></returns>
        public override string GetSource()
        {
            return this.Url;
        }
        
        /// <summary>
        /// Gets all the styles used by this score.
        /// </summary>
        /// <returns>The list of styles' names.</returns>
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
                    || (String.Compare(this.XPathRow, newScore.XPathRow, true) != 0)
                    || (String.Compare(this.XPathCol, newScore.XPathCol, true) != 0)
                    || (this.Skip != newScore.Skip)
                    || (this.MaxLines != newScore.MaxLines)
                    || (String.Compare(this.Encoding, newScore.Encoding, true) != 0)
                    || (String.Compare(this.Element, newScore.Element, true) != 0);

                this.Url = newScore.Url;
                this.XPath = newScore.XPath;
                this.XPathRow = newScore.XPathRow;
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

        /// <summary>
        /// Gets the URL for this scores.
        /// If the URL has a variable parameter, this method returns the URL with the parameter sets.
        /// </summary>
        /// <returns></returns>
        public string GetUrl()
        {
            if (this.Range == null)
                return this.Url;
            return this.Range.Apply(this.Url, this.Range.Value.ToString());
        }

        /// <summary>
        /// Adds a rule to the score.
        /// </summary>
        /// <param name="col">The column number: -1 = the line number, 0 = any columns</param>
        /// <param name="ruleOperator"></param>
        /// <param name="ruleValue"></param>
        /// <param name="action"></param>
        /// <param name="ruleFormat"></param>
        public void AddRule(int col, Operation ruleOperator, string ruleValue, RuleAction action, string ruleFormat)
        {
            MediaPortal.Plugin.ScoreCenter.Rule rule = new MediaPortal.Plugin.ScoreCenter.Rule();
            rule.Column = col;
            rule.Operator = ruleOperator;
            rule.Value = ruleValue;
            rule.Action = action;
            rule.Format = ruleFormat;

            List<MediaPortal.Plugin.ScoreCenter.Rule> rules = new List<MediaPortal.Plugin.ScoreCenter.Rule>();
            if (this.Rules != null)
                rules.AddRange(this.Rules.AsEnumerable());
            rules.Add(rule);
            this.Rules = rules.ToArray();
        }

        /// <summary>
        /// Adds an highlight rule to a score.
        /// </summary>
        /// <param name="highlights">The string to highlight.</param>
        /// <param name="col">The column to search.</param>
        /// <param name="action">The action to do (should be FormatLine or FormatMerge).</param>
        public void AddHighlightRule(string highlights, int col, RuleAction action)
        {
            if (String.IsNullOrEmpty(highlights))
                return;

            List<MediaPortal.Plugin.ScoreCenter.Rule> rules = new List<MediaPortal.Plugin.ScoreCenter.Rule>();
            if (this.Rules != null)
                rules.AddRange(this.Rules.AsEnumerable());

            string[] hh = highlights.Split(',');
            foreach (string h in hh)
            {
                Rule rule = new Rule();
                rule.Column = col;
                rule.Operator = Operation.Contains;
                rule.Value = h;
                rule.Action = action;
                rule.Format = "Highlight";
                rules.Add(rule);
                //Tools.LogMessage("Rule = {0}", rule);
            }

            this.Rules = rules.ToArray();
        }

        /// <summary>
        /// Adds rules for levels.
        /// </summary>
        /// <param name="levels">The level string, ex: 1,3,-3,-2 </param>
        public void AddLevelsRule(string levels)
        {
            if (String.IsNullOrEmpty(levels) == false)
            {
                int i = 0;
                int j = 0;
                foreach (string level in levels.Split(','))
                {
                    if (level.StartsWith("-"))
                    {
                        this.AddRule(-1, Operation.IsLast, level.Substring(1), RuleAction.FormatLine, String.Format("Level{0}", --j));
                    }
                    else
                    {
                        this.AddRule(-1, Operation.LE, level, RuleAction.FormatLine, String.Format("Level{0}", ++i));
                    }
                }
            }
        }
    }
}
