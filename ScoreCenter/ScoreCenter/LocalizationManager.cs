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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Localisation.LanguageStrings;
using MediaPortal.Profile;
using System.Xml.Serialization;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class LocalizationManager
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static LocalizationManager m_instance = new LocalizationManager();
        
        /// <summary>
        /// The provider for strings localisation.
        /// </summary>
        private Localisation.LocalisationProvider m_provider;
        private ScoreLocalisation m_scoreLocaliser = null;

        private LocalizationManager()
        {
            string path = Config.GetSubFolder(Config.Dir.Language, "ScoreCenter");
            if (Directory.Exists(path))
            {
                // get the string localistion provider
                Settings settings = new Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"));
                string language = settings.GetValueAsString("skin", "language", "English");

                CultureInfo[] cultureList = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
                string code = "en";
                foreach (CultureInfo c in cultureList)
                {
                    if (c.EnglishName == language)
                    {
                        code = c.Name;
                        break;
                    }
                }

                m_provider = new MediaPortal.Localisation.LocalisationProvider(path, code, false);

                // read the score localisation XML files
                string file = Path.Combine(path, "score_" + code + ".xml");
                if (File.Exists(file))
                {
                    using (FileStream tr = new FileStream(file, FileMode.Open))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(ScoreLocalisation));
                        m_scoreLocaliser = (ScoreLocalisation)xml.Deserialize(tr);
                    }
                }
            }
        }

        public static string GetString(int id)
        {
            StringLocalised loc = m_instance.m_provider.Get("unmapped", id);
            if (loc == null)
                return "?";
            return loc.text;
        }

        public static string GetScoreString(string id, string defaultValue)
        {
            if (m_instance.m_scoreLocaliser == null) return defaultValue;

            // first check fo id
            LocString loc = m_instance.m_scoreLocaliser.Strings.Where(x => x.id == id).FirstOrDefault();
            if (loc != null)
                return loc.Value;

            // then check for global
            loc = m_instance.m_scoreLocaliser.Globals.Where(x => !x.isRegEx && x.id == defaultValue).FirstOrDefault();
            if (loc != null)
                return loc.Value;

            // regex
            foreach (var rg in m_instance.m_scoreLocaliser.Globals.Where(x => x.isRegEx))
            {
                Regex r = new Regex(rg.id);
                string res = r.Replace(defaultValue, rg.Value);
                if (res != defaultValue)
                    return res;
            }

            return defaultValue;
        }
    }

    public class Labels
    {
        public const int ClearCache = 1;
        public const int UseAutoMode = 2;
        public const int UnuseAutoMode = 3;
        public const int SynchroOnline = 4;
        public const int DisableItem = 5;
        public const int UseAutoWrap = 6;
        public const int UnuseAutoWrap = 7;
        public const int SetAsHome = 8;
        public const int ClearHome = 9;
    }
}
