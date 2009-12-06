#region Copyright (C) 2005-2009 Team MediaPortal

/* 
 *      Copyright (C) 2005-2009 Team MediaPortal
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MediaPortal.Configuration;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Score Center configuration dialog.
    /// </summary>
    public partial class ScoreCenterConfig : Form
    {
        //private IList<Score> m_scores;
        private ScoreCenter m_center;
        private ScoreParser m_parser;
        private string m_settings;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScoreCenterConfig() : this(false)
        {
        }
        
        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="showInTaskbar">Ture to show the dialog in the taskbar.</param>
        public ScoreCenterConfig(bool showInTaskbar)
        {
            InitializeComponent();
            
            m_parser = new ScoreParser(0);
            tvwScores.TreeViewNodeSorter = new ScoreNodeComparer();

            this.ShowInTaskbar = showInTaskbar;

            tbxCategory.TextChanged += new EventHandler(ScoreChanged);
            tbxLeague.TextChanged += new EventHandler(ScoreChanged);
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

            ckxAllowWrapping.CheckedChanged += new EventHandler(ScoreChanged);
            ckxNewLine.CheckedChanged += new EventHandler(ScoreChanged);
            ckxUseTheader.CheckedChanged += new EventHandler(ScoreChanged);
        }

        private void ScoreChanged(object sender, EventArgs e)
        {
            SetScoreStatus(false);
        }
        
        private void SetScoreStatus(bool saved)
        {
            if (saved)
            {
                btnSave.BackColor = SystemColors.Control;
                btnSave.UseVisualStyleBackColor = true;
            }
            else
            {
                btnSave.BackColor = Color.Salmon;
            }
        }

        private void ScoreCenterConfig_Load(object sender, EventArgs e)
        {
            try
            {
                m_settings = Config.GetFile(Config.Dir.Config, ScoreCenterGui.SettingsFileName);
                m_center = Tools.ReadSettings(m_settings, true);

                // rule action
                colAction.DataSource = EnumManager.ReadRuleAction();
                colAction.ValueMember = "ID";
                colAction.DisplayMember = "NAME";

                colOperator.DataSource = EnumManager.ReadOperation();
                colOperator.ValueMember = "ID";
                colOperator.DisplayMember = "NAME";

                UpdateStyleList();

                BuildScoreList(tvwScores, m_center, true);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public static void BuildScoreList(ThreeStateTreeView tree, ScoreCenter center, bool show)
        {
            tree.Nodes.Clear();
            if (center == null || center.Scores == null)
                return;

            IEnumerable<string> categories = center.Scores.Select(sc => sc.Category).Distinct();

            foreach (string cat in categories)
            {
                ThreeStateTreeNode cnode = new ThreeStateTreeNode(cat);
                cnode.Name = cat;
                tree.Nodes.Add(cnode);

                IEnumerable<Score> scores = center.Scores.Where(sc => sc.Category == cat);
                IEnumerable<string> leagues = scores.Select(sc => sc.Ligue).Distinct();
                foreach (string league in leagues)
                {
                    ThreeStateTreeNode lnode = new ThreeStateTreeNode(league);
                    lnode.Name = league;
                    cnode.Nodes.Add(lnode);

                    IEnumerable<Score> leagueScores = scores.Where(sc => sc.Ligue == league);
                    foreach (Score score in leagueScores)
                    {
                        ThreeStateTreeNode snode = new ThreeStateTreeNode(score.Name);
                        lnode.Nodes.Add(snode);
                        snode.Tag = score;
                        if (score.enable && show)
                        {
                            snode.State = CheckBoxState.Checked;
                        }
                    }

                    ThreeStateTreeNode leaf = lnode.FirstNode as ThreeStateTreeNode;
                    leaf.UpdateStateOfRelatedNodes();
                }
            }

            tree.Sort();
        }

        private void RefreshTree()
        {
            if (tvwScores.Nodes.Count == 0)
                return;

            // keep selected node and expanded nodes
            IList<string> keys = new List<string>();
            string selectedPath = String.Empty;
            if (tvwScores.SelectedNode != null)
                selectedPath = tvwScores.SelectedNode.FullPath;

            TreeNode node = tvwScores.Nodes[0];
            do
            {
                if (node.IsExpanded)
                    keys.Add(node.FullPath);

                node = node.NextVisibleNode;
            }
            while (node != null);

            // rebuild tree
            BuildScoreList(tvwScores, m_center, true);

            // reselect
            TreeNode selected = SelectNodes(tvwScores.Nodes, keys, selectedPath);
            if (selected == null && tvwScores.Nodes.Count > 0) selected = tvwScores.Nodes[0];
            tvwScores.SelectedNode = selected;
        }

        private TreeNode SelectNodes(TreeNodeCollection nodes, IList<string> keys, string current)
        {
            TreeNode result = null;
            if (nodes == null || nodes.Count == 0)
                return result;

            foreach (TreeNode node in nodes)
            {
                if (keys.Contains(node.FullPath))
                    node.Expand();

                if (result == null && node.FullPath == current)
                    result = node;

                TreeNode n = SelectNodes(node.Nodes, keys, current);
                if (result == null)
                    result = n;
            }

            return result;
        }

        private void tvwScores_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            errorProvider1.Clear();
            if (tvwScores.SelectedNode.Level == 2)
            {
                // score
                gbxScore.Enabled = true;
                Score score = tvwScores.SelectedNode.Tag as Score;

                tbxCategory.Text = score.Category;
                tbxLeague.Text = score.Ligue;
                tbxScore.Text = score.Name;

                tbxUrl.Text = score.Url;
                tbxXpath.Text = score.XPath;
                tbxEncoding.Text = score.Encoding;
                tbxHeaders.Text = score.Headers;
                tbxSizes.Text = score.Sizes;
                tbxSkip.Text = score.Skip.ToString();
                tbxMaxLines.Text = score.MaxLines.ToString();
                tbxElement.Text = score.Element;
                ckxUseTheader.Checked = score.UseTheader;
                ckxNewLine.Checked = score.NewLine;
                ckxAllowWrapping.Checked = score.WordWrap;
                SetIcon(score.Image);

                grdRule.Enabled = true;
                SetRules(score);
            }
            else
            {
                // category or league
                gbxScore.Enabled = false;

                tbxCategory.Text = String.Empty;
                tbxLeague.Text = String.Empty;
                tbxScore.Text = String.Empty;

                tbxUrl.Text = String.Empty;
                tbxXpath.Text = String.Empty;
                tbxEncoding.Text = String.Empty;
                tbxHeaders.Text = String.Empty;
                tbxSizes.Text = String.Empty;
                tbxSkip.Text = String.Empty;
                tbxMaxLines.Text = String.Empty;
                tbxElement.Text = String.Empty;
                ckxUseTheader.Checked = false;
                ckxNewLine.Checked = false;
                ckxAllowWrapping.Checked = false;

                ClearIcon();
                if (tvwScores.SelectedNode.Level == 0)
                {
                    CategoryImg img = m_center.FindCategoryImage(tvwScores.SelectedNode.Text);
                    if (img != null) SetIcon(img.Path);
                }
                else
                {
                    LeagueImg img = m_center.FindLeagueImage(tvwScores.SelectedNode.Parent.Text, tvwScores.SelectedNode.Text);
                    if (img != null) SetIcon(img.Path);
                }

                grdRule.Rows.Clear();
                grdRule.Enabled = false;
            }

            ClearTestGrid();
            tsbCopyScore.Enabled = (tvwScores.SelectedNode.Level == 2);
            tsbMoveUp.Enabled = tsbCopyScore.Enabled && tvwScores.SelectedNode.PrevNode != null;
            tsbMoveDown.Enabled = tsbCopyScore.Enabled && tvwScores.SelectedNode.NextNode != null;

            SetScoreStatus(true);
        }

        private void ClearTestGrid()
        {
            pnlTest.Controls.Clear();
            pnlTest.Tag = null;
        }

        private void SetRules(Score score)
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

        #region Icon Management
        private void ClearIcon()
        {
            if (pbxIcon.Image != null)
            {
                pbxIcon.Image.Dispose();
                pbxIcon.Image = null;
            }
        }

        private void SetIcon(string name)
        {
            ClearIcon();
            if (String.IsNullOrEmpty(name) == false)
            {
                string path = Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", name + ".png");
                if (File.Exists(path))
                {
                    pbxIcon.Image = new Bitmap(path);
                }
            }
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null
                || tvwScores.SelectedNode.Tag == null)
                return;

            errorProvider1.Clear();
            if (CheckData())
            {
                Score score = tvwScores.SelectedNode.Tag as Score;

                // check if hierarchie changed
                bool refresh = true;
                if (score.Category == tbxCategory.Text
                    && score.Ligue == tbxLeague.Text)
                {
                    refresh = false;
                }

                if (score.Name != tbxScore.Text)
                {
                    tvwScores.SelectedNode.Text = tbxScore.Text;
                }

                score.Category = tbxCategory.Text;
                score.Ligue = tbxLeague.Text;
                score.Name = tbxScore.Text;
                
                score.Url = tbxUrl.Text;
                score.XPath = tbxXpath.Text;
                score.Headers = tbxHeaders.Text;
                score.Sizes = tbxSizes.Text;
                score.Encoding = tbxEncoding.Text;
                score.Element = tbxElement.Text;
                score.UseTheader = ckxUseTheader.Checked;
                score.NewLine = ckxNewLine.Checked;
                score.WordWrap = ckxAllowWrapping.Checked;

                if (tbxSkip.Text.Length == 0) score.Skip = 0;
                else score.Skip = int.Parse(tbxSkip.Text);

                if (tbxMaxLines.Text.Length == 0) score.MaxLines = 0;
                else score.MaxLines = int.Parse(tbxMaxLines.Text);

                SaveRules(score);

                if (refresh)
                {
                    RefreshTree();
                }

                SetScoreStatus(true);
            }
        }

        private void SaveRules(Score score)
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

        private bool CheckData()
        {
            bool result = CheckTextBox(tbxUrl, lblUrl, true);
            result &= CheckTextBox(tbxXpath, lblXPath, true);
            result &= CheckNumber(tbxSkip, lblSkip, false);
            result &= CheckNumber(tbxMaxLines, lblMaxLines, false);

            return result;
        }

        private bool CheckTextBox(TextBox control, Label label, bool require)
        {
            if (control.Text.Length == 0 && require)
            {
                string message = String.Format(Properties.Resources.RequiredField, label.Text);
                errorProvider1.SetError(control, message);
            }

            return errorProvider1.GetError(control).Length == 0;
        }
        
        private bool CheckNumber(TextBox control, Label label, bool require)
        {
            if (control.Text.Length == 0)
            {
                if (require)
                {
                    string message = String.Format(Properties.Resources.RequiredField, label.Text);
                    errorProvider1.SetError(control, message);
                }
            }
            else
            {
                int test;
                if (false == int.TryParse(control.Text, out test))
                {
                    string message = String.Format(Properties.Resources.BadNumberFormat, label.Text);
                    errorProvider1.SetError(control, message);
                }
            }

            return errorProvider1.GetError(control).Length == 0;
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            AboutDialog dlg = new AboutDialog();
            dlg.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveConfig();
            this.Close();
        }

        private void SaveConfig()
        {
            #region Order
            if (m_center.Images != null && m_center.Images.CategoryImg != null)
            {
                m_center.Images.CategoryImg = m_center.Images.CategoryImg.OrderBy(img => img.Name).ToArray();
            }

            if (m_center.Images !=null && m_center.Images.LeagueImg != null)
            {
                m_center.Images.LeagueImg = m_center.Images.LeagueImg.OrderBy(img => img.Category)
                    .ThenBy(img => img.Name)
                    .ToArray();
            }

            if (m_center.Scores != null)
            {
                m_center.Scores = m_center.Scores.OrderBy(sc => sc.Category)
                    .ThenBy(sc => sc.Ligue)
                    .ThenBy(sc => sc.Order)
                    .ThenBy(sc => sc.Name)
                    .ToArray();
            }
            #endregion

            Tools.SaveSettings(m_settings, m_center, true);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            pnlTest.Controls.Clear();
            pnlTest.Refresh();

            try
            {
                this.Cursor = Cursors.WaitCursor;

                // note create a fake ScoreCenterScore to use current values
                // instead of saved values
                Score score = new Score();
                score.Url = tbxUrl.Text;
                score.Encoding = tbxEncoding.Text;
                score.XPath = tbxXpath.Text;
                score.Sizes = tbxSizes.Text;
                score.Headers = tbxHeaders.Text;
                score.UseTheader = ckxUseTheader.Checked;
                score.NewLine = ckxNewLine.Checked;
                score.WordWrap = ckxAllowWrapping.Checked;

                score.Element = tbxElement.Text;
                score.Skip = ReadInt(tbxSkip);
                score.MaxLines = ReadInt(tbxMaxLines);
                SaveRules(score);
                
                // read and parse the score
                string[][] lines = m_parser.Read(score, ckxReload.Checked);

                ScoreBuilder<Control> bld = new ScoreBuilder<Control>();
                bld.Center = m_center;
                bld.Score = score;
                bld.LimitToPage = false;
                bld.AutoSize = false;
                bld.AutoWrap = false;
                
                int fh = pnlTest.Font.Height;
                int fw = (int)pnlTest.Font.SizeInPoints;
                bld.SetFont("", m_center.Setup.DefaultFontColor, 14, fw, fh);
                
                bool overRight = false;
                bool overDown = false;
                int lineNumber, colNumber;

                pnlTest.BackColor = Color.FromArgb(m_center.Setup.DefaultSkinColor);
                //pnlTest.Width = 970;
                //pnlTest.Height = 600;
                //pnlTest.Font = new Font(pnlTest.Font.FontFamily, 14.0f);

                IList<Control> controls = bld.Build(lines,
                    0, 0,
                    0, 0, pnlTest.Width, pnlTest.Height,
                    this.CreateControl,
                    out overRight, out overDown, out lineNumber, out colNumber);

                tabScore.SelectedTab = tpgTest;

                pnlTest.Tag = lines;

                pnlTest.SuspendLayout();
                pnlTest.Controls.AddRange(controls.ToArray());
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pnlTest.ResumeLayout();
                this.Cursor = Cursors.Default;
            }
        }

        public Control CreateControl(int posX, int posY, int width, int height,
            ColumnDisplay.Alignment alignement,
            string label, string font, int fontSize, Style style, int nbMax, int columnIndex)
        {
            string strLabel = label;

            // always start with a space
            strLabel = " " + label;

            // shrink text for small labels
            if (nbMax <= 6 && strLabel.Length > nbMax)
            {
                strLabel = strLabel.Substring(0, nbMax);
            }

            // create the control
            Label control = new Label()
            {
                Left = posX,
                Top = posY,
                AutoSize = false,
                //AutoEllipsis = true,
                Width = width,
                Height = height,
                Text = strLabel,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.FromArgb((int)style.ForeColor),
                TextAlign = alignement == ColumnDisplay.Alignment.Center ? ContentAlignment.MiddleCenter
                    : (alignement == ColumnDisplay.Alignment.Left ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleRight)
            };

            control.Tag = columnIndex;
            control.ContextMenuStrip = contextMenuStrip1;

            return control;
        }

        private static int ReadInt(TextBox control)
        {
            int result = 0;
            if (int.TryParse(control.Text, out result))
            {
                return result;
            }

            return -1;
        }

        private void tsbNewLigue_Click(object sender, EventArgs e)
        {
            string newName = Properties.Resources.NewItem;

            TreeNode parentNode = null;
            if (tvwScores.SelectedNode == null)
            {
                if (tvwScores.Nodes.Count == 0)
                {
                    tvwScores.Nodes.Add(newName);
                }

                parentNode = tvwScores.Nodes[0];
            }
            else
            {
                parentNode = tvwScores.SelectedNode;
            }

            TreeNode categoryNode = null;
            TreeNode leagueNode = null;
            string name = newName;

            switch (parentNode.Level)
            {
                case 1:
                    categoryNode = parentNode.Parent;
                    break;
                case 2:
                    categoryNode = parentNode.Parent.Parent;
                    leagueNode = parentNode.Parent;
                    break;
            }

            if (categoryNode == null)
            {
                categoryNode = new TreeNode();
                categoryNode.Text = newName;
                tvwScores.Nodes.Add(categoryNode);
            }

            if (leagueNode == null)
            {
                leagueNode = new TreeNode();
                leagueNode.Text = newName;
                categoryNode.Nodes.Add(leagueNode);
            }

            // create new item
            Score score = new Score();
            score.Id = Tools.GenerateId();
            score.Category = categoryNode.Text;
            score.Ligue = leagueNode.Text;
            score.Name = name;

            // add the new item and refresh
            m_center.AddScore(score);

            // create the tree node
            ThreeStateTreeNode newNode = new ThreeStateTreeNode(score.Name);
            newNode.Tag = score;

            leagueNode.Nodes.Add(newNode);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            string result = String.Empty;

            string[][] lines = pnlTest.Tag as string[][];
            if (lines != null)
            {
                int nbCols = 0;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j] != null)
                    {
                        nbCols = Math.Max(nbCols, lines[j].Length);
                    }
                }

                for (int i = 0; i < nbCols; i++)
                {
                    int max = 0;
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (lines[j] == null || i >= lines[j].Length)
                            continue;
                        
                        int v = String.IsNullOrEmpty(lines[j][i]) ? 0 : lines[j][i].Length;
                        max = - Math.Max(max, v);
                    }

                    if (result.Length > 0) result += ",";
                    result += max.ToString();
                }
            }

            if (String.IsNullOrEmpty(result) == false)
            {
                tbxSizes.Text = result;
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            string message = Properties.Resources.DeleteNode;
            if (DialogResult.Yes == MessageBox.Show(message, Properties.Resources.DeleteNodeTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                DeleteScore(tvwScores.SelectedNode);

                if (tvwScores.SelectedNode.Parent == null)
                {
                    tvwScores.Nodes.Remove(tvwScores.SelectedNode);
                }
                else
                {
                    tvwScores.SelectedNode.Parent.Nodes.Remove(tvwScores.SelectedNode);
                }
            }
        }

        private void DeleteScore(TreeNode node)
        {
            Score score = node.Tag as Score;
            if (score != null)
            {
                m_center.RemoveScore(score);
            }

            foreach (TreeNode sub in node.Nodes)
            {
                DeleteScore(sub);
            }
        }

        private void tsbCopyScore_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null
                || tvwScores.SelectedNode.Level != 2)
                return;

            Score source = tvwScores.SelectedNode.Tag as Score;
            Score copy = new Score();
            copy.Id = Tools.GenerateId();
            copy.Category = source.Category;
            copy.Ligue = source.Ligue;
            copy.Name = source.Name;
            copy.Url = source.Url;
            copy.XPath = source.XPath;
            copy.Headers = source.Headers;
            copy.Sizes = source.Sizes;
            copy.Skip = source.Skip;
            copy.MaxLines = source.MaxLines;
            copy.Image = source.Image;
            copy.Element = source.Element;
            copy.Encoding = source.Encoding;
            copy.UseTheader = source.UseTheader;

            // add the new item and refresh
            m_center.AddScore(copy);

            // create node
            ThreeStateTreeNode node = new ThreeStateTreeNode(copy.Name);
            node.Tag = copy;
            tvwScores.SelectedNode.Parent.Nodes.Add(node);
        }

        private void tvwScores_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            
            switch (e.Node.Level)
            {
                case 0:
                    // category
                    UpdateScore(e.Node.Nodes, e.Label, String.Empty);
                    tbxCategory.Text = e.Label;
                    UpdateCategoryImage(e.Node.Text, e.Label);
                    break;
                case 1:
                    // league
                    UpdateScore(e.Node.Nodes, String.Empty, e.Label);
                    UpdateLeagueImage(e.Node.Text, e.Node.Parent.Text, e.Label);
                    tbxLeague.Text = e.Label;
                    break;
                case 2:
                    // score
                    Score score = e.Node.Tag as Score;
                    score.Name = e.Label;
                    tbxScore.Text = e.Label;
                    break;
            }
        }

        private void UpdateScore(TreeNodeCollection nodes, string category, string league)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag != null)
                {
                    Score score = node.Tag as Score;
                    if (!String.IsNullOrEmpty(category)) score.Category = category;
                    if (!String.IsNullOrEmpty(league)) score.Ligue = league;
                }
                else
                {
                    UpdateScore(node.Nodes, category, league);
                }
            }
        }

        private void UpdateCategoryImage(string prev, string category)
        {
            if (m_center.Images == null)
                return;

            if (m_center.Images.CategoryImg != null)
            {
                foreach (CategoryImg img in m_center.Images.CategoryImg)
                {
                    if (img.Name == prev)
                        img.Name = category;
                }
            }
        }

        private void UpdateLeagueImage(string prev, string category, string league)
        {
            if (m_center.Images == null)
                return;

            if (m_center.Images.LeagueImg != null)
            {
                foreach (LeagueImg img in m_center.Images.LeagueImg)
                {
                    if (img.Name == prev && img.Category == category)
                        img.Name = league;
                }
            }
        }

        private void tvwScores_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                Score score = e.Node.Tag as Score;
                score.enable = e.Node.Checked;
            }
        }

        private void btnSetIcon_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            if (ofdSelectIcon.ShowDialog() == DialogResult.OK)
            {
                string file = ofdSelectIcon.FileName;
                FileInfo imgFile = new FileInfo(file);

                string root = Config.GetFile(Config.Dir.Thumbs, "ScoreCenter", "");
                string newFile = file;
                if (!imgFile.DirectoryName.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                {
                    // copy
                    newFile = Tools.GenerateFileName(root, imgFile.Name, imgFile.Extension);
                    File.Copy(file, newFile);
                }

                FileInfo newFI = new FileInfo(newFile);
                string iconName = newFI.FullName.Substring(root.Length + 1, newFI.FullName.Length - newFI.Extension.Length - root.Length - 1);
                SetIcon(iconName);

                switch (tvwScores.SelectedNode.Level)
                {
                    case 0:
                        CategoryImg img = m_center.FindCategoryImage(tvwScores.SelectedNode.Text);
                        if (img == null)
                        {
                            img = new CategoryImg();
                            img.Name = tvwScores.SelectedNode.Text;
                            m_center.Images.CategoryImg = Tools.AddElement<CategoryImg>(m_center.Images.CategoryImg, img);
                        }

                        img.Path = iconName;
                        break;
                    case 1:
                        LeagueImg limg = m_center.FindLeagueImage(tvwScores.SelectedNode.Parent.Text, tvwScores.SelectedNode.Text);
                        if (limg == null)
                        {
                            limg = new LeagueImg();
                            limg.Name = tvwScores.SelectedNode.Text;
                            limg.Category = tvwScores.SelectedNode.Parent.Text;
                            m_center.Images.LeagueImg = Tools.AddElement<LeagueImg>(m_center.Images.LeagueImg, limg);
                        }

                        limg.Path = iconName;
                        break;
                    case 2:
                        Score score = tvwScores.SelectedNode.Tag as Score;
                        score.Image = iconName;
                        break;
                }
            }
        }

        private void btnClearIcon_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            switch (tvwScores.SelectedNode.Level)
            {
                case 0:
                    m_center.Images.CategoryImg = m_center.Images.CategoryImg.Where(limg => limg.Name != tvwScores.SelectedNode.Text).ToArray();
                    break;
                case 1:
                    m_center.Images.LeagueImg = m_center.Images.LeagueImg.Where(limg => limg.Category != tvwScores.SelectedNode.Parent.Text
                        || limg.Name != tvwScores.SelectedNode.Text).ToArray();
                    break;
                case 2:
                    Score score = tvwScores.SelectedNode.Tag as Score;
                    score.Image = String.Empty;
                    break;
            }

            ClearIcon();
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            using (ExportDialog dlg = new ExportDialog(m_center))
            {
                dlg.ShowDialog();
            }
        }

        private void tsbImport_Click(object sender, EventArgs e)
        {
            if (ofdImport.ShowDialog() == DialogResult.OK)
            {
                using (ImportDialog dlg = new ImportDialog(ofdImport.FileName, m_center))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        RefreshTree();
                    }
                }
            }
        }

        private void tsbOptions_Click(object sender, EventArgs e)
        {
            using (OptionsDialog dlg = new OptionsDialog(m_center))
            {
                dlg.ShowDialog();

                if (dlg.ReloadRequired)
                {
                    this.Refresh();
                    RefreshTree();
                }
            }
        }

        private void tsbEditStyles_Click(object sender, EventArgs e)
        {
            using (StyleDialog dlg = new StyleDialog(m_center))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    UpdateStyleList();
                }
            }
        }

        private void grdRule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == colStyle.Index)
            {
                grdRule[e.ColumnIndex, e.RowIndex].Value = String.Empty;
            }
        }

        private void btnOpenUrl_Click(object sender, EventArgs e)
        {
            if (tbxUrl.Text.Length > 0)
            {
                string url = ScoreParser.ParseUrl(tbxUrl.Text);
                Process.Start(url);
            }
        }

        private class ScoreNodeComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;
                if (tx.Nodes.Count > 0)
                    return String.Compare(tx.Text, ty.Text);

                Score scx = tx.Tag as Score;
                Score scy = ty.Tag as Score;
                if (scx == null && scy == null)
                    return 0;
                if (scx == null) return -1;
                if (scy == null) return 1;
                return scx.CompareTo(scy);
            }

            #endregion
        }

        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            TreeNode node = tvwScores.SelectedNode;
            if (node == null || node.PrevNode == null)
                return;

            ReorderNodes(node);

            Score prevScore = node.PrevNode.Tag as Score;
            Score currScore = node.Tag as Score;
            int tmp = currScore.Order;
            currScore.Order = prevScore.Order;
            prevScore.Order = tmp;

            tvwScores.Sort();
            tvwScores.SelectedNode = node;
        }

        private void tsbMoveDown_Click(object sender, EventArgs e)
        {
            TreeNode node = tvwScores.SelectedNode;
            if (node == null || node.NextNode == null)
                return;

            ReorderNodes(node);

            Score nextScore = node.NextNode.Tag as Score;
            Score currScore = node.Tag as Score;
            int tmp = currScore.Order;
            currScore.Order = nextScore.Order;
            nextScore.Order = tmp;

            tvwScores.Sort();
            tvwScores.SelectedNode = node;
        }

        private static void ReorderNodes(TreeNode node)
        {
            int i = 1;
            foreach (TreeNode n in node.Parent.Nodes)
            {
                Score score = n.Tag as Score;
                score.Order = i++;
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

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlignColumn(ContentAlignment.MiddleLeft);
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlignColumn(ContentAlignment.MiddleCenter);
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlignColumn(ContentAlignment.MiddleRight);
        }

        private void AlignColumn(ContentAlignment alignement)
        {
            Point pt = contextMenuStrip1.Location;
            pt = pnlTest.PointToClient(pt);
            Control ctr = pnlTest.GetChildAtPoint(pt);
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

            foreach (Label c in pnlTest.Controls)
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
    }
}
