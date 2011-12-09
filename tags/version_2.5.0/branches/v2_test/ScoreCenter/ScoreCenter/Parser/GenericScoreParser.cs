using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    public class GenericScoreParser : ScoreParser<GenericScore>
    {
        public GenericScoreParser(int lifetime)
            : base(lifetime)
        {
        }
        protected override string[][] Parse(GenericScore score, bool reload, ScoreParameter[] parameters)
        {
            // get score definition
            string url = Tools.ParseUrl(score.Url, score.variable, parameters);

            ParsingOptions poptions = score.GetParseOption();
            bool newLine = GenericScore.CheckParsingOption(poptions, ParsingOptions.NewLine);

            // get the html
            string html = m_cache.GetScore(url, score.Encoding, reload);
            string rep = newLine ? Environment.NewLine : " ";
            html = html.Replace("<br>", rep);
            html = html.Replace("<br/>", rep);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(html);

            // parse it
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(score.XPath);
            List<string[]> ll = new List<string[]>();

            // add headers
            AddHeaders(score, ll);

            List<int> indexes = null;
            if (score.Element.Length > 0)
            {
                string[] elts = score.Element.Split(';');
                indexes = new List<int>(elts.Length);
                for (int i = 0; i < elts.Length; i++)
                {
                    int index;
                    if (int.TryParse(elts[i], out index))
                    {
                        indexes.Add(index);
                    }
                }
            }

            int skip = score.Skip;
            int max = score.MaxLines;
            if (max > 0) max += skip;

            if (nodes != null && nodes.Count > 0)
            {
                int inode = -1;
                foreach (HtmlNode node in nodes)
                {
                    inode++;
                    if (indexes != null && !indexes.Contains(inode))
                        continue;

                    string[][] rr = ParseTable(node, skip, max, poptions);
                    if (rr != null)
                    {
                        ll.AddRange(rr);
                        if (inode != nodes.Count) // not the last table
                        {
                            switch (score.BetweenElts)
                            {
                                case BetweenElements.EmptyLine:
                                    ll.Add(new string[] { ScoreCenterPlugin.C_HEADER });
                                    break;
                                case BetweenElements.RepeatHeader:
                                    AddHeaders(score, ll);
                                    break;
                            }
                        }
                    }
                }
            }

            int nbLines = ll.Count;
            if (nodes != null && nodes[0].Name != "table" && score.MaxLines > 0) nbLines = Math.Min(ll.Count, score.MaxLines);
            string[][] aa = new string[nbLines][];
            ll.CopyTo(0, aa, 0, nbLines);
            return aa;
        }
        /// <summary>
        /// Adds a header line in the score.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="ll"></param>
        private static void AddHeaders(GenericScore score, List<string[]> ll)
        {
            if (score.Headers != null && score.Headers.Length > 0)
            {
                ll.Add((ScoreCenterPlugin.C_HEADER + "," + score.Headers).Split(','));
            }
        }
    }
}
