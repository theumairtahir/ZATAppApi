using System;

namespace ZATApp.Common
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Constants
    {
        public static readonly string APPLICATION_NAME = "ZAT";
        public static readonly string TAG_LINE = "Ride Booking App";
        public static readonly double AJAX_INTERVAL = 0.30; //min
        public static readonly int PAGGING_RANGE = 50;
        public static readonly string CURRENCY_SYMBOL = "Rs. ";
        public static readonly string GOOGLE_API_KEY = "AIzaSyC9T637lzXkuTe7q2Vy_1fIo3azZzDhHwM";
        public static readonly DateTime MINIMUM_CAR_MODEL_YEAR = new DateTime(2000, 1, 1);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}