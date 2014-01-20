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
    public class ImportStudentProjectBonusController : ApiController
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
                if (row.GetCell(1).ToString() != "加分项目编号")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第二列应为：加分项目编号";
                    return return_msg;
                }
                if (row.GetCell(2).ToString() != "日期")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第三列应为：日期";
                    return return_msg;
                }
                if (row.GetCell(3).ToString() != "备注")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第三列应为：备注";
                    return return_msg;
                }

                int i = 1;
                row = sheet.GetRow(i);
                BonusController bc = new BonusController();
                while (row != null && row.GetCell(0) != null)
                {
                    String stu_id = row.GetCell(0).ToString();
                    BonusT bt = new BonusT();
                    bt.StudentInfoId = stu_id;
                    bt.Student = db.StudentInfoes.Find(stu_id);
                    if (bt.Student == null)
                    {
                        return_msg += " Error: Student ID ";
                        return_msg += stu_id + " don't exists.";
                        row = sheet.GetRow(++i);
                        continue;
                    }
                    if (row.GetCell(1) == null)
                    {
                        return_msg += " Error: Bonus ID is empty.";
                        row = sheet.GetRow(++i);
                        continue;
                    }
                    Int32 bonus_num = Int32.Parse(row.GetCell(1).ToString());         
                    bt.Bonustype = bc.CheckBonusType(bonus_num); //BonusType.ProjectBonus;
                    if (bt.Bonustype != BonusType.ProjectBonus)
                    {
                        return_msg += " Error: Bonus ID ";
                        return_msg += bonus_num + " don't exists or belong to normal bonus.";
                        row = sheet.GetRow(++i);
                        continue;
                    }
                    bt.DetailID = bc.CheckID(bonus_num, BonusType.ProjectBonus);
                    if (row.GetCell(2) == null)
                    {
                        return_msg += " Error: Date is empty.";
                        row = sheet.GetRow(++i);
                        continue;
                    }
                    bt.Date = DateTime.Parse(row.GetCell(2).ToString());
                    if (row.GetCell(3) == null)
                        bt.Notes = null;
                    else
                        bt.Notes = row.GetCell(3).ToString();
                    bt.Status = "通过";
                    db.BonusTs.Add(bt);
                    row = sheet.GetRow(++i);
                }
                db.SaveChanges();
                stream.Close();
                return_msg += "学生普通加分项导入成功！";
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
