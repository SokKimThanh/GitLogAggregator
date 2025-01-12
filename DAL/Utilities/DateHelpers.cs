using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLogAggregator.Utilities
{
    /// <summary>
    /// Helper methods for date parsing and calculation
    /// </summary>
    public static class DateHelpers
    {
        // Parse day of the week from a string
        public static DayOfWeek ParseDayOfWeek(this string dayOfWeekStr)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekStr, true);
        }

        // Calculate the date from a given day of the week and start date
        public static DateTime GetDateFromDayOfWeek(this DayOfWeek dayOfWeek, DateTime startDate)
        {
            int dayOffset = (dayOfWeek - startDate.DayOfWeek) % 7;
            if (dayOffset < 0)
            {
                dayOffset += 7;
            }
            return startDate.AddDays(dayOffset);
        }

    }
}
