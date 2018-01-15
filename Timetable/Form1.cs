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
        String fromPath = @"C:\Users\Анастасия\Downloads\Telegram Desktop\17Vz_17Uz_s_30_10_2017_po_12_11_2017_utv_24_10_2017.xlsx";
        String toPath = @"C:\Users\Анастасия\Desktop\Table_To_Object.xlsx";

        //Напиши свои, а мои закомментируй
        //String fromPath = @"C:\Users\Анастасия\Downloads\Telegram Desktop\17Vz_17Uz_s_30_10_2017_po_12_11_2017_utv_24_10_2017.xlsx";
        //String toPath = @"C:\Users\Анастасия\Desktop\Table_To_Object.xlsx";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnDoStuff_Click(object sender, EventArgs e)
        {
            var fi_from = new FileInfo(fromPath);
            var fi_to = new FileInfo(toPath);
            
            //удаляем каждый раз результирующий файл
            if (File.Exists(toPath))
                File.Delete(toPath);

            using (var package = new ExcelPackage(fi_from))
            {
                var workbook = package.Workbook;
                using (var result = new ExcelPackage(fi_to))
                {
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        //копируем все листы с содержимым
                        var new_worksheet = result.Workbook.Worksheets.Add(worksheet.Name, worksheet);
                        int start_row = new_worksheet.Dimension.Start.Row;
                        int end_row = new_worksheet.Dimension.End.Row;
                        int start_col = new_worksheet.Dimension.Start.Column;
                        int end_col = new_worksheet.Dimension.End.Column;
                        for (int i = start_row; i < end_row; i++)
                        {
                            for (int j = start_col; j < end_col; j++)
                            {
                                //можно что-нибудь поделать
                            }
                        }
                    }

                    result.Save();
                }
                
            }
        }
    }
}
