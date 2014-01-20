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
    public class ImportEnglishController : ApiController
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
                if (row.GetCell(1).ToString() != "CET4")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第二列应为：CET4";
                    return return_msg;
                }
                if (row.GetCell(2).ToString() != "CET6")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第三列应为：CET6";
                    return return_msg;
                }
                if (row.GetCell(3).ToString() != "TOFEL")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第四列应为：TOFEL";
                    return return_msg;
                }

                int i = 1;
                row = sheet.GetRow(i);
                while (row != null && row.GetCell(0) != null)
                {
                    String new_user = row.GetCell(0).ToString();
                    Int32 new_cet4 = Int32.Parse(row.GetCell(1).ToString());
                    Int32 new_cet6 = Int32.Parse(row.GetCell(2).ToString());
                    Int32 new_tofel = Int32.Parse(row.GetCell(3).ToString());
                    English s = db.Englishs.Find(new_user);
                    if (s == null)
                    {
                        s = new English();
                        s.Id = new_user;
                        s.CET4 = new_cet4;
                        s.CET6 = new_cet6;
                        s.TOFEL = new_tofel;
                        db.Englishs.Add(s);
                        db.SaveChanges();
                    }
                    else
                    {
                        s.CET4 = new_cet4;
                        s.CET6 = new_cet6;
                        s.TOFEL = new_tofel;
                        db.Entry(s).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    row = sheet.GetRow(++i);
                }

                stream.Close();
                return_msg = "学生英语成绩导入成功！";
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
