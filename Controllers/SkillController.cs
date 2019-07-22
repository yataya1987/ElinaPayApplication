using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BnetApplication.Models;
using System.Data;

namespace BnetApplication.Models
{
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        Opiration op = new Opiration();
        // GET: Skill
        public ActionResult Index()
        {
         List<Skill> skill = new List<Skill>();
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectSkill());
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                Skill a = new Skill();
                a.SkillID = dr["SKILL_ID"].ToString();
                a.SkillName = dr["SKILL_NAME"].ToString();
                a.Status = dr["SKILL_STATUS"].ToString();
                a.AccountName = dr["ACCOUNT_NAME"].ToString();
                skill.Add(a);
            }
            return View(skill);
        }

        // GET: Skill/Create
        public ActionResult Create()
        {
            BindAccount();
            return View();
        }

        // POST: Skill/Create
        [HttpPost]
        public ActionResult Create(Skill skill)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.CreateSkill(skill.AccountName,skill.SkillName);
                    if (mes != "done")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        BindAccount();
                        return View();
                    }
                }
                else
                {
                    BindAccount();
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;
                BindAccount();
                return View();
            }
        }

        // GET: Skill/Edit/5
        public ActionResult Edit(string id)
        {
            DataSet dst = new DataSet();
            BindAccount();
            Bindstatus();
            dst.Tables.Add(op.SelectSkill(id));
            Skill skill = new Skill();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {

                skill.SkillID = dr["SKILL_ID"].ToString();
                skill.AccountName = dr["account_id"].ToString();
                skill.SkillName = dr["SKILL_NAME"].ToString();
                skill.Status = dr["SKILL_STATUS"].ToString();
            }
            return View(skill);
        }

        // POST: Skill/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Skill skill)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.EditSkill(id,skill.AccountName,skill.SkillName,skill.Status);
                    if (mes != "done")
                    {
                        ViewBag.mes = mes;
                        return View();
                    }
                    return RedirectToAction("Index");

                }
                Bindstatus();
                BindAccount();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;
                BindAccount();
                Bindstatus();
                return View();
            }
        }

        // GET: Skill/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Skill/Delete/5
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
        private void Bindstatus()
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
    }
}
