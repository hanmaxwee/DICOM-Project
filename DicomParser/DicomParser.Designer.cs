namespace testDCMLIB
{
    partial class DicomParser
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbTransferSyntax = new System.Windows.Forms.ComboBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnParse = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.lvOutput = new System.Windows.Forms.ListView();
            this.TAG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VR = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Length = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据集传输语法：";
            // 
            // cbTransferSyntax
            // 
            this.cbTransferSyntax.FormattingEnabled = true;
            this.cbTransferSyntax.Location = new System.Drawing.Point(204, 26);
            this.cbTransferSyntax.Name = "cbTransferSyntax";
            this.cbTransferSyntax.Size = new System.Drawing.Size(362, 32);
            this.cbTransferSyntax.TabIndex = 1;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(34, 119);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(727, 182);
            this.txtInput.TabIndex = 2;
            // 
            // btnParse
            // 
            this.btnParse.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnParse.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btnParse.Location = new System.Drawing.Point(795, 152);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(100, 50);
            this.btnParse.TabIndex = 3;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = false;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // btnFile
            // 
            this.btnFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFile.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btnFile.Location = new System.Drawing.Point(795, 238);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(100, 50);
            this.btnFile.TabIndex = 4;
            this.btnFile.Text = "File";
            this.btnFile.UseVisualStyleBackColor = false;
            // 
            // lvOutput
            // 
            this.lvOutput.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TAG,
            this.VR,
            this.name,
            this.Length,
            this.Value});
            this.lvOutput.FullRowSelect = true;
            this.lvOutput.Location = new System.Drawing.Point(34, 380);
            this.lvOutput.Name = "lvOutput";
            this.lvOutput.Size = new System.Drawing.Size(727, 270);
            this.lvOutput.TabIndex = 5;
            this.lvOutput.UseCompatibleStateImageBehavior = false;
            this.lvOutput.View = System.Windows.Forms.View.Details;
            // 
            // TAG
            // 
            this.TAG.Text = "TAG";
            this.TAG.Width = 80;
            // 
            // VR
            // 
            this.VR.Text = "VR";
            // 
            // name
            // 
            this.name.Text = "Name";
            this.name.Width = 200;
            // 
            // Length
            // 
            this.Length.Text = "Length";
            this.Length.Width = 74;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.Width = 300;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "16进制字符串：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 347);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "解析结果：";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClear.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btnClear.Location = new System.Drawing.Point(795, 70);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 50);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // DicomParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 684);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvOutput);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.cbTransferSyntax);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DicomParser";
            this.Text = "DicomParser";
            this.Load += new System.EventHandler(this.DicomParser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTransferSyntax;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.ListView lvOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader TAG;
        private System.Windows.Forms.ColumnHeader VR;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader Length;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.Button btnClear;
    }
}