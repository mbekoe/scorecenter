#region Copyright (C) 2005-2012 Team MediaPortal

/* 
 *      Copyright (C) 2005-2012 Team MediaPortal
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
using System.Linq;
using System.Text;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class LiveCenter : BackgroundWorker
    {
        /// <summary>List of scores to check.</summary>
        private IEnumerable<BaseScore> m_scores;
        /// <summary>The ScoreCenter to use.</summary>
        private ScoreCenter m_center;
        /// <summary>A cache to keep the previous version of each scores.</summary>
        private Dictionary<BaseScore, string[][]> m_cache = new Dictionary<BaseScore, string[][]>();
        /// <summary>The timer.</summary>
        public int SleepTimer { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scores"></param>
        /// <param name="center"></param>
        public LiveCenter(IEnumerable<BaseScore> scores, ScoreCenter center)
        {
            m_scores = scores;
#if DEBUG
            Rule r = new Rule();
            r.Column = 2;
            r.Operator = Operation.EqualTo;
            r.Action = RuleAction.ReplaceText;
            r.Value = "b,zz";

            GenericScore sc = m_scores.First() as GenericScore;
            sc.Rules = new Rule[1];
            sc.Rules[0] = r;
#endif

            m_center = center;
            this.WorkerReportsProgress = false;
            this.WorkerSupportsCancellation = true;
            this.SleepTimer = m_center.Setup.LiveCheckDelay * 60000;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

#if DEBUG
            this.SleepTimer = 15000; // 15s
#endif

            while (!this.CancellationPending)
            {
                foreach (BaseScore score in m_scores)
                {
#if DEBUG
                    string[][] results = { new string[] { DateTime.Now.TimeOfDay.ToString(), "b", "c" } };
#else
                    string[][] results = ScoreFactory.Parse(score, false, m_center.Parameters);
#endif
                    results = EvaluateRules(score, results);
                    if (!m_cache.ContainsKey(score))
                    {
                        m_cache[score] = results;
                    }
                    else
                    {
                        string message = CompareScores(score, m_cache[score], results);
                        if (String.IsNullOrEmpty(message) == false)
                        {
                            Notify(score, message);
                            m_cache[score] = results;
                        }
                    }
                }

                System.Threading.Thread.Sleep(this.SleepTimer);
            }
        }

        /// <summary>
        /// Compares 2 scores.
        /// </summary>
        /// <param name="score">The score definition.</param>
        /// <param name="oldScore">The previous score.</param>
        /// <param name="newScore">The new score.</param>
        /// <returns>A message with the differences between scores.</returns>
        private string CompareScores(BaseScore score, string[][] oldScore, string[][] newScore)
        {
            if (oldScore == null || newScore == null)
                return String.Format("ERROR {0} {1}", oldScore == null, newScore == null);

            string message = "";
            try
            {
                string scoreFormat = "";
                if (!String.IsNullOrEmpty(score.LiveConfig.Value) && score.LiveConfig.Value.Contains("{"))
                {
                    scoreFormat = score.LiveConfig.Value;
                }

                string[] filters = score.LiveConfig.filter.Split(',');

                // for all row of old score
                for (int iRow = 0; iRow < oldScore.Length; iRow++)
                {
                    // ignore missing row in new score
                    if (newScore.Length <= iRow || newScore[iRow] == null)
                        continue;

                    // format the row
                    string newRow = BuildScore(scoreFormat, newScore[iRow]);

                    // if old score is missing then new is new
                    if (oldScore[iRow] == null)
                    {
                        message = AddToMessage(message, newRow, filters);
                        continue;
                    }

                    // format the old row the same way
                    string oldRow = BuildScore(scoreFormat, oldScore[iRow]);

                    // compare old and new
                    if (!String.Equals(oldRow, newRow))
                    {
                        Tools.LogMessage("OLD = {0}", oldRow);
                        Tools.LogMessage("NEW = {0}", newRow);
                        var zz = score.GetDifferences(oldRow, newRow);
                        message = AddToMessage(message, ScoreDifference.StringFromList(zz), filters);
                    }
                }
            }
            catch (Exception exc)
            {
                Tools.LogError("CompareScore", exc);
                message = "ERROR";
            }

            return message;
        }

        private string[][] EvaluateRules(BaseScore score, string[][] labels)
        {
            GenericScore genScore = score as GenericScore;
            if (genScore == null)
                return null;

            RuleEvaluator engine = new RuleEvaluator(genScore.Rules);

            ParsingOptions opt = genScore.GetParseOption();
            bool reverseOrder = GenericScore.CheckParsingOption(opt, ParsingOptions.Reverse);
            bool wordWrap = GenericScore.CheckParsingOption(opt, ParsingOptions.WordWrap);

            int lineNumber = -1;
            // for all the rows
            IList<string[]> controls = new List<string[]>();
            int totalLines = labels.Count(p => p != null && p[0] != ScoreCenterPlugin.C_HEADER);
            foreach (string[] row_ in labels)
            {
                // ignore empty lines
                if (row_ == null || row_.Length == 0)
                    continue;

                lineNumber++;

                bool isHeader = (row_[0] == ScoreCenterPlugin.C_HEADER);
                string[] row = isHeader ? row_.Where(c => c != ScoreCenterPlugin.C_HEADER).ToArray() : row_;

                // Evaluate rule for full line
                if (!isHeader)
                {
                    Rule rule = engine.CheckLine(row, lineNumber, totalLines);
                    if (rule != null)
                    {
                        // skip lines and continue
                        if (rule.Action == RuleAction.SkipLine)
                            continue;
                    }
                }

                #region For all columns
                for (int index = 0; index < row.Length; index++)
                {
                    int colIndex = reverseOrder ? row.Length - index - 1 : index;

                    // get cell
                    string cell = row[colIndex];

                    #region Evaluate rule for the cell
                    if (!isHeader)
                    {
                        Rule cellRule = engine.CheckCell(cell, colIndex);
                        if (cellRule != null)
                        {
                            if (cellRule.Action == RuleAction.ReplaceText)
                            {
                                string str1 = cellRule.Value;
                                string str2 = String.Empty;
                                if (cellRule.Value.Contains(","))
                                {
                                    string[] elts = cellRule.Value.Split(',');
                                    str1 = elts[0];
                                    str2 = elts[1];
                                }

                                cell = cell.Replace(str1, str2);
                            }
                        }
                    }
                    #endregion

                    row[colIndex] = cell;
                }
                #endregion

                // create the control
                controls.Add(row);
            }

            return controls.ToArray();
        }

        private static string AddToMessage(string message, string newRow, string[] filters)
        {
            // check if filtered
            bool add = true;
            if (filters != null)
            {
                add = false;
                string lowrow = newRow.ToLower();
                foreach (string f in filters)
                {
                    if (lowrow.Contains(f))
                    {
                        add = true;
                        break;
                    }
                }
            }

            if (add)
            {
                if (message.Length > 0) message += Environment.NewLine;
                message += newRow;
            }
            
            return message;
        }

        private static string BuildScore(string format, string[] row)
        {
            if (String.IsNullOrEmpty(format))
                return String.Join(" ", row);

            try
            {
                return String.Format(format, row);
            }
            catch (FormatException exc)
            {
                Tools.LogError("Format failed", exc);
                return String.Join(" ", row);
            }
        }

        private void Notify(BaseScore score, string message)
        {
            //Tools.LogMessage("\n>> Notify: {0}", message);
            string title = "";
            string icon = "";
            if (score != null)
            {
                title = score.LocName;
                icon = score.Image;
            }

            LiveScoreNotifyDialog dlg = (LiveScoreNotifyDialog)GUIWindowManager.GetWindow(LiveScoreNotifyDialog.SCORE_NOTIFY_DIALOG_ID);
            dlg.PlaySound = m_center.Setup.LivePlaySound;
            dlg.SetScore(title, icon, message);
            dlg.TimeOut = m_center.Setup.LiveNotifTime;
            dlg.DoModal(GUIWindowManager.ActiveWindow);
            //Tools.LogMessage("<< Notify\n");
        }
    }
}
