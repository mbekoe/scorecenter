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
using System.Text;
using System.ComponentModel;

namespace MediaPortal.Plugin.ScoreCenter
{
    public abstract partial class BaseScore
    {
        /// <summary>
        /// Flag to identify new downloaded score.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore(), DefaultValue(false)]
        public bool IsNew { get; set; }

        [System.Xml.Serialization.XmlIgnore(), DefaultValue(false)]
        public bool IsVirtual { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        private bool m_canLive = true;

        [System.Xml.Serialization.XmlIgnore()]
        private bool m_virtualResolved = false;

        [System.Xml.Serialization.XmlIgnore()]
        public string LocName
        {
            get
            {
                return LocalizationManager.GetScoreString(this.Id, this.Name);
            }
        }

        internal abstract BaseScore Clone(string id);
        internal abstract void SetDefaultIcon();

        public virtual bool IsFolder()
        {
            return false;
        }

        public virtual bool IsVirtualFolder()
        {
            return false;
        }

        public bool IsContainer()
        {
            return IsFolder() || IsVirtualFolder();
        }

        public virtual bool IsScore()
        {
            return !IsFolder() && !IsVirtualFolder();
        }

        public virtual bool IsLive()
        {
            return this.LiveConfig != null && this.LiveConfig.enabled;
        }

        public virtual bool CanLive()
        {
            return m_canLive;
        }

        public virtual void SetCanLive(bool enable)
        {
            m_canLive = enable;
        }

        public virtual void SetLive(bool enable)
        {
            if (this.LiveConfig == null)
            {
                if (!enable)
                    return;
                this.LiveConfig = new LiveConfig();
            }
            this.LiveConfig.enabled = enable;
        }

        public bool IsVirtualResolved()
        {
            return m_virtualResolved;
        }

        public void SetVirtualResolved()
        {
            m_virtualResolved = true;
        }

        public virtual IList<BaseScore> GetVirtualScores(ScoreParameter[] parameters)
        {
            return null;
        }

        public virtual string GetSource()
        {
            return String.Empty;
        }

        public virtual IList<string> GetStyles()
        {
            return new List<string>();
        }

        /// <summary>
        /// Merge this score with newScore using given options.
        /// </summary>
        /// <param name="newScore">The score to merge with.</param>
        /// <param name="type">Merge options.</param>
        /// <returns>True if the score changed.</returns>
        public virtual bool Merge(BaseScore newScore, ImportOptions option)
        {
            bool result = false;
            if ((option & ImportOptions.Names) == ImportOptions.Names)
            {
                result |= (String.Compare(this.Name, newScore.Name, true) != 0);

                this.Name = newScore.Name;
                this.Parent = newScore.Parent;
                this.Image = newScore.Image;
            }

            return result;
        }

        public int CompareTo(BaseScore other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.LocName, other.LocName);
            return diff;
        }

        public int CompareToNoLoc(BaseScore other)
        {
            int diff = this.Order - other.Order;
            if (diff == 0) diff = String.Compare(this.Name, other.Name);
            return diff;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
