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
                    excelWorksheet.Name != Settings.TimesWorksheetName)
                {
                    //копируем лист для студентов
                    var newWorksheet = resultS.Workbook.Worksheets.Add(excelWorksheet.Name, excelWorksheet);
                    //проверяем наличие листа в преподавтелях
                    if (newWorksheetT == null)
                    {
                        newWorksheetT = resultT
                            .Workbook
                            .Worksheets
                            .Add(excelWorksheet.Name, excelWorksheet);
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
                    newWorksheet.Cells[row, 3].Value = _dataContainer.Time[(Convert.ToInt32(newWorksheetCells))];
                    //ставим время пар в представлении для преподавателей
                    if (flag)
                        newWorksheetT.Cells[row, 4, row, endCol].Value = null;

                    var excelWorksheetCell = excelWorksheet.Cells[row, 3].Value;
                    var address = "";
                    newWorksheetT.Cells[row, 3].Value = _dataContainer.Time[Convert.ToInt32(excelWorksheetCell)];
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
            progressBar.Increment(20);
            DataContainer.AddToDataContainerDictionaries(workbook, _dataContainer.Disciplines, Settings.DisciplinesWorksheetName);
            progressBar.Increment(9);
            DataContainer.AddToDataContainerDictionaries(workbook, _dataContainer.Time, Settings.TimesWorksheetName);
            progressBar.Increment(8);
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
                    .Split(',')
                    .Select(Int32.Parse)
                    .ToArray();

                var teacherIndex = mas[1];
                var teacherColumn = _dataContainer.Teachers[teacherIndex].Column;
                if (teacherColumn == 0)
                {
                    newCol++;
                    _dataContainer.Teachers[teacherIndex].Column = newCol;
                    newWorksheetT.Cells[3, newCol].Value = _dataContainer
                        .Teachers[teacherIndex].Name;
                }
                if (newWorksheetT.Cells[row, teacherColumn].Value == null)
                {
                    var disciplineIndex = mas[0];
                    newWorksheetT.Cells[row, teacherColumn].Value =
                        _dataContainer.Disciplines[disciplineIndex] + Convert.ToChar(10) +
                        excelWorksheet.Cells[3, col].Value;
                }
                else
                {
                    newWorksheetT.Cells[row, teacherColumn].Value =
                        newWorksheetT.Cells[row, teacherColumn].Value +
                        ", " +
                        excelWorksheet.Cells[3, col].Value;
                }
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
                    .Split(',')
                    .Select(Int32.Parse)
                    .ToArray();

                newWorksheet.Cells[row, col].Value =
                    _dataContainer.Disciplines[mas[0]] + Convert.ToChar(10) +
                    _dataContainer.Teachers[mas[1]].Name;

                if (newWorksheet.Cells[row, col - 1].Value != null &&
                    disciplines.ToString() == newWorksheet.Cells[row, col - 1]
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
