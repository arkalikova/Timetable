using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Mail;
using System.Windows.Forms;

namespace Timetable
{
    public partial class FormAuth : Form
    {
        private readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public FormAuth()
        {
            InitializeComponent();
            txtAdress.Text = ConfigurationManager.AppSettings.Get("EmailAddress");
            txtPassword.Text = ConfigurationManager.AppSettings.Get("EmailPassword");
        }

        private void btnSaveAuthData_Click(object sender, EventArgs e)
        {
            var txtAddressText = txtAdress.Text.Trim();
            var txtPasswordText = txtPassword.Text.Trim();
            if (txtAddressText.Length != 0 && txtPasswordText.Length != 0)
            {
                try
                {
                    var m = new MailAddress(txtAddressText);
                    _config.AppSettings.Settings.Remove("EmailAddress");
                    _config.AppSettings.Settings.Add("EmailAddress", txtAddressText);
                    _config.AppSettings.Settings.Remove("EmailPassword");
                    _config.AppSettings.Settings.Add("EmailPassword", txtPasswordText);
                    _config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    MessageBox.Show(@"Учетные данные успешно сохранены");
                    Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        $@"При сохранении учетных данных произошла ошибка : {exception.Message}");
                }
            }
            else
            {
                MessageBox.Show(@"Перед сохранением введите учетные данные");
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
                    MessageBox.Show(@"Адрес почтового ящика должен быть вида example@hse.ru");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = txtPassword.PasswordChar == '\0' ? '*' : '\0';
        }
    }
}