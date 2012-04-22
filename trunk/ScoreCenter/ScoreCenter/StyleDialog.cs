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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class StyleDialog : Form
    {
        private ScoreCenter m_center;

        public StyleDialog(ScoreCenter center)
        {
            InitializeComponent();

            m_center = center;
            if (m_center.Setup != null)
            {
                pnlSkinColor.BackColor = Color.FromArgb(m_center.Setup.DefaultSkinColor);
                pnlFontColor.BackColor = Color.FromArgb(m_center.Setup.DefaultFontColor);
                pnlAltColor.BackColor = Color.FromArgb(m_center.Setup.AltFontColor);
            }
        }

        private void StyleDialog_Load(object sender, EventArgs e)
        {
            if (m_center.Styles == null)
                return;
            
            foreach (Style style in m_center.Styles)
            {
                AddControl(style);
            }
        }

        private void StyleDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                List<Style> toSave = new List<Style>();
                Dictionary<string, string> rename = new Dictionary<string, string>();
                foreach (StyleControl ctrl in flowLayoutPanel1.Controls)
                {
                    Style style = ctrl.Tag as Style;
                    style.Name = ctrl.StyleName;
                    style.ForeColor = ctrl.ColorCode;
                    if (style.Name.Length > 0)
                    {
                        toSave.Add(style);
                        if (!String.IsNullOrEmpty(ctrl.OriginalName) && ctrl.OriginalName != style.Name)
                        {
                            rename.Add(ctrl.OriginalName.ToUpper(), style.Name);
                        }
                    }
                }

                m_center.Styles = new Style[toSave.Count];
                toSave.CopyTo(m_center.Styles, 0);

                // rename in scores
                foreach (GenericScore score in m_center.Scores.Items.OfType<GenericScore>())
                {
                    if (score != null && score.Rules != null)
                    {
                        foreach (Rule r in score.Rules)
                        {
                            if (rename.ContainsKey(r.Format.ToUpper()))
                            {
                                r.Format = rename[r.Format.ToUpper()];
                            }
                        }
                    }
                }

                // save setup
                if (m_center.Setup == null)
                {
                    m_center.Setup = new ScoreCenterSetup();
                }

                m_center.Setup.DefaultSkinColor = pnlSkinColor.BackColor.ToArgb();
                m_center.Setup.DefaultFontColor = pnlFontColor.BackColor.ToArgb();
                m_center.Setup.AltFontColor = pnlAltColor.BackColor.ToArgb();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Style ns = new Style();
            ns.Name = "new";
            ns.ForeColor = -1;
            AddControl(ns);
        }

        private void AddControl(Style style)
        {
            StyleControl ctrl = new StyleControl(style.Name, style.ForeColor);
            ctrl.SetBackColor(pnlSkinColor.BackColor);
            ctrl.OnDelete += new EventHandler(btnDelete_Click);
            ctrl.Tag = style;
            flowLayoutPanel1.Controls.Add(ctrl);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            StyleControl ctrl = sender as StyleControl;
            if (ctrl != null)
            {
                flowLayoutPanel1.Controls.Remove(ctrl);
            }
        }

        private void pnlColor_Click(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            colorDialog1.Color = pnl.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pnl.BackColor = colorDialog1.Color;
                if (sender == pnlSkinColor)
                {
                    foreach (StyleControl ctrl in flowLayoutPanel1.Controls)
                    {
                        ctrl.SetBackColor(pnlSkinColor.BackColor);
                    }
                }
            }
        }
    }
}
