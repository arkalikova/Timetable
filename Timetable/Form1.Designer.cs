namespace Timetable
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnOpenStudents = new System.Windows.Forms.Button();
            this.btnOpenTeachers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(16, 15);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(153, 28);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Загрузить шаблон";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Папка для сохранения:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(401, 17);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(167, 22);
            this.txtPath.TabIndex = 4;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(577, 15);
            this.btnChoosePath.Margin = new System.Windows.Forms.Padding(4);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(32, 28);
            this.btnChoosePath.TabIndex = 5;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(16, 50);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(152, 28);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 118);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(623, 28);
            this.progressBar1.TabIndex = 7;
            // 
            // btnOpenStudents
            // 
            this.btnOpenStudents.Enabled = false;
            this.btnOpenStudents.Location = new System.Drawing.Point(205, 50);
            this.btnOpenStudents.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenStudents.Name = "btnOpenStudents";
            this.btnOpenStudents.Size = new System.Drawing.Size(188, 49);
            this.btnOpenStudents.TabIndex = 8;
            this.btnOpenStudents.Text = "Открыть расписание для студентов";
            this.btnOpenStudents.UseVisualStyleBackColor = true;
            this.btnOpenStudents.Click += new System.EventHandler(this.btnOpenStudents_Click);
            // 
            // btnOpenTeachers
            // 
            this.btnOpenTeachers.Enabled = false;
            this.btnOpenTeachers.Location = new System.Drawing.Point(401, 50);
            this.btnOpenTeachers.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenTeachers.Name = "btnOpenTeachers";
            this.btnOpenTeachers.Size = new System.Drawing.Size(188, 49);
            this.btnOpenTeachers.TabIndex = 9;
            this.btnOpenTeachers.Text = "Открыть расписание для преподавателей";
            this.btnOpenTeachers.UseVisualStyleBackColor = true;
            this.btnOpenTeachers.Click += new System.EventHandler(this.btnOpenTeachers_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 146);
            this.Controls.Add(this.btnOpenTeachers);
            this.Controls.Add(this.btnOpenStudents);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoad);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Расписание ВЗФ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnOpenStudents;
        private System.Windows.Forms.Button btnOpenTeachers;
    }
}

