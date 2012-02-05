#region Copyright (C) 2005-2011 Team MediaPortal

/* 
 *      Copyright (C) 2005-2011 Team MediaPortal
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

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    public interface IScoreParser
    {
        string[][] Read(BaseScore score, bool reload, ScoreParameter[] parameters);
        void ClearCache();
    }

    public abstract class ScoreParser<T> : IScoreParser
        where T: BaseScore
    {
        protected ScoreCache m_cache;

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
        protected abstract string[][] Parse(T score, bool reload, ScoreParameter[] parameters);
        public string[][] Read(BaseScore score, bool reload, ScoreParameter[] parameters)
        {
            return Parse(score as T, reload, parameters);
        }

        /// <summary>
        /// Parse a HTML table.
        /// </summary>
        /// <param name="table">The HtmlNode representing the table to parse.</param>
        /// <param name="skip">The number of line to parse.</param>
        /// <param name="max">The maximum number of line to parse.</param>
        /// <param name="options">Parsing options.</param>
        /// <returns></returns>
        protected static string[][] ParseTable(HtmlNode table,
            int skip, int max, bool allowNewLine, bool useTheader, bool useCaption, bool parseImgAlt)
        {
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

            string[][] result = ParseLines(lines, ".//td | .//th", skip, max, allowNewLine, parseImgAlt);

            return result.Where(l => l != null).ToArray();
        }

        protected static string[][] ParseLines(HtmlNodeCollection lines, string xpathCol,
            int skip, int max, bool allowNewLine, bool parseImgAlt)
        {
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

                HtmlNodeCollection columns = line.SelectNodes(xpathCol);
                if (columns == null || columns.Count == 0)
                {
                    //continue;
                    columns = new HtmlNodeCollection(line.ParentNode);
                    columns.Add(line);
                }

                int i = 0;
                int nbChar = 0;
                string[] strLine = new string[columns.Count];
                foreach (HtmlNode column in columns)
                {
                    string value = column.InnerText.Normalize();
                    string tt = Tools.TransformHtml(value.Trim(' ', '\n', '\t', '\r'), allowNewLine).Trim();
                    if (tt.Length == 0 && parseImgAlt)
                    {
                        var images = column.SelectNodes(".//img");
                        if (images != null && images.Count > 0)
                        {
                            foreach (var img in images)
                            {
                                if (tt.Length > 0) tt += " ";
                                tt += img.GetAttributeValue("alt", "");
                            }
                        }
                    }

                    nbChar += tt.Length;
                    strLine[i] = tt;
                    i++;
                }

                if (nbChar > 0)
                    result[nbLines++] = strLine;
            }
            return result;
        }

        protected static string[][] _ParseLines(HtmlNodeCollection lines, string xpathCol, int skip, int max, ParsingOptions options)
        {
            bool allowNewLine = GenericScore.CheckParsingOption(options, ParsingOptions.NewLine);
            bool parseImgAlt = GenericScore.CheckParsingOption(options, ParsingOptions.ImgAlt);

            string[][] result = new string[lines.Count][];
            int lineIndex = 0;
            int nbLines = 0;
            foreach (HtmlNode line in lines)
            {
                lineIndex++;
                if ((skip > 0 && lineIndex <= skip)
                    || (max > 0 && lineIndex > max))
                    continue;

                HtmlNodeCollection columns = line.SelectNodes(xpathCol);
                if (columns == null || columns.Count == 0)
                    continue;

                int i = 0;
                int nbChar = 0;
                string[] strLine = new string[columns.Count];
                foreach (HtmlNode column in columns)
                {
                    string value = column.InnerText.Normalize();
                    string tt = Tools.TransformHtml(value.Trim(' ', '\n', '\t', '\r'), allowNewLine).Trim();
                    if (tt.Length == 0 && parseImgAlt)
                    {
                        var images = column.SelectNodes(".//img");
                        if (images != null && images.Count > 0)
                        {
                            foreach (var img in images)
                            {
                                if (tt.Length > 0) tt += " ";
                                tt += img.GetAttributeValue("alt", "");
                            }
                        }
                    }

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

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            m_cache.Clear(); ;
        }
    }
}
