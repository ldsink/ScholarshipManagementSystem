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
    public class ExportForStudentInfoController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/ExportForStudentInfo/?Name=&classId=&studentId=
        public List<StudentInfo> GetStudentInfoes(String Name, String classId, String studentId)
        {
            if (Name != null)
            {
                return db.StudentInfoes.Where(s => (s.Name.Contains(Name))).ToList();
            }
            else if (classId != null)
            {
                return db.StudentInfoes.Where(s => (s.ClassId.Contains(classId))).ToList();
            }
            else if (studentId != null)
            {
                return db.StudentInfoes.Where(s => (s.Id.Contains(studentId))).ToList();
            }
            else
            {
                return db.StudentInfoes.ToList();
            }
        }

        // GET api/ExportForStudentInfo/?method=&Name=&classId=&studentId=
        public String GetExportScoringTs(String method, String Name, String classId, String studentId)
        {
            try
            {
                if (method == "export")
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

                    List<StudentInfo> sis = GetStudentInfoes(Name, classId, studentId);
                    StudentInfo si = new StudentInfo();
                    int i = 1, j = sis.Count(), k = 0;
                    for (; k < j; k++, i++)
                    {
                        row = sheet1.CreateRow(i);
                        si = sis[k];
                        row.CreateCell(0).SetCellValue(si.Id.ToString());
                        row.CreateCell(1).SetCellValue(si.Name.ToString());
                        row.CreateCell(2).SetCellValue(si.ClassId.ToString());
                        row.CreateCell(3).SetCellValue(si.ApplyScholarship.ToString());
                        row.CreateCell(4).SetCellValue(si.ApplyGrant.ToString());
                        row.CreateCell(5).SetCellValue(si.SubmitScoring.ToString());
                        row.CreateCell(6).SetCellValue(si.Qualification.ToString());
                    }

                    String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
                    filePath += "StudentInfoSimple.xlsx";
                    FileStream sw = File.Create(filePath);
                    workbook.Write(sw);
                    sw.Close();
                    return "Success";
                }
                else
                {
                    return "Error : Not Found.";
                }
            }
            catch (Exception ex)
            {
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