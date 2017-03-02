namespace VectorDrawApp
{
    partial class frmVD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVD));
            this.VD = new vdControls.vdFramedControl();
            this.grpxCond = new System.Windows.Forms.GroupBox();
            this.btnDisNode = new System.Windows.Forms.Button();
            this.grpxItemList = new System.Windows.Forms.GroupBox();
            this.tree = new System.Windows.Forms.TreeView();
            this.ctxTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSelectAllSub = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUnSelectAllSub = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGetDescription = new System.Windows.Forms.ToolStripMenuItem();
            this.btnVisibleProperty = new System.Windows.Forms.Button();
            this.btnFindById = new System.Windows.Forms.Button();
            this.txtHandleId = new System.Windows.Forms.TextBox();
            this.lblHandleId = new System.Windows.Forms.Label();
            this.tsmiExport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSepExport = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiImport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.grpxCond.SuspendLayout();
            this.grpxItemList.SuspendLayout();
            this.ctxTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // VD
            // 
            this.VD.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            resources.ApplyResources(this.VD, "VD");
            this.VD.DisplayPolarCoord = false;
            this.VD.HistoryLines = ((uint)(3u));
            this.VD.Name = "VD";
            this.VD.PropertyGridWidth = ((uint)(300u));
            // 
            // grpxCond
            // 
            resources.ApplyResources(this.grpxCond, "grpxCond");
            this.grpxCond.Controls.Add(this.btnDisNode);
            this.grpxCond.Controls.Add(this.grpxItemList);
            this.grpxCond.Controls.Add(this.btnVisibleProperty);
            this.grpxCond.Controls.Add(this.btnFindById);
            this.grpxCond.Controls.Add(this.txtHandleId);
            this.grpxCond.Controls.Add(this.lblHandleId);
            this.grpxCond.Name = "grpxCond";
            this.grpxCond.TabStop = false;
            // 
            // btnDisNode
            // 
            resources.ApplyResources(this.btnDisNode, "btnDisNode");
            this.btnDisNode.Name = "btnDisNode";
            this.btnDisNode.UseVisualStyleBackColor = true;
            this.btnDisNode.Click += new System.EventHandler(this.btnDisNode_Click);
            // 
            // grpxItemList
            // 
            resources.ApplyResources(this.grpxItemList, "grpxItemList");
            this.grpxItemList.Controls.Add(this.tree);
            this.grpxItemList.Name = "grpxItemList";
            this.grpxItemList.TabStop = false;
            // 
            // tree
            // 
            this.tree.CheckBoxes = true;
            this.tree.ContextMenuStrip = this.ctxTree;
            resources.ApplyResources(this.tree, "tree");
            this.tree.ItemHeight = 16;
            this.tree.Name = "tree";
            this.tree.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeCheck);
            this.tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterCheck);
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseClick);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            // 
            // ctxTree
            // 
            this.ctxTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelectAllSub,
            this.tsmiUnSelectAllSub,
            this.tsmiGetDescription,
            this.tsmiSepExport,
            this.tsmiDelete,
            this.tsmiExport,
            this.tsmiImport});
            this.ctxTree.Name = "ctxTree";
            resources.ApplyResources(this.ctxTree, "ctxTree");
            this.ctxTree.Opening += new System.ComponentModel.CancelEventHandler(this.ctxTree_Opening);
            // 
            // tsmiSelectAllSub
            // 
            this.tsmiSelectAllSub.Name = "tsmiSelectAllSub";
            resources.ApplyResources(this.tsmiSelectAllSub, "tsmiSelectAllSub");
            this.tsmiSelectAllSub.Click += new System.EventHandler(this.tsmiSelectAllSub_Click);
            // 
            // tsmiUnSelectAllSub
            // 
            this.tsmiUnSelectAllSub.Name = "tsmiUnSelectAllSub";
            resources.ApplyResources(this.tsmiUnSelectAllSub, "tsmiUnSelectAllSub");
            this.tsmiUnSelectAllSub.Click += new System.EventHandler(this.tsmiUnSelectAllSub_Click);
            // 
            // tsmiGetDescription
            // 
            this.tsmiGetDescription.Name = "tsmiGetDescription";
            resources.ApplyResources(this.tsmiGetDescription, "tsmiGetDescription");
            this.tsmiGetDescription.Click += new System.EventHandler(this.tsmiGetDescription_Click);
            // 
            // btnVisibleProperty
            // 
            resources.ApplyResources(this.btnVisibleProperty, "btnVisibleProperty");
            this.btnVisibleProperty.Name = "btnVisibleProperty";
            this.btnVisibleProperty.UseVisualStyleBackColor = true;
            this.btnVisibleProperty.Click += new System.EventHandler(this.btnVisibleProperty_Click);
            // 
            // btnFindById
            // 
            resources.ApplyResources(this.btnFindById, "btnFindById");
            this.btnFindById.Name = "btnFindById";
            this.btnFindById.UseVisualStyleBackColor = true;
            this.btnFindById.Click += new System.EventHandler(this.btnFindById_Click);
            // 
            // txtHandleId
            // 
            resources.ApplyResources(this.txtHandleId, "txtHandleId");
            this.txtHandleId.Name = "txtHandleId";
            // 
            // lblHandleId
            // 
            resources.ApplyResources(this.lblHandleId, "lblHandleId");
            this.lblHandleId.Name = "lblHandleId";
            // 
            // tsmiExport
            // 
            this.tsmiExport.Name = "tsmiExport";
            resources.ApplyResources(this.tsmiExport, "tsmiExport");
            this.tsmiExport.Click += new System.EventHandler(this.tsmiExport_Click);
            // 
            // tsmiSepExport
            // 
            this.tsmiSepExport.Name = "tsmiSepExport";
            resources.ApplyResources(this.tsmiSepExport, "tsmiSepExport");
            // 
            // tsmiImport
            // 
            this.tsmiImport.Name = "tsmiImport";
            resources.ApplyResources(this.tsmiImport, "tsmiImport");
            this.tsmiImport.Click += new System.EventHandler(this.tsmiImport_Click);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            resources.ApplyResources(this.tsmiDelete, "tsmiDelete");
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // frmVD
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.grpxCond);
            this.Controls.Add(this.VD);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "frmVD";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVD_Load);
            this.grpxCond.ResumeLayout(false);
            this.grpxCond.PerformLayout();
            this.grpxItemList.ResumeLayout(false);
            this.ctxTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public vdControls.vdFramedControl VD;
        private System.Windows.Forms.GroupBox grpxCond;
        private System.Windows.Forms.TextBox txtHandleId;
        private System.Windows.Forms.Label lblHandleId;
        private System.Windows.Forms.Button btnFindById;
        private System.Windows.Forms.Button btnVisibleProperty;
        private System.Windows.Forms.GroupBox grpxItemList;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.Button btnDisNode;
        private System.Windows.Forms.ContextMenuStrip ctxTree;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAllSub;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnSelectAllSub;
        private System.Windows.Forms.ToolStripMenuItem tsmiGetDescription;
        private System.Windows.Forms.ToolStripSeparator tsmiSepExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiImport;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
    }
}