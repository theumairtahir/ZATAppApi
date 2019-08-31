using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATAppApi.Common.Validators;
using ZATAppApi.Models.Common;
using static ZATAppApi.Models.Vehicle;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ZATAppApi.ViewModels
{
    public class RegisterSubAdminViewModel
    {
        [Display(Name = "First Name", Prompt = "Enter the First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name", Prompt = "Enter the Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Country Code", Prompt = "+92")]
        [RegularExpression("[+][1-9][1-9]", ErrorMessage = "Country Code Should be '+92'")]
        [StringLength(3, ErrorMessage = "Country Code should not be greater or less than 3", MinimumLength = 3)]
        [Required]
        public string CountryCode { get; set; }
        [Display(Name = "Company Code", Prompt = "312")]
        [StringLength(3, ErrorMessage = "Company Code should not be greater or less than 3", MinimumLength = 3)]
        [RegularExpression("[3][0-9][0-9]", ErrorMessage = "Company Code must starts from 3 like: 300, 312, 345, 332")]
        [Required]
        public string CompanyCode { get; set; }
        [Display(Name = "Number", Prompt = "1234567")]
        [StringLength(7, ErrorMessage = "Number should not be greater or less than 7", MinimumLength = 7)]
        [RegularExpression(@"\b\d{7,7}\b", ErrorMessage = "There must be a number like: 1234567")]
        [Required]
        public string Number { get; set; }
        [Display(Name = "Areas")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Areas { get; set; }
        [Required]
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [UniqueUsername]
        public string Username { get; set; }
        //[Required]
        //[Display(Name = "Password")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[PasswordValidator]
        //public string Password { get; set; }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member