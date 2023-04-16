using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Dto
{
    public class RelationshipDto
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; }
        public int Status { get; set; }
    }
}