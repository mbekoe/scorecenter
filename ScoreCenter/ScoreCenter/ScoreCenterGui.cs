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
using System.IO;
using System.Linq;
using System.Net;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Plugin GUI.
    /// </summary>
    public class ScoreCenterGui : GUIWindow
    {
        #region Members
        public const string SkinFileName = "MyScoreCenter.xml";
        public const string SettingsFileName = "MyScoreCenter.Settings.xml";
        
        /// <summary>Start index for GUI controls.</summary>
        private const int StartIndex = 42100;

        /// <summary>Index of the current GUI control.</summary>
        private int m_currentIndex = StartIndex;

        /// <summary>List of indices of dymacally build controls.</summary>
        private List<int> m_indices = new List<int>();
        private int m_currentLine = 0;
        private int m_currentColumn = 0;
        private Score m_currentScore = null;
        private string[][] m_lines = null;
        private Stack<int> m_prevIndex = new Stack<int>();

        /// <summary>
        /// Kind of view mode.
        /// </summary>
        private enum ViewMode
        {
            Category,
            League,
            Results
        }

        private ScoreCenter m_center;
        private ScoreParser m_parser;

        // current status
        private ViewMode m_mode = ViewMode.Category;
        private string m_currentCategory;
        private string m_currentLeague;
        private bool m_autoSize = false;

        #region Skin Controls
        [SkinControlAttribute(10)]
        protected GUIListControl lstDetails = null;
        [SkinControlAttribute(20)]
        protected GUITextScrollUpControl tbxDetails = null;
        [SkinControlAttribute(30)]
        protected GUIImage imgBackdrop = null;
        [SkinControlAttribute(40)]
        protected GUIButtonControl btnNextPage = null;

        #endregion

        #region Label numbers
        private const int C_CLEAR_CACHE = 1;
        private const int C_USE_AUTO_MODE = 2;
        private const int C_UNUSE_AUTO_MODE = 3;
        private const int C_SYNCHRO_ONLINE = 4;
        private const int C_DISABLE_ITEM = 5;
        #endregion

        #endregion

        internal ScoreParser Parser
        {
            get
            {
                if (m_parser == null)
                {
                    m_parser = new ScoreParser(m_center.Setup.CacheExpiration);
                }

                return m_parser;
            }
        }

        #region GUI overrides
        public override int GetID
        {
            get { return ScoreCenterPlugin.PluginId; }
            set { }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\" + SkinFileName);
        }

        protected override void OnPageLoad()
        {
            Tools.LogMessage("entering OnPageLoad()");
            base.OnPageLoad();

            ShowNextButton(false);
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();

            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                try
                {
                    ReadSettings();

                    GUIPropertyManager.SetProperty("#ScoreCenter.Title", m_center.Setup.Name);
                    GUIPropertyManager.SetProperty("#ScoreCenter.Category", "_");
                    GUIPropertyManager.SetProperty("#ScoreCenter.League", " ");
                    GUIPropertyManager.SetProperty("#ScoreCenter.Results", " ");
                    GUIPropertyManager.SetProperty("#ScoreCenter.Source", " ");

                    UpdateSettings(false, false);
                    LoadCategories();

                    GUIControl.FocusControl(GetID, lstDetails.GetID);
                }
                catch (Exception ex)
                {
                    Tools.LogError("Error occured while executing the OnPageLoad: ", ex);
                }
                finally
                {
                    GUIWaitCursor.Hide();
                }
            });
        }

        protected override void OnPageDestroy(int new_windowId)
        {
            SetScore(null);
            SetLeague(" ");
            SetCategory("_");
            m_parser = null;
            m_center = null;
            ClearGrid();
            base.OnPageDestroy(new_windowId);
        }

        public override void OnAction(MediaPortal.GUI.Library.Action action)
        {
            if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU)
            {
                if (m_mode != ViewMode.Category)
                {
                    UpdateListView(null, true);
                    return;
                }
            }
            
            base.OnAction(action);
        }

        protected override void OnShowContextMenu()
        {
            GUIListItem item = lstDetails.SelectedListItem;

            GUIDialogMenu menu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            menu.Reset();
            menu.SetHeading(m_center.Setup.Name);
            menu.Add(LocalizationManager.GetString(C_CLEAR_CACHE));

            if (m_autoSize)
                menu.Add(LocalizationManager.GetString(C_UNUSE_AUTO_MODE));
            else
                menu.Add(LocalizationManager.GetString(C_USE_AUTO_MODE));

            menu.Add(LocalizationManager.GetString(C_SYNCHRO_ONLINE));

            if (item.Label != "..")
            {
                menu.Add(String.Format("{1} '{0}'", item.Label, LocalizationManager.GetString(C_DISABLE_ITEM)));
            }

            menu.DoModal(GetID);
            switch (menu.SelectedId)
            {
                case 1:
                    Parser.ClearCache();
                    break;
                case 2:
                    m_autoSize = !m_autoSize;
                    break;
                case 3:
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
                    {
                        try
                        {
                            UpdateSettings(true, true);
                        }
                        catch (Exception ex)
                        {
                            Tools.LogError("Error occured while executing the Update Online: ", ex);
                        }
                        finally
                        {
                            GUIWaitCursor.Hide();
                        }
                    });
                    break;
                case 4:
                    GUIDialogYesNo dlg = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
                    string disable = LocalizationManager.GetString(C_DISABLE_ITEM);
                    dlg.SetHeading(disable);
                    dlg.SetLine(1, String.Format("{1} '{0}'?", item.Label, disable));
                    dlg.DoModal(GetID);

                    if (dlg.IsConfirmed)
                    {
                        Disable(item.Label);
                    }
                    break;
            }


            base.OnShowContextMenu();
        }

        private void UpdateSettings(bool force, bool reload)
        {
            bool updated = ExchangeManager.OnlineUpdate(m_center, force);
            if (updated)
            {
                SaveSettings();
                if (reload) LoadCategories();
            }
        }

        private void Disable(string name)
        {
            switch (m_mode)
            {
                case ViewMode.Results:
                    m_center.Scores.Where(sc => sc.Category == m_currentCategory && sc.Ligue == m_currentLeague && sc.Name == name)
                        .ForEach(sc => sc.enable = false);
                    LoadScores();
                    break;
                case ViewMode.League:
                    m_center.Scores.Where(sc => sc.Category == m_currentCategory && sc.Ligue == name)
                        .ForEach(sc => sc.enable = false);
                    LoadLeagues();
                    break;
                case ViewMode.Category:
                    m_center.Scores.Where(sc => sc.Category == name).ForEach(sc => sc.enable = false);
                    LoadCategories();
                    break;
            }

            SaveSettings();
        }

        protected override void OnClicked(int controlId, GUIControl control,
            MediaPortal.GUI.Library.Action.ActionType actionType)
        {
            if (control == btnNextPage || actionType == MediaPortal.GUI.Library.Action.ActionType.ACTION_FORWARD)
            {
                if (m_currentScore != null && m_lines != null)
                {
                    ClearGrid();
                    CreateGrid(m_lines, m_currentScore, m_currentLine, m_currentColumn);
                }
            }
            else if (control == lstDetails)
            {
                GUIListItem item = lstDetails.SelectedListItem;
                bool back = (item.Label == "..");
                if (!back && m_mode != ViewMode.Results)
                {
                    m_prevIndex.Push(lstDetails.SelectedListItemIndex);
                }

                UpdateListView(item, back);
            }

            base.OnClicked(controlId, control, actionType);
        }

        private void UpdateListView(GUIListItem item, bool back)
        {
            m_currentLine = 0;
            m_currentColumn = 0;
            m_currentScore = null;
            m_lines = null;

            ShowNextButton(false);
            ClearGrid();
            switch (m_mode)
            {
                case ViewMode.Category:
                    m_currentCategory = item.Label;
                    LoadLeagues();
                    imgBackdrop.Refresh();
                    break;
                case ViewMode.League:
                    if (back) LoadCategories();
                    else LoadScores();
                    break;
                case ViewMode.Results:
                    if (back) LoadLeagues();
                    else DisplayScore();
                    break;
            }

            if (back && m_prevIndex.Count > 0)
            {
                // always pop
                int prev = m_prevIndex.Pop();

                // if enough set
                if (lstDetails.Count > prev)
                {
                    lstDetails.SelectedListItemIndex = prev;
                }
            }
        }

        #endregion

        #region Load Selection
        private void LoadCategories()
        {
            lstDetails.Clear();
            if (m_center != null && m_center.Scores != null)
            {
                SetCategory("_");

                foreach (string category in m_center.ReadCategories())
                {
                    GUIListItem item = new GUIListItem();
                    item.Label = category;
                    item.IsFolder = true;
                    item.IconImage = GetCategoryImage(category);
                    item.IsPlayed = m_center.IsCategoryUpdated(category);

                    lstDetails.Add(item);
                }
            }

            lstDetails.Sort(new ListComparer(m_mode));
            m_mode = ViewMode.Category;
        }

        private void LoadLeagues()
        {
            string category = m_currentCategory;
            SetCategory(category);
            SetLeague(" ");
            SetScore(null);

            lstDetails.Clear();

            GUIListItem item1 = new GUIListItem();
            item1.Label = "..";
            item1.IsFolder = true;
            MediaPortal.Util.Utils.SetDefaultIcons(item1);
            lstDetails.Add(item1);

            foreach (string league in m_center.ReadLeagues(m_currentCategory))
            {
                GUIListItem item = new GUIListItem();
                item.Label = league;
                item.IsFolder = true;
                item.IconImage = GetLeagueImage(m_currentCategory, league);
                item.IsPlayed = m_center.IsLeagueUpdated(m_currentCategory, league);

                lstDetails.Add(item);
            }

            lstDetails.Sort(new ListComparer(m_mode));
            m_mode = ViewMode.League;
        }

        private void LoadScores()
        {
            m_currentLeague = lstDetails.SelectedListItem.Label;
            SetLeague(m_currentLeague);
            lstDetails.Clear();

            GUIListItem item1 = new GUIListItem();
            item1.Label = "..";
            item1.IsFolder = true;
            MediaPortal.Util.Utils.SetDefaultIcons(item1);
            lstDetails.Add(item1);

            foreach (Score score in m_center.Scores)
            {
                if (score.enable == false)
                    continue;

                if (score.Category == m_currentCategory
                    && score.Ligue == m_currentLeague)
                {
                    GUIListItem item = new GUIListItem();
                    item.Label = score.Name;
                    item.IsFolder = false;
                    item.IconImage = GetImage(score.Image);
                    item.TVTag = score;
                    item.IsPlayed = score.IsNew();

                    lstDetails.Add(item);
                }
            }

            m_mode = ViewMode.Results;
            lstDetails.Sort(new ListComparer(m_mode));
        }
        
        private void DisplayScore()
        {
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                string url = String.Empty;
                try
                {
                    GUIListItem item = lstDetails.SelectedListItem;
                    Score score = item.TVTag as Score;
                    if (score == null)
                        return;

                    url = score.Url;
                    Tools.LogMessage("ShowScore: Url={0}", score.Url);
                    Tools.LogMessage("ShowScore: XPath={0}", score.XPath);

                    string[][] results = Parser.Read(score, false);
                    SetScore(score);
                    m_currentLine = 0;
                    m_currentColumn = 0;
                    m_currentScore = score;
                    m_lines = results;
                    ShowNextButton(false);
                    CreateGrid(results, score, 0, 0);
                }
                catch (WebException exc)
                {
                    Tools.LogError("Error in LoadScore", exc);
                    string txt = "Address not found:" + Environment.NewLine;
                    txt += url;
                    GUIControl.SetControlLabel(GetID, 20, txt);
                }
                catch (Exception exc)
                {
                    Tools.LogError("Error in LoadScore", exc);
                    string txt = exc.Message + Environment.NewLine;
                    txt += exc.StackTrace;
                    GUIControl.SetControlLabel(GetID, 20, txt);
                }
                finally
                {
                    GUIWaitCursor.Hide();
                }
            });
        }

        #endregion

        #region Properties

        private void SetCategory(string name)
        {
            GUIPropertyManager.SetProperty("#ScoreCenter.Category", name);
            GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", GetCategoryImage(name));

            string bd = "-";
            if (m_center.Setup != null && !String.IsNullOrEmpty(m_center.Setup.BackdropDir))
            {
                bd = Path.Combine(m_center.Setup.BackdropDir, name);
                if (File.Exists(bd + ".jpg") == false)
                {
                    bd = Path.Combine(m_center.Setup.BackdropDir, "_");
                }

                GUIPropertyManager.SetProperty("#ScoreCenter.bd", bd);
            }
        }

        private void SetLeague(string name)
        {
            GUIPropertyManager.SetProperty("#ScoreCenter.League", name);
            GUIPropertyManager.SetProperty("#ScoreCenter.LIco", GetLeagueImage(m_currentCategory, name));
        }

        private void SetScore(Score score)
        {
            string name = " ";
            string image = "-";
            string source = " ";
            if (score != null)
            {
                name = score.Name;
                image = score.Image;

                source = score.Url.Replace("http://", String.Empty);
                source = source.Substring(0, source.IndexOf('/'));
                if (source.StartsWith("www.")) source = source.Substring(4);
            }

            GUIPropertyManager.SetProperty("#ScoreCenter.Results", name);
            GUIPropertyManager.SetProperty("#ScoreCenter.ScoreIco", GetImage(image));
            GUIPropertyManager.SetProperty("#ScoreCenter.Source", source);
        }
        #endregion

        #region Grid management
        /// <summary>
        /// Clears all controls in the grid.
        /// </summary>
        private void ClearGrid()
        {
            foreach (int i in m_indices)
            {
                Remove(i);
            }

            m_indices.Clear();
            m_currentIndex = StartIndex;
            GUIControl.SetControlLabel(GetID, 20, " ");
        }

        /// <summary>
        /// Create a grid.
        /// </summary>
        /// <param name="labels">The labels to fill the grid with.</param>
        /// <param name="score">The Score to display.</param>
        /// <param name="startLine">The first line to display.</param>
        private void CreateGrid(string[][] labels, Score score, int startLine, int startColumn)
        {
            //Tools.LogMessage("CreateGrid: L={0}, C={1}", startLine, startColumn);
            int startX = tbxDetails.XPosition;
            int maxX = startX + tbxDetails.Width;
            int maxY = tbxDetails.YPosition + tbxDetails.Height;
            int posX = startX;
            int posY = tbxDetails.YPosition;
            string fontName = tbxDetails.FontName;
            Style defaultStyle = new Style();
            defaultStyle.ForeColor = tbxDetails.TextColor;
            //Tools.LogMessage("StartX={0}, MaxX={2}, startY={1}, MaxY={3}", startX, posY, maxX, maxY);

            GUIFont font = GUIFontManager.GetFont(fontName);
            int fontSize = font.FontSize;
            int charHeight = 0, charWidth = 0;
            GetCharFonSize(fontSize, ref charWidth, ref charHeight);

            #region Get Columns Sizes
            Tools.ColumnDisplay[] cols = null;
            if (!m_autoSize)
            {
                cols = Tools.GetSizes(score.Sizes);
            }
            else
            {
                Dictionary<int, int> coldic = new Dictionary<int,int>();
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

                cols = new Tools.ColumnDisplay[coldic.Count];
                foreach (int key in coldic.Keys)
                {
                    cols[key] = new Tools.ColumnDisplay(coldic[key].ToString());
                }
            }
            #endregion

            RuleEvaluator engine = new RuleEvaluator(score.Rules);

            int lineNumber = - 1;
            bool overRight = false;
            bool overDown = false;

            // for all the rows
            List<GUIControl> controls = new List<GUIControl>();
            foreach (string[] row in labels)
            {
                // ignore empty lines
                if (row == null)
                    continue;

                lineNumber++;

                // ignore lines on previous page
                if (lineNumber < startLine)
                {
                    //Tools.LogMessage("Skip lineNumber {0} < startLine {1}", lineNumber, startLine);
                    continue;
                }

                // calculate X position
                posX = startX - startColumn;
                //Tools.LogMessage("L={0}, C={1}, V={2}", lineNumber, m_currentColumn, row[0]);

                // ignore if outside and break to next row
                if (posY > maxY)
                {
                    //Tools.LogMessage("OverDown LN={0} Start={1}", lineNumber, startLine);
                    overDown = true;
                    break;
                }

                #region Evaluate rule for full line
                bool isHeader = !String.IsNullOrEmpty(score.Headers) && lineNumber == 0;
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
                        lineStyle = m_center.FindStyle(rule.Format) ?? defaultStyle;
                    }
                }
                #endregion

                for (int colIndex = 0; colIndex < row.Length; colIndex++)
                {
                    // ignore controls outside area
                    if (posX > maxX)
                    {
                        //Tools.LogMessage("OverRight X={0} MaxX={1}", posX, maxX);
                        overRight = true;
                        continue;
                    }

                    // get cell
                    string cell = row[colIndex];
                    Tools.ColumnDisplay colSize = GetColumnSize(colIndex, cols, cell, merge);
                    if (colSize.Size == 0)
                    {
                        //Tools.LogMessage("colSize.Size == 0");
                        continue;
                    }

                    // evaluate size of the control in pixel
                    int maxChar = Math.Abs(colSize.Size + 1);
                    int length = charWidth * maxChar;
                    if (posX < startX)
                    {
                        //Tools.LogMessage("PrevPage X={0} MaxX={1}", posX, startX);
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
                            cellStyle = m_center.FindStyle(cellRule.Format) ?? lineStyle;
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
                    //Tools.LogMessage("*** {1}x{2} - CreateControl = {0}", cell, posX, posY);
                    GUIControl control = CreateControl(posX, posY, length, charHeight,
                        colSize.Alignement,
                        cell,
                        fontName, cellStyle, maxChar);

                    // set X pos to the end of the control
                    posX += length;

                    controls.Add(control);
                }

                // set Y pos to the bottom of the control
                posY += charHeight;
            }

            // add controls to screen
            for (int i = 0; i < controls.Count; i++)
            {
                GUIControl c = controls[i];
                c.AllocResources();
                Add(ref c);
            }

            #region Set for Next page
            if (overRight)
            {
                // keep current line
                m_currentColumn += maxX - startX;
                ShowNextButton(true);
            }
            else
            {
                // reset current column
                m_currentColumn = 0;

                if (overDown)
                {
                    m_currentLine = lineNumber - 1;
                    ShowNextButton(true);
                }
                else
                {
                    // no more pages
                    m_currentLine = 0;
                }
            }
            #endregion
        }

        private void GetCharFonSize(int fontSize, ref int width, ref int height)
        {
            float w1 = 0, w2 = 0, h1 = 0, h2 = 0;

            GUIFontManager.MeasureText("_", ref w1, ref h1, fontSize);
            GUIFontManager.MeasureText("__", ref w2, ref h2, fontSize);

            width = (int)(w2 - w1);
            height = (int)h1;
        }

        private static Tools.ColumnDisplay GetColumnSize(int colIndex, Tools.ColumnDisplay[] cols, string text, bool span)
        {
            if (cols == null || span)
                return new Tools.ColumnDisplay(text.Length, GUIControl.Alignment.Left);

            return colIndex < cols.Length ? cols[colIndex] : new Tools.ColumnDisplay("0");
        }

        private GUIControl CreateControl(int posX, int posY, int width, int height,
            GUIControl.Alignment alignement,
            string label, string font, Style style, int nbMax)
        {
            string strLabel = label;

            // always start with a space
            strLabel = " " + label;
            nbMax++;

            // shrink text for small labels
            if (nbMax <= 6 && strLabel.Length > nbMax)
            {
                strLabel = strLabel.Substring(0, nbMax);
            }

            if (alignement == GUIControl.Alignment.Right)
            {
                posX = posX + width;
            }

            // create the control
            GUILabelControl labelControl = new GUILabelControl(GetID);
            labelControl.GetID = m_currentIndex++;
            labelControl._positionX = posX;
            labelControl._positionY = posY;
            labelControl._width = width;
            labelControl._height = height;
            labelControl.FontName = font;
            labelControl.Label = strLabel;
            labelControl.TextColor = style.ForeColor;
            labelControl.TextAlignment = alignement;
            GUIControl control = labelControl as GUIControl;

            m_indices.Add(control.GetID);

            return control;
        }

        private void ShowNextButton(bool visible)
        {
            if (btnNextPage != null)
            {
                btnNextPage.Visibility = visible
                    ? System.Windows.Visibility.Visible
                    : System.Windows.Visibility.Hidden;
            }
        }

        #endregion

        #region Utils
        public void ReadSettings()
        {
            try
            {
                string filename = Config.GetFile(Config.Dir.Config, SettingsFileName);
                m_center = Tools.ReadSettings(filename, false);
            }
            catch (Exception exc)
            {
                GUIControl.SetControlLabel(GetID, 20, exc.Message);
            }
        }

        private void SaveSettings()
        {
            try
            {
                string filename = Config.GetFile(Config.Dir.Config, SettingsFileName);
                Tools.SaveSettings(filename, m_center, true);
            }
            catch (Exception exc)
            {
                GUIControl.SetControlLabel(GetID, 20, exc.Message);
            }
        }

        private string GetImage(string name)
        {
            if (name == " ")
                return "-";

            return Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", name + ".png");
        }

        private string GetCategoryImage(string name)
        {
            CategoryImg img = m_center.FindCategoryImage(name);
            if (img == null)
                return "-";
            return GetImage(img.Path);
        }

        private string GetLeagueImage(string category, string name)
        {
            LeagueImg img = m_center.FindLeagueImage(category, name);
            if (img == null)
                return "-";
            return GetImage(img.Path);
        }

        #endregion

        private class ListComparer : IComparer<GUIListItem>
        {
            private ViewMode m_mode = ViewMode.Category;

            public ListComparer(ViewMode mode)
            {
                m_mode = mode;
            }

            #region IComparer<GUIListItem> Members

            public int Compare(GUIListItem x, GUIListItem y)
            {
                if (m_mode != ViewMode.Results)
                    return String.Compare(x.Label, y.Label);

                Score scx = x.TVTag as Score;
                Score scy = y.TVTag as Score;
                if (scx == null || scy == null)
                    return String.Compare(x.Label, y.Label);

                return scx.CompareTo(scy);
            }

            #endregion
        }
    }
}
