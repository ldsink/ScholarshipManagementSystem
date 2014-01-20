using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ScholarshipManagementSystem.Models
{
    public class English
    {
        [Key]
        [Required]
        public String Id { get; set; }

        public int CET4 { get; set; }
        public int CET6 { get; set; }
        public int TOFEL { get; set; }
    }
}