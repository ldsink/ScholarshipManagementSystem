using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScholarshipManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Teacher"))
                return RedirectToAction("Teacher", "Home");
            else if (User.IsInRole("Student"))
                return RedirectToAction("Student", "Home");
            ViewBag.Message = "";
            return View();
        }

        // 教师首页
        [Authorize(Roles = "Teacher")]
        public ActionResult Teacher()
        {
            return View();
        }

        // 学生首页
        [Authorize(Roles = "Student")]
        public ActionResult Student()
        {
            return View();
        }

        // 学生首页动漫版
        [Authorize(Roles = "Student")]
        public ActionResult StudentForSink()
        {
            return View();
        }

        // 班级打分
        [Authorize(Roles = "Student")]
        public ActionResult Scoring()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Scoring", });
            return View();
        }

        // 申请
        [Authorize(Roles = "Student")]
        public ActionResult Apply()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Apply", });
            return View();
        }

        // 加分项申报
        [Authorize(Roles = "Student")]
        public ActionResult Bonus()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", });
            return View();
        }

        // 普通加分项编辑
        [Authorize(Roles = "Student")]
        public ActionResult BonusNormalEdit(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", }); ;
            ViewBag.Id = id;

            return View();
        }

        // 论文加分项编辑
        [Authorize(Roles = "Student")]
        public ActionResult BonusPaperEdit(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", });
            ViewBag.Id = id;

            return View();
        }
        // 比赛加分项编辑
        [Authorize(Roles = "Student")]
        public ActionResult BonusCompetitionEdit(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", });
            ViewBag.Id = id;

            return View();
        }

        // 普通加分项编辑（无刷新）
        [Authorize(Roles = "Student")]
        public ActionResult BonusNormalEdit2(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", });
            ViewBag.Id = id;

            return View();
        }

        // 论文加分项编辑（无刷新）
        [Authorize(Roles = "Student")]
        public ActionResult BonusPaperEdit2(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", });
            ViewBag.Id = id;

            return View();
        }
        // 比赛加分项编辑（无刷新）
        [Authorize(Roles = "Student")]
        public ActionResult BonusCompetitionEdit2(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Bonus", });
            ViewBag.Id = id;

            return View();
        }

        // 奖励项目列表
        [Authorize(Roles = "Student")]
        public ActionResult BonusList()
        {
            return View();
        }

        // 加分项审核
        [Authorize(Roles = "Teacher")]
        public ActionResult Verify()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Verify", });
            return View();
        }

        // 成绩总表
        [Authorize(Roles = "Teacher")]
        public ActionResult BigTable()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "BigTable", });
            return View();
        }

        // 违纪情况管理
        [Authorize(Roles = "Teacher")]
        public ActionResult Disciplinary()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Disciplinary", });
            return View();
        }

        // 违纪情况弹出框
        [Authorize(Roles = "Teacher")]
        public ActionResult DisciplinaryDialog()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Disciplinary", });
            return View();
        }

        // 违纪情况弹出框
        [Authorize(Roles = "Teacher")]
        public ActionResult DisciplinaryModify(String id)
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Disciplinary", });
            ViewBag.Id = id;

            return View();
        }

        // 数据导入导出
        [Authorize(Roles = "Teacher")]
        public ActionResult ImExport()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Export", });

            return View();
        }

        // 数据导入（初始化数据库）
        [Authorize(Roles = "Teacher")]
        public ActionResult Import()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Import", });

            return View();
        }

        // 开关控制
        [Authorize(Roles = "Teacher")]
        public ActionResult OnOff()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "OnOff", });

            return View();
        }

        // 普通加分项目管理
        [Authorize(Roles = "Teacher")]
        public ActionResult BonusNormal()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "BonusNormal", });
            return View();
        }

        // 论文加分项目管理
        [Authorize(Roles = "Teacher")]
        public ActionResult BonusPaper()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "BonusPaper", });
            return View();
        }

        // 比赛加分项目管理
        [Authorize(Roles = "Teacher")]
        public ActionResult BonusCompetition()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "BonusCompetition", });
            return View();
        }

        // 删除学生帐号信息
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteUser()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "DeleteUser", });
            return View();
        }

        // 导出学生信息
        [Authorize(Roles = "Teacher")]
        public ActionResult ExportForStudentInfo()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Export", });
            return View();
        }

        // 导出班级打分信息
        [Authorize(Roles = "Teacher")]
        public ActionResult ExportForScoring()
        {
            ViewBag.ApiUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "ExportForScoring", });
            return View();
        }
    }
}
