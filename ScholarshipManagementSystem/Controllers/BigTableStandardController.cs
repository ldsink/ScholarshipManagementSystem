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
    public class BigTableStandardController : ApiController
    {
        private StudentContext db = new StudentContext();
        
        // GET api/BigTableStandard
        public BigTableStandard GetBigTableStandards()
        {
            return db.BigTableStandards.First();
        }

        // PUT api/BigTableStandard/1
        public HttpResponseMessage PutBigTableStandard(int id, BigTableStandard bigtablestandard)
        {
            if (!ModelState.IsValid) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != bigtablestandard.Id) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(bigtablestandard).State = EntityState.Modified;

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