using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using ZATAppApi.Models;

namespace ZATAppApi.Common.Validators
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
    public class UsernameValidator : RegularExpressionAttribute
    {
        public UsernameValidator() : base("[A-Za-z0-9@_]")
        {
            ErrorMessage = "Username can contain only Alphanumeric Values";
        }
    }
    public class UniqueUsername : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            SqlConnection con = new SqlConnection(DbModel.CONNECTION_STRING);
            SqlCommand cmd = new SqlCommand("IsUsernameTaken", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@username", System.Data.SqlDbType.NVarChar)).Value = Convert.ToString(value);
            con.Open();
            try
            {
                int val = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                if (val == 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Sorry! " + value + " is already taken");
                }
            }
            catch (Exception ex)
            {
                con.Close();
                return new ValidationResult("Sorry! " + value + " is already taken");
            }
        }
    }
    public class PasswordValidator : RegularExpressionAttribute
    {
        public PasswordValidator() : base("([A-Z]|[a-z]|[0-9])+")
        {
            ErrorMessage = "Your password must include letter and numbers";
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}