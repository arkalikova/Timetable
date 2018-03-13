using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;

namespace Timetable
{
    internal class Converter
    {
        private static DataContainer _dataContainer;
        public static void ConvertTemplateToResult(
            ref DataContainer dataContainer,
            FileInfo fiFrom,
            FileInfo filePathToS,
            FileInfo filePathToT,
            ProgressBar progressBar)
        {
            _dataContainer = dataContainer;
            using (var workbook = new ExcelPackage(fiFrom).Workbook)
            {
                using (var resultS = new ExcelPackage(filePathToS))
                {
                    using (var resultT = new ExcelPackage(filePathToT))
                    {
                        ClearDictionaries();
                        FillTransformerDictionaries(progressBar, workbook);
                        ConvertWorksheets(progressBar, workbook, resultS, resultT);
                        SaveResults(resultS, resultT);
                    }
                }
            }
        }

        private static void SaveResults(
            ExcelPackage resultS,
            ExcelPackage resultT)
        {
            resultS.Save();
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = excel.Workbooks.Open(resultS.File.FullName);
            foreach (Worksheet sh in wb.Sheets)
                sh.Columns.AutoFit();
            wb.SaveAs(resultS.File.FullName.Remove(resultS.File.FullName.Length - 1, 1), XlFileFormat.xlWorkbookDefault);
            excel.Quit();
            File.Delete(resultS.File.FullName);
            resultT.Save();
            excel = new Microsoft.Office.Interop.Excel.Application();
            wb = excel.Workbooks.Open(resultT.File.FullName);
            foreach (Worksheet sh in wb.Sheets)
                sh.Columns.AutoFit();
            wb.SaveAs(resultT.File.FullName.Remove(resultT.File.FullName.Length - 1, 1), XlFileFormat.xlWorkbookDefault);
            excel.Quit();
            File.Delete(resultT.File.FullName);
        }

        private static void ConvertWorksheets(
            ProgressBar progressBar,
            ExcelWorkbook workbook,
            ExcelPackage resultS,
            ExcelPackage resultT)
        {
            ExcelWorksheet newWorksheetT = null;
            var flag = true;
            var newCol = 3;

            foreach (var excelWorksheet in workbook.Worksheets)
            {
                //копируем все листы с содержимым
                if (excelWorksheet.Name != Settings.TeacherWorksheetName &&
                    excelWorksheet.Name != Settings.DisciplinesWorksheetName &&
                    excelWorksheet.Name != Settings.TimesWorksheetName &&
                    excelWorksheet.Name != Settings.AuditoriaWorksheetName)
                {
                    //копируем лист для студентов
                    var newWorksheet = resultS.Workbook.Worksheets.Add(excelWorksheet.Name, excelWorksheet);
                    //проверяем наличие листа в преподавтелях
                    if (newWorksheetT == null)
                    {
                        newWorksheetT = resultT
                            .Workbook
                            .Worksheets
                            .Add(Settings.TeacherWorksheetName, excelWorksheet);
                    }
                    else
                    {
                        flag = false;
                    }

                    var endRow = excelWorksheet.Dimension.End.Row;
                    var endCol = excelWorksheet.Dimension.End.Column;
                    progressBar.Increment(10);

                    ConvertWorksheet(newWorksheet, newWorksheetT, excelWorksheet,
                        flag, endRow, endCol, ref newCol, progressBar);
                    newWorksheetT.Cells[3, newCol + 1, endRow, endCol].Clear();
                    //newWorksheet.Cells[3, 3, endRow, endCol].AutoFitColumns();
                    //newWorksheetT.Cells[3, 3, endRow, newCol].AutoFitColumns();
                }
            }
            progressBar.Value = 100;
        }

        private static void ConvertWorksheet(
            ExcelWorksheet newWorksheet,
            ExcelWorksheet newWorksheetT,
            ExcelWorksheet excelWorksheet,
            bool flag, 
            int endRow,
            int endCol,
            ref int newCol,
            ProgressBar progressBar)
        {
            for (var row = 4; row <= endRow; row++)
            {
                var newWorksheetCells = newWorksheet.Cells[row, 3].Value;
                if (newWorksheetCells != null)
                {
                    //ставим дату
                    //if (newWorksheet.Cells[row, 1].Value!=null)


                    //ставим время пар в представлении для студентов
                    string tmp = _dataContainer.Time[(Convert.ToInt32(newWorksheetCells))];
                    tmp = newWorksheet.Cells[row, 2].Value.ToString() + Convert.ToChar(10) + tmp;
                    newWorksheet.Cells[row, 2].Value = tmp;
                    newWorksheet.Cells[row, 2, row, 3].Merge = true;
                    //ставим время пар в представлении для преподавателей
                    if (flag)
                        newWorksheetT.Cells[row, 4, row, endCol].Value = null;

                    var address = "";
                    if (!newWorksheetT.Cells[row, 2, row, 3].Merge)
                    {
                        var excelWorksheetCell = excelWorksheet.Cells[row, 3].Value;
                        tmp = _dataContainer.Time[(Convert.ToInt32(excelWorksheetCell))];
                        tmp = newWorksheetT.Cells[row, 2].Value.ToString() + Convert.ToChar(10) + tmp;
                        newWorksheetT.Cells[row, 2].Value = tmp;
                        newWorksheetT.Cells[row, 2, row, 3].Merge = true;
                    }
                    progressBar.Increment(7 / (endRow - 3));

                    ChangeCellValues(newWorksheet,
                        newWorksheetT,
                        excelWorksheet, ref newCol, endCol, row, ref address, progressBar);

                    if (!string.IsNullOrEmpty(address))
                        newWorksheet.Cells[address].Merge = true;
                }
            }
        }

        private static void ChangeCellValues(
            ExcelWorksheet newWorksheet,
            ExcelWorksheet newWorksheetT,
            ExcelWorksheet excelWorksheet, 
            ref int newCol, 
            int endCol,
            int row,
            ref string address,
            ProgressBar progressBar)
        {
            var maxbreak = 0;
            var maxbreakT = 0;
            for (var col = 4; col <= endCol; col++)
            {
                if (excelWorksheet.Cells[row - 1, col].Value != null && row == 4)
                {
                    _dataContainer.Groups.Add(_dataContainer.Groups.Count + 1,
                        excelWorksheet.Cells[row - 1, col].Value.ToString());
                }
                progressBar.Increment(6 / (endCol - 3));

                ChangeStudentCellValue(newWorksheet, row, col, ref address, ref maxbreak);
                ChangeTeacherCellValue(excelWorksheet, newWorksheetT, row, col, ref newCol, ref maxbreakT);

                progressBar.Increment(16 / (endCol - 3));
            }
            newWorksheet.Row(row).Height = 52.5 + maxbreak * 26.3;
            newWorksheetT.Row(row).Height = 52.5 + maxbreakT * 26.3;
        }

        private static void FillTransformerDictionaries(ProgressBar progressBar, ExcelWorkbook workbook)
        {
            DataContainer.AddTeachers(workbook, _dataContainer.Teachers, Settings.TeacherWorksheetName);
            progressBar.Increment(10);
            DataContainer.AddToDataContainerDictionaries(workbook, _dataContainer.Disciplines, Settings.DisciplinesWorksheetName);
            progressBar.Increment(9);
            DataContainer.AddToDataContainerDictionaries(workbook, _dataContainer.Time, Settings.TimesWorksheetName);
            progressBar.Increment(8);
            DataContainer.AddToDataContainerDictionaries(workbook, _dataContainer.Auditorium, Settings.AuditoriaWorksheetName);
            progressBar.Increment(10);
        }

        private static void ClearDictionaries()
        {
            _dataContainer.Teachers.Clear();
            _dataContainer.Disciplines.Clear();
            _dataContainer.Time.Clear();
            _dataContainer.Auditorium.Clear();
            _dataContainer.Groups.Clear();
        }

        private static void ChangeTeacherCellValue(
            ExcelWorksheet excelWorksheet,
            ExcelWorksheet newWorksheetT,
            int row, 
            int col,
            ref int newCol,
            ref int maxbreakT)
        {
            var teachers = excelWorksheet.Cells[row, col].Value;
            if (teachers != null)
            {
                var mas = teachers
                    .ToString()
                    .Split(';',',');
                var disciplineIndex = "";
                var teacherIndex = "";
                var teacherColumn = 0;
                var val = "";
                var ind = 0;
                foreach (string s in mas)
                {
                    if (s.Length > 0)
                    switch (s[0])
                        {
                            case 'Д':
                                disciplineIndex = s;
                                break;
                            case 'П':
                                teacherIndex = s;
                                teacherColumn = _dataContainer.Teachers[teacherIndex].Column;
                                if (teacherColumn == 0)
                                {
                                    newCol++;

                                    var endrow = newWorksheetT.Dimension.End.Row;

                                    newWorksheetT.Cells[3, 4, endrow, 4].Copy(newWorksheetT.Cells[3, newCol, endrow, newCol]);
                                    newWorksheetT.Cells[3, newCol, endrow, newCol].Value = null;
                                    //newWorksheetT.Cells[1, newCol, endrow, newCol].StyleID = newWorksheetT.Cells[1, 4, endrow, 4].StyleID;
                                    //newWorksheetT.Column(newCol).
                                    //newWorksheetT.Cells[3, newCol].StyleID = newWorksheetT.Cells[3, 4].StyleID;
                                    _dataContainer.Teachers[teacherIndex].Column = newCol;
                                    newWorksheetT.Cells[3, newCol].Value = _dataContainer
                                        .Teachers[teacherIndex].Name;
                                    teacherColumn = newCol;
                                }
                                if (newWorksheetT.Cells[row, teacherColumn].Value == null)
                                {
                                    newWorksheetT.Cells[row, teacherColumn].Value =
                                        _dataContainer.Disciplines[disciplineIndex] + Convert.ToChar(10) +
                                        excelWorksheet.Cells[3, col].Value;
                                }
                                else
                                {
                                    val = newWorksheetT.Cells[row, teacherColumn].Value.ToString();
                                    ind = val.IndexOf(_dataContainer.Disciplines[disciplineIndex]);
                                    var indbreak = -1;
                                    if (ind > -1)
                                    {
                                        indbreak = val.IndexOf('\n', ind);
                                        newWorksheetT.Cells[row, teacherColumn].Value =
                                                val.Substring(0, indbreak + 1) +
                                                excelWorksheet.Cells[3, col].Value + ", " +
                                                val.Substring(indbreak + 1);
                                    }
                                    else
                                        newWorksheetT.Cells[row, teacherColumn].Value =
                                            val + Convert.ToChar(10) +
                                            _dataContainer.Disciplines[disciplineIndex] + Convert.ToChar(10) +
                                            excelWorksheet.Cells[3, col].Value;
                                }
                                break;
                            case 'А':
                                val = newWorksheetT.Cells[row, teacherColumn].Value.ToString();
                                ind = val.IndexOf(_dataContainer.Disciplines[disciplineIndex]);
                                var indaud = val.IndexOf(_dataContainer.Auditorium[s]);
                                if (indaud == -1)
                                    newWorksheetT.Cells[row, teacherColumn].Value =
                                        val.Substring(0, ind + _dataContainer.Disciplines[disciplineIndex].Length) +
                                        ' ' + _dataContainer.Auditorium[s] +
                                        val.Substring(ind + _dataContainer.Disciplines[disciplineIndex].Length);
                                break;
                        }
                }
                newWorksheetT.Column(teacherColumn).Width = 75;
                val = newWorksheetT.Cells[row, teacherColumn].Value.ToString();
                var res = val.Length - val.Replace("\n", "").Length;
                maxbreakT = (res > maxbreakT ? res : maxbreakT);
            }
        }

        private static void ChangeStudentCellValue(
            ExcelWorksheet newWorksheet, 
            int row,
            int col, 
            ref string address,
            ref int maxbreak)
        {
            var disciplines = newWorksheet.Cells[row, col].Value;
            if (disciplines != null)
            {
                var mas = disciplines
                    .ToString()
                    .Split(';');
                var result = "";
                foreach (string s in mas)
                {
                    var mas1 = s.Split(',');
                    foreach(string s1 in mas1)
                    {
                        if (s1.Length>0)
                            switch (s1[0])
                            {
                                case 'Д':
                                    result += (result == "" ? "" : " ") + _dataContainer.Disciplines[s1];
                                    break;
                                case 'П':
                                    result += (result == "" ? "" : " ") + _dataContainer.Teachers[s1].Name;
                                    break;
                                case 'А':
                                    result += (result == "" ? "" : " ") + _dataContainer.Auditorium[s1];
                                    break;
                            }
                    }
                    result += Convert.ToChar(10);
                }

                result = result.Remove(result.Length - 1);
                newWorksheet.Cells[row, col].Value = result;

                if (newWorksheet.Cells[row, col - 1].Value != null &&
                    result == newWorksheet.Cells[row, col - 1]
                        .Value.ToString())
                {
                    var addressPart = address.Substring(0,
                        address.IndexOf(":", StringComparison.Ordinal) + 1);
                    var addressPreffix =
                        address.IndexOf(":", StringComparison.Ordinal) == -1
                            ? address + ":"
                            : addressPart;

                    address = addressPreffix + newWorksheet.Cells[row, col]
                                  .Address;
                }
                else
                {
                    if (!String.IsNullOrEmpty(address))
                        newWorksheet.Cells[address].Merge = true;
                    address = newWorksheet.Cells[row, col].Address;
                }
            }

            newWorksheet.Column(col).Width = 70;
            if (newWorksheet.Cells[row, col].Value != null)
            {
                var val = newWorksheet.Cells[row, col].Value.ToString();
                var res = val.Length - val.Replace("\n", "").Length;
                maxbreak = (res > maxbreak ? res : maxbreak);
            }
            else
                maxbreak = 0;
        }
    }
}
