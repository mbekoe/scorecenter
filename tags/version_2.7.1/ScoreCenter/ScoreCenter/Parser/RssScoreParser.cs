#region Copyright (C) 2005-2012 Team MediaPortal

/* 
 *      Copyright (C) 2005-2012 Team MediaPortal
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
using System.Text;
using System.Xml.Linq;

namespace MediaPortal.Plugin.ScoreCenter.Parser
{
    public class RssScoreParser : ScoreParser<RssScore>
    {
        public RssScoreParser(int lifetime)
            : base(lifetime)
        {
        }
        private class FeedItem
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }

            public FeedItem(XElement elt)
            {
                XElement xe = elt.Element("title");
                if (xe != null) this.Title = xe.Value.Normalize();

                xe = elt.Element("link");
                if (xe != null) this.Link = xe.Value;

                xe = elt.Element("description");
                if (xe != null) this.Description = xe.Value;

                xe = elt.Element("pubDate");
                if (xe != null) this.Date = DateTime.Parse(xe.Value);
            }

            public override string ToString()
            {
                //return String.Format("{0:g}: {1}", this.Date, this.Title.Substring(0, Math.Min(Title.Length, 10)));
                //if (this.Title.Length <= 49)
                return String.Format("{0}: {1}", this.Date.ToString("dd/MM HH:mm"), this.Title);
                //return "---";
            }
        }
        protected override string[][] Parse(RssScore score, bool reload, ScoreParameter[] parameters)
        {
            XDocument feedXML = XDocument.Load(score.Url);

            var title = feedXML.Descendants("title");
            if (title != null && title.Count() > 0)
                System.Console.WriteLine(title.First().Value);

            IList<string[]> feeds = new List<string[]>();
            foreach (var f in feedXML.Descendants("item"))
            {
                feeds.Add(new string[] { new FeedItem(f).ToString() });
            }

            return feeds.ToArray();
        }
    }
}
