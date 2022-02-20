﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CakesAsp.Areas.Admin.Models
{
    public class Account
    {
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string FullName { get; set; }
        public string Address { get; set; }
      
        public string Email { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string Phone { get; set; }
        public string Type  { get; set; }

        public bool Status { get; set; }
      

    }
}
