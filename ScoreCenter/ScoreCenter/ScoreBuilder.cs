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
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ScoreBuilder<TC>
    {
        public delegate TC CreateControlDelegate(int posX, int posY, int width, int height,
            ColumnDisplay.Alignment alignement,
            string label, string font, int fontSize, Style style, int nbMax, int columnIndex);

        public ScoreCenter Center { get; set; }
        public Score Score { get; set; }
        public bool LimitToPage { get; set; }
        public bool AutoSize { get; set; }
        public bool AutoWrap { get; set; }

        public ScoreBuilder()
        {
            LimitToPage = true;
        }

        private string m_fontName;
        private long m_textColor;
        private int m_fontSize = 12;
        private int m_charWidth = 8;
        private int m_charHeight = 12;
        
        public void SetFont(string fontName, long textColor, int fontSize,
            int charWidth, int charHeight)
        {
            m_fontName = fontName;
            m_textColor = textColor;
            m_fontSize = fontSize;
            m_charWidth = charWidth;
            m_charHeight = charHeight;
        }

        public IList<TC> Build(string[][] labels, int startLine, int startColumn,
            int startX, int startY, int pnlWidth, int pnlHeight,
            CreateControlDelegate createControl,
            out bool overRight, out bool overDown, out int lineNumber, out int colNumber)
        {
            lineNumber = -1;
            colNumber = -1;
            overRight = false;
            overDown = false;

            //Tools.LogMessage("X = {0}, Y = {1}, W = {2}, H = {3}", startX, startY, pnlWidth, pnlHeight);

            if (Score == null || Center == null)
                return null;

            int maxX = startX + pnlWidth;
            int maxY = startY + pnlHeight;
            int posX = startX;
            int posY = startY;
            int maxColumnSize = pnlWidth / m_charWidth;

            Style defaultStyle = new Style();
            defaultStyle.ForeColor = m_textColor;

            // Get Columns Sizes
            ColumnDisplay[] cols = GetSizes(labels);

            RuleEvaluator engine = new RuleEvaluator(Score.Rules);

            ParsingOptions opt = Score.GetParseOption();
            bool reverseOrder = Score.HasPO(opt, ParsingOptions.Reverse);
            bool wordWrap = Score.HasPO(opt, ParsingOptions.WordWrap);

            // for all the rows
            List<TC> controls = new List<TC>();
            foreach (string[] row_ in labels)
            {
                // ignore empty lines
                if (row_ == null || row_.Length == 0)
                    continue;

                lineNumber++;

                // ignore lines on previous page
                if (lineNumber < startLine)
                {
                    continue;
                }

                // calculate X position
                posX = startX;// -startColumn;

                // ignore if outside and break to next row
                if (LimitToPage && posY > maxY)
                {
                    overDown = true;
                    break;
                }

                bool isHeader = (row_[0] == ScoreCenterPlugin.C_HEADER);
                string[] row = isHeader ? row_.Where(c => c != ScoreCenterPlugin.C_HEADER).ToArray() : row_;
                
                #region Evaluate rule for full line
                //bool isHeader = !String.IsNullOrEmpty(Score.Headers) && lineNumber == 0;
                Style lineStyle = defaultStyle;
                bool merge = false;
                if (!isHeader)
                {
                    Rule rule = engine.CheckLine(row, lineNumber);
                    if (rule != null)
                    {
                        // skip lines and continue
                        if (rule.Action == RuleAction.SkipLine)
                            continue;

                        merge = rule.Action == RuleAction.MergeCells;
                        lineStyle = Center.FindStyle(rule.Format) ?? defaultStyle;
                    }
                }
                #endregion

                int nbLines = 1;
                for (int index = startColumn; index < row.Length; index++)
                {
                    int colIndex = reverseOrder ? row.Length - index - 1 : index;

                    // get cell
                    string cell = row[colIndex];
                    ColumnDisplay colSize = GetColumnSize(colIndex, cols, cell, merge);
                    if (colSize.Size == 0)
                    {
                        continue;
                    }

                    // ignore controls outside area
                    if (LimitToPage && posX > maxX)
                    {
                        overRight = true;
                        continue;
                    }

                    // evaluate size of the control in pixel
                    int maxChar = Math.Min(colSize.Size + 1, maxColumnSize);

                    // wrap needed?
                    if (wordWrap || AutoWrap)
                    {
                        int i = maxChar + 1;
                        while (i < cell.Length)
                        {
                            cell = cell.Insert(i, Environment.NewLine);
                            i += maxChar + 1;
                        }
                    }

                    // count new lines
                    int nb = Tools.CountLines(cell);
                    nbLines = Math.Max(nbLines, nb);

                    int length = m_charWidth * maxChar;
                    int height = m_charHeight * nbLines;
                    if (posX < startX)
                    {
                        // cell was on previous page
                        // increase posX so that we can get to current page
                        posX += length;
                        continue;
                    }

                    #region Evaluate rule for the cell
                    Style cellStyle = lineStyle;
                    if (!isHeader)
                    {
                        Rule cellRule = engine.CheckCell(cell, colIndex);
                        if (cellRule != null)
                        {
                            cellStyle = Center.FindStyle(cellRule.Format) ?? lineStyle;
                            if (cellRule.Action == RuleAction.ReplaceText)
                            {
                                string str1 = cellRule.Value;
                                string str2 = String.Empty;
                                if (cellRule.Value.Contains(","))
                                {
                                    string[] elts = cellRule.Value.Split(',');
                                    str1 = elts[0];
                                    str2 = elts[1];
                                }

                                cell = cell.Replace(str1, str2);
                            }
                        }
                    }
                    #endregion

                    // create the control
                    string[] aa = cell.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < aa.Length; i++)
                    {
                        TC control = createControl(posX, posY + (i * m_charHeight), length, m_charHeight,
                            colSize.Alignement,
                            aa[i],
                            m_fontName, m_fontSize, cellStyle, maxChar, colIndex);

                        if (control != null)
                        {
                            controls.Add(control);
                        }
                    }

                    // set X pos to the end of the control
                    posX += Math.Min(length, pnlWidth - 1);
                }

                // set Y pos to the bottom of the control
                posY += m_charHeight * nbLines;
            }

            Tools.LogMessage("{0} controls created", controls.Count);
            return controls;
        }

        private ColumnDisplay[] GetSizes(string[][] labels)
        {
            ColumnDisplay[] cols = null;
            if (!this.AutoSize)
            {
                cols = ColumnDisplay.GetSizes(Score.Sizes);
            }
            else
            {
                // evaluate max size foreach column
                // warning: all columns do not have the same number of columns
                Dictionary<int, int> coldic = new Dictionary<int, int>();
                foreach (string[] row in labels)
                {
                    if (row == null)
                        continue;

                    for (int i = 0; i < row.Length; i++)
                    {
                        string cell = row[i];
                        int length = String.IsNullOrEmpty(cell) ? 0 : cell.Length;
                        if (coldic.ContainsKey(i))
                        {
                            coldic[i] = Math.Max(coldic[i], length);
                        }
                        else
                        {
                            coldic[i] = length;
                        }
                    }
                }

                cols = new ColumnDisplay[coldic.Count];
                foreach (int key in coldic.Keys)
                {
                    cols[key] = new ColumnDisplay(coldic[key], ColumnDisplay.Alignment.Left);
                }
            }

            return cols;
        }

        private static ColumnDisplay GetColumnSize(int colIndex, ColumnDisplay[] cols, string text, bool span)
        {
            if (cols == null || span)
                return new ColumnDisplay(text.Length, ColumnDisplay.Alignment.Left);

            return colIndex < cols.Length ? cols[colIndex] : new ColumnDisplay(0, ColumnDisplay.Alignment.Left);
        }
    }
}
