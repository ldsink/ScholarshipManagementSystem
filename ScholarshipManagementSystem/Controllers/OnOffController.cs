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
    public class OnOffController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/OnOff
        public IEnumerable<OnOff> GetOnOffs()
        {
            return db.OnOffs.AsEnumerable();
        }

        // GET api/OnOff/5
        public OnOff GetOnOff(int id)
        {
            OnOff onoff = db.OnOffs.Find(id);
            if (onoff == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return onoff;
        }

        // PUT api/OnOff/5
        public HttpResponseMessage PutOnOff(int id, OnOff onoff)
        {
            if (ModelState.IsValid && id == onoff.Id)
            {
                db.Entry(onoff).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/OnOff
        public HttpResponseMessage PostOnOff(OnOff onoff)
        {
            if (ModelState.IsValid)
            {
                db.OnOffs.Add(onoff);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, onoff);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = onoff.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/OnOff/5
        public HttpResponseMessage DeleteOnOff(int id)
        {
            OnOff onoff = db.OnOffs.Find(id);
            if (onoff == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.OnOffs.Remove(onoff);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, onoff);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}