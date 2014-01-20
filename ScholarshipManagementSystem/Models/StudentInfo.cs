using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class StudentInfo
    {
        [Key]
        [Required]
        public String Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String ClassId { get; set; }
        
        public bool ApplyGrant { get; set; }
        public bool ApplyScholarship { get; set; }
        public bool SubmitScoring { get; set; }
        public bool Qualification { get; set; }
        public bool Active { get; set; }
    }
}
