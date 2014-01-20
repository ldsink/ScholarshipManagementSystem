using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementSystem.Models
{
    public class Dormitory
    {
        [Key]
        [Required]
        public String Id { get; set; }

        public float Score1 { get; set; }
        public float Score2 { get; set; }
    }
}