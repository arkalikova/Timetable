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
            this.ColumnTeacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSend = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chbFull = new System.Windows.Forms.CheckBox();
            this.chbCards = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbText
            // 
            this.rtbText.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbText.Location = new System.Drawing.Point(0, 195);
            this.rtbText.Name = "rtbText";
            this.rtbText.Size = new System.Drawing.Size(561, 154);
            this.rtbText.TabIndex = 0;
            this.rtbText.Text = "";
            // 
            // dgvTeachers
            // 
            this.dgvTeachers.AllowUserToAddRows = false;
            this.dgvTeachers.AllowUserToDeleteRows = false;
            this.dgvTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeachers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTeacher,
            this.ColumnSend});
            this.dgvTeachers.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvTeachers.Location = new System.Drawing.Point(0, 0);
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.RowHeadersVisible = false;
            this.dgvTeachers.Size = new System.Drawing.Size(319, 195);
            this.dgvTeachers.TabIndex = 1;
            // 
            // ColumnTeacher
            // 
            this.ColumnTeacher.HeaderText = "Преподаватель";
            this.ColumnTeacher.Name = "ColumnTeacher";
            // 
            // ColumnSend
            // 
            this.ColumnSend.HeaderText = "";
            this.ColumnSend.Name = "ColumnSend";
            // 
            // chbFull
            // 
            this.chbFull.AutoSize = true;
            this.chbFull.Location = new System.Drawing.Point(325, 12);
            this.chbFull.Name = "chbFull";
            this.chbFull.Size = new System.Drawing.Size(143, 17);
            this.chbFull.TabIndex = 2;
            this.chbFull.Text = "Отправить расписание";
            this.chbFull.UseVisualStyleBackColor = true;
            // 
            // chbCards
            // 
            this.chbCards.AutoSize = true;
            this.chbCards.Location = new System.Drawing.Point(325, 35);
            this.chbCards.Name = "chbCards";
            this.chbCards.Size = new System.Drawing.Size(128, 17);
            this.chbCards.TabIndex = 3;
            this.chbCards.Text = "Отправить карточку";
            this.chbCards.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(474, 9);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // FormSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 349);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.chbCards);
            this.Controls.Add(this.chbFull);
            this.Controls.Add(this.dgvTeachers);
            this.Controls.Add(this.rtbText);
            this.Name = "FormSend";
            this.Text = "Отправка расписания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSend_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbText;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTeacher;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSend;
        private System.Windows.Forms.CheckBox chbFull;
        private System.Windows.Forms.CheckBox chbCards;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.DataGridView dgvTeachers;
    }
}