using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameChien.Models.FormModel
{
    public class LoginFormModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập tên tài khoản.")]
        public string AccountName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}