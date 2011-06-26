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
using System.Collections;
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
    public partial class ScoreCenterPlugin : GUIWindow
    {
        #region Members
        public const string SkinFileName = "MyScoreCenter.xml";
        public const string SettingsFileName = "MyScoreCenter2.Settings.xml";
        public const string C_HEADER = "*HEADER*";

        /// <summary>Start index for GUI controls.</summary>
        private const int StartIndex = 42100;

        /// <summary>Index of the current GUI control.</summary>
        private int m_currentIndex = StartIndex;

        /// <summary>List of indices of dymacally build controls.</summary>
        private List<int> m_indices = new List<int>();
        private int m_currentLine; // 0
        private int m_currentColumn; // 0
        private BaseScore m_currentScore;
        private string[][] m_lines;
        private Stack<int> m_prevIndex = new Stack<int>();

        private bool m_autoSize = false;
        private bool m_autoWrap = false;

        private ScoreCenter m_center;

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
        [SkinControlAttribute(60)]
        protected GUIButtonControl btnNextScore = null;
        [SkinControlAttribute(70)]
        protected GUIButtonControl btnPreviousScore = null;

        #endregion

        #endregion

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
            ShowNextPrevScoreButtons(false);
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();

            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                try
                {
                    ReadSettings();
                    GUIPropertyManager.SetProperty("#ScoreCenter.Title", m_center.Setup.Name);
                    SetScoreProperties(null);

                    UpdateSettings(false, false);

                    List<BaseScore> vslist = new List<BaseScore>();
                    foreach (BaseScore sc in m_center.Scores.Items.Where(p => p.IsVirtualFolder()))
                    {
                        IList<BaseScore> children = sc.GetVirtualScores();
                        if (children != null)
                        {
                            vslist.AddRange(children);
                        }
                    }

                    m_center.Scores.Items = m_center.Scores.Items.Concat(vslist.ToArray()).ToArray();

                    #region Set HOME
                    if (String.IsNullOrEmpty(m_center.Setup.Home))
                    {
                        LoadScores("");
                    }
                    else
                    {
                        string firstId = m_center.Setup.Home;
                        BaseScore score = m_center.FindScore(firstId);
                        if (score == null)
                        {
                            LoadScores("");
                            m_center.Setup.Home = "";
                        }
                        else
                        {
                            LoadScores(score.Parent);
                            DisplayScore(score);
                        }
                    }
                    #endregion

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
            ClearProperties(0);
            m_center = null;
            ClearGrid();
            base.OnPageDestroy(new_windowId);
        }

        public override void OnAction(MediaPortal.GUI.Library.Action action)
        {
            if (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_PREVIOUS_MENU)
            {
                // back => find parent folder to get the score to display
                GUIListItem item = lstDetails.ListItems.Where(x => x.Label == "..").FirstOrDefault();
                if (item != null)
                {
                    UpdateListView(item, true);
                    return;
                }
            }

            base.OnAction(action);
        }

        protected override void OnShowContextMenu()
        {
            GUIListItem item = lstDetails.SelectedListItem;
            BaseScore sc = item.TVTag as BaseScore;

            #region create menu
            GUIDialogMenu menu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            menu.Reset();
            menu.SetHeading(m_center.Setup.Name);

            // 1: clear cache
            menu.Add(LocalizationManager.GetString(Labels.ClearCache));

            // 2: auto mode
            if (m_autoSize) menu.Add(LocalizationManager.GetString(Labels.UnuseAutoMode));
            else menu.Add(LocalizationManager.GetString(Labels.UseAutoMode));

            // 3: auto wrap
            if (m_autoWrap) menu.Add(LocalizationManager.GetString(Labels.UnuseAutoWrap));
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
                    ScoreFactory.Instance.ClearCache();
                    break;
                case 2:
                    m_autoSize = !m_autoSize;
                    ClearGrid();
                    CreateGrid(m_lines, m_currentScore, 0, 0);
                    break;
                case 3:
                    m_autoWrap = !m_autoWrap;
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
                        m_center.DisableScore(sc);
                        SaveSettings();
                    }
                    break;
                case 7:
                    m_center.Setup.Home = sc.Id;
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
                if (reload) LoadScores("");
            }
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
            else if (control == btnNextScore || control == btnPreviousScore)
            {
                ClearGrid();
                if (m_currentScore != null)
                {
                    int delta = control == btnPreviousScore ? -1 : 1;
                    //m_currentScore.variable += delta;
                    DisplayScore();
                }
            }
            else if (control == lstDetails)
            {
                GUIListItem item = lstDetails.SelectedListItem;
                bool back = (item.Label == "..");
                if (!back)
                {
                    m_prevIndex.Push(lstDetails.SelectedListItemIndex);
                }

                UpdateListView(item, back);
            }

            base.OnClicked(controlId, control, actionType);
        }

        private void UpdateListView(GUIListItem item, bool back)
        {
            //Tools.LogMessage("UpdateListView: {0} back = {1}", item.Label, back);
            m_currentLine = 0;
            m_currentColumn = 0;
            m_lines = null;

            // clear grid and hide next button
            ShowNextButton(false);
            ShowNextPrevScoreButtons(false);
            ClearGrid();

            BaseScore sc = item.TVTag as BaseScore;
            bool currIsFolder = (m_currentScore == null || ScoreIsFolder(m_currentScore));
            m_currentScore = sc;

            bool reselect = true;
            if (back)
            {
                if (!currIsFolder) m_level--;
                m_level--;
                if (lstDetails.Visible) LoadScores(sc);
                else
                {
                    lstDetails.Visible = true;
                    reselect = false; // no need to reselect
                }

                if (reselect && m_prevIndex.Count > 0)
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
            else
            {
                if (currIsFolder) m_level++;
                if (ScoreIsFolder(sc))
                {
                    LoadScores(sc);
                }
                else
                {
                    DisplayScore();
                    lstDetails.Visible = (lblVisible == null || !lblVisible.Visible);
                }
            }

            SetScoreProperties(sc);
        }

        #endregion

        #region Load Selection
        private void LoadScores(string id)
        {
            BaseScore sc = m_center.FindScore(id);
            LoadScores(sc);
        }
        private void LoadScores(BaseScore root)
        {
            //Tools.LogMessage("========= load score {0} level = {1}", root == null ? "NULL" : root.ToString(), m_level);
            lstDetails.Clear();

            if (root == null)
                m_level = 0;
            ClearProperties(m_level);

            string id = root == null ? "" : root.Id;
            if (m_center != null && m_center.Scores != null)
            {
                if (String.IsNullOrEmpty(id) == false)
                {
                    GUIListItem item1 = new GUIListItem();
                    item1.Label = "..";
                    item1.IsFolder = true;
                    item1.TVTag = m_center.FindScore(root.Parent);
                    MediaPortal.Util.Utils.SetDefaultIcons(item1);
                    lstDetails.Add(item1);
                }

                foreach (BaseScore sc in m_center.ReadChildren(id))
                {
                    GUIListItem item = new GUIListItem();
                    item.Label = sc.LocName;
                    item.IsFolder = ScoreIsFolder(sc);
                    item.IconImage = GetImage(sc.Image);
                    item.IsPlayed = sc.IsNew;
                    item.TVTag = sc;

                    lstDetails.Add(item);
                }
            }

            lstDetails.Sort(new ListComparer());
        }

        private void DisplayScore()
        {
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                try
                {
                    if (m_currentScore == null)
                        return;

                    DisplayScore(m_currentScore);
                }
                catch (WebException exc)
                {
                    Tools.LogError("Error in LoadScore", exc);
                    string txt = "Address not found:" + Environment.NewLine + m_currentScore.GetSource();
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

        private void DisplayScore(BaseScore score)
        {
            if (ScoreIsFolder(score))
                return;

            string[][] results = ScoreFactory.Parse(score, false, m_center.Parameters);
            m_currentLine = 0;
            m_currentColumn = 0;
            m_lines = results;
            ShowNextButton(false);
            //ShowNextPrevScoreButtons(score.Url.Contains("{"));

            CreateGrid(results, score, 0, 0);
        }

        #endregion

        #region Properties

        private Random m_randomizer = new Random(DateTime.Now.Millisecond);
        private string FindBackdrop(BaseScore score)
        {
            Tools.LogMessage("SetBackdrop for '{0}'", score == null ? "NULL" : score.Name);
            string bd = "";
            if (score != null)
            {
                if (m_center.Setup != null && !String.IsNullOrEmpty(m_center.Setup.BackdropDir))
                {
                    string name = Path.Combine(m_center.Setup.BackdropDir, m_center.GetFullName(score, ".")); // score.Name;
                    var bds = Directory.GetFiles(m_center.Setup.BackdropDir, "*.jpg").Where(x => x.StartsWith(name));
                    if (bds != null && bds.Count() > 0)
                    {
                        int index = m_randomizer.Next(1, bds.Count()) - 1;
                        bd = Path.GetFileNameWithoutExtension(bds.ElementAt(index));
                    }
                }

                if (bd.Length == 0)
                    bd = FindBackdrop(m_center.FindScore(score.Parent));
            }
            
            return bd;
        }

        private void SetBackdrop(BaseScore score)
        {
            string bd = FindBackdrop(score);
            if (bd.Length == 0)
            {
                bd = GetDefaultBackdrop();
            }

            Tools.LogMessage("==> BD={0}", bd);
            GUIPropertyManager.SetProperty("#ScoreCenter.bd", Path.Combine(m_center.Setup.BackdropDir, bd));
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

        private int m_level = 0;
        private void SetScoreProperties(BaseScore score)
        {
            string name = " ";
            string image = "-";
            string source = " ";

            if (score == null)
            {
                m_level = 0;
            }
            else
            {
                name = score.LocName;
                image = score.Image;

                source = score.GetSource();
                if (String.IsNullOrEmpty(source) == false)
                {
                    source = Tools.ParseUrl(source, m_center.Parameters);
                    source = source.Replace("http://", String.Empty);
                    source = source.Substring(0, source.IndexOf('/'));
                    if (source.StartsWith("www.")) source = source.Substring(4);
                }
            }

            ClearProperties(m_level);
            GUIPropertyManager.SetProperty("#ScoreCenter.Source", source);

            //Tools.LogMessage("LEVEL = {0}, {1}", m_level, name);
            GUIPropertyManager.SetProperty("#ScoreCenter.Results", m_center.GetFullName(score, " > "));
            //SetProperties(m_level, name, GetImage(image));
            SetIcons(score, m_level);
            SetBackdrop(score);
        }

        private void ClearProperties(int start)
        {
            GUIPropertyManager.SetProperty("#ScoreCenter.Category", "");
            GUIPropertyManager.SetProperty("#ScoreCenter.League", "");
            GUIPropertyManager.SetProperty("#ScoreCenter.Results", "");
            GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", "-");
            GUIPropertyManager.SetProperty("#ScoreCenter.LIco", "-");
            for (int i = start; i < 10; i++)
            {
                GUIPropertyManager.SetProperty(String.Format("#ScoreCenter.Ico{0}", i), "-");
            }
        }

        private void SetIcons(BaseScore score, int level)
        {
            BaseScore curr = score;
            for (int i = level; i >= 0; i--)
            {
                if (curr == null)
                    continue;
                string image = GetImage(curr.Image);
                GUIPropertyManager.SetProperty(String.Format("#ScoreCenter.Ico{0}", i), image);

                if (i == 0) GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", image);
                else if (i == 1) GUIPropertyManager.SetProperty("#ScoreCenter.LeagueIco", image);
                
                curr = m_center.FindScore(curr.Parent);
            }
        }

        private void SetProperties(int level, string name, string image)
        {
            //Tools.LogMessage("Set Prop = {0}", String.Format("#ScoreCenter.Label{0}", level));
            //GUIPropertyManager.SetProperty(String.Format("#ScoreCenter.Label{0}", level), name);
            GUIPropertyManager.SetProperty(String.Format("#ScoreCenter.Ico{0}", level), image);

            if (level == 1)
            {
                GUIPropertyManager.SetProperty("#ScoreCenter.Category", name);
                GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", image);
            }
            else if (level == 2)
            {
                GUIPropertyManager.SetProperty("#ScoreCenter.League", name);
                GUIPropertyManager.SetProperty("#ScoreCenter.LIco", image);
            }
            else if (level > 2)
            {
                //GUIPropertyManager.SetProperty("#ScoreCenter.Results", name);
                GUIPropertyManager.SetProperty("#ScoreCenter.ScoreIco", image);
            }
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
        private void CreateGrid(string[][] labels, BaseScore score, int startLine, int startColumn)
        {
            bool overRight, overDown;
            int lineNumber, colNumber;

            IScoreBuilder<GUIControl> bld = ScoreFactory.Instance.GetBuilder<GUIControl>(score);
            bld.Styles = m_center.Styles.ToList().AsReadOnly();

            GUIFont font = GUIFontManager.GetFont(tbxDetails.FontName);
            int fontSize = font.FontSize;
            int charHeight = 0, charWidth = 0;
            GetCharFonSize(fontSize, ref charWidth, ref charHeight);
            bld.SetFont(tbxDetails.FontName, tbxDetails.TextColor, fontSize, charWidth, charHeight);

            IList<GUIControl> controls = bld.Build(score, labels,
                startLine, startColumn,
                tbxDetails.XPosition, tbxDetails.YPosition, tbxDetails.Width, tbxDetails.Height,
                this.CreateControl,
                out overRight, out overDown, out lineNumber, out colNumber);

            // add controls to screen
            for (int i = 0; i < controls.Count; i++)
            {
                GUIControl c = controls[i] as GUIControl;
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
            try
            {
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
            }
            catch (Exception exc)
            {
                Tools.LogError(String.Format("bad {0}", strLabel), exc);
                control = null;
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

        private void ShowNextPrevScoreButtons(bool visible)
        {
            if (btnNextScore != null && btnPreviousScore != null)
            {
                btnNextScore.Visible = visible;
                btnPreviousScore.Visible = visible;
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
            if (String.IsNullOrEmpty(name) || name == " ")
                return "-";

            string ext = "";
            if (!name.Contains(".")) ext = ".png";

            return Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", name + ext);
        }

        private static bool ScoreIsFolder(BaseScore score)
        {
            return score.IsFolder() || score.IsVirtualFolder();
        }

        #endregion

        private class ListComparer : IComparer<GUIListItem>
        {
            public ListComparer()
            {
            }

            #region IComparer<GUIListItem> Members

            public int Compare(GUIListItem x, GUIListItem y)
            {
                int res = 0;
                if (x.Label == ".." && y.Label == "..")
                    res = 0;
                else if (x.Label == "..")
                    res = -1;
                else if (y.Label == "..")
                    res = 1;
                else
                {
                    BaseScore scx = x.TVTag as BaseScore;
                    BaseScore scy = y.TVTag as BaseScore;
                    if (scx == null || scy == null)
                        res = String.Compare(x.Label, y.Label);
                    else
                        res = scx.CompareTo(scy);
                }

                return res;
            }

            #endregion
        }
    }
}
