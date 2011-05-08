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
                foreach (GenericScore score in m_center.Scores.Items)
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
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddControl(new Style());
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

        private void btnSetSkinColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = sender == btnSetSkinColor
                ? pnlSkinColor.BackColor : pnlFontColor.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (sender == btnSetSkinColor)
                {
                    pnlSkinColor.BackColor = colorDialog1.Color;
                    foreach (StyleControl ctrl in flowLayoutPanel1.Controls)
                    {
                        ctrl.SetBackColor(pnlSkinColor.BackColor);
                    }
                }
                else if (sender == btnSetFontColor)
                {
                    pnlFontColor.BackColor = colorDialog1.Color;
                }
            }
        }
    }
}
