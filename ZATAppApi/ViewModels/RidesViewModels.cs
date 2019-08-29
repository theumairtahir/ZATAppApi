using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATApp.Common.Validators;
using ZATApp.Models;

namespace ZATApp.ViewModels
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}