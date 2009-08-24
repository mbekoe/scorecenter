using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.ServiceImplementations;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ExchangeManager
    {
        public static void Export(ScoreCenter source, string fileName, List<Score> scores, List<string> styles)
        {
            // create a new object and add the exported scores
            ScoreCenter center = new ScoreCenter();
            center.Scores = new Score[scores.Count];
            scores.CopyTo(center.Scores);

            // add the styles
            center.Styles = new Style[styles.Count];
            int i = 0;
            foreach (string str in styles)
            {
                center.Styles[i++] = source.FindStyle(str);
            }

            Tools.SaveSettings(fileName, center, false);
        }

        public static void Import(ScoreCenter center, string fileName, ImportOptions mergeOptions)
        {
            ScoreCenter imported = Tools.ReadSettings(fileName, true);
            ExchangeManager.Import(center, imported, mergeOptions);
        }

        public static int Import(ScoreCenter center, ScoreCenter imported, ImportOptions mergeOptions)
        {
            int result = 0;
            if (imported.Scores == null)
                return result;

            List<Score> toImport = new List<Score>();
            foreach (Score score in imported.Scores)
            {
                Score exist = center.FindScore(score.Id);
                if (exist == null)
                {
                    if ((mergeOptions & ImportOptions.New) == ImportOptions.New)
                    {
                        toImport.Add(score);
                        result++;
                    }
                }
                else
                {
                    bool merged = exist.Merge(score, mergeOptions);
                    if (merged)
                    {
                        result++;
                    }
                }
            }

            if (toImport.Count > 0)
            {
                Score[] list = new Score[center.Scores.Length + toImport.Count];
                center.Scores.CopyTo(list, 0);

                int i = center.Scores.Length;
                foreach (Score sc in toImport)
                {
                    list[i++] = sc;
                }

                center.Scores = list;
            }

            return result;
        }

        public static bool OnlineUpdate(ScoreCenter center)
        {
            bool result = false;

            if (center.Setup != null
                && center.Setup.UpdateOnline
                && String.IsNullOrEmpty(center.Setup.UpdateUrl) == false)
            {
                ScoreCenter online = Tools.ReadOnlineSettings(center.Setup.UpdateUrl, false);
                if (online != null)
                {
                    ImportOptions options = (ImportOptions)Enum.Parse(typeof(ImportOptions), center.Setup.UpdateRule, true);
                    int nb = ExchangeManager.Import(center, online, options);
                    Tools.LogMessage(" Imported: {0}", nb);
                    result = (nb > 0);
                }
            }

            return result;
        }
    }
}
