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
            this.lblObis = new System.Windows.Forms.Label();
            this.txtObis = new System.Windows.Forms.TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.treeObis = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbMeterType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGuard)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer.Size = new System.Drawing.Size(800, 450);
            this.splitContainer.SplitterDistance = 285;
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
            this.tableLayoutPanel.Controls.Add(this.lblObis, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.txtObis, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.lblData, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.txtData, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.treeObis, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel3, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.lblType, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.cmbMeterType, 1, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(285, 450);
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
            this.cmbPort.Size = new System.Drawing.Size(199, 21);
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
            this.txtPassword.Size = new System.Drawing.Size(199, 20);
            this.txtPassword.TabIndex = 2;
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
            this.numGuard.Size = new System.Drawing.Size(199, 20);
            this.numGuard.TabIndex = 3;
            this.numGuard.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // lblObis
            // 
            this.lblObis.AutoSize = true;
            this.lblObis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblObis.Location = new System.Drawing.Point(3, 120);
            this.lblObis.Name = "lblObis";
            this.lblObis.Size = new System.Drawing.Size(74, 30);
            this.lblObis.TabIndex = 9;
            this.lblObis.Text = "Obis:";
            this.lblObis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtObis
            // 
            this.txtObis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObis.Location = new System.Drawing.Point(83, 123);
            this.txtObis.Name = "txtObis";
            this.txtObis.Size = new System.Drawing.Size(199, 20);
            this.txtObis.TabIndex = 5;
            this.txtObis.TextChanged += new System.EventHandler(this.txtObis_TextChanged);
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblData.Location = new System.Drawing.Point(3, 380);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(74, 30);
            this.lblData.TabIndex = 12;
            this.lblData.Text = "Data:";
            this.lblData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtData
            // 
            this.txtData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtData.Location = new System.Drawing.Point(83, 383);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(199, 20);
            this.txtData.TabIndex = 7;
            // 
            // treeObis
            // 
            this.treeObis.CheckBoxes = true;
            this.tableLayoutPanel.SetColumnSpan(this.treeObis, 2);
            this.treeObis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeObis.Location = new System.Drawing.Point(3, 153);
            this.treeObis.Name = "treeObis";
            this.treeObis.Size = new System.Drawing.Size(279, 224);
            this.treeObis.TabIndex = 6;
            this.treeObis.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeObis_AfterCheck);
            this.treeObis.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeObis_BeforeCollapse);
            this.treeObis.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeObis_BeforeExpand);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.cmbMode, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnRun, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 413);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(279, 34);
            this.tableLayoutPanel3.TabIndex = 16;
            // 
            // cmbMode
            // 
            this.cmbMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.FormattingEnabled = true;
            this.cmbMode.Items.AddRange(new object[] {
            "Read",
            "Write",
            "Execute"});
            this.cmbMode.Location = new System.Drawing.Point(3, 3);
            this.cmbMode.Name = "cmbMode";
            this.cmbMode.Size = new System.Drawing.Size(133, 21);
            this.cmbMode.TabIndex = 8;
            this.cmbMode.SelectedIndexChanged += new System.EventHandler(this.cmbMode_SelectedIndexChanged);
            // 
            // btnRun
            // 
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRun.Location = new System.Drawing.Point(149, 3);
            this.btnRun.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(120, 28);
            this.btnRun.TabIndex = 9;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.txtLog, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtResult, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblResult, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(511, 450);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // txtLog
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.txtLog, 2);
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(505, 286);
            this.txtLog.TabIndex = 9;
            this.txtLog.TabStop = false;
            this.txtLog.WordWrap = false;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(63, 295);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(445, 152);
            this.txtResult.TabIndex = 8;
            this.txtResult.TabStop = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Location = new System.Drawing.Point(3, 292);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(54, 158);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "Result:";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblType.Location = new System.Drawing.Point(3, 90);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(74, 30);
            this.lblType.TabIndex = 17;
            this.lblType.Text = "Meter Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbMeterType
            // 
            this.cmbMeterType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeterType.FormattingEnabled = true;
            this.cmbMeterType.Items.AddRange(new object[] {
            "1-Phase (E10126)",
            "3-Phase (E30127)"});
            this.cmbMeterType.Location = new System.Drawing.Point(83, 93);
            this.cmbMeterType.Name = "cmbMeterType";
            this.cmbMeterType.Size = new System.Drawing.Size(199, 21);
            this.cmbMeterType.TabIndex = 4;
            this.cmbMeterType.SelectedIndexChanged += new System.EventHandler(this.cmbMeterType_SelectedIndexChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGuard)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblObis;
        private System.Windows.Forms.TextBox txtObis;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TreeView treeObis;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbMeterType;
    }
}
