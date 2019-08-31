﻿using System;
using System.Runtime.Serialization;

namespace ZATAppApi.Models.Common
{
    /// <summary>
    /// Structure to store location attributes
    /// </summary>
    [DataContract]
    public struct Location
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        /// <summary>
        /// Returns the distance from a location point to this point
        /// </summary>
        /// <param name="from">Point from which the distance is being calculated to this point</param>
        /// <param name="type">Kilometers/Meters</param>
        /// <returns></returns>
        public double DistanceFromAPoint(Location from, DistanceType type = DistanceType.Kilometers)
        {
            //using haveresine's formula
            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = ToRadian(Latitude - from.Latitude);
            double dLon = ToRadian(Longitude - from.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(Latitude)) * Math.Cos(ToRadian(from.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }
        /// <summary>
        /// Returns the distance from a location point to this point
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from">Point from which the distance is being calculated to this point</param>
        /// <param name="type">Kilometers/Meters</param>
        /// <returns></returns>
        public double DistanceToAPoint(Location to, DistanceType type = DistanceType.Kilometers)
        {
            //using haveresine's formula
            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = ToRadian(to.Latitude - Latitude);
            double dLon = ToRadian(to.Longitude - Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(to.Latitude)) * Math.Cos(ToRadian(Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }
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
    /// <summary>
    /// Structure to store ratings and comments given by the rider to the driver after a successfull ride
    /// </summary>
    [DataContract]
    public struct RatingAndComments
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [DataMember]
        public int Rating { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public Rider Rider { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}