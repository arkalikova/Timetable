namespace Timetable
{
    internal class Settings
    {
        public static readonly string TeacherWorksheetName = "Преподаватель";
        public static readonly string DisciplinesWorksheetName = "Предметы";
        public static readonly string TimesWorksheetName = "Время пар";
        public static readonly string SuccessConvertationMessage = "Экспортирование завершено";
        public static readonly string FailedConvertationMessage = "Файл уже используется. Закройте его и повторите попытку.";
        public static readonly string ExcelFilter = "Файл Excel (*.xlsx)| *.xlsx";
        public static readonly string StudentsResultFileName = "\\Расписание для студентов.xlsx";
        public static readonly string TeachersResultFileName = "\\Расписание для преподавателей.xlsx";
    }
}
