using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class SignupModel
    {
		[Required]
		[EmailAddress]
		[Display(Name ="EMail")]
        public string EMail { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(6, ErrorMessage = "Must be at least six characters length.")]
		[Display(Name ="Password")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password and its confirmation do not match.")]
		public string ConfirmPassword { get; set; }

	}
}
