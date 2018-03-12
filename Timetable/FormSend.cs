using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable
{
    public partial class FormSend : Form
    {
        public Form1 Parent;

        public FormSend()
        {
            InitializeComponent();
        }

        private void FormSend_FormClosing(object sender, FormClosingEventArgs e)
        {
            Parent.Enabled = true;
        }
    }
}
