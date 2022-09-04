using rentapp.BL.Enums;

namespace rentapp.BL.Helpers
{
    public class DateHelper
    {
        public static string GetDayOfWeekText(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Lunes";
                case DayOfWeek.Tuesday:
                    return "Martes";
                case DayOfWeek.Wednesday:
                    return "Miércoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sábado";
                default:
                    return "Domingo";
            }
        }

        public static string GetDateRangeText(DateRangeEnum dateRange)
        {
            switch (dateRange)
            {
                case DateRangeEnum.Day:
                    return "Día";
                case DateRangeEnum.Month:
                    return "Mes";
                case DateRangeEnum.Year:
                    return "Año";
                default:
                    return "Día";
            }
        }
    }
}
