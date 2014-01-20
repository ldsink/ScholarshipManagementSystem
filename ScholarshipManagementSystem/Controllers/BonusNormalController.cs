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
    public class BonusNormalController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/BonusNormal
        public IEnumerable<BonusProject> GetBonusProjects()
        {
            return db.BonusProjects.AsEnumerable();
        }

        // PUT api/BonusNormal/5
        public HttpResponseMessage PutBonusProject(int id, BonusProject bonusproject)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != bonusproject.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(bonusproject).State = EntityState.Modified;
            IEnumerable<BonusT> bonusts = db.BonusTs.AsEnumerable();
            foreach (BonusT b in bonusts)
            {
                if (b.Bonustype == BonusType.ProjectBonus && b.DetailID == bonusproject.Id) {
                    b.Status = "未审核";
                    db.Entry(b).State = EntityState.Modified;
                }
            }

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

        // POST api/BonusNormal
        public HttpResponseMessage PostBonusProject(BonusProject bonusproject)
        {

            if (ModelState.IsValid)
            {
                if (CheckBonusType(bonusproject.Num) != BonusType.NoExistBonus)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                db.BonusProjects.Add(bonusproject);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, bonusproject);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = bonusproject.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/BonusNormal/5
        public HttpResponseMessage DeleteBonusProject(int id)
        {
            BonusProject bonusproject = db.BonusProjects.Find(id);
            if (bonusproject == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            // Remove BonusT records.
            IEnumerable<BonusT> bonusts = db.BonusTs.AsEnumerable();
            foreach (BonusT b in bonusts) {
                if (b.Bonustype == BonusType.ProjectBonus && b.DetailID == bonusproject.Id)
                    db.BonusTs.Remove(b);
            }
            // Remove BonusProject record.
            db.BonusProjects.Remove(bonusproject);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, bonusproject);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public BonusType CheckBonusType(int v)
        {
            IEnumerable<BonusPaper> re1 = db.BonusPapers.Where(
                (p) => (p.Num == v));
            if (re1.Any())
                return BonusType.PaperBonus;

            IEnumerable<BonusCompetition> re2 = db.BonusCompetitions.Where(
                (p) => (p.Num == v));
            if (re2.Any())
                return BonusType.CompetitionBonus;

            IEnumerable<BonusProject> re3 = db.BonusProjects.Where(
                (p) => (p.Num == v));
            if (re3.Any())
                return BonusType.ProjectBonus;

            return BonusType.NoExistBonus;
        }
    }
}
