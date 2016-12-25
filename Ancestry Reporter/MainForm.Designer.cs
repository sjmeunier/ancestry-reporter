namespace Ancestry_Reporter
{
	partial class MainForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblIndividuals = new System.Windows.Forms.Label();
			this.lblFamilies = new System.Windows.Forms.Label();
			this.butImport = new System.Windows.Forms.Button();
			this.cboReportType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtRootPerson = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.txtOutputPath = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMaxDepth = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.butOutput = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butImport);
			this.groupBox1.Controls.Add(this.lblFamilies);
			this.groupBox1.Controls.Add(this.lblIndividuals);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(357, 59);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Gedcom Data";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Individuals:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(47, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Families:";
			// 
			// lblIndividuals
			// 
			this.lblIndividuals.AutoSize = true;
			this.lblIndividuals.Location = new System.Drawing.Point(76, 20);
			this.lblIndividuals.Name = "lblIndividuals";
			this.lblIndividuals.Size = new System.Drawing.Size(13, 13);
			this.lblIndividuals.TabIndex = 2;
			this.lblIndividuals.Text = "0";
			// 
			// lblFamilies
			// 
			this.lblFamilies.AutoSize = true;
			this.lblFamilies.Location = new System.Drawing.Point(76, 33);
			this.lblFamilies.Name = "lblFamilies";
			this.lblFamilies.Size = new System.Drawing.Size(13, 13);
			this.lblFamilies.TabIndex = 3;
			this.lblFamilies.Text = "0";
			// 
			// butImport
			// 
			this.butImport.Location = new System.Drawing.Point(273, 19);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(75, 23);
			this.butImport.TabIndex = 4;
			this.butImport.Text = "Import";
			this.butImport.UseVisualStyleBackColor = true;
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// cboReportType
			// 
			this.cboReportType.FormattingEnabled = true;
			this.cboReportType.Location = new System.Drawing.Point(106, 119);
			this.cboReportType.Name = "cboReportType";
			this.cboReportType.Size = new System.Drawing.Size(121, 21);
			this.cboReportType.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Root Person";
			// 
			// txtRootPerson
			// 
			this.txtRootPerson.Location = new System.Drawing.Point(106, 66);
			this.txtRootPerson.Name = "txtRootPerson";
			this.txtRootPerson.Size = new System.Drawing.Size(100, 20);
			this.txtRootPerson.TabIndex = 3;
			this.txtRootPerson.Text = "I0001";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 119);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(66, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Report Type";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 222);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(370, 22);
			this.statusStrip1.TabIndex = 5;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(0, 17);
			// 
			// txtOutputPath
			// 
			this.txtOutputPath.Location = new System.Drawing.Point(106, 91);
			this.txtOutputPath.Name = "txtOutputPath";
			this.txtOutputPath.Size = new System.Drawing.Size(228, 20);
			this.txtOutputPath.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 94);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Output Path";
			// 
			// txtMaxDepth
			// 
			this.txtMaxDepth.Location = new System.Drawing.Point(106, 149);
			this.txtMaxDepth.Name = "txtMaxDepth";
			this.txtMaxDepth.Size = new System.Drawing.Size(100, 20);
			this.txtMaxDepth.TabIndex = 9;
			this.txtMaxDepth.Text = "50";
			this.txtMaxDepth.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 152);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Max Depth";
			this.label6.Click += new System.EventHandler(this.label6_Click);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(276, 179);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(75, 23);
			this.btnGenerate.TabIndex = 5;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// butOutput
			// 
			this.butOutput.Location = new System.Drawing.Point(340, 91);
			this.butOutput.Name = "butOutput";
			this.butOutput.Size = new System.Drawing.Size(20, 20);
			this.butOutput.TabIndex = 10;
			this.butOutput.Text = ">";
			this.butOutput.UseVisualStyleBackColor = true;
			this.butOutput.Click += new System.EventHandler(this.butOutput_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(370, 244);
			this.Controls.Add(this.butOutput);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.txtMaxDepth);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtOutputPath);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtRootPerson);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cboReportType);
			this.Controls.Add(this.groupBox1);
			this.Name = "MainForm";
			this.Text = "Ancestry Reporter";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button butImport;
		private System.Windows.Forms.Label lblFamilies;
		private System.Windows.Forms.Label lblIndividuals;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboReportType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRootPerson;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.TextBox txtOutputPath;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtMaxDepth;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Button butOutput;
	}
}

