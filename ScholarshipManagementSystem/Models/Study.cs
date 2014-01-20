using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementSystem.Models
{
    public class Study
    {
        [Key]
        [Required]
        public String Id { get; set; }

        public float Score { get; set; }
    }
}