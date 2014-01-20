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
    public class BonusController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/Bonus
        public List<BonusDTO> GetBonusTs()
        {
            List<BonusT> bonusts = db.BonusTs.Where(
                (p) => string.Equals(p.StudentInfoId, User.Identity.Name)).ToList();

            List<BonusDTO> bonusdtos = new List<BonusDTO>();
            for (int i = 0, j = bonusts.Count(); i < j; i++)
            {
                BonusDTO bdto = new BonusDTO();
                bdto.Id = bonusts[i].Id;
                bdto.Bonustype = bonusts[i].Bonustype;
                if (bonusts[i].Bonustype == BonusType.ProjectBonus)
                {
                    bdto.Date = bonusts[i].Date.ToShortDateString();
                    bdto.State = bonusts[i].Status;
                    bdto.Notes = bonusts[i].Notes;
                    BonusProject bb = new BonusProject();
                    bb = db.BonusProjects.Find(bonusts[i].DetailID);
                    if (bb != null)
                    {
                        bdto.Type = bb.Type;
                        bdto.Num = bb.Num;
                        bdto.Content = bb.Content;
                        // bdto.HName = bb.Name;
                        bdto.Score = bb.Score;
                    }
                }
                else if (bonusts[i].Bonustype == BonusType.PaperBonus)
                {
                    bdto.Date = bonusts[i].Date.ToShortDateString();
                    bdto.State = bonusts[i].Status;
                    bdto.Notes = bonusts[i].Notes;
                    BonusPaper bp = new BonusPaper();
                    bp = db.BonusPapers.Find(bonusts[i].DetailID);
                    if (bp != null)
                    {
                        bdto.Type = bp.Type;
                        bdto.Num = bp.Num;
                        bdto.Content = bp.Content;
                        bdto.Score = bp.Score;
                    }
                    int temp_id = bonusts[i].Id;
                    BonusPaperDetail bpd = db.BonusPaperDetails.SingleOrDefault((p) => (p.BelongedID == temp_id));
                    if (bpd != null)
                    {
                        bdto.Level = bpd.Level;
                        bdto.Way = bpd.Way;
                        bdto.HName = bpd.HName;
                        bdto.LName = bpd.LName;
                        bdto.Ranking = bpd.Ranking;
                    }
                }
                else if (bonusts[i].Bonustype == BonusType.CompetitionBonus)
                {
                    bdto.Date = bonusts[i].Date.ToShortDateString();
                    bdto.State = bonusts[i].Status;
                    bdto.Notes = bonusts[i].Notes;
                    BonusCompetition bc = new BonusCompetition();
                    bc = db.BonusCompetitions.Find(bonusts[i].DetailID);
                    if (bc != null)
                    {
                        bdto.Type = bc.Type;
                        bdto.Num = bc.Num;
                        bdto.Content = bc.Content;
                        bdto.Score = bc.Score;
                    }
                    int temp_id = bonusts[i].Id;
                    BonusCompetitionDetail bcd = db.BonusCompetitionDetails.SingleOrDefault((p) => (p.BelongedID == temp_id));
                    if (bcd != null)                        
                    {
                        bdto.Level = bcd.Level;
                        bdto.HName = bcd.HName;
                        bdto.LName = bcd.LName;
                        bdto.Ranking = bcd.Ranking;
                    }
                }
                bonusdtos.Add(bdto);
            }
           
            return bonusdtos;
        }

        // GET api/Bonus/5
        public BonusDTO GetBonusT(int id)
        {
            BonusT b = db.BonusTs.Find(id);
            if (b == null || b.StudentInfoId != User.Identity.Name) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            BonusDTO bdto = new BonusDTO();
            bdto.Id = b.Id;
            bdto.Bonustype = b.Bonustype;
                if (b.Bonustype == BonusType.ProjectBonus)
                {
                    bdto.Date = b.Date.ToShortDateString();
                    bdto.State = b.Status;
                    bdto.Notes = b.Notes;
                    BonusProject bb = new BonusProject();
                    bb = db.BonusProjects.Find(b.DetailID);
                    if (bb != null)
                    {
                        bdto.Type = bb.Type;
                        bdto.Num = bb.Num;
                        bdto.Content = bb.Content;
                        // bdto.HName = bb.Name;
                        bdto.Score = bb.Score;
                    }
                }
                else if (b.Bonustype == BonusType.PaperBonus)
                {
                    bdto.Date = b.Date.ToShortDateString();
                    bdto.State = b.Status;
                    bdto.Notes = b.Notes;
                    BonusPaper bp = new BonusPaper();
                    bp = db.BonusPapers.Find(b.DetailID);
                    if (bp != null)
                    {
                        bdto.Type = bp.Type;
                        bdto.Num = bp.Num;
                        bdto.Content = bp.Content;
                        bdto.Score = bp.Score;
                    }
                    BonusPaperDetail bpd = new BonusPaperDetail();
                    bpd = db.BonusPaperDetails.SingleOrDefault((p) => (p.BelongedID == b.Id));
                    if (bpd != null)
                    {
                        bdto.Level = bpd.Level;
                        bdto.Way = bpd.Way;
                        bdto.HName = bpd.HName;
                        bdto.LName = bpd.LName;
                        bdto.Ranking = bpd.Ranking;
                    }
                }
                else if (b.Bonustype == BonusType.CompetitionBonus)
                {
                    bdto.Date = b.Date.ToShortDateString();
                    bdto.State = b.Status;
                    bdto.Notes = b.Notes;
                    BonusCompetition bc = new BonusCompetition();
                    bc = db.BonusCompetitions.Find(b.DetailID);
                    if (bc != null)
                    {
                        bdto.Type = bc.Type;
                        bdto.Num = bc.Num;
                        bdto.Content = bc.Content;
                        bdto.Score = bc.Score;
                    }
                    BonusCompetitionDetail bcd = new BonusCompetitionDetail();
                    bcd = db.BonusCompetitionDetails.SingleOrDefault((p) => (p.BelongedID == b.Id));
                    if (bcd != null)
                    {
                        bdto.Level = bcd.Level;
                        bdto.HName = bcd.HName;
                        bdto.LName = bcd.LName;
                        bdto.Ranking = bcd.Ranking;
                    }
                }
            return bdto;
        }

        // PUT api/Bonus/5
        public HttpResponseMessage PutBonusT(int id, BonusDTO bonusdto)
        {
            if (ModelState.IsValid && id == bonusdto.Id)
            {
                // check user and bonus.
                StudentInfo sinfo = db.StudentInfoes.Find(User.Identity.Name);
                if (sinfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                BonusT bonust = db.BonusTs.Find(id);
                if (bonust == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                if (bonust.StudentInfoId != sinfo.Id)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                bonust.Date = DateTime.Parse(bonusdto.Date);
                bonust.Notes = bonusdto.Notes;
                bonust.Status = "未审核";
                db.Entry(bonust).State = EntityState.Modified;
                try {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ex);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // POST api/Bonus
        public Int32 PostBonusT(BonusDTO bonusDTO)
        {
            int BonusNum = bonusDTO.Num;
            BonusT AddBonus = new BonusT();
            AddBonus.StudentInfoId = User.Identity.Name;
            AddBonus.Student = db.StudentInfoes.Find(AddBonus.StudentInfoId);
            AddBonus.Bonustype = CheckBonusType(BonusNum);
            AddBonus.DetailID = CheckID(BonusNum, AddBonus.Bonustype);
            AddBonus.Status = "未审核";
            AddBonus.Notes = "你的申报信息未填写完全，请点击编辑填写剩余信息！（信息补充完整后可删除本提示）";
            AddBonus.Date = DateTime.Now;

            try
            {
                if (AddBonus.Bonustype != BonusType.NoExistBonus)
                {
                    db.BonusTs.Add(AddBonus);
                    db.SaveChanges();
                    if (AddBonus.Bonustype == BonusType.CompetitionBonus)
                    {
                        BonusCompetitionDetail bcd = new BonusCompetitionDetail();
                        bcd.BelongedID = AddBonus.Id;
                        bcd.BelongedBonusT = AddBonus;
                        db.BonusCompetitionDetails.Add(bcd);
                        db.SaveChanges();
                    }
                    else if (AddBonus.Bonustype == BonusType.PaperBonus)
                    {
                        BonusPaperDetail bpd = new BonusPaperDetail();
                        bpd.BelongedID = AddBonus.Id;
                        bpd.BelongedBonusT = AddBonus;
                        db.BonusPaperDetails.Add(bpd);
                        db.SaveChanges();
                    }
                    return AddBonus.Id;
                }
                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // DELETE api/Bonus/5
        public HttpResponseMessage DeleteBonusT(int id)
        {
            // 验证加分项和用户身份
            BonusT bonust = db.BonusTs.Find(id);
            if (bonust == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            if (bonust.StudentInfoId != User.Identity.Name)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (bonust.Bonustype == BonusType.CompetitionBonus) {
                BonusCompetitionDetail bcd = db.BonusCompetitionDetails.SingleOrDefault((p) => (p.BelongedID == bonust.Id));
                if (bcd != null)
                    db.BonusCompetitionDetails.Remove(bcd);
            }
            else if (bonust.Bonustype == BonusType.PaperBonus) {
                BonusPaperDetail bpd = db.BonusPaperDetails.SingleOrDefault((p) => (p.BelongedID == bonust.Id));
                if (bpd != null)
                    db.BonusPaperDetails.Remove(bpd);
            }
            db.BonusTs.Remove(bonust);

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, bonust);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public BonusType CheckBonusType(int v) {
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

        public int CheckID(int v, BonusType bt) {
            if (bt == BonusType.PaperBonus) {
                IEnumerable<BonusPaper> re = db.BonusPapers.Where((p) => (p.Num == v));
                if (re.Any())
                    return re.First().Id;
                return -1;
            }
            if (bt == BonusType.ProjectBonus)
            {
                IEnumerable<BonusProject> re = db.BonusProjects.Where((p) => (p.Num == v));
                if (re.Any())
                    return re.First().Id; 
                return -1;  
            }
            if (bt == BonusType.CompetitionBonus)
            {
                IEnumerable<BonusCompetition> re = db.BonusCompetitions.Where((p) => (p.Num == v));
                if (re.Any())
                    return re.First().Id; 
                return -1;
            }
            return -1;
        }
    }
}
