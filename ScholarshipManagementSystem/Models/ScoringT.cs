using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ScoringT
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public String ScoringStudentInfoId { get; set; }
        public virtual StudentInfo ScoringStudent { get; set; }

        public String ScoredStudentInfoId { get; set; }
        public virtual StudentInfo ScoredStudent { get; set; }

        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }
        public float E { get; set; }
        public float F { get; set; }
        public float Total { get; set; }
        public String Notes { get; set; }
    }
}