// CUSTOM: Mode C Tool customization to ease future upstream merges.
namespace Director.Extensions.ModeC
{
    partial class ModeCTool
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
            if (disposing)
            {
                components?.Dispose();
                // CUSTOM: Mode C Tool customization to ease future upstream merges.
                DetachCurrentSerial()?.Dispose();
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblPort = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblGuard = new System.Windows.Forms.Label();
            this.numGuard = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRead = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGuard)).BeginInit();
            this.flowLayoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.txtLog);
            this.splitContainer.Size = new System.Drawing.Size(800, 450);
            this.splitContainer.SplitterDistance = 260;
            this.splitContainer.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.lblPort, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.cmbPort, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.lblPassword, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.txtPassword, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.lblGuard, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.numGuard, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutButtons, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lblResult, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.txtResult, 1, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(260, 450);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPort.Location = new System.Drawing.Point(3, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(74, 30);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPort
            // 
            this.cmbPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(83, 3);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(174, 21);
            this.cmbPort.TabIndex = 1;
            this.cmbPort.DropDown += new System.EventHandler(this.cmbPort_DropDown);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassword.Location = new System.Drawing.Point(3, 30);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(74, 30);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Location = new System.Drawing.Point(83, 33);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(174, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // lblGuard
            // 
            this.lblGuard.AutoSize = true;
            this.lblGuard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuard.Location = new System.Drawing.Point(3, 60);
            this.lblGuard.Name = "lblGuard";
            this.lblGuard.Size = new System.Drawing.Size(74, 30);
            this.lblGuard.TabIndex = 4;
            this.lblGuard.Text = "Guard (ms):";
            this.lblGuard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numGuard
            // 
            this.numGuard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numGuard.Location = new System.Drawing.Point(83, 63);
            this.numGuard.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numGuard.Name = "numGuard";
            this.numGuard.Size = new System.Drawing.Size(174, 20);
            this.numGuard.TabIndex = 5;
            this.numGuard.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // flowLayoutButtons
            // 
            this.tableLayoutPanel.SetColumnSpan(this.flowLayoutButtons, 2);
            this.flowLayoutButtons.Controls.Add(this.btnRead);
            this.flowLayoutButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutButtons.Location = new System.Drawing.Point(3, 93);
            this.flowLayoutButtons.Name = "flowLayoutButtons";
            this.flowLayoutButtons.Size = new System.Drawing.Size(254, 34);
            this.flowLayoutButtons.TabIndex = 6;
            // 
            // btnRead
            // 
            this.btnRead.AutoSize = true;
            this.btnRead.Location = new System.Drawing.Point(3, 3);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(80, 23);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "Read 1.8.0";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Location = new System.Drawing.Point(3, 130);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(74, 320);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "Result:";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(83, 133);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(174, 314);
            this.txtResult.TabIndex = 8;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(536, 450);
            this.txtLog.TabIndex = 0;
            this.txtLog.WordWrap = false;
            // 
            // ModeCTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "ModeCTool";
            this.Size = new System.Drawing.Size(800, 450);
            this.Load += new System.EventHandler(this.ModeCTool_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGuard)).EndInit();
            this.flowLayoutButtons.ResumeLayout(false);
            this.flowLayoutButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblGuard;
        private System.Windows.Forms.NumericUpDown numGuard;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutButtons;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtLog;
    }
}
