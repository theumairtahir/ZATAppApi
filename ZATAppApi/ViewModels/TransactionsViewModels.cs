using System.ComponentModel.DataAnnotations;
using ZATApp.Common.Validators;

namespace ZATApp.ViewModels
{
    public class ViewUnverifiedTransactionsViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }
        [Display(Name = "Transaction Register Time")]
        public string Time { get; set; }
        [Display(Name = "Service Provider")]
        public string ServiceName { get; set; }
        [Display(Name = "Amount of Transaction")]
        public decimal Amount { get; set; }
        [Display(Name = "Driver's Name")]
        public string DriverName { get; set; }
        [Display(Name = "Verified")]
        public bool IsVerified { get; set; }
    }
    public class ReceivePaymentViewModel
    {
        [Required]
        [Display(Name ="CNIC-Number", Description ="Format: XXXXX-XXXXXXX-X")]
        [CNICValidator]
        public string Cnic { get; set; }
        [Required]
        [Display(Name ="Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }

}