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
        [Display(Name ="Active Status")]
        public bool IsActive { get; set; }
        [Display(Name ="Last Location")]
        public Location LastLocation { get; set; }
        public List<RatingAndComments> Comments { get; set; }
        public List<MobileTransactionsViewModel> MobileTransactions { get; set; }
        public List<ManualTransactionViewModel> ManualTransactions { get; set; }
    }
    public class EditDriverViewModel
    {
        [Display(Name ="First Name",Prompt ="Enter the First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name", Prompt = "Enter the Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Country Code", Prompt = "+92")]
        [RegularExpression("[+][1-9][1-9]", ErrorMessage ="Country Code Should be '+92'")]
        [StringLength(3,ErrorMessage ="Country Code should not be greater or less than 3", MinimumLength =3)]
        [Required]
        public string CountryCode { get; set; }
        [Display(Name = "Country Code", Prompt = "312")]
        [StringLength(3, ErrorMessage = "Company Code should not be greater or less than 3", MinimumLength = 3)]
        [RegularExpression("[3][0-9][0-9]", ErrorMessage = "Company Code must starts from 3 like: 300, 312, 345, 332")]
        [Required]
        public string CompanyCode { get; set; }
        [Display(Name = "Country Code", Prompt = "1234567")]
        [StringLength(7, ErrorMessage = "Number should not be greater or less than 7", MinimumLength = 7)]
        [RegularExpression(@"\b\d{7,7}\b", ErrorMessage = "Company Code must starts from 3 like: 300, 312, 345, 332")]
        [Required]
        public string Number { get; set; }
        [Required]
        [Display(Name = "Credit Limit", Prompt = "1000")]
        [DataType(DataType.Currency)]
        public decimal CreditLimit { get; set; }
        public long Id { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member