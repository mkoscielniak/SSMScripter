namespace SSMScripter.Config
{
    partial class OptionsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbScriptDatabaseContext = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.gbRunner = new System.Windows.Forms.GroupBox();
            this.btnEditRunner = new System.Windows.Forms.Button();
            this.lblRunnerArgs = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRunnerTool = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbRunner.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbScriptDatabaseContext
            // 
            this.cbScriptDatabaseContext.AutoSize = true;
            this.cbScriptDatabaseContext.Location = new System.Drawing.Point(19, 13);
            this.cbScriptDatabaseContext.Name = "cbScriptDatabaseContext";
            this.cbScriptDatabaseContext.Size = new System.Drawing.Size(138, 17);
            this.cbScriptDatabaseContext.TabIndex = 0;
            this.cbScriptDatabaseContext.Text = "Script database context";
            this.cbScriptDatabaseContext.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfo.Location = new System.Drawing.Point(0, 159);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(255, 61);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "{INFO}";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // gbRunner
            // 
            this.gbRunner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRunner.Controls.Add(this.btnEditRunner);
            this.gbRunner.Controls.Add(this.lblRunnerArgs);
            this.gbRunner.Controls.Add(this.label3);
            this.gbRunner.Controls.Add(this.lblRunnerTool);
            this.gbRunner.Controls.Add(this.label1);
            this.gbRunner.Location = new System.Drawing.Point(19, 37);
            this.gbRunner.Name = "gbRunner";
            this.gbRunner.Size = new System.Drawing.Size(220, 90);
            this.gbRunner.TabIndex = 2;
            this.gbRunner.TabStop = false;
            this.gbRunner.Text = "Runner";
            // 
            // btnEditRunner
            // 
            this.btnEditRunner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditRunner.Location = new System.Drawing.Point(164, 0);
            this.btnEditRunner.Name = "btnEditRunner";
            this.btnEditRunner.Size = new System.Drawing.Size(50, 23);
            this.btnEditRunner.TabIndex = 4;
            this.btnEditRunner.Text = "Edit";
            this.btnEditRunner.UseVisualStyleBackColor = true;
            this.btnEditRunner.Click += new System.EventHandler(this.btnEditRunner_Click);
            // 
            // lblRunnerArgs
            // 
            this.lblRunnerArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRunnerArgs.Location = new System.Drawing.Point(16, 68);
            this.lblRunnerArgs.Name = "lblRunnerArgs";
            this.lblRunnerArgs.Size = new System.Drawing.Size(198, 13);
            this.lblRunnerArgs.TabIndex = 3;
            this.lblRunnerArgs.Text = "{ARGS}";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Args";
            // 
            // lblRunnerTool
            // 
            this.lblRunnerTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRunnerTool.Location = new System.Drawing.Point(19, 33);
            this.lblRunnerTool.Name = "lblRunnerTool";
            this.lblRunnerTool.Size = new System.Drawing.Size(195, 13);
            this.lblRunnerTool.TabIndex = 1;
            this.lblRunnerTool.Text = "{TOOL}";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tool";
            // 
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbRunner);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.cbScriptDatabaseContext);
            this.Name = "OptionsControl";
            this.Size = new System.Drawing.Size(255, 220);
            this.Load += new System.EventHandler(this.OptionsControl_Load);
            this.gbRunner.ResumeLayout(false);
            this.gbRunner.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbScriptDatabaseContext;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox gbRunner;
        private System.Windows.Forms.Button btnEditRunner;
        private System.Windows.Forms.Label lblRunnerArgs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblRunnerTool;
        private System.Windows.Forms.Label label1;
    }
}
