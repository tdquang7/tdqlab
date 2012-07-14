namespace TestReadTwitterData
{
    partial class Form1
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
            this.btnTransformParMetis = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCheckInfo = new System.Windows.Forms.Button();
            this.btnGenerateSmallerDatabase = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxNodes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFileToShowFractionData = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.txtLinesToShow = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOriginaDataBrowse = new System.Windows.Forms.Button();
            this.txtOriginalData = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.btnToParMetisPath = new System.Windows.Forms.Button();
            this.txtToParMetisPath = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTransformParMetis
            // 
            this.btnTransformParMetis.Location = new System.Drawing.Point(27, 93);
            this.btnTransformParMetis.Name = "btnTransformParMetis";
            this.btnTransformParMetis.Size = new System.Drawing.Size(147, 23);
            this.btnTransformParMetis.TabIndex = 0;
            this.btnTransformParMetis.Tag = "";
            this.btnTransformParMetis.Text = "Transform to ParMetis";
            this.btnTransformParMetis.UseVisualStyleBackColor = true;
            this.btnTransformParMetis.Click += new System.EventHandler(this.btnTransformParMetis_Click);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(6, 19);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(350, 307);
            this.txtContent.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 433);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(837, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar1
            // 
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(194, 93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCheckInfo
            // 
            this.btnCheckInfo.Location = new System.Drawing.Point(294, 93);
            this.btnCheckInfo.Name = "btnCheckInfo";
            this.btnCheckInfo.Size = new System.Drawing.Size(75, 23);
            this.btnCheckInfo.TabIndex = 4;
            this.btnCheckInfo.Text = "Check info";
            this.btnCheckInfo.UseVisualStyleBackColor = true;
            this.btnCheckInfo.Click += new System.EventHandler(this.btnCheckInfo_Click);
            // 
            // btnGenerateSmallerDatabase
            // 
            this.btnGenerateSmallerDatabase.Location = new System.Drawing.Point(346, 27);
            this.btnGenerateSmallerDatabase.Name = "btnGenerateSmallerDatabase";
            this.btnGenerateSmallerDatabase.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateSmallerDatabase.TabIndex = 5;
            this.btnGenerateSmallerDatabase.Text = "Generate";
            this.btnGenerateSmallerDatabase.UseVisualStyleBackColor = true;
            this.btnGenerateSmallerDatabase.Click += new System.EventHandler(this.btnGenerateSmallerDatabase_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMaxNodes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGenerateSmallerDatabase);
            this.groupBox1.Location = new System.Drawing.Point(381, 163);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 70);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generate smaller database from original data";
            // 
            // txtMaxNodes
            // 
            this.txtMaxNodes.Location = new System.Drawing.Point(178, 29);
            this.txtMaxNodes.Name = "txtMaxNodes";
            this.txtMaxNodes.Size = new System.Drawing.Size(162, 20);
            this.txtMaxNodes.TabIndex = 7;
            this.txtMaxNodes.Text = "15000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Max nodes (Also max node id:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.txtFileToShowFractionData);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnLoadData);
            this.groupBox2.Controls.Add(this.txtLinesToShow);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtContent);
            this.groupBox2.Location = new System.Drawing.Point(13, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(362, 408);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Small fraction of data";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(281, 341);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFileToShowFractionData
            // 
            this.txtFileToShowFractionData.Location = new System.Drawing.Point(105, 343);
            this.txtFileToShowFractionData.Name = "txtFileToShowFractionData";
            this.txtFileToShowFractionData.Size = new System.Drawing.Size(170, 20);
            this.txtFileToShowFractionData.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 346);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Path:";
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(281, 374);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(75, 23);
            this.btnLoadData.TabIndex = 4;
            this.btnLoadData.Text = "Load";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // txtLinesToShow
            // 
            this.txtLinesToShow.Location = new System.Drawing.Point(105, 376);
            this.txtLinesToShow.Name = "txtLinesToShow";
            this.txtLinesToShow.Size = new System.Drawing.Size(170, 20);
            this.txtLinesToShow.TabIndex = 3;
            this.txtLinesToShow.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 379);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of lines:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnToParMetisPath);
            this.groupBox3.Controls.Add(this.txtToParMetisPath);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnOriginaDataBrowse);
            this.groupBox3.Controls.Add(this.txtOriginalData);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnTransformParMetis);
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Controls.Add(this.btnCheckInfo);
            this.groupBox3.Location = new System.Drawing.Point(381, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(444, 138);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Transform data";
            // 
            // btnOriginaDataBrowse
            // 
            this.btnOriginaDataBrowse.Location = new System.Drawing.Point(346, 17);
            this.btnOriginaDataBrowse.Name = "btnOriginaDataBrowse";
            this.btnOriginaDataBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnOriginaDataBrowse.TabIndex = 11;
            this.btnOriginaDataBrowse.Text = "Browse...";
            this.btnOriginaDataBrowse.UseVisualStyleBackColor = true;
            this.btnOriginaDataBrowse.Click += new System.EventHandler(this.btnOriginaDataBrowse_Click);
            // 
            // txtOriginalData
            // 
            this.txtOriginalData.Location = new System.Drawing.Point(99, 19);
            this.txtOriginalData.Name = "txtOriginalData";
            this.txtOriginalData.Size = new System.Drawing.Size(241, 20);
            this.txtOriginalData.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Original data:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "To ParMetis:";
            // 
            // btnToParMetisPath
            // 
            this.btnToParMetisPath.Location = new System.Drawing.Point(346, 56);
            this.btnToParMetisPath.Name = "btnToParMetisPath";
            this.btnToParMetisPath.Size = new System.Drawing.Size(75, 23);
            this.btnToParMetisPath.TabIndex = 14;
            this.btnToParMetisPath.Text = "Browse...";
            this.btnToParMetisPath.UseVisualStyleBackColor = true;
            this.btnToParMetisPath.Click += new System.EventHandler(this.btnToParMetisPath_Click);
            // 
            // txtToParMetisPath
            // 
            this.txtToParMetisPath.Location = new System.Drawing.Point(99, 58);
            this.txtToParMetisPath.Name = "txtToParMetisPath";
            this.txtToParMetisPath.Size = new System.Drawing.Size(241, 20);
            this.txtToParMetisPath.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 455);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "Triangle counting data partitioning preparation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTransformParMetis;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCheckInfo;
        private System.Windows.Forms.Button btnGenerateSmallerDatabase;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaxNodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFileToShowFractionData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.TextBox txtLinesToShow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnOriginaDataBrowse;
        private System.Windows.Forms.TextBox txtOriginalData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnToParMetisPath;
        private System.Windows.Forms.TextBox txtToParMetisPath;
        private System.Windows.Forms.Label label5;
    }
}

