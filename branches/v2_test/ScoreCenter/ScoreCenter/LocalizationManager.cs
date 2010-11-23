using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
            if (loc == null)
            {
                // then check for global
                loc = m_instance.m_scoreLocaliser.Globals.Where(x => x.id == defaultValue).FirstOrDefault();
            }

            return (loc == null ? defaultValue : loc.Value);
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
