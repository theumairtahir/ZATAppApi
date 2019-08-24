using System.ComponentModel.DataAnnotations;

namespace ZATApp.Common.Validators
{
    public class CNICValidator: RegularExpressionAttribute
    {
        public CNICValidator() : base("([0-9]{5})[-]([0-9]{7})[-]([0-9])")
        {
            ErrorMessage = "CNIC Number should be in XXXXX-XXXXXXX-X format";
        }
    }
}