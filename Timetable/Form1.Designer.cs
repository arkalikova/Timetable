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
            this.btnForStudents = new System.Windows.Forms.Button();
            this.btnForTeachers = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnForStudents
            // 
            this.btnForStudents.Location = new System.Drawing.Point(12, 82);
            this.btnForStudents.Name = "btnForStudents";
            this.btnForStudents.Size = new System.Drawing.Size(207, 23);
            this.btnForStudents.TabIndex = 0;
            this.btnForStudents.Text = "Представление для студентов";
            this.btnForStudents.UseVisualStyleBackColor = true;
            this.btnForStudents.Click += new System.EventHandler(this.btnForStudents_Click);
            // 
            // btnForTeachers
            // 
            this.btnForTeachers.Location = new System.Drawing.Point(250, 82);
            this.btnForTeachers.Name = "btnForTeachers";
            this.btnForTeachers.Size = new System.Drawing.Size(207, 23);
            this.btnForTeachers.TabIndex = 1;
            this.btnForTeachers.Text = "Представление для преподавателей";
            this.btnForTeachers.UseVisualStyleBackColor = true;
            this.btnForTeachers.Click += new System.EventHandler(this.btnForTeachers_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(115, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Загрузить шаблон";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Директория для выгрузки:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(301, 14);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(100, 20);
            this.txtPath.TabIndex = 4;
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(407, 12);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(24, 23);
            this.btnChoosePath.TabIndex = 5;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(13, 42);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(114, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 178);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(445, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 213);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnForTeachers);
            this.Controls.Add(this.btnForStudents);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnForStudents;
        private System.Windows.Forms.Button btnForTeachers;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

