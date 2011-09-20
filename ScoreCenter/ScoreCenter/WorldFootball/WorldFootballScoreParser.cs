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
        private const string IMG_SCORER_HISTORY = @"Football\Top Scorers History";
        private const string IMG_TOP_SCORER = @"Football\Top Scorers";
        private const string IMG_PLAYER = @"Misc\Player";
        private const string IMG_TRANSFERS = @"Misc\Transfers";
        private const string WF_URL = "{@worldfootball}";

        private const string SIZES_LEAGUE_RESULTS = "11,5,20,+1,-20,8";
        private const string SIZES_HISTORY = "-10,0,-24";
        private const string SIZES_STANDINGS = "5,0,-20,3,3,3,3,5,3,3";
        private const string SIZES_SCORER_HISTORY = "-10,0,-20,0,-20,4";
        private const string SIZES_SCORER = "6,0,-22,-20,-12";
        private const string SIZES_CUP_RESULTS = "11,5,20,+1,-20,8";
        private const string SIZES_CUP_LEVEL = "-5,17,+1,-17,-15,0";
        private const string SIZES_RESULTS = "-12,5,15,+1,-15,-8,0";
        private const string SIZES_GROUP_RESULTS = "-12,15,+1,-15,-8,0";
        private const string SIZES_GROUP_STANDINGS = "-6,0,-15,3,3,3,3,6,3,3";
        private const string SIZES_TEAM_LAST = "-10,-8,-10,-2,0,-18,-3,0";
        private const string SIZES_TEAM_RESULTS = "-10,-10,-5,-2,0,-15,-3";
        private const string SIZES_TEAM_PLAYERS = "+4,-40";
        private const string SIZES_TEAM_TRANSFERS = "+6,0,-20,+3,0,-20";
        private const string SIZES_TEAM_ARCHIVES = "-50";
        private const string HEADERS_STANDINGS = ",,Team,Pl,W,D,L,Goals,+/-,Pts";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lifetime"></param>
        public WorldFootballScoreParser(int lifetime)
            : base(lifetime)
        {
        }

        public static IList<BaseScore> GetRealScores(WorldFootballScore score, ScoreParameter[] parameters)
        {
            if (score.Kind == WorldFootballKind.League)
                return AddLeague(score, parameters);

            if (score.Kind == WorldFootballKind.Cup)
                return AddCup(score, parameters);

            if (score.Kind == WorldFootballKind.Tournament)
                return AddTournament(score, parameters);

            if (score.Kind == WorldFootballKind.Qualification)
                return AddQualification(score, parameters);

            if (score.Kind == WorldFootballKind.Team)
                return AddTeam(score, parameters);
            
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
            sc.CannotLive = true;

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

        private static string GetParameter(ScoreParameter[] parameters, string name, string defaultValue)
        {
            string result = defaultValue;

            if (parameters != null && parameters.Length > 0)
            {
                ScoreParameter p = parameters.FirstOrDefault(pp => pp.name == name);
                if (p != null && !String.IsNullOrEmpty(p.Value))
                    result = p.Value;
            }
            
            return result;
        }
        #endregion
        
        /// <summary>
        /// Defines a League.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <returns></returns>
        private static List<BaseScore> AddLeague(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            string fln2 = wfscore.FullLeagueName;
            if (wfscore.League == "otp-liga") fln2 = "hun-monicomp-liga";

            // Last Results
            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", IMG_RESULTS, "3", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.LeagueResults", SIZES_LEAGUE_RESULTS);
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            // Next Round
            sc = CreateNewScore(wfscore.Id, "next", "Next Round", IMG_NEXT, "2", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.LeagueNext", SIZES_LEAGUE_RESULTS);
            sc.LiveConfig = wfscore.LiveConfig;
            if (sc.LiveConfig != null) sc.LiveConfig.Value = GetParameter(parameters, "WF.LiveFormat", "{2} {5} {4}");
            sc.CannotLive = false;
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
            sc.Url = String.Format("{0}spielplan/{1}-{2}-spieltag/{3}/tabelle/", WF_URL, fln2, wfscore.Season, nbrounds);
            sc.Skip = 2;
            sc.Sizes = GetParameter(parameters, "WF.LeagueStandings", SIZES_STANDINGS);
            sc.Headers = GetParameter(parameters, "WF.HeaderStandings", HEADERS_STANDINGS);
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
            sc.Url = String.Format("{0}torjaeger/{1}-{2}/", WF_URL, fln2, wfscore.Season);
            sc.Sizes = GetParameter(parameters, "WF.TopScorers", SIZES_SCORER);
            AddRule(sc, -1, Operation.EqualTo, "1", RuleAction.FormatLine, "Level1");
            AddHighlightRule(sc, wfscore.Highlights, 4, RuleAction.FormatLine);
            scores.Add(sc);

            // History
            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "0", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = GetParameter(parameters, "WF.History", SIZES_HISTORY);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            // Top scorer History
            sc = CreateNewScore(wfscore.Id, "scorerhistory", "Top Scorer History", IMG_SCORER_HISTORY, "0", index++);
            sc.Url = String.Format("{0}torschuetzenkoenige/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = GetParameter(parameters, "WF.TopScorerHistory", SIZES_SCORER_HISTORY);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        /// <summary>
        /// Defines a Cup.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <returns></returns>
        private static List<BaseScore> AddCup(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", IMG_RESULTS, "2", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.CupResults", SIZES_CUP_RESULTS);
            AddRule(sc, 3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            string fullname = wfscore.FullLeagueName;
            if (String.IsNullOrEmpty(wfscore.Season) == false)
                fullname += "-" + wfscore.Season;

            if (!String.IsNullOrEmpty(wfscore.Details))
            {
                string[] details = wfscore.Details.Split(',');
                foreach (string level in details)
                {
                    sc = CreateNewScore(wfscore.Id, level, level, IMG_RESULTS, "0", index++);
                    sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, fullname, level);
                    sc.Sizes = GetParameter(parameters, "WF.CupLevel", SIZES_CUP_LEVEL);
                    AddRule(sc, 3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                    scores.Add(sc);
                }
            }

            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "0", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = GetParameter(parameters, "WF.History", SIZES_HISTORY);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        /// <summary>
        /// Defines a Qualification Tournament.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <returns></returns>
        private static List<BaseScore> AddQualification(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;

            string fullname = wfscore.FullLeagueName;
            if (String.IsNullOrEmpty(wfscore.Season) == false)
                fullname += "-" + wfscore.Season;

            sc = CreateNewScore(wfscore.Id, "results", "Results", "Next", "2", index++);
            sc.Url = String.Format("{0}wettbewerb/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.Results", SIZES_RESULTS);
            AddRule(sc, 3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            sc.LiveConfig = wfscore.LiveConfig;
            if (sc.LiveConfig != null) sc.LiveConfig.Value = GetParameter(parameters, "WF.LiveFormat", "{2} {5} {4}");
            sc.CannotLive = true;
            scores.Add(sc);

            string[] details = wfscore.Details.Split(',');
            char[] groups = details[0].ToCharArray();
            foreach (char g in groups)
            {
                sc = CreateNewScore(wfscore.Id, "gr" + g, String.Format("Group {0}", Char.ToUpper(g)), "", "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, fullname, g);
                sc.Sizes = GetParameter(parameters, "WF.GroupResults", SIZES_GROUP_RESULTS);
                sc.Image = String.Format(@"Groups\Group{0}", Char.ToUpper(g));
                AddRule(sc, 3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                AddHighlightRule(sc, wfscore.Highlights, 0, RuleAction.FormatCell);
                scores.Add(sc);

                sc = CreateNewScore(wfscore.Id, "resgr" + g, "Standings", IMG_STANDINGS, "1", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, fullname, g);
                sc.Skip = 2;
                sc.Headers = GetParameter(parameters, "WF.HeaderStandings", HEADERS_STANDINGS);
                sc.Sizes = GetParameter(parameters, "WF.GroupStandings", SIZES_GROUP_STANDINGS);
                sc.Image = IMG_STANDINGS;
                AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
                scores.Add(sc);
            }

            foreach (string level in details)
            {
                if (level == details[0]) continue;
                sc = CreateNewScore(wfscore.Id, level, level, IMG_RESULTS, "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, fullname, level);
                sc.Sizes = GetParameter(parameters, "WF.CupLevel", SIZES_CUP_LEVEL);
                AddRule(sc, 3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            sc = CreateNewScore(wfscore.Id, "topscorers", "Top Scorers", IMG_TOP_SCORER, "0", index++);
            sc.Url = String.Format("{0}torjaeger/{1}/", WF_URL, fullname);
            sc.Sizes = GetParameter(parameters, "WF.TopScorers", SIZES_SCORER);
            AddRule(sc, -1, Operation.EqualTo, "1", RuleAction.FormatLine, "Level1");
            AddHighlightRule(sc, wfscore.Highlights, 4, RuleAction.FormatLine);
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "0", index++);
            sc.Url = String.Format("{0}sieger/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = GetParameter(parameters, "WF.History", SIZES_HISTORY);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfscore"></param>
        /// <returns></returns>
        private static List<BaseScore> AddTournament(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            var scores = AddQualification(wfscore, parameters);

            // Top scorer History
            var sc = CreateNewScore(wfscore.Id, "scorerhistory", "Top Scorer History", IMG_SCORER_HISTORY, "0", scores.Count() + 1);
            sc.Url = String.Format("{0}torschuetzenkoenige/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Skip = 1;
            sc.Sizes = GetParameter(parameters, "WF.TopScorerHistory", SIZES_SCORER_HISTORY);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            return scores;
        }

        private static List<BaseScore> AddTeam(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;

            sc = CreateNewScore(wfscore.Id, "last", "Last", IMG_RESULTS, "1", index++);
            sc.Url = String.Format("{0}teams/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.TeamLast", SIZES_TEAM_LAST);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            AddRule(sc, 1, Operation.EndsWith, ";", RuleAction.SkipLine, "");
            scores.Add(sc);

            sc = CreateNewScore(wfscore.Id, "results", "Results", IMG_RESULTS, "0", index++);
            sc.Url = String.Format("{0}teams/{1}/{{YYYY+1}}/3/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.TeamResults", SIZES_TEAM_RESULTS);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            List<string> items = wfscore.Details.Split(',').ToList();

            if (items.Contains("Players"))
            {
                sc = CreateNewScore(wfscore.Id, "team", "Players", IMG_PLAYER, "4", index++);
                sc.Url = String.Format("{0}teams/{1}/", WF_URL, wfscore.FullLeagueName);
                sc.Sizes = GetParameter(parameters, "WF.TeamPlayers", SIZES_TEAM_PLAYERS);
                AddRule(sc, 1, Operation.EndsWith, ";", RuleAction.SkipLine, "");
                AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            if (wfscore.Details.Contains("Transfers"))
            {
                sc = CreateNewScore(wfscore.Id, "transfert", "Transfers", IMG_TRANSFERS, "0", index++);
                sc.Url = String.Format("{0}teams/{1}/{{YYYY+1}}/6", WF_URL, wfscore.FullLeagueName);
                sc.Sizes = GetParameter(parameters, "WF.TeamTransfers", SIZES_TEAM_TRANSFERS);
                AddRule(sc, 3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            sc = CreateNewScore(wfscore.Id, "archives", "History", IMG_HISTORY, "1", index++);
            sc.Url = String.Format("{0}teams/{1}/1/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = GetParameter(parameters, "WF.TeamHistory", SIZES_TEAM_ARCHIVES);
            AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            AddRule(sc, 1, Operation.Contains, " x ", RuleAction.FormatLine, "Header");
            scores.Add(sc);

            return scores;
        }
    }
}
