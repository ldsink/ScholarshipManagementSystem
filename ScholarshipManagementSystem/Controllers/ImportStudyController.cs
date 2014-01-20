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
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Security;
using WebMatrix.WebData;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ScholarshipManagementSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ImportStudyController : ApiController
    {
        private StudentContext db = new StudentContext();

        public String Postgetfile()
        {
            try
            {
                String return_msg = "";

                System.Web.HttpPostedFile excel_file = System.Web.HttpContext.Current.Request.Files["file1"];
                String file_name = excel_file.FileName;
                file_name = Path.GetFileName(file_name);
                String path = System.Web.HttpContext.Current.Server.MapPath("~/App_Import/");
                file_name = path + file_name;
                System.Web.HttpContext.Current.Request.Files["file1"].SaveAs(file_name);

                FileStream stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                IWorkbook wb = new XSSFWorkbook(stream);
                ISheet sheet = wb.GetSheetAt(0);
                IRow row = sheet.GetRow(0);
                if (row.GetCell(0).ToString() != "学号")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第一列应为：学号";
                    return return_msg;
                }
                if (row.GetCell(1).ToString() != "成绩")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第二列应为：第一学期成绩";
                    return return_msg;
                }
                

                int i = 1;
                row = sheet.GetRow(i);
                while (row != null && row.GetCell(0) != null)
                {

                    String new_user = row.GetCell(0).ToString();
                    float new_score = float.Parse(row.GetCell(1).ToString());
                    Study s = db.Studys.Find(new_user);
                    if (s == null)
                    {
                        s = new Study();
                        s.Id = new_user;
                        s.Score = new_score;
                        db.Studys.Add(s);
                        db.SaveChanges();
                    }
                    else
                    {
                        s.Score = new_score;
                        db.Entry(s).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    row = sheet.GetRow(++i);
                }

                stream.Close();
                return_msg = "学生成绩导入成功！";
                return return_msg;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}