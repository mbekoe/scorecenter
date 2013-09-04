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
        Reverse = 16,
        ImgAlt = 32
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
            dt.Rows.Add(Operation.IsNotNull, "Is Not Null");

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
        public void ReplaceVirtualScores()
        {
            List<BaseScore> vslist = new List<BaseScore>();
            foreach (BaseScore sc in this.Scores.Items.Where(p => p.IsVirtualFolder()))
            {
                IList<BaseScore> children = sc.GetVirtualScores(this.Parameters);
                if (children != null)
                {
                    vslist.AddRange(children);
                }
            }

            this.Scores.Items = this.Scores.Items.Concat(vslist.ToArray()).ToArray();
        }

        public void AddScore(BaseScore score)
        {
            this.Scores.Items = Tools.AddElement<BaseScore>(this.Scores.Items, score);
        }

        public void RemoveScore(BaseScore score)
        {
            this.Scores.Items = Tools.RemoveElement<BaseScore>(this.Scores.Items, score);
        }

        public IEnumerable<BaseScore> ReadChildren(BaseScore parent)
        {
            if (parent != null && parent.IsVirtualFolder() && !parent.IsVirtualResolved())
            {
                IList<BaseScore> scores = parent.GetVirtualScores(this.Parameters);
                parent.SetVirtualResolved();
                this.Scores.Items = this.Scores.Items.Concat(scores).ToArray();
                return scores;
            }
            else
            {
                string id = parent == null ? "" : parent.Id;
                return this.Scores.Items.Where(score => score.enable && score.Parent == id);
            }
        }

        public void DisableScore(BaseScore score)
        {
            if (score == null)
                return;

            score.enable = false;
            foreach (BaseScore sc in this.Scores.Items.Where(s => s.enable && s.Parent == score.Id))
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
                full = GetFullName(FindScore(score.Parent), sep) + sep + full;
            }

            return full;
        }

        /// <summary>
        /// Get the score level.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <returns>The score level.</returns>
        public int GetLevel(BaseScore score)
        {
            int level = 0;
            if (score != null)
            {
                level = 1 + GetLevel(FindScore(score.Parent));
            }

            return level;
        }

        public BaseScore FindScore(string id)
        {
            if (this.Scores.Items == null || String.IsNullOrEmpty(id))
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

            if (this.Scores.Items != null)
            {
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
            }
            
            return plist;
        }

        public string GetParameterValue(string parameterName, string defaultValue)
        {
            string result = defaultValue;

            if (this.Parameters != null && this.Parameters.Length > 0)
            {
                ScoreParameter p = this.Parameters.FirstOrDefault(pp => pp.name == parameterName);
                if (p != null && !String.IsNullOrEmpty(p.Value))
                    result = p.Value;
            }

            return result;
        }

        public void SetLiveScore(BaseScore score, bool enable)
        {
            if (score.IsVirtual())
            {
                BaseScore sc = FindScore(score.Parent);
                if (sc != null) sc.SetLive(enable);
            }

            score.SetLive(enable);
        }

        /// <summary>
        /// Get the Home Score.
        /// </summary>
        /// <returns>The Home score or Null if not set or found.</returns>
        public BaseScore GetHomeScore()
        {
            if (this.Setup.Home == null)
                return null;

            if (!String.IsNullOrEmpty(this.Setup.Home.parent))
            {
                // home score is a virtual score, we need to get its parent first
                BaseScore parent = FindScore(this.Setup.Home.parent);
                ReadChildren(parent);
            }

            return FindScore(this.Setup.Home.Value);
        }

        /// <summary>
        /// Set the Home Score.
        /// </summary>
        /// <param name="score"></param>
        public void SetHomeScore(BaseScore score)
        {
            if (score == null)
            {
                this.Setup.Home = null;
            }
            else
            {
                if (this.Setup.Home == null)
                    this.Setup.Home = new HomeScore();
                this.Setup.Home.Value = score.Id;
                if (score.IsVirtual()) this.Setup.Home.parent = score.Parent;
            }
        }
    }

    partial class Rule
    {
        public override string ToString()
        {
            return String.Format("{0} {1} => {2}", this.Operator, this.Value, this.Action);
        }

        public Rule Clone()
        {
            Rule r = new Rule();
            r.columnField = this.columnField;
            r.operatorField = this.operatorField;
            r.valueField = this.valueField;
            r.actionField = this.actionField;
            r.formatField = this.formatField;
            return r;
        }
    }

    partial class Style
    {
        public static Style CreateFromColor(long color)
        {
            Style st = new Style();
            st.ForeColor = color;
            return st;
        }

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

    partial class LiveConfig
    {
        public bool IsNull()
        {
            return !this.enabled && String.IsNullOrEmpty(this.Value) && String.IsNullOrEmpty(this.filter);
        }

        public static LiveConfig Copy(LiveConfig source, string format)
        {
            if (source == null)
                return null;

            LiveConfig copy = new LiveConfig();
            copy.enabled = source.enabled;
            copy.Value = String.IsNullOrEmpty(format) ? source.Value : format;
            copy.filter = source.filter;
            return copy;
        }
    }
}
