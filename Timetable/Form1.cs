using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable
{
    public partial class Form1 : Form
    {
        //пути к файлам
        FileInfo fi_to_S;
        FileInfo fi_to_T;

        public Form1()
        {
            InitializeComponent();
            Transformer.Teachers = new Dictionary<int, Teacher>();
            Transformer.Disciplines = new Dictionary<int, string>();
            Transformer.Time = new Dictionary<int, string>();
            Transformer.Groups = new Dictionary<int, string>();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файл Excel (*.xlsx)| *.xlsx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
                ExcelWorksheet new_worksheet_T = null;
                bool flag = true;
                var fi_from = new FileInfo(ofd.FileName);
                fi_to_S = new FileInfo(txtPath.Text + "\\Расписание для студентов.xlsx");
                fi_to_T = new FileInfo(txtPath.Text + "\\Расписание для преподавателей.xlsx");
                int new_col = 3;
                int start_row;
                int end_row;
                int end_col;

                //удаляем каждый раз результирующий файл
                if (File.Exists(fi_to_S.FullName))
                    File.Delete(fi_to_S.FullName);
                if (File.Exists(fi_to_T.FullName))
                    File.Delete(fi_to_T.FullName);
                progressBar1.Increment(10);
                //чистим словари
                Transformer.Disciplines.Clear();
                Transformer.Groups.Clear();
                Transformer.Teachers.Clear();
                Transformer.Time.Clear();

                progressBar1.Increment(7);
                try
                {
                    using (var package = new ExcelPackage(fi_from))
                    {
                        var workbook = package.Workbook;
                        using (var result_S = new ExcelPackage(fi_to_S))
                        {
                            using (var result_T = new ExcelPackage(fi_to_T))
                            {
                                //Преподаватели
                                var worksheet = workbook.Worksheets["Преподаватель"];
                                start_row = worksheet.Dimension.Start.Row;
                                end_row = worksheet.Dimension.End.Row;
                                for (int i = start_row + 1; i <= end_row; i++)
                                {
                                    Transformer.Teachers.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), new Teacher
                                    {
                                        Name = worksheet.Cells[i, 2].Value.ToString(),
                                        Email = worksheet.Cells[i, 3].Value.ToString(),
                                        Column = 0
                                    });
                                }
                                progressBar1.Increment(20);
                                //Предметы
                                worksheet = workbook.Worksheets["Предмет"];
                                start_row = worksheet.Dimension.Start.Row;
                                end_row = worksheet.Dimension.End.Row;
                                for (int i = start_row + 1; i <= end_row; i++)
                                {
                                    Transformer.Disciplines.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), worksheet.Cells[i, 2].Value.ToString());
                                }
                                progressBar1.Increment(9);
                                //Время пар
                                worksheet = workbook.Worksheets["Время пар"];
                                start_row = worksheet.Dimension.Start.Row;
                                end_row = worksheet.Dimension.End.Row;
                                for (int i = start_row + 1; i <= end_row; i++)
                                {
                                    Transformer.Time.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), worksheet.Cells[i, 2].Value.ToString());
                                }
                                progressBar1.Increment(8);
                                foreach (var worksheet_ in workbook.Worksheets)
                                {
                                    //копируем все листы с содержимым
                                    if (worksheet_.Name != "Преподаватель" && worksheet_.Name != "Предмет" && worksheet_.Name != "Время пар")
                                    {
                                        //копируем лист для студентов
                                        var new_worksheet = result_S.Workbook.Worksheets.Add(worksheet_.Name, worksheet_);
                                        //проверяем наличие листа в преподавтелях
                                        if (new_worksheet_T == null)
                                            new_worksheet_T = result_T.Workbook.Worksheets.Add(worksheet_.Name, worksheet_);
                                        else
                                            flag = false;
                                        end_row = worksheet_.Dimension.End.Row;
                                        end_col = worksheet_.Dimension.End.Column;
                                        progressBar1.Increment(10);
                                        for (int i = 4; i <= end_row; i++)
                                        {
                                            if (new_worksheet.Cells[i, 3].Value != null)
                                            {
                                                //ставим время пар в представлении для студентов
                                                new_worksheet.Cells[i, 3].Value = Transformer.Time[Convert.ToInt32(new_worksheet.Cells[i, 3].Value)];
                                                //ставим время пар в представлении для преподавателей
                                                if (flag)
                                                    new_worksheet_T.Cells[i, 4, i, end_col].Value = null;
                                                new_worksheet_T.Cells[i, 3].Value = Transformer.Time[Convert.ToInt32(worksheet_.Cells[i, 3].Value)];

                                                string address = "";
                                                progressBar1.Increment(7 / (end_row - 3));
                                                for (int j = 4; j <= end_col; j++)
                                                {
                                                    if ((worksheet_.Cells[i - 1, j].Value != null) && (i == 4))
                                                    {
                                                        Transformer.Groups.Add(Transformer.Groups.Count + 1, worksheet_.Cells[i - 1, j].Value.ToString());
                                                    }
                                                    progressBar1.Increment(6 / (end_col - 3));
                                                    //изменяем ячейку для студентов
                                                    if (new_worksheet.Cells[i, j].Value != null)
                                                    {
                                                        string[] mas = new_worksheet.Cells[i, j].Value.ToString().Split(',');
                                                        new_worksheet.Cells[i, j].Value = Transformer.Disciplines[Convert.ToInt32(mas[0])] + Convert.ToChar(10) + Transformer.Teachers[Convert.ToInt32(mas[1])].Name;

                                                        if ((new_worksheet.Cells[i, j - 1].Value != null) && (new_worksheet.Cells[i, j].Value.ToString() == new_worksheet.Cells[i, j - 1].Value.ToString()))
                                                        {
                                                            address = (address.IndexOf(":") == -1 ? address + ":" : address.Substring(0, address.IndexOf(":") + 1)) + new_worksheet.Cells[i, j].Address;
                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(address))
                                                                new_worksheet.Cells[address].Merge = true;
                                                            address = new_worksheet.Cells[i, j].Address;
                                                        }
                                                    }
                                                    // изменяем ячейку для преподавателей
                                                    if (worksheet_.Cells[i, j].Value != null)
                                                    {
                                                        string[] mas = worksheet_.Cells[i, j].Value.ToString().Split(',');
                                                        if (Transformer.Teachers[Convert.ToInt32(mas[1])].Column == 0)
                                                        {
                                                            new_col++;
                                                            Transformer.Teachers[Convert.ToInt32(mas[1])].Column = new_col;
                                                            new_worksheet_T.Cells[3, new_col].Value = Transformer.Teachers[Convert.ToInt32(mas[1])].Name;
                                                        }
                                                        if (new_worksheet_T.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value == null)
                                                        {
                                                            new_worksheet_T.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value = Transformer.Disciplines[Convert.ToInt32(mas[0])] + Convert.ToChar(10) + worksheet_.Cells[3, j].Value.ToString();
                                                        }
                                                        else
                                                        {
                                                            new_worksheet_T.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value = new_worksheet_T.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value.ToString() + ", " + worksheet_.Cells[3, j].Value.ToString();
                                                        }
                                                    }
                                                    progressBar1.Increment(16 / (end_col - 3));
                                                }
                                                if (!string.IsNullOrEmpty(address))
                                                    new_worksheet.Cells[address].Merge = true;
                                            }
                                        }
                                        new_worksheet_T.Cells[3, new_col + 1, end_row, end_col].Clear();
                                    }
                                }
                                progressBar1.Value = 100;

                                //сохраняем
                                result_S.Save();
                                result_T.Save();
                                MessageBox.Show("Экспортирование завершено");
                                btnOpenStudents.Enabled = true;
                                btnOpenTeachers.Enabled = true;
                            }
                        }

                    }
                }
                catch(IOException)
                {
                    MessageBox.Show("Файл уже используется. Закройте его и повторите попытку.");
                }
            }
        }

        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog()==DialogResult.OK)
            {
                txtPath.Text = fbd.SelectedPath;
            }
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = !string.IsNullOrEmpty(txtPath.Text);
        }

        private void btnOpenStudents_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(fi_to_S.FullName);
        }

        private void btnOpenTeachers_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(fi_to_T.FullName);
        }
    }
}
