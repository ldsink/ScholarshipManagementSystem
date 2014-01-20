using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    public class BonusFVD
    {
        public int Id { get; set; }
        public BonusType Bonustype { get; set; }

        public String StudentId { get; set; }
        public String StudentName { get; set; }
        public String StudentClassId { get; set; }
        
        public String State { get; set; }   // 状态
        public float Score { get; set; }    // 评分
        public String Notes { get; set; }   // 备注

        public String Type { get; set; }    // 类别
        public int Num { get; set; }        // 编号
        public String Content { get; set; } // 项目内容
        public String Level { get; set; }   // 项目/竞赛/会议级别
        public String Way { get; set; }     // 会议方向
        public String HName { get; set; }   // 竞赛/会议名称
        public String LName { get; set; }   // 参赛项目/论文题目
        public String Ranking { get; set; } // 获奖等级/作者顺序
        public String Date { get; set; }    // 时间
    }
}