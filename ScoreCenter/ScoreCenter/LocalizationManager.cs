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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using MediaPortal.Configuration;
using MediaPortal.Localisation.LanguageStrings;
using MediaPortal.Profile;

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
        private Localisation.LocalisationProvider m_defaultProvider;
        private ScoreLocalisation m_scoreLocaliser = null;

        private LocalizationManager()
        {
            string path = Config.GetSubFolder(Config.Dir.Language, "ScoreCenter");
            if (Directory.Exists(path))
            {
                // get the string localistion provider
                Settings settings = new Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"));
                string language = settings.GetValueAsString("gui", "language", "English");

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
                m_defaultProvider = new MediaPortal.Localisation.LocalisationProvider(path, "en", false);

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

        public static string GetString(int id, params object[] args)
        {
            StringLocalised loc = m_instance.m_provider.Get("unmapped", id);
            if (loc == null)
            {
                loc = m_instance.m_defaultProvider.Get("unmapped", id);
                if (loc == null)
                    return String.Format("?{0}", id);
            }

            try
            {
                return String.Format(loc.text, args);
            }
            catch (FormatException)
            {
                return loc.text;
            }
        }

        public static string GetScoreString(string id, string defaultValue)
        {
            if (m_instance.m_scoreLocaliser == null) return defaultValue;

            // first check for id
            LocString loc = m_instance.m_scoreLocaliser.Strings.FirstOrDefault(x => x.id == id);
            if (loc != null)
                return loc.Value;

            // then check for global
            loc = m_instance.m_scoreLocaliser.Globals.FirstOrDefault(x => !x.isRegEx && x.id == defaultValue);
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

        public static string[][] LocalizeScore(string[][] score, string dictionaryName)
        {
            if (m_instance.m_scoreLocaliser.ScoreDictionaries == null
                || m_instance.m_scoreLocaliser.ScoreDictionaries.Length == 0 || score == null)
                return score;

            ScoreDictionary dic = m_instance.m_scoreLocaliser.ScoreDictionaries.FirstOrDefault(d => d.name == dictionaryName);
            if (dic == null || dic.LocString == null)
                return score;

            foreach (string[] line in score)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = dic.Translate(line[i]);
                }
            }

            return score;
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
        public const int LiveOn = 10;
        public const int LiveOff = 11;
        public const int DefaultNotifyTitle = 12;
        public const int DisableLive = 13;
        public const int ActivateLive = 14;
        public const int Configuration = 15;
        public const int NoLiveScore = 16;
        public const int StartLive = 17;
        public const int StopLive = 18;
        public const int ClearLive = 19;
    }

    public partial class ScoreDictionary
    {
        public string Translate(string key)
        {
            LocString translation = this.LocString.FirstOrDefault(w => w.id == key && !w.isRegEx);
            if (translation != null)
                return translation.Value;

            // regex
            foreach (var rg in this.LocString.Where(x => x.isRegEx))
            {
                Regex r = new Regex(rg.id);
                string res = r.Replace(key, rg.Value);
                if (res != key)
                    return res;
            }

            return key;
        }
    }
}
