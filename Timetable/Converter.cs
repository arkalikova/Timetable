using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;

namespace Timetable
{
    internal class Converter
    {
        private static DataContainer _dataContainer;
        public static void ConvertTemplateToResult(
            FileInfo fiFrom,
            FileInfo filePathToS,
            FileInfo filePathToT,
            ProgressBar progressBar)
        {
            _dataContainer = new DataContainer();
            using (var workbook = new ExcelPackage(fiFrom).Workbook)
            {
                using (var resultS = new ExcelPackage(filePathToS))
                {
                    using (var resultT = new ExcelPackage(filePathToT))
                    {
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
            resultT.Save();
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
                    //newWorksheet.Cells[3, 1, endRow, endCol].AutoFitColumns();
                    //newWorksheetT.Cells[3, 1, endRow, newCol].AutoFitColumns();
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
            for (var col = 4; col <= endCol; col++)
            {
                if (excelWorksheet.Cells[row - 1, col].Value != null && row == 4)
                {
                    _dataContainer.Groups.Add(_dataContainer.Groups.Count + 1,
                        excelWorksheet.Cells[row - 1, col].Value.ToString());
                }
                progressBar.Increment(6 / (endCol - 3));

                ChangeStudentCellValue(newWorksheet, row, col, ref address);
                ChangeTeacherCellValue(excelWorksheet, newWorksheetT, row, col, ref newCol);

                progressBar.Increment(16 / (endCol - 3));
            }
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

        private static void ChangeTeacherCellValue(
            ExcelWorksheet excelWorksheet,
            ExcelWorksheet newWorksheetT,
            int row, 
            int col,
            ref int newCol)
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
                                    newWorksheetT.Cells[3, newCol].Copy(excelWorksheet.Cells[3, 4]);
                                    _dataContainer.Teachers[teacherIndex].Column = newCol;
                                    newWorksheetT.Cells[3, newCol].Value = _dataContainer
                                        .Teachers[teacherIndex].Name;
                                    teacherColumn = newCol;
                                }
                                if (newWorksheetT.Cells[row, teacherColumn].Value == null)
                                {
                                    newWorksheetT.Cells[row, teacherColumn].Copy(excelWorksheet.Cells[row, 4]);
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

                //var teacherIndex = mas[1];
                //var teacherColumn = _dataContainer.Teachers[teacherIndex].Column;
                //if (teacherColumn == 0)
                //{
                //    newCol++;
                //    _dataContainer.Teachers[teacherIndex].Column = newCol;
                //    newWorksheetT.Cells[3, newCol].Value = _dataContainer
                //        .Teachers[teacherIndex].Name;
                //    teacherColumn = newCol;
                //}
                //if (newWorksheetT.Cells[row, teacherColumn].Value == null)
                //{
                //    var disciplineIndex = mas[0];
                //    newWorksheetT.Cells[row, teacherColumn].Value =
                //        _dataContainer.Disciplines[disciplineIndex] + Convert.ToChar(10) +
                //        excelWorksheet.Cells[3, col].Value;
                //}
                //else
                //{
                //    newWorksheetT.Cells[row, teacherColumn].Value =
                //        newWorksheetT.Cells[row, teacherColumn].Value +
                //        ", " +
                //        excelWorksheet.Cells[3, col].Value;
                //}
            }
        }

        private static void ChangeStudentCellValue(
            ExcelWorksheet newWorksheet, 
            int row,
            int col, 
            ref string address)
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

                newWorksheet.Cells[row, col].Value = result;
                    /*_dataContainer.Disciplines[mas[0]] + Convert.ToChar(10) +
                    _dataContainer.Teachers[mas[1]].Name;*/

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
        }
    }
}
