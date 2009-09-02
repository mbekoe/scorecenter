﻿using System;
using System.Collections.Generic;
using System.Linq;

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
            Dictionary<string, CategoryImg> importedCategories = new Dictionary<string, CategoryImg>();
            Dictionary<string, LeagueImg> importedLeagues = new Dictionary<string, LeagueImg>();
            foreach (Score score in imported.Scores)
            {
                Score exist = center.FindScore(score.Id);
                if (exist == null)
                {
                    if ((mergeOptions & ImportOptions.New) == ImportOptions.New)
                    {
                        toImport.Add(score);
                        result++;
                        score.SetNew();
                        score.enable = true;

                        if (!importedCategories.ContainsKey(score.Category)
                            && center.FindCategoryImage(score.Category) == null)
                        {
                            CategoryImg img = imported.FindCategoryImage(score.Category);
                            if (img != null)
                            {
                                importedCategories.Add(score.Category, img);
                            }
                        }

                        if (!importedLeagues.ContainsKey(score.LeagueFullName)
                            && center.FindLeagueImage(score.Category, score.Ligue) == null)
                        {
                            LeagueImg img = imported.FindLeagueImage(score.Category, score.Ligue);
                            if (img != null)
                            {
                                importedLeagues.Add(score.LeagueFullName, img);
                            }
                        }
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
                if (center.Scores == null)
                {
                    center.Scores = toImport.ToArray();
                }
                else
                {
                    Score[] list = new Score[center.Scores.Length + toImport.Count];
                    if (center.Scores != null)
                    {
                        center.Scores.CopyTo(list, 0);
                    }

                    int i = center.Scores.Length;
                    foreach (Score sc in toImport)
                    {
                        list[i++] = sc;
                    }

                    center.Scores = list;
                }
            }

            if (center.Images == null)
                center.Images = new ScoreCenterImages();

            if (importedCategories.Count > 0)
            {
                if (center.Images.CategoryImg == null)
                {
                    center.Images.CategoryImg = importedCategories.Values.ToArray();
                }
                else
                {
                    center.Images.CategoryImg = center.Images.CategoryImg.Concat(importedCategories.Values).ToArray();
                }
            }

            if (importedLeagues.Count > 0)
            {
                if (center.Images.LeagueImg == null)
                {
                    center.Images.LeagueImg = importedLeagues.Values.ToArray();
                }
                else
                {
                    center.Images.LeagueImg = center.Images.LeagueImg.Concat(importedLeagues.Values).ToArray();
                }
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

            if (center.Setup.DoUpdate(force)
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
    }
}
