using System.ComponentModel.DataAnnotations;
using ZATApp.Common.Validators;

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
        [Display(Name ="Balance")]
        public decimal Balance { get; set; }
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }
        [Display(Name ="Cleared")]
        public bool IsCleared { get; set; }
    }

}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member