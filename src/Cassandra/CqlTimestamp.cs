namespace Cassandra
{
    public struct CqlTimestamp
    {
        public long Miliseconds { get; set; }

        private const int MilisecondsPerSecond = 1000;
        private const int MilisecondsPerMinute = MilisecondsPerSecond * 60;
        private const int MilisecondsPerHour = MilisecondsPerMinute * 60;
        private const int MilisecondsPerDay = MilisecondsPerHour * 24;

        private static readonly int[] DaysToMonth365 = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        private static readonly int[] DaysToMonth366 = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

        private const int DaysPerYear = 365;
        private const int DaysPer4Years = DaysPerYear * 4 + 1;
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;
        private const int DaysPer400Years = DaysPer100Years * 4 + 1;

        public int Year
        {
            get
            {
                ParseDate(out var year, out _, out _);
                return year;
            }
        }

        public int Month
        {
            get
            {
                ParseDate(out _, out var month, out _);
                return month;
            }
        }

        public int Day
        {
            get
            {
                ParseDate(out _, out _, out var day);
                return day;
            }
        }
        
        public int Hour
        {
            get
            {
                ParseTime(out var hour, out _, out _, out _);
                return hour;
            }
        }

        public int Minute
        {
            get
            {
                ParseTime(out _, out var minute, out _, out _);
                return minute;
            }
        }

        public int Second
        {
            get
            {
                ParseTime(out _, out _, out var second, out _);
                return second;
            }
        }

        public int Milisecond
        {
            get
            {
                ParseTime(out _, out _, out _, out var milisecond);
                return milisecond;
            }
        }


        public CqlTimestamp(long miliseconds)
        {
            Miliseconds = miliseconds;
        }

        private void ParseDate(out int year, out int month, out int day)
        {
            int n = (int)(Miliseconds / MilisecondsPerDay);

            int y400 = n / DaysPer400Years;
            n -= y400 * DaysPer400Years;

            int y100 = n / DaysPer100Years;
            if (y100 == 4) y100 = 3;

            n -= y100 * DaysPer100Years;

            int y4 = n / DaysPer4Years;
            n -= y4 * DaysPer4Years;

            int y1 = n / DaysPerYear;
            if (y1 == 4) y1 = 3;

            year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1970;
            n -= y1 * DaysPerYear;

            bool leapYear = year % 400 == 0 || (year % 100 != 0 && year % 4 == 0);
            int[] days = leapYear ? DaysToMonth366 : DaysToMonth365;

            int m = (n >> 5) + 1;
            while (n >= days[m]) m++;

            month = m;
            day = n - days[m - 1] + 1;
        }

        private void ParseTime(out int hour, out int minute, out int second, out int milisecond)
        {
            var n = (int)(Miliseconds % MilisecondsPerDay);

            hour = n / MilisecondsPerHour;
            n -= hour * MilisecondsPerHour;

            minute = n / MilisecondsPerMinute;
            n -= minute * MilisecondsPerMinute;

            second = n / MilisecondsPerSecond;
            n -= second * MilisecondsPerSecond;

            milisecond = n;
        }

        public override string ToString()
        {
            ParseDate(out var year, out var month, out var day);
            ParseTime(out var hour, out var minute, out var second, out var milisecond);

            return $"{year:0000}-{month:00}-{day:00} {hour:00}:{minute:00}:{second:00}.{milisecond:000}";
        }
    }
}
