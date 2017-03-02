using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VectorDrawApp.MatchingLib;
using VectorDraw.Professional.vdPrimaries;
using VectorDrawApp.Commands;
using VectorDrawApp.VdUtils;

namespace VectorDrawApp
{
    public partial class frmVD : Form
    {
        public frmVD()
        {
            InitializeComponent();
        }

        #region 窗口事件

        // 窗体载入
        private void frmVD_Load(object sender, EventArgs e)
        {
            VD.BaseControl.ActiveDocument.UCS("VIEW");
            VD.CommandLine.CommandExecute -= CommandLine_CommandExecute;
            VD.CommandLine.CommandExecute += CommandLine_CommandExecute;

            VD.BaseControl.vdKeyUp -= BaseControl_vdKeyUp;
            VD.BaseControl.vdKeyUp += BaseControl_vdKeyUp;
        }
        #endregion

        #region "VD控件的事件"
        /// <summary>
        /// execute system command
        /// </summary>
        /// <param name="commandname"></param>
        /// <param name="isDefaultImplemented"></param>
        /// <param name="success"></param>
        private void CommandLine_CommandExecute(string commandname, bool isDefaultImplemented, ref bool success)
        {
            if (isDefaultImplemented)
                return;
            VectorDrawCommand command;
            switch (commandname)
            {
                case "cls":
                    VD.CommandLine.History.Clear();
                    break;
                case "11":
                    command = new LocateGridSelectionCommand();
                    command.Execute(VD);
                    success = true;
                    break;
                case "22":
                    command = new FindSimilarsCommand();
                    var matchItem = command.Execute(VD) as MatchItem;
                    if (matchItem != null)
                    {
                        AddMatchItem2Tree(matchItem);
                        VD.CommandLine.History.AppendText("\r\n");
                        VD.CommandLine.History.AppendText($"Execute {command.CommandName}, result count={matchItem.Results.Count}");
                        VD.CommandLine.History.AppendText("\r\n");
                    }
                    success = true;
                    break;
                case "33":
                    command = new DetailSelectionCommand();
                    var sbDetail = command.Execute(VD) as StringBuilder;
                    if (sbDetail != null)
                    {
                        VD.CommandLine.History.AppendText("\r\n");
                        VD.CommandLine.History.AppendText($"Execute {command.CommandName}, result =\r\n{sbDetail}");
                        VD.CommandLine.History.AppendText("\r\n");
                    }
                    break;
            }
        }

        private void BaseControl_vdKeyUp(KeyEventArgs e, ref bool cancel)
        {

        }
        #endregion

        private void btnFindById_Click(object sender, EventArgs e)
        {
            var handles = GetHandles(txtHandleId.Text.Trim());
            if (handles.Count == 0)
            {
                MessageBox.Show($"输入参数错误，无法识别为HandleId");
                return;
            }
            var document = VD.BaseControl.ActiveDocument;
            var figures = new List<vdFigure>();
            foreach (var handleId in handles)
            {
                var figure = VdSqlUtil.GetFigureByHandle(document, handleId);
                if (figure == null)
                {
                    MessageBox.Show($"没有找到HandleId={handleId}的图元");
                    return;
                }
                figures.Add(figure);
            }

            VdProUtil.SelectFigures(document, figures);
            VdProUtil.LocateFigures(document, figures);
            VdProUtil.RefreshVectorDraw(document);
        }

        private void btnVisibleProperty_Click(object sender, EventArgs e)
        {
            VdControlUtil.SmartVisibleLayout(VD, vdControls.vdFramedControl.LayoutStyle.PropertyGrid);
        }

        private List<ulong> GetHandles(string handlesText)
        {
            var handleList = new List<ulong>();
            var sepArr = new[] { " ", "\r", ",", "，", ";", "；" };
            foreach (var sep in sepArr)
            {
                if (!handlesText.Contains(sep))
                    continue;
                var strArr = txtHandleId.Text.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length <= 0)
                    continue;
                foreach (var str in strArr)
                {
                    ulong handleId;
                    if (ulong.TryParse(str, out handleId))
                        handleList.Add(handleId);
                }
            }
            if (handleList.Count == 0)
            {
                ulong handleId;
                if (ulong.TryParse(handlesText, out handleId))
                    handleList.Add(handleId);
            }
            return handleList;
        }

        private void AddMatchItem2Tree(MatchItem matchItem)
        {
            var nodeMatchItem = new TreeNode($"{matchItem.Name}, results={matchItem.Results.Count}") { Tag = matchItem };
            var nodeSample = new TreeNode($"Sample, set={matchItem.Sample.Entities.Count}") { Tag = matchItem.Sample };
            nodeMatchItem.Nodes.Add(nodeSample);
            foreach (var result in matchItem.Results)
            {
                var nodeResult = new TreeNode($"MatchResult, set={result.Entities.Distinct().Count()}") { Tag = result };
                nodeMatchItem.Nodes.Add(nodeResult);
            }
            nodeMatchItem.ExpandAll();
            tree.Nodes.Add(nodeMatchItem);
            grpxItemList.Text = $"匹配结果，共匹配{tree.Nodes.Count}次";
        }

        private void tree_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            //if (e.Node.Tag is FigureSet == false)
            //    e.Cancel = true;
        }

        private void tree_AfterCheck(object sender, TreeViewEventArgs e)
        {

        }

        private void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var figureSet = e.Node.Tag as FigureSet;
            if (figureSet == null)
                return;
            VdProUtil.SelectFigures(VD.BaseControl.ActiveDocument, figureSet.Entities);
            VdProUtil.LocateFigures(VD.BaseControl.ActiveDocument, figureSet.Entities);
            VdProUtil.RefreshVectorDraw(VD.BaseControl.ActiveDocument);
        }

        private void btnDisNode_Click(object sender, EventArgs e)
        {
            var list = new List<vdFigure>();
            foreach (TreeNode node in tree.Nodes)
            {
                foreach (TreeNode subNode in node.Nodes)
                {
                    if (!subNode.Checked)
                        continue;
                    var figureSet = subNode.Tag as FigureSet;
                    if (figureSet == null)
                        continue;
                    list.AddRange(figureSet.Entities);
                }
            }
            VdProUtil.SelectFigures(VD.BaseControl.ActiveDocument, list);
            VdProUtil.LocateFigures(VD.BaseControl.ActiveDocument, list);
            VdProUtil.RefreshVectorDraw(VD.BaseControl.ActiveDocument);
        }

        private void tsmiSelectAllSub_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode.Tag is MatchItem == false)
                return;
            tree.SuspendLayout();
            foreach (TreeNode node in tree.SelectedNode.Nodes)
            {
                node.Checked = true;
            }
            tree.ResumeLayout(true);
        }

        private void tsmiUnSelectAllSub_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode.Tag != null)
                return;
            tree.SuspendLayout();
            foreach (TreeNode node in tree.SelectedNode.Nodes)
            {
                node.Checked = false;
            }
            tree.ResumeLayout(true);
        }

        private void ctxTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tree.SelectedNode == null)
            {
                e.Cancel = true;
                return;
            }
            var nodeIsFigureSet = tree.SelectedNode.Tag is FigureSet;
            tsmiSelectAllSub.Enabled = !nodeIsFigureSet;
            tsmiUnSelectAllSub.Enabled = !nodeIsFigureSet;

            var nodeIsMatchItem = tree.SelectedNode.Tag is MatchItem;
            tsmiDelete.Enabled = nodeIsMatchItem;
            //tsmiGetDescription.Visible = nodeIsFigureSet;
        }

        private void tsmiGetDescription_Click(object sender, EventArgs e)
        {
            var desc = string.Empty;
            var figureSet = tree.SelectedNode.Tag as FigureSet;
            if (figureSet != null)
                desc = figureSet.GetDescription();
            var matchItem = tree.SelectedNode.Tag as MatchItem;
            if (matchItem != null)
                desc = matchItem.GetDescription();
            if (string.IsNullOrWhiteSpace(desc))
            {
                MessageBox.Show($"没有获得任何描述");
                return;
            }
            Clipboard.SetText(desc);
            MessageBox.Show($"图元集描述已保存到剪切板");
        }

        private void tsmiExport_Click(object sender, EventArgs e)
        {
            var list = (from TreeNode node in tree.Nodes where node.Checked select node.Tag).OfType<MatchItem>().ToList();
            var xml = new XmlProvider().BuildXml(list);
            if (string.IsNullOrWhiteSpace(xml))
            {
                MessageBox.Show($"没有获得任何Xml字符串");
                return;
            }
            Clipboard.SetText(xml);
            MessageBox.Show($"Xml字符串已保存到剪切板");
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            var matchItem = tree.SelectedNode.Tag as MatchItem;
            if (matchItem == null)
            {
                MessageBox.Show($"没有选择匹配项，无法进行删除");
                return;
            }
            tree.Nodes.Remove(tree.SelectedNode);
            MessageBox.Show($"删除成功");
        }

        private void tsmiImport_Click(object sender, EventArgs e)
        {
            var xml = Clipboard.GetText();
            if (string.IsNullOrEmpty(xml))
            {
                MessageBox.Show($"剪切板中没有任何文本数据");
                return;
            }
            var list = new XmlProvider().ParseXml(xml, VD.BaseControl.ActiveDocument);
            foreach (var matchItem in list)
            {
                AddMatchItem2Tree(matchItem);
            }
            MessageBox.Show($"成功导入{list.Count}条数据");
        }
    }
}
