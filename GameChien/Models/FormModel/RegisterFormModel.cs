using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameChien.Models.FormModel
{
    public class RegisterFormModel
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string GameAccount { get; set; }
    }
}