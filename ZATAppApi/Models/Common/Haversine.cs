using System;
using ZATApp.Models.Common;

namespace ZATAppApi.Models.Common
{
    /// <summary>
    /// Class which contains methods to calculate distance between two points by using Haversine's Formula
    /// </summary>
    public static class Haversine
    {
        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double Distance(Location pos1, Location pos2, DistanceType type = DistanceType.Kilometers)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = ToRadian(pos2.Latitude - pos1.Latitude);
            double dLon = ToRadian(pos2.Longitude - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(pos1.Latitude)) * Math.Cos(ToRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static double ToRadian(decimal val)
        {
            return (Math.PI / 180) * Convert.ToDouble(val);
        }
        /// <summary>
        /// The distance type to return the results in.
        /// </summary>
        public enum DistanceType
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Miles,
            Kilometers
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        };
    }
}