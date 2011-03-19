using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel;

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

    public static class EnumManager
    {
        public static DataTable ReadNodeTypes()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(Node.Folder.ToString(), "Folder");
            dt.Rows.Add(Node.RSS.ToString(), "RSS");
            dt.Rows.Add(Node.Score.ToString(), "Score");

            return dt;
        }

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

    partial class ScoreCenter
    {
        public void AddScore(Score score)
        {
            this.Scores = Tools.AddElement<Score>(this.Scores, score);
        }

        public void RemoveScore(Score score)
        {
            this.Scores = Tools.RemoveElement<Score>(this.Scores, score);
        }

        public IEnumerable<Score> ReadChildren(string id)
        {
            return this.Scores.Where(score => score.enable && score.Parent == id);
        }

        public void DisableScore(Score score)
        {
            if (score == null)
                return;

            score.enable = false;
            foreach (Score sc in this.ReadChildren(score.Id))
            {
                DisableScore(sc);
            }
        }
        public string GetFullName(Score score, string sep)
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

        public Score FindScore(string id)
        {
            if (this.Scores == null || String.IsNullOrEmpty(id))
                return null;

            return this.Scores.FirstOrDefault(score => score.Id == id);
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
    }

    partial class Score
    {
        /// <summary>
        /// Flag to identify new downloaded score.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore(), DefaultValue(false)]
        public bool IsNew { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        public string LocName
        {
            get
            {
                return LocalizationManager.GetScoreString(this.Id, this.Name);
            }
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

        /// <summary>
        /// Merge this score with newScore using given options.
        /// </summary>
        /// <param name="newScore">The score to merge with.</param>
        /// <param name="type">Merge options.</param>
        /// <returns>True if the score changed.</returns>
        public bool Merge(Score newScore, ImportOptions option)
        {
            bool result = false;
            if ((option & ImportOptions.Names) == ImportOptions.Names)
            {
                result |= (String.Compare(this.Name, newScore.Name, true) != 0);

                this.Name = newScore.Name;
                this.Parent = newScore.Parent;
                this.Image = newScore.Image;

                if (newScore.Headers.Length > 0 || this.Headers.Length == 0)
                {
                    this.Headers = newScore.Headers;
                    result = true;
                }
            }

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

        public int CompareTo(Score other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.LocName, other.LocName);
            return diff;
        }
        public int CompareToNoLoc(Score other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.Name, other.Name);
            return diff;
        }

        internal Score Clone(string id)
        {
            Score copy = new Score();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Type = this.Type;
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

            return copy;
        }

        public override string ToString()
        {
            return String.Format("{0} [{1}]", this.Name, this.Id);
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
