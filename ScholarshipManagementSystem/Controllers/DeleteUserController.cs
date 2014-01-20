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
using System.Web.Security;
using ScholarshipManagementSystem.Models;

namespace ScholarshipManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class DeleteUserController : ApiController
    {
        private StudentContext db = new StudentContext();

        // DELETE api/DeleteUserFirst/5
        public HttpResponseMessage DeleteStudentInfo(string id)
        {
            StudentInfo studentinfo = db.StudentInfoes.Find(id);
            if (studentinfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            // 删除班级打分中的全部记录
            {
                IEnumerable<ScoringT> s1 = db.ScoringTs.Where((p) => string.Equals(p.ScoringStudentInfoId, id));
                foreach (ScoringT s in s1)
                    db.ScoringTs.Remove(s);
                IEnumerable<ScoringT> s2 = db.ScoringTs.Where((p) => string.Equals(p.ScoredStudentInfoId, id));
                foreach (ScoringT s in s2)
                    db.ScoringTs.Remove(s);
            }
            // 将学生信息标记为注销
            studentinfo.Active = false;
            db.Entry(studentinfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, studentinfo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}