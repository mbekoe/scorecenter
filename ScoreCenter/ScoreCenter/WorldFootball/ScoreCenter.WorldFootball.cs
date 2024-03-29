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
using System.Data;
using System.Linq;
using System.Text;

namespace MediaPortal.Plugin.ScoreCenter
{
    public static partial class EnumManager
    {
        public static DataTable ReadWorldFootballKind()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(WorldFootballKind.Cup, "Cup");
            dt.Rows.Add(WorldFootballKind.League, "League");
            dt.Rows.Add(WorldFootballKind.Qualification, "Qualification");
            dt.Rows.Add(WorldFootballKind.Tournament, "Tournament");
            dt.Rows.Add(WorldFootballKind.Team, "Team");

            return dt;
        }
    }

    public partial class WorldFootballScore
    {
        [System.Xml.Serialization.XmlIgnore()]
        public string FullLeagueName
        {
            get
            {
                if (String.IsNullOrEmpty(this.Country))
                    return this.League;

                return String.Format("{0}-{1}", this.Country, this.League);
            }
        }
        public override bool IsVirtualFolder()
        {
            return true;
        }

        public override IList<BaseScore> GetVirtualScores(ScoreParameter[] parameters)
        {
            return Parser.WorldFootballScoreParser.GetRealScores(this, parameters);
        }

        internal override BaseScore Clone(string id)
        {
            WorldFootballScore copy = new WorldFootballScore();
            copy.Id = id;
            if (String.IsNullOrEmpty(id)) copy.Id = Tools.GenerateId();

            copy.Name = this.Name;
            copy.Country = this.Country;
            copy.Image = this.Image;
            copy.Parent = this.Parent;
            copy.League = this.League;
            copy.Season = this.Season;
            copy.Kind = this.Kind;
            copy.TwoLegs = this.TwoLegs;
            copy.NbTeams = this.NbTeams;
            copy.Rounds = this.Rounds;
            copy.Levels = this.Levels;
            copy.Highlights = this.Highlights;
            copy.Details = this.Details;
            copy.LiveConfig = this.LiveConfig;
            if (this.Range != null)
            {
                copy.Range = this.Range.Clone();
            }

            return (BaseScore)copy;
        }

        internal override bool CanApplySettings()
        {
            return true;
        }

        internal override void ApplySettings(BaseScore score)
        {
            WorldFootballScore settings = score as WorldFootballScore;
            if (settings != null)
            {
                this.Kind = settings.Kind;
                this.NbTeams = settings.NbTeams;
                this.Rounds = settings.Rounds;
                this.Levels = settings.Levels;
                this.Highlights = settings.Highlights;
                this.Details = settings.Details;
                this.LiveConfig = settings.LiveConfig;
                this.TwoLegs = settings.TwoLegs;
            }
        }

        internal override void SetDefaultIcon()
        {
            this.Image = @"Misc\wfb";
        }
        public override bool CanLive()
        {
            return false;
        }
        public override bool Merge(BaseScore newBaseScore, ImportOptions option)
        {
            WorldFootballScore newScore = newBaseScore as WorldFootballScore;
            if (newScore == null)
                return false;

            bool result = base.Merge(newBaseScore, option);

            if ((option & ImportOptions.Parsing) == ImportOptions.Parsing)
            {
                result |= (String.Compare(this.Country, newScore.Country, true) != 0)
                    || (String.Compare(this.League, newScore.League, true) != 0)
                    || (String.Compare(this.Season, newScore.Season, true) != 0)
                    || (this.NbTeams != newScore.NbTeams)
                    || (this.Rounds != newScore.Rounds)
                    || (this.Kind != newScore.Kind)
                    || (this.TwoLegs != newScore.TwoLegs);

                this.Country = newScore.Country;
                this.League = newScore.League;
                this.Season = newScore.Season;
                this.NbTeams = newScore.NbTeams;
                this.Rounds = newScore.Rounds;
                this.Details = newScore.Details;
                this.Kind = newScore.Kind;
                this.TwoLegs = newScore.TwoLegs;
            }

            if ((option & ImportOptions.Rules) == ImportOptions.Rules)
            {
                result |= (String.Compare(this.Levels, newScore.Levels, true) != 0);

                this.Levels = newScore.Levels;
                result = true;
            }

            return result;
        }
    }
}
