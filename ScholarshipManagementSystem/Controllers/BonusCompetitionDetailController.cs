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
    public class BonusCompetitionDetailController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/BonusCompetitionDetail/5
        public BonusCompetitionDetail GetBonusCompetitionDetail(int id)
        {
            BonusCompetitionDetail bcd = db.BonusCompetitionDetails.SingleOrDefault((p) => (p.BelongedID == id));
            if (bcd != null) {
                bcd.BelongedBonusT = db.BonusTs.Find(bcd.BelongedID);
            }
            if (bcd == null || bcd.BelongedBonusT ==null || bcd.BelongedBonusT.StudentInfoId != User.Identity.Name)
            {
                bcd = new BonusCompetitionDetail();
                bcd.Id = -1;
            }
            return bcd;
        }

        // PUT api/BonusCompetitionDetail/5
        public HttpResponseMessage PutBonusCompetitionDetail(int id, BonusCompetitionDetail bcd)
        {
            bcd.BelongedBonusT = db.BonusTs.Find(bcd.BelongedID);
            if (id != bcd.Id || bcd.BelongedBonusT == null || bcd.BelongedBonusT.StudentInfoId != User.Identity.Name) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            db.Entry(bcd).State = EntityState.Modified;

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) {
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