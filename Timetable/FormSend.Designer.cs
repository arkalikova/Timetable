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
            this.rtbMailBody = new System.Windows.Forms.RichTextBox();
            this.dgvTeachers = new System.Windows.Forms.DataGridView();
            this.chbFull = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.rtbMailTheme = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSaveMailTemplate = new System.Windows.Forms.Button();
            this.loginLabel = new System.Windows.Forms.Label();
            this.btnAuth = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbMailBody
            // 
            this.rtbMailBody.Location = new System.Drawing.Point(433, 193);
            this.rtbMailBody.Margin = new System.Windows.Forms.Padding(4);
            this.rtbMailBody.Name = "rtbMailBody";
            this.rtbMailBody.Size = new System.Drawing.Size(406, 291);
            this.rtbMailBody.TabIndex = 0;
            this.rtbMailBody.Text = "";
            // 
            // dgvTeachers
            // 
            this.dgvTeachers.AllowUserToAddRows = false;
            this.dgvTeachers.AllowUserToDeleteRows = false;
            this.dgvTeachers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeachers.Location = new System.Drawing.Point(13, 48);
            this.dgvTeachers.Margin = new System.Windows.Forms.Padding(4);
            this.dgvTeachers.Name = "dgvTeachers";
            this.dgvTeachers.RowHeadersVisible = false;
            this.dgvTeachers.Size = new System.Drawing.Size(393, 482);
            this.dgvTeachers.TabIndex = 1;
            this.dgvTeachers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTeachers_CellValueChanged);
            // 
            // chbFull
            // 
            this.chbFull.AutoSize = true;
            this.chbFull.Checked = true;
            this.chbFull.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chbFull.Location = new System.Drawing.Point(280, 15);
            this.chbFull.Margin = new System.Windows.Forms.Padding(4);
            this.chbFull.Name = "chbFull";
            this.chbFull.Size = new System.Drawing.Size(18, 17);
            this.chbFull.TabIndex = 2;
            this.chbFull.UseVisualStyleBackColor = true;
            this.chbFull.CheckedChanged += new System.EventHandler(this.chbFull_CheckedChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(647, 492);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(192, 38);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Отправить расписание";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rtbMailTheme
            // 
            this.rtbMailTheme.Location = new System.Drawing.Point(433, 72);
            this.rtbMailTheme.Margin = new System.Windows.Forms.Padding(4);
            this.rtbMailTheme.Name = "rtbMailTheme";
            this.rtbMailTheme.Size = new System.Drawing.Size(406, 86);
            this.rtbMailTheme.TabIndex = 5;
            this.rtbMailTheme.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Отметитиь всех/Снять выделение";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(430, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Текст письма";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(434, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Тема письма";
            // 
            // btnSaveMailTemplate
            // 
            this.btnSaveMailTemplate.Location = new System.Drawing.Point(433, 492);
            this.btnSaveMailTemplate.Name = "btnSaveMailTemplate";
            this.btnSaveMailTemplate.Size = new System.Drawing.Size(183, 38);
            this.btnSaveMailTemplate.TabIndex = 9;
            this.btnSaveMailTemplate.Text = "Сохранить как шаблон";
            this.btnSaveMailTemplate.UseVisualStyleBackColor = true;
            this.btnSaveMailTemplate.Click += new System.EventHandler(this.btnSaveMailTemplate_Click);
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(434, 14);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(143, 17);
            this.loginLabel.TabIndex = 10;
            this.loginLabel.Text = "Нет учетных данных";
            // 
            // btnAuth
            // 
            this.btnAuth.Location = new System.Drawing.Point(658, 6);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(181, 33);
            this.btnAuth.TabIndex = 11;
            this.btnAuth.Text = "Ввести учетные данные";
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // FormSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(854, 543);
            this.Controls.Add(this.btnAuth);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.btnSaveMailTemplate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbMailTheme);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.chbFull);
            this.Controls.Add(this.dgvTeachers);
            this.Controls.Add(this.rtbMailBody);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormSend";
            this.Text = "Отправка расписания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSend_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeachers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbMailBody;
        private System.Windows.Forms.CheckBox chbFull;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.DataGridView dgvTeachers;
        private System.Windows.Forms.RichTextBox rtbMailTheme;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSaveMailTemplate;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.Button btnAuth;
    }
}