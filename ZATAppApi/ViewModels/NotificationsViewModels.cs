using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZATAppApi.Common.Validators;
using ZATAppApi.Models;

namespace ZATAppApi.ViewModels
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SmsViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Message Body")]
        public string Body { get; set; }
        [Display(Name = "Created")]
        public string Time { get; set; }
    }
    public class SendSmsViewModel
    {
        [Required]
        [Display(Name = "Message Body")]
        [StringLength(160, ErrorMessage = "The length of an SMS should not be greater than 160.")]
        [UIHint("Enter your Message")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        [Display(Name ="Send To")]
        [Required]
        public Receivers Receiver { get; set; }
        public long MessageId { get; set; }
        public enum Receivers
        {
            All=0,
            Drivers=1,
            Riders=2,
            [Display(Name ="Sub-Admin")]
            SubAdmin=3
        }
    }
    public class SmsReceiversViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public string Contact { get; set; }
    }
    public class DriversWithDuesViewModel
    {
        public long Id { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string Contact { get; set; }
        [Display(Name = "Amount Due")]
        public decimal AmountDue { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}