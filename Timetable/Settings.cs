namespace Timetable
{
    internal class Settings
    {
        public static readonly string TeacherWorksheetName = "Преподаватели";
        public static readonly string DisciplinesWorksheetName = "Предметы";
        public static readonly string TimesWorksheetName = "Время пар";
        public static readonly string AuditoriaWorksheetName = "Аудитории";
        public static readonly string SuccessConvertationMessage = "Экспортирование завершено";
        public static readonly string FailedConvertationMessage = "Файлы с шаблоном или расписаниями уже используется. Закройте их и повторите попытку.";
        public static readonly string FailedFoundListMessage = @"Произошла ошибка!
Возможные причины:
-Некорректно названы справочные листы
-Отсутствие одного или нескольких справочных листов";
        public static readonly string FailedFoundKeyMessage = @"Произошла ошибка!
Возможные причины:
-В шаблоне присутствуют идентификаторы, которые отсутствуют в справочнике";
        public static readonly string FailedOtherMessage = @"Произошла ошибка!
Возможные причины:
-Некорректно составлен шаблон";
        public static readonly string ExcelFilter = "Файл Excel (*.xlsx)| *.xlsx";
        public static readonly string StudentsResultFileName = "\\Расписание для студентов.xlsx";
        public static readonly string TeachersResultFileName = "\\Расписание для преподавателей.xlsx";
    }
}
