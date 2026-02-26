using System;

namespace ET
{
    /// <summary>
    /// 数值紧凑显示辅助类
    /// </summary>
    public static partial class NumberDisplayHelper
    {
        // 后缀定义（可扩展）
        [StaticField]
        private static readonly string[] Suffixes = { "", "K", "M", "B", "T", "Q", "S", "O", "N", "D" };

        /// <summary>
        /// 将长整型数字格式化为紧凑显示（如 1500 → "1.5K"）
        /// </summary>
        public static string Format(long value, int maxDecimals = 0, long minValueForSuffix = 1000)
        {
            if (value == 0)
            {
                return "0";
            }

            if (value < 0)
            {
                return $"-{Format(-value, maxDecimals, minValueForSuffix)}";
            }

            if (value < minValueForSuffix)
            {
                return value.ToString();
            }

            var suffixIndex = (int)(Math.Log(value) / Math.Log(1000));
            suffixIndex = Math.Clamp(suffixIndex, 0, Suffixes.Length - 1);

            var divisor = (long)Math.Pow(1000, suffixIndex);
            var integerPart = value / divisor;
            var fractionalPart = value % divisor;

            var formattedFraction = FormatFraction(fractionalPart, divisor, maxDecimals);
            return formattedFraction == "" ? $"{integerPart}{Suffixes[suffixIndex]}" : $"{integerPart}.{formattedFraction}{Suffixes[suffixIndex]}";
        }

        /// <summary>
        /// 用整数运算生成小数部分字符串
        /// </summary>
        private static string FormatFraction(long remainder, long divisor, int maxDecimals)
        {
            if (remainder == 0 || maxDecimals <= 0)
            {
                return "";
            }

            var fractionalValue = (double)remainder / divisor;
            var fractionalStr = fractionalValue.ToString($"F{maxDecimals}").TrimStart('0').TrimStart('.');
            return fractionalStr.TrimEnd('0');
        }

        /// <summary>
        /// 根据3n+2位规则格式化长整型数字（如10,000 → "10K"）
        /// </summary>
        /// <param name="value">原始数值（long类型避免浮点精度问题）</param>
        /// <param name="maxDecimals">最大小数位数（默认0位）</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatBy3n2Rule(long value, int maxDecimals = 0)
        {
            if (value == 0)
            {
                return "0";
            }

            if (value < 0)
            {
                return $"-{FormatBy3n2Rule(-value, maxDecimals)}";
            }

            var digitCount = GetIntegerDigitCount(value);

            int suffixIndex = 0;
            if (digitCount >= 5)
            {
                suffixIndex = (digitCount - 2) / 3;
                suffixIndex = Math.Clamp(suffixIndex, 0, Suffixes.Length - 1);
            }

            var divisor = (long)Math.Pow(1000, suffixIndex);
            var integerPart = value / divisor;
            var fractionalPart = value % divisor;

            var formattedFraction = FormatFraction(fractionalPart, divisor, maxDecimals);
            return formattedFraction == "" ? $"{integerPart}{Suffixes[suffixIndex]}" : $"{integerPart}.{formattedFraction}{Suffixes[suffixIndex]}";
        }

        private static int GetIntegerDigitCount(long value)
        {
            if (value < 10) return 1;
            return (int)Math.Floor(Math.Log10(value)) + 1;
        }
    }
}