#region

/* 
 *      Copyright (C) 2009-2014 Team MediaPortal
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
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Plugin.ScoreCenter;

namespace MediaPortal.Dialogs
{
    /// <summary>
    /// Notification Dialog for Score changed.
    /// </summary>
    public class LiveScoreNotifyDialog : GUIDialogNotify
    {
        public const int SCORE_NOTIFY_DIALOG_ID = 42010;

        /// <summary>
        /// Default Icon.
        /// </summary>
        private string m_defaultIcon = Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", "Standings.png");

        /// <summary>The sound to play.</summary>
        private string m_sound = "notify.wav";

        /// <summary>Enables or Disables Play Sound.</summary>
        public bool PlaySound { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LiveScoreNotifyDialog()
        {
            GetID = SCORE_NOTIFY_DIALOG_ID;

            this.PlaySound = true;
            string sound = GUIGraphicsContext.Skin + @"\Sounds\notifyscore.wav";
            if (File.Exists(sound)) m_sound = sound;
        }
        
        public override bool Init()
        {
            string skin = GUIGraphicsContext.Skin + @"\MyScoreCenterNotify.xml";
            if (File.Exists(skin))
                return Load(skin);
            return base.Init();
        }

        public static void CreateNotifyDialog()
        {
            GUIWindow dlg = new LiveScoreNotifyDialog();
            dlg.Init();
            GUIWindowManager.Add(ref dlg);
        }

        /// <summary>
        /// Set the dialog score properties.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="icon"></param>
        /// <param name="text"></param>
        public void SetScore(string title, string icon, string text)
        {
            if (String.IsNullOrEmpty(title))
                this.SetHeading(LocalizationManager.GetString(Labels.DefaultNotifyTitle));
            else
                this.SetHeading(title);

            if (String.IsNullOrEmpty(icon))
                this.SetImage(m_defaultIcon);
            else
                this.SetImage(Tools.GetThumbs(icon));

            this.SetText(text + Environment.NewLine);
        }

        protected override void OnPageLoad()
        {
            base.OnPageLoad();

            if (this.PlaySound)
            {
                MediaPortal.Util.Utils.PlaySound(m_sound, false, true);
            }
        }
    }
}
