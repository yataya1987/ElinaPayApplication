using BnetApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BnetApplication.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AccountController : Controller
    {
        Opiration op = new Opiration();

        // GET: Account
        public ActionResult AccountList()
        {
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectAccount());
            List<Account> accounts = new List<Account>();
            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                Account a = new Account();
                a.AccountID = dr["ACCOUNT_ID"].ToString();
                a.AccountName = dr["ACCOUNT_NAME"].ToString();
                a.Status = dr["ACCOUNT_STATUS"].ToString();
                accounts.Add(a);
            }
            return View(accounts);
        }

        // GET: Account/Create
      
        public ActionResult Create()
        {
            BindStatus();
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(Account  account)
        {
            try
            {
                BindStatus();

                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                  string mes=op.CreateAccount(account.AccountName);
                    if (mes != "done")
                    {
                        BindStatus();
                        ViewBag.mes = mes;
                        return View();
                    }

                    return RedirectToAction("AccountList");

                }
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.mes = ex.Message;

                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(string id)
        {
            BindStatus();
            DataSet dst = new DataSet();
            dst.Tables.Add(op.SelectAccount(id));
           Account account = new Account();

            foreach (DataRow dr in dst.Tables[0].Rows)
            {
                account.AccountID = dr["ACCOUNT_ID"].ToString();
                account.AccountName = dr["ACCOUNT_NAME"].ToString();
                account.Status = dr["ACCOUNT_STATUS"].ToString();
            }
           
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Account account)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string mes = op.EditAccount(id,account.AccountName,account.Status);
                    if (mes != "done")
                    {
                        BindStatus();

                        ViewBag.mes = mes;
                        return View();
                    }
                    return RedirectToAction("AccountList");

                }
                BindStatus();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex.Message;
                BindStatus();
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
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
        private void BindStatus()
        {
            List<SelectListItem> Status = new List<SelectListItem>();
            Status.Add(new SelectListItem
            {
                Text = "Enabled"
                ,
                Value = "Enabled"
            });


            Status.Add(new SelectListItem
            {
                Text = "Disabled"
                ,
                Value = "Disabled"
        });
            ViewBag.Status = Status;
            }


    }
}
