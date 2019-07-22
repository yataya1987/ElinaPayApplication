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
    public class GroupController : Controller
    {
        Opiration op = new Opiration();
        // GET: Group
        public ActionResult GroupList()
        {
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectGROUPS());
            List<Group> group = new List<Group>();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                Group groups = new Group();
                groups.id = dr["GROUP_ID"].ToString();
                groups.GroupName = dr["GROUP_NAME"].ToString();
                groups.groupPrivilege = dr["PRIVILEGE_NAME"].ToString();
                groups.groupAccount = dr["ACCOUNT_NAME"].ToString();
                groups.groupStatus = dr["GROUP_STATUS"].ToString();

                group.Add(groups);
            }
            return View(group);
        }
        

        // GET: Group/Create
        public ActionResult Create()
        {
            BindAccount();
            BindStatus();
            BindPrivilege();
            BindSkill();
            return View();
        }

        // POST: Group/Create
        [HttpPost]
        public ActionResult Create(Group group)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.CreateGROUPS(group.GroupName, group.groupPrivilege, group.groupAccount, group.groupSkill);
                    if (mes != "done")
                    {
                        ViewBag.mes = mes;
                        BindAccount();
                        BindStatus();
                        BindPrivilege();
                        BindSkill();
                        return View();
                    }
                    return RedirectToAction("GroupList");

                }
                BindAccount();
                BindStatus();
                BindPrivilege();
                BindSkill();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;
                BindAccount();
                BindStatus();
                BindPrivilege();
                BindSkill();
                return View();
            }
        }

        // GET: Group/Edit/5
        public ActionResult Edit(string id)
        {
            BindAccount();
            BindStatus();
            BindPrivilege();
            BindSkill();
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectGROUPS(id));
            Group group = new Group();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                group.id = dr["GROUP_ID"].ToString();
                group.GroupName = dr["group_name"].ToString();
                group.groupPrivilege = dr["privilege_id"].ToString();
                group.groupAccount = dr["account_id"].ToString();
                group.groupStatus = dr["group_status"].ToString();
                group.groupSkill= dr["skill_id"].ToString(); 
            }
            return View(group);
        }

        // POST: Group/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Group group)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.EditGROUPS(id, group.GroupName, group.groupPrivilege, group.groupAccount, group.groupSkill, group.groupStatus);
                    if (mes != "done")
                    {
                        BindAccount();
                        BindStatus();
                        BindPrivilege();
                        BindSkill();
                        ViewBag.mes = mes;
                        return View();
                    }
                    return RedirectToAction("GroupList");

                }
                BindAccount();
                BindStatus();
                BindPrivilege();
                BindSkill();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;
                BindStatus();
                return View();
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
        private void BindAccount()
        {
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectAccount());
            List<SelectListItem> AccontTypeList = new List<SelectListItem>();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                AccontTypeList.Add(new SelectListItem
                {
                    Text = dr["ACCOUNT_NAME"].ToString()
                    ,
                    Value = dr["ACCOUNT_ID"].ToString()
                });
            }
            ViewData["Account"] = AccontTypeList;

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
