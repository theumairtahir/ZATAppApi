using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZATApp.Models;

namespace ZATAppApi.ApiModels
{
    /// <summary>
    /// A user of the System who books the ride and does related tasks
    /// </summary>
    public class RiderApiModel
    {
        /// <summary>
        /// Full Name of the rider
        /// </summary>
        [Required]
        public User.NameFormat FullName { get; set; }
        /// <summary>
        /// Country Code of the Contact Number
        /// </summary>
        [Required]
        [StringLength(3, ErrorMessage = "Country Code is not greater or smaller than 3 digits.", MinimumLength = 3)]
        [RegularExpression("[+][1-9][1-9]", ErrorMessage = "Country Code will be like: +92")]
        public string CountryCode { get; set; }
        /// <summary>
        /// Company Code of the Contact Number
        /// </summary>
        [Required]
        [StringLength(3, ErrorMessage = "Company Code can not be greater or smaller than 3 digits.", MinimumLength = 3)]
        [RegularExpression("[3][0-9][0-9]", ErrorMessage = "Company code starts from digit '3'")]
        public string CompanyCode { get; set; }
        /// <summary>
        /// Number of the Contact Number
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "Number can not be greater or smaller than 7 digits.", MinimumLength = 7)]
        [RegularExpression(@"\b\d{7,7}\b", ErrorMessage = "Number only contain digits")]
        public string Number { get; set; }
    }
}