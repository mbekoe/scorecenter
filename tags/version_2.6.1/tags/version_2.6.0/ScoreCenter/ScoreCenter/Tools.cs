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
using System.Net;
using System.Text;
using System.Xml.Serialization;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace MediaPortal.Plugin.ScoreCenter
{
    public static class Tools
    {
        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <param name="client">The webclient to use.</param>
        /// <param name="url">The URL to sownload.</param>
        /// <returns>The content of the dowloaded file.</returns>
        /// <pparam name="encoding">File encoding.</pparam>
        public static string DownloadFile(WebClient client, string url, string encoding)
        {
            string file;
            Stream response = null;

            try
            {
                Encoding code = GetEncoding(encoding);
                response = client.OpenRead(url);
                StreamReader sr = new StreamReader(response, code);
                file = sr.ReadToEnd();
                sr.Close();
            }
            finally
            {
                if (response != null) response.Close();
            }

            return file;
        }

        /// <summary>
        /// Get the encoding from a name or a code page.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(string encoding)
        {
            Encoding result = Encoding.UTF8;
            if (String.IsNullOrEmpty(encoding))
                return result;

            try
            {
                int code;
                if (int.TryParse(encoding, out code))
                {
                    result = Encoding.GetEncoding(code);
                }
                else
                {
                    result = Encoding.GetEncoding(encoding);
                }
            }
            catch (Exception)
            {
                result = Encoding.UTF8;
            }

            return result;
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="file">The setting file.</param>
        /// <param name="allowException">True to rethrow any exceptions.</param>
        /// <returns>The settings objects.</returns>
        public static ScoreCenter ReadSettings(string file, bool allowException)
        {
            ScoreCenter scores = null;

            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += Tools.CurrentDomain_AssemblyResolve;

                using (FileStream tr = new FileStream(file, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ScoreCenter));
                    scores = (ScoreCenter)xml.Deserialize(tr);
                }

                if (scores.Setup.AutoRefresh == null)
                {
                    scores.Setup.AutoRefresh = new AutoRefreshSettings();
                    scores.Setup.AutoRefresh.Value = 30;
                }
                
                if (scores.Scores != null && scores.Scores.Items != null)
                {
                    foreach (BaseScore s in scores.Scores.Items)
                    {
                        if (String.IsNullOrEmpty(s.Id))
                        {
                            s.Id = GenerateId();
                        }
                    }
                }

                AppDomain.CurrentDomain.AssemblyResolve -= Tools.CurrentDomain_AssemblyResolve;
            }
            catch (Exception)
            {
                if (allowException)
                    throw;
                scores = null;
            }

            return scores;
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="allowException">True to rethrow any exceptions.</param>
        /// <returns>The settings objects.</returns>
        public static ScoreCenter ReadOnlineSettings(string url, bool allowException)
        {
            ScoreCenter scores = null;

            Stream response = null;
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += Tools.CurrentDomain_AssemblyResolve;

                WebClient client = new WebClient();
                response = client.OpenRead(url);
                using (StreamReader sr = new StreamReader(response, Encoding.UTF8))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ScoreCenter));
                    scores = (ScoreCenter)xml.Deserialize(sr);
                }

                foreach (BaseScore s in scores.Scores.Items)
                {
                    if (String.IsNullOrEmpty(s.Id))
                    {
                        s.Id = GenerateId();
                    }
                }

                client.Dispose();
            }
            catch (Exception exc)
            {
                Tools.LogError("Error in ReadOnlineSettings", exc);
                if (allowException)
                    throw;
                scores = null;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= Tools.CurrentDomain_AssemblyResolve;
            }

            return scores;
        }

        /// <summary>
        /// Save the settings to a file.
        /// </summary>
        /// <param name="file">The file tosave to.</param>
        /// <param name="scores">The objects to serialize.</param>
        /// <param name="backup">True to backup the previous file.</param>
        /// <param name="keepVirtual">Keep the virtual scores.</param>
        public static void SaveSettings(string file, ScoreCenter scores, bool backup, bool keepVirtual)
        {
            // backup
            if (backup && File.Exists(file))
            {
                File.Copy(file, file + ".bak", true);
            }

            var originalList = scores.Scores.Items;
            if (!keepVirtual)
            {
                // remove children from virtual scores
                scores.Scores.Items = scores.Scores.Items.Where(p => !p.IsVirtual()).ToArray();
            }

            foreach (BaseScore sc in scores.Scores.Items)
            {
                if (sc.LiveConfig != null)
                {
                    if (sc.LiveConfig.IsNull())
                    {
                        sc.LiveConfig = null;
                    }
                }
            }

            try
            {
                using (TextWriter tw = new StreamWriter(file))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ScoreCenter));
                    xml.Serialize(tw, scores);
                }
            }
            finally
            {
                // restore list
                scores.Scores.Items = originalList;
            }
        }

        /// <summary>
        /// Gets a thumb picture.
        /// </summary>
        /// <param name="name">The thumb relative path.</param>
        /// <returns>The full path to the thumb.</returns>
        public static string GetThumbs(string name)
        {
            if (String.IsNullOrEmpty(name) || name == " ")
                return "-";

            string ext = "";
            if (!name.Contains(".")) ext = ".png";

            return Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", name + ext);
        }

        /// <summary>
        /// Event Handler for AssemblyResolve.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.FullName == args.Name)
                    return asm;
            }
            return null;
        }

        public static string SizesToText(ColumnDisplay[] sizes)
        {
            string txt = String.Empty;

            if (sizes != null)
            {
                foreach (ColumnDisplay col in sizes)
                {
                    if (txt.Length > 0) txt += ",";
                    txt += col.ToString();
                }
            }

            return txt;
        }

        /// <summary>
        /// Table used to transform HTML characters.
        /// </summary>
        private static string[] HtmlCode = new string[] {
            "&nbsp;", " ",// "\n", "",
            "&ccedil;", "ç", "&Ccedil;", "Ç",
            "&auml;", "ä", "&acirc;", "â", "&agrave;", "à", "&aacute;", "á", "&atilde;", "ã",
            "&Auml;", "Ä", "&Acirc;", "Â", "&Agrave;", "À", "&Aacute;", "Á",
            "&euml;", "ë", "&ecirc;", "ê", "&egrave;", "è", "&eacute;", "é",
            "&Euml;", "Ë", "&Ecirc;", "Ê", "&Egrave;", "È", "&Eacute;", "É",
            "&iuml;", "ï", "&icirc;", "î",
            "&Iuml;", "Ï", "&Icirc;", "Î",
            "&ouml;", "ö", "&ocirc;", "ô",
            "&Ouml;", "Ö", "&Ocirc;", "Ô",
            "&uuml;", "ü", "&ucirc;", "û",
            "&Uuml;", "Ü", "&Ucirc;", "Û",
            "&gt;", ">", "&lt;", "<", "&quot;", "\"",
            "&#233;", "é", "&#39;", "'",
        };

        /// <summary>
        /// Transform an HTML string.
        /// </summary>
        /// <param name="value">The HTML to transform.</param>
        /// <param name="allowNewLine">If FALSE replace new lines with a white space.</param>
        /// <returns>The transformed HTML.</returns>
        public static string TransformHtml(string value, bool allowNewLine)
        {
            string result = value.Replace("\t", "  ");

            if (!allowNewLine)
            {
                result = result.Replace("\r", " ");
                result = result.Replace("\n", " ");
            }

            for (int i = 0; i < HtmlCode.Length; i = i + 2)
            {
                result = result.Replace(HtmlCode[i], HtmlCode[i + 1]);
            }

            string[] pp = result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            result = String.Join(" ", pp);

            return result;
        }

        public static string TrimList(string list)
        {
            return TrimList(list, ',');
        }
        public static string TrimList(string list, char sep)
        {
            return String.Join(sep.ToString(), list.Split(sep).Select(f => f.Trim()).ToArray());
        }

        #region Array Processing
        /// <summary>
        /// Add an element to an array.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="tab">The array.</param>
        /// <param name="element">The element to add.</param>
        /// <returns>The new array.</returns>
        public static T[] AddElement<T>(T[] tab, T element)
        {
            T[] newTab = null;
            if (tab == null)
            {
                newTab = new T[1];
                newTab[0] = element;
            }
            else
            {
                newTab = new T[tab.Length + 1];
                tab.CopyTo(newTab, 0);

                newTab[newTab.Length - 1] = element;
            }

            return newTab;
        }

        /// <summary>
        /// Remove an element from an array.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="tab">The array.</param>
        /// <param name="element">The element to remove.</param>
        /// <returns>The new array.</returns>
        public static T[] RemoveElement<T>(T[] tab, T element)
        {
            T[] newTab = new T[tab.Length - 1];

            int i = 0;
            foreach (T elt in tab)
            {
                if (!elt.Equals(element))
                    newTab[i++] = elt;
            }

            return newTab;
        }
        #endregion

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateFileName(string directory, string file, string extension)
        {
            string path;
            int i = 0;
            do
            {
                string name = file;
                if (i > 0) name = file.Insert(file.Length - extension.Length, i.ToString());
                path = Path.Combine(directory, name);
                i++;
            }
            while (File.Exists(path));

            return path;
        }

        public static string GetDomain(string url, string path)
        {
            Uri root = new Uri(url);
            UriBuilder bld = new UriBuilder(root.Host);
            bld.Path = path;

            return bld.Uri.ToString();
        }

        public static int CountLines(string cell)
        {
            int nb = 0;
            int index = 0;
            do
            {
                nb++;
                index = cell.IndexOf(Environment.NewLine, index) + 1;
            }
            while (index > 0 && index < cell.Length);

            return nb;
        }

        #region Log
        public static void LogMessage(string format, params object[] args)
        {
            MediaPortal.ServiceImplementations.Log.Debug("[ScoreCenter] " + format, args);
        }

        public static void LogError(string message, Exception exc)
        {
            MediaPortal.ServiceImplementations.Log.Error("[ScoreCenter] " + message);
            MediaPortal.ServiceImplementations.Log.Error("[ScoreCenter] " + exc);
        }

        public static string GetExceptionMessage(Exception exc)
        {
            if (exc == null) return "?";

            string err = "";
            Exception inner = exc;
            while (inner != null)
            {
                err += inner.Message + Environment.NewLine;
                inner = exc.InnerException;
            }

            // add stack
            err += Environment.NewLine + exc.StackTrace;

            return err;
        }
        #endregion

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

        /// <summary>
        /// Parse the URL to replace variables parts.
        /// </summary>
        /// <param name="url">The URL to parse.</param>
        /// <returns>The parsed URL.</returns>
        public static string ParseUrl(string url, ScoreParameter[] parameters)
        {
            if (url.IndexOf("{") < 0)
                return url;

            string result = url;
            
            // subst special tag
            DateTime now = DateTime.Now;
            if (now.Month < 7) now = now.AddYears(-1);
            DateTime next = now.AddYears(1);
            DateTime prev = now.AddYears(-1);
            result = result.Replace("{YY-YY+1}", String.Format("{0}-{1}", now.ToString("yy"), next.ToString("yy")));
            result = result.Replace("{YYYY-YY+1}", String.Format("{0}-{1}", now.ToString("yyyy"), next.ToString("yy")));
            result = result.Replace("{YYYY-YYYY+1}", String.Format("{0}-{1}", now.ToString("yyyy"), next.ToString("yyyy")));
            result = result.Replace("{YY+1}", String.Format("{0}", next.ToString("yy")));
            result = result.Replace("{YYYY+1}", String.Format("{0}", next.ToString("yyyy")));
            result = result.Replace("{YY-1}", String.Format("{0}", now.ToString("yy")));
            result = result.Replace("{YYYY-1}", String.Format("{0}", now.ToString("yyyy")));

            // subst parameters
            if (parameters != null)
            {
                foreach (ScoreParameter param in parameters)
                {
                    result = result.Replace("{@" + param.name + "}", param.Value);
                }
            }

            // parse date format
            now = DateTime.Now; // reset
            int start, end;
            while ((start = result.IndexOf('{') + 1) > 0)
            {
                end = result.IndexOf('}');
                string format = result.Substring(start, end - start);
                if (format.Length == 1)
                {
                    // if format contains only one char add a space in the format and then remove it
                    result = result.Substring(0, start - 1) + now.ToString(" " + format).Substring(1) + result.Substring(end + 1);
                }
                else
                {
                    result = result.Substring(0, start - 1) + now.ToString(format) + result.Substring(end + 1);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Describes how to display a column (size and alignement).
    /// </summary>
    public class ColumnDisplay
    {
        public enum Alignment
        {
            Center,
            Left,
            Right
        }

        public int Size { get; set; }
        public ColumnDisplay.Alignment Alignement { get; set; }

        public ColumnDisplay(int size, ColumnDisplay.Alignment alignment)
        {
            Size = Math.Abs(size);
            Alignement = alignment;
        }

        public ColumnDisplay(string strCol)
        {
            int c = 0;
            if (false == int.TryParse(strCol.Trim(), out c))
            {
                // default size
                c = 5;
            }

            Size = Math.Abs(c);
            Alignement = ColumnDisplay.Alignment.Right;
            if (strCol.StartsWith("-")) Alignement = ColumnDisplay.Alignment.Left;
            else if (strCol.StartsWith("+")) Alignement = ColumnDisplay.Alignment.Center;
        }

        public override string ToString()
        {
            string txt = String.Empty;
            switch (Alignement)
            {
                case ColumnDisplay.Alignment.Center:
                    txt += "+";
                    break;
                case ColumnDisplay.Alignment.Left:
                    txt += "-";
                    break;
            }

            txt += Size.ToString();
            return txt;
        }

        /// <summary>
        /// Parse a string "1,2,3,4" to a ColumnDisplay array.
        /// </summary>
        /// <param name="size">The string to parse.</param>
        /// <returns>The integer array.</returns>
        public static ColumnDisplay[] GetSizes(string size)
        {
            ColumnDisplay[] sizes = null;
            if (String.IsNullOrEmpty(size) == false)
            {
                string[] elts = size.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                sizes = new ColumnDisplay[elts.Length];
                for (int i = 0; i < elts.Length; i++)
                {
                    sizes[i] = new ColumnDisplay(elts[i]);
                }
            }

            return sizes;
        }
    }
}
