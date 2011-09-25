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
        public const string LiveSettingsFileName = "MyScoreCenterLive.Settings.xml";
        public const string C_HEADER = "*HEADER*";
        public const string C_EMPTY = "*EMPTY*";

        /// <summary>Start index for GUI controls.</summary>
        private const int StartIndex = 42100;

        private int m_currentLine; // 0
        private int m_currentColumn; // 0
        private BaseScore m_currentScore;
        private string[][] m_lines;
        private Stack<int> m_prevIndex = new Stack<int>();

        private int m_level = 0;
        private bool m_autoSize = false;
        private bool m_autoWrap = false;
        private bool m_liveEnabled = false;
        private string m_livePinImage = Tools.GetThumbs("Misc\\Live");
        private string m_settgins = Config.GetFile(Config.Dir.Config, SettingsFileName);

        private ScoreCenter m_center;

        /// <summary>Group to add the score labels.</summary>
        private GUIGroup m_scoreGroup = null;

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
        //[SkinControlAttribute(60)]
        //protected GUIButtonControl btnNextScore = null;
        //[SkinControlAttribute(70)]
        //protected GUIButtonControl btnPreviousScore = null;

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

        private void SetLiveStatus()
        {
            int nb = 0;
            if (m_center != null)
            {
                nb = m_center.Scores.Items.Where(sc => sc.IsLive()).Count();
            }

            int liveLabel = m_liveEnabled ? Labels.LiveOn : Labels.LiveOff;
            GUIPropertyManager.SetProperty("#ScoreCenter.Live", LocalizationManager.GetString(liveLabel, nb));
        }

        protected override void OnPageLoad()
        {
            //Tools.LogMessage("entering OnPageLoad()");
            base.OnPageLoad();

            m_level = 0;
            m_prevIndex.Clear();
            m_currentScore = null;

            // prepare live parameters
            m_liveEnabled = File.Exists(Config.GetFile(Config.Dir.Config, LiveSettingsFileName));
            string liveSkinIcon = GUIGraphicsContext.Skin + @"\Media\ScoreCenterLive.png";
            if (File.Exists(liveSkinIcon)) m_livePinImage = liveSkinIcon;

            ShowNextButton(false);
            ShowNextPrevScoreButtons(false);
            GUIWaitCursor.Init();
            GUIWaitCursor.Show();
            m_scoreGroup = new GUIGroup(this.GetID);
            m_scoreGroup.WindowId = this.GetID;
            this.Children.Add(m_scoreGroup);

            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                try
                {
                    ReadSettings();
                    ScoreFactory.Instance.CacheExpiration = m_center.Setup.CacheExpiration;
                    GUIPropertyManager.SetProperty("#ScoreCenter.Title", m_center.Setup.Name);
                    SetScoreProperties(null);

                    // update
                    UpdateSettings(false, false);

                    // replace virtual scores
                    m_center.ReplaceVirtualScores();

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
                            m_level = m_center.GetLevel(score);
                            SetScoreProperties(score);
                        }
                    }
                    #endregion

                    SetLiveStatus();
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
            BaseScore itemScore = item.TVTag as BaseScore;

            int menuIndice = 1;

            #region Create Menu
            GUIDialogMenu menu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            menu.Reset();
            menu.SetHeading(m_center.Setup.Name);

            // configure
            menu.Add(LocalizationManager.GetString(Labels.Configuration));
            int menuConfigure = menuIndice++;

            // enable/disable live
            if (m_liveEnabled) menu.Add(LocalizationManager.GetString(Labels.StopLive));
            else menu.Add(LocalizationManager.GetString(Labels.StartLive));
            int menuLive = menuIndice++;

            // clear all live
            int menuClearLive = 0;
            if (!m_liveEnabled)
            {
                menu.Add(LocalizationManager.GetString(Labels.ClearLive));
                menuClearLive = menuIndice++;
            }

            int menuDelete = 0;
            int menuSetHome = 0;
            int menuSetLive = 0;
            if (item.Label != "..")
            {
                // disable
                menu.Add(LocalizationManager.GetString(Labels.DisableItem, item.Label));
                menuDelete = menuIndice++;

                // set home
                if (!itemScore.IsContainer())
                {
                    menu.Add(LocalizationManager.GetString(Labels.SetAsHome));
                    menuSetHome = menuIndice++;
                }

                if (!m_liveEnabled && !item.IsFolder && !itemScore.CannotLive)
                {
                    // set live
                    menu.Add(LocalizationManager.GetString(item.HasPinIcon ? Labels.DisableLive : Labels.ActivateLive, item.Label));
                    menuSetLive = menuIndice++;
                }
            }

            #endregion

            // show the menu
            menu.DoModal(GetID);

            #region process user action
            if (menu.SelectedId == menuLive)
            {
                SetLiveSettings();
            }
            else if (menu.SelectedId == menuClearLive)
            {
                ClearLiveSettings();
            }
            else if (menu.SelectedId == menuConfigure)
            {
                ShowConfigurationMenu();
            }
            else if (menu.SelectedId == menuDelete)
            {
                GUIDialogYesNo dlg = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
                string disable = LocalizationManager.GetString(Labels.DisableItem);
                dlg.SetHeading(disable);
                dlg.SetLine(1, String.Format(CultureInfo.CurrentCulture, "{0} '{1}'?", disable, item.Label));
                dlg.DoModal(GetID);

                if (dlg.IsConfirmed)
                {
                    m_center.DisableScore(itemScore);
                    SaveSettings();
                }
            }
            else if (menu.SelectedId == menuSetHome)
            {
                m_center.Setup.Home = itemScore.Id;
                SaveSettings();
            }
            else if (menu.SelectedId == menuSetLive)
            {
                m_center.SetLiveScore(itemScore, !item.HasPinIcon);
                item.PinImage = item.HasPinIcon ? "" : m_livePinImage;
                SaveSettings();
                SetLiveStatus();
            }
            #endregion

            base.OnShowContextMenu();
        }

        private void ShowConfigurationMenu()
        {
            GUIDialogMenu menu = (GUIDialogMenu)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            menu.Reset();
            menu.SetHeading(LocalizationManager.GetString(Labels.Configuration));

            int menuIndice = 1;

            // synchro
            menu.Add(LocalizationManager.GetString(Labels.SynchroOnline));
            int menuSyncho = menuIndice++;

            // clear cache
            menu.Add(LocalizationManager.GetString(Labels.ClearCache));
            int menuClearCache = menuIndice++;

            // clear home
            menu.Add(LocalizationManager.GetString(Labels.ClearHome));
            int menuHome = menuIndice++;

            // auto mode
            if (m_autoSize) menu.Add(LocalizationManager.GetString(Labels.UnuseAutoMode));
            else menu.Add(LocalizationManager.GetString(Labels.UseAutoMode));
            int menuAutoMode = menuIndice++;

            // auto wrap
            if (m_autoWrap) menu.Add(LocalizationManager.GetString(Labels.UnuseAutoWrap));
            else menu.Add(LocalizationManager.GetString(Labels.UseAutoWrap));
            int menuAutoWrap = menuIndice++;

            menu.DoModal(GetID);

            if (menu.SelectedId == menuClearCache)
            {
                ScoreFactory.Instance.ClearCache();
            }
            else if (menu.SelectedId == menuAutoMode)
            {
                m_autoSize = !m_autoSize;
                ClearGrid();
                CreateGrid(m_lines, m_currentScore, 0, 0);
            }
            else if (menu.SelectedId == menuAutoWrap)
            {
                m_autoWrap = !m_autoWrap;
                ClearGrid();
                CreateGrid(m_lines, m_currentScore, 0, 0);
            }
            else if (menu.SelectedId == menuHome)
            {
                m_center.Setup.Home = "";
                SaveSettings();
            }
            else if (menu.SelectedId == menuSyncho)
            {
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
            }

            base.OnShowContextMenu();
        }

        private void ClearLiveSettings()
        {
            foreach (BaseScore score in m_center.Scores.Items)
            {
                //score.SetLive(false);
                m_center.SetLiveScore(score, false);
            }

            // clear pin icon
            foreach (var ii in lstDetails.ListItems)
            {
                ii.PinImage = "";
            }

            SaveSettings();
            SetLiveStatus();
        }

        private void SetLiveSettings()
        {
            if (m_liveEnabled)
            {
                File.Delete(Config.GetFile(Config.Dir.Config, LiveSettingsFileName));
                m_liveEnabled = false;
                SetLiveStatus();
            }
            else
            {
                // create a settings with all live settings
                var liveList = m_center.Scores.Items.Where(sc => sc.LiveConfig != null && sc.LiveConfig.enabled && sc.IsScore());
                if (liveList.Count() == 0)
                {
                    // no live score configure
                    GUIDialogText ww = (GUIDialogText)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_TEXT);
                    ww.SetHeading("ScoreCenter");
                    ww.SetText(LocalizationManager.GetString(Labels.NoLiveScore));
                    ww.DoModal(GetID);
                }
                else
                {
                    // create new score center
                    ScoreCenter liveExport = new ScoreCenter();
                    liveExport.Parameters = m_center.Parameters;
                    liveExport.Setup = m_center.Setup;
                    liveExport.Styles = m_center.Styles;

                    // clone the scores
                    var ll = new List<BaseScore>(liveList.Count());
                    foreach (BaseScore score in liveList)
                    {
                        BaseScore livescore = score.Clone(score.Id);
                        BaseScore parent = m_center.FindScore(livescore.Parent);
                        if (parent != null)
                        {
                            // use parent icon and name for notification
                            livescore.Name = parent.LocName;
                            livescore.Image = parent.Image;
                        }
                        ll.Add(livescore);

                    }
                    liveExport.Scores = new ScoreCenterScores();
                    liveExport.Scores.Items = ll.ToArray();

                    // create the settings
                    Tools.SaveSettings(Config.GetFile(Config.Dir.Config, LiveSettingsFileName), liveExport, false, true);

                    // update the status
                    m_liveEnabled = true;
                    SetLiveStatus();
                }
            }
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
                    GUIControl.FocusControl(GetID, btnNextPage.GetID);
                }
            }
            //else if (control == btnNextScore || control == btnPreviousScore)
            //{
            //    ClearGrid();
            //    if (m_currentScore != null)
            //    {
            //        int delta = control == btnPreviousScore ? -1 : 1;
            //        //m_currentScore.variable += delta;
            //        DisplayScore();
            //    }
            //}
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
            bool currIsFolder = (m_currentScore == null || m_currentScore.IsContainer());
            m_currentScore = sc;

            bool reselect = true;
            if (back)
            {
                if (!currIsFolder) m_level--;
                m_level--;

                if (lstDetails.Visible)
                {
                    LoadScores(sc);
                }
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

                if (sc.IsContainer())
                {
                    LoadScores(sc);
                }
                else
                {
                    DisplayScore();
                    lstDetails.Visible = (lblVisible == null || !lblVisible.Visible);
                    // pop the score index because it does not need to be reselected
                    m_prevIndex.Pop();
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
                    item.IsFolder = sc.IsContainer();
                    item.IconImage = Tools.GetThumbs(sc.Image);

                    if (sc.IsLive())
                    {
                        item.PinImage = m_livePinImage;
                    }
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
            if (score.IsContainer())
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
            //Tools.LogMessage("SetBackdrop for '{0}'", score == null ? "NULL" : score.Name);
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

            //Tools.LogMessage("==> BD={0}", bd);
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
                    source = source.Replace("https://", String.Empty);
                    source = source.Substring(0, source.IndexOf('/'));
                    if (source.StartsWith("www.")) source = source.Substring(4);
                }
            }

            ClearProperties(m_level);
            GUIPropertyManager.SetProperty("#ScoreCenter.Source", source);

            //Tools.LogMessage("LEVEL = {0}, {1}", m_level, name);
            GUIPropertyManager.SetProperty("#ScoreCenter.Results", m_center.GetFullName(score, " > "));
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
            //Tools.LogMessage(">>> {0}: Icon = {1}", level, score == null ? "" : score.Image);
            BaseScore curr = score;
            for (int i = level; i >= 0; i--)
            {
                if (curr == null)
                    continue;
                string image = Tools.GetThumbs(curr.Image);
                GUIPropertyManager.SetProperty(String.Format("#ScoreCenter.Ico{0}", i), image);
                //Tools.LogMessage(">>>> SetProperties Ico{0} = ", i, image);

                if (i == 1) GUIPropertyManager.SetProperty("#ScoreCenter.CatIco", image);
                else if (i == 2) GUIPropertyManager.SetProperty("#ScoreCenter.LeagueIco", image);
                
                curr = m_center.FindScore(curr.Parent);
            }
        }

        #endregion

        #region Grid management
        /// <summary>
        /// Clears all controls in the grid.
        /// </summary>
        private void ClearGrid()
        {
            m_scoreGroup.Visibility = System.Windows.Visibility.Hidden;
            try
            {
                foreach (GUIControl g in new List<GUIControl>(m_scoreGroup.Children))
                {
                    m_scoreGroup.Children.Remove(g);
                    g.Dispose();
                }
            }
            finally
            {
                m_scoreGroup.Visibility = System.Windows.Visibility.Visible;
            }

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
            bool overRight = false;
            bool overDown = false;
            int lineNumber = 0;
            int colNumber = 0;

            IScoreBuilder<GUIControl> bld = ScoreFactory.Instance.GetBuilder<GUIControl>(score);
            bld.Styles = m_center.Styles.ToList().AsReadOnly();

            GUIFont font = GUIFontManager.GetFont(tbxDetails.FontName);
            int fontSize = font.FontSize;
            int charHeight = 0, charWidth = 0;
            GetCharFonSize(fontSize, ref charWidth, ref charHeight);
            bld.SetFont(tbxDetails.FontName, tbxDetails.TextColor, fontSize, charWidth, charHeight);

            // add controls to screen
            try
            {
                m_scoreGroup.Visibility = System.Windows.Visibility.Hidden;
                IList<GUIControl> controls = bld.Build(score, labels,
                    startLine, startColumn,
                    tbxDetails.XPosition, tbxDetails.YPosition, tbxDetails.Width, tbxDetails.Height,
                    this.CreateControl,
                    out overRight, out overDown, out lineNumber, out colNumber);

                foreach (GUIControl c in controls)
                {
                    c.AllocResources();
                    m_scoreGroup.AddControl(c);
                }
            }
            catch (Exception exc)
            {
                Tools.LogError(">>>>>>> Error in Create Grid", exc);
            }
            finally
            {
                m_scoreGroup.Visibility = System.Windows.Visibility.Visible;
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

        private static void GetCharFonSize(int fontSize, ref int width, ref int height)
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
            GUIControl control = null;
            try
            {
                GUILabelControl labelControl = new GUILabelControl(GetID);

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
            int id = lstDetails.GetID;
            if (btnNextPage != null)
            {
                // set focus if button is currently not visible but will be
                bool focus = !btnNextPage.Visible && visible;
                btnNextPage.Visible = visible;

                if (focus)
                {
                    id = btnNextPage.GetID;
                }
            }

            GUIControl.FocusControl(GetID, id);
        }

        private void ShowNextPrevScoreButtons(bool visible)
        {
            //if (btnNextScore != null && btnPreviousScore != null)
            //{
            //    btnNextScore.Visible = visible;
            //    btnPreviousScore.Visible = visible;
            //}
        }

        #endregion

        #region Utils
        public void ReadSettings()
        {
            try
            {
                m_center = Tools.ReadSettings(m_settgins, false);
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
                Tools.SaveSettings(m_settgins, m_center, true, false);
            }
            catch (Exception exc)
            {
                GUIControl.SetControlLabel(GetID, 20, exc.Message);
            }
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
