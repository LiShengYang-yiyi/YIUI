using System;
using System.Text;

namespace ET
{
    public static partial class TimeDisplayHelper
    {
        [StaticField]
        private static readonly TimeSpan DayThreshold = TimeSpan.FromDays(1);

        [StaticField]
        private static readonly TimeSpan HourThreshold = TimeSpan.FromHours(1);

        [StaticField]
        private static readonly TimeSpan MinuteThreshold = TimeSpan.FromMinutes(1);

        #if YIUILocalize
        [StaticField]
        private static readonly string DayLocalize = "D";

        [StaticField]
        private static readonly string HourLocalize = "H";

        [StaticField]
        private static readonly string MinuteLocalize = "M";

        [StaticField]
        private static readonly string SecondLocalize = "S";
        #else
        [StaticField]
        private static readonly string DayLocalize = "D";

        [StaticField]
        private static readonly string HourLocalize = "H";

        [StaticField]
        private static readonly string MinuteLocalize = "M";

        [StaticField]
        private static readonly string SecondLocalize = "S";
        #endif

        // 时间格式化（支持多语言和灵活单位）
        public static string FormatDuration(long seconds, EDurationFormat format = EDurationFormat.Compact)
        {
            if (seconds < 0)
            {
                return $"0{SecondLocalize}";
            }

            var timeSpan = TimeSpan.FromSeconds(seconds);
            var sb = new StringBuilder();

            if (format == EDurationFormat.Compact)
            {
                if (timeSpan.Days > 0)
                {
                    sb.AppendFormat($"{timeSpan.Days}{DayLocalize}");
                    if (timeSpan.Hours > 0)
                    {
                        sb.AppendFormat($"{timeSpan.Hours}{HourLocalize}");
                    }
                }
                else if (timeSpan.Hours > 0)
                {
                    sb.AppendFormat($"{timeSpan.Hours}{HourLocalize}");
                    if (timeSpan.Minutes > 0)
                    {
                        sb.AppendFormat($"{timeSpan.Minutes}{MinuteLocalize}");
                    }
                }
                else
                {
                    if (timeSpan.Minutes > 0)
                    {
                        sb.AppendFormat($"{timeSpan.Minutes}{MinuteLocalize}");
                    }

                    if (timeSpan.Seconds > 0)
                    {
                        sb.AppendFormat($"{timeSpan.Seconds}{SecondLocalize}");
                    }
                }
            }
            else
            {
                if (timeSpan.Days > 0)
                {
                    sb.AppendFormat($"{timeSpan.Days}{DayLocalize}").Append(' ');
                }

                if (timeSpan.Hours > 0)
                {
                    sb.AppendFormat($"{timeSpan.Hours}{HourLocalize}").Append(' ');
                }

                if (timeSpan.Minutes > 0)
                {
                    sb.AppendFormat($"{timeSpan.Minutes}{MinuteLocalize}").Append(' ');
                }

                if (timeSpan.Seconds > 0 || sb.Length == 0)
                {
                    sb.AppendFormat($"{timeSpan.Seconds}{SecondLocalize}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string FormatMinutesSeconds(long milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }

        public static string FormatTimestamp(long timestamp, string format = "yyyy-MM-dd HH:mm:ss", bool toLocal = false)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return (toLocal ? dateTime.LocalDateTime : dateTime.DateTime).ToString(format);
        }
    }

    public enum EDurationFormat
    {
        Compact, // 紧凑模式（最多显示两个单位）
        Full // 完整模式（显示所有非零单位）
    }
}