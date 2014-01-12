#region

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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter
{
    public partial class StyleControl : UserControl
    {
        private string m_originalName;
        public EventHandler OnDelete;
        public StyleControl()
        {
            InitializeComponent();
        }

        public StyleControl(string name, long code)
        {
            InitializeComponent();

            m_originalName = name;
            tbxStyleName.Text = name;
            pnlStyle.BackColor = Color.FromArgb((int)code);
            tbxStyleName.ForeColor = pnlStyle.BackColor;
        }

        public string OriginalName
        {
            get { return m_originalName; }
        }

        public string StyleName
        {
            get { return tbxStyleName.Text; }
        }

        public int ColorCode
        {
            get { return pnlStyle.BackColor.ToArgb(); }
        }
        
        public void SetBackColor(Color bckColor)
        {
            tbxStyleName.BackColor = bckColor;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (OnDelete != null)
            {
                OnDelete(this, null);
            }
        }

        private void pnlStyle_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = pnlStyle.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pnlStyle.BackColor = colorDialog1.Color;
                tbxStyleName.ForeColor = colorDialog1.Color;
            }
        }
    }
}
