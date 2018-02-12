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
        String fromPath = @"C:\Users\Анастасия\Downloads\Telegram Desktop\Шаблон.xlsx";
        String toPathS = @"C:\Users\Анастасия\Desktop\ResultStudents.xlsx";
        String toPathT = @"C:\Users\Анастасия\Desktop\Resultteachers.xlsx";

        //Напиши свои, а мои закомментируй
        //String fromPath = @"C:\Users\Анастасия\Downloads\Telegram Desktop\17Vz_17Uz_s_30_10_2017_po_12_11_2017_utv_24_10_2017.xlsx";
        //String toPath = @"C:\Users\Анастасия\Desktop\Table_To_Object.xlsx";

        public Form1()
        {
            InitializeComponent();
            Transformer.Teachers = new Dictionary<int, Teacher>();
            Transformer.Disciplines = new Dictionary<int, string>();
            Transformer.Time = new Dictionary<int, string>();
            Transformer.Groups = new Dictionary<int, string>();
        }

        private void btnForStudents_Click(object sender, EventArgs e)
        {
            var fi_from = new FileInfo(fromPath);
            var fi_to = new FileInfo(toPathS);
            int start_row;
            int end_row;
            int start_col;
            int end_col;

            //удаляем каждый раз результирующий файл
            if (File.Exists(toPathS))
                File.Delete(toPathS);

            //чистим словари
            Transformer.Disciplines.Clear();
            Transformer.Groups.Clear();
            Transformer.Teachers.Clear();
            Transformer.Time.Clear();

            using (var package = new ExcelPackage(fi_from))
            {
                var workbook = package.Workbook;
                using (var result = new ExcelPackage(fi_to))
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
                    //Предметы
                    worksheet = workbook.Worksheets["Предмет"];
                    start_row = worksheet.Dimension.Start.Row;
                    end_row = worksheet.Dimension.End.Row;
                    for (int i = start_row + 1; i <= end_row; i++)
                    {
                        Transformer.Disciplines.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), worksheet.Cells[i, 2].Value.ToString());
                    }
                    //Время пар
                    worksheet = workbook.Worksheets["Время пар"];
                    start_row = worksheet.Dimension.Start.Row;
                    end_row = worksheet.Dimension.End.Row;
                    for (int i = start_row + 1; i <= end_row; i++)
                    {
                        Transformer.Time.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), worksheet.Cells[i, 2].Value.ToString());
                    }
                    foreach (var worksheet_ in workbook.Worksheets)
                    {
                        //копируем все листы с содержимым
                        if (worksheet_.Name != "Преподаватель" && worksheet_.Name != "Предмет" && worksheet_.Name != "Время пар")
                        {
                            var new_worksheet = result.Workbook.Worksheets.Add(worksheet_.Name, worksheet_);
                            end_row = new_worksheet.Dimension.End.Row;
                            end_col = new_worksheet.Dimension.End.Column;
                            for (int i = 4; i <= end_row; i++)
                            {
                                if (new_worksheet.Cells[i, 3].Value != null)
                                {
                                    new_worksheet.Cells[i, 3].Value = Transformer.Time[Convert.ToInt32(new_worksheet.Cells[i, 3].Value)];

                                    string address = "";
                                    for (int j = end_col; j >= 4; j--)
                                    {
                                        if ((new_worksheet.Cells[i - 1, j].Value != null) && (i == 4))
                                        {
                                            Transformer.Groups.Add(Transformer.Groups.Count + 1, new_worksheet.Cells[i - 1, j].Value.ToString());
                                        }
                                        if (new_worksheet.Cells[i, j].Value != null)
                                        {
                                            string[] mas = new_worksheet.Cells[i, j].Value.ToString().Split(',');
                                            new_worksheet.Cells[i, j].Value = Transformer.Disciplines[Convert.ToInt32(mas[0])] + Convert.ToChar(10) + Transformer.Teachers[Convert.ToInt32(mas[1])].Name;

                                            if ((new_worksheet.Cells[i, j + 1].Value != null) && (new_worksheet.Cells[i, j].Value.ToString() == new_worksheet.Cells[i, j + 1].Value.ToString()))
                                            {
                                                address = new_worksheet.Cells[i, j].Address + (address.IndexOf(":") == -1 ? ":" + address : address.Substring(address.IndexOf(":")));
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(address))
                                                    new_worksheet.Cells[address].Merge = true;
                                                address = new_worksheet.Cells[i, j].Address;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(address))
                                        new_worksheet.Cells[address].Merge = true;
                                }
                            }
                        }
                    }

                    result.Save();
                    MessageBox.Show("You've done excelennt work!");
                }
                
            }
        }

        private void btnForTeachers_Click(object sender, EventArgs e)
        {
            var fi_from = new FileInfo(fromPath);
            var fi_to = new FileInfo(toPathT);
            ExcelWorksheet new_worksheet = null;
            bool flag = true;
            int start_row;
            int new_col = 3;
            int end_row;
            int end_col;

            //удаляем каждый раз результирующий файл
            if (File.Exists(toPathT))
                File.Delete(toPathT);

            //чистим словари
            Transformer.Disciplines.Clear();
            Transformer.Groups.Clear();
            Transformer.Teachers.Clear();
            Transformer.Time.Clear();

            using (var package = new ExcelPackage(fi_from))
            {
                var workbook = package.Workbook;
                using (var result = new ExcelPackage(fi_to))
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
                    //Предметы
                    worksheet = workbook.Worksheets["Предмет"];
                    start_row = worksheet.Dimension.Start.Row;
                    end_row = worksheet.Dimension.End.Row;
                    for (int i = start_row + 1; i <= end_row; i++)
                    {
                        Transformer.Disciplines.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), worksheet.Cells[i, 2].Value.ToString());
                    }
                    //Время пар
                    worksheet = workbook.Worksheets["Время пар"];
                    start_row = worksheet.Dimension.Start.Row;
                    end_row = worksheet.Dimension.End.Row;
                    for (int i = start_row + 1; i <= end_row; i++)
                    {
                        Transformer.Time.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), worksheet.Cells[i, 2].Value.ToString());
                    }
                    foreach (var worksheet_ in workbook.Worksheets)
                    {
                        //копируем все листы с содержимым
                        if (worksheet_.Name != "Преподаватель" && worksheet_.Name != "Предмет" && worksheet_.Name != "Время пар")
                        {
                            if (new_worksheet == null)
                                new_worksheet = result.Workbook.Worksheets.Add(worksheet_.Name, worksheet_);
                            else
                                flag = false;
                            end_row = worksheet_.Dimension.End.Row;
                            end_col = worksheet_.Dimension.End.Column;
                            for (int i = 4; i <= end_row; i++)
                            {
                                if (worksheet_.Cells[i, 3].Value != null)
                                {
                                    if (flag)
                                        new_worksheet.Cells[i, 4, i, end_col].Value = null;
                                    new_worksheet.Cells[i, 3].Value = Transformer.Time[Convert.ToInt32(worksheet_.Cells[i, 3].Value)];
                                    
                                    for (int j = 4; j <= end_col; j++)
                                    {
                                        if ((worksheet_.Cells[i - 1, j].Value != null) && (i == 4))
                                        {
                                            Transformer.Groups.Add(Transformer.Groups.Count + 1, worksheet_.Cells[i - 1, j].Value.ToString());
                                        }
                                        if (worksheet_.Cells[i, j].Value != null)
                                        {
                                            string[] mas = worksheet_.Cells[i, j].Value.ToString().Split(',');
                                            if (Transformer.Teachers[Convert.ToInt32(mas[1])].Column==0)
                                            {
                                                new_col++;
                                                Transformer.Teachers[Convert.ToInt32(mas[1])].Column = new_col;
                                                new_worksheet.Cells[3, new_col].Value = Transformer.Teachers[Convert.ToInt32(mas[1])].Name;
                                            }
                                            if (new_worksheet.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value==null)
                                            {
                                                new_worksheet.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value = Transformer.Disciplines[Convert.ToInt32(mas[0])] + Convert.ToChar(10) + worksheet_.Cells[3, j].Value.ToString();
                                            }
                                            else
                                            {
                                                new_worksheet.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value = new_worksheet.Cells[i, Transformer.Teachers[Convert.ToInt32(mas[1])].Column].Value.ToString() + ", " + worksheet_.Cells[3, j].Value.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            new_worksheet.Cells[3, new_col + 1, end_row, end_col].Clear();
                        }
                    }

                    result.Save();
                    MessageBox.Show("You've done excelennt work!");
                }

            }
        }
    }
}
