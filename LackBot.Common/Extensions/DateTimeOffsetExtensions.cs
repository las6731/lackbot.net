using System;

namespace LackBot.Common.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// <para>Truncates a DateTime to a specified resolution.</para>
        /// <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
        /// </summary>
        /// <param name="date">The DateTime object to truncate</param>
        /// <param name="resolution">e.g. to round to nearest second, TimeSpan.TicksPerSecond</param>
        /// <returns>Truncated DateTime</returns>
        public static DateTimeOffset Truncate(this DateTimeOffset date, long resolution)
        {
            var dt = new DateTime(date.Ticks - (date.Ticks % resolution), date.Date.Kind);
            return new DateTimeOffset(dt);
        }
    }
}