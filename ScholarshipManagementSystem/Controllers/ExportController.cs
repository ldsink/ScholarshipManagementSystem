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
    public class ExportController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/Export/?type=
        public String GetExportExcel(String type)
        {
            try
            {
                if (type == "StudentInfo")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("学生信息");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("学号");
                    row.CreateCell(1).SetCellValue("姓名");
                    row.CreateCell(2).SetCellValue("班级");
                    row.CreateCell(3).SetCellValue("奖学金申请");
                    row.CreateCell(4).SetCellValue("助学金申请");
                    row.CreateCell(5).SetCellValue("班级打分提交");
                    row.CreateCell(6).SetCellValue("评选资格");

                    IEnumerable<StudentInfo> studentinfoes = db.StudentInfoes.AsEnumerable();
                    int num = 1;
                    foreach (StudentInfo si in studentinfoes)
                    {
                        row = sheet1.CreateRow(num);
                        row.CreateCell(0).SetCellValue(si.Id.ToString());
                        row.CreateCell(1).SetCellValue(si.Name.ToString());
                        row.CreateCell(2).SetCellValue(si.ClassId.ToString());
                        row.CreateCell(3).SetCellValue(si.ApplyScholarship.ToString());
                        row.CreateCell(4).SetCellValue(si.ApplyGrant.ToString());
                        row.CreateCell(5).SetCellValue(si.SubmitScoring.ToString());
                        row.CreateCell(6).SetCellValue(si.Qualification.ToString());
                        num++;
                    }

                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "StudentInfo.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else if (type == "ClassScoring")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("班级打分原始数据表");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("Id");
                    row.CreateCell(1).SetCellValue("评分学生学号");
                    row.CreateCell(2).SetCellValue("被评分学生学号");
                    row.CreateCell(3).SetCellValue("A");
                    row.CreateCell(4).SetCellValue("B");
                    row.CreateCell(5).SetCellValue("C");
                    row.CreateCell(6).SetCellValue("D");
                    row.CreateCell(7).SetCellValue("E");
                    row.CreateCell(8).SetCellValue("F");
                    row.CreateCell(9).SetCellValue("总计");
                    row.CreateCell(10).SetCellValue("备注");

                    IEnumerable<ScoringT> scoringts = db.ScoringTs.AsEnumerable();

                    int num = 1;
                    foreach (ScoringT si in scoringts)
                    {
                        row = sheet1.CreateRow(num);
                        row.CreateCell(0).SetCellValue(si.Id.ToString());
                        row.CreateCell(1).SetCellValue(si.ScoringStudentInfoId);
                        row.CreateCell(2).SetCellValue(si.ScoredStudentInfoId);
                        row.CreateCell(3).SetCellValue(si.A.ToString());
                        row.CreateCell(4).SetCellValue(si.B.ToString());
                        row.CreateCell(5).SetCellValue(si.C.ToString());
                        row.CreateCell(6).SetCellValue(si.D.ToString());
                        row.CreateCell(7).SetCellValue(si.E.ToString());
                        row.CreateCell(8).SetCellValue(si.F.ToString());
                        row.CreateCell(9).SetCellValue(si.Total.ToString());
                        row.CreateCell(10).SetCellValue(si.Notes);
                        num++;
                    }

                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "ClassScoringAll.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else if (type == "Disciplinary")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("违纪信息");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("Id");
                    row.CreateCell(1).SetCellValue("学号");
                    row.CreateCell(2).SetCellValue("姓名");
                    row.CreateCell(3).SetCellValue("班级");
                    row.CreateCell(4).SetCellValue("违纪类型");
                    row.CreateCell(5).SetCellValue("时间");
                    row.CreateCell(6).SetCellValue("描述");
                    row.CreateCell(7).SetCellValue("评选资格");

                    List<Punishment> ps = db.Punishments.ToList();

                    int num = 1;
                    for (int i = 0, tmp_count = ps.Count(); i < tmp_count; i++)
                    {
                        row = sheet1.CreateRow(num);
                        row.CreateCell(0).SetCellValue(ps[i].Id.ToString());
                        row.CreateCell(1).SetCellValue(ps[i].StudentInfoId);
                        row.CreateCell(2).SetCellValue(ps[i].Student.Name);
                        row.CreateCell(3).SetCellValue(ps[i].Student.ClassId);
                        row.CreateCell(4).SetCellValue(ps[i].Type);
                        row.CreateCell(5).SetCellValue(ps[i].Date);
                        row.CreateCell(6).SetCellValue(ps[i].Notes);
                        row.CreateCell(7).SetCellValue(ps[i].Qualification.ToString());
                        num++;
                    }
                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "Disciplinary.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else if (type == "BigTable")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("成绩大表");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("姓名");
                    row.CreateCell(1).SetCellValue("班级");
                    row.CreateCell(2).SetCellValue("学号");
                    row.CreateCell(3).SetCellValue("学积分");
                    row.CreateCell(4).SetCellValue("学积分排名");
                    row.CreateCell(5).SetCellValue("学积分比例");
                    row.CreateCell(6).SetCellValue("班级得分");
                    row.CreateCell(7).SetCellValue("班级打分表提交");
                    row.CreateCell(8).SetCellValue("宿舍得分");
                    row.CreateCell(9).SetCellValue("加分");
                    row.CreateCell(10).SetCellValue("CET4");
                    row.CreateCell(11).SetCellValue("CET6");
                    row.CreateCell(12).SetCellValue("TOFEL");
                    row.CreateCell(13).SetCellValue("奖（助)学金资格");
                    row.CreateCell(14).SetCellValue("总分数");
                    row.CreateCell(15).SetCellValue("总排名");
                    row.CreateCell(16).SetCellValue("总排名比例");

                    BigTableController tempController = new BigTableController();
                    IEnumerable<BigTableDTO> total_scores = tempController.GetBigTableDTO();

                    int num = 1;
                    foreach (BigTableDTO ss in total_scores)
                    {
                        row = sheet1.CreateRow(num);
                        row.CreateCell(0).SetCellValue(ss.Name);
                        row.CreateCell(1).SetCellValue(ss.ClassId);
                        row.CreateCell(2).SetCellValue(ss.NumId);
                        row.CreateCell(3).SetCellValue(ss.Score.ToString());
                        row.CreateCell(4).SetCellValue(ss.Ranking.ToString());
                        row.CreateCell(5).SetCellValue(ss.Percent.ToString());
                        row.CreateCell(6).SetCellValue(ss.Scoring.ToString());
                        row.CreateCell(7).SetCellValue(ss.SubmitScoring.ToString());
                        row.CreateCell(8).SetCellValue(ss.Dormitoty.ToString());
                        row.CreateCell(9).SetCellValue(ss.Bonus.ToString());
                        row.CreateCell(10).SetCellValue(ss.CET4.ToString());
                        row.CreateCell(11).SetCellValue(ss.CET6.ToString());
                        row.CreateCell(12).SetCellValue(ss.TOFEL.ToString());
                        row.CreateCell(13).SetCellValue(ss.Qualification.ToString());
                        row.CreateCell(14).SetCellValue(ss.TotalScore.ToString());
                        row.CreateCell(15).SetCellValue(ss.TotalRanking.ToString());
                        row.CreateCell(16).SetCellValue(ss.TotalPercent.ToString());
                        num++;
                    }
                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "BigTable.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else if (type == "BonusTable")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    
                    VerifyController tempController = new VerifyController();
                    IEnumerable<BonusFVD> bfvds = tempController.GetBonusFVDs();

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
                    filePath += "BonusTable.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else if (type == "ClassScoringForAnnounce")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("公示班级打分表");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("学号");
                    row.CreateCell(1).SetCellValue("班级");
                    row.CreateCell(2).SetCellValue("姓名");
                    row.CreateCell(3).SetCellValue("A");
                    row.CreateCell(4).SetCellValue("B");
                    row.CreateCell(5).SetCellValue("C");
                    row.CreateCell(6).SetCellValue("D");
                    row.CreateCell(7).SetCellValue("E");
                    row.CreateCell(8).SetCellValue("F");
                    row.CreateCell(9).SetCellValue("平均分");
                    row.CreateCell(10).SetCellValue("调整后与年级平均偏差");

                    List<ScoringT> scoringts = db.ScoringTs.ToList();
                    IEnumerable<StudentInfo> stuinfoes = db.StudentInfoes.AsEnumerable();
                    BigTableController tempController = new BigTableController();

                    int excel_num = 1;
                    foreach (StudentInfo si in stuinfoes)
                    { 
                        if (si.Active == true)
                        {
                            double tmp_sum_A = 0;
                            double tmp_sum_B = 0;
                            double tmp_sum_C = 0;
                            double tmp_sum_D = 0;
                            double tmp_sum_E = 0;
                            double tmp_sum_F = 0;
                            int tmp_sum_num = 0;
                            double avg_sum_A = 0;
                            double avg_sum_B = 0;
                            double avg_sum_C = 0;
                            double avg_sum_D = 0;
                            double avg_sum_E = 0;
                            double avg_sum_F = 0;
                            double avg_sum_Total = 0;
                            for (int i = 0, j = scoringts.Count(); i < j; i++)
                            {
                                if (scoringts[i].ScoredStudentInfoId == si.Id)
                                {
                                    tmp_sum_A += scoringts[i].A;
                                    tmp_sum_B += scoringts[i].B;
                                    tmp_sum_C += scoringts[i].C;
                                    tmp_sum_D += scoringts[i].D;
                                    tmp_sum_E += scoringts[i].E;
                                    tmp_sum_F += scoringts[i].F;
                                    tmp_sum_num++;
                                }
                            }
                            if (tmp_sum_num != 0)
                            {
                                avg_sum_A = tmp_sum_A / (double)tmp_sum_num;
                                avg_sum_B = tmp_sum_B / (double)tmp_sum_num;
                                avg_sum_C = tmp_sum_C / (double)tmp_sum_num;
                                avg_sum_D = tmp_sum_D / (double)tmp_sum_num;
                                avg_sum_E = tmp_sum_E / (double)tmp_sum_num;
                                avg_sum_F = tmp_sum_F / (double)tmp_sum_num;
                                avg_sum_Total = avg_sum_A + avg_sum_B + avg_sum_C + avg_sum_D
                                    + avg_sum_E + avg_sum_F;
                            }
                            row = sheet1.CreateRow(excel_num);
                            row.CreateCell(0).SetCellValue(si.Id.ToString());
                            row.CreateCell(1).SetCellValue(si.ClassId.ToString());
                            row.CreateCell(2).SetCellValue(si.Name.ToString());
                            row.CreateCell(3).SetCellValue(avg_sum_A.ToString());
                            row.CreateCell(4).SetCellValue(avg_sum_B.ToString());
                            row.CreateCell(5).SetCellValue(avg_sum_C.ToString());
                            row.CreateCell(6).SetCellValue(avg_sum_D.ToString());
                            row.CreateCell(7).SetCellValue(avg_sum_E.ToString());
                            row.CreateCell(8).SetCellValue(avg_sum_F.ToString());
                            row.CreateCell(9).SetCellValue(avg_sum_Total.ToString());
                            row.CreateCell(10).SetCellValue(tempController.CalculateScoring(si.Id, si.ClassId).ToString());
                            excel_num++;
                        }
                    }

                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "ClassScoringForAnnounce.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else // 未找到合适的选项
                {
                    return "Not Found";
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}