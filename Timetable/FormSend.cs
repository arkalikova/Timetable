﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        private readonly string _file;
        private static Configuration Config;
        private readonly Dictionary<string, int> _smtpPorts;
        private readonly Dictionary<string, string> _smtpServers;

        public FormSend(List<Teacher> teachers, string file, Configuration config)
        {
            InitializeComponent();
            SetAuthData();
            SetMailData();
            _file = file;
            _smtpPorts = SmtpPorts();
            _smtpServers = SmtpServers();
            FillTeacherDgv(teachers);
            Config = config;
        }

        private void FillTeacherDgv(List<Teacher> teachers)
        {
            if (teachers.Count != 0)
            {
                dgvTeachers.DataSource = teachers;
                var dataGridViewColumn = dgvTeachers.Columns["Name"];
                if (dataGridViewColumn != null) dataGridViewColumn.ReadOnly = true;
                var dgvTeachersColumn = dgvTeachers.Columns["Email"];
                if (dgvTeachersColumn != null) dgvTeachersColumn.Visible = false;
                var gridViewColumn = dgvTeachers.Columns["Column"];
                if (gridViewColumn != null) gridViewColumn.Visible = false;
                SetChkFullState();
            }
        }

        private static Dictionary<string, int> SmtpPorts()
        {
            return new Dictionary<string, int>()
            {
                {"mail.ru", 25},
                {"gmail.com", 587},
                {"yandex.ru", 587}
                //{"hse.ru", 587 }
            };
        }

        private static Dictionary<string, string> SmtpServers()
        {
            return new Dictionary<string, string>()
            {
                {"mail.ru", "smtp.mail.ru"},
                {"gmail.com", "smtp.gmail.com"},
                {"yandex.ru", "smtp.yandex.ru" }
                //{"hse.ru", "hse.ru" }
                //{"hse.ru", "mail.hse.ru" }
                //{"hse.ru", "mailperm.hse.ru" }
                //{"hse.ru", "smtp.mail.hse.ru" }
                //{"hse.ru", "smtp.mailperm.hse.ru" }
                //{"hse.ru", "smtp.hse.ru" }
            };
        }

        private void SetAuthData()
        {
            var loginLabelText = ConfigurationManager.AppSettings.Get("EmailAddress");
            loginLabel.Text = loginLabelText ?? "Нет учетных данных";
        }

        private void SetMailData()
        {
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
                    SendMailToTeacher(teacherMails.Distinct().ToList());
                }
                else
                {
                    MessageBox.Show(@"Необходимо выбрать преподавателей для отправки писем", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(@"Перед рассылкой необходимо ввести учетные данные", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OpenAuthForm();
            }
        }

        private void OpenAuthForm()
        {
            var authForm = new FormAuth(Config);
            authForm.ShowDialog();
            SetAuthData();
        }

        private void SendMailToTeacher(List<string> teacherMails)
        {
            var mailAddress = ConfigurationManager.AppSettings.Get("EmailAddress");
            var password = Encryption.DecryptString(ConfigurationManager.AppSettings.Get("EmailPassword"));

            try
            {
                try
                {
                    var smtp = GetSmtpClient(mailAddress, password);
                    var batch = new List<string>();
                    for (var i = teacherMails.Count - 1; i >= 0; i--)
                    {
                        batch.Add(teacherMails[i]);
                        if (i % 25 == 0)
                        {
                            var message = GetMailMessage(teacherMails, mailAddress, out Attachment data);
                            smtp.Send(message);
                            data.Dispose();
                            batch.Clear();
                        }
                    }
                    MessageBox.Show($@"Рассылка успешно завершена", "", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (KeyNotFoundException exception)
                {
                    var smtpServer = mailAddress.Split('@')[1];
                    MessageBox.Show($"Отправка писем с домена {smtpServer} на данный момент не поддерживается.\n\n" +
                                    "Поддерживаемые домены:\n" +
                                    "mail.ru\n"+
                                    "gmail.com\n" +
                                    "yandex.ru\n", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Возникла ошибка при отправке письма.\n"+
                                    "Проверьте интернет-соединение и правильность введенных учетных данных.\n\n" +
                                    $"Подробности:\n{exception.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (IOException)
            {
                MessageBox.Show(Settings.FailedConvertationMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(Settings.FailedOtherMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private MailMessage GetMailMessage(IEnumerable<string> teacherMails, string mailAddress, out Attachment data)
        {
            var message = new MailMessage
            {
                From = new MailAddress(mailAddress),
                Body = rtbMailBody.Text,
                Subject = rtbMailTheme.Text
            };
            foreach (var teacherMail in teacherMails)
            {
                message.To.Add(teacherMail);
            }
            data = new Attachment(_file, MediaTypeNames.Application.Octet);
            message.Attachments.Add(data);
            return message;
        }

        private SmtpClient GetSmtpClient(string mailAddress, string mailPassword)
        {
            var smtpServer = mailAddress.Split('@')[1];
            var smtp = new SmtpClient(GetSmtpServer(smtpServer), GetSmtpPort(smtpServer));
            smtp.Credentials = new NetworkCredential(mailAddress, mailPassword); 
            smtp.EnableSsl = true;

            return smtp;
        }

        private int GetSmtpPort(string smtpServer)
        {
            return _smtpPorts[smtpServer];
        }

        private string GetSmtpServer(string smtpServer)
        {
            return _smtpServers[smtpServer];
        }

        private void btnSaveMailTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                //var _config = ;
                Config.AppSettings.Settings.Remove("EmailTheme");
                Config.AppSettings.Settings.Add("EmailTheme", rtbMailTheme.Text);
                Config.AppSettings.Settings.Remove("EmailBody");
                Config.AppSettings.Settings.Add("EmailBody", rtbMailBody.Text);
                Config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show(@"Шаблон письма успешно сохранен", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"При сохранении шаблона письма произошла ошибка : {exception.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            OpenAuthForm();
        }
    }
}