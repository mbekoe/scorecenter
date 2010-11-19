using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Ionic.Zip;
using MediaPortal.Configuration;

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

        /// <summary>
        /// Import a list of scores in an existing center.
        /// </summary>
        /// <param name="center">The existing center.</param>
        /// <param name="imported">The center to import.</param>
        /// <param name="mergeOptions">The merge options.</param>
        /// <returns>The number of scores imported.</returns>
        public static int Import(ScoreCenter center, ScoreCenter imported, ImportOptions mergeOptions)
        {
            int result = 0;
            if (imported.Scores == null)
                return result;

            List<Score> toImport = new List<Score>();
            foreach (Score score in imported.Scores)
            {
                Score exist = center.FindScore(score.Id);
                if (exist != null)
                {
                    // merge (or not)
                    bool merged = exist.Merge(score, mergeOptions);
                    if (merged)
                    {
                        result++;
                    }
                }
                else
                {
                    // import new
                    if ((mergeOptions & ImportOptions.New) == ImportOptions.New)
                    {
                        toImport.Add(score);
                        result++;
                        score.SetNew();
                        score.enable = true;
                    }
                }
            }

            if (toImport.Count > 0)
            {
                if (center.Scores == null)
                {
                    center.Scores = toImport.ToArray();
                }
                else
                {
                    //Score[] list = new Score[center.Scores.Length + toImport.Count];
                    //center.Scores.CopyTo(list, 0);

                    //int i = center.Scores.Length;
                    //foreach (Score sc in toImport)
                    //{
                    //    list[i++] = sc;
                    //}

                    //center.Scores = list;
                    center.Scores = center.Scores.Concat(toImport).ToArray();
                }
            }

            return result;
        }

        public static bool OnlineUpdate(ScoreCenter center, bool force)
        {
            bool result = false;

            if (center.Setup == null)
                return false;

            Tools.LogMessage("Start Online Update");
            Tools.LogMessage("        Mode: {0}", center.Setup.UpdateOnlineMode);
            Tools.LogMessage("        URL: {0}", center.Setup.UpdateUrl);
            Tools.LogMessage("        Options: {0}", center.Setup.UpdateRule);
            Tools.LogMessage("        Force: {0}", force);

            if (center.Setup.DoUpdate(force, center.Scores == null ? 0 : center.Scores.Length)
                && String.IsNullOrEmpty(center.Setup.UpdateUrl) == false)
            {
                ImportOptions options = (ImportOptions)Enum.Parse(typeof(ImportOptions), center.Setup.UpdateRule, true);
                result = OnlineUpdate(center, center.Setup.UpdateUrl, options);
                Tools.LogMessage("End Update: {0}", result);
            }
            else
            {
                Tools.LogMessage("End Update: Cancelled");
            }

            return result;
        }

        public static bool OnlineUpdate(ScoreCenter center, string url, ImportOptions options)
        {
            bool result = false;

            if (String.IsNullOrEmpty(url) == false)
            {
                ScoreCenter online = Tools.ReadOnlineSettings(url, false);
                if (online != null)
                {
                    int nb = ExchangeManager.Import(center, online, options);
                    Tools.LogMessage("Imported: {0}", nb);
                    result = (nb > 0);

                    // if scores imported
                    if (result)
                    {
                        // check icons
                        UpdateIcons(center);
                    }
                }
            }

            return result;
        }

        private static void UpdateIcons(ScoreCenter center)
        {
            // create list including all icons
            List<string> icons = new List<string>();

            icons.AddRange(center.Scores.Select(sc => sc.Image));

            // check if an icon is missing
            bool missing = false;
            foreach (string image in icons.Distinct())
            {
                string fileName = Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", image + ".png");
                if (String.IsNullOrEmpty(image) == false && File.Exists(fileName) == false)
                {
                    missing = true;
                    break;
                }
            }

            if (missing)
            {
                // DL zip
                string url = center.Setup.UpdateUrl;
                url = url.Substring(0, url.LastIndexOf("/") + 1) + "ScoreCenter.zip";
                string zipFileName = Config.GetFile(Config.Dir.Thumbs, "ScoreCenter.zip");
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(url, zipFileName);
                        ReadZip(zipFileName, center.OverrideIcons());
                    }
                }
                finally
                {
                    File.Delete(zipFileName);
                }
            }
        }

        /// <summary>
        /// Extract all missing images from the zip file.
        /// </summary>
        /// <param name="zipFileName">The full path of the zip file.</param>
        private static void ReadZip(string zipFileName, bool overwrite)
        {
            string dest = Config.GetFolder(Config.Dir.Thumbs);
            using (ZipFile zip = ZipFile.Read(zipFileName))
            {
                foreach (ZipEntry entry in zip)
                {
                    try
                    {
                        string path = Path.Combine(dest, entry.FileName);
                        bool extract = true;
                        if (File.Exists(path))
                        {
                            if (overwrite) File.Delete(path);
                            else extract = false;
                        }

                        if (extract)
                            entry.Extract(dest);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
