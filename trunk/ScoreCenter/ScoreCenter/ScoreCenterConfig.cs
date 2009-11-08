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
        /// Contructor.
        /// </summary>
        public ScoreCenterConfig()
        {
            InitializeComponent();
            
            m_parser = new ScoreParser(0);
            tvwScores.TreeViewNodeSorter = new ScoreNodeComparer();

#if DEBUG
            this.ShowInTaskbar = true;
#endif
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
        public static void BuildScoreList2(ThreeStateTreeView tree, ScoreCenter center, bool show)
        {
            tree.Nodes.Clear();
            if (center == null || center.Scores == null)
                return;

            foreach (Score score in center.Scores)
            {
                // create category node if necessary
                if (tree.Nodes.ContainsKey(score.Category) == false)
                {
                    ThreeStateTreeNode cnode = new ThreeStateTreeNode(score.Category);
                    cnode.Name = score.Category;
                    tree.Nodes.Add(cnode);
                }

                TreeNode cat = tree.Nodes[score.Category];

                // create league node if necessary
                if (cat.Nodes.ContainsKey(score.Ligue) == false)
                {
                    ThreeStateTreeNode lnode = new ThreeStateTreeNode(score.Ligue);
                    lnode.Name = score.Ligue;
                    cat.Nodes.Add(lnode);
                }

                TreeNode league = cat.Nodes[score.Ligue];

                // create the score node
                ThreeStateTreeNode snode = new ThreeStateTreeNode(score.Name);
                league.Nodes.Add(snode);
                snode.Tag = score;
                if (score.enable && show)
                {
                    snode.State = CheckBoxState.Checked;
                    snode.UpdateStateOfRelatedNodes();
                }
            }

            tree.TreeViewNodeSorter = new ScoreNodeComparer();
            tree.Sort();
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
            RefreshTree(String.Empty);
        }
        private void RefreshTree(string path)
        {
            if (tvwScores.Nodes.Count == 0)
                return;

            // keep selected node and expanded nodes
            IList<string> keys = new List<string>();
            string selectedPath = path;
            if (String.IsNullOrEmpty(path) && tvwScores.SelectedNode != null)
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
                SetIcon(score.Image);

                grdRule.Enabled = true;
                SetRules(score);
            }
            else
            {
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

            tsbCopyScore.Enabled = (tvwScores.SelectedNode.Level == 2);
            tsbMoveUp.Enabled = tsbCopyScore.Enabled && tvwScores.SelectedNode.PrevNode != null;
            tsbMoveDown.Enabled = tsbCopyScore.Enabled && tvwScores.SelectedNode.NextNode != null;
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
                    st == null ? "" : rule.Format);
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

                if (tbxSkip.Text.Length == 0) score.Skip = 0;
                else score.Skip = int.Parse(tbxSkip.Text);

                if (tbxMaxLines.Text.Length == 0) score.MaxLines = 0;
                else score.MaxLines = int.Parse(tbxMaxLines.Text);

                SaveRules(score);

                if (refresh)
                {
                    RefreshTree();
                }
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

                score.Element = tbxElement.Text;
                score.Skip = ReadInt(tbxSkip);
                score.MaxLines = ReadInt(tbxMaxLines);

                // read and parse the score
                string[][] lines = m_parser.Read(score, ckxReload.Checked);

                int nbColumns = 0;
                grdTest.Columns.Clear();
                if (lines != null)
                {
                    foreach (string[] ss in lines)
                    {
                        if (ss != null)
                        {
                            nbColumns = Math.Max(nbColumns, ss.Length);
                        }
                    }

                    for (int i = 0; i < Math.Min(20, nbColumns); i++)
                    {
                        grdTest.Columns.Add(i.ToString(), i.ToString());
                    }

                    foreach (string[] ss in lines)
                    {
                        if (ss != null)
                        {
                            grdTest.Rows.Add(ss);
                        }
                    }

                    tabScore.SelectedTab = tpgTest;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
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

            TreeNode node = null;
            if (tvwScores.SelectedNode == null)
            {
                if (tvwScores.Nodes.Count == 0)
                {
                    tvwScores.Nodes.Add(newName);
                }

                node = tvwScores.Nodes[0];
            }
            else
            {
                node = tvwScores.SelectedNode;
            }

            string category = newName;
            string league = newName;
            string name = newName;

            switch (node.Level)
            {
                case 1:
                    category = node.Parent.Text;
                    break;
                case 2:
                    category = node.Parent.Parent.Text;
                    league = node.Parent.Text;
                    break;
            }

            // create new item
            Score score = new Score();
            score.Id = Tools.GenerateId();
            score.Category = category;
            score.Ligue = league;
            score.Name = name;

            // add the new item and refresh
            m_center.AddScore(score);
            RefreshTree(score.ScorePath);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            string result = String.Empty;
            for (int i = 0; i < grdTest.Columns.Count; i++)
            {
                int max = 0;
                foreach (DataGridViewRow row in grdTest.Rows)
                {
                    if (row.Cells.Count > i)
                    {
                        int v = (row.Cells[i].Value == null ? 0 : row.Cells[i].Value.ToString().Length);
                        max = Math.Max(max, v);
                    }
                }

                if (result.Length > 0) result += ",";
                result += max.ToString();
            }

            tbxSizes.Text = result;
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

                RefreshTree();
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
            RefreshTree(copy.ScorePath);
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
            Tools.ColumnDisplay[] sizes = Tools.GetSizes(tbxSizes.Text);

            if (sizes != null)
            {
                res = sizes.Sum(col => Math.Abs(col.Size));
            }

            lblTotalSize.Text = String.Format("Total Size = {0}", res);
        }
    }
}
