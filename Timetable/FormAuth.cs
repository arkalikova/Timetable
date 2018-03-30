using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Mail;
using System.Windows.Forms;

namespace Timetable
{
    public partial class FormAuth : Form
    {
        private static Configuration _config;
        private static Dictionary<string, string> inboxes;

        public FormAuth(Configuration config)
        {
            InitializeComponent();
            inboxes = new Dictionary<string, string>()
            {
                {"mail.ru", "smtp.mail.ru"},
                {"gmail.com", "smtp.gmail.com"},
                {"yandex.ru", "smtp.yandex.ru" }
            };
            _config = config;
            txtAdress.Text = ConfigurationManager.AppSettings.Get("EmailAddress");
            txtPassword.Text = Encryption.DecryptString(ConfigurationManager.AppSettings.Get("EmailPassword"));
        }

        private void btnSaveAuthData_Click(object sender, EventArgs e)
        {
            var txtAddressText = txtAdress.Text.Trim();
            var txtPasswordText = txtPassword.Text.Trim();
            if (txtAddressText.Length != 0 && txtPasswordText.Length != 0)
            {
                string smtpServer = "";
                try
                {
                    var mailAddress = new MailAddress(txtAddressText);
                    smtpServer = mailAddress.Host;
                    var server = inboxes[smtpServer];
                    _config.AppSettings.Settings.Remove("EmailAddress");
                    _config.AppSettings.Settings.Add("EmailAddress", txtAddressText);
                    _config.AppSettings.Settings.Remove("EmailPassword");
                    _config.AppSettings.Settings.Add("EmailPassword", Encryption.EncryptString(txtPasswordText));
                    _config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                    MessageBox.Show(@"Учетные данные успешно сохранены", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (KeyNotFoundException exception)
                {
                    MessageBox.Show($"Отправка писем с домена \"{smtpServer}\" на данный момент не поддерживается.\n\n" +
                                    "Поддерживаемые домены:\n" +
                                    "\"mail.ru \"\n" +
                                    "\"gmail.com \"\n" +
                                    "\"yandex.ru \"\n", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        $@"При сохранении учетных данных произошла ошибка : {exception.Message}");
                }
            }
            else
            {
                MessageBox.Show(@"Перед сохранением введите учетные данные", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtAdress_Validating(object sender, CancelEventArgs e)
        {
            var txtAddressText = txtAdress.Text;
            if (txtAddressText.Trim().Length > 0)
            {
                try
                {
                    var m = new MailAddress(txtAddressText);
                }
                catch (FormatException)
                {
                    MessageBox.Show(@"Необходимо ввести адрес почтового ящика (вида example@hse.ru)", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = txtPassword.PasswordChar == '\0' ? '*' : '\0';
        }
    }
}