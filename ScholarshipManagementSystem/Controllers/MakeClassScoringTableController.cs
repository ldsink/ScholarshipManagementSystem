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

namespace ScholarshipManagementSystem.Views.Home
{
    [Authorize(Roles = "Teacher")]
    public class MakeClassScoringTableController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/MakeClassScoringTable/5
        public HttpResponseMessage GetScoringT(int id)
        {
            if (id != 1)
                return Request.CreateResponse(HttpStatusCode.NotFound);

             //Clear all current data
            List<ScoringT> scoringts = db.ScoringTs.ToList();
            if (scoringts != null)
            {
                for (int i = 0; i < scoringts.Count(); i++)
                {
                    db.ScoringTs.Remove(scoringts[i]);
                }
            }
           
            try
            {
                db.SaveChanges();

                // Create new class scoring table
                List<StudentInfo> sInfos = db.StudentInfoes.ToList();
                for (int i = 0, tmp_counti = sInfos.Count(); i < tmp_counti; i++)
                {
                    if (sInfos[i].Active == true)
                        for (int j = 0, tmp_countj = sInfos.Count(); j < tmp_countj; j++)
                        {
                            if (sInfos[i].ClassId == sInfos[j].ClassId && sInfos[i] != sInfos[j] && sInfos[j].Active == true)
                            {
                                ScoringT st = new ScoringT();
                                st.ScoringStudentInfoId = sInfos[i].Id;
                                st.ScoringStudent = sInfos[i];
                                st.ScoredStudentInfoId = sInfos[j].Id;
                                st.ScoredStudent = sInfos[j];
                                db.ScoringTs.Add(st);
                            }
                        }
                    db.SaveChanges();
                }
                
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, ex.Message);
            }            
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}