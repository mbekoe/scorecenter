using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    public class WorldFootballScoreParser : ScoreParser<WorldFootballScore>
    {
        private const string GLOBAL_XPATH = "//table[@class='standard_tabelle']";
        private const string EMBLEM_PATH = "//div[@class='emblem']//img";
        private const string IMG_RESULTS = "Results";
        private const string IMG_NEXT = "Next";
        private const string IMG_STANDINGS = "Standings";
        private const string IMG_HISTORY = "Trophy";
        private const string IMG_TOP_SCORER = @"Football\Top Scorers";
        private const string WF_URL = "{@worldfootball}";

        public WorldFootballScoreParser(int lifetime)
            : base(lifetime)
        {
        }

        public static IList<BaseScore> GetRealScores(WorldFootballScore score)
        {
            if (score.Kind == WorldFootballKind.League)
                return AddLeague(score);

            if (score.Kind == WorldFootballKind.Cup)
                return AddCup(score);

            if (score.Kind == WorldFootballKind.Tournament)
                return AddTournament(score);
            
            return null;
        }

        public static string GetEmblemUrl(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(html);

            // parse it
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(EMBLEM_PATH);
            return nodes[0].GetAttributeValue("src", "");
        }

        protected override string[][] Parse(WorldFootballScore score, bool reload, ScoreParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        private static GenericScore CreateNewScore(string parent, string id, string name, int index)
        {
            GenericScore sc = new GenericScore();
            sc.Order = index;
            sc.enable = true;
            sc.Name = name;
            sc.Id = String.Format("{0}-{1}", parent, id);
            sc.Parent = parent;

            return sc;
        }

        private static void AddRule(GenericScore score, int col, Operation ruleOperator, string ruleValue, RuleAction action, string ruleFormat)
        {
            MediaPortal.Plugin.ScoreCenter.Rule rule = new MediaPortal.Plugin.ScoreCenter.Rule();
            rule.Column = col;
            rule.Operator = ruleOperator;
            rule.Value = ruleValue;
            rule.Action = action;
            rule.Format = ruleFormat;

            List<MediaPortal.Plugin.ScoreCenter.Rule> rules = new List<MediaPortal.Plugin.ScoreCenter.Rule>();
            if (score.Rules != null)
                rules.AddRange(score.Rules.AsEnumerable());
            rules.Add(rule);
            score.Rules = rules.ToArray();
        }
        private static void AddHighlightRule(GenericScore score, string highlights, int col, RuleAction action)
        {
            if (String.IsNullOrEmpty(highlights))
                return;

            List<MediaPortal.Plugin.ScoreCenter.Rule> rules = new List<MediaPortal.Plugin.ScoreCenter.Rule>();
            if (score.Rules != null)
                rules.AddRange(score.Rules.AsEnumerable());

            string[] hh = highlights.Split(',');
            foreach (string h in hh)
            {
                MediaPortal.Plugin.ScoreCenter.Rule rule = new MediaPortal.Plugin.ScoreCenter.Rule();
                rule.Column = col;
                rule.Operator = Operation.Contains;
                rule.Value = h;
                rule.Action = action;
                rule.Format = "WFHighlight";
                rules.Add(rule);
                //Tools.LogMessage("Rule = {0}", rule);
            }

            score.Rules = rules.ToArray();
        }

        private static List<BaseScore> AddLeague(WorldFootballScore wfscore)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "3";
            sc.Sizes = "11,5,20,+1,-20,4";
            sc.Image = IMG_RESULTS;
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "next", "Next Round", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "2";
            sc.Sizes = "11,5,20,+1,-20,4";
            sc.Image = IMG_NEXT;
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "table", "Standings", index++);
            int nbrounds = wfscore.NbTeams * 2 - 2;
            sc.Url = String.Format("{0}spielplan/{1}-{2}-spieltag/{3}/tabelle/", WF_URL, wfscore.FullLeagueName, wfscore.Season, nbrounds);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "0";
            sc.Skip = 2;
            sc.Sizes = "5,0,-20,3,3,3,3,5,3,3";
            sc.Headers = ",,,GP,V,T,L,Goals,+/-,Pts";
            sc.Image = IMG_STANDINGS;
            if (String.IsNullOrEmpty(wfscore.Levels) == false)
            {
                int i = 0;
                int j = 0;
                foreach (string level in wfscore.Levels.Split(','))
                {
                    if (level.StartsWith("-"))
                    {
                        AddRule(sc, -1, Operation.IsLast, level.Substring(1), RuleAction.FormatLine, String.Format("Level{0}", --j));
                    }
                    else
                    {
                        AddRule(sc, -1, Operation.LE, level, RuleAction.FormatLine, String.Format("Level{0}", ++i));
                    }
                }
            }
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "topscorers", "Top Scorers", index++);
            sc.Url = String.Format("{0}torjaeger/{1}-{2}/", WF_URL, wfscore.FullLeagueName, wfscore.Season);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "0";
            sc.Sizes = "6,0,-22,-20,-12";
            sc.Image = IMG_TOP_SCORER;
            AddRule(sc, -1, Operation.EqualTo, "1", RuleAction.FormatLine, "Leader");
            AddHighlightRule(sc, wfscore.Highlights, 4, RuleAction.FormatLine);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "archives", "History", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "0";
            sc.Skip = 1;
            sc.Sizes = "-10,0,-24";
            sc.Image = IMG_HISTORY;
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        private static List<BaseScore> AddCup(WorldFootballScore wfscore)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "2";
            sc.Sizes = "11,5,20,+1,-20,8";
            sc.Image = IMG_RESULTS;
            AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "archives", "History", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.XPath = GLOBAL_XPATH;
            sc.Element = "0";
            sc.Skip = 1;
            sc.Sizes = "-10,0,-24";
            sc.Image = IMG_HISTORY;
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }
        
        private static List<BaseScore> AddTournament(WorldFootballScore wfscore)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            string[] levels= wfscore.Levels.Split(',');
            char[] groups = levels[0].ToCharArray();
            foreach (char g in groups)
            {
                GenericScore sc = CreateNewScore(wfscore.Id, "gr" + g, String.Format("Group {0}", Char.ToUpper(g)), index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, wfscore.FullLeagueName, g);
                sc.XPath = GLOBAL_XPATH;
                sc.Element = "0";
                sc.Sizes = "-12,15,+1,-15,-8,0";
                sc.Image = String.Format(@"Groups\Group{0}", Char.ToUpper(g));
                AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
                scores.Add(sc);

                sc = CreateNewScore(wfscore.Id, "resgr" + g, "Standings", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, wfscore.FullLeagueName, g);
                sc.XPath = GLOBAL_XPATH;
                sc.Element = "1";
                sc.Skip = 2;
                sc.Headers = ",,Team,Pl,W,D,L,goals,+/-,Pts";
                sc.Sizes = "-6,0,-15,3,3,3,3,6,3,3";
                sc.Image = IMG_STANDINGS;
                AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
                scores.Add(sc);
            }

            foreach (string level in levels)
            {
                if (level == levels[0]) continue;
                GenericScore sc = CreateNewScore(wfscore.Id, level, level, index++);
                sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, wfscore.FullLeagueName, level);
                sc.XPath = GLOBAL_XPATH;
                sc.Element = "0";
                sc.Sizes = "-5,18,-1,-18,-8,0";
                sc.Image = IMG_RESULTS;
                AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            return scores;
        }
    }
}
