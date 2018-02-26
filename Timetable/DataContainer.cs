using System;
using System.Collections.Generic;
using OfficeOpenXml;

namespace Timetable
{
    internal class DataContainer
    {
        public DataContainer()
        {
            Teachers = new Dictionary<int, Teacher>();
            Disciplines = new Dictionary<int, string>();
            Time = new Dictionary<int, string>();
            Groups = new Dictionary<int, string>();
        }

        public void ClearDictionaries()
        {
            Disciplines.Clear();
            Groups.Clear();
            Teachers.Clear();
            Time.Clear();
        }

        public static void AddToDataContainerDictionaries(
            ExcelWorkbook workbook,
            IDictionary<int, string> dictionary,
            string worksheetName)
        {
            var worksheet = workbook.Worksheets[worksheetName];
            var startRow = worksheet.Dimension.Start.Row;
            var endRow = worksheet.Dimension.End.Row;
            for (var i = startRow + 1; i <= endRow; i++)
            {
                var key = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                var worksheetValue = worksheet.Cells[i, 2].Value.ToString();

                dictionary.Add(key, worksheetValue);
            }
        }

        public static void AddTeachers(
            ExcelWorkbook workbook,
            IDictionary<int, Teacher> dictionary,
            string worksheetName)
        {
            var worksheet = workbook.Worksheets[worksheetName];
            var startRow = worksheet.Dimension.Start.Row;
            var endRow = worksheet.Dimension.End.Row;
            for (var i = startRow + 1; i <= endRow; i++)
            {
                dictionary.Add(Convert.ToInt32(worksheet.Cells[i, 1].Value), new Teacher
                {
                    Name = worksheet.Cells[i, 2].Value.ToString(),
                    Email = worksheet.Cells[i, 3].Value.ToString(),
                    Column = 0
                });
            }
        }

        public Dictionary<int, Teacher> Teachers { get; set; }
        public Dictionary<int, string> Disciplines { get; set; }
        public Dictionary<int, string> Time { get; set; }
        public Dictionary<int, string> Groups { get; set; }
    }
}
