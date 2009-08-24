using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MediaPortal.ServiceImplementations;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class RuleEvaluator
    {
        private Rule[] m_rules;

        public RuleEvaluator(Rule[] rules)
        {
            m_rules = rules;
        }

        public Rule CheckCell(string text, int colIndex)
        {
            if (m_rules == null || m_rules.Length == 0)
                return null;

            foreach (Rule rule in m_rules)
            {
                if (rule.Action != RuleAction.FormatCell)
                    continue;
                
                if (rule.Column == 0 && Evaluate(rule, text))
                    return rule;

                if ((rule.Column == colIndex + 1) && Evaluate(rule, text))
                    return rule;
            }

            return null;
        }

        public Rule CheckLine(string[] text, int line)
        {
            if (m_rules == null || m_rules.Length == 0)
                return null;

            foreach (Rule rule in m_rules)
            {
                // only check rules applying to line
                if (rule.Action != RuleAction.FormatLine
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
                    if (Evaluate(rule, line.ToString()))
                        return rule;
                }
                else if (text.Length >= rule.Column)
                {
                    // rule applies to a specific column
                    if (Evaluate(rule, text[rule.Column - 1]))
                        return rule;
                }
            }

            return null;
        }

        private bool Evaluate(Rule rule, string text)
        {
            bool result = false;

            string newText = text.Trim().ToUpper();
            string newValue = rule.Value.Trim().ToUpper();

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
            }

            return result;
        }

        public int CompareStr(string x, string y)
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
            {
                return 1;
            }
            else if (x1.Length > y1.Length)
            {
                return -1;
            }
            else
            {
                return 0;
            }
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
