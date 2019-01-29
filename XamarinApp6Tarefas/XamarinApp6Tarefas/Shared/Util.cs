using System;

namespace XamarinApp6Tarefas.Shared
{
    public static class Util
    {
        public static TimeSpan ToTimeSpan(this DateTime date)
        {
            var TimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
            return TimeSpan;
        }
    }
}
