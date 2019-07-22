using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BnetApplication.Data_Access_Layer;
using BnetApplication.Models;
using System.Data;
using System.Web.Security;
using Logger;
using System.Diagnostics;

namespace BnetApplication.Controllers
{
    public class LoginController : Controller
    {
        Opiration op = new Opiration();

        // GET: Login
        [HttpGet]
        public ActionResult login()
        {
            BindUserType();
            return View();
        }

        [HttpPost]
        [ActionName("login")]
        public ActionResult login(Signin signin ,string ReturnUrl)
        {
            int res ;
            if (ModelState.IsValid)
            {
                res = LogInClass.LogInMethod(signin.UserName, signin.PassWord, signin.UserType, "DBS", "ConnString");
                //Success
                if (res == 1)
                {
                    ViewBag.status = 1;
                    ViewBag.Mes = Session["ErrorMessage"];
                    ViewBag.UserName = Session["UserName"];
                    ViewBag.UserAccount = Session["UserAccount"];
                    ViewBag.UserType = Session["UserType"];
                    BindUserType();

                    FormsAuthentication.SetAuthCookie(signin.UserName, false);

                    var authTicket = new FormsAuthenticationTicket(1, signin.UserName, DateTime.Now, DateTime.Now.AddMinutes(20), false, Session["UserType"].ToString());
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    //returnURL needs to be decoded
                    string decodedUrl = "";
                    if (!string.IsNullOrEmpty(ReturnUrl))
                        decodedUrl = Server.UrlDecode(ReturnUrl);

                    //Login logic...

                    if (Url.IsLocalUrl(decodedUrl))
                    {
                        return Redirect(decodedUrl);
                    }
                    else
                    {

                        if (Session["UserType"].ToString() == "Admin")
                        {
                            return RedirectToAction("index", "dea");

                        }
                        if (Session["UserType"].ToString() == "Agent")
                        {
                            return RedirectToAction("AddTicket", "Ticket");
                        }
                        if (Session["UserType"].ToString() == "NightShift")
                        {
                            return RedirectToAction("AddTicket", "Ticket");
                        }
                        if (Session["UserType"].ToString() == "Bnet")
                        {
                            return RedirectToAction("TicketPerStatus2", "Ticket");
                        }

                        if (Session["UserType"].ToString() == "Reach")
                        {
                            return RedirectToAction("TicketPerStatus", "Ticket");
                        }
                        else
                        {
                            return RedirectToAction("Login", "Login");

                        }
                    }
                }
                //Fild
                else if (res == 0)
                {
                    ViewBag.status = 0;
                    ViewBag.Mes = Session["ErrorMessage"];
                    BindUserType();
                    BindUserType();
                    return View("login");
                }
                //chang Passwoard
                else if (res == 2)
                {
                    ViewBag.status = 1;
                    ViewBag.Mes = Session["ErrorMessage"];
                    ViewBag.UserName = Session["UserName"];
                    ViewBag.UserAccount = Session["UserAccount"];
                    ViewBag.UserType = Session["UserType"];
                    BindUserType();
                    return RedirectToAction("ChangePass");
                }
                else
                {
                    ViewBag.status = 0;
                    ViewBag.Mes = Session["ErrorMessage"];
                    BindUserType();
                    return View("login");
                }
            }
            else
            {
                BindUserType();
                return View("login");
            }
        }

        //change passWoard
        [HttpGet]
        public ActionResult ChangePass()
        {
            
            return View();
        }

        [HttpPost]
        [ActionName("ChangePass")]
        public ActionResult ChangePass(ChangePassWoard changepass)
        {
           try { 
           if( ChangePasswordClass.ChangePasswordMethod(changepass.CurrentPass, changepass.NewPass, changepass.ConfirmPass, "DBS", "ConnString"))
            {
                ViewBag.status = 1;
                ViewBag.mes = Session["ErrorMessage"];
                ViewBag.UserName = Session["UserName"];
                ViewBag.UserAccount = Session["UserAccount"];
                ViewBag.UserType = Session["UserType"];
                BindUserType();
                return RedirectToAction("login");
            }
            else
            {
                ViewBag.status = 0;
                ViewBag.mes = Session["ErrorMessage"];
                return View("ChangePass");
                }
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex;
                ErrorLogs.WriteErrorLogs("ChangePass, ChangePass, Session[\"ErrorMessage\"] = Null", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, Session["UserName"].ToString(), "White_List_System", ex);
                ErrorLogs.WriteErrorLogsXML("ChangePass, ChangePass, Session[\"ErrorMessage\"] = Null", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, Session["UserName"].ToString(), "White_List_System", ex);
                return View("ChangePass");
            }

        }

        //Forget passWord
        [HttpGet]
        public ActionResult ForgotPasswoard()
        {
            BindUserType();
            return View();
        }

        //Forget passWord
        [HttpPost]
        public ActionResult ForgotPasswoard(ForgotPasswoard forget)
        {
            try
            {
                if ((GetPasswordClass.GetPasswordMethod(forget.UserName, forget.UserType, "DBS", "ConnString")))
                 {

                    ViewBag.status = 1;
                    
                    return RedirectToAction("login");
                }
                else
                {
                    ViewBag.mes = Session["ErrorMessage"];
                }
            }
            catch (Exception ex)
            {
                ViewBag.mes = ex;
                ErrorLogs.WriteErrorLogs("ForgotPasswordPage, btnSend_Click, Session[\"ErrorMessage\"] = Null", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, Session["UserName"].ToString(), "White_List_System", ex);
                ErrorLogs.WriteErrorLogsXML("ForgotPasswordPage, btnSend_Click, Session[\"ErrorMessage\"] = Null", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, Session["UserName"].ToString(), "White_List_System", ex);
            }

            BindUserType();
            return View();
        }
        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("login", "login");
        }

        [NonAction]
        private void BindUserType() {
            DataSet dst = new DataSet();
            dst.Tables.Add ( op.SelectUserType());
            List<SelectListItem> UserTypeList = new List<SelectListItem>();
            foreach (DataRow  dr in dst.Tables[0].Rows)
            {
                UserTypeList.Add(new SelectListItem
                {
                    Text = dr["privilege_name"].ToString()
                    ,
                    Value = dr["privilege_id"].ToString()
                });
            }
            ViewData["UserType"] = UserTypeList;

        }
    }
}