using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATApp.Models;
using ZATApp.Models.Common;

namespace ZATApp.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ViewRidersViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public string Contact { get; set; }
    }
    public class ViewRiderDetailsViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public string Contact { get; set; }
        [Display(Name = "Total Rides Completed")]
        public int CompletedRides { get; set; }
        public List<RideDetailsViewModel> Rides { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }

        public class RideDetailsViewModel
        {
            public Location StartPoint { get; set; }
            public Location EndPoint { get; set; }
            public Ride.RouteDetails Route { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public decimal AmountPaid { get; set; }
        }

    }
    public class EditRiderViewModel
    {
        public long Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Second Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Country Code", Prompt = "+92")]
        [RegularExpression("[+][1-9][1-9]", ErrorMessage = "Country Code Should be '+92'")]
        [StringLength(3, ErrorMessage = "Country Code should not be greater or less than 3", MinimumLength = 3)]
        [Required]
        public string CountryCode { get; set; }
        [Display(Name = "Country Code", Prompt = "312")]
        [StringLength(3, ErrorMessage = "Company Code should not be greater or less than 3", MinimumLength = 3)]
        [RegularExpression("[3][0-9][0-9]", ErrorMessage = "Company Code must starts from 3 like: 300, 312, 345, 332")]
        [Required]
        public string CompanyCode { get; set; }
        [Display(Name = "Country Code", Prompt = "1234567")]
        [StringLength(7, ErrorMessage = "Number should not be greater or less than 7", MinimumLength = 7)]
        [RegularExpression(@"\b\d{7,7}\b", ErrorMessage = "There must be a number like: 1234567")]
        [Required]
        public string Number { get; set; }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}