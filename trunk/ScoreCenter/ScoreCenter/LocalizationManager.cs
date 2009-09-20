using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Configuration;
using MediaPortal.Localisation.LanguageStrings;
using MediaPortal.Profile;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class LocalizationManager
    {
        private static LocalizationManager m_instance = new LocalizationManager();
        private Localisation.LocalisationProvider m_provider;

        private LocalizationManager()
        {
            string path = Config.GetSubFolder(Config.Dir.Language, "ScoreCenter");
            if (Directory.Exists(path))
            {
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
            }
        }

        public static string GetString(int id)
        {
            StringLocalised loc = m_instance.m_provider.Get("unmapped", id);
            if (loc == null)
                return "?";
            return loc.text;
        }
        /*
        public static string GetEnumString(string name, int id)
        {
            StringLocalised loc = m_instance.m_provider.Get(name, id);
            if (loc == null)
                return "?";
            return loc.text;
        }

        public static void GetString(Control control, int id)
        {
            if (m_instance.m_provider != null)
            {
                control.Text = GetString(id);
            }
        }
        public static void GetString(ToolStripButton control, int id)
        {
            if (m_instance.m_provider != null)
            {
                control.ToolTipText = GetString(id);
            }
        }
        public static void GetString(DataGridViewColumn control, int id)
        {
            if (m_instance.m_provider != null)
            {
                control.HeaderText = GetString(id);
            }
        }*/
    }
}
