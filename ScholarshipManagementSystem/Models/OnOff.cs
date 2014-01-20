using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    public class OnOff
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Switch { get; set; }
    }
}