using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows.Forms;

namespace Timetable
{
    public partial class FormSend : Form
    {
        private bool _blockChkFull;
        private bool _blockDgvChk;
        private string _file;
        private readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private readonly Dictionary<string, int> _smtpServers;

        public FormSend(List<Teacher> teachers, string file)
        {
            InitializeComponent();
            SetConfigData();
            _file = file;
            _smtpServers = SmtpServers();
            FillTeacherDgv(teachers);
        }

        private void FillTeacherDgv(List<Teacher> teachers)
        {
            if (teachers.Count != 0)
            {
                dgvTeachers.DataSource = teachers;
                dgvTeachers.Columns["Name"].ReadOnly = true;
                dgvTeachers.Columns["Email"].Visible = false;
                dgvTeachers.Columns["Column"].Visible = false;
                SetChkFullState();
            }
        }

        private static Dictionary<string, int> SmtpServers()
        {
            return new Dictionary<string, int>()
            {
                {"mail.ru", 25},
                {"gmail.com", 587},
                {"yandex.ru", 587 },
                {"hse.ru", 587 }
            };
        }

        private void SetConfigData()
        {
            var loginLabelText = ConfigurationManager.AppSettings.Get("EmailAddress");
            loginLabel.Text = loginLabelText ?? "Нет учетных данных";
            var mailBody = ConfigurationManager.AppSettings.Get("EmailBody");
            rtbMailBody.Text = mailBody;
            var mailTheme = ConfigurationManager.AppSettings.Get("EmailTheme");
            rtbMailTheme.Text = mailTheme;
        }
        private void FormSend_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void chbFull_CheckedChanged(object sender, EventArgs e)
        {
            if (!_blockDgvChk)
            {
                _blockChkFull = true;
                for (var i = 0; i < dgvTeachers.Rows.Count; i++)
                {
                    dgvTeachers.Rows[i].Cells[3].Value = chbFull.Checked;
                }
                _blockChkFull = false;
            }
        }

        private void dgvTeachers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && !_blockChkFull)
            {
                _blockDgvChk = true;
                SetChkFullState();
                _blockDgvChk = false;
            }
        }

        private void SetChkFullState()
        {
            var allRowsAreChecked = true;
            var allRowsAreNotChecked = true;
            for (var i = 0; i < dgvTeachers.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvTeachers.Rows[i].Cells[3].Value))
                {
                    allRowsAreNotChecked = false;
                }
                else
                {
                    allRowsAreChecked = false;
                }
            }

            if (allRowsAreNotChecked)
            {
                chbFull.CheckState = CheckState.Unchecked;
                btnSend.Enabled = false;
            }
            else if (allRowsAreChecked)
            {
                chbFull.CheckState = CheckState.Checked;
                btnSend.Enabled = true;
            }
            else
            {
                chbFull.CheckState = CheckState.Indeterminate;
                btnSend.Enabled = true;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings.Get("EmailAddress") != null)
            {
                if (dgvTeachers.Rows.Count > 0 && chbFull.CheckState != CheckState.Unchecked)
                {
                    var teacherMails = new List<string>();
                    for (var i = 0; i < dgvTeachers.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvTeachers.Rows[i].Cells["isNotificated"].Value))
                            teacherMails.Add(Convert.ToString(dgvTeachers.Rows[i].Cells["Email"].Value));
                    }
                    SendMailToTeacher(teacherMails);
                }
                else
                {
                    MessageBox.Show(@"Необходимо выбрать преподавателей для отправки писем");
                }
            }
            else
            {
                MessageBox.Show(@"Перед рассылкой необходимо ввести учетные данные");
                OpenAuthForm();
            }
        }

        private void OpenAuthForm()
        {
            var authForm = new FormAuth();
            authForm.ShowDialog();
            SetConfigData();
        }

        private void SendMailToTeacher(IEnumerable<string> teacherMails)
        {
            var mailAddress = ConfigurationManager.AppSettings.Get("EmailAddress");
            var message = new MailMessage
            {
                From = new MailAddress(mailAddress),
                Body = rtbMailBody.Text,
                Subject = rtbMailTheme.Text
            };
            
            teacherMails = new List<string>()
            {
                "kar-pm@yandex.ru"
            };
            foreach (var teacherMail in teacherMails)
            {
                message.To.Add(teacherMail);
            }
            var data = new Attachment(_file, MediaTypeNames.Application.Octet);
            message.Attachments.Add(data);

            var smtp = GetSmtpClient(mailAddress);

            try
            {
                smtp.Send(message);
                MessageBox.Show($@"Рассылка успешно завершена");
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Возникла следующая ошибка при отправке письма: {exception.Message}");
            }
            data.Dispose();
        }

        private SmtpClient GetSmtpClient(string mailAddress)
        {
            var password = ConfigurationManager.AppSettings.Get("EmailPassword");
            var smtpServer = mailAddress.Split('@')[1];

            var smtp = new SmtpClient("smtp." + smtpServer, GetSmtpPort(smtpServer));
            smtp.Credentials = new NetworkCredential(mailAddress, password);
            smtp.EnableSsl = true;
            return smtp;
        }

        private int GetSmtpPort(string smtpServer)
        {
            return _smtpServers[smtpServer];
        }

        private void btnSaveMailTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                _config.AppSettings.Settings.Remove("EmailTheme");
                _config.AppSettings.Settings.Add("EmailTheme", rtbMailTheme.Text);
                _config.AppSettings.Settings.Remove("EmailBody");
                _config.AppSettings.Settings.Add("EmailBody", rtbMailBody.Text);
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show(@"Шаблон письма успешно сохранен");
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"При сохранении шаблона письма произошла ошибка : {exception.Message}");
            }
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            OpenAuthForm();
        }
    }
}