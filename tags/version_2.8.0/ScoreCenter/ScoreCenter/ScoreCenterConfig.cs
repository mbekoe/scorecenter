#region

/* 
 *      Copyright (C) 2009-2013 Team MediaPortal
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Plugin.ScoreCenter.Editor;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// Score Center configuration dialog.
    /// </summary>
    public partial class ScoreCenterConfig : Form
    {
        private ScoreCenter m_center;
        private string m_settings;
        private BaseScore m_scoreSettings = null;

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
            
            tvwScores.TreeViewNodeSorter = new ScoreNodeComparer();

            this.ShowInTaskbar = showInTaskbar;
        }

        private void ScoreCenterConfig_Load(object sender, EventArgs e)
        {
            try
            {
                m_settings = Config.GetFile(Config.Dir.Config, ScoreCenterPlugin.SettingsFileName);
                m_center = Tools.ReadSettings(m_settings, true);

                BuildScoreList(tvwScores, m_center, true);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (m_center.Scores.Items == null || m_center.Scores.Items.Length == 0)
            {
                DialogResult res = MessageBox.Show("The settings is empty, do you want to download the online settings?",
                    "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    ShowOptions(true);
                }
            }
        }

        public static void BuildScoreList(ThreeStateTreeView tree, ScoreCenter center, bool show)
        {
            tree.Nodes.Clear();
            if (center == null || center.Scores.Items == null)
                return;

            tree.BeginUpdate();
            Dictionary<string, ThreeStateTreeNode> nodes = new Dictionary<string, ThreeStateTreeNode>(center.Scores.Items.Count());
            foreach (BaseScore sc in center.Scores.Items)
            {
                ThreeStateTreeNode node = new ThreeStateTreeNode(sc.Name);
                node.Tag = sc;
                nodes.Add(sc.Id, node);
                if (!sc.IsFolder() && sc.enable && show)
                {
                    node.Checked = true;
                    node.State = CheckBoxState.Checked;
                }
            }

            foreach (KeyValuePair<string, ThreeStateTreeNode> pair in nodes)
            {
                ThreeStateTreeNode node = pair.Value;
                BaseScore sc = node.Tag as BaseScore;
                if (String.IsNullOrEmpty(sc.Parent))
                {
                    tree.Nodes.Add(node);
                }
                else
                {
                    if (nodes.ContainsKey(sc.Parent))
                    {
                        nodes[sc.Parent].Nodes.Add(node);
                    }
                }
            }

            RefreshTreeState(tree.Nodes);
            tree.EndUpdate();
            tree.Sort();
        }

        private static void RefreshTreeState(TreeNodeCollection nodes)
        {
            foreach (ThreeStateTreeNode node in nodes)
            {
                if (node.Nodes.Count == 0)
                    node.UpdateStateOfRelatedNodes();
                RefreshTreeState(node.Nodes);
            }
        }

        private void RefreshTree()
        {
            IList<string> keys = new List<string>();
            string selectedPath = String.Empty;
            if (tvwScores.Nodes.Count != 0)
            {
                // keep selected node and expanded nodes
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
            }

            // rebuild tree
            BuildScoreList(tvwScores, m_center, true);

            // reselect
            if (!String.IsNullOrEmpty(selectedPath))
            {
                TreeNode selected = SelectNodes(tvwScores.Nodes, keys, selectedPath);
                if (selected == null && tvwScores.Nodes.Count > 0) selected = tvwScores.Nodes[0];
                tvwScores.SelectedNode = selected;
            }
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

        private void tvwScores_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = !SaveScore();
        }
        private bool SaveScore()
        {
            if (tvwScores.SelectedNode == null
                || tvwScores.SelectedNode.Tag == null)
                return true;

            bool res = true;
            BaseScoreEditor editor = GetEditor();
            if (editor != null)
            {
                BaseScore score = tvwScores.SelectedNode.Tag as BaseScore;
                if (!editor.SaveScore(ref score))
                {
                    // errors while saving Cancel selection
                    res = false;
                }
                else
                {
                    if (tvwScores.SelectedNode.Text != score.Name)
                    {
                        tvwScores.SelectedNode.Text = score.Name;
                    }
                }
            }

            return res;
        }

        private void tvwScores_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            // always clear
            ClearIcon();

            BaseScore bscore = tvwScores.SelectedNode.Tag as BaseScore;

            BaseScoreEditor prevEditor = GetEditor();
            BaseScoreEditor editor = null;
            string editorType = ScoreFactory.Instance.GetEditorType(bscore);
            if (prevEditor != null)
            {
                if (editorType != prevEditor.GetType().Name)
                {
                    pnlEditor.Controls.Clear();
                    (prevEditor as Control).Dispose();
                }
                else
                {
                    editor = prevEditor;
                }
            }

            if (editor == null)
            {
                var zeditor = ScoreFactory.Instance.CreateEditor(editorType, pnlTest);
                pnlEditor.Controls.Add(zeditor);
                zeditor.Dock = DockStyle.Fill;
                editor = zeditor as BaseScoreEditor;
            }

            editor.LoadScore(bscore, m_center);
            SetIcon(bscore.Image);

            ClearTestGrid();
            btnTest.Enabled = editor.HasTest;
            tsbMoveUp.Enabled = tvwScores.SelectedNode.PrevNode != null;
            tsbMoveDown.Enabled = tvwScores.SelectedNode.NextNode != null;
            tsbMoveBack.Enabled = tvwScores.SelectedNode.Parent != null;
            tsbMoveRight.Enabled = tsbMoveUp.Enabled;

            tsbCopySettings.Enabled = bscore.CanApplySettings();
            tsbApplySettings.Enabled = tsbCopySettings.Enabled && m_scoreSettings != null && bscore.GetType() == m_scoreSettings.GetType();
        }

        private BaseScoreEditor GetEditor()
        {
            if (pnlEditor.Controls.Count == 1)
                return pnlEditor.Controls[0] as BaseScoreEditor;
            return null;
        }

        private void ClearTestGrid()
        {
            pnlTest.Controls.Clear();
            pnlTest.Tag = null;
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

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            AboutDialog dlg = new AboutDialog(m_center);
            dlg.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(@"Warning unsaved changes will be lost.
Are you sure you want to quit ?", "Score Center", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!SaveScore())
                return;
            SaveConfig();
        }

        private void SaveConfig()
        {
            #region Order
            if (m_center.Scores != null)
            {
                m_center.Scores.Items = m_center.Scores.Items.OrderBy(sc => sc.Parent)
                    .ThenBy(sc => sc.Id)
                    .ToArray();
            }
            #endregion

            Tools.SaveSettings(m_settings, m_center, true, false);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            pnlTest.Controls.Clear();
            pnlTest.Refresh();

            try
            {
                this.Cursor = Cursors.WaitCursor;

                BaseScoreEditor editor = GetEditor();
                if (editor == null)
                    return;

                Type scoreType = editor.GetScoreType();
                ScoreFactory.Instance.CacheExpiration = m_center.Setup.CacheExpiration;
                BaseScore score = ScoreFactory.Instance.CreateScore(scoreType);
                if (!editor.SaveScore(ref score))
                    return;

                if (score.IsVirtualFolder())
                {
                    var zz = score.GetVirtualScores(m_center.Parameters);
                    using (TestScoreSelector dlg = new TestScoreSelector(zz))
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            score = dlg.SelectedScore;
                        }
                    }
                }

                // read and parse the score
                string[][] lines = ScoreFactory.Parse(score, ckxReload.Checked, m_center.Parameters);
                if (lines == null)
                    return;

                IScoreBuilder<Control> bld = ScoreFactory.Instance.GetBuilder<Control>(score);
                bld.Styles = m_center.Styles.ToList().AsReadOnly();
                bld.UseAltColor = m_center.Setup.UseAltColor;

                int fh = pnlTest.Font.Height;
                int fw = (int)pnlTest.Font.SizeInPoints;
                bld.SetFont("", m_center.Setup.DefaultFontColor, m_center.Setup.AltFontColor, 14, fw, fh);
                
                bool overRight = false;
                bool overDown = false;
                int lineNumber, colNumber;

                pnlTest.BackColor = Color.FromArgb(m_center.Setup.DefaultSkinColor);

                IList<Control> controls = bld.Build(score, lines,
                    0, 0,
                    0, 0, pnlTest.Width, 10000,
                    this.CreateControl,
                    out overRight, out overDown, out lineNumber, out colNumber);
                if (controls == null)
                    return;

                pnlTest.Tag = lines;

                pnlTest.SuspendLayout();
                pnlTest.Controls.AddRange(controls.Cast<Control>().ToArray());
            }
            catch (Exception exc)
            {
                MessageBox.Show(Tools.GetExceptionMessage(exc), Properties.Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void tsbNewItem_Click(object sender, EventArgs e)
        {
            TreeNode parentNode = null;
            BaseScore parent = null;
            if (tvwScores.SelectedNode != null)
            {
                parentNode = tvwScores.SelectedNode;
                parent = parentNode.Tag as BaseScore;
            }

            using (var dlg = new CreateScoreDlg(parent))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    BaseScore score = dlg.NewScore;
                    m_center.AddScore(score);
                    
                    // create the tree node
                    ThreeStateTreeNode newNode = new ThreeStateTreeNode(score.Name);
                    newNode.Checked = true;
                    newNode.State = CheckBoxState.Checked;
                    newNode.Tag = score;

                    if (String.IsNullOrEmpty(score.Parent))
                    {
                        tvwScores.Nodes.Add(newNode);
                    }
                    else
                    {
                        parentNode.Nodes.Add(newNode);
                    }
                }
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
            BaseScore score = node.Tag as BaseScore;
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
            if (tvwScores.SelectedNode == null)
                return;

            BaseScore source = tvwScores.SelectedNode.Tag as BaseScore;
            BaseScore copy = source.Clone(Tools.GenerateId());

            // add the new item and refresh
            m_center.AddScore(copy);

            // create node
            ThreeStateTreeNode node = new ThreeStateTreeNode(copy.Name);
            node.Tag = copy;
            if (tvwScores.SelectedNode.Parent != null)
            {
                tvwScores.SelectedNode.Parent.Nodes.Add(node);
            }
            else
            {
                tvwScores.Nodes.Add(node);
            }
        }

        private void tvwScores_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            
            BaseScore score = e.Node.Tag as BaseScore;
            score.Name = e.Label;
            //TODO tbxScore.Text = e.Label;
        }

        private void tvwScores_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                BaseScore score = e.Node.Tag as BaseScore;
                score.enable = e.Node.Checked;
            }
        }

        private void btnSetIcon_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            BaseScore score = tvwScores.SelectedNode.Tag as BaseScore;
            if (score == null)
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

                score.Image = iconName;
            }
        }

        private void btnClearIcon_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode == null)
                return;

            BaseScore score = tvwScores.SelectedNode.Tag as BaseScore;
            if (score != null)
            {
                score.Image = String.Empty;
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

        private void tsbCopySettings_Click(object sender, EventArgs e)
        {
            if (tvwScores.SelectedNode != null && tvwScores.SelectedNode.Tag != null)
            {
                m_scoreSettings = tvwScores.SelectedNode.Tag as BaseScore;
                tsbApplySettings.Enabled = true;
            }
        }

        private void tsbApplySettings_Click(object sender, EventArgs e)
        {
            if (m_scoreSettings == null) return;
            if (tvwScores.SelectedNode == null || tvwScores.SelectedNode.Tag == null) return;

            BaseScore score = tvwScores.SelectedNode.Tag as BaseScore;
            if (score != m_scoreSettings && score.GetType() == m_scoreSettings.GetType())
            {
                score.ApplySettings(m_scoreSettings);
                BaseScoreEditor editor = GetEditor();
                if (editor != null)
                    editor.LoadScore(score, m_center);
            }
        }

        private void tsbApplySettings_EnabledChanged(object sender, EventArgs e)
        {
            if (tsbApplySettings.Enabled)
            {
                tsbApplySettings.ToolTipText = String.Format("Apply Settings from {0}", m_scoreSettings.Name);
            }
            else
            {
                tsbApplySettings.ToolTipText = "Apply Settings";
            }
        }

        private void tsbOptions_Click(object sender, EventArgs e)
        {
            ShowOptions(false);
        }
        private void ShowOptions(bool selectUpdate)
        {
            using (OptionsDialog dlg = new OptionsDialog(m_center, selectUpdate))
            {
                dlg.ShowDialog();
                if (dlg.ReloadRequired)
                {
                    RefreshTree();
                }
            }
        }

        private void tsbEditStyles_Click(object sender, EventArgs e)
        {
            StyleDialog dlg = new StyleDialog(m_center);
            dlg.ShowDialog();
        }

        #region Move Nodes
        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            TreeNode node = tvwScores.SelectedNode;
            if (node == null || node.PrevNode == null)
                return;

            ReorderNodes(node);

            BaseScore prevScore = node.PrevNode.Tag as BaseScore;
            BaseScore currScore = node.Tag as BaseScore;
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

            BaseScore nextScore = node.NextNode.Tag as BaseScore;
            BaseScore currScore = node.Tag as BaseScore;
            int tmp = currScore.Order;
            currScore.Order = nextScore.Order;
            nextScore.Order = tmp;

            tvwScores.Sort();
            tvwScores.SelectedNode = node;
        }

        private void tsbMoveBack_Click(object sender, EventArgs e)
        {
            TreeNode node = tvwScores.SelectedNode;
            if (node == null || node.Parent == null)
                return;

            BaseScore parentScore = node.Parent.Tag as BaseScore;
            BaseScore currScore = node.Tag as BaseScore;

            currScore.Parent = parentScore.Parent;

            var gpNode = node.Parent.Parent;
            node.Parent.Nodes.Remove(node);
            if (gpNode == null)
            {
                tvwScores.Nodes.Add(node);
                currScore.Order = tvwScores.Nodes.Count + 1;
            }
            else
            {
                gpNode.Nodes.Add(node);
                currScore.Order = gpNode.Nodes.Count + 1;
            }

            ReorderNodes(node.PrevNode);
            tvwScores.Sort();
            tvwScores.SelectedNode = node;
        }

        private void tsbMoveRight_Click(object sender, EventArgs e)
        {
            TreeNode node = tvwScores.SelectedNode;
            if (node == null || node.PrevNode == null)
                return;

            TreeNode prevNode = node.PrevNode;
            BaseScore prevScore = prevNode.Tag as BaseScore;
            BaseScore currScore = node.Tag as BaseScore;

            ReorderNodes(node.PrevNode.FirstNode);
            currScore.Parent = prevScore.Id;
            currScore.Order = prevNode.Nodes.Count + 1;

            if (node.Parent == null)
                tvwScores.Nodes.Remove(node);
            else
                node.Parent.Nodes.Remove(node);
            
            prevNode.Nodes.Add(node);
            tvwScores.Sort();
            tvwScores.SelectedNode = node;
        }

        private static void ReorderNodes(TreeNode node)
        {
            if (node == null)
                return;

            int i = 1;
            var coll = (node.Parent == null ? node.TreeView.Nodes : node.Parent.Nodes);
            foreach (TreeNode n in coll)
            {
                BaseScore score = n.Tag as BaseScore;
                score.Order = i++;
            }
        }
        #endregion

        #region Context Menu
        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetEditor();
            if (editor != null)
            {
                editor.AlignColumn(pnlTest.PointToClient(contextMenuStrip1.Location), ContentAlignment.MiddleLeft);
            }
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetEditor();
            if (editor != null)
            {
                editor.AlignColumn(pnlTest.PointToClient(contextMenuStrip1.Location), ContentAlignment.MiddleCenter);
            }
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = GetEditor();
            if (editor != null)
            {
                editor.AlignColumn(pnlTest.PointToClient(contextMenuStrip1.Location), ContentAlignment.MiddleRight);
            }
        }
        #endregion

        private class ScoreNodeComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;
                BaseScore scx = tx.Tag as BaseScore;
                BaseScore scy = ty.Tag as BaseScore;

                if (scx == null && scy == null)
                    return 0;
                if (scx == null) return -1;
                if (scy == null) return 1;
                return scx.CompareToNoLoc(scy);
            }

            #endregion
        }
    }
}
