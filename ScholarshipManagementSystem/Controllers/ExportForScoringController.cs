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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ScholarshipManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ExportForScoringController : ApiController
    {
        private StudentContext db = new StudentContext();
        
        // GET api/ExportForScoring/?id1=&id2=
        public IEnumerable<ScoringT> GetScoringTs(String id1, String id2)
        {
            if (id1 != null && id2 == null)
            {
                IEnumerable<ScoringT> scoringts = db.ScoringTs.Where(p => (String.Equals(p.ScoringStudentInfoId, id1)));
                return scoringts;
            }
            if (id2 != null && id1 == null)
            {
                IEnumerable<ScoringT> scoringts = db.ScoringTs.Where(p => (String.Equals(p.ScoredStudentInfoId, id2)));
                return scoringts;
            }
            else if (id2 != null && id1 != null)
            {
                IEnumerable<ScoringT> scoringts = db.ScoringTs.Where(p => (String.Equals(p.ScoringStudentInfoId, id1) && String.Equals(p.ScoredStudentInfoId, id2)));
                return scoringts;
            }
            else {
                List<ScoringT> scoringts = new List<ScoringT>();
                return scoringts;
            }
        }

        // GET api/Verify/?way=&name=&classid=&id=&num=&type=
        public String GetExportScoringTs(String method, String id1, String id2)
        {
            try
            {
                if (method == "export")
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("Sheet1");
                    IRow row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("Id");
                    row.CreateCell(1).SetCellValue("打分人学号");
                    row.CreateCell(2).SetCellValue("打分人姓名");
                    row.CreateCell(3).SetCellValue("被打分人学号");
                    row.CreateCell(4).SetCellValue("被打分人姓名");
                    row.CreateCell(5).SetCellValue("A");
                    row.CreateCell(6).SetCellValue("B");
                    row.CreateCell(7).SetCellValue("C");
                    row.CreateCell(8).SetCellValue("D");
                    row.CreateCell(9).SetCellValue("E");
                    row.CreateCell(10).SetCellValue("F");
                    row.CreateCell(11).SetCellValue("总计");
                    row.CreateCell(12).SetCellValue("备注");

                    List<ScoringT> scoringts = GetScoringTs(id1, id2).ToList();
                    ScoringT st = new ScoringT();
                    int i = 1, j = scoringts.Count(), k = 0;
                    for (; k < j; k ++, i ++)
                    {
                        row = sheet1.CreateRow(i);
                        st = scoringts[k];
                        row.CreateCell(0).SetCellValue(st.Id);
                        row.CreateCell(1).SetCellValue(st.ScoringStudentInfoId);
                        row.CreateCell(2).SetCellValue(st.ScoringStudent.Name);
                        row.CreateCell(3).SetCellValue(st.ScoredStudentInfoId);
                        row.CreateCell(4).SetCellValue(st.ScoredStudent.Name);
                        row.CreateCell(5).SetCellValue(st.A.ToString());
                        row.CreateCell(6).SetCellValue(st.B.ToString());
                        row.CreateCell(7).SetCellValue(st.C.ToString());
                        row.CreateCell(8).SetCellValue(st.D.ToString());
                        row.CreateCell(9).SetCellValue(st.E.ToString());
                        row.CreateCell(10).SetCellValue(st.F.ToString());
                        row.CreateCell(11).SetCellValue(st.Total.ToString());
                        row.CreateCell(12).SetCellValue(st.Notes);
                    }

                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "ClassScoring.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();

                    return "Success!";
                }
                else {
                    return "Error : Not Found.";
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