using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class ConfirmModel
    {
		[Required(ErrorMessage = "EMail is required")]
		[Display(Name = "Email")]
		[EmailAddress]
		public string EMail { get; set; }
		
		[Required(ErrorMessage ="Code is required")]
		public string Code { get; set; }
	}
}
