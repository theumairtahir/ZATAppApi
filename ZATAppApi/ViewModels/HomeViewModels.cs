using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZATAppApi.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class WeeklyCompletedRidesViewModel
    {
        public string linecolor { get; set; }
        public string title { get; set; }
        public List<GraphCordinate> values { get; set; }
    }
    public struct GraphCordinate
    {
        public string X { get; set; }
        public double Y { get; set; }
    }
    public class AccountsGraphViewModel
    {
        public AccountsGraphViewModel()
        {
            datasets = new Dataset[2];
        }
        public List<string> labels
        {
            get
            {
                List<string> lstLabels = new List<string>()
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                };
                return lstLabels;
            }
        }
        public Dataset[] datasets { get; set; }
        public class Dataset
        {
            public Dataset()
            {
                data = new List<decimal>();
            }
            public string label { get; set; }
            public string backgroundColor { get; set; }
            public int borderWidth { get; set; }
            public string borderColor { get; set; }
            public List<decimal> data { get; set; }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}