namespace Timetable
{
    partial class FormSend
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
            this.rtbText = new System.Windows.Forms.RichTextBox();
            this.dgvTeachers = new System.Windows.Forms.DataGridView();
            this.chbFull = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbText
            // 
            this.rtbText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbText.Location = new System.Drawing.Point(0, 262);
            this.rtbText.Margin = new System.Windows.Forms.Padding(4);
            this.rtbText.Name = "rtbText";
            this.rtbText.Size = new System.Drawing.Size(748, 168);
            this.rtbText.TabIndex = 0;
            this.rtbText.Text = "";
            // 
            // dgvTeachers
            // 
            this.dgvTeachers.AllowUserToAddRows = false;
            this.dgvTeachers.AllowUserToDeleteRows = false;
            this.dgvTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeachers.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvTeachers.Location = new System.Drawing.Point(0, 0);
            this.dgvTeachers.Margin = new System.Windows.Forms.Padding(4);
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.RowHeadersVisible = false;
            this.dgvTeachers.Size = new System.Drawing.Size(425, 262);
            this.dgvTeachers.TabIndex = 1;
            this.dgvTeachers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTeachers_CellValueChanged);
            // 
            // chbFull
            // 
            this.chbFull.AutoSize = true;
            this.chbFull.Checked = true;
            this.chbFull.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chbFull.Location = new System.Drawing.Point(433, 15);
            this.chbFull.Margin = new System.Windows.Forms.Padding(4);
            this.chbFull.Name = "chbFull";
            this.chbFull.Size = new System.Drawing.Size(248, 21);
            this.chbFull.TabIndex = 2;
            this.chbFull.Text = "Отметитиь все/Снять выделение";
            this.chbFull.UseVisualStyleBackColor = true;
            this.chbFull.CheckedChanged += new System.EventHandler(this.chbFull_CheckedChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(433, 43);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(192, 28);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Отправить расписание";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FormSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 430);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.chbFull);
            this.Controls.Add(this.dgvTeachers);
            this.Controls.Add(this.rtbText);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormSend";
            this.Text = "Отправка расписания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSend_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbText;
        private System.Windows.Forms.CheckBox chbFull;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.DataGridView dgvTeachers;
    }
}