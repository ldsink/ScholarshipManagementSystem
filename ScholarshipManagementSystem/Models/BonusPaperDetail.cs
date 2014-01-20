using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarshipManagementSystem.Models
{
    public class BonusPaperDetail
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public int BelongedID { get; set; }
        [ForeignKey("BelongedID")]
        public BonusT BelongedBonusT;

        public String Level { get; set; }   // 会议级别
        public String Way { get; set; }     // 会议方向
        public String LName { get; set; }   // 论文名称
        public String HName { get; set; }   // 会议名称
        public String Ranking { get; set; } // 作者顺序
        public String Content { get; set; } // 内容
    }
}