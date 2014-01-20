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
    [Authorize(Roles = "Student")]
    public class ScoringController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/Scoring
        public List<ScoringDTO> GetScoringDTOs()
        {
            List<ScoringT> scoringts = db.ScoringTs.Where(
                (p) => string.Equals(p.ScoringStudentInfoId, User.Identity.Name)).ToList();

            List<ScoringDTO> scoringdtos = new List<ScoringDTO>();
            for (int i = 0, tmp_count = scoringts.Count(); i < tmp_count; i++)
            {
                ScoringDTO scdto = new ScoringDTO();
                scdto.Id = scoringts[i].Id;
                scdto.ScoringStudentInfoId = scoringts[i].ScoringStudentInfoId;
                scdto.ScoredStudentInfoId = scoringts[i].ScoredStudentInfoId;
                scdto.ScoredStudentName = scoringts[i].ScoredStudent.Name;
                scdto.A = scoringts[i].A;
                scdto.B = scoringts[i].B;
                scdto.C = scoringts[i].C;
                scdto.D = scoringts[i].D;
                scdto.E = scoringts[i].E;
                scdto.F = scoringts[i].F;
                scdto.Total = scoringts[i].Total;
                scdto.Notes = scoringts[i].Notes;
                scoringdtos.Add(scdto);
            }

            return scoringdtos;
        }
        
        // GET api/Scoring/?submit=
        public string GetSubmit(string submit)
        {
            if (submit == "check") {
                IEnumerable<ScoringT> scoringts;
                scoringts = db.ScoringTs.Where((p) => string.Equals(p.ScoringStudentInfoId, User.Identity.Name));
                if (scoringts == null) // 避免总记录数为零的情况，后面不必检测总人数为零。
                    return "数据库读取错误，提交失败！";
                
                // 班级整个表格的检测规则
                // 1.没有空记录
                foreach (ScoringT sc in scoringts)
                    if (sc.Total == 0)
                    {
                        string ret = "学号：" + sc.ScoredStudentInfoId;
                        ret += " 的打分尚未提交，请重试!";
                        return ret;
                    }
                // 2.低于20分或高于25分的人数均不超过班级人数的15%
                int total_num = 0;
                double total_score = 0.0;
                int low_num = 0;
                int high_num = 0;
                foreach (ScoringT sc in scoringts) {
                    total_num++;
                    total_score += sc.Total;
                    if (sc.Total < 20)
                        low_num++;
                    else if (sc.Total > 25)
                        high_num++;
                }
                if (((double)low_num / total_num) > 0.15)
                {
                    return "提交失败：打分低于20分的人数超过班级人数的15%!";
                }
                if (((double)high_num / total_num) > 0.15)
                {
                    return "提交失败：打分高于25分的人数超过班级人数的15%!";
                }
                // 3.低于和高于班级平均分的人数均超过班级人数的30%
                double avg_score = total_score / total_num;
                low_num = 0;
                high_num = 0;
                foreach (ScoringT sc in scoringts) {
                    if (sc.Total < avg_score)
                        low_num++;
                    else if (sc.Total > avg_score)
                        high_num++;
                }
                if (((double)low_num / total_num) <= 0.3)
                {
                    return "提交失败：打分低于班级平均分的人数应超过班级人数的30%!";
                }
                if (((double)high_num / total_num) <= 0.3)
                {
                    return "提交失败：打分高于班级平均分的人数应超过班级人数的30%!";
                }
                
                // 修改打分人的 SubmitScoring 记录
                StudentInfo sinfo = db.StudentInfoes.Find(User.Identity.Name);
                if (sinfo == null)
                    return "登录信息错误！\n请重新登录！";
                sinfo.SubmitScoring = true;
                db.Entry(sinfo).State = EntityState.Modified;
                db.SaveChanges();
                return "班级打分表提交成功！";
            }
            else if (submit == "UnSubmited")
            {
                StudentInfo sinfo = db.StudentInfoes.Find(User.Identity.Name);
                if (sinfo == null)
                    return "登录信息错误！\n请重新登录！";
                if (!sinfo.SubmitScoring)
                    return "你的班级打分表还未提交！";
                else return "";
            }
            else if (submit == "Submited")
            {
                StudentInfo sinfo = db.StudentInfoes.Find(User.Identity.Name);
                if (sinfo == null)
                    return "登录信息错误！\n请重新登录！";
                if (sinfo.SubmitScoring)
                    return "你的班级打分表已提交！";
                else return "";
            }
            else
                return "Not Found.";
        }
        
        // PUT api/Scoring/5
        public HttpResponseMessage PutScoringT(int id, ScoringDTO scoringdto)
        {
            if (ModelState.IsValid && id == scoringdto.Id)
            {
                try {
                    ScoringT scoringt = db.ScoringTs.Find(id);
                    StudentInfo sinfo = db.StudentInfoes.Find(User.Identity.Name);
                    if (sinfo == null || sinfo.Id != scoringt.ScoringStudentInfoId)
                        return Request.CreateResponse(HttpStatusCode.NotFound);

                    scoringt.A = scoringdto.A;
                    if (scoringt.A < 4.9999 || 8.0001 < scoringt.A) { 
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "A error]");
                    }
                    scoringt.B = scoringdto.B;
                    if (scoringt.B < 3.9999 || 7.0001 < scoringt.B)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "B error");
                    }
                    scoringt.C = scoringdto.C;
                    if (scoringt.C < 2.9999 || 6.0001 < scoringt.C)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "C error");
                    }
                    scoringt.D = scoringdto.D;
                    if (scoringt.D < 0.9999 || 3.0001 < scoringt.D)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "D error");
                    }
                    scoringt.E = scoringdto.E;
                    if (scoringt.E < 0.9999 || 3.0001 < scoringt.E)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "E error");
                    }
                    scoringt.F = scoringdto.F;
                    if (scoringt.F < 0.9999 || 3.0001 < scoringt.F)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "F error");
                    }
                    scoringt.Total = scoringdto.A + scoringdto.B + scoringdto.C + scoringdto.D + scoringdto.E + scoringdto.F;
                    scoringt.Notes = scoringdto.Notes;
                    if (scoringt.Notes != null && (scoringt.Notes.Contains(':') || scoringt.Notes.Contains('<') || scoringt.Notes.Contains('>') || scoringt.Notes.Contains('/') || scoringt.Notes.Contains('\'') || scoringt.Notes.Contains('\"')))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Notes contains illegal characters");
                    }
                    db.Entry(scoringt).State = EntityState.Modified;
                
                    sinfo.SubmitScoring = false;
                    db.Entry(sinfo).State = EntityState.Modified;

                    db.SaveChanges();
                }
                catch (Exception ex) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}