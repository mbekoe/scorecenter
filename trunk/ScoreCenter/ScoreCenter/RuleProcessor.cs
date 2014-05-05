using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public static class RuleProcessor
    {
        public static string Process(string cell, Rule rule)
        {
            if (rule == null)
                return cell;

            string res = cell;
            switch (rule.Action)
            {
                case RuleAction.ReplaceText:
                    res = RuleProcessor.ReplaceText(cell, rule);
                    break;
                case RuleAction.CutBefore:
                    res = RuleProcessor.CutBeforeText(cell, rule);
                    break;
                case RuleAction.CutAfter:
                    res = RuleProcessor.CutAfterText(cell, rule);
                    break;
            }

            return res;
        }

        private static string ReplaceText(string cell, Rule rule)
        {
            string str1 = rule.Value;
            string str2 = String.Empty;
            if (rule.Value.Contains(","))
            {
                string[] elts = rule.Value.Split(',');
                str1 = elts[0];
                str2 = elts[1];
            }

            cell = cell.Replace(str1, str2);
            return cell;
        }

        private static string CutBeforeText(string cell, Rule rule)
        {
            return cell.Split(new string[] { rule.Value }, StringSplitOptions.None)[0].Trim();
        }
        
        private static string CutAfterText(string cell, Rule rule)
        {
            var elts = cell.Split(new string[] { rule.Value }, StringSplitOptions.None);
            if (elts.Length > 1)
                return elts[1].Trim();
            return cell;
        }
    }
}
