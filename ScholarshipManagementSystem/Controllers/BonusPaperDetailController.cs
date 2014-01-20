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
    public class BonusPaperDetailController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/BonusPaperDetail/5
        public BonusPaperDetail GetBonusPaperDetail(int id)
        {
            BonusPaperDetail bpd = db.BonusPaperDetails.SingleOrDefault((p) => (p.BelongedID == id));
            if (bpd != null)
            {
                bpd.BelongedBonusT = db.BonusTs.Find(bpd.BelongedID);
            }
            if (bpd == null || bpd.BelongedBonusT == null || bpd.BelongedBonusT.StudentInfoId != User.Identity.Name)
            {
                bpd = new BonusPaperDetail();
                bpd.Id = -1;
            }
            return bpd;
        }

        // PUT api/BonusPaperDetail/5
        public HttpResponseMessage PutBonusPaperDetail(int id, BonusPaperDetail bpd)
        {
            bpd.BelongedBonusT = db.BonusTs.Find(bpd.BelongedID);
            if (id != bpd.Id || bpd.BelongedBonusT == null || bpd.BelongedBonusT.StudentInfoId != User.Identity.Name)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            db.Entry(bpd).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
