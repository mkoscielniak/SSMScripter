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
            // OptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.cbScriptDatabaseContext);
            this.Name = "OptionsControl";
            this.Size = new System.Drawing.Size(255, 220);
            this.Load += new System.EventHandler(this.OptionsControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbScriptDatabaseContext;
        private System.Windows.Forms.Label lblInfo;
    }
}
