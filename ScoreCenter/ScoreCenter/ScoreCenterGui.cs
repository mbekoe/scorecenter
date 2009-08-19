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
            LogMessage("entering OnPageLoad()");
            base.OnPageLoad();

            try
            {
                LogMessage("showing wait cursor");
                GUIWaitCursor.Init();
                GUIWaitCursor.Show();

                System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
                {
                    ReadSettings();

                    GUIPropertyManager.SetProperty("#ScoreCenter.Title", m_center.Setup.Name);
                    GUIPropertyManager.SetProperty("#ScoreCenter.Category", "_");
                    GUIPropertyManager.SetProperty("#ScoreCenter.League", " ");
                    GUIPropertyManager.SetProperty("#ScoreCenter.Results", " ");

                    LoadCategories();
                });
            }
            catch (Exception ex)
            {
                LogError("Error occured while executing the OnPageLoad: ", ex);
            }
            finally
            {
                GUIWaitCursor.Hide();
            }
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
            menu.SetHeading(String.Format("Disable {0}", item.Label));
            menu.Add("Clear Cache");

            if (m_autoSize)
                menu.Add("Disable Auto Resize Columns Mode");
            else
                menu.Add("Use Auto Resize Columns Mode");

            if (item.Label != "..")
            {
                menu.Add(String.Format("Hide '{0}'", item.Label));
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
                    GUIDialogYesNo dlg = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
                    dlg.SetHeading("Disable");
                    dlg.SetLine(1, String.Format("Hide '{0}'?", item.Label));
                    dlg.DoModal(GetID);

                    if (dlg.IsConfirmed)
                    {
                        Disable(item.Label);
                    }
                    break;
            }


            base.OnShowContextMenu();
        }

        private void Disable(string name)
        {
            switch (m_mode)
            {
                case ViewMode.Results:
                    foreach (Score score in m_center.Scores)
                    {
                        if (score.Category == m_currentCategory
                            && score.Ligue == m_currentLeague
                            && score.Name == name)
                            score.enable = false;
                    }
                    LoadScores();
                    break;
                case ViewMode.League:
                    foreach (Score score in m_center.Scores)
                    {
                        if (score.Category == m_currentCategory
                            && score.Ligue == name)
                            score.enable = false;
                    }
                    LoadLeagues();
                    break;
                case ViewMode.Category:
                    foreach (Score score in m_center.Scores)
                    {
                        if (score.Category == name)
                            score.enable = false;
                    }
                    LoadCategories();
                    break;
            }

            SaveSettings();
        }

        protected override void OnClicked(int controlId, GUIControl control,
            MediaPortal.GUI.Library.Action.ActionType actionType)
        {
            if (control == lstDetails)
            {
                GUIListItem item = lstDetails.SelectedListItem;
                bool back = (item.Label == "..");
                UpdateListView(item, back);
            }

            base.OnClicked(controlId, control, actionType);
        }

        private void UpdateListView(GUIListItem item, bool back)
        {
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
                    else LoadScore();
                    break;
            }
        }

        #endregion
        
        private void LoadCategories()
        {
            lstDetails.Clear();
            if (m_center != null && m_center.Scores != null)
            {
                SetCategory("_");

                List<string> categories = new List<string>();
                foreach (Score score in m_center.Scores)
                {
                    if (score.enable == false)
                        continue;
                    
                    if (categories.Contains(score.Category) == false)
                    {
                        GUIListItem item = new GUIListItem();
                        item.Label = score.Category;
                        item.IsFolder = true;
                        item.IconImage = GetCategoryImage(score.Category);

                        lstDetails.ListItems.Add(item);
                        categories.Add(score.Category);
                    }
                }
            }

            lstDetails.Sort(new ListComparer());
            m_mode = ViewMode.Category;
        }

        private void LoadLeagues()
        {
            string category = m_currentCategory;
            SetCategory(category);
            SetLeague(" ");
            SetScore(null);

            lstDetails.Clear();
            List<string> leagues = new List<string>();

            GUIListItem item1 = new GUIListItem();
            item1.Label = "..";
            item1.IsFolder = true;
            MediaPortal.Util.Utils.SetDefaultIcons(item1);
            lstDetails.Add(item1);

            foreach (Score score in m_center.Scores)
            {
                if (score.enable == false)
                    continue;

                if (score.Category == category
                    && leagues.Contains(score.Ligue) == false)
                {
                    GUIListItem item = new GUIListItem();
                    item.Label = score.Ligue;
                    item.IsFolder = true;
                    item.IconImage = GetLeagueImage(score.Category, score.Ligue);

                    lstDetails.ListItems.Add(item);
                    leagues.Add(score.Ligue);
                }
            }

            lstDetails.Sort(new ListComparer());
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

                if (score.Ligue == m_currentLeague)
                {
                    GUIListItem item = new GUIListItem();
                    item.Label = score.Name;
                    item.IsFolder = false;
                    item.IconImage = GetImage(score.Image);
                    item.TVTag = score;
                    lstDetails.ListItems.Add(item);
                }
            }

            lstDetails.Sort(new ListComparer());
            m_mode = ViewMode.Results;
        }
        
        private void LoadScore()
        {
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                try
                {
                    GUIListItem item = lstDetails.SelectedListItem;
                    Score score = item.TVTag as Score;
                    if (score == null)
                        return;

                    LogMessage("ShowScore: Url={0}", score.Url);
                    LogMessage("ShowScore: XPath={0}", score.XPath);

                    string[][] results = Parser.Read(score, false);
                    SetScore(score);
                    CreateGrid(results, score);
                }
                catch (Exception exc)
                {
                    LogError("Error in LoadScore", exc);
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

        #region Properties

        private void SetCategory(string name)
        {
            GUIPropertyManager.SetProperty("#ScoreCenter.Category", name);
            GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", GetCategoryImage(name));

            string bd = "-";
            if (m_center.Setup != null && !String.IsNullOrEmpty(m_center.Setup.BackdropDir))
            {
                bd = Path.Combine(m_center.Setup.BackdropDir, name);
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
            if (score != null)
            {
                name = score.Name;
                image = score.Image;
            }
            
            GUIPropertyManager.SetProperty("#ScoreCenter.Results", name);
            GUIPropertyManager.SetProperty("#ScoreCenter.ScoreIco", GetImage(image));
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
        /// <param name="score"></param>
        private void CreateGrid(string[][] labels, Score score)
        {
            int startX = tbxDetails.XPosition;
            int maxX = startX + tbxDetails.Width;
            int maxY = tbxDetails.YPosition + tbxDetails.Height;
            int posX = startX;
            int posY = tbxDetails.YPosition;
            string fontName = tbxDetails.FontName;
            Style defaultStyle = new Style();
            defaultStyle.ForeColor = tbxDetails.TextColor;

            GUIFont font = GUIFontManager.GetFont(fontName);
            int fontSize = font.FontSize;
            int charHeight = 0, charWidth = 0;
            GetCharFonSize(fontSize, ref charWidth, ref charHeight);

            #region Get Columns Sizes
            int[] cols = null;
            if (!m_autoSize)
            {
                cols = Tools.GetSizes(score.Sizes);
                LogMessage("Sizes: {0}", score.Sizes);
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

                cols = new int[coldic.Count];
                foreach (int key in coldic.Keys)
                {
                    cols[key] = coldic[key];
                }
            }
            #endregion

            RuleEvaluator engine = new RuleEvaluator(score.Rules, m_center);

            int lineNumber = -1;
            foreach (string[] row in labels)
            {
                if (row == null)
                    continue;

                posX = startX;
                lineNumber++;
                bool isHeader = !String.IsNullOrEmpty(score.Headers) && lineNumber == 0;

                // set style for the line
                Style lineStyle = defaultStyle;
                if (!isHeader)
                {
                    lineStyle = engine.CheckLine(row, lineNumber) ?? defaultStyle;
                }

                for (int colIndex = 0; colIndex < row.Length; colIndex++)
                {
                    // ignore controls outside area
                    if (posX > maxX || posY > maxY)
                        continue;

                    string cell = row[colIndex];

                    // size of the columns (can be < 0)
                    int colSize = GetColumnSize(colIndex, cols, cell);
                    if (colSize == 0)
                        continue;

                    GUIControl.Alignment alignement = colSize > 0 ? GUIControl.Alignment.Right : GUIControl.Alignment.Left;

                    // evaluate size of the control in pixel
                    int maxChar = Math.Abs(colSize + 1);
                    int length = charWidth * maxChar;

                    // set style for the cell
                    Style cellStyle = lineStyle;
                    if (!isHeader)
                    {
                        cellStyle = engine.CheckCell(cell, colIndex) ?? lineStyle;
                    }

                    //LogMessage("Label: {0}, {1}, {2}, {3}", cell, colSize, length, posX);
                    GUIControl control = CreateControl(posX, posY, length, charHeight, alignement,
                        cell, fontName, cellStyle, maxChar);

                    posX += length;
                }

                posY += charHeight;
            }
        }

        private void GetCharFonSize(int fontSize, ref int width, ref int height)
        {
            float w1 = 0, w2 = 0, h1 = 0, h2 = 0;

            GUIFontManager.MeasureText("_", ref w1, ref h1, fontSize);
            GUIFontManager.MeasureText("__", ref w2, ref h2, fontSize);

            width = (int)(w2 - w1);
            height = (int)h1;
        }

        private static int GetColumnSize(int colIndex, int[] cols, string text)
        {
            if (cols == null)
                return -text.Length;

            return colIndex < cols.Length ? cols[colIndex] : 0;
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

            /*GUILabelControl labelControl = new GUILabelControl(GetID, m_currentIndex++,
                x, y, w, h, font,
                strLabel, color, alignement, true);*/
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

            control.AllocResources();
            Add(ref control);

            return control;
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
            if (name == " ")
                return "-";

            if (m_center.Images != null && m_center.Images.CategoryImg != null)
            {
                foreach (CategoryImg img in m_center.Images.CategoryImg)
                {
                    if (String.Compare(name, img.Name, true) == 0)
                    {
                        return GetImage(img.Path);
                    }
                }
            }

            return "-";
        }
        
        private string GetLeagueImage(string category, string name)
        {
            if (name == " ")
                return "-";

            if (m_center.Images != null && m_center.Images.LeagueImg != null)
            {
                foreach (LeagueImg img in m_center.Images.LeagueImg)
                {
                    if (String.Compare(category, img.Category, true) == 0
                        && String.Compare(name, img.Name, true) == 0)
                    {
                        return GetImage(img.Path);
                    }
                }
            }

            return "-";
        }
        #endregion

        #region Log
        private void LogMessage(string format, params object[] args)
        {
            Log.Debug(format, args);
        }

        private void LogError(string message, Exception exc)
        {
            Log.Error(message);
            Log.Error(exc);
        }
        #endregion

        private class ListComparer : IComparer<GUIListItem>
        {
            #region IComparer<GUIListItem> Members

            public int Compare(GUIListItem x, GUIListItem y)
            {
                return String.Compare(x.Label, y.Label);
            }

            #endregion
        }
    }
}
