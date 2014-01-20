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
    public class AnnounceBonusCompetitionController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/AnnounceBonusCompetition
        public IEnumerable<BonusCompetition> GetBonusCompetitions()
        {
            return db.BonusCompetitions.AsEnumerable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}