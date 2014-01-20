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
    public class DisciplinaryController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/Disciplinary
        public List<PunishmentDTO> GetAllPunishmentDTOes()
        {
            List<Punishment> ps = db.Punishments.ToList();

            List<PunishmentDTO> pdtos = new List<PunishmentDTO>();
            for (int i = 0, tmp_count = ps.Count(); i < tmp_count; i++)
            {
                PunishmentDTO v = new PunishmentDTO();
                v.Id = ps[i].Id;
                v.SId = ps[i].Student.Id;
                v.Name = ps[i].Student.Name;
                v.Class = ps[i].Student.ClassId;
                v.Type = ps[i].Type;
                v.Date = ps[i].Date.ToShortDateString();
                v.Notes = ps[i].Notes;
                v.Qualification = ps[i].Qualification;
                pdtos.Add(v);
            }
           
            return pdtos;
        }

        // GET api/Disciplinary/5
        public List<PunishmentDTO> GetPunishmentDTOs(String name, String classs, String sid, String type)
        {
                List<PunishmentDTO> pdtos = GetAllPunishmentDTOes();
                if (sid != null)
                {
                    List<PunishmentDTO> temp = new List<PunishmentDTO>();
                    foreach (PunishmentDTO p in pdtos)
                        temp.Add(p);
                    pdtos.Clear();
                    foreach (PunishmentDTO p in temp)
                        if (p.SId.Contains(sid))
                            pdtos.Add(p);
                }
                if (name != null)
                {
                    List<PunishmentDTO> temp = new List<PunishmentDTO>();
                    foreach (PunishmentDTO p in pdtos)
                        temp.Add(p);
                    pdtos.Clear();
                    foreach (PunishmentDTO p in temp)
                        if (p.Name.Contains(name))
                            pdtos.Add(p);
                }
                if (classs != null)
                {
                    List<PunishmentDTO> temp = new List<PunishmentDTO>();
                    foreach (PunishmentDTO p in pdtos)
                        temp.Add(p);
                    pdtos.Clear();
                    foreach (PunishmentDTO p in temp)
                        if (p.Class.Contains(classs))
                            pdtos.Add(p);
                }
                if (type != null)
                {
                    List<PunishmentDTO> temp = new List<PunishmentDTO>();
                    foreach (PunishmentDTO p in pdtos)
                        temp.Add(p);
                    pdtos.Clear();
                    foreach (PunishmentDTO p in temp)
                        if (p.Type.Contains(type))
                            pdtos.Add(p);
                }
                return pdtos;
        }

        // PUT api/Disciplinary/5
        public HttpResponseMessage PutPunishmentDTO(int id, PunishmentDTO pdto)
        {
            if (id != pdto.Id)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            Punishment p = db.Punishments.Find(id);
            if (p == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            p.Type = pdto.Type;
            p.Date = DateTime.Parse(pdto.Date);
            p.Notes = pdto.Notes;
            p.Qualification = pdto.Qualification;
            db.Entry(p).State = EntityState.Modified;

            try {
                db.SaveChanges();
                // 更新 StudentInfo 中的 Qualification 信息
                IEnumerable<Punishment> ss = db.Punishments.Where(s => string.Equals(s.StudentInfoId, p.StudentInfoId));
                bool qualification = true;
                foreach (Punishment s in ss) {
                    qualification = qualification && s.Qualification;
                }
                StudentInfo stu = db.StudentInfoes.Find(p.StudentInfoId);
                stu.Qualification = qualification;
                db.Entry(stu).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex) {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Disciplinary
        public HttpResponseMessage PostPunishment(PunishmentDTO punishmentdto)
        {
            Punishment n = new Punishment();
            n.StudentInfoId = punishmentdto.SId;
            n.Student = db.StudentInfoes.Find(n.StudentInfoId);
            if (n.Student == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            n.Type = punishmentdto.Type;
            n.Date = DateTime.Parse(punishmentdto.Date);
            n.Notes = punishmentdto.Notes;
            n.Qualification = punishmentdto.Qualification;
            
            db.Punishments.Add(n);
            db.SaveChanges();

            // 更新 StudentInfo 中的 Qualification 信息
            IEnumerable<Punishment> ss = db.Punishments.Where(s => string.Equals(s.StudentInfoId, punishmentdto.SId));
            bool qualification = true;
            foreach (Punishment s in ss)
            {
                qualification = qualification && s.Qualification;
            }
            StudentInfo stu = db.StudentInfoes.Find(punishmentdto.SId);
            if (stu != null)
            {
                stu.Qualification = qualification;
                db.Entry(stu).State = EntityState.Modified;
                db.SaveChanges();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, punishmentdto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = n.Id }));
            return response;
        }

        // DELETE api/Disciplinary/5
        public HttpResponseMessage DeletePunishment(int id)
        {
            Punishment p = db.Punishments.Find(id);
            String sid = p.StudentInfoId;
            if (p == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Punishments.Remove(p);

            try {
                db.SaveChanges();

                // 更新 StudentInfo 中的 Qualification 信息
                IEnumerable<Punishment> ss = db.Punishments.Where(s => string.Equals(s.StudentInfoId, sid));
                bool qualification = true;
                foreach (Punishment s in ss)
                {
                    qualification = qualification && s.Qualification;
                }
                StudentInfo stu = db.StudentInfoes.Find(sid);
                if (stu != null)
                {
                    stu.Qualification = qualification;
                    db.Entry(stu).State = EntityState.Modified;
                    db.SaveChanges();
                }
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