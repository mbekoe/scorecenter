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

        private const string IMG_REFEREE = @"Misc\Referee.png";
        private const string IMG_TOPSCORER = @"Football\Top Scorers.png";
        private const string IMG_TABELLE = @"Misc\score.png";
        private const string IMG_TABELLE_HOME = @"Misc\tabelle_home.png";
        private const string IMG_TABELLE_AWAY = @"Misc\tabelle_away.png";
        private const string IMG_TABELLE_ROUND1 = @"Misc\tabelle_round1.png";
        private const string IMG_TABELLE_ROUND2 = @"Misc\tabelle_round2.png";
        private const string IMG_RESULTS = @"Results.png";

        private const string XPATH_RESULTS = "//table[@class='egmSnippetContent egmMatchesTable']";
        private const string XPATH_TABELLE = "//table[@class='egmSnippetContent']";
        private const string XPATH_FAIRNESS = "//table[@class='egmSnippetContent']";
        private const string XPATH_SCORER = "//table[@class='egmSnippetContent']";

        private const string SIZES_RESULTS = "-15,-12,20,+1,-20,-8";
        private const string SIZES_TABELLE = "3,-25,3,3,3,3,6,4,4,0";
        private const string SIZES_FAIRNESS = "3,-25,3,3,3,3,3,3,3,5";
        private const string SIZES_SCORER = "4,-25,-25,4";

        private const string HEADER_RESULTS = "Date,Time";
        private const string HEADER_TABELLE = ",Team,Pl,W,D,L,Goals,+/-,Pts";
        private const string HEADER_FAIRNESS = ",Team,Pl,Y,YR,R,T,U,Pts,R";
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

        public static IList<BaseScore> GetRealScores(FussballdeScore score, ScoreParameter[] parameters)
        {
            List<BaseScore> scores = new List<BaseScore>();
            GenericScore sc = null;
            int index = 0;

            var details = score.Details.Split(',').ToList();

            sc = GenericScore.CreateNewScore(score.Id, C_KEY_RESULTS, "Results", XPATH_RESULTS, IMG_RESULTS, "", index++);
            sc.Url = GetUrl(score.Url, C_KEY_RESULTS);
            sc.Skip = 1;
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesResults", SIZES_RESULTS);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderResults", HEADER_RESULTS);
            sc.AddRule(3, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddRule(5, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddRule(1, Operation.Contains, "vorheriger Spieltag", RuleAction.SkipLine, "");
            sc.AddRule(1, Operation.Contains, "Verlegte Spiele", RuleAction.MergeCells, "");
            scores.Add(sc);

            sc = GenericScore.CreateNewScore(score.Id, "tabelle", "tabelle", XPATH_TABELLE, IMG_TABELLE, "", index++);
            sc.Url = GetUrl(score.Url, C_KEY_TABELLE);
            sc.Skip = 1;
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
            sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddLevelsRule(score.Levels);
            sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
            scores.Add(sc);

            if (details.Contains("runde"))
            {
                sc = GenericScore.CreateNewScore(score.Id, "hinrunde", "Hin Runde", XPATH_TABELLE, IMG_TABELLE_HOME, "0", index++);
                sc.Url = GetUrl(score.Url, C_KEY_RUNDE);
                sc.Skip = 1;
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
                sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
                scores.Add(sc);

                sc = GenericScore.CreateNewScore(score.Id, "rueckrunde", "Rueck Runde", XPATH_TABELLE, IMG_TABELLE_AWAY, "1", index++);
                sc.Url = GetUrl(score.Url, C_KEY_RUNDE);
                sc.Skip = 1;
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
                sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
                scores.Add(sc);
            }

            sc = GenericScore.CreateNewScore(score.Id, "heim", "Heim", XPATH_TABELLE, IMG_TABELLE_ROUND1, "0", index++);
            sc.Url = GetUrl(score.Url, C_KEY_HEIMAUS);
            sc.Skip = 1;
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
            sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
            scores.Add(sc);

            sc = GenericScore.CreateNewScore(score.Id, "auswaerts", "Auswaerts", XPATH_TABELLE, IMG_TABELLE_ROUND2, "1", index++);
            sc.Url = GetUrl(score.Url, C_KEY_HEIMAUS);
            sc.Skip = 1;
            sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesTabelle", SIZES_TABELLE);
            sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderTabelle", HEADER_TABELLE);
            sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
            sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
            scores.Add(sc);

            if (details.Contains("fairness"))
            {
                sc = GenericScore.CreateNewScore(score.Id, "fairnesstabelle", "Fairness", XPATH_FAIRNESS, IMG_REFEREE, "", index++);
                sc.Url = GetUrl(score.Url, C_KEY_FAIRNESS);
                sc.Skip = 1;
                sc.Sizes = ScoreCenter.GetParameter(parameters, "Fde.SizesFairness", SIZES_FAIRNESS);
                sc.Headers = ScoreCenter.GetParameter(parameters, "Fde.HeaderFairness", HEADER_FAIRNESS);
                sc.AddRule(2, Operation.Contains, CUT_BEFORE, RuleAction.CutBefore, "");
                sc.AddHighlightRule(score.Highlights, 2, RuleAction.FormatLine);
                scores.Add(sc);
            }

            if (details.Contains("torjaeger"))
            {
                sc = GenericScore.CreateNewScore(score.Id, "torjaeger", "Top Scorer", XPATH_SCORER, IMG_TOPSCORER, "", index++);
                sc.Url = GetUrl(score.Url, C_KEY_SCORER);
                sc.Skip = 1;
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
        public static string GetEmblemUrl(string html)
        {
            string url = GetUrl(html, C_KEY_RESULTS);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(url);

            // parse it
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(EMBLEM_PATH);
            if (nodes == null)
                return "";
            return nodes[0].GetAttributeValue("src", "");
        }

        protected override string[][] Parse(FussballdeScore score, bool reload, ScoreParameter[] parameters)
        {
            return null;
        }
    }
}
