using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATApp.Common.Validators;
using ZATApp.Models.Common;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ZATApp.ViewModels
{
    public class ViewDriversViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "CNIC Number")]
        public string CNIC { get; set; }
        [Display(Name = "Balance")]
        public decimal Balance { get; set; }
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }
        [Display(Name = "Cleared")]
        public bool IsCleared { get; set; }
    }
    public class DriverDetailsViewModel
    {
        public long Id { get; set; }
        [Display(Name ="Full Name")]
        public string Name { get; set; }
        [Display(Name ="CNIC Number")]
        public string CNIC { get; set; }
        [Display(Name ="Phone Number")]
        public string ContactNumber { get; set; }
        [Display(Name ="Rating")]
        public decimal Rating { get; set; }
        [Display(Name ="Balance")]
        public decimal Balance { get; set; }
        [Display(Name ="Credit Limit")]
        public decimal CreditLimit { get; set; }
        [Display(Name ="Rides Completed")]
        public int RidesCompleted { get; set; }
        [Display(Name ="Blocked")]
        public bool IsBlocked { get; set; }
        [Display(Name ="Vehicle Type")]
        public string VehcileType { get; set; }
        [Display(Name ="Model")]
        public string VehicleModel { get; set; }
        [Display(Name ="Registeration Number")]
        public string RegisterationNumber { get; set; }
        public List<RatingAndComments> Comments { get; set; }
        public List<MobileTransactionsViewModel> MobileTransactions { get; set; }
        public List<ManualTransactionViewModel> ManualTransactions { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member