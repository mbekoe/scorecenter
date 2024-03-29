﻿#region

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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    /// <summary>
    /// Parser for Worldfootball site.
    /// </summary>
    public class WorldFootballScoreParser : ScoreParser<WorldFootballScore>
    {
        #region Constants
        private const string GLOBAL_XPATH = "//table[@class='standard_tabelle']";
        private const string EMBLEM_PATH = "//div[@class='emblem']//img";
        private const string IMG_RESULTS = "Results";
        private const string IMG_NEXT = @"Misc\NextRound";
        private const string IMG_PREV = @"Misc\PrevRound";
        private const string IMG_STANDINGS = "Standings";
        private const string IMG_HISTORY = "Trophy";
        private const string IMG_SCORER_HISTORY = @"Football\Top Scorers History";
        private const string IMG_TOP_SCORER = @"Football\Top Scorers";
        private const string IMG_ASSIST = @"Football\Assists";
        private const string IMG_PLAYER = @"Misc\Player";
        private const string IMG_REFEREE = @"Misc\Referee";
        private const string IMG_STADIUM = @"Misc\Stadium";
        private const string IMG_TRANSFERS = @"Misc\Transfers";
        private const string WF_URL = "{@worldfootball}";
        private const string ROUND_REGEX = @"spieltag/(?<round>[\w]+)/";

        private const string SIZES_LEAGUE_RESULTS = "11,5,20,+1,-20,8";
        private const string SIZES_LEAGUE_ROUND_RESULTS = "11,5,20,+1,-20,8";
        private const string SIZES_HISTORY = "-10,0,-24";
        private const string SIZES_STANDINGS = "5,0,-20,3,3,3,3,7,3,3";
        private const string SIZES_SCORER_HISTORY = "-10,0,-20,0,-20,4";
        private const string SIZES_SCORER = "6,-22,0,0,-20,-12";
        private const string SIZES_CUP_RESULTS = "11,5,20,+1,-20,8";
        private const string SIZES_CUP_LEVEL1 = "15,10,17,+1,-17,-15";
        private const string SIZES_CUP_LEVEL2 = "10,17,+3,-17,-15";
        private const string SIZES_QUALIFICATION_LEVEL1 = "15,10,17,+3,-17,-15";
        private const string SIZES_QUALIFICATION_LEVEL2 = "10,17,+3,-17,-15";
        private const string SIZES_RESULTS = "-12,5,15,0,+1,0,-15,-8,0";
        private const string SIZES_GROUP_RESULTS = "-12,5,15,+1,-15,-8,0";
        private const string SIZES_GROUP_STANDINGS = "-6,0,-15,3,3,3,3,6,3,3";
        private const string SIZES_TEAM_LAST = "-10,-8,-10,-2,0,-18,-3,0";
        private const string SIZES_TEAM_RESULTS = "-10,-10,-5,-2,0,-15,-3";
        private const string SIZES_TEAM_PLAYERS = "0,+4,-30";
        private const string SIZES_TEAM_TRANSFERS = "+6,0,-20,+3,-20";
        private const string SIZES_TEAM_ARCHIVES = "-50";
        private const string SIZES_STADIUM = "0,-25,-17,0,-15,10";
        private const string SIZES_REFEREE = "-25,-10,0,4,4,4,4";
        private const string HEADERS_STANDINGS = ",,Team,Pl,W,D,L,Goals,+/-,Pts";
        private const string HEADERS_STADIUM = ",Stadium,Team,,Country,Capacity";
        private const string HEADERS_REFEREE = "Name,,,Match,Y,YR,R";
        private const string HEADERS_SCORER = ",Player,,,Team,Goals (pen.)";
        private const string HEADERS_SCORER_HIST = "Season,,Top Scorer,,Team,Goals";
        private const string HEADERS_ASSIST = ",,Player,,,Team,Assists";

        private const string KEY_COMPETITION = "competition";
        private const string KEY_TOP_SCORERS = "top scorers";
        private const string KEY_ASSISTS = "assists";
        private const string KEY_TOP_SCORERS_LIST = "top scorers' list";
        private const string KEY_ALL_WINNERS = "all winners";
        private const string KEY_REFEREES = "referees";
        private const string KEY_STADIUMS = "stadiums";

        #endregion

        private static ScoreCache s_cache = new ScoreCache(0);

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
                return DefineLeague(score, parameters);

            if (score.Kind == WorldFootballKind.Cup)
                return DefineCup(score, parameters);

            if (score.Kind == WorldFootballKind.Tournament)
                return DefineTournament(score, parameters);

            if (score.Kind == WorldFootballKind.Qualification)
                return DefineQualification(score, parameters);

            if (score.Kind == WorldFootballKind.Team)
                return DefineTeam(score, parameters);
            
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
            return GenericScore.CreateNewScore(parent, id, name, GLOBAL_XPATH, image, element, index);
        }

        private static ScoreDetails GetScoreDetails(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            string competition = ScoreCenter.GetParameter(parameters, "WF.Competition", KEY_COMPETITION);
            string home = Tools.ParseUrl(String.Format("{0}{2}/{1}/", WF_URL, wfscore.FullLeagueName, competition), parameters);
            string html = s_cache.GetScore(home, "", true);

            ScoreDetails details = new ScoreDetails(wfscore, parameters);

            // get round
            string regex = ScoreCenter.GetParameter(parameters, "WF.RoundRegEx", ROUND_REGEX);
            Regex re = new Regex(regex);
            Match fm = re.Match(html);
            int round = 0;
            int.TryParse(fm.Groups["round"].Value, out round);
            details.Round = round;

            // get items
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='navibox2']//li/a");
            if (nodes == null)
                throw new ArgumentException("div with class 'navibox2' not found, check site adress and WF.Competition parameter.");
            foreach (var node in nodes)
            {
                details.AddDetail(node.InnerHtml, node.GetAttributeValue("href", ""));
            }

            return details;
        }
        #endregion
        
        /// <summary>
        /// Defines a League.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<BaseScore> DefineLeague(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            string fullname = wfscore.FullLeagueName + "-" + wfscore.Season;

            ScoreDetails details = GetScoreDetails(wfscore, parameters);
            int round = details.Round;

            // add results
            details.AddResults(scores, round, IMG_RESULTS, fullname, index++);

            // Standings
            string element = "0";
            string nbLeague = wfscore.Details.Split(',').FirstOrDefault(x => x.Contains(";"));
            if (!String.IsNullOrEmpty(nbLeague))
            {
                element = nbLeague;
            }

            GenericScore sc = CreateNewScore(wfscore.Id, "table", "Standings", IMG_STANDINGS, element, index++);
            sc.BetweenElts = BetweenElements.RepeatHeader;
            sc.Url = String.Format("{0}spielplan/{1}-spieltag/{2}/tabelle/", WF_URL, fullname, round);
            sc.Skip = 1;
            sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.LeagueStandings", SIZES_STANDINGS);
            sc.Dictionary = "WF.table";
            sc.Headers = ScoreCenter.GetParameter(parameters, "WF.HeaderStandings", HEADERS_STANDINGS);
            sc.AddLevelsRule(wfscore.Levels);
            sc.AddHighlightRule(wfscore.Highlights, 3, RuleAction.FormatLine);
            scores.Add(sc);

            details.AddTopScorerScore(scores, fullname, index++);
            details.AddAssistsScore(scores, fullname, index++);
            details.AddStadiumScore(scores, fullname, index++);
            details.AddRefereeScore(scores, fullname, index++);
            details.AddHistoryScore(scores, index++);
            details.AddTopScorerHistScore(scores, index++);
            
            return scores;
        }

        /// <summary>
        /// Defines a Cup.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<BaseScore> DefineCup(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();

            // add last results score
            GenericScore sc = CreateNewScore(wfscore.Id, "last", "Last Results", IMG_RESULTS, "2", index++);
            string competition = ScoreCenter.GetParameter(parameters, "WF.Competition", KEY_COMPETITION);
            sc.Url = String.Format("{0}{2}/{1}/", WF_URL, wfscore.FullLeagueName, competition);
            sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.CupResults", SIZES_CUP_RESULTS);
            sc.Dictionary = "WF.last";
            sc.AddRule(3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            sc.AddHighlightRule(wfscore.Highlights, 0, RuleAction.FormatCell);
            scores.Add(sc);

            string fullname = wfscore.FullLeagueName;
            if (String.IsNullOrEmpty(wfscore.Season) == false)
                fullname += "-" + wfscore.Season;

            // add rounds if any
            if (!String.IsNullOrEmpty(wfscore.Details))
            {
                string[] rounds = wfscore.Details.Split(',');
                foreach (string round in rounds)
                {
                    sc = CreateNewScore(wfscore.Id, round, round, IMG_RESULTS, "0", index++);
                    sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, fullname, round);
                    sc.Sizes = wfscore.TwoLegs && round != "finale"
                        ? ScoreCenter.GetParameter(parameters, "WF.CupLevel2", SIZES_CUP_LEVEL2)
                        : ScoreCenter.GetParameter(parameters, "WF.CupLevel1", SIZES_CUP_LEVEL1);
                    sc.AddRule(3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                    scores.Add(sc);
                }
            }

            // add details
            ScoreDetails details = GetScoreDetails(wfscore, parameters);
            details.AddTopScorerScore(scores, fullname, index++);
            details.AddAssistsScore(scores, fullname, index++);
            details.AddStadiumScore(scores, fullname, index++);
            details.AddRefereeScore(scores, fullname, index++);
            details.AddHistoryScore(scores, index++);
            details.AddTopScorerHistScore(scores, index++);

            return scores;
        }

        /// <summary>
        /// Defines a Qualification Tournament.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<BaseScore> DefineQualification(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;

            string fullname = wfscore.FullLeagueName;
            if (String.IsNullOrEmpty(wfscore.Season) == false)
                fullname += "-" + wfscore.Season;

            // add results scores
            sc = CreateNewScore(wfscore.Id, "results", "Results", IMG_RESULTS, "2", index++);
            string competition = ScoreCenter.GetParameter(parameters, "WF.Competition", KEY_COMPETITION);
            sc.Url = String.Format("{0}{2}/{1}/", WF_URL, wfscore.FullLeagueName, competition);
            sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.Results", SIZES_RESULTS);
            sc.AddRule(3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            sc.LiveConfig = LiveConfig.Copy(wfscore.LiveConfig, ScoreCenter.GetParameter(parameters, "WF.LiveFormat", "{2} {5} {4}"));
            sc.SetCanLive(true);
            sc.Element = "0";
            scores.Add(sc);

            // retrieve details: first element is the list of groups
            List<string> items = wfscore.Details.Split(',').ToList();
            char[] groups = items[0].ToCharArray();
            foreach (char g in groups)
            {
                // add group results
                sc = CreateNewScore(wfscore.Id, "gr" + g, String.Format("Group {0}", Char.ToUpper(g)), "", "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, fullname, g);
                sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.GroupResults", SIZES_GROUP_RESULTS);
                sc.Image = String.Format(@"Groups\Group{0}", Char.ToUpper(g));
                sc.AddRule(3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                sc.AddHighlightRule(wfscore.Highlights, 0, RuleAction.FormatCell);
                scores.Add(sc);

                // add group standings
                sc = CreateNewScore(wfscore.Id, "resgr" + g, "Standings", "", "1", index++);
                sc.Url = String.Format("{0}spielplan/{1}-gruppe-{2}/0/", WF_URL, fullname, g);
                sc.Skip = 1;
                sc.Headers = ScoreCenter.GetParameter(parameters, "WF.HeaderStandings", HEADERS_STANDINGS);
                sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.GroupStandings", SIZES_GROUP_STANDINGS);
                sc.Image = String.Format(@"Groups\Table{0}", Char.ToUpper(g));
                sc.AddHighlightRule(wfscore.Highlights, 3, RuleAction.FormatLine);
                scores.Add(sc);
            }

            // add rounds
            foreach (string round in items)
            {
                if (round == items[0]) continue;
                if (round == "stadium" || round == "referee") continue;
                sc = CreateNewScore(wfscore.Id, round, round, IMG_RESULTS, "0", index++);
                sc.Url = String.Format("{0}spielplan/{1}-{2}/0/", WF_URL, fullname, round);
                sc.Sizes = wfscore.TwoLegs && round != "finale"
                    ? ScoreCenter.GetParameter(parameters, "WF.QualificationLevel2", SIZES_QUALIFICATION_LEVEL2)
                    : ScoreCenter.GetParameter(parameters, "WF.QualificationLevel1", SIZES_QUALIFICATION_LEVEL1);
                sc.AddRule(3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                sc.AddRule(0, Operation.Contains, "{Rückspiel},", RuleAction.ReplaceText, "");
                scores.Add(sc);
            }

            // add details
            ScoreDetails details = GetScoreDetails(wfscore, parameters);
            details.AddTopScorerScore(scores, fullname, index++);
            details.AddAssistsScore(scores, fullname, index++);
            details.AddStadiumScore(scores, fullname, index++);
            details.AddRefereeScore(scores, fullname, index++);
            details.AddHistoryScore(scores, index++);
            details.AddTopScorerHistScore(scores, index++);

            return scores;
        }

        /// <summary>
        /// Defines a Tournament = Qualification + Top Scorer History.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<BaseScore> DefineTournament(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            return DefineQualification(wfscore, parameters);
        }

        /// <summary>
        /// Defines a Team score.
        /// </summary>
        /// <param name="wfscore">The Worldfootball definition.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<BaseScore> DefineTeam(WorldFootballScore wfscore, ScoreParameter[] parameters)
        {
            int index = 0;
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;

            sc = CreateNewScore(wfscore.Id, "teamlast", "Last", IMG_PREV, "2", index++);
            sc.Url = String.Format("{0}teams/{1}/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.TeamLast", SIZES_TEAM_LAST);
            sc.AddHighlightRule(wfscore.Highlights, 3, RuleAction.FormatLine);
            sc.AddRule(1, Operation.EndsWith, ";", RuleAction.SkipLine, "");
            scores.Add(sc);

            //sc = CreateNewScore(wfscore.Id, "teamresults", "Results", IMG_RESULTS, "0", index++);
            //sc.Url = String.Format("{0}teams/{1}/{{YYYY+1}}/3/", WF_URL, wfscore.FullLeagueName);
            //sc.Sizes = GetParameter(parameters, "WF.TeamResults", SIZES_TEAM_RESULTS);
            //AddHighlightRule(sc, wfscore.Highlights, 3, RuleAction.FormatLine);
            //AddRule(sc, 2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
            //AddRule(sc, 4, Operation.IsNull, "", RuleAction.FormatLine, "Level1");
            //scores.Add(sc);

            List<string> items = wfscore.Details.ToLower().Split(',').ToList();

            if (items.Contains("players"))
            {
                sc = CreateNewScore(wfscore.Id, "team", "Players", IMG_PLAYER, "4", index++);
                sc.Url = String.Format("{0}teams/{1}/", WF_URL, wfscore.FullLeagueName);
                sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.TeamPlayers", SIZES_TEAM_PLAYERS);
                sc.AddRule(1, Operation.EndsWith, ";", RuleAction.SkipLine, "");
                sc.AddRule(2, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            if (items.Contains("transfers"))
            {
                sc = CreateNewScore(wfscore.Id, "transfert", "Transfers", IMG_TRANSFERS, "0", index++);
                sc.Url = String.Format("{0}teams/{1}/{{YYYY+1}}/6", WF_URL, wfscore.FullLeagueName);
                sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.TeamTransfers", SIZES_TEAM_TRANSFERS);
                sc.AddRule(3, Operation.IsNull, "", RuleAction.MergeCells, "Header");
                scores.Add(sc);
            }

            sc = CreateNewScore(wfscore.Id, "teamarchives", "History", IMG_HISTORY, "1", index++);
            sc.Url = String.Format("{0}teams/{1}/1/", WF_URL, wfscore.FullLeagueName);
            sc.Sizes = ScoreCenter.GetParameter(parameters, "WF.TeamHistory", SIZES_TEAM_ARCHIVES);
            sc.AddHighlightRule(wfscore.Highlights, 3, RuleAction.FormatLine);
            sc.AddRule(1, Operation.Contains, " x ", RuleAction.FormatLine, "Header");
            scores.Add(sc);

            return scores;
        }

        private class ScoreDetails
        {
            private Dictionary<string, string> m_details = new Dictionary<string, string>();
            public int Round { get; set; }

            private WorldFootballScore m_score;
            private ScoreParameter[] m_parameters;

            public ScoreDetails(WorldFootballScore wfscore, ScoreParameter[] parameters)
            {
                m_score = wfscore;
                m_parameters = parameters;
            }

            public void AddDetail(string detail, string url)
            {
                if (!HasDetail(detail))
                    m_details.Add(detail.ToLower(), url);
            }

            public bool HasDetail(string detail)
            {
                return m_details.Keys.Contains(detail.ToLower());
            }

            public void AddResults(List<BaseScore> scores, int round, string icon, string fullname, int index)
            {
                // Round Results
                GenericScore sc = CreateNewScore(m_score.Id, "round", "Results", icon, "0", index);
                sc.Url = String.Format("{0}spielplan/{1}-spieltag/{2}/ergebnisse/", WF_URL, fullname, VariableUrl.PARAM);
                sc.Range = new VariableUrl(round, 1, m_score.Rounds, LocalizationManager.GetString(Labels.RoundLabel));
                sc.SetCanLive(true);
                sc.SetLive(m_score.IsLive());
                sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.LeagueRoundResults", SIZES_LEAGUE_ROUND_RESULTS);
                sc.Dictionary = "WF.last";
                sc.AddRule(1, Operation.IsNotNull, "", RuleAction.FormatCell, "Header");
                sc.AddHighlightRule(m_score.Highlights, 0, RuleAction.FormatCell);
                scores.Add(sc);
            }

            public void AddTopScorerScore(List<BaseScore> scores, string fullname, int index)
            {
                // Top Scorers
                if (this.HasDetail(ScoreCenter.GetParameter(m_parameters, "WF.KeyTopScorers", KEY_TOP_SCORERS)))
                {
                    GenericScore sc = CreateNewScore(m_score.Id, "topscorers", "Top Scorers", IMG_TOP_SCORER, "0", index);
                    sc.Url = String.Format("{0}torjaeger/{1}/", WF_URL, fullname);
                    sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.TopScorers", SIZES_SCORER);
                    sc.Skip = 1;
                    sc.Headers = ScoreCenter.GetParameter(m_parameters, "WF.HeaderScorer", HEADERS_SCORER);
                    sc.Dictionary = "WF.scorer";
                    sc.AddRule(-1, Operation.EqualTo, "1", RuleAction.FormatLine, "Level1");
                    sc.AddHighlightRule(m_score.Highlights, 4, RuleAction.FormatLine);
                    scores.Add(sc);
                }
            }

            public void AddAssistsScore(List<BaseScore> scores, string fullname, int index)
            {
                // Assists
                if (this.HasDetail(ScoreCenter.GetParameter(m_parameters, "WF.KeyAssists", KEY_ASSISTS)))
                {
                    GenericScore sc = CreateNewScore(m_score.Id, "assists", "Assists", IMG_ASSIST, "0", index++);
                    sc.Url = String.Format("{0}assists/{1}/", WF_URL, fullname);
                    sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.TopScorers", SIZES_SCORER);
                    sc.Skip = 1;
                    sc.Headers = ScoreCenter.GetParameter(m_parameters, "WF.HeaderAssist", HEADERS_ASSIST);
                    sc.Dictionary = "WF.scorer";
                    sc.AddRule(-1, Operation.EqualTo, "1", RuleAction.FormatLine, "Level1");
                    sc.AddHighlightRule(m_score.Highlights, 4, RuleAction.FormatLine);
                    scores.Add(sc);
                }
            }

            public void AddTopScorerHistScore(List<BaseScore> scores, int index)
            {
                // Top scorer History
                if (this.HasDetail(ScoreCenter.GetParameter(m_parameters, "WF.KeyTopScorersList", KEY_TOP_SCORERS_LIST)))
                {
                    GenericScore sc = CreateNewScore(m_score.Id, "scorerhistory", "Top Scorer History", IMG_SCORER_HISTORY, "0", index);
                    sc.Url = String.Format("{0}torschuetzenkoenige/{1}/", WF_URL, m_score.FullLeagueName);
                    sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.TopScorerHistory", SIZES_SCORER_HISTORY);
                    sc.Skip = 1;
                    sc.Headers = ScoreCenter.GetParameter(m_parameters, "WF.HeaderScorerHist", HEADERS_SCORER_HIST);
                    sc.Dictionary = "WF.scorer";
                    sc.AddHighlightRule(m_score.Highlights, 3, RuleAction.FormatLine);
                    scores.Add(sc);
                }
            }

            public void AddHistoryScore(List<BaseScore> scores, int index)
            {
                // History
                if (this.HasDetail(ScoreCenter.GetParameter(m_parameters, "WF.KeyAllWinners", KEY_ALL_WINNERS)))
                {
                    GenericScore sc = CreateNewScore(m_score.Id, "archives", "History", IMG_HISTORY, "0", index);
                    sc.Url = String.Format("{0}sieger/{1}/", WF_URL, m_score.FullLeagueName);
                    sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.History", SIZES_HISTORY);
                    sc.Skip = 1;
                    sc.Dictionary = "WF.archives";
                    sc.AddHighlightRule(m_score.Highlights, 3, RuleAction.FormatLine);
                    scores.Add(sc);
                }
            }

            public void AddRefereeScore(List<BaseScore> scores, string fullname, int index)
            {
                // Referee
                if (this.HasDetail(ScoreCenter.GetParameter(m_parameters, "WF.KeyReferees", KEY_REFEREES)))
                {
                    GenericScore sc = CreateNewScore(m_score.Id, "referee", "Referees", IMG_REFEREE, "0", index);
                    sc.Url = String.Format("{0}schiedsrichter/{1}/1/", WF_URL, fullname);
                    sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.Referee", SIZES_REFEREE);
                    sc.Skip = 1;
                    sc.Headers = ScoreCenter.GetParameter(m_parameters, "WF.HeaderReferee", HEADERS_REFEREE);
                    sc.AddRule(-1, Operation.IsLast, "1", RuleAction.SkipLine, "");
                    scores.Add(sc);
                }
            }

            public void AddStadiumScore(List<BaseScore> scores, string fullname, int index)
            {
                // Stadiums
                if (this.HasDetail(ScoreCenter.GetParameter(m_parameters, "WF.KeyStadiums", KEY_STADIUMS)))
                {
                    GenericScore sc = CreateNewScore(m_score.Id, "stadium", "Stadiums", IMG_STADIUM, "0", index);
                    sc.Url = String.Format("{0}spielorte/{1}/", WF_URL, fullname);
                    sc.Sizes = ScoreCenter.GetParameter(m_parameters, "WF.Stadium", SIZES_STADIUM);
                    sc.Skip = 1;
                    sc.Headers = ScoreCenter.GetParameter(m_parameters, "WF.HeaderStadium", HEADERS_STADIUM);
                    scores.Add(sc);
                }
            }
        }
    }
}
