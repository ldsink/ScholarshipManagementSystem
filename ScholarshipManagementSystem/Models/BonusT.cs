using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarshipManagementSystem.Models
{
    public enum BonusType { ProjectBonus = 1, CompetitionBonus, PaperBonus, NoExistBonus }; 

    public class BonusT
    {
        [Key]
        //[ScaffoldColumn(false)]
        public int Id { get; set; }
        
        public String StudentInfoId { get; set; }
        [ForeignKey("StudentInfoId")]
        public virtual StudentInfo Student { get; set; }

        [Required]
        public BonusType Bonustype { get; set; }
        [Required]
        public int DetailID { get; set; }

        public DateTime Date { get; set; }
        public String Status { get; set; }
        public String Notes { get; set; }
    }
}
