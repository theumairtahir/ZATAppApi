using System;

namespace ZATApp.Common.Functions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class UISupportiveFunctions
    {
        public static string GetPassedTimeSpanFromNow(DateTime time)
        {
            string s = time.ToString("dd-mmm-yyyy hh:mm:ss");
            TimeSpan span = DateTime.Now - time;
            var mins = decimal.Round(Convert.ToDecimal(span.TotalMinutes));
            var hrs = decimal.Round(Convert.ToDecimal(span.TotalHours));
            var days = decimal.Round(Convert.ToDecimal(span.TotalDays));
            if (mins < 1)
            {
                s = "Just Now";
            }
            else if (hrs < 1)
            {
                if (mins == 1)
                {
                    s = "1 minute ago";
                }
                else
                {
                    s = mins + " minutes ago";
                }
            }
            else if (days < 1)
            {
                if (hrs == 1)
                {
                    s = "1 hour ago";
                }
                else
                {
                    s = hrs + " hours ago";
                }
            }
            else if (days == 1)
            {
                s = "1 day ago";
            }
            else if (days < 31)
            {
                s = days + " days ago";
            }
            return s;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}