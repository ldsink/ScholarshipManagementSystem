using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarshipManagementSystem.Models
{
    public class BigTableStandard
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public String Grade { get; set; }
        public float Study { get; set; }
        public float Scoring { get; set; }
        public float Dormitory { get; set; }
        public float Bonus { get; set; }
    }
}
