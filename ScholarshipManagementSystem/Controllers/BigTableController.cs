using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ScholarshipManagementSystem.Models;

namespace ScholarshipManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class BigTableController : ApiController
    {
        private StudentContext db = new StudentContext();
        
        //GET api/BigTable
        public List<BigTableDTO> GetBigTableDTO()
        {
            List<StudentInfo> sis = db.StudentInfoes.ToList();
            List<BigTableDTO> bigtabledtos = new List<BigTableDTO>();
            //Select the students from the same grade, and get their Score, Dormitory, Bonus nd English Grades
            //which is included CET4, CET6 and TOFEL
            String classid = db.BigTableStandards.Find(1).Grade;
            //float avg_grade_score = CalculateGradeAvgScoring(classid);
            for (int i = 0, tmp_count = sis.Count(); i < tmp_count; i++)
            {
                if (sis[i].ClassId.Contains(classid))
                {
                    BigTableDTO btd = new BigTableDTO();
                    btd.Name = sis[i].Name;
                    btd.ClassId = sis[i].ClassId;
                    btd.NumId = sis[i].Id;
                    btd.Qualification = sis[i].Qualification;
                    btd.SubmitScoring = sis[i].SubmitScoring;
                    if (sis[i].Active == true)
                    {
                        btd.Scoring = CalculateScoring(sis[i].Id, sis[i].ClassId) + 24.0F;
                    }
                    else
                    {
                        btd.Scoring = 15.0F;
                    }
                    Study score = db.Studys.Find(sis[i].Id);
                    if (score != null)
                        btd.Score = score.Score;
                    else btd.Score = 0;
                    Dormitory dormitoty = db.Dormitorys.Find(sis[i].Id);
                    if (dormitoty != null)
                        btd.Dormitoty = (dormitoty.Score1 + dormitoty.Score2) / 2;
                    else btd.Dormitoty = 0;
                    btd.Bonus = CalculateBonus(sis[i].Id);
                    English english = db.Englishs.Find(sis[i].Id);
                    if (english != null)
                    {
                        btd.CET4 = english.CET4;
                        btd.CET6 = english.CET6;
                        btd.TOFEL = english.TOFEL;
                    }
                    else
                    {
                        btd.CET4 = 0;
                        btd.CET6 = 0;
                        btd.TOFEL = 0;
                    }
                    bigtabledtos.Add(btd);
                }
                else
                {
                    continue;
                }
            }
       
            if (bigtabledtos.Any())
            {
                //sort the grade(descending)，give the value of Ranking and Percent
                bigtabledtos.OrderByDescending(b => b.Score);
                int total = bigtabledtos.Count();
                foreach (BigTableDTO btdr in bigtabledtos)
                {
                    btdr.Ranking = 1; // default
                    foreach (BigTableDTO btdrcmp in bigtabledtos) {
                        if (btdrcmp.Score > btdr.Score)
                            btdr.Ranking ++;
                    }
                    btdr.Percent = (float)btdr.Ranking / (float)total;
                }

                BigTableStandard bts = db.BigTableStandards.First();
                if (bts != null)
                {
                    foreach (BigTableDTO btdt in bigtabledtos)
                    {
                        btdt.TotalScore = btdt.Score * bts.Study
                            + btdt.Scoring * bts.Scoring
                            + btdt.Dormitoty * bts.Dormitory
                            + btdt.Bonus * bts.Bonus;
                    }
                    
                    //sort the totalscore(descending)，give the value of TotalRanking and TotalPercent
                    bigtabledtos.OrderByDescending(b => b.TotalScore);
                    foreach (BigTableDTO btdr in bigtabledtos)
                    {
                        btdr.TotalRanking = 1; // default
                        foreach (BigTableDTO btdrcmp in bigtabledtos)
                        {
                            if (btdrcmp.TotalScore > btdr.TotalScore)
                                btdr.TotalRanking++;
                        }
                        btdr.TotalPercent = (float)btdr.TotalRanking / (float)total;
                    }
                }
            }
            return bigtabledtos;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private float CalculateBonus(string studentid)
        {
            float sum_bonus = 0;
            List<BonusT> bts = db.BonusTs.Where((p) => string.Equals(p.StudentInfoId, studentid)).ToList();
            for (int i = 0, tmp_count = bts.Count(); i < tmp_count; i++)
            {
                string pass_str = "通过";
                if (bts[i].Status == pass_str)
                {
                    if (bts[i].Bonustype == BonusType.ProjectBonus)
                    {
                        sum_bonus += db.BonusProjects.Find(bts[i].DetailID).Score;
                    }
                    else if (bts[i].Bonustype == BonusType.PaperBonus)
                    {
                        sum_bonus += db.BonusPapers.Find(bts[i].DetailID).Score;
                    }
                    else if (bts[i].Bonustype == BonusType.CompetitionBonus)
                    {
                        sum_bonus += db.BonusCompetitions.Find(bts[i].DetailID).Score;
                    }
                }
                else
                {
                    continue;
                }
            }
            return sum_bonus;
        }

        public float CalculateScoring(string studentid, string s_classid)
        {
            double sum_score = 0;
            int sum_num = 0;
            int class_sum_num = 0;
            double class_sum_score = 0;
            double avg_score = 0;     
            double class_avg_score = 0;
            double std_dev = 0;
            double tmp_sum_score = 0;
            int tmp_sum_num = 0;

            List <ScoringT> all_sts = db.ScoringTs.ToList();
        
            List<ScoringT> c_sts = new List<ScoringT>();
            List<StudentInfo> c_sis = new List<StudentInfo>();
            for (int i = 0, tmp_count = all_sts.Count(); i < tmp_count; i++)
            {
                if (all_sts[i].ScoredStudent.ClassId == s_classid)
                {
                    class_sum_score += all_sts[i].Total;
                    class_sum_num++;
                    c_sts.Add(all_sts[i]);
                    if (c_sis.Contains(all_sts[i].ScoredStudent) == false)
                    {
                        c_sis.Add(all_sts[i].ScoredStudent);
                    }
                }
                if (all_sts[i].ScoredStudentInfoId == studentid)
                {
                    sum_score += all_sts[i].Total;
                    sum_num++;
                }
            }
            Int32 num_of_stu = c_sis.Count();
            if (num_of_stu == 0)
            {
                return 0;
            }
            else if (num_of_stu == 1)
            {
                return 0;
            }
            else //计算标准差
            {
                class_avg_score = class_sum_score / (double)class_sum_num;
                foreach (StudentInfo csi in c_sis)
                {
                    for (int i = 0, j = c_sts.Count(); i < j; i++)
                    {
                        if (c_sts[i].ScoredStudentInfoId == csi.Id)
                        {
                            tmp_sum_score += c_sts[i].Total;
                            tmp_sum_num++;
                        }
                    }
                    if (tmp_sum_num != 0)
                    {
                        std_dev += (tmp_sum_score / tmp_sum_num - class_avg_score) * (tmp_sum_score / tmp_sum_num - class_avg_score);
                        tmp_sum_score = 0;
                        tmp_sum_num = 0;
                    }
                }
                //foreach (ScoringT cst in c_sts)
                //{
                //    std_dev += (cst.Total - class_avg_score) * (cst.Total - class_avg_score);
                //}
                std_dev = std_dev / (num_of_stu - 1); 
                std_dev = Math.Pow(std_dev, 0.5);
            }

            if (sum_num == 0)
            {
                return 0;
            }          
            else 
            {
                avg_score = sum_score / (double)sum_num;
            }
            if (Math.Abs(std_dev - 0) < Math.Pow(0.1, 8))
            {
                return 0;
            }
            else
            {
                double rtn_val = 0;
                rtn_val = ((avg_score - class_avg_score) / std_dev);
                return (float)rtn_val;
            }
        }
        
        //private float CalculateAvgScoring(String s_id, List<ScoringT> all_sts)
        //{
        //    double tmp_sum_score = 0;
        //    int tmp_sum_num = 0;
        //    for (int i = 0, tmp_count = all_sts.Count(); i < tmp_count; i++)
        //    {
        //        if (all_sts[i].ScoredStudentInfoId == s_id)
        //        {
        //            tmp_sum_score += all_sts[i].Total;
        //            tmp_sum_num++;
        //        }
        //    }
        //    if (tmp_sum_num == 0)
        //    {
        //        return 0F;
        //    }
        //    else
        //    {
        //        return (float)(tmp_sum_score / tmp_sum_num);
        //    }
        //}
        private float CalculateGradeAvgScoring(string gradeid)
        {
            int grade_sum_num = 0;
            double grade_sum_score = 0;
            double grade_avg_score = 0;

            List<ScoringT> scts = db.ScoringTs.ToList();
            for (int i = 0, tmp_count = scts.Count(); i < tmp_count; i++)
            {
                if (scts[i].ScoredStudent.ClassId.Contains(gradeid))
                {
                    grade_sum_score += scts[i].Total;
                    grade_sum_num++;
                }
            }
            if (grade_sum_num == 0)
            {
                return 0;
            }
            else
            {
                grade_avg_score = grade_sum_score / (double)grade_sum_num;
                return (float)grade_avg_score;
            }
            
        }
    }
}