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
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace MediaPortal.Plugin.ScoreCenter
{
    /// <summary>
    /// ThreeStateTreeNode inherits from <see cref="http://msdn2.microsoft.com/en-us/library/sc9ba94b(vs.80).aspx">TreeView</see>
    /// and adds the ability to cascade state changes to related nodes, i.e. child nodes and or parent nodes, as well as to optionally
    /// use the three state functionality.
    /// </summary>
    public class ThreeStateTreeView : TreeView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ThreeStateTreeView class in addition to intializing
        /// the base class (<see cref="http://msdn2.microsoft.com/en-us/library/system.windows.forms.treeview.treeview(VS.80).aspx">TreeView Constructor</see>). 
        /// </summary>
        public ThreeStateTreeView() : base()
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Flag. If true, use three state checkboxes, otherwise, use the default behavior of the TreeView and associated TreeNodes.
        /// </summary>
        private bool mUseThreeStateCheckBoxes = true;
        [Category("Three State TreeView"), 
        Description("Flag. If true, use three state checkboxes, otherwise, use the default behavior of the TreeView and associated TreeNodes."), 
        DefaultValue(true), 
        TypeConverter(typeof(bool)), 
        Editor("System.Boolean", typeof(bool))]
        public bool UseThreeStateCheckBoxes
        {
            get { return this.mUseThreeStateCheckBoxes; }
            set { this.mUseThreeStateCheckBoxes = value; }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Raises the AfterCheck event.
        /// </summary>
        /// <param name="e">A <see cref="http://msdn2.microsoft.com/en-us/library/system.windows.forms.treevieweventargs.aspx">TreeViewEventArgs</see> containing the event data.</param>
        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);

            if (this.UseThreeStateCheckBoxes)
            {
                switch (e.Action)
                {
                    case TreeViewAction.ByKeyboard:
                    case TreeViewAction.ByMouse:
                        {
                            if (e.Node is ThreeStateTreeNode)
                            {
                                // Toggle to the next state.
                                ThreeStateTreeNode tn = e.Node as ThreeStateTreeNode;
                                tn.Toggle();
                            }

                            break;
                        }
                    case TreeViewAction.Collapse:
                    case TreeViewAction.Expand:
                    case TreeViewAction.Unknown:
                    default:
                        {
                            // Do nothing.
                            break;
                        }
                }
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);
            e.DrawDefault = true;

            ThreeStateTreeNode node = e.Node as ThreeStateTreeNode;
            if (node != null)
            {
                switch (node.State)
                {
                    case CheckBoxState.Indeterminate:
                        e.Node.NodeFont = new Font(this.Font, FontStyle.Italic);
                        e.Node.ForeColor = this.ForeColor;
                        break;
                    case CheckBoxState.Unchecked:
                        e.Node.NodeFont = this.Font;
                        e.Node.ForeColor = Color.Gray;
                        break;
                    case CheckBoxState.Checked:
                        e.Node.NodeFont = this.Font;
                        e.Node.ForeColor = this.ForeColor;
                        break;
                }
            }
        }

        #endregion

        public IList<ThreeStateTreeNode> GetCheckedNodes()
        {
            return GetCheckedNodes(this.Nodes, new List<ThreeStateTreeNode>());
        }

        private IList<ThreeStateTreeNode> GetCheckedNodes(TreeNodeCollection nodes, IList<ThreeStateTreeNode> checkedNodes)
        {
            foreach (ThreeStateTreeNode node in nodes)
            {
                if (node.Checked && !checkedNodes.Contains(node))
                {
                    checkedNodes.Add(node);
                }

                GetCheckedNodes(node.Nodes, checkedNodes);
            }

            return checkedNodes;
        }
    }
}
