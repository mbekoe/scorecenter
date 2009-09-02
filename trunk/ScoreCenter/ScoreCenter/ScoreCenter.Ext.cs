using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
        All = New | Names | Parsing | Rules
    }

    public static class EnumManager
    {
        public static DataTable ReadRuleAction()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(RuleAction.FormatCell.ToString(), "Format Cell");
            dt.Rows.Add(RuleAction.FormatLine.ToString(), "Format Line");
            dt.Rows.Add(RuleAction.MergeCells.ToString(), "Merge Cells");
            dt.Rows.Add(RuleAction.ReplaceText.ToString(), "Replace Text");

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

        public IEnumerable<string> ReadCategories()
        {
            return this.Scores.Where(score => score.enable)
                .Select(score => score.Category).Distinct();
        }

        public IEnumerable<string> ReadLeagues(string category)
        {
            return this.Scores.Where(score => score.enable && score.Category == category)
                .Select(score => score.Ligue).Distinct();
        }

        public Score FindScore(string id)
        {
            if (this.Scores == null)
                return null;

            return this.Scores.FirstOrDefault(score => score.Id == id);
        }

        public Style FindStyle(string name)
        {
            if (this.Styles == null || String.IsNullOrEmpty(name))
                return null;

            return this.Styles.FirstOrDefault(style => style.Name == name);
        }

        public bool IsCategoryUpdated(string category)
        {
            bool res = this.Scores.Count(score => score.IsNew() && score.Category == category) > 0;
            //Tools.LogMessage("IsCategoryUpdated {0} {1}", category, res);
            return res;
        }

        public bool IsLeagueUpdated(string category, string league)
        {
            return this.Scores.Count(score => score.IsNew() && score.Category == category && score.Ligue == league) > 0;
        }

        public CategoryImg FindCategoryImage(string category)
        {
            if (category == " " || this.Images == null || this.Images.CategoryImg == null)
                return null;

            return this.Images.CategoryImg.FirstOrDefault(img => img.Name == category);
        }

        public LeagueImg FindLeagueImage(string category, string league)
        {
            if (league == " " || this.Images == null || this.Images.LeagueImg == null)
                return null;

            return this.Images.LeagueImg.FirstOrDefault(img => img.Category == category && img.Name == league);
        }
    }

    partial class Score
    {
        [NonSerialized]
        private bool m_new = false;
        
        public bool IsNew()
        {
            return m_new;
        }
        public void SetNew()
        {
            m_new = true;
        }

        public bool Merge(Score newScore, ImportOptions type)
        {
            bool result = false;
            if ((type & ImportOptions.Names) == ImportOptions.Names)
            {
                result |= (String.Compare(this.Name, newScore.Name, true) != 0)
                    || (String.Compare(this.Category, newScore.Category, true) != 0)
                    || (String.Compare(this.Ligue, newScore.Ligue, true) != 0);

                this.Name = newScore.Name;
                this.Category = newScore.Category;
                this.Ligue = newScore.Ligue;
                this.Image = newScore.Image;

                if (newScore.Headers.Length > 0 || this.Headers.Length == 0)
                {
                    this.Headers = newScore.Headers;
                    result = true;
                }
            }

            if ((type & ImportOptions.Parsing) == ImportOptions.Parsing)
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
            }

            if ((type & ImportOptions.Rules) == ImportOptions.Rules)
            {
                if (newScore.Rules != null && newScore.Rules.Length > 0)
                {
                    this.Rules = newScore.Rules;
                    result = true;
                }
            }

            return result;
        }

        public string LeagueFullName
        {
            get
            {
                return String.Format("{0}#{1}", Category, Ligue);
            }
        }

        public string ScorePath
        {
            get
            {
                return String.Format(@"{0}\{1}\{2}", Category, Ligue, Name);
            }
        }

        public int CompareTo(Score other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.Name, other.Name);
            return diff;
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
        [NonSerialized]
        private int m_updateCount = 0;

        public bool DoUpdate(bool force)
        {
            bool result = false;

            switch (this.UpdateOnlineMode)
            {
                case UpdateMode.Once:
                    result = m_updateCount == 0;
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

            m_updateCount++;
            return result;
        }
    }
}
