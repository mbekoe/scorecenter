#region Copyright (C) 2005-2012 Team MediaPortal

/* 
 *      Copyright (C) 2005-2012 Team MediaPortal
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
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Plugin class definition.
    /// </summary>
    [PluginIcons(@"MediaPortal.Plugin.ScoreCenter.Resources.ScoreCenter.png",
        @"MediaPortal.Plugin.ScoreCenter.Resources.ScoreCenterDisabled.png")]
    public partial class ScoreCenterPlugin : ISetupForm, IShowPlugin
    {
        internal const string DefaultPluginName = "My Score Center";
        internal const int PluginId = 42000;

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
            return "Keep track of the scores";
        }

        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
            string name = DefaultPluginName;

            string filename = Config.GetFile(Config.Dir.Config, ScoreCenterPlugin.SettingsFileName);
            ScoreCenter center = Tools.ReadSettings(filename, false);
            if (center.Setup != null)
            {
                name = center.Setup.Name;
            }

            strButtonText = name;
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = "hover_score center.png";
            return true;
        }

        public int GetWindowId()
        {
            return PluginId;
        }

        public bool HasSetup()
        {
            return true;
        }

        public string PluginName()
        {
            string name = DefaultPluginName;
            return name;
        }

        public void ShowPlugin()
        {
            ScoreCenterConfig editor = new ScoreCenterConfig();
            editor.ShowDialog();
        }

        #endregion

        #region IShowPlugin Members

        public bool ShowDefaultHome()
        {
            return true;
        }

        #endregion
    }
}
