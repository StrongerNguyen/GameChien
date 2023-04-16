using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models
{
    public class UserSessionModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}