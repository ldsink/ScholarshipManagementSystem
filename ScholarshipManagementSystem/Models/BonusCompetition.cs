using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarshipManagementSystem.Models
{
    public class BonusCompetition
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public int Num { get; set; }
        public String Type { get; set; }
        public String Name { get; set; }
        public String Content { get; set; }
        public float Score { get; set; }
    }
}