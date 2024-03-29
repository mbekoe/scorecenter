﻿#region

/* 
 *      Copyright (C) 2009-2014 Team MediaPortal
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
        /// <summary>Flag to identify new downloaded score.</summary>
        [System.Xml.Serialization.XmlIgnore()]
        protected bool m_new = false;

        /// <summary>Flag to identify virtual score.</summary>
        [System.Xml.Serialization.XmlIgnore()]
        protected bool m_virtual = false;

        [System.Xml.Serialization.XmlIgnore()]
        protected bool m_canLive = true;

        [System.Xml.Serialization.XmlIgnore()]
        protected bool m_virtualResolved = false;

        [System.Xml.Serialization.XmlIgnore()]
        public VariableUrl Range = null;

        public bool IsNew() { return m_new; }
        public void SetNew(bool isNew) { m_new = isNew; }

        public bool IsVirtual() { return m_virtual; }
        public void SetVirtual(bool isVirtual) { m_virtual = isVirtual; }

        [System.Xml.Serialization.XmlIgnore()]
        public string LocName
        {
            get
            {
                return LocalizationManager.GetScoreString(this.Id, this.Name);
            }
        }

        internal abstract BaseScore Clone(string id);
        internal abstract void ApplySettings(BaseScore score);
        internal abstract bool CanApplySettings();
        internal abstract void SetDefaultIcon();

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

        #region Folder/Container
        public virtual bool IsFolder() { return false; }
        public virtual bool IsVirtualFolder() { return false; }
        public bool IsContainer()
        {
            return IsFolder() || IsVirtualFolder();
        }

        public virtual bool IsScore()
        {
            return !IsFolder() && !IsVirtualFolder();
        }

        public bool IsVirtualResolved() { return m_virtualResolved; }
        public void SetVirtualResolved() { m_virtualResolved = true; }

        public virtual IList<BaseScore> GetVirtualScores(ScoreParameter[] parameters)
        {
            return null;
        }

        #endregion

        #region Live
        public virtual bool IsLive()
        {
            return this.LiveConfig != null && this.LiveConfig.enabled;
        }

        public virtual bool CanLive() { return m_canLive; }
        public virtual void SetCanLive(bool enable) { m_canLive = enable; }

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
        #endregion

        #region Range Settings
        public bool HasNext() { return this.Range != null && this.Range.HasNext(); }
        public bool HasPrev() { return this.Range != null && this.Range.HasPrev(); }
        public void MoveNext() { if (this.Range != null) this.Range.MoveNext(); }
        public void MovePrev() { if (this.Range != null) this.Range.MovePrev(); }
        public int GetRangeValue()
        {
            if (this.Range == null) return 0;
            return this.Range.Value;
        }
        public string GetRangeLabel()
        {
            if (this.Range == null) return " ";
            return this.Range.ToString();
        }
        public virtual void ApplyRangeValue(bool setDefault) { }
        public virtual void ResetRangeValue() { if (this.Range != null) this.Range.Reset(); }
        #endregion

        public virtual List<ScoreDifference> GetDifferences(string oldScore, string newScore)
        {
            List<ScoreDifference> res = ScoreDifference.ListFromString(newScore);
            List<ScoreDifference> old = ScoreDifference.ListFromString(oldScore);

            for (int i = 0; i < res.Count(); i++)
            {
                if (old.Count() <= i)
                    res[i].IsNew = true;
                else
                    res[i].IsNew = !String.Equals(res[i].Word, old[i].Word);
            }

            return res;
        }
    }
}
