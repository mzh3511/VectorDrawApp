namespace Ranplan.iBuilding.TeighaApp.Forms
{
    partial class FormDwgReader
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.grpxHandle = new System.Windows.Forms.GroupBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.grpxView = new System.Windows.Forms.GroupBox();
            this.grpxHandle.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpxHandle
            // 
            this.grpxHandle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpxHandle.Controls.Add(this.btnLoadFile);
            this.grpxHandle.Location = new System.Drawing.Point(470, 3);
            this.grpxHandle.Name = "grpxHandle";
            this.grpxHandle.Size = new System.Drawing.Size(307, 485);
            this.grpxHandle.TabIndex = 0;
            this.grpxHandle.TabStop = false;
            this.grpxHandle.Text = "操作";
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(44, 30);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 0;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // grpxView
            // 
            this.grpxView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpxView.Location = new System.Drawing.Point(3, 3);
            this.grpxView.Name = "grpxView";
            this.grpxView.Size = new System.Drawing.Size(461, 485);
            this.grpxView.TabIndex = 1;
            this.grpxView.TabStop = false;
            this.grpxView.Text = "显示";
            this.grpxView.Paint += new System.Windows.Forms.PaintEventHandler(this.grpxView_Paint);
            // 
            // FormDwgReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 491);
            this.Controls.Add(this.grpxView);
            this.Controls.Add(this.grpxHandle);
            this.DoubleBuffered = true;
            this.Name = "FormDwgReader";
            this.Text = "Teigha .dwg Reader";
            this.Load += new System.EventHandler(this.FormDwgReader_Load);
            this.grpxHandle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpxHandle;
        private System.Windows.Forms.GroupBox grpxView;
        private System.Windows.Forms.Button btnLoadFile;
    }
}

