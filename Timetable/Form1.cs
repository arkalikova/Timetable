using System;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace Timetable
{
    public partial class Form1 : Form
    {
        private FileInfo _filePathToStudents;
        private FileInfo _filePathToTeachers;
        private DataContainer _dataContainer;
        private Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public Form1()
        {
            InitializeComponent();
            _dataContainer = new DataContainer();
            txtPath.Text = ConfigurationManager.AppSettings.Get("SavePath");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = Settings.ExcelFilter
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
                
                var fiFrom = new FileInfo(ofd.FileName);
                _filePathToStudents = new FileInfo(txtPath.Text + Settings.StudentsResultFileName);
                _filePathToTeachers = new FileInfo(txtPath.Text + Settings.TeachersResultFileName);

                try
                {
                    DeleteFilesIfExist();
                    progressBar1.Increment(10);
                    progressBar1.Increment(7);

                    Converter.ConvertTemplateToResult(ref _dataContainer, fiFrom, _filePathToStudents, _filePathToTeachers, progressBar1);

                    MessageBox.Show(Settings.SuccessConvertationMessage);
                    btnOpenStudents.Enabled = true;
                    btnOpenTeachers.Enabled = true;
                }
                catch (IOException)
                {
                    progressBar1.Value = 0;
                    btnOpenStudents.Enabled = false;
                    btnOpenTeachers.Enabled = false;
                    MessageBox.Show(Settings.FailedConvertationMessage);
                }
                catch (NullReferenceException)
                {
                    progressBar1.Value = 0;
                    btnOpenStudents.Enabled = false;
                    btnOpenTeachers.Enabled = false;
                    MessageBox.Show(Settings.FailedFoundListMessage);
                }
                catch (System.Collections.Generic.KeyNotFoundException)
                {
                    progressBar1.Value = 0;
                    btnOpenStudents.Enabled = false;
                    btnOpenTeachers.Enabled = false;
                    MessageBox.Show(Settings.FailedFoundKeyMessage);
                }
                catch (Exception)
                {
                    progressBar1.Value = 0;
                    btnOpenStudents.Enabled = false;
                    btnOpenTeachers.Enabled = false;
                    MessageBox.Show(Settings.FailedOtherMessage);
                }
            }
        }

        private void DeleteFilesIfExist()
        {
            string st = _filePathToStudents.FullName.Remove(_filePathToStudents.FullName.Length - 1);
            if (File.Exists(st))
                File.Delete(st);
            string th = _filePathToTeachers.FullName.Remove(_filePathToTeachers.FullName.Length - 1);
            if (File.Exists(th))
                File.Delete(th);
        }


        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
                config.AppSettings.Settings.Remove("SavePath");
                config.AppSettings.Settings.Add("SavePath", fbd.SelectedPath);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = !string.IsNullOrEmpty(txtPath.Text);
        }

        private void btnOpenStudents_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_filePathToStudents.FullName.Remove(_filePathToStudents.FullName.Length - 1));
        }

        private void btnOpenTeachers_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_filePathToTeachers.FullName.Remove(_filePathToTeachers.FullName.Length - 1));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            FormSend frm = new FormSend();
            frm.dgvTeachers.DataSource = _dataContainer.Teachers.Values;
            frm.Parent = this;
            this.Enabled = false;
            frm.Show();
        }
    }
}