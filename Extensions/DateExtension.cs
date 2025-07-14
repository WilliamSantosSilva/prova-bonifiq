using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaPub.Extensions
{
    public static class DateExtension
    {
        public static DateTime CastTimeZone(this DateTime date, string timeZoneId)
        {
            var timeZoneFilter = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return timeZoneFilter != null ? TimeZoneInfo.ConvertTimeToUtc(date, timeZoneFilter) : date;
        }
    }
}