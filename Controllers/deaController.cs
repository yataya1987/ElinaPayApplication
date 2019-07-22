using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BnetApplication.Models;
using System.Data;

namespace BnetApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class deaController : Controller
    {
        Opiration op = new Opiration();
        // GET: dea
        public ActionResult Index()
        {
            ViewBag.QuestionTypeCounter = op.CountQuestionTypes().ToString();
            ViewBag.RouterTypeCounter = op.CountRouterTypes().ToString();
            ViewBag.ProblemTypeCounter = op.CountProblemTypes().ToString();
            ViewBag.count = op.CountUser().ToString();
            ViewBag.Account = op.CountAccount().ToString();
            ViewBag.Skill = op.CountSkill().ToString();
            ViewBag.GROUPS = op.CountGROUPS().ToString();
            DataTable dt = new DataTable();
            dt = op.SelectLastUsers();
            User user = new User();
            List<User> Users = new List<User>();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i += 1;
                User u = new User();
                u.UserID = dr["USER_ID"].ToString();
                u.userName = dr["USER_NAME"].ToString();
                u.userMobile = dr["USER_MOBILE"].ToString();
                u.userEmail = dr["USER_EMAIL"].ToString();
                u.userIP = dr["USER_IP"].ToString();
                u.userPrivilege = dr["PRIVILEGE_NAME"].ToString();
                u.userStatus = dr["USER_STATUS"].ToString();
                u.userAuthType = dr["USER_AUTH"].ToString();
                u.Idate = Convert.ToDateTime(dr["idate"]);
                Users.Add(u);
                if (i ==10)
                {
                    break;
                }
            }
            return View(Users);
        }

        // GET: dea/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: dea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: dea/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: dea/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: dea/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: dea/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: dea/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
