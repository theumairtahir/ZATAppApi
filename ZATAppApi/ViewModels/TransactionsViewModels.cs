using System.ComponentModel.DataAnnotations;

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

}