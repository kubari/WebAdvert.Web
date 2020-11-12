﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		public bool RememberMe { get; set; }

	}
}
