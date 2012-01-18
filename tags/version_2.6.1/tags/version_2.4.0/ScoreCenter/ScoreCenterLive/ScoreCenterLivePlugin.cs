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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugin.ScoreCenter
{
    [PluginIcons(@"ScoreCenterLive.Resources.Live.png",
        @"ScoreCenterLive.Resources.LiveDisabled.png")]
    public class ScoreCenterLivePlugin : ISetupForm, IPlugin
    {
        private LiveCenter m_liveCenter = null;
        private string m_liveSettings = Config.GetFile(Config.Dir.Config, ScoreCenterPlugin.LiveSettingsFileName);

        #region IPlugin Members

        /// <summary>
        /// Start the plugin.
        /// </summary>
        public void Start()
        {
            // create the notification dialog once
            LiveScoreNotifyDialog.CreateNotifyDialog();

            // create a watcher to watch for the live settings file
            FileSystemWatcher fsw = new FileSystemWatcher();
            fsw.Filter = ScoreCenterPlugin.LiveSettingsFileName;
            fsw.Path = Config.GetFolder(Config.Dir.Config);
            fsw.EnableRaisingEvents = true;
            fsw.NotifyFilter = NotifyFilters.FileName;
            fsw.Created += new FileSystemEventHandler(OnLiveSettingsCreated);
            fsw.Deleted += new FileSystemEventHandler(OnLiveSettingsDeleted);
        }

        /// <summary>
        /// Stops the plugin.
        /// </summary>
        public void Stop()
        {
            Tools.LogMessage("Stop ScoreCenterLive");
            StopLiveCenter();
        }

        private void StopLiveCenter()
        {
            Tools.LogMessage("STOPPING LIVE SCORE");
            if (m_liveCenter != null && m_liveCenter.IsBusy)
            {
                m_liveCenter.CancelAsync();
            }

            if (File.Exists(m_liveSettings))
            {
                File.Delete(m_liveSettings);
            }
        }

        private void OnLiveSettingsCreated(object sender, FileSystemEventArgs e)
        {
            Tools.LogMessage("Start ScoreCenterLive");
            System.Threading.Thread.Sleep(1000);
            
            ScoreCenter center = Tools.ReadSettings(m_liveSettings, false);
            if (center == null)
            {
                Tools.LogMessage("NO LIVE Settings: {0}", m_liveSettings);
                return;
            }

            center.Setup.CacheExpiration = 0;
            center.ReplaceVirtualScores();

            var liveScores = center.Scores.Items;
            Tools.LogMessage("Live: nb scores = {0}", liveScores.Count());
            if (liveScores.Count() == 0)
                return;

            m_liveCenter = new LiveCenter(liveScores, center);
            m_liveCenter.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(LiveCenterWorkerCompleted);
            m_liveCenter.RunWorkerAsync();
        }

        private void LiveCenterWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Tools.LogMessage("Live Score Stopped");
            }
            else
            {
                Tools.LogError("****************** Live Stopped on Error", e.Error);
            }
        }

        private void OnLiveSettingsDeleted(object sender, FileSystemEventArgs e)
        {
            StopLiveCenter();
        }

        #endregion

        #region ISetupForm Members

        public string Author()
        {
            return "FredP42";
        }

        public bool CanEnable()
        {
            return true;
        }

        public bool DefaultEnabled()
        {
            return true;
        }

        public string Description()
        {
            return "Keep track of the scores LIVE";
        }

        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = null;
            strButtonImage = null;
            strButtonImageFocus = null;
            strPictureImage = null;
            return false;
        }

        public int GetWindowId()
        {
            return -1;
        }

        public bool HasSetup()
        {
            return false;
        }

        public string PluginName()
        {
            return "ScoreCenterLive";
        }

        public void ShowPlugin()
        {
            //MessageBox.Show("Use ScoreCenter to configure the LIVE scores.");
        }

        #endregion
    }
}
