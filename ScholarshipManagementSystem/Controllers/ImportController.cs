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
    public class ImportController : ApiController
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
                while (row != null && row.GetCell(1) != null)
                {
                    String new_user = row.GetCell(1).ToString();
                    String new_pass = row.GetCell(3).ToString();
                    if (!WebSecurity.UserExists(new_user))
                    {
                        // Add student info
                        StudentInfo new_si = new StudentInfo();
                        new_si.Id = new_user;
                        new_si.Name = row.GetCell(0).ToString();
                        new_si.ClassId = row.GetCell(2).ToString();
                        new_si.SubmitScoring = false;
                        new_si.Qualification = true;
                        new_si.Active = true;
                        db.StudentInfoes.Add(new_si);
                        //增加班级打分表的相应项
                        IEnumerable<StudentInfo> sInfos = db.StudentInfoes.AsEnumerable();
                        foreach (StudentInfo another_si in sInfos)
                        {
                            if (new_si.ClassId == another_si.ClassId && new_si != another_si && another_si.Active == true)
                            {
                                ScoringT st = new ScoringT();
                                st.ScoringStudentInfoId = new_si.Id;
                                st.ScoringStudent = new_si;
                                st.ScoredStudentInfoId = another_si.Id;
                                st.ScoredStudent = another_si;
                                db.ScoringTs.Add(st);
                                ScoringT st_re = new ScoringT();
                                st_re.ScoredStudentInfoId = new_si.Id;
                                st_re.ScoredStudent = new_si;
                                st_re.ScoringStudentInfoId = another_si.Id;
                                st_re.ScoringStudent = another_si;
                                db.ScoringTs.Add(st_re);
                            }
                        }
                        db.SaveChanges();
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
                        //覆盖姓名、班号
                        old_si.Name = row.GetCell(0).ToString();
                        old_si.ClassId = row.GetCell(2).ToString();
                        //初始化Active属性
                        old_si.Active = true;
                        //增加该用户的班级打分表
                        IEnumerable<StudentInfo> sInfos = db.StudentInfoes.AsEnumerable();
                        foreach (StudentInfo another_si in sInfos)
                        {
                            if (old_si.ClassId == another_si.ClassId && old_si != another_si && another_si.Active == true)
                            {
                                ScoringT st = new ScoringT();
                                st.ScoringStudentInfoId = old_si.Id;
                                st.ScoringStudent = old_si;
                                st.ScoredStudentInfoId = another_si.Id;
                                st.ScoredStudent = another_si;
                                db.ScoringTs.Add(st);
                                ScoringT st_re = new ScoringT();
                                st_re.ScoredStudentInfoId = old_si.Id;
                                st_re.ScoredStudent = old_si;
                                st_re.ScoringStudentInfoId = another_si.Id;
                                st_re.ScoringStudent = another_si;
                                db.ScoringTs.Add(st_re);
                            }
                        }
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