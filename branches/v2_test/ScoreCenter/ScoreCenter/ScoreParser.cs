﻿#region Copyright (C) 2005-2009 Team MediaPortal

/* 
 *      Copyright (C) 2005-2009 Team MediaPortal
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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Xml;
using System.Xml.Linq;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ScoreParser
    {
        private ScoreCache m_cache;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ScoreParser(int lifeTime)
        {
            m_cache = new ScoreCache(lifeTime);
        }

        /// <summary>
        /// Reads a Score.
        /// </summary>
        /// <param name="score">The score to read.</param>
        /// <param name="reload">True to reload the page, False to reuse the one in the cache.</param>
        /// <returns>A string matrix representing the score to be displayed.</returns>
        public string[][] Read(Score score, bool reload)
        {
            if (score.Type == Node.Score)
                return ReadScore(score, reload);
            if (score.Type == Node.RSS)
                return ReadRss(score, reload);

            throw new System.NotSupportedException(String.Format("Score Type {0} is not supported", score.Type));
        }

        private string[][] ReadRss(Score score, bool reload)
        {
            XDocument feedXML = XDocument.Load("http://www.lequipe.fr/Xml/Football/Titres/actu_rss.xml");

            var title = feedXML.Descendants("title");
            if (title != null && title.Count() > 0)
                System.Console.WriteLine(title.First().Value);

            IList<string[]> feeds = new List<string[]>();
            foreach (var f in feedXML.Descendants("item"))
            {
                feeds.Add(new string[] { new FeedItem(f).ToString() });
            }

            return feeds.ToArray();
        }

        private string[][] ReadScore(Score score, bool reload)
        {
            // get score definition
            string url = ParseUrl(score.Url, score.variable);
            
            ParsingOptions poptions = score.GetParseOption();
            bool newLine = Score.CheckParsingOption(poptions, ParsingOptions.NewLine);

            // get the html
            string html = m_cache.GetScore(url, score.Encoding, reload);
            html = html.Replace("<br>", newLine ? Environment.NewLine : " ");
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
        private static void AddHeaders(Score score, List<string[]> ll)
        {
            if (score.Headers != null && score.Headers.Length > 0)
            {
                ll.Add((ScoreCenterPlugin.C_HEADER + "," + score.Headers).Split(','));
            }
        }

        /// <summary>
        /// Parse the URL to replace variables parts.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The parsed URL.</returns>
        public static string ParseUrl(string url)
        {
            return ParseUrl(url, 0);
        }
        public static string ParseUrl(string url, int delta)
        {
            string result = url;

            // parse date format
            DateTime now = DateTime.Now.AddDays(delta);
            int start, end;
            while ((start = result.IndexOf('{') + 1) > 0)
            {
                end = result.IndexOf('}');
                string format = result.Substring(start, end - start);
                if (format.Length == 1)
                {
                    // if format contains only one char add a space in the format and then remove it
                    format = " " + format;
                    result = result.Substring(0, start - 1) + now.ToString(format).Substring(1) + result.Substring(end + 1);
                }
                else
                {
                    result = result.Substring(0, start - 1) + now.ToString(format) + result.Substring(end + 1);
                }
            }

            return result;
        }

        /// <summary>
        /// Parse a HTML table.
        /// </summary>
        /// <param name="table">The HtmlNode representing the table to parse.</param>
        /// <param name="skip">The number of line to parse.</param>
        /// <param name="max">The maximum number of line to parse.</param>
        /// <param name="options">Parsing options.</param>
        /// <returns></returns>
        private static string[][] ParseTable(HtmlNode table,
            int skip, int max, ParsingOptions options)
        {
            bool allowNewLine = Score.CheckParsingOption(options, ParsingOptions.NewLine);
            bool useTheader = Score.CheckParsingOption(options, ParsingOptions.UseTheader);
            bool useCaption = Score.CheckParsingOption(options, ParsingOptions.Caption);

            string xpathHeader = ".//tr";
            if (useTheader) xpathHeader += " | .//thead | .//tfoot";
            if (useCaption) xpathHeader += " | .//caption";
            HtmlNodeCollection lines = table.SelectNodes(xpathHeader);
            if (lines == null)
            {
                string[][] aa = new string[1][];
                aa[0] = new string[] { Tools.TransformHtml(table.InnerText.Trim(' ', '\n'), allowNewLine).Trim() };
                return aa;
            }

            string[][] result = new string[lines.Count][];
            int lineIndex = 0;
            int nbLines = 0;
            foreach (HtmlNode line in lines)
            {
                lineIndex++;
                if ((skip > 0 && lineIndex <= skip)
                    || (max > 0 && lineIndex > max))
                    continue;

                if (line.Name == "caption")
                {
                    result[nbLines++] = new string[] { Tools.TransformHtml(line.InnerText.Trim(' ', '\n'), allowNewLine).Trim() };
                    continue;
                }

                HtmlNodeCollection columns = line.SelectNodes(".//td | .//th");

                if (columns == null || columns.Count == 0)
                    continue;

                int i = 0;
                int nbChar = 0;
                string[] strLine = new string[columns.Count];
                foreach (HtmlNode column in columns)
                {
                    string value = column.InnerText.Normalize();
                    string tt = Tools.TransformHtml(value.Trim(' ', '\n'), allowNewLine).Trim();
                    nbChar += tt.Length;
                    strLine[i] = tt;
                    i++;
                }

                if (nbChar > 0)
                    result[nbLines++] = strLine;
            }

            return result;
        }

        public List<Score> ParseDynamicList(Score source, bool reload)
        {
            string html = m_cache.GetScore(source.Url, source.Encoding, reload);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            List<Score> scores = new List<Score>();
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(source.XPath);
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    string url = node.Attributes[0].Value;

                    Score item = new Score();
                    item.Name = node.NextSibling.InnerHtml;
                    item.Url = Tools.GetDomain(source.Url, source.Headers + "/" + url);
                    item.Encoding = source.Encoding;
                    item.XPath = source.Name;

                    scores.Add(item);
                }
            }

            return scores;
        }

        #region Format
        /// <summary>
        /// Format a string.
        /// </summary>
        /// <remarks>
        /// If the number of character is positive the string is pad to the left.
        /// If the number of character is negative the string is pad to the right.
        /// </remarks>
        /// <param name="value">The string to format.</param>
        /// <param name="pad">The number of character to pad.</param>
        /// <param name="usePad">True to pad the string.</param>
        /// <param name="padChar">The char to use to pad.</param>
        /// <returns>The formated string.</returns>
        private static string FormatLabel(string value, int pad, bool usePad, char padChar)
        {
            string result = value;

            if (usePad)
            {
                int size = 0;
                if (pad > 0)
                {
                    result = result.PadLeft(pad, padChar);
                    size = pad;
                }
                else if (pad < 0)
                {
                    result = result.PadRight(-pad, padChar);
                    size = -pad;
                }

                if (result.Length > size)
                    result = result.Substring(0, size);
            }
            
            return result;
        }

        #endregion

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            m_cache.Clear(); ;
        }

        public class FeedItem
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }

            public FeedItem(XElement elt)
            {
                XElement xe = elt.Element("title");
                if (xe != null) this.Title = xe.Value.Normalize();

                xe = elt.Element("link");
                if (xe != null) this.Link = xe.Value;

                xe = elt.Element("description");
                if (xe != null) this.Description = xe.Value;

                xe = elt.Element("pubDate");
                if (xe != null) this.Date = DateTime.Parse(xe.Value);
            }

            public override string ToString()
            {
                //return String.Format("{0:g}: {1}", this.Date, this.Title.Substring(0, Math.Min(Title.Length, 10)));
                //if (this.Title.Length <= 49)
                return String.Format("{0}: {1}", this.Date.ToString("dd/MM HH:mm"), this.Title);
                //return "---";
            }
        }
    }
}
