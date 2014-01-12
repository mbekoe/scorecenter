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
using System.Text;
using System.Text.RegularExpressions;
using MediaPortal.ServiceImplementations;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class RuleEvaluator
    {
        /// <summary>The list of rules.</summary>
        private Rule[] m_rules;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rules">The list of rules.</param>
        public RuleEvaluator(Rule[] rules)
        {
            m_rules = rules;
        }

        /// <summary>
        /// Check the rules on a cell.
        /// </summary>
        /// <param name="text">The text of the cell.</param>
        /// <param name="colIndex">The index of the cell.</param>
        /// <returns>The first rule satisifed by the cell or null.</returns>
        public Rule CheckCell(string text, int colIndex)
        {
            if (m_rules == null || m_rules.Length == 0)
                return null;

            foreach (Rule rule in m_rules)
            {
                if (rule.Action != RuleAction.FormatCell
                    && rule.Action != RuleAction.ReplaceText)
                    continue;
                
                if (rule.Column == 0 && Evaluate(rule, text))
                    return rule;

                if ((rule.Column == colIndex + 1) && Evaluate(rule, text))
                    return rule;
            }

            return null;
        }

        /// <summary>
        /// Check the rules on a line.
        /// </summary>
        /// <param name="text">The text of the cells of the line.</param>
        /// <param name="line">The index of the line.</param>
        /// <param name="nbLines">The total number of lines.</param>
        /// <returns>The first rule satisfied by the line or null.</returns>
        public Rule CheckLine(string[] text, int line, int nbLines)
        {
            if (m_rules == null || m_rules.Length == 0)
                return null;

            foreach (Rule rule in m_rules)
            {
                // only check rules applying to line
                if (rule.Action != RuleAction.FormatLine
                    && rule.Action != RuleAction.SkipLine
                    && rule.Action != RuleAction.MergeCells)
                    continue;

                if (rule.Column == 0)
                {
                    // rule applies to any cell in the line
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (Evaluate(rule, text[i]))
                            return rule;
                    }
                }
                else if (rule.Column == -1)
                {
                    // rule applies to the line number
                    if (Evaluate(rule, line.ToString(), line, nbLines))
                        return rule;
                }
                else if (text.Length >= rule.Column)
                {
                    // rule applies to a specific column
                    if (Evaluate(rule, text[rule.Column - 1]))
                        return rule;
                }
                else if (rule.Operator == Operation.IsNull)
                {
                    return rule;
                }
            }

            return null;
        }

        /// <summary>
        /// Evaluate a rule.
        /// </summary>
        /// <param name="rule">The rule to evaluate.</param>
        /// <param name="text">The text to check.</param>
        /// <param name="args">Arguments for the rule.</param>
        /// <returns>True if the text satisfied the rule, False otherwise.</returns>
        private bool Evaluate(Rule rule, string text, params int[] args)
        {
            bool result = false;

            string newText = text.Trim().ToUpper();
            string newValue = rule.Value;
            if (rule.Action == RuleAction.ReplaceText) newValue = rule.Value.Split(',')[0];
            newValue = newValue.ToUpper();

            switch (rule.Operator)
            {
                case Operation.EqualTo:
                    result = String.Compare(newText, newValue) == 0;
                    break;
                case Operation.NotEqualTo:
                    result = String.Compare(newText, newValue) != 0;
                    break;
                case Operation.GT:
                    result = CompareStr(newText, newValue) > 0;
                    break;
                case Operation.LT:
                    result = CompareStr(newText, newValue) < 0;
                    break;
                case Operation.GE:
                    result = CompareStr(newText, newValue) >= 0;
                    break;
                case Operation.LE:
                    result = CompareStr(newText, newValue) <= 0;
                    break;
                case Operation.Contains:
                    result = newText.Contains(newValue);
                    break;
                case Operation.NotContains:
                    result = !newText.Contains(newValue);
                    break;
                case Operation.StartsWith:
                    result = newText.StartsWith(newValue);
                    break;
                case Operation.NotStartsWith:
                    result = !newText.StartsWith(newValue);
                    break;
                case Operation.EndsWith:
                    result = newText.EndsWith(newValue);
                    break;
                case Operation.NotEndsWith:
                    result = !newText.EndsWith(newValue);
                    break;
                case Operation.MOD:
                    int vi, vv;
                    if (int.TryParse(newText, out vi) && int.TryParse(newValue, out vv))
                    {
                        result = (vi % vv == 0);
                    }
                    break;
                case Operation.InList:
                    string[] list = newValue.Split(',');
                    foreach (string str in list)
                    {
                        if (str == newText)
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case Operation.IsNull:
                    result = String.IsNullOrEmpty(newText);
                    break;
                case Operation.IsNotNull:
                    result = !String.IsNullOrEmpty(newText);
                    break;
                case Operation.IsLast:
                    if (args != null && args.Length > 1)
                    {
                        if (newValue.Length == 0)
                            result = (args[0] == args[1]);
                        else
                            result = CompareStr((args[1] - args[0]).ToString(), newValue) < 0;
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Compares two strings.
        /// </summary>
        /// <param name="x">The first string.</param>
        /// <param name="y">The second string.</param>
        /// <returns>0 if the strings are equal, 1 if x > y and -1 otherwise.</returns>
        private static int CompareStr(string x, string y)
        {
            if (String.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0)
                return 0;

            string[] x1, y1;
            x1 = Regex.Split(x.ToUpper().Replace(" ", ""), "([0-9]+)");
            y1 = Regex.Split(y.ToUpper().Replace(" ", ""), "([0-9]+)");

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    return PartCompare(x1[i], y1[i]);
                }
            }

            if (y1.Length > x1.Length)
                return 1;
            
            if (x1.Length > y1.Length)
                return -1;

            return 0;
        }

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
            {
                return left.CompareTo(right);
            }

            if (!int.TryParse(right, out y))
            {
                return left.CompareTo(right);
            }

            return x.CompareTo(y);
        }
    }
}
