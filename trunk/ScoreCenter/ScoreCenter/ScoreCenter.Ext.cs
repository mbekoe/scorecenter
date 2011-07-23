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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MediaPortal.Plugin.ScoreCenter.Parser;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Import Options.
    /// </summary>
    [Flags]
    public enum ImportOptions
    {
        None = 0,
        New = 1,
        Names = 2,
        Parsing = 4,
        Rules = 8,
        OverwriteIcons = 16,
        All = New | Names | Parsing | Rules | OverwriteIcons
    }
    [Flags]
    public enum ParsingOptions
    {
        None = 0,
        UseTheader = 1,
        Caption = 2,
        NewLine = 4,
        WordWrap = 8,
        Reverse = 16
    }

    public static partial class EnumManager
    {
        public static DataTable ReadBetweenElements()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(BetweenElements.EmptyLine.ToString(), "Empty Line");
            dt.Rows.Add(BetweenElements.None.ToString(), "Nothing");
            dt.Rows.Add(BetweenElements.RepeatHeader.ToString(), "Repeat Header");

            return dt;
        }

        public static DataTable ReadRuleAction()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(RuleAction.FormatCell.ToString(), "Format Cell");
            dt.Rows.Add(RuleAction.FormatLine.ToString(), "Format Line");
            dt.Rows.Add(RuleAction.MergeCells.ToString(), "Merge Cells");
            dt.Rows.Add(RuleAction.ReplaceText.ToString(), "Replace Text");
            dt.Rows.Add(RuleAction.SkipLine.ToString(), "Skip Line");

            return dt;
        }

        public static DataTable ReadOperation()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(Operation.EqualTo, "=");
            dt.Rows.Add(Operation.NotEqualTo, "!=");
            dt.Rows.Add(Operation.GT, ">");
            dt.Rows.Add(Operation.GE, ">=");
            dt.Rows.Add(Operation.LT, "<");
            dt.Rows.Add(Operation.LE, "<=");
            dt.Rows.Add(Operation.Contains, "Contains");
            dt.Rows.Add(Operation.NotContains, "Not Contains");
            dt.Rows.Add(Operation.MOD, "Modulo");
            dt.Rows.Add(Operation.EndsWith, "Ends with");
            dt.Rows.Add(Operation.NotEndsWith, "Not Ends with");
            dt.Rows.Add(Operation.StartsWith, "Starts with");
            dt.Rows.Add(Operation.NotStartsWith, "Not Starts with");
            dt.Rows.Add(Operation.IsLast, "Is Last");
            dt.Rows.Add(Operation.InList, "In List");
            dt.Rows.Add(Operation.IsNull, "Is Null");

            return dt;
        }

        public static DataTable ReadOnlineUpdateMode()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(UpdateMode.Never, "Never");
            dt.Rows.Add(UpdateMode.Once, "One Time");
            dt.Rows.Add(UpdateMode.Always, "Always");
            dt.Rows.Add(UpdateMode.Manually, "Manually");

            return dt;
        }
    }

    public partial class ScoreCenter
    {
        public void AddScore(BaseScore score)
        {
            this.Scores.Items = Tools.AddElement<BaseScore>(this.Scores.Items, score);
        }

        public void RemoveScore(BaseScore score)
        {
            this.Scores.Items = Tools.RemoveElement<BaseScore>(this.Scores.Items, score);
        }

        public IEnumerable<BaseScore> ReadChildren(string id)
        {
            return this.Scores.Items.Where(score => score.enable && score.Parent == id);
        }

        public void DisableScore(BaseScore score)
        {
            if (score == null)
                return;

            score.enable = false;
            foreach (BaseScore sc in this.ReadChildren(score.Id))
            {
                DisableScore(sc);
            }
        }
        public string GetFullName(BaseScore score, string sep)
        {
            if (score == null)
                return "";

            string full = score.LocName;
            if (!String.IsNullOrEmpty(score.Parent))
            {
                full = GetFullName(this.FindScore(score.Parent), sep) + sep + full;
            }

            return full;
        }

        public BaseScore FindScore(string id)
        {
            if (this.Scores == null || String.IsNullOrEmpty(id))
                return null;

            return this.Scores.Items.FirstOrDefault(score => score.Id == id);
        }

        public Style FindStyle(string name)
        {
            if (this.Styles == null || String.IsNullOrEmpty(name))
                return null;

            return this.Styles.FirstOrDefault(style => style.Name == name);
        }

        public bool OverrideIcons()
        {
            if (this.Setup.UpdateRule.Length == 0)
                return false;

            ImportOptions option = (ImportOptions)Enum.Parse(typeof(ImportOptions), this.Setup.UpdateRule, true);
            return ((option & ImportOptions.OverwriteIcons) == ImportOptions.OverwriteIcons);
        }

        public List<string> GetParameters()
        {
            List<string> plist = new List<string>();
            Regex rule = new Regex(@"{@(?<a>[^}]*)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            foreach (GenericScore sc in this.Scores.Items.OfType<GenericScore>())
            {
                if (!sc.enable || !sc.Url.Contains("{"))
                    continue;

                MatchCollection mc = rule.Matches(sc.Url);
                if (mc == null)
                    continue;
                foreach (Match m in mc)
                {
                    string v = m.Value.Substring(2); // remove '{@'
                    if (plist.Contains(v) == false)
                    {
                        plist.Add(v);
                    }
                }
            }
            
            return plist;
        }
    }

    public abstract partial class BaseScore
    {
        /// <summary>
        /// Flag to identify new downloaded score.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore(), DefaultValue(false)]
        public bool IsNew { get; set; }

        [System.Xml.Serialization.XmlIgnore(), DefaultValue(false)]
        public bool IsVirtual { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        public string LocName
        {
            get
            {
                return LocalizationManager.GetScoreString(this.Id, this.Name);
            }
        }

        internal abstract BaseScore Clone(string id);
        internal abstract void SetDefaultIcon();

        public virtual bool IsFolder()
        {
            return false;
        }
        public virtual bool IsVirtualFolder()
        {
            return false;
        }
        public virtual IList<BaseScore> GetVirtualScores()
        {
            return null;
        }
        public virtual string GetSource()
        {
            return String.Empty;
        }
        public virtual IList<string> GetStyles()
        {
            return new List<string>();
        }

        /// <summary>
        /// Merge this score with newScore using given options.
        /// </summary>
        /// <param name="newScore">The score to merge with.</param>
        /// <param name="type">Merge options.</param>
        /// <returns>True if the score changed.</returns>
        public virtual bool Merge(BaseScore newScore, ImportOptions option)
        {
            bool result = false;
            if ((option & ImportOptions.Names) == ImportOptions.Names)
            {
                result |= (String.Compare(this.Name, newScore.Name, true) != 0);

                this.Name = newScore.Name;
                this.Parent = newScore.Parent;
                this.Image = newScore.Image;
            }

            return result;
        }

        public int CompareTo(BaseScore other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.LocName, other.LocName);
            return diff;
        }
        public int CompareToNoLoc(BaseScore other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.Name, other.Name);
            return diff;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
    public partial class FolderScore
    {
        internal override BaseScore Clone(string id)
        {
            FolderScore copy = new FolderScore();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Name = this.Name;
            copy.Image = this.Image;
            copy.Parent = this.Parent;

            return (BaseScore)copy;
        }
        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\folder";
        }

        public override bool IsFolder()
        {
            return true;
        }
    }
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
        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\rss";
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

        public void SetParseOption(bool caption, bool theader, bool newLine, bool wordWrap, bool reverse)
        {
            ParsingOptions opt = ParsingOptions.None;

            if (caption) opt |= ParsingOptions.Caption;
            if (theader) opt |= ParsingOptions.UseTheader;
            if (newLine) opt |= ParsingOptions.NewLine;
            if (wordWrap) opt |= ParsingOptions.WordWrap;
            if (reverse) opt |= ParsingOptions.Reverse;

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
            copy.Headers = this.Headers;
            copy.Sizes = this.Sizes;
            copy.Skip = this.Skip;
            copy.MaxLines = this.MaxLines;
            copy.Image = this.Image;
            copy.Element = this.Element;
            copy.Encoding = this.Encoding;
            copy.ParseOptions = this.ParseOptions;
            copy.Parent = this.Parent;

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
                    || (this.Skip != newScore.Skip)
                    || (this.MaxLines != newScore.MaxLines)
                    || (String.Compare(this.Encoding, newScore.Encoding, true) != 0)
                    || (String.Compare(this.Element, newScore.Element, true) != 0);

                this.Url = newScore.Url;
                this.XPath = newScore.XPath;
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
    }

    partial class Rule
    {
        public override string ToString()
        {
            return String.Format("{0} {1} => {2}", this.Operator, this.Value, this.Action);
        }
    }

    partial class Style
    {
        public Style Clone()
        {
            Style style = new Style();
            style.ForeColor = this.ForeColor;
            return style;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    partial class ScoreCenterSetup
    {
        /// <summary>
        /// Checks if Update is allowed.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="nb"></param>
        /// <returns></returns>
        public bool DoUpdate(bool force, int nb)
        {
            bool result = false;

            switch (this.UpdateOnlineMode)
            {
                case UpdateMode.Once:
                    result = nb == 0;
                    break;
                case UpdateMode.Never:
                    result = false;
                    break;
                case UpdateMode.Manually:
                    result = force;
                    break;
                case UpdateMode.Always:
                    result = true;
                    break;
            }

            return result;
        }
    }
}
