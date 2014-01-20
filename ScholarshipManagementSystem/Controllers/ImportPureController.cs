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
    public class ImportPureController : ApiController
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
                if (row.GetCell(0).ToString() != "姓名")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第一列应为：姓名";
                    return return_msg;
                }
                if (row.GetCell(1).ToString() != "学号")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第二列应为：学号";
                    return return_msg;
                }
                if (row.GetCell(2).ToString() != "班级")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第三列应为：班级";
                    return return_msg;
                }
                if (row.GetCell(3).ToString() != "密码")
                {
                    stream.Close();
                    return_msg = "表格格式不正确！第一行第四列应为：密码";
                    return return_msg;
                }

                //注册新的用户,初始化StudentInfo，如果用户已存在，则重置为输入的密码
                int i = 1;
                row = sheet.GetRow(i);
                List<StudentInfo> newStudents = new List<StudentInfo>();
                while (row != null && row.GetCell(1) != null)
                {
                    String new_user = row.GetCell(1).ToString();
                    String new_pass = row.GetCell(3).ToString();
                    if (!WebSecurity.UserExists(new_user))
                    {
                        // Create new student info
                        StudentInfo new_si = new StudentInfo();
                        new_si.Id = new_user;
                        new_si.Name = row.GetCell(0).ToString();
                        new_si.ClassId = row.GetCell(2).ToString();
                        new_si.ApplyGrant = false;
                        new_si.ApplyScholarship = false;
                        new_si.Qualification = true;
                        new_si.SubmitScoring = false;
                        new_si.Active = true;
                        newStudents.Add(new_si);
                        // Create login account
                        WebSecurity.CreateUserAndAccount(new_user, new_pass);
                        Roles.AddUserToRole(new_user, "Student");
                    }
                    else
                    {
                        //覆盖密码
                        Roles.RemoveUserFromRole(new_user, "Student");
                        Membership.DeleteUser(new_user, true);
                        WebSecurity.CreateUserAndAccount(new_user, new_pass);
                        Roles.AddUserToRole(new_user, "Student");
                        StudentInfo old_si = db.StudentInfoes.Find(new_user);
						if (new_user != null)
						{
							//覆盖姓名、班号
							old_si.Name = row.GetCell(0).ToString();
							old_si.ClassId = row.GetCell(2).ToString();
							//初始化Active属性
							old_si.Active = true;
							db.Entry(old_si).State = EntityState.Modified;
							db.SaveChanges();
						}
                        else
						{
							old_si = new StudentInfo();
							old_si.Id = new_user;
							old_si.Name = row.GetCell(0).ToString();
							old_si.ClassId = row.GetCell(2).ToString();
							old_si.ApplyGrant = false;
							old_si.ApplyScholarship = false;
							old_si.Qualification = true;
							old_si.SubmitScoring = false;
							old_si.Active = true;
							newStudents.Add(old_si);
						}
                    }
                    row = sheet.GetRow(++i);
                }
                newStudents.ForEach(s => db.StudentInfoes.Add(s));
                db.SaveChanges();
                stream.Close();
                return_msg = "用户信息导入成功！";
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