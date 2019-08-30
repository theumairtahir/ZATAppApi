using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATApp.Common.Validators;
using ZATApp.Models;

namespace ZATApp.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SmsViewModel
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "Message Body")]
        [StringLength(160, ErrorMessage = "The length of an SMS should not be greater than 160.")]
        [UIHint("Enter your Message")]
        public string Body { get; set; }
        [Display(Name = "Created")]
        public string Time { get; set; }
    }
    public class SmsReceiversViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public string Contact { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}