using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Timetable
{
    public partial class FormSend : Form
    {
        private bool _blockChkFull;
        private bool _blockDgvChk;

        public FormSend(List<Teacher> teachers)
        {
            InitializeComponent();
            if (teachers.Count != 0)
            {
                dgvTeachers.DataSource = teachers;
                dgvTeachers.Columns["Преподаватель"].ReadOnly = true;
                dgvTeachers.Columns["Email"].Visible = false;
                dgvTeachers.Columns["Колонка"].Visible = false;
                SetChkFullState();
            }
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
            for (var i = 0; i < dgvTeachers.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvTeachers.Rows[i].Cells["Отправить письмо"].Value))
                    SendMailToTeacher(Convert.ToString(dgvTeachers.Rows[i].Cells["Email"].Value));
            }
        }

        private void SendMailToTeacher(string email)
        {
            throw new NotImplementedException();
        }
    }
}
