namespace Timetable
{
    public class Teacher
    {
        [System.ComponentModel.DisplayName("Преподаватель")]
        public string Name { get; set; }
        [System.ComponentModel.DisplayName("Email")]
        public string Email { get; set; }
        [System.ComponentModel.DisplayName("Колонка")]
        public int Column { get; set; }
        [System.ComponentModel.DisplayName("Отправить письмо")]
        public bool IsNotificated { get; set; }
    }
}
