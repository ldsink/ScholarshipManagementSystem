using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    public class BigTableDTO
    {
        public String Name { get; set; }//姓名
        public String ClassId { get; set; }//班号
        public String NumId { get; set; }//学号
        public float Score { get; set; }//学积分
        public int Ranking { get; set; }//学积分排名
        public float Percent { get; set; }//学积分比例
        public float Scoring { get; set; }//班级打分
        public float Dormitoty { get; set; }//宿舍得分
        public float Bonus { get; set; }//加分
        public int CET4 { get; set; }//四级成绩
        public int CET6 { get; set; }//六级成绩
        public int TOFEL { get; set; }//托福成绩
        public bool Qualification { get; set; }//奖（助)学金资格
        public bool SubmitScoring { get; set; }//提交班级打分表
        public float TotalScore { get; set; }//总分数
        public int TotalRanking { get; set; }//总排名
        public float TotalPercent { get; set; }//综合测评成绩比例
    }
}