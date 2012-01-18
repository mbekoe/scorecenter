using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MediaPortal.Plugin.ScoreCenter
{
    internal class ScoreCache
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
        
        public ScoreCache(int lifeTime)
        {
            m_client = new WebClient();
            m_cache = new Dictionary<string, CacheElement>();
            m_lifeTime = lifeTime;
        }

        public void Clear()
        {
            m_cache.Clear();
        }

        public string GetScore(string url, string encoding, bool reload)
        {
            string html = String.Empty;
            if (!reload && m_cache.ContainsKey(url))
            {
                if (!m_cache[url].Expired)
                    html = m_cache[url].Value;
            }

            if (html.Length == 0)
            {
                html = Tools.DownloadFile(m_client, url, encoding);
                m_cache[url] = new CacheElement(m_lifeTime, html);
            }

            return html;
        }

        private class CacheElement
        {
            public string Value;
            public DateTime ExpirationDate;

            public CacheElement(int lifeTime, string value)
            {
                Value = value;
                ExpirationDate = lifeTime == 0 ? DateTime.MaxValue : DateTime.Now.AddMinutes(lifeTime);
            }

            public bool Expired
            {
                get { return DateTime.Now > ExpirationDate; }
            }
        }
    }
}
