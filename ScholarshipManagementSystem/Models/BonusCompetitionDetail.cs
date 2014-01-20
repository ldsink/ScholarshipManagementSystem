using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarshipManagementSystem.Models
{
    public class BonusCompetitionDetail
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        public int BelongedID { get; set; }
        [ForeignKey("BelongedID")]
        public BonusT BelongedBonusT;

        public String Level { get; set; } // 竞赛等级
        public String LName { get; set; } // 项目名称
        public String HName { get; set; } // 比赛名称
        public String Ranking { get; set; } // 获奖等级
        public String Content { get; set; } // 内容
    }
}
