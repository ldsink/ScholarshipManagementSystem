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
using System.Security.Cryptography;
using System.Text;
using ScholarshipManagementSystem.Models;
//NPOI
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;


namespace ScholarshipManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class ExportOwnScoringController : ApiController
    {
        private StudentContext db = new StudentContext();

        // GET api/ExportOwnScoring
        public String GetExportOwnScoringTs()
        {
            List<ScoringT> scoringts = db.ScoringTs.Where(
                (p) => string.Equals(p.ScoringStudentInfoId, User.Identity.Name)).ToList();
          
            String rtn_str = "ClassScoringForStudent_";
            String user_id = User.Identity.Name;
            rtn_str = rtn_str + user_id + "_";

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("班级打分表");
            IRow row = sheet1.CreateRow(0);
            row.CreateCell(0).SetCellValue("Id");
            row.CreateCell(1).SetCellValue("打分人学号");
            row.CreateCell(2).SetCellValue("被打分人学号");
            row.CreateCell(3).SetCellValue("姓名");
            row.CreateCell(4).SetCellValue("A");
            row.CreateCell(5).SetCellValue("B");
            row.CreateCell(6).SetCellValue("C");
            row.CreateCell(7).SetCellValue("D");
            row.CreateCell(8).SetCellValue("E");
            row.CreateCell(9).SetCellValue("F");
            row.CreateCell(10).SetCellValue("总计");
            row.CreateCell(11).SetCellValue("备注");
           
            int num = 1;
            for (int i = 0, tmp_count = scoringts.Count(); i < tmp_count; i++)
            {
                row = sheet1.CreateRow(num);
                row.CreateCell(0).SetCellValue(num.ToString());
                row.CreateCell(1).SetCellValue(scoringts[i].ScoringStudentInfoId);
                row.CreateCell(2).SetCellValue(scoringts[i].ScoredStudentInfoId);
                row.CreateCell(3).SetCellValue(scoringts[i].ScoredStudent.Name);
                row.CreateCell(4).SetCellValue(scoringts[i].A.ToString());
                row.CreateCell(5).SetCellValue(scoringts[i].B.ToString());
                row.CreateCell(6).SetCellValue(scoringts[i].C.ToString());
                row.CreateCell(7).SetCellValue(scoringts[i].D.ToString());
                row.CreateCell(8).SetCellValue(scoringts[i].E.ToString());
                row.CreateCell(9).SetCellValue(scoringts[i].F.ToString());
                row.CreateCell(10).SetCellValue(scoringts[i].Total.ToString());
                row.CreateCell(11).SetCellValue(scoringts[i].Notes);
                num++;
            }

            String filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Export/");
            rtn_str += GetMD5Hash(user_id).Substring(8, 8);
            rtn_str = rtn_str + ".xlsx";
            filePath += rtn_str;
            FileStream sw = File.Create(filePath);
            workbook.Write(sw);
            sw.Close();
            return rtn_str;
        }

        private string GetMD5Hash(String input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(input);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length - 1; i++)
            { 
                str += md5data[i].ToString("x").PadLeft(2, '0'); 
            } 
            return str;
        } 

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}