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
    public class ImportProjectBonusDataController : ApiController
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
                if (row.GetCell(0).ToString() != "编号")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第一列应为：编号";
                    return return_msg;
                }
                if (row.GetCell(1).ToString() != "项目类型")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第二列应为：项目类型";
                    return return_msg;
                }
                if (row.GetCell(2).ToString() != "项目设置")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第三列应为：项目设置";
                    return return_msg;
                }
                if (row.GetCell(3).ToString() != "项目内容")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第四列应为：项目内容";
                    return return_msg;
                }
                if (row.GetCell(4).ToString() != "分值")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第五列应为：分值";
                    return return_msg;
                }

                int i = 1;
                row = sheet.GetRow(i);
                while (row != null && row.GetCell(0) != null)
                {
                    int new_num = Int32.Parse(row.GetCell(0).ToString());
                    String new_type = row.GetCell(1).ToString();
                    String new_name = row.GetCell(2).ToString();
                    String new_content = row.GetCell(3).ToString();
                    float new_score = float.Parse(row.GetCell(4).ToString());
                    BonusProject s = new BonusProject();
                    s.Num = new_num;
                    s.Type = new_type;
                    s.Name = new_name;
                    s.Content = new_content;
                    s.Score = new_score;
                    db.BonusProjects.Add(s);
                    row = sheet.GetRow(++i);
                }
                db.SaveChanges();
                stream.Close();
                return_msg = "普通加分项目导入成功！";
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
