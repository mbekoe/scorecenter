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
using System.Globalization;
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
        private int m_currentLine; // 0
        private int m_currentColumn; // 0
        private Score m_currentScore;
        private string[][] m_lines;
        private Stack<int> m_prevIndex = new Stack<int>();

        private ScoreBuilder<GUIControl> m_builder;

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

        #region Skin Controls
        [SkinControlAttribute(10)]
        protected GUIListControl lstDetails = null;
        [SkinControlAttribute(20)]
        protected GUITextScrollUpControl tbxDetails = null;
        [SkinControlAttribute(30)]
        protected GUIImage imgBackdrop = null;
        [SkinControlAttribute(40)]
        protected GUIButtonControl btnNextPage = null;
        [SkinControlAttribute(50)]
        protected GUILabelControl lblVisible = null;

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
            //Tools.LogMessage("entering OnPageLoad()");
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

                    m_builder = new ScoreBuilder<GUIControl>();
                    m_builder.Center = m_center;

                    GUIFont font = GUIFontManager.GetFont(tbxDetails.FontName);
                    int fontSize = font.FontSize;
                    int charHeight = 0, charWidth = 0;
                    GetCharFonSize(fontSize, ref charWidth, ref charHeight);
                    m_builder.SetFont(tbxDetails.FontName, tbxDetails.TextColor, fontSize, charWidth, charHeight);

                    UpdateSettings(false, false);

                    if (String.IsNullOrEmpty(m_center.Setup.Home))
                    {
                        LoadCategories();
                    }
                    else
                    {
                        string firstId = m_center.Setup.Home;
                        Score score = m_center.Scores.First(s => s.Id == firstId);
                        Tools.LogMessage("/////////// Score = ", score.Name);
                        m_currentCategory = score.Category;
                        SetCategory(score.Category);
                        LoadScores(score.Ligue);
                        DisplayScore(score);
                        m_mode = ViewMode.Results;
                    }

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

            #region create menu
            GUIDialogMenu menu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            menu.Reset();
            menu.SetHeading(m_center.Setup.Name);
            
            // 1: clear cache
            menu.Add(LocalizationManager.GetString(Labels.ClearCache));

            // 2: auto mode
            if (m_builder.AutoSize) menu.Add(LocalizationManager.GetString(Labels.UnuseAutoMode));
            else menu.Add(LocalizationManager.GetString(Labels.UseAutoMode));

            // 3: auto wrap
            if (m_builder.AutoWrap) menu.Add(LocalizationManager.GetString(Labels.UnuseAutoWrap));
            else menu.Add(LocalizationManager.GetString(Labels.UseAutoWrap));

            // 4: synchro
            menu.Add(LocalizationManager.GetString(Labels.SynchroOnline));

            // 5: clear home
            menu.Add(LocalizationManager.GetString(Labels.ClearHome));

            if (item.Label != "..")
            {
                // 6: disable
                menu.Add(String.Format(CultureInfo.CurrentCulture, "{1} '{0}'", item.Label, LocalizationManager.GetString(Labels.DisableItem)));
                
                // 7: set home
                menu.Add(LocalizationManager.GetString(Labels.SetAsHome));
            }
            #endregion

            // show the menu
            menu.DoModal(GetID);
            
            // process user action
            switch (menu.SelectedId)
            {
                case 1:
                    Parser.ClearCache();
                    break;
                case 2:
                    m_builder.AutoSize = !m_builder.AutoSize;
                    ClearGrid();
                    CreateGrid(m_lines, m_currentScore, 0, 0);
                    break;
                case 3:
                    m_builder.AutoWrap = !m_builder.AutoWrap;
                    ClearGrid();
                    CreateGrid(m_lines, m_currentScore, 0, 0);
                    break;
                case 4:
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
                    {
                        try
                        {
                            UpdateSettings(true, true);
                        }
                        catch (Exception ex)
                        {
                            Tools.LogError("Error occured while executing the Online Update: ", ex);
                        }
                        finally
                        {
                            GUIWaitCursor.Hide();
                        }
                    });
                    break;
                case 5:
                    m_center.Setup.Home = "";
                    SaveSettings();
                    break;
                case 6:
                    GUIDialogYesNo dlg = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
                    string disable = LocalizationManager.GetString(Labels.DisableItem);
                    dlg.SetHeading(disable);
                    dlg.SetLine(1, String.Format(CultureInfo.CurrentCulture, "{0} '{1}'?", disable, item.Label));
                    dlg.DoModal(GetID);

                    if (dlg.IsConfirmed)
                    {
                        Disable(item.Label);
                    }
                    break;
                case 7:
                    m_center.Setup.Home = m_currentScore.Id;
                    SaveSettings();
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
                    LoadScores(m_currentLeague);
                    break;
                case ViewMode.League:
                    m_center.Scores.Where(sc => sc.Category == m_currentCategory && sc.Ligue == name)
                        .ForEach(sc => sc.enable = false);
                    LoadLeagues(m_currentCategory);
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

            // clear grid and hide next button
            ShowNextButton(false);
            ClearGrid();

            bool reselect = true;
            switch (m_mode)
            {
                case ViewMode.Category:
                    LoadLeagues(item.Label);
                    imgBackdrop.Refresh();
                    break;
                case ViewMode.League:
                    if (back) LoadCategories();
                    else LoadScores(lstDetails.SelectedListItem.Label);
                    break;
                case ViewMode.Results:
                    if (back)
                    {
                        if (lstDetails.Visible) LoadLeagues(m_currentCategory);
                        else
                        {
                            lstDetails.Visible = true;
                            reselect = false; // no need to reselect
                        }
                    }
                    else
                    {
                        DisplayScore();
                        lstDetails.Visible = (lblVisible == null || !lblVisible.Visible);
                    }
                    
                    break;
            }

            if (reselect && back && m_prevIndex.Count > 0)
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

        private void LoadLeagues(string category)
        {
            m_currentCategory = category;
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

        private void LoadScores(string league)
        {
            m_currentLeague = league;
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

                    url = DisplayScore(score);
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

        private string DisplayScore(Score score)
        {
            string url = score.Url;
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
            return url;
        }

        #endregion

        #region Properties

        private void SetCategory(string name)
        {
            GUIPropertyManager.SetProperty("#ScoreCenter.Category", name);
            GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", GetCategoryImage(name));

            string bd = "";
            if (m_center.Setup != null && !String.IsNullOrEmpty(m_center.Setup.BackdropDir))
            {
                string[] bds = Directory.GetFiles(m_center.Setup.BackdropDir, name + "*.jpg");
                if (bds != null && bds.Length > 0)
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    int index = r.Next(1, bds.Length) - 1;
                    bd = Path.GetFileNameWithoutExtension(bds[index]);
                }

                if (bd.Length == 0)
                {
                    bd = GetDefaultBackdrop();
                }

                if (bd.Length > 0)
                {
                    GUIPropertyManager.SetProperty("#ScoreCenter.bd", Path.Combine(m_center.Setup.BackdropDir, bd));
                    //Tools.LogMessage("BD={0}", bd);
                }
            }
        }

        private string GetDefaultBackdrop()
        {
            string[] def = new string[] { "_", "default" };

            string bd = "";
            int i = 0;
            while (bd.Length == 0 && i < def.Length)
            {
                if (File.Exists(Path.Combine(m_center.Setup.BackdropDir, def[i] + ".jpg")))
                {
                    bd = def[i];
                }

                i++;
            }

            return bd;
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
            m_builder.Score = score;

            bool overRight, overDown;
            int lineNumber, colNumber;
            IList<GUIControl> controls = m_builder.Build(labels,
                startLine, startColumn,
                tbxDetails.XPosition, tbxDetails.YPosition, tbxDetails.Width, tbxDetails.Height, score.ReverseOrder,
                this.CreateControl,
                out overRight, out overDown, out lineNumber, out colNumber);

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
                m_currentColumn += tbxDetails.Width;
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

        private GUIControl CreateControl(int posX, int posY, int width, int height,
            ColumnDisplay.Alignment alignment,
            string label, string font, int fontSize, Style style, int nbMax, int columnIndex)
        {
            // always start with a space
            string strLabel = " " + label;

            // shrink text for small labels
            if (nbMax <= 6 && strLabel.Length > nbMax)
            {
                strLabel = strLabel.Substring(0, nbMax);
            }

            int px = posX;
            if (alignment == ColumnDisplay.Alignment.Right)
            {
                px = posX + width;
            }

            // create the control
            int choice = 0;
            GUIControl control = null;
            if (choice == 0)
            {
                GUILabelControl labelControl = new GUILabelControl(GetID);

                labelControl.GetID = m_currentIndex++;
                labelControl._positionX = px;
                labelControl._positionY = posY;
                labelControl._width = width;
                labelControl._height = height;
                labelControl.FontName = font;
                labelControl.Label = strLabel;
                labelControl.TextColor = style.ForeColor;
                labelControl.TextAlignment = ConvertAlignment(alignment);
                
                control = labelControl;
            }
            else if (choice == 1)
            {
                GUITextControl labelControl = new GUITextControl(GetID, m_currentIndex++,
                    px, posY, width, height,
                    font, 0, 0, "", "", "", "", 0, 0, 0, style.ForeColor);
                labelControl.Label = strLabel;

                control = labelControl;
            }
            else if (choice == 2)
            {
                GUITextScrollUpControl labelControl = new GUITextScrollUpControl(GetID, m_currentIndex++,
                    px, posY, width, height,
                    font, style.ForeColor);
                labelControl.Label = strLabel;

                control = labelControl;
            }
            else if (choice == 3)
            {
                GUIFadeLabel labelControl = new GUIFadeLabel(GetID);

                labelControl.GetID = m_currentIndex++;
                labelControl.XPosition = px;
                labelControl.YPosition = posY;
                labelControl._width = width;
                labelControl._height = height;
                labelControl.FontName = font;
                labelControl.TextColor = style.ForeColor;
                labelControl.TextAlignment = ConvertAlignment(alignment);
                labelControl.AllowScrolling = false;
                labelControl.Label = strLabel;
                labelControl.Visible = true;

                control = labelControl;
            }

            if (control != null)
            {
                m_indices.Add(control.GetID);
            }

            return control;
        }

        private static GUIControl.Alignment ConvertAlignment(ColumnDisplay.Alignment alignment)
        {
            if (alignment == ColumnDisplay.Alignment.Center)
                return GUIControl.Alignment.Center;
            
            if (alignment == ColumnDisplay.Alignment.Left)
                return GUIControl.Alignment.Left;

            return GUIControl.Alignment.Right;
        }

        private void ShowNextButton(bool visible)
        {
            if (btnNextPage != null)
            {
                // set focus if button is currently not visible but will be
                bool focus = !btnNextPage.Visible && visible;
                btnNextPage.Visible = visible;

                if (focus)
                {
                    GUIControl.FocusControl(GetID, btnNextPage.GetID);
                }
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

        private static string GetImage(string name)
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
