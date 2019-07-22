using PagedList;
using PagedList.Mvc;
using BnetApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BnetApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        Opiration op = new Opiration();

        // GET: Account
        public ActionResult UserList(int? page,string search)
        {
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectUsers(search));
            List<User> Users = new List<User>();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                User u = new User();
                u.UserID = dr["USER_ID"].ToString();
                u.userName = dr["USER_NAME"].ToString();
                u.userMobile = dr["USER_MOBILE"].ToString();
                u.userEmail = dr["USER_EMAIL"].ToString();
                u.userIP = dr["USER_IP"].ToString();
                u.userPrivilege = dr["PRIVILEGE_NAME"].ToString();
                u.userStatus = dr["USER_STATUS"].ToString();
                u.userAuthType = dr["USER_AUTH"].ToString();
                Users.Add(u);
            }
            return View(Users.ToPagedList(page?? 1,10));
        }

        
        // GET: User/Create
        public ActionResult Create()
        {
            BindStatus();
            BindPrivilege();
            BindSkill();
            BindAuth();
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
               

                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.CreateUsers(user.userName,user.userPass,user.userPrivilege,
                                                user.userAuthType,user.userSkill,user.userEmail,
                                                user.userMobile,user.userIP);
                    if (mes != "done")
                    {
                        ViewBag.mes = mes;
                        return View();
                    }

                    return RedirectToAction("UserList");

                }
                BindStatus();
                BindPrivilege();
                BindSkill();
                BindAuth();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;

                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string  id)
        {

            BindStatus();
            BindPrivilege();
            BindSkill();
            BindAuth();
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectUser(id));
            User u = new User();

            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                u.UserID = dr["USER_ID"].ToString();
                u.userName = dr["USER_NAME"].ToString();
                u.userMobile = dr["USER_MOBILE"].ToString();
                u.userEmail = dr["USER_EMAIL"].ToString();
                u.userIP = dr["USER_IP"].ToString();
                u.userPrivilege = dr["PRIVILEGE_ID"].ToString();
                u.userStatus = dr["USER_STATUS"].ToString();
                u.userAuthType = dr["USER_AUTH"].ToString();
                u.userPass = "Abcd@1234";
                u.ConfirmPass = "Abcd@1234";
                u.userSkill = dr["SKILL_ID"].ToString();
            }

            return View(u);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
              
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.UpdaeUsers(user.UserID, user.userName, user.userPass, user.userPrivilege, user.userAuthType, user.userSkill, user.userEmail, user.userMobile, user.userIP, user.userStatus);
                    if (mes != "done")
                    {
                        BindStatus();

                        ViewBag.mes = mes;
                        return View();
                    }
               
                    return RedirectToAction("UserList");

                }
                BindStatus();
                BindPrivilege();
                BindSkill();
                BindAuth();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;
                BindStatus();
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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
        [HttpPost]
        public ActionResult EnabledAccount(string id, string Status)
        {
            try
            {
                string result = op.EnableorDisableUsers(id, Status);
                // TODO: Add delete logic here
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [NonAction]
        private void BindStatus()
        {
            List<SelectListItem> AccountType = new List<SelectListItem>();
            AccountType.Add(new SelectListItem
            {
                Text = "Enabled"
                ,
                Value = "Enabled"
            });


            AccountType.Add(new SelectListItem
            {
                Text = "Disabled"
                    ,
                Value = "Disabled"
            });
            ViewBag.Status = AccountType;
        }

        [NonAction]
        private void BindAuth()
        {
            List<SelectListItem> AuthType = new List<SelectListItem>();
            AuthType.Add(new SelectListItem
            {
                Text = "LDAP"
                ,
                Value = "LDAP"
            });


            AuthType.Add(new SelectListItem
            {
                Text = "NOT_LDAP"
                    ,
                Value = "NOT_LDAP"
            });
            ViewBag.AuthType = AuthType;
        }

        [NonAction]
        private void BindPrivilege()
        {
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectPRIVILEGE());
            List<SelectListItem> PRIVILEGEList = new List<SelectListItem>();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                PRIVILEGEList.Add(new SelectListItem
                {
                    Text = dr["PRIVILEGE_NAME"].ToString()
                    ,
                    Value = dr["PRIVILEGE_ID"].ToString()
                });
            }
            ViewData["PRIVILEGE"] = PRIVILEGEList;

        }

        [NonAction]
        private void BindSkill()
        {
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectSkill());
            List<SelectListItem> SkillList = new List<SelectListItem>();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                SkillList.Add(new SelectListItem
                {
                    Text = dr["SKILL_NAME"].ToString()
                    ,
                    Value = dr["SKILL_ID"].ToString()
                });
            }
            ViewData["Skill"] = SkillList;

        }
    }

}
