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
// NPOI
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;


namespace ScholarshipManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class VerifyController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/Verify
        public List<BonusFVD> GetBonusFVDs()
        {
            List<BonusT> bonusts = db.BonusTs.Include(b => b.Student).ToList();
            List<BonusFVD> bonusfvds = new List<BonusFVD>();
            for (int i = 0, tmp_count = bonusts.Count(); i < tmp_count; i++)
            {
                BonusFVD bfvd = new BonusFVD();
                bfvd.Id = bonusts[i].Id;
                bfvd.Bonustype = bonusts[i].Bonustype;
                bfvd.StudentId = bonusts[i].StudentInfoId;
                StudentInfo si = new StudentInfo();
                si = db.StudentInfoes.Find(bonusts[i].StudentInfoId);
                bfvd.StudentName = si.Name;
                bfvd.StudentClassId = si.ClassId;
                if (bonusts[i].Bonustype == BonusType.ProjectBonus)
                {
                    bfvd.Date = bonusts[i].Date.ToShortDateString();
                    bfvd.State = bonusts[i].Status;
                    bfvd.Notes = bonusts[i].Notes;
                    BonusProject bp = new BonusProject();
                    bp = db.BonusProjects.Find(bonusts[i].DetailID);
                    if (bp != null)
                    {
                        bfvd.Type = bp.Type;
                        bfvd.Num = bp.Num;
                        bfvd.Content = bp.Content;
                        bfvd.HName = bp.Name;
                        bfvd.Score = bp.Score;
                    }
                }
                else if (bonusts[i].Bonustype == BonusType.PaperBonus)
                {
                    bfvd.Date = bonusts[i].Date.ToShortDateString();
                    bfvd.State = bonusts[i].Status;
                    bfvd.Notes = bonusts[i].Notes;
                    BonusPaper bp = new BonusPaper();
                    bp = db.BonusPapers.Find(bonusts[i].DetailID);
                    if (bp != null)
                    {
                        bfvd.Type = bp.Type;
                        bfvd.Num = bp.Num;
                        bfvd.Content = bp.Content;
                        bfvd.Score = bp.Score;
                    }
                    int temp_id = bonusts[i].Id;
                    BonusPaperDetail bpd = db.BonusPaperDetails.SingleOrDefault((p) => (p.BelongedID == temp_id));
                    if (bpd != null)
                    {
                        bfvd.Level = bpd.Level;
                        bfvd.Way = bpd.Way;
                        bfvd.HName = bpd.HName;
                        bfvd.LName = bpd.LName;
                        bfvd.Ranking = bpd.Ranking;
                    }
                }
                else if (bonusts[i].Bonustype == BonusType.CompetitionBonus)
                {
                    bfvd.Date = bonusts[i].Date.ToShortDateString();
                    bfvd.State = bonusts[i].Status;
                    bfvd.Notes = bonusts[i].Notes;
                    BonusCompetition bc = new BonusCompetition();
                    bc = db.BonusCompetitions.Find(bonusts[i].DetailID);
                    if (bc != null)
                    {
                        bfvd.Type = bc.Type;
                        bfvd.Num = bc.Num;
                        bfvd.Content = bc.Content;
                        bfvd.Score = bc.Score;
                    }
                    int temp_id = bonusts[i].Id;
                    BonusCompetitionDetail bcd = db.BonusCompetitionDetails.SingleOrDefault((p) => (p.BelongedID == temp_id));
                    if (bcd != null)
                    {
                        bfvd.Level = bcd.Level;
                        bfvd.HName = bcd.HName;
                        bfvd.LName = bcd.LName;
                        bfvd.Ranking = bcd.Ranking;
                    }
                }
                bonusfvds.Add(bfvd);
            }
            
            return bonusfvds;
         }

        // GET api/Verify/?name=&classid=&id=&num=&type=
        public List<BonusFVD> GetBonusTs(string name, string classid, string id, string num, string type)
        {
            List<BonusFVD> bonusfvds = GetBonusFVDs();
            if (name != null)
            {
                List<BonusFVD> temp = new List<BonusFVD>();
                foreach (BonusFVD b in bonusfvds)
                    temp.Add(b);
                bonusfvds.Clear();
                foreach (BonusFVD p in temp)
                    if (p.StudentName.Contains(name))
                        bonusfvds.Add(p);
            }
            if (classid != null)
            {
                List<BonusFVD> temp = new List<BonusFVD>();
                foreach (BonusFVD b in bonusfvds)
                    temp.Add(b);
                bonusfvds.Clear();
                foreach (BonusFVD p in temp)
                    if (p.StudentClassId.Contains(classid))
                        bonusfvds.Add(p);
            }
            if (id != null)
            {
                List<BonusFVD> temp = new List<BonusFVD>();
                foreach (BonusFVD b in bonusfvds)
                    temp.Add(b);
                bonusfvds.Clear();
                foreach (BonusFVD p in temp)
                    if (p.StudentId.Contains(id))
                        bonusfvds.Add(p);
            }
            if (num != null)
            {
                List<BonusFVD> temp = new List<BonusFVD>();
                foreach (BonusFVD b in bonusfvds)
                    temp.Add(b);
                bonusfvds.Clear();
                foreach (BonusFVD p in temp)
                    if (p.Num.ToString().Contains(num))
                        bonusfvds.Add(p);
            }
            if (type != null)
            {
                List<BonusFVD> temp = new List<BonusFVD>();
                foreach (BonusFVD b in bonusfvds)
                    temp.Add(b);
                bonusfvds.Clear();
                foreach (BonusFVD p in temp)
                    if (p.State.Contains(type))
                        bonusfvds.Add(p);
            }
            return bonusfvds;
        }

        // GET api/Verify/?way=&name=&classid=&id=&num=&type=
        public String GetExportBonusTs(string way, string name, string classid, string id, string num, string type)
        {
            try
            {
                if (way == "Export")
                {
                    List<BonusFVD> bfvds = GetBonusTs(name, classid, id, num, type);
                    IWorkbook workbook = new XSSFWorkbook();

                    ISheet sheet1 = workbook.CreateSheet("普通加分项");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("学号");
                    row.CreateCell(1).SetCellValue("姓名");
                    row.CreateCell(2).SetCellValue("班号");
                    row.CreateCell(3).SetCellValue("项目编号");
                    row.CreateCell(4).SetCellValue("项目类型");
                    row.CreateCell(5).SetCellValue("项目内容");
                    row.CreateCell(6).SetCellValue("分值");
                    row.CreateCell(7).SetCellValue("审核状态");
                    row.CreateCell(8).SetCellValue("项目设置");
                    row.CreateCell(9).SetCellValue("获奖时间");
                    row.CreateCell(10).SetCellValue("备注信息");

                    ISheet sheet2 = workbook.CreateSheet("竞赛加分项");
                    row = sheet2.CreateRow(0);
                    row.CreateCell(0).SetCellValue("学号");
                    row.CreateCell(1).SetCellValue("姓名");
                    row.CreateCell(2).SetCellValue("班号");
                    row.CreateCell(3).SetCellValue("项目编号");
                    row.CreateCell(4).SetCellValue("项目类型");
                    row.CreateCell(5).SetCellValue("项目内容");
                    row.CreateCell(6).SetCellValue("分值");
                    row.CreateCell(7).SetCellValue("审核状态");
                    row.CreateCell(8).SetCellValue("竞赛等级");
                    row.CreateCell(9).SetCellValue("竞赛名称");
                    row.CreateCell(10).SetCellValue("参赛项目");
                    row.CreateCell(11).SetCellValue("获奖等级");
                    row.CreateCell(12).SetCellValue("获奖时间");
                    row.CreateCell(13).SetCellValue("备注信息");

                    ISheet sheet3 = workbook.CreateSheet("论文加分项");
                    row = sheet3.CreateRow(0);
                    row.CreateCell(0).SetCellValue("学号");
                    row.CreateCell(1).SetCellValue("姓名");
                    row.CreateCell(2).SetCellValue("班号");
                    row.CreateCell(3).SetCellValue("项目编号");
                    row.CreateCell(4).SetCellValue("项目类型");
                    row.CreateCell(5).SetCellValue("项目内容");
                    row.CreateCell(6).SetCellValue("分值");
                    row.CreateCell(7).SetCellValue("审核状态");
                    row.CreateCell(8).SetCellValue("会议等级");
                    row.CreateCell(9).SetCellValue("会议方向");
                    row.CreateCell(10).SetCellValue("会议名称");
                    row.CreateCell(11).SetCellValue("论文名称");
                    row.CreateCell(12).SetCellValue("作者顺序");
                    row.CreateCell(13).SetCellValue("获奖时间");
                    row.CreateCell(14).SetCellValue("备注信息");

                    int numpr = 1;
                    int numpa = 1;
                    int numco = 1;
                    foreach (BonusFVD bfvd in bfvds)
                    {
                        if (bfvd.Bonustype == BonusType.ProjectBonus)
                        {
                            row = sheet1.CreateRow(numpr);
                            row.CreateCell(0).SetCellValue(bfvd.StudentId.ToString());
                            row.CreateCell(1).SetCellValue(bfvd.StudentName.ToString());
                            row.CreateCell(2).SetCellValue(bfvd.StudentClassId.ToString());
                            row.CreateCell(3).SetCellValue(bfvd.Num.ToString());
                            row.CreateCell(4).SetCellValue(bfvd.Type.ToString());
                            row.CreateCell(5).SetCellValue(bfvd.Content.ToString());
                            row.CreateCell(6).SetCellValue(bfvd.Score.ToString());
                            row.CreateCell(7).SetCellValue(bfvd.State.ToString());

                            if (bfvd.HName != null)
                            {
                                row.CreateCell(8).SetCellValue(bfvd.HName.ToString());
                            }
                            if (bfvd.Date != null)
                            {
                                row.CreateCell(9).SetCellValue(bfvd.Date.ToString());
                            }
                            if (bfvd.Notes != null)
                            {
                                row.CreateCell(10).SetCellValue(bfvd.Notes.ToString());
                            }
                            numpr++;
                        }
                        else if (bfvd.Bonustype == BonusType.CompetitionBonus)
                        {
                            row = sheet2.CreateRow(numco);
                            row.CreateCell(0).SetCellValue(bfvd.StudentId.ToString());
                            row.CreateCell(1).SetCellValue(bfvd.StudentName.ToString());
                            row.CreateCell(2).SetCellValue(bfvd.StudentClassId.ToString());
                            row.CreateCell(3).SetCellValue(bfvd.Num.ToString());
                            row.CreateCell(4).SetCellValue(bfvd.Type.ToString());
                            row.CreateCell(5).SetCellValue(bfvd.Content.ToString());
                            row.CreateCell(6).SetCellValue(bfvd.Score.ToString());
                            row.CreateCell(7).SetCellValue(bfvd.State.ToString());

                            if (bfvd.Level != null)
                            {
                                row.CreateCell(8).SetCellValue(bfvd.Level.ToString());
                            }
                            if (bfvd.HName != null)
                            {
                                row.CreateCell(9).SetCellValue(bfvd.HName.ToString());
                            }
                            if (bfvd.LName != null)
                            {
                                row.CreateCell(10).SetCellValue(bfvd.LName.ToString());
                            }
                            if (bfvd.Ranking != null)
                            {
                                row.CreateCell(11).SetCellValue(bfvd.Ranking.ToString());
                            }
                            if (bfvd.Date != null)
                            {
                                row.CreateCell(12).SetCellValue(bfvd.Date.ToString());
                            }
                            if (bfvd.Notes != null)
                            {
                                row.CreateCell(13).SetCellValue(bfvd.Notes.ToString());
                            }
                            numco++;
                        }
                        else if (bfvd.Bonustype == BonusType.PaperBonus)
                        {
                            row = sheet3.CreateRow(numpa);
                            row.CreateCell(0).SetCellValue(bfvd.StudentId.ToString());
                            row.CreateCell(1).SetCellValue(bfvd.StudentName.ToString());
                            row.CreateCell(2).SetCellValue(bfvd.StudentClassId.ToString());
                            row.CreateCell(3).SetCellValue(bfvd.Num.ToString());
                            row.CreateCell(4).SetCellValue(bfvd.Type.ToString());
                            row.CreateCell(5).SetCellValue(bfvd.Content.ToString());
                            row.CreateCell(6).SetCellValue(bfvd.Score.ToString());
                            row.CreateCell(7).SetCellValue(bfvd.State.ToString());

                            if (bfvd.Level != null)
                            {
                                row.CreateCell(8).SetCellValue(bfvd.Level.ToString());
                            }
                            if (bfvd.Way != null)
                            {
                                row.CreateCell(9).SetCellValue(bfvd.Way.ToString());
                            }
                            if (bfvd.HName != null)
                            {
                                row.CreateCell(10).SetCellValue(bfvd.HName.ToString());
                            }
                            if (bfvd.LName != null)
                            {
                                row.CreateCell(11).SetCellValue(bfvd.LName.ToString());
                            }
                            if (bfvd.Ranking != null)
                            {
                                row.CreateCell(12).SetCellValue(bfvd.Ranking.ToString());
                            }
                            if (bfvd.Date != null)
                            {
                                row.CreateCell(13).SetCellValue(bfvd.Date.ToString());
                            }
                            if (bfvd.Notes != null)
                            {
                                row.CreateCell(14).SetCellValue(bfvd.Notes.ToString());
                            }
                            numpa++;
                        }
                    }
                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "SpecificBonusTable.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else
                {
                    return "Wrong Method";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Get api/Verify/?id=&state=
        public bool GetBonusTChanged(int id, string state)
        {
            BonusT bt = db.BonusTs.Find(id);
            if (bt == null)
                return false;
            else
            {
                if (state == "true")
                    bt.Status = "通过";
                else if (state == "false")
                    bt.Status = "不通过";
                else
                    bt.Status = "未审核";
            }
            db.SaveChanges();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}