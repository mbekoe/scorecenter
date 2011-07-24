using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    /// <summary>
    /// Parser for Worldfootball site.
    /// </summary>
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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lifetime"></param>
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

            if (score.Kind == WorldFootballKind.Tournament || score.Kind == WorldFootballKind.Qualification)
                return AddTournament(score);
            
            return null;
        }

        /// <summary>
        /// Get the icon for worldfootball.
        /// </summary>
        /// <param name="html">The HTML of the home page of the score.</param>
        /// <returns>The URL of the icon.</returns>
        public static string GetEmblemUrl(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(html);

            // parse it
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(EMBLEM_PATH);
            if (nodes == null)
                return "";
            return nodes[0].GetAttributeValue("src", "");
        }

        protected override string[][] Parse(WorldFootballScore score, bool reload, ScoreParameter[] parameters)
        {
            return null;
        }

        #region Utils
        /// <summary>
        /// Create a new score for the WorldFootball score.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <param name="element"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static GenericScore CreateNewScore(string parent, string id, string name,
            string image, string element, int index)
        {
            GenericScore sc = new GenericScore();
            sc.Order = index;
            sc.enable = true;
            sc.Name = name;
            sc.Id = String.Format("{0}-{1}", parent, id);
            sc.Parent = parent;
            sc.XPath = GLOBAL_XPATH;
            sc.Image = image;
            sc.Element = element;
            sc.IsVirtual = true;

            return sc;
        }

        /// <summary>
        /// Adds a rule to a score.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="col"></param>
        /// <param name="ruleOperator"></param>
        /// <param name="ruleValue"></param>
        /// <param name="action"></param>
        /// <param name="ruleFormat"></param>
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
        
        /// <summary>
        /// Adds a highlight rule to a score.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="highlights"></param>
        /// <param name="col"></param>
        /// <param name="action"></param>
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
                rule.Format = "Highlight";
                rules.Add(rule);
                //Tools.LogMessage("Rule = {0}", rule);
            }

            score.Rules = rules.ToArray();
        }
        #endregion
        
        /// <summary>
        /// Defines a League.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <returns></returns>
        private static List<BaseScore> AddLeague(WorldFootballScore wfscore)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            // Last Results
            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", IMG_RESULTS, "3", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = "11,5,20,+1,-20,4";
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            // Next Round
            sc = CreateNewScore(wfscore.Id, "next", "Next Round", IMG_NEXT, "2", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = "11,5,20,+1,-20,4";
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            // Standings
            string element = "0";
            string nbLeague = wfscore.Details.Split(',').FirstOrDefault(x => x.Contains(";"));
            if (!String.IsNullOrEmpty(nbLeague))
            {
                element = nbLeague;
            }

            sc = CreateNewScore(wfscore.Id, "table", "Standings", IMG_STANDINGS, element, index++);
            sc.BetweenElts = BetweenElements.RepeatHeader;
            int nbrounds = wfscore.NbTeams * 2 - 2;
            sc.Url = String.Format("{0}spielplan/{1}-{2}-spieltag/{3}/tabelle/", WF_URL, wfscore.FullLeagueName, wfscore.Season, nbrounds);
            sc.Skip = 2;
            sc.Sizes = "5,0,-20,3,3,3,3,5,3,3";
            sc.Headers = ",,,Pl,W,D,L,Goals,+/-,Pts";
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

            // Top Scorers
            sc = CreateNewScore(wfscore.Id, "topscorers", "Top Scorers", IMG_TOP_SCORER, "0", index++);
            sc.Url = String.Format("{0}torjaeger/{1}-{2}/", WF_URL, wfscore.FullLeagueName, wfscore.Season);
            sc.Sizes = "6,0,-22,-20,-12";
            AddRule(sc, -1, Operation.EqualTo, "1", RuleAction.FormatLine, "Level1");
            AddHighlightRule(sc, wfscore.Highlights, 4, RuleAction.FormatLine);
            scores.Add(sc);

            // History
            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "0", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = "-10,0,-24";
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        /// <summary>
        /// Defines a Cup.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <returns></returns>
        private static List<BaseScore> AddCup(WorldFootballScore wfscore)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", IMG_RESULTS, "2", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = "11,5,20,+1,-20,8";
            AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            string fullname = wfscore.FullLeagueName;
            if (String.IsNullOrEmpty(wfscore.Season) == false)
                fullname += "-" + wfscore.Season;

            string[] details = wfscore.Details.Split(',');
            foreach (string level in details)
            {
                sc = CreateNewScore(wfscore.Id, level, level, IMG_RESULTS, "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, fullname, level);
                sc.Sizes = "-5,17,+1,-17,-15,0";
                AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "0", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = "-10,0,-24";
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        /// <summary>
        /// Defines a Tournament.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <returns></returns>
        private static List<BaseScore> AddTournament(WorldFootballScore wfscore)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;

            string fullname = wfscore.FullLeagueName;
            if (String.IsNullOrEmpty(wfscore.Season) == false)
                fullname += "-" + wfscore.Season;

            string[] details = wfscore.Details.Split(',');
            char[] groups = details[0].ToCharArray();
            foreach (char g in groups)
            {
                sc = CreateNewScore(wfscore.Id, "gr" + g, String.Format("Group {0}", Char.ToUpper(g)), "", "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, fullname, g);
                sc.Sizes = "-12,15,+1,-15,-8,0";
                sc.Image = String.Format(@"Groups\Group{0}", Char.ToUpper(g));
                AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
                scores.Add(sc);

                sc = CreateNewScore(wfscore.Id, "resgr" + g, "Standings", IMG_STANDINGS, "1", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, fullname, g);
                sc.Skip = 2;
                sc.Headers = ",,Team,Pl,W,D,L,Goals,+/-,Pts";
                sc.Sizes = "-6,0,-15,3,3,3,3,6,3,3";
                sc.Image = IMG_STANDINGS;
                AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
                scores.Add(sc);
            }

            foreach (string level in details)
            {
                if (level == details[0]) continue;
                sc = CreateNewScore(wfscore.Id, level, level, IMG_RESULTS, "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, fullname, level);
                sc.Sizes = "-5,17,+1,-17,-15,0";
                AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            sc = CreateNewScore(wfscore.Id, "topscorers", "Top Scorers", IMG_TOP_SCORER, "0", index++);
            sc.Url = String.Format("{0}torjaeger/{1}/", WF_URL, fullname);
            sc.Sizes = "6,0,-22,-20,-12";
            AddRule(sc, -1, Operation.EqualTo, "1", RuleAction.FormatLine, "Level1");
            AddHighlightRule(sc, wfscore.Highlights, 4, RuleAction.FormatLine);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "0", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = "-10,0,-24";
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }
    }
}
