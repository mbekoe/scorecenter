#region Copyright (C) 2005-2009 Team MediaPortal

/* 
 *      Copyright (C) 2005-2009 Team MediaPortal
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
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class Tools
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

                foreach (Score s in scores.Scores)
                {
                    if (String.IsNullOrEmpty(s.Id))
                    {
                        s.Id = GenerateId();
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
        /// Save the settings to a file.
        /// </summary>
        /// <param name="file">The file tosave to.</param>
        /// <param name="scores">The objects to serialize.</param>
        /// <param name="backup">True to backup the previous file.</param>
        public static void SaveSettings(string file, ScoreCenter scores, bool backup)
        {
            // backup
            if (backup && File.Exists(file))
            {
                File.Copy(file, file + ".bak", true);
            }

            using (TextWriter tw = new StreamWriter(file))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ScoreCenter));
                xml.Serialize(tw, scores);
            }
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

        /// <summary>
        /// Parse a string "1,2,3,4" to an integer array.
        /// </summary>
        /// <param name="size">The string to parse.</param>
        /// <returns>The integer array.</returns>
        public static int[] GetSizes(string size)
        {
            int[] sizes = null;
            if (String.IsNullOrEmpty(size) == false)
            {
                string[] elts = size.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                sizes = new int[elts.Length];
                for (int i = 0; i < elts.Length; i++)
                {
                    if (false == int.TryParse(elts[i].Trim(), out sizes[i]))
                    {
                        sizes[i] = 5;
                    }
                }
            }

            return sizes;
        }

        /// <summary>
        /// Table used to transform HTML characters.
        /// </summary>
        private static string[] HtmlCode = new string[] {
            "&nbsp;", " ", "\n", "",
            "&ccedil;", "ç", "&Ccedil;", "Ç",
            "&auml;", "ä", "&acirc;", "â", "&agrave;", "à", "&aacute;", "á",
            "&Auml;", "Ä", "&Acirc;", "Â", "&Agrave;", "À", "&Aacute;", "Á",
            "&euml;", "ë", "&ecirc;", "ê", "&egrave;", "è", "&eacute;", "é",
            "&Euml;", "Ë", "&Ecirc;", "Ê", "&Egrave;", "È", "&Eacute;", "É",
            "&iuml;", "ï", "&icirc;", "î",
            "&Iuml;", "Ï", "&Icirc;", "Î",
            "&ouml;", "ö", "&ocirc;", "ô",
            "&Ouml;", "Ö", "&Ocirc;", "Ô",
            "&uuml;", "ü", "&ucirc;", "û",
            "&Uuml;", "Ü", "&Ucirc;", "Û",
            "&gt;", ">", "&lt;", "<"
        };

        /// <summary>
        /// Transform an HTML string.
        /// </summary>
        /// <param name="value">The HTML to transform.</param>
        /// <returns>The transformed HTML.</returns>
        public static string TransformHtml(string value)
        {
            string result = value;

            for (int i = 0; i < HtmlCode.Length; i = i + 2)
            {
                result = result.Replace(HtmlCode[i], HtmlCode[i + 1]);
            }

            string[] pp = result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            result = String.Join(" ", pp);

            return result;
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
            T[] newTab = new T[tab.Length + 1];
            tab.CopyTo(newTab, 0);

            newTab[newTab.Length - 1] = element;
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
    }
}
