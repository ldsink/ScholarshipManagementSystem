using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Punishment
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public String StudentInfoId { get; set; }
        public virtual StudentInfo Student { get; set; }

        public String Type { get; set; }
        public DateTime Date { get; set; }
        public String Notes { get; set; }
        public bool Qualification { get; set; }
    }
}