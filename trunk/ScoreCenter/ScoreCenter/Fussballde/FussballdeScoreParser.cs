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

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    /// <summary>
    /// Parser for Worldfootball site.
    /// </summary>
    public class FussballdeScoreParser : ScoreParser<FussballdeScore>
    {
        #region Constants
        private const string MAIN_URL = @"http://ergebnisdienst.fussball.de";
        private const string EMBLEM_PATH = "//a[@class='egmAssociationLogo']/img";
        private const string ROUND_XPATH = @"//select[@class='egmMatchDaySelect']/option";

        private const string IMG_REFEREE = @"Misc\Referee";
        private const string IMG_TOPSCORER = @"Football\Top Scorers";
        private const string IMG_TABELLE = @"Misc\score";
        private const string IMG_TABELLE_HOME = @"Misc\table_home";
        private const string IMG_TABELLE_AWAY = @"Misc\table_away";
        private const string IMG_TABELLE_ROUND1 = @"Misc\table_round1";
        private const string IMG_TABELLE_ROUND2 = @"Misc\table_round2";
        private const string IMG_RESULTS = @"Results";

        private const string XPATH_RESULTS = "//table[@class='egmSnippetContent egmMatchesTable']";
        private const string XPATH_TABELLE = "//table[@class='egmSnippetContent']";
        private const string XPATH_FAIRPLAY = "//table[@class='egmSnippetContent']";
        private const string XPATH_SCORER = "//table[@class='egmSnippetContent']";

        private const string SIZES_RESULTS = "-10,-6,20,+1,-20,-6";
        private const string SIZES_TABELLE = "3,-25,3,3,3,3,6,4,4,0";
        private const string SIZES_FAIRPLAY = "3,-25,3,3,3,3,3,3,3,5";
        private const string SIZES_SCORER = "4,-25,-25,4";

        private const string HEADER_RESULTS = "Date,Time";
        private const string HEADER_TABELLE = ",Team,Pl,W,D,L,Goals,+/-,Pts";
        private const string HEADER_FAIRPLAY = ",Team,Pl,Y,YR,R,T,U,Pts,R";
        private const string HEADER_SCORER = ",Player,Team,Goals";

        private const string CUT_BEFORE = "Weiter zur";

        private const string C_KEY_RESULTS = "begegnungen";
        private const string C_KEY_TABELLE = "tabelle";
        private const string C_KEY_RUNDE = "hinrueckrunde";
        private const string C_KEY_HEIMAUS = "heimauswaerts";
        private const string C_KEY_FAIRNESS = "fairnesstabelle";
        private const string C_KEY_SCORER = "torjaeger";
        #endregion

        private static ScoreCache s_cache = new ScoreCache(0);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lifetime"></param>
        public FussballdeScoreParser(int lifetime)
            : base(lifetime)
        {
        }

        private static string GetUrl(string url, string name)
        {
            return String.Format("{0}/{1}/{2}", MAIN_URL, name, url);
        }

        public static string GetMainUrl(string url)
        {
            return GetUrl(url, C_KEY_RESULTS);
        }

        private static void SetRound(GenericScore score)
        {
            // get main page
            string html = s_cache.GetScore(score.Url, "", true);
            // load it as html document
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(html);
            // search for nodes
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(ROUND_XPATH);
            if (nodes != null)
            {
                int max = nodes.Count;
                int round = -1;

                foreach (var node in nodes)
                {
                    if (node.GetAttributeValue("selected", "") == "selected")
                    {
                        string rr = node.GetAttributeValue("value", "");
                        var elements = rr.Split('/');
                        int.TryParse(elements[elements.Length - 3], out round);
                        break;
                    }
                }

                if (round > 0)
                {
                    score.Url += String.Format("/spieltag/{0}/mandant/89", VariableUrl.PARAM);
                    score.Range = new VariableUrl(round, 1, max, LocalizationManager.GetString(Labels.RoundLabel));
                }
            }
        }

        public static IList<BaseScore> GetRealScores(FussballdeScore score, ScoreParameter[] parameters)
        {
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;
            int index = 0;

            var details = score.Details.Split(',').ToList();

            sc = GenericScore.CreateNewScore(score.Id, C_KEY_RESULTS, "Results", XPATH_RESULTS, IMG_RESULTS, "", index++);
            sc.Url = GetUrl(score.Url, C_KEY_RESULTS);
            SetRound(sc);
            sc.Skip = 1;
            sc.Dictionary = "Fussballde";
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesResults", SIZES_RESULTS);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderResults", HEADER_RESULTS);
            sc.AddRule(3, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddRule(5, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddRule(1, Operation.Contains, "vorheriger Spieltag", RuleAction.SkipLine, "");
            sc.AddRule(1, Operation.Contains, "Verlegte Spiele", RuleAction.MergeCells, "");
            scores.Add(sc);

            sc = GenericScore.CreateNewScore(score.Id, C_KEY_TABELLE, "Standings", XPATH_TABELLE, IMG_TABELLE, "", index++);
            sc.Url = GetUrl(score.Url, C_KEY_TABELLE);
            sc.Skip = 1;
            sc.Dictionary = "Fussballde";
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
            sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddLevelsRule(score.Levels);
            sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
            scores.Add(sc);

            if (details.Contains("runde"))
            {
                sc = GenericScore.CreateNewScore(score.Id, "hinrunde", "1st Round", XPATH_TABELLE, IMG_TABELLE_ROUND1, "0", index++);
                sc.Url = GetUrl(score.Url, C_KEY_RUNDE);
                sc.Skip = 1;
                sc.Dictionary = "Fussballde";
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
                sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
                scores.Add(sc);

                sc = GenericScore.CreateNewScore(score.Id, "rueckrunde", "2nd Round", XPATH_TABELLE, IMG_TABELLE_ROUND2, "1", index++);
                sc.Url = GetUrl(score.Url, C_KEY_RUNDE);
                sc.Skip = 1;
                sc.Dictionary = "Fussballde";
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
                sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
                scores.Add(sc);
            }

            sc = GenericScore.CreateNewScore(score.Id, "heim", "Home", XPATH_TABELLE, IMG_TABELLE_HOME, "0", index++);
            sc.Url = GetUrl(score.Url, C_KEY_HEIMAUS);
            sc.Skip = 1;
            sc.Dictionary = "Fussballde";
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
            sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
            scores.Add(sc);

            sc = GenericScore.CreateNewScore(score.Id, "auswaerts", "Away", XPATH_TABELLE, IMG_TABELLE_AWAY, "1", index++);
            sc.Url = GetUrl(score.Url, C_KEY_HEIMAUS);
            sc.Skip = 1;
            sc.Dictionary = "Fussballde";
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
            sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
            scores.Add(sc);

            if (details.Contains("fairness"))
            {
                sc = GenericScore.CreateNewScore(score.Id, C_KEY_FAIRNESS, "Fair Play", XPATH_FAIRPLAY, IMG_REFEREE, "", index++);
                sc.Url = GetUrl(score.Url, C_KEY_FAIRNESS);
                sc.Skip = 1;
                sc.Dictionary = "Fussballde";
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesFairplay", SIZES_FAIRPLAY);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderFairplay", HEADER_FAIRPLAY);
                sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
                scores.Add(sc);
            }

            if (details.Contains("torjaeger"))
            {
                sc = GenericScore.CreateNewScore(score.Id, C_KEY_SCORER, "Top Scorers", XPATH_SCORER, IMG_TOPSCORER, "", index++);
                sc.Url = GetUrl(score.Url, C_KEY_SCORER);
                sc.Skip = 1;
                sc.Dictionary = "Fussballde";
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesScorer", SIZES_SCORER);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderScorer", HEADER_SCORER);
                sc.AddRule(3, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 3, RuleAction.FormatLine);
                scores.Add(sc);
            }

            return scores;
        }

        /// <summary>
        /// Get the icon for worldfootball.
        /// </summary>
        /// <param name="html">The HTML of the home page of the score.</param>
        /// <returns>The URL of the icon.</returns>
        public static string GetEmblemUrl(string html, int index)
        {
            string url = GetUrl(html, C_KEY_RESULTS);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(url);

            // parse it
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(EMBLEM_PATH);
            if (nodes == null)
                return "";
            int v = index % nodes.Count;
            return nodes[v].GetAttributeValue("src", "");
        }

        protected override string[][] Parse(FussballdeScore score, bool reload, ScoreParameter[] parameters)
        {
            return null;
        }
    }
}
