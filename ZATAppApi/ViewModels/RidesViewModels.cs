using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATAppApi.Common.Validators;
using ZATAppApi.Models;
using ZATAppApi.Models.Common;

namespace ZATAppApi.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SetFareViewModel
    {
        [Required]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Per Kilometer Charges (Rs.)")]
        public decimal DistanceTravelledPerKmCharges { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Ride Pick-Up Fee (Rs.)")]
        public decimal PickUpFee { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Ride Drop-off Fee (Rs.)")]
        public decimal DropOffFee { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Service Charges (%)")]
        [Range(0, 100, ErrorMessage = "Please Enter a valid percentage.")]
        public decimal ServiceCharges { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "GST (%)")]
        [Range(0, 100, ErrorMessage = "Please Enter a valid percentage.")]
        public decimal GST { get; set; }
    }

    public class ViewFaresViewModel
    {
        public ViewFaresViewModel()
        {
            CurrentFareInfo = new List<FareInfo>();
            UpdationHistory = new List<FareInfo>();
        }
        public List<FareInfo> CurrentFareInfo { get; set; }
        public List<FareInfo> UpdationHistory { get; set; }
        public class FareInfo
        {
            public string VehicleType { get; set; }
            public decimal PickUpFee { get; set; }
            public decimal DropOffFee { get; set; }
            public decimal Gst { get; set; }
            public decimal ServiceCharges { get; set; }
            public decimal DistanceTravelledPerKmFee { get; set; }
            public string DateOfInclusion { get; set; }
        }
    }
    public class PromoCodeViewModel
    {
        string code;
        public PromoCodeViewModel()
        {
            code = "";
        }
        [Required]
        [Display(Name = "Promo Code")]
        public string Code
        {
            get
            {
                return code.ToUpper();
            }
            set
            {
                code = value;
            }
        }
        [Required]
        [Display(Name = "Discount (%)")]
        [Range(0, 100, ErrorMessage = "Please Enter a valid percentage.")]
        public short Discount { get; set; }
        public bool IsOpen { get; set; }
    }
    public class BookRideViewModel
    {
        public decimal PLat { get; set; }
        public decimal Plng { get; set; }
        public decimal DLat { get; set; }
        public decimal Dlng { get; set; }
        [Display(Name ="Rider First Name")]
        [Required]
        public string RiderFirstName { get; set; }
        [Display(Name = "Rider Last Name")]
        [Required]
        public string RidierLastName { get; set; }
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
        [Required]
        [Display(Name ="Vehcile Type")]
        public int VehicleType { get; set; }
        [Required(AllowEmptyStrings =true)]
        [Display(Name ="Promo Code")]
        public string PromoCode { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}