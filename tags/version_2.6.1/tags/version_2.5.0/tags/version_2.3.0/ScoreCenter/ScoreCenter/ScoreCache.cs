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
using System.Linq;
using System.Net;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public class ScoreCache
    {
        /// <summary>
        /// Cache.
        /// </summary>
        private IDictionary<string, CacheElement> m_cache;

        /// <summary>
        /// Client used to download files.
        /// </summary>
        private WebClient m_client;

        private int m_lifeTime = 0;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lifeTime"></param>
        public ScoreCache(int lifeTime)
        {
            m_client = new WebClient();
            m_cache = new Dictionary<string, CacheElement>();
            m_lifeTime = lifeTime;
            //Tools.LogMessage("ScoreCache = {0}", m_lifeTime);
            m_client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            m_cache.Clear();
        }

        /// <summary>
        /// Gets a score from the cache.
        /// Get it from the cache if the score has not expired or download it.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="reload"></param>
        /// <returns></returns>
        public string GetScore(string url, string encoding, bool reload)
        {
            if (m_client.IsBusy)
                return "";

            string html = String.Empty;
            if (!reload && m_cache.ContainsKey(url))
            {
                if (!m_cache[url].Expired)
                    html = m_cache[url].Value;
            }

            if (String.IsNullOrEmpty(html))
            {
                Tools.LogMessage("Download = {0}", url);
                html = Tools.DownloadFile(m_client, url, encoding);
                if (m_lifeTime > 0)
                {
                    m_cache[url] = new CacheElement(m_lifeTime, html);
                }
            }

            return html;
        }

        /// <summary>
        /// Gets an image.
        /// Note that there is no "cache here".
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        public void GetImage(string url, string fileName)
        {
            m_client.DownloadFile(url, fileName);
        }

        /// <summary>
        /// Class to reprensent an element to store in the cache.
        /// </summary>
        private class CacheElement
        {
            public string Value;
            public DateTime ExpirationDate;

            public CacheElement(int lifeTime, string value)
            {
                this.Value = value;
                this.ExpirationDate = DateTime.Now.AddMinutes(lifeTime);
            }

            public bool Expired
            {
                get { return DateTime.Now > this.ExpirationDate; }
            }
        }
    }
}
