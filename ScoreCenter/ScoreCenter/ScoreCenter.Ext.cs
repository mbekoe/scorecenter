using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Type of merge.
    /// </summary>
    [Flags]
    public enum MergeType
    {
        None = 0,
        Names = 1,
        Parsing = 2,
        Rules = 4,
        All = Names | Parsing | Rules
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

        public Score FindScore(string id)
        {
            if (this.Scores == null)
                return null;
            
            foreach (Score score in this.Scores)
            {
                if (String.Equals(id, score.Id, StringComparison.OrdinalIgnoreCase))
                {
                    return score; 
                }
            }
            
            return null;
        }

        public Style FindStyle(string name)
        {
            if (this.Styles == null || String.IsNullOrEmpty(name))
                return null;
            
            foreach (Style style in this.Styles)
            {
                if (String.Equals(name, style.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return style;
                }
            }

            return null;
        }
    }

    partial class Score
    {
        public void Merge(Score newScore, MergeType type)
        {
            if ((type & MergeType.Names) == MergeType.Names)
            {
                this.Name = newScore.Name;
                this.Category = newScore.Category;
                this.Ligue = newScore.Ligue;
                this.Image = newScore.Image;

                if (newScore.Headers.Length > 0 || this.Headers.Length == 0)
                    this.Headers = newScore.Headers;
            }

            if ((type & MergeType.Names) == MergeType.Parsing)
            {
                this.Url = newScore.Url;
                this.XPath = newScore.XPath;
                this.Skip = newScore.Skip;
                this.MaxLines = newScore.MaxLines;
                this.Sizes = newScore.Sizes;
            }

            if ((type & MergeType.Names) == MergeType.Rules)
            {
                if (newScore.Rules != null)
                {
                    this.Rules = newScore.Rules;
                }
            }
        }

        public string ScorePath
        {
            get
            {
                return String.Format(@"{0}\{1}\{2}", Category, Ligue, Name);
            }
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
}
