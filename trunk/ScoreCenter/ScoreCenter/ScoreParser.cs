#region Copyright (C) 2005-2009 Team MediaPortal

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
using System.Collections.Generic;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ScoreParser
    {
        /// <summary>
        /// Client used to download files.
        /// </summary>
        private WebClient m_client;

        /// <summary>
        /// Cache.
        /// </summary>
        private IDictionary<string, CacheElement> m_cache;

        private int m_lifeTime = 0;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ScoreParser(int lifeTime)
        {
            m_client = new WebClient();
            m_cache = new Dictionary<string, CacheElement>();
            m_lifeTime = lifeTime;
        }

        public string[][] Read(Score score, bool reload)
        {
            // get score definition
            string url = ParseUrl(score.Url);
            string xpath = score.XPath;
            int index = -1;
            if (xpath.Contains(";"))
            {
                xpath = score.XPath.Split(';')[0];
                index = int.Parse(score.XPath.Split(';')[1]);
            }

            int skip = score.Skip;
            int max = score.MaxLines;
            if (max > 0) max += skip;

            string html = String.Empty;
            if (!reload && m_cache.ContainsKey(url))
            {
                if (!m_cache[url].Expired)
                    html = m_cache[url].Value;
            }

            if (html.Length == 0)
            {
                html = Tools.DownloadFile(m_client, url, score.Encoding);
                m_cache[url] = new CacheElement(m_lifeTime, html);
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xpath);
            List<string[]> ll = new List<string[]>();
            
            // add headers
            if (score.Headers != null && score.Headers.Length > 0)
            {
                ll.Add(score.Headers.Split(','));
            }

            if (nodes != null && nodes.Count > 0)
            {
                if (index >= 0 && nodes.Count > index)
                {
                    string[][] rr = ParseTable(nodes[index], skip, max);
                    if (rr != null)
                    {
                        ll.AddRange(rr);
                    }
                }
                else
                {
                    foreach (HtmlNode node in nodes)
                    {
                        string[][] rr = ParseTable(node, skip, max);
                        if (rr != null)
                        {
                            ll.AddRange(rr);
                        }
                    }
                }
            }

            int nbLines = ll.Count;
            if (nodes != null && nodes[0].Name != "table" && score.MaxLines > 0) nbLines = score.MaxLines;
            string[][] aa = new string[nbLines][];
            ll.CopyTo(0, aa, 0, nbLines);
            return aa;
        }

        /// <summary>
        /// Parse the URL to replace variables parts.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The parsed URL.</returns>
        public static string ParseUrl(string url)
        {
            string result = url;

            DateTime now = DateTime.Now;
            int start, end;
            while ((start = result.IndexOf('{') + 1) > 0)
            {
                end = result.IndexOf('}');
                string format = result.Substring(start, end - start);
                if (format.Length == 1)
                {
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

        private static string[][] ParseTable(HtmlNode table,
            int skip, int max)
        {
            HtmlNodeCollection lines = table.SelectNodes(".//tr | .//thead");
            if (lines == null)
            {
                string[][] aa = new string[1][];
                aa[0] = new string[] { Tools.TransformHtml(table.InnerText.Trim(' ', '\n')).Trim() };
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

                HtmlNodeCollection columns = line.SelectNodes(".//td | .//th");

                if (columns == null || columns.Count == 0)
                    continue;

                int i = 0;
                int nbChar = 0;
                string[] strLine = new string[columns.Count];
                foreach (HtmlNode column in columns)
                {
                    string value = column.InnerText.Normalize();
                    string tt = Tools.TransformHtml(value.Trim(' ', '\n')).Trim();
                    nbChar += tt.Length;
                    strLine[i] = tt;
                    i++;
                }

                if (nbChar > 0)
                    result[nbLines++] = strLine;
            }

            return result;
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

        public void ClearCache()
        {
            m_cache.Clear(); ;
        }

        private class CacheElement
        {
            public string Value;
            public DateTime ExpirationDate;

            public CacheElement(int lifeTime, string value)
            {
                Value = value;
                ExpirationDate = lifeTime == 0 ? DateTime.MaxValue : DateTime.Now.AddMinutes(lifeTime);
            }

            public bool Expired
            {
                get { return DateTime.Now > ExpirationDate; }
            }
        }
    }
}
