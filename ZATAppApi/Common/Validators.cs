using System;
using System.ComponentModel.DataAnnotations;

namespace ZATApp.Common.Validators
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CNICValidator : RegularExpressionAttribute
    {
        public CNICValidator() : base("([0-9]{5})[-]([0-9]{7})[-]([0-9])")
        {
            ErrorMessage = "CNIC Number should be in XXXXX-XXXXXXX-X format";
        }
    }
    public class CurrentMonthRange : RangeAttribute
    {
        public CurrentMonthRange() : base(typeof(DateTime), DateTime.MinValue.ToShortDateString(), new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).Day).ToShortDateString())
        {

        }
    }
    public class CurrentDateRange : RangeAttribute
    {
        public CurrentDateRange() : base(typeof(DateTime), DateTime.MinValue.ToShortDateString(), DateTime.Now.ToLongDateString())
        {

        }
    }
    public class RegisterationYearRange : RangeAttribute
    {
        public RegisterationYearRange() : base(typeof(DateTime), Constants.MINIMUM_CAR_MODEL_YEAR.ToLongDateString(), DateTime.Now.ToLongDateString())
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (base.IsValid(value, validationContext) == ValidationResult.Success)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? "Please Enter a Valid year between " + Constants.MINIMUM_CAR_MODEL_YEAR.Year + " and " + DateTime.Now.Year);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}