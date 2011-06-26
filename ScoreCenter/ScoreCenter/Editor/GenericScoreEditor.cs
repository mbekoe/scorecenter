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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace MediaPortal.Plugin.ScoreCenter.Editor
{
    public partial class GenericScoreEditor : BaseScoreEditor
    {
        public GenericScoreEditor()
        {
            InitializeComponent();

            tbxScore.TextChanged += new EventHandler(ScoreChanged);
            tbxUrl.TextChanged += new EventHandler(ScoreChanged);
            tbxXpath.TextChanged += new EventHandler(ScoreChanged);
            tbxEncoding.TextChanged += new EventHandler(ScoreChanged);
            tbxElement.TextChanged += new EventHandler(ScoreChanged);
            tbxEncoding.TextChanged += new EventHandler(ScoreChanged);
            tbxHeaders.TextChanged += new EventHandler(ScoreChanged);
            tbxSizes.TextChanged += new EventHandler(ScoreChanged);
            tbxSkip.TextChanged += new EventHandler(ScoreChanged);
            tbxMaxLines.TextChanged += new EventHandler(ScoreChanged);
            cbxBetweenElements.SelectedValueChanged += new EventHandler(ScoreChanged);

            ckxAllowWrapping.CheckedChanged += new EventHandler(ScoreChanged);
            ckxNewLine.CheckedChanged += new EventHandler(ScoreChanged);
            ckxUseTheader.CheckedChanged += new EventHandler(ScoreChanged);
            ckxUseCaption.CheckedChanged += new EventHandler(ScoreChanged);
        }

        public override bool HasTest
        {
            get
            {
                return true;
            }
        }
        public override Type GetScoreType()
        {
            return typeof(GenericScore);
        }

        private void ScoreChanged(object sender, EventArgs e)
        {
            //SetScoreStatus(false);
        }
        private void SetRules(GenericScore score)
        {
            grdRule.Rows.Clear();
            if (score.Rules == null)
                return;

            foreach (Rule rule in score.Rules)
            {
                Style st = m_center.FindStyle(rule.Format);
                grdRule.Rows.Add(rule.Column.ToString(),
                    rule.Operator.ToString(),
                    rule.Value,
                    rule.Action.ToString(),
                    st == null ? String.Empty : rule.Format);
            }
        }
        private void SaveRules(GenericScore score)
        {
            score.Rules = null;
            if (grdRule.Rows.Count > 1)
            {
                score.Rules = new Rule[grdRule.Rows.Count - 1];
                int i = 0;
                foreach (DataGridViewRow row in grdRule.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    Rule r = new Rule();
                    r.Column = int.Parse(row.Cells[colColumn.Name].Value.ToString());
                    r.Value = (row.Cells[colValue.Name].Value == null ? String.Empty : row.Cells[colValue.Name].Value.ToString());
                    r.Operator = Tools.ParseEnum<Operation>(row.Cells[colOperator.Name].Value.ToString());
                    r.Action = Tools.ParseEnum<RuleAction>(row.Cells[colAction.Name].Value.ToString());
                    r.Format = (row.Cells[colStyle.Name].Value == null ? String.Empty : row.Cells[colStyle.Name].Value.ToString());

                    score.Rules[i++] = r;
                }
            }
        }
        private void btnAuto_Click(object sender, EventArgs e)
        {
            string result = String.Empty;

            string[][] lines = this.TestPanel == null ? null : this.TestPanel.Tag as string[][];
            if (lines != null)
            {
                // get number of columns
                int nbCols = 0;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j] != null)
                    {
                        nbCols = Math.Max(nbCols, lines[j].Length);
                    }
                }

                // for all columns
                double dummy;
                for (int i = 0; i < nbCols; i++)
                {
                    int max = 0;
                    bool digitOnly = true;
                    // for all lines
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j] == null || i >= lines[j].Length)
                            continue;

                        string cell = lines[j][i];
                        int cellSize = String.IsNullOrEmpty(cell) ? 0 : lines[j][i].Length;
                        if (j > 0) digitOnly &= (cellSize == 0 || double.TryParse(cell.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out dummy));
                        max = Math.Max(max, cellSize);
                    }

                    if (digitOnly) max++;
                    else max = -max;

                    if (result.Length > 0) result += ",";
                    result += max.ToString();
                }
            }

            if (String.IsNullOrEmpty(result) == false)
            {
                tbxSizes.Text = result;
            }
        }
        private void tbxSizes_TextChanged(object sender, EventArgs e)
        {
            int res = 0;
            ColumnDisplay[] sizes = ColumnDisplay.GetSizes(tbxSizes.Text);

            if (sizes != null)
            {
                res = sizes.Sum(col => col.Size);
            }

            lblTotalSize.Text = String.Format("Total Size = {0}", res);
        }
        public override void AlignColumn(Point pt, ContentAlignment alignement)
        {
            if (this.TestPanel == null)
                return;

            Control ctr = this.TestPanel.GetChildAtPoint(pt);
            Label lbl = ctr as Label;

            if (lbl == null || lbl.Tag == null)
                return;

            int columnIndex = Convert.ToInt32(lbl.Tag);
            if (tbxSizes.Text.Length > 0)
            {
                ColumnDisplay[] dd = ColumnDisplay.GetSizes(tbxSizes.Text);
                dd[columnIndex].Alignement = ConvertAlignment(alignement);

                tbxSizes.Text = Tools.SizesToText(dd);
            }

            //lbl.TextAlign = alignement;

            foreach (Label c in this.TestPanel.Controls)
            {
                if (c == null || c.Tag == null) continue;
                int col = Convert.ToInt32(c.Tag);
                if (col == columnIndex)
                    c.TextAlign = alignement;
            }
        }
        private static ColumnDisplay.Alignment ConvertAlignment(ContentAlignment align)
        {
            if (align == ContentAlignment.MiddleRight)
                return ColumnDisplay.Alignment.Right;

            if (align == ContentAlignment.MiddleCenter)
                return ColumnDisplay.Alignment.Center;

            return ColumnDisplay.Alignment.Left;
        }

        private void btnOpenUrl_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text.Length > 0)
            {
                string url = Tools.ParseUrl(tbxUrl.Text, m_center.Parameters);
                Process.Start(url);
            }
        }

        private void grdRule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == colStyle.Index)
            {
                grdRule[e.ColumnIndex, e.RowIndex].Value = String.Empty;
            }
        }

        public override bool CheckData()
        {
            bool result = CheckTextBox(tbxScore, lblName, true);
            result &= CheckTextBox(tbxUrl, lblUrl, true);
            result &= CheckTextBox(tbxXpath, lblXPath, true);
            result &= CheckNumber(tbxSkip, lblSkip, false);
            result &= CheckNumber(tbxMaxLines, lblMaxLines, false);

            return result;
        }

        public override void Clear()
        {
        }

        public override void LoadScore(BaseScore baseScore, ScoreCenter center)
        {
            m_center = center;
            UpdateStyleList();

            GenericScore score = baseScore as GenericScore;
            if (score == null)
                throw new NullReferenceException("Not a generic score!");

            errorProvider1.Clear();

            // always clear
            tbxScore.Clear();
            tbxUrl.Clear();
            tbxXpath.Clear();
            tbxEncoding.Clear();
            tbxHeaders.Clear();
            tbxSizes.Clear();
            tbxSkip.Clear();
            tbxMaxLines.Clear();
            tbxElement.Clear();
            ckxUseTheader.Checked = false;
            ckxUseCaption.Checked = false;
            ckxNewLine.Checked = false;
            ckxAllowWrapping.Checked = false;

            grdRule.Rows.Clear();
            grdRule.Enabled = false;

            // score
            EnableControls();

            ParsingOptions options = score.GetParseOption();
            tbxScore.Text = score.Name;

            tbxUrl.Text = score.Url;
            tbxXpath.Text = score.XPath;
            tbxEncoding.Text = score.Encoding;
            tbxHeaders.Text = score.Headers;
            tbxSizes.Text = score.Sizes;
            tbxSkip.Text = score.Skip.ToString();
            tbxMaxLines.Text = score.MaxLines.ToString();
            tbxElement.Text = score.Element;

            ckxUseTheader.Checked = GenericScore.CheckParsingOption(options, ParsingOptions.UseTheader);
            ckxUseCaption.Checked = GenericScore.CheckParsingOption(options, ParsingOptions.Caption);
            ckxNewLine.Checked = GenericScore.CheckParsingOption(options, ParsingOptions.NewLine);
            ckxAllowWrapping.Checked = GenericScore.CheckParsingOption(options, ParsingOptions.WordWrap);
            ckxReverseOrder.Checked = GenericScore.CheckParsingOption(options, ParsingOptions.Reverse);
            cbxBetweenElements.SelectedValue = score.BetweenElts;

            grdRule.Enabled = true;
            SetRules(score, center);
        }

        public override bool SaveScore(ref BaseScore baseScore)
        {
            if (!CheckData())
                return false;

            GenericScore score = baseScore as GenericScore;

            score.Name = tbxScore.Text;
            score.Url = tbxUrl.Text;
            score.XPath = tbxXpath.Text;
            score.Headers = tbxHeaders.Text;
            score.Sizes = tbxSizes.Text;
            score.Encoding = tbxEncoding.Text;
            score.Element = tbxElement.Text;

            score.SetParseOption(ckxUseCaption.Checked,  ckxUseTheader.Checked,
                ckxNewLine.Checked, ckxAllowWrapping.Checked, ckxReverseOrder.Checked);
            score.BetweenElts = (BetweenElements)Enum.Parse(typeof(BetweenElements), cbxBetweenElements.SelectedValue.ToString());

            if (tbxSkip.Text.Length == 0) score.Skip = 0;
            else score.Skip = int.Parse(tbxSkip.Text);

            if (tbxMaxLines.Text.Length == 0) score.MaxLines = 0;
            else score.MaxLines = int.Parse(tbxMaxLines.Text);

            SaveRules(score);
            return true;
        }
        
        private void EnableControls(params Control[] controls)
        {
            foreach (Control ctr in tpgGeneral.Controls)
            {
                ctr.Enabled = (controls.Length == 0 || controls.Contains(ctr));
            }
        }
        private void UpdateStyleList()
        {
            colStyle.Items.Clear();
            colStyle.Items.Add("");

            if (m_center.Styles != null)
            {
                foreach (Style st in m_center.Styles)
                {
                    colStyle.Items.AddRange(st.Name);
                }
            }
        }

        private void SetRules(GenericScore score, ScoreCenter center)
        {
            grdRule.Rows.Clear();
            if (score.Rules == null)
                return;

            foreach (Rule rule in score.Rules)
            {
                Style st = center.FindStyle(rule.Format);
                grdRule.Rows.Add(rule.Column.ToString(),
                    rule.Operator.ToString(),
                    rule.Value,
                    rule.Action.ToString(),
                    st == null ? String.Empty : rule.Format);
            }
        }

        private void GenericScoreEditor_Load(object sender, EventArgs e)
        {
            // rule action
            colAction.DataSource = EnumManager.ReadRuleAction();
            colAction.ValueMember = "ID";
            colAction.DisplayMember = "NAME";

            colOperator.DataSource = EnumManager.ReadOperation();
            colOperator.ValueMember = "ID";
            colOperator.DisplayMember = "NAME";

            cbxBetweenElements.DataSource = EnumManager.ReadBetweenElements();
            cbxBetweenElements.ValueMember = "ID";
            cbxBetweenElements.DisplayMember = "NAME";
        }
    }
}
