namespace ZATApp.Models.Common
{
    /// <summary>
    /// Structure to store location attributes
    /// </summary>
    public struct Location
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
    /// <summary>
    /// Structure to store ratings and comments given by the rider to the driver after a successfull ride
    /// </summary>
    public struct RatingAndComments
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Rider Rider { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}