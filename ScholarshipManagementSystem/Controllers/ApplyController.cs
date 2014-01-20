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
    public class ApplyController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/Apply/?apply=
        public string GetApply(string apply)
        {
            if (apply == "Scholarship")
            {
                if (db.StudentInfoes.Find(User.Identity.Name).ApplyScholarship == true)
                {
                    return "取消申请奖学金";
                }
                else
                {
                    return "申请奖学金";
                }
            }
            else if (apply == "Grant")
            {
                if (db.StudentInfoes.Find(User.Identity.Name).ApplyGrant == true)
                {
                    return "取消申请助学金";
                }
                else
                {
                    return "申请助学金";
                }
            }
            else if (apply == "submitScholarship")
            {
                db.StudentInfoes.Find(User.Identity.Name).ApplyScholarship =
                    !db.StudentInfoes.Find(User.Identity.Name).ApplyScholarship;
                db.SaveChanges();
                return "";
            }
            else if (apply == "submitGrant")
            {
                db.StudentInfoes.Find(User.Identity.Name).ApplyGrant =
                    !db.StudentInfoes.Find(User.Identity.Name).ApplyGrant;
                db.SaveChanges();
                return "";
            }
            else
                return "Not Found.";
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}