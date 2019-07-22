using BnetApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BnetApplication.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        TicketOpiration op = new TicketOpiration();

        // GET: Ticket
        public ActionResult AddTicket(string ANI = "")
        {
            ViewBag.ANI = ANI;        
            BindProblemType();
            BindCallAddress();
            BindRouter();
            BindQuestionTypes();            
            if(User.IsInRole("NightShift"))
                ViewBag.UserType = "NightShift";
            return View();
        }

        [HttpPost]
        public ActionResult AddTicket(TicketAdd TicketAdd)
        {
            string questiontype = "NOT Question";
            if (TicketAdd.ProblemType == "اخرى")
            {
                TicketAdd.ProblemType = TicketAdd.OthersProblemType;
            }
             questiontype = op.getQuestionType(Request["Question"].ToString());
            //get the first,sec,third,last name from form and set to SubscriberName
            string customerName = Request["SubscriberFirstName"].Trim(' ') + " " + Request["SubscriberSecName"].Trim(' ') + " "
                                    + Request["SubscriberThirdName"].Trim(' ') + " " + Request["SubscriberLastName"].Trim(' ');
            TicketAdd.SubscriberName = customerName;
            //to get the call type & callerGender from the dropdownlist @Ticketform
            TicketAdd.CallType = Request["dropDownProblemType"];
            if(Request["callerGender"] == "1")
            TicketAdd.Gender = "Male".ToString();
            else
                TicketAdd.Gender = "Female".ToString();
            //defult value for calladdress its not used now
            TicketAdd.CallAddress = "1";

            //set defult values if not exists
            if (TicketAdd.CallType.Equals("2"))
            {
                
                TicketAdd.Router = "1";
                TicketAdd.ProblemType = "Question";
                TicketAdd.questiontext = questiontype;
                TicketAdd.TicketDetails = TicketAdd.TicketDetails;
            }
            else if (TicketAdd.CallType.Equals("3"))
            {
                TicketAdd.Router = "1";
                TicketAdd.ProblemType = "Complaint";
                TicketAdd.questiontext = questiontype;
            }
            else if (TicketAdd.CallType.Equals("0"))
            {
                TicketAdd.Router = "1";
                TicketAdd.ProblemType = "New Lead";
                TicketAdd.questiontext = questiontype;
            }

            if (User.IsInRole("NightShift"))
                TicketAdd.Router = "1";
            TicketAdd.questiontext = questiontype;
            string ServiceNo = "970" + TicketAdd.ServiceNumber;
            TicketAdd.ServiceNumber = ServiceNo;

            string ms = op.AddTicket(TicketAdd);

            return Json(new { ms }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetTicketPerServiceNo(string No)
        {
            List<TicketSearch> Tickets = new List<TicketSearch>();
            Tickets = op.TicketSearch(No);
            string []customerNameSplited = Tickets[0].SubscriberName.Split(' ');            
            if (customerNameSplited.Length == 4)
            {
                ViewBag.firstName = customerNameSplited[0];
                ViewBag.secName = customerNameSplited[1];
                ViewBag.thirdName = customerNameSplited[2];
                ViewBag.lastName = customerNameSplited[3];
            }
            else
            {                
                ViewBag.secName = "";
                ViewBag.thirdName = "";
                ViewBag.lastName = "";
                for (int i = 0; i < customerNameSplited.Length; i++)
                {
                    ViewBag.firstName += customerNameSplited[i] + " ";
                }
            }
            return View(Tickets);
        }

        public ActionResult TicketDetails(string No)
        {
            Ticket ticket = new Ticket();
            ticket = op.TicketSingl(No);
            BindStatus();
            return View(ticket);
        }

        [HttpGet]
        [Authorize(Roles = "Agent")]
        public ActionResult Nightshift()
        {
            List<TicketSearch> Tickets = new List<TicketSearch>();
            Tickets = op.NightShiftTickets();
            return View(Tickets);
        }

        [HttpPost]
        [Authorize(Roles = "Agent")]
        public JsonResult setTicketStatusOpned()
        {
            try
            {
                string ms = op.updateNightShiftTicketStatus(Request.Form["id"].ToString());
                return Json(new { ms}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                string ms = "Fail";
                return Json(new { ms }, JsonRequestBehavior.AllowGet);
            }
            
        }


        [HttpPost]
        public ActionResult UpdateAgentTickit(TicketAgentUpdate ticketAgentUpdate)
        {
            string ms = op.UpdateAgentTicket(ticketAgentUpdate);
            return Json(new { ms }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Report()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Report1(Nullable<DateTime> from, Nullable<DateTime> to)
        {
            List<Ticket> Tickets = new List<Ticket>();
            Tickets = op.TicketReport(from, to);


            var JsonResult = Json(new { data = Tickets });
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;

            //return Json(new { data = Tickets }, JsonRequestBehavior.AllowGet);
        }
        /******************************** *******************************/

        [HttpGet]
        [Authorize(Roles = "Reach")]
        public ActionResult TicketPerStatus()
        {
            BindStatus1();
            return View();
        }
        
        [HttpPost]
        public ActionResult TicketPerStatus1(string Status, string CallType)
        {
            List<Ticket> Tickets = new List<Ticket>();
            Tickets = op.TicketlistIntearnal(Status, CallType);


            var JsonResult = Json(new { data = Tickets });
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
           // return Json(new { data = Tickets});

        }

        [HttpGet]
        [Authorize(Roles = "Bnet")]

        public ActionResult TicketPerStatus2()
        {
            BindStatus1();
            return View();
        }


        [HttpPost]
        public ActionResult TicketPerStatus3( string CallType,string Status)
        {
            List<Ticket> Tickets = new List<Ticket>();
            if(Status.Equals("1"))
            {
                Tickets = op.TicketlistExtarnal(CallType, Status);
            }
            else
            {
                Tickets = op.TicketlistExtarnaloption2(CallType, Status);
            }
            var JsonResult = Json(new { data  = Tickets });
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }



        [HttpGet]
        public ActionResult UpdateTicketInternal(string id)
        {
            Ticket ticket = new Ticket();
            ticket = op.TicketSingl(id);
            if (ticket.StatusExternal == "جديد")
            {
            op.TicketExtarnalUpdateStatus(id);
            }

            BindStatus();
            return View(ticket);
        }

        [HttpPost]
        public ActionResult UpdateTicketInternal(TicketInternalUpdate ticketInternalUpdate)
        {
            if (ticketInternalUpdate.IsRedirect==null)
            {
                ticketInternalUpdate.IsRedirect = "N";
            }
            //ticketInternalUpdate.TicketInternalAction = "3";
            string ms = op.TicketInternalUpdate(ticketInternalUpdate);
            return Json(new { ms }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult UpdateTicketExternal(string id)
        {
            Ticket ticket = new Ticket();
            ticket = op.TicketSingl(id);
            if (ticket.StatusExternal == "جديد")
            {
                op.TicketExtarnalUpdateStatus(id);
            }
            BindStatus();
            return View(ticket);
        }

        [HttpGet]
        [Authorize(Roles = "Agent")]
        public ActionResult updateNightShiftTicket(string id)
        {
            Ticket ticket = new Ticket();
            ticket = op.TicketSingl(id);            
            BindStatus();
            return View(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Agent")]
        public ActionResult updateNightShiftTicketA(TicketExtarnalUpdate ticketExtarnalUpdate)
        {
            string  ms = op.TicketNightshiftAction(ticketExtarnalUpdate);
            return Json(new { ms }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateTicketExternal(TicketExtarnalUpdate ticketExtarnalUpdate)
        {
            //ticketInternalUpdate.TicketInternalAction = "3";
            string ms = op.TicketExtarnalUpdate(ticketExtarnalUpdate);
            return Json(new { ms }, JsonRequestBehavior.AllowGet);
        }


        /***************************************************************/
       

        [NonAction]
        private void BindProblemType()
        {
            DataTable dst = new DataTable();
            dst = op.BindProblemType();
            List<SelectListItem> CallType = new List<SelectListItem>();
            foreach (DataRow dr in dst.Rows)
            {
                CallType.Add(new SelectListItem
                {
                    Text = dr["problemname"].ToString()
                    ,
                    Value = dr["problemname"].ToString()
                });
            }
            ViewData["ProblemType"] = CallType;

        }

        [NonAction]
        private void BindCallAddress()
        {
            DataTable dst = new DataTable();
            dst = op.BindCallAddress();
            List<SelectListItem> CallType = new List<SelectListItem>();
            foreach (DataRow dr in dst.Rows)
            {
                CallType.Add(new SelectListItem
                {
                    Text = dr["calladdress"].ToString()
                    ,
                    Value = dr["id"].ToString()
                });
            }
            ViewData["CallAddress"] = CallType;

        }

        [NonAction]
        private void BindRouter()
        {
            DataTable dst = new DataTable();
            dst = op.BindRouter();
            List<SelectListItem> CallType = new List<SelectListItem>();
            foreach (DataRow dr in dst.Rows)
            {
                CallType.Add(new SelectListItem
                {
                    Text = dr["routername"].ToString()
                    ,
                    Value = dr["id"].ToString()
                });
            }
            ViewData["Router"] = CallType;

        }


        [NonAction]
        private void BindQuestionTypes()
        {
            DataTable dst = new DataTable();
            dst = op.BindQuestionType();
            List<SelectListItem> QuestionType = new List<SelectListItem>();
            foreach (DataRow dr in dst.Rows)
            {
                QuestionType.Add(new SelectListItem
                {
                    Text = dr["questiontext"].ToString()
                    ,
                    Value = dr["id"].ToString()
                });
            }
            ViewData["Question"] = QuestionType;

        }

        [NonAction]
        private void BindStatus()
        {
            DataTable dst = new DataTable();
            dst = op.BindsTATUS();
            List<SelectListItem> CallType = new List<SelectListItem>();
            foreach (DataRow dr in dst.Rows)
            {
                if (dr["id"].ToString() != "1")
                {
                    CallType.Add(new SelectListItem
                    {
                        Text = dr["statusname"].ToString(),
                        Value = dr["id"].ToString()
                    });
                }
            }
            ViewData["Status"] = CallType;

        }
        
        [NonAction]
        private void BindStatus1()
        {
            DataTable dst = new DataTable();
            dst = op.BindsTATUS();
            List<SelectListItem> CallType = new List<SelectListItem>();
            foreach (DataRow dr in dst.Rows)
            {
                
                    CallType.Add(new SelectListItem
                    {
                        Text = dr["statusname"].ToString(),
                        Value = dr["id"].ToString()
                    });
                
            }
            ViewData["Status1"] = CallType;

        }
    }
}