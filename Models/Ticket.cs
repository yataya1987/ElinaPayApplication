using BnetApplication.Data_Access_Layer;
using Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BnetApplication.Models
{
    public class Ticket
    {
        [Display(Name = "رقم المكالمة")]
        public int Id { get; set; }

        [Display(Name = "نوع المكالمة")]
        public string CallType { get; set; }

        [Display(Name = "رقم المتصل")]
        public string Ani { get; set; }

        [Display(Name = "جنس المتصل")]
        public string Gender { get; set; }

        [Display(Name = "اسم المشترك")]
        public string SubscriberName { get; set; }

        [Display(Name = "رقم الخدمة")]
        public string ServiceNumber { get; set; }

        [Display(Name = "رقم التواصل")]
        public string AlternativeNumber { get; set; }

        [Display(Name = "العنوان الرئيسي")]
        public string CallAddress { get; set; }

        [Display(Name = "عنوان المشكلة")]
        public string ProblemType { get; set; }

        [Display(Name = "نوع الاستفسار")]
        public string questiontext { get; set; }

        [Display(Name = "تفاصيل")]
        public string TicketDetails { get; set; }

        [Display(Name = "نوع الراوتر")]
        public string Router { get; set; }

        [Display(Name = "تاريخ الادخال")]
        public Nullable<DateTime> Idate { get; set; }

        [Display(Name = "اسم المدخل")]
        public string Iuser { get; set; }

        [Display(Name = "تاريخ تعديل المشرف")]
        public Nullable<DateTime> Udate { get; set; }

        [Display(Name = "اسم المشرف")]
        public string Uuser { get; set; }

        [Display(Name = "تاريخ التعديل من قبل الموظف")]
        public Nullable<DateTime> AgentUdate { get; set; }

        [Display(Name = "الموظف المعدل")]
        public string AgentUpdate { get; set; }

        [Display(Name = "ملاحظات التعديل")]
        public string AgentUNote { get; set; }

        [Display(Name = "حالة المشرف 1")]
        public string StatusInternal { get; set; }

        [Display(Name = "اجراء المشرف 1")]
        public string TicketInternalAction { get; set; }

        [Display(Name = "حالة المشرف 2")]
        public string StatusExternal { get; set; }

        [Display(Name = "اجراء المشرف 2")]
        public string TicketExternalAction { get; set; }

        [Display(Name = "تاريخ تعديل المشرف 2")]
        public Nullable<DateTime> UdateExternal { get; set; }

        [Display(Name = "المشرف 2")]
        public string UuserExternal { get; set; }

        [Display(Name = "جهة المتابعة")]
        public string FollowUp { get; set; }
    }


    public class TicketSearch
    {
        [Display(Name = "رقم المكالمة")]
        public int Id { get; set; }

        [Display(Name = "تاريخ الادخال")]
        public Nullable<DateTime> Idate { get; set; }
       
        //to return the costomer name
        [Display(Name = "اسم المشترك")]
        public string SubscriberName { get; set; }

        [Display(Name = "العنوان الرئيسي")]
        public string CallAddress { get; set; }

        [Display(Name = "عنوان المشكلة")]
        public string ProblemType { get; set; }

        //to display CALL TYPE on View GetTicketPerServiceNo
        [Display(Name = "نوع المكالمة")]
        public string CALLTYPE { get; set; }

        [Display(Name = "نوع الراوتر")]
        public string Router { get; set; }

        [Display(Name = "1 تاريخ تعديل المشرف")]
        public Nullable<DateTime> Udate { get; set; }

        [Display(Name = "اسم المشرف 1")]
        public string Uuser { get; set; }

        [Display(Name = "حالة المشرف 1")]
        public string StatusInternal { get; set; }

        [Display(Name = "حالة المشرف 2")]
        public string StatusExternal { get; set; }

        [Display(Name = "اسم المشرف 2")]
        public string Uuserexternal { get; set; }

        [Display(Name = "تاريخ تعديل المشرف 2")]
        public Nullable<DateTime> UdateExternal { get; set; }
    }

    public class TicketAdd
    {
        [Display(Name = "نوع المكالمة")]
        public string CallType { get; set; }

        [Display(Name = "رقم المتصل")]
        public string Ani { get; set; }

        [Display(Name = "اسم المشترك")]
        public string SubscriberName { get; set; }

        [Display(Name = "جنس المتصل")]
        public string Gender { get; set; }

        [Display(Name = "رقم الخدمة")]
        public string ServiceNumber { get; set; }

        [Display(Name = "رقم التواصل")]
        public string AlternativeNumber { get; set; }

        [Display(Name = "العنوان الرئيسي")]
        public string CallAddress { get; set; }

        [Display(Name = "عنوان المشكلة")]
        public string ProblemType { get; set; }

        [Display(Name = "نوع الاستفسار")]
        public string questiontext { get; set; }

        [Display(Name = "نوع الراوتر")]
        public string OthersProblemType { get; set; }

        [Display(Name = "تفاصيل")]
        public string TicketDetails { get; set; }

        [Display(Name = "نوع الراوتر")]
        public string Router { get; set; }

    }

    public class TicketAgentUpdate
    {
        [Display(Name = "رقم المكالمة")]
        public int Id { get; set; }


        [Display(Name = "ملاحظات التعديل")]
        public string AgentUNote { get; set; }
    }

    public class TicketInternalUpdate
    {
        [Display(Name = "رقم المكالمة")]
        public int Id { get; set; }
        

        [Display(Name = "حالة المشرف 1")]
        public string StatusInternal { get; set; }

        [Display(Name = "جهة المتابعة")]
        public string FollowUp { get; set; }

        [Display(Name = "اجراء المشرف 1")]
        public string TicketInternalAction { get; set; }


        [Display(Name = "تحويل الى المشرف 2")]
        public string IsRedirect { get; set; }
    }


    public class TicketExtarnalUpdate
    {
        [Display(Name = "رقم المكالمة")]
        public int Id { get; set; }

        [Display(Name = "حالة المشرف 2")]
        public string StatusExternal { get; set; }

        [Display(Name = "اجراء المشرف 2")]
        public string TicketExternalAction { get; set; }
        
    }


    public class TicketOpiration
    {
        DataAccessLayer da = new DataAccessLayer();
        
        public string AddTicket(TicketAdd TicketAdd)
        {
            try
            {

                string query = @" insert into
                             Ticket(id,calltype,
                                    ani,subscribername,
                                    servicenumber,alternativenumber,
                                    calladdress,problemtype,
                                    Ticketdetails,iuser,idate,statusinternal,Router,gender,STATUSEXTERNAL,questiontext)
                                      values
                                    (Ticket_SEQ.NEXTVAL,
                                    :calltype,:ani,:subscribername,
                                    :servicenumber,:alternativenumber,
                                    :calladdress,:problemtype,:Ticketdetails,
                                    :iuser,sysdate,:statusinternal,:Router,:gender,:STATUSEXTERNAL,:questiontxt)";
                OracleParameter[] par = new OracleParameter[14];
                par[0] = new OracleParameter("calltype", TicketAdd.CallType);
                par[1] = new OracleParameter("ani", TicketAdd.Ani);
                par[2] = new OracleParameter("subscribername", TicketAdd.SubscriberName);
                par[3] = new OracleParameter("servicenumber", TicketAdd.ServiceNumber);
                par[4] = new OracleParameter("alternativenumber", TicketAdd.AlternativeNumber == null ? " " : TicketAdd.AlternativeNumber);
                par[5] = new OracleParameter("calladdress", TicketAdd.CallAddress);
                par[6] = new OracleParameter("problemtype", TicketAdd.ProblemType == null ? " " : TicketAdd.ProblemType);
                par[7] = new OracleParameter("Ticketdetails", TicketAdd.TicketDetails == null ? " " : TicketAdd.TicketDetails);
                par[8] = new OracleParameter("iuser", HttpContext.Current.Session["UserName"]);
                par[9] = new OracleParameter("statusinternal", "1");
                par[10] = new OracleParameter("Router", TicketAdd.Router);
                par[11] = new OracleParameter("gender", TicketAdd.Gender);
                par[12] = new OracleParameter("STATUSEXTERNAL", "1");
                par[13] = new OracleParameter("questiontxt", TicketAdd.questiontext);

                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                
                return "Success";
            }
            catch (Exception ex)
            {                
                ErrorLogs.WriteErrorLogs("TicketOpiration, AddTicket", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, AddTicket", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
        }

        public string getQuestionType(string id)
        {
            string questiontext = "";
            if (id != "" && id != null)
            {
                DataTable dt = new DataTable();
                string query = @"SELECT a.QUESTIONTEXT
                            FROM QUESTIONTYPES a where a.id='"+id+"'";
                da.open();
                dt = da.SelectData(query, null);
                da.close();
                foreach (DataRow item in dt.Rows)
                {
                    questiontext=item["QUESTIONTEXT"].ToString();
                }
                return questiontext;
            }
            else
            {
                return questiontext;
            }
           
        }
        public string UpdateAgentTicket(TicketAgentUpdate TicketAgentUpdate)
        {
            try
            {
                string query = @" update Ticket a set 
                                  a.agentudate=sysdate,
                                  a.agentunote=CONCAT(a.agentunote,:agentunote),
                                  a.agentupdate=:agentupdate
                                  where a.id=:id";
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("agentunote", " \n " + "(NewUpdate)-(" + HttpContext.Current.Session["UserName"] + ")(" + DateTime.Now.ToString() + ")-" + TicketAgentUpdate.AgentUNote);
                par[1] = new OracleParameter("id", TicketAgentUpdate.Id);
                par[2] = new OracleParameter("agentupdate", HttpContext.Current.Session["UserName"]);
                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("TicketOpiration, UpdateAgentTicket", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, UpdateAgentTicket", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;

            }
        }

        public string  TicketInternalUpdate(TicketInternalUpdate TicketInternalUpdate)
        {
            try
            {
                if (TicketInternalUpdate.TicketInternalAction == null)
                {
                    TicketInternalUpdate.TicketInternalAction = "";
                }
                if (TicketInternalUpdate.IsRedirect == "Y")
                {
                    string query = @" update Ticket a set 
                                  a.udate=sysdate,
                                  a.uuser=:uuser,
                                  a.statusinternal=:statusinternal,
                                  a.Ticketinternalaction=:Ticketinternalaction,
                                  a.followup=:followup,
                                  a.ISREDIRECT=:ISREDIRECT,
                                  a.statusexternal=:statusexternal
                                  where a.id=:id";

                    OracleParameter[] par = new OracleParameter[7];
                    par[0] = new OracleParameter("statusinternal", TicketInternalUpdate.StatusInternal);
                    par[1] = new OracleParameter("Ticketinternalaction", TicketInternalUpdate.TicketInternalAction);
                    par[2] = new OracleParameter("followup", TicketInternalUpdate.FollowUp);
                    par[3] = new OracleParameter("id", TicketInternalUpdate.Id);
                    par[4] = new OracleParameter("uuser", HttpContext.Current.Session["UserName"]);
                    par[5] = new OracleParameter("ISREDIRECT", TicketInternalUpdate.IsRedirect);
                    par[6] = new OracleParameter("statusexternal", "1");


                    da.open();
                    da.ExicuteCommand(query, par);
                    da.close();
                    return "Success";

                }
                else
                {
                    string query = @" update Ticket a set 
                                  a.udate=sysdate,
                                  a.uuser=:uuser,
                                  a.statusinternal=:statusinternal,
                                  a.Ticketinternalaction=:Ticketinternalaction,
                                  a.followup=:followup,
                                  a.ISREDIRECT=:ISREDIRECT
                                  where a.id=:id";

                    OracleParameter[] par = new OracleParameter[6];
                    par[0] = new OracleParameter("statusinternal", TicketInternalUpdate.StatusInternal);
                    par[1] = new OracleParameter("Ticketinternalaction", TicketInternalUpdate.TicketInternalAction);
                    par[2] = new OracleParameter("followup", TicketInternalUpdate.FollowUp);
                    par[3] = new OracleParameter("id", TicketInternalUpdate.Id);
                    par[4] = new OracleParameter("uuser", HttpContext.Current.Session["UserName"]);
                    par[5] = new OracleParameter("ISREDIRECT", TicketInternalUpdate.IsRedirect);


                    da.open();
                    da.ExicuteCommand(query, par);
                    da.close();
                    return "Success";

                }

            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("TicketOpiration, TicketInternalUpdate", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, TicketInternalUpdate", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
        }

        public string TicketInternalUpdateStatus(string Id)
        {
            try
            {
                string query = @" update Ticket a set 
                                  a.udate=sysdate,
                                  a.uuser=:uuser,
                                  a.statusinternal=:statusinternal
                                  where a.id=:id";
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("statusinternal", "2");
                par[1] = new OracleParameter("id", Id);
                par[2] = new OracleParameter("uuser", HttpContext.Current.Session["UserName"]);

                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("TicketOpiration, TicketInternalUpdateStatus", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, TicketInternalUpdateStatus", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
        }

        public string TicketExtarnalUpdateStatus(string Id)
        {
            try
            {
                string query = @" update Ticket a set 
                                  a.udateexternal=sysdate,
                                  a.uuserexternal=:uuserexternal,
                                  a.statusexternal=:statusexternal
                                  where a.id=:id";
                OracleParameter[] par = new OracleParameter[3];
                par[0] = new OracleParameter("statusexternal", "2");
                par[1] = new OracleParameter("id", Id);
                par[2] = new OracleParameter("uuserexternal", HttpContext.Current.Session["UserName"]);

                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("TicketOpiration, TicketInternalUpdateStatus", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, TicketInternalUpdateStatus", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
        }

        
        public string TicketExtarnalUpdate(TicketExtarnalUpdate TicketExtarnalUpdate)
        {
            try
            {
                if (TicketExtarnalUpdate.TicketExternalAction == null)
                {
                    TicketExtarnalUpdate.TicketExternalAction = "";
                }
                string query = @"  update Ticket a set 
                                  a.udateexternal=sysdate,
                                  a.uuserexternal=:uuserexternal,
                                  a.statusexternal=:statusexternal,
                                  a.ticketexternalaction=:Ticketexternalaction
                                  where a.id=:id";
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("statusexternal", TicketExtarnalUpdate.StatusExternal);
                par[1] = new OracleParameter("Ticketexternalaction", TicketExtarnalUpdate.TicketExternalAction);
                par[2] = new OracleParameter("id", TicketExtarnalUpdate.Id);
                par[3] = new OracleParameter("uuserexternal", HttpContext.Current.Session["UserName"]);
                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("TicketOpiration, TicketInternalUpdate", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, TicketInternalUpdate", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;

            }
        }



        public string TicketNightshiftAction(TicketExtarnalUpdate TicketExtarnalUpdate)
        {
            try
            {
                if (TicketExtarnalUpdate.TicketExternalAction == null)
                {
                    TicketExtarnalUpdate.TicketExternalAction = "";
                }
                if (TicketExtarnalUpdate.TicketExternalAction == "" || TicketExtarnalUpdate.TicketExternalAction == null ||
                    TicketExtarnalUpdate.StatusExternal == "" || TicketExtarnalUpdate.StatusExternal == null)
                    return "noticketstatus";
                string query = @"  update Ticket a set 
                                  a.AGENTUDATE=sysdate,
                                  a.AGENTUPDATE=:uuserexternal,
                                  a.statusexternal=:statusexternal,
                                  a.ticketexternalaction=:Ticketexternalaction
                                  where a.id=:id";
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("statusexternal", TicketExtarnalUpdate.StatusExternal);
                par[1] = new OracleParameter("Ticketexternalaction", TicketExtarnalUpdate.TicketExternalAction);
                par[2] = new OracleParameter("id", TicketExtarnalUpdate.Id);
                par[3] = new OracleParameter("uuserexternal", HttpContext.Current.Session["UserName"]);
                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("TicketOpiration, TicketInternalUpdate", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("TicketOpiration, TicketInternalUpdate", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;

            }
        }

        public string updateNightShiftTicketStatus(string id)
        {
            try
            {
                string query = @"  update Ticket a set 
                                  a.STATUSEXTERNAL=4
                                  where a.id=:id";
                OracleParameter[] par = new OracleParameter[1];
                par[0] = new OracleParameter("id", id);
                da.open();
                da.ExicuteCommand(query, par);
                da.close();
                return "Success";
            }
            catch(Exception ex)
            {                
                return ex.Message;
            }
        }

        public DataTable BindProblemType()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, a.problemname
                            FROM problemtype a";
            da.open();
            dt = da.SelectData(query, null);
            da.close();
            return dt;
        }

        public DataTable BindCallAddress()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, a.calladdress
                                FROM calladdress a";
            da.open();
            dt = da.SelectData(query, null);
            da.close();
            return dt;
        }

        public DataTable BindRouter()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, a.routername
                              FROM router a";
            da.open();
            dt = da.SelectData(query, null);
            da.close();
            return dt;
        }

        public DataTable BindQuestionType()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, a.questiontext
                              FROM questiontypes a";
            da.open();
            dt = da.SelectData(query, null);
            da.close();
            return dt;
        }

        public DataTable BindsTATUS()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, a.statusname FROM Ticketstatus a where a.id !='4'";
            da.open();
            dt = da.SelectData(query, null);
            da.close();
            return dt;
        }

        public List<TicketSearch> TicketSearch(string ServiceNo)
        {
            if (ServiceNo == "")
            {
                ServiceNo = "00000000000000000000000";
            }
            List<TicketSearch> Tickets = new List<TicketSearch>();
            DataTable dt = new DataTable();
            string query = @"select a.id,a.idate,SUBSCRIBERNAME,
                              (SELECT  w.statusname  FROM Ticketstatus w where  w.id=a.statusinternal)statusinternal,
                               (SELECT  b.statusname  FROM Ticketstatus b where b.id=a.statusexternal) statusexternal,
                             (SELECT  b.calladdress FROM calladdress b where b.id=a.calladdress)calladdress,
                             (SELECT  c.calltype FROM calltype c where c.id=a.calltype)calltype,
                             a.problemtype
                             ,(SELECT b.routername  FROM router b where b.id=a.router) router,
                             udateexternal,a.uuserexternal,
                             a.udate, a.uuser 
                             FROM Ticket a where a.servicenumber like'%'||:par1||'%' 
                                order by a.idate desc";
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("par1", ServiceNo);
            da.open();
            dt = da.SelectData(query, par);
            da.close();

            //to return the last 10 tickets 
            int ticketsCounter = 0; 

            foreach (DataRow item in dt.Rows)
            {
                if (item["statusexternal"].ToString() != "تم فتح المتابعة")
                {
                    ticketsCounter++;

                    TicketSearch Ticket = new TicketSearch();
                    Ticket.Id = Convert.ToInt32(item["id"].ToString());
                    //  Ticket.CallAddress = item["calladdress"].ToString();
                    //to return the customerName with the search tickets
                    Ticket.SubscriberName = item["SUBSCRIBERNAME"].ToString();
                    //to bind call type to GetTicketServiceNo
                    Ticket.CALLTYPE = item["calltype"].ToString();
                    Ticket.Idate = Convert.ToDateTime(item["idate"]);
                    Ticket.ProblemType = item["problemtype"].ToString();
                    Ticket.Router = item["router"].ToString();
                    Ticket.StatusExternal = item["statusexternal"].ToString();
                    Ticket.StatusInternal = item["statusinternal"].ToString();
                    Ticket.Udate = item["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(item["udate"]);
                    Ticket.UdateExternal = item["udateexternal"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(item["udateexternal"]);
                    Ticket.Uuser = item["uuser"].ToString();
                    Ticket.Uuserexternal = item["uuserexternal"].ToString();
                    Tickets.Add(Ticket);
                    if (ticketsCounter == 10)
                        return Tickets;
                }
                
            }
            return Tickets;
        }


        public List<TicketSearch> NightShiftTickets()
        {
            DateTime startdate = Convert.ToDateTime("00:00:00 AM");
            DateTime enddate = Convert.ToDateTime("08:00:00 AM");
            List<TicketSearch> Tickets = new List<TicketSearch>();
            DataTable dt = new DataTable();
            string query = @"select a.id,a.idate,SUBSCRIBERNAME,
                              (SELECT  w.statusname  FROM Ticketstatus w where  w.id=a.statusinternal)statusinternal,
                           (SELECT  b.statusname  FROM Ticketstatus b where b.id=a.statusexternal) statusexternal,
                             (SELECT  b.calladdress FROM calladdress b where b.id=a.calladdress)calladdress,
                             (SELECT  c.calltype FROM calltype c where c.id=a.calltype)calltype,
                                a.problemtype  ,(SELECT b.routername  FROM router b where b.id=a.router) router,
                         udateexternal,a.uuserexternal, a.udate, a.uuser ,a.iuser
                         FROM Ticket a where a.iuser in (select g.user_name from TBL_LOGIN_USERS g where  g.PRIVILEGE_ID =:par1)
                                           and  a.STATUSEXTERNAL =:par2 and a.calltype !=:par3
                             order by a.idate desc";
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("par1","5");
            par[1] = new OracleParameter("par2", "1");
            par[2] = new OracleParameter("par3","2");
            da.open();
            dt = da.SelectData(query, par);
            da.close();


            foreach (DataRow item in dt.Rows)
            {
                TicketSearch Ticket = new TicketSearch();
                Ticket.Id = Convert.ToInt32(item["id"].ToString());
                //  Ticket.CallAddress = item["calladdress"].ToString();
                //to return the customerName with the search tickets
                Ticket.SubscriberName = item["SUBSCRIBERNAME"].ToString();
                //to bind call type to GetTicketServiceNo
                Ticket.CALLTYPE = item["calltype"].ToString();
                Ticket.Idate = Convert.ToDateTime(item["idate"]);
                Ticket.ProblemType = item["problemtype"].ToString();
                Ticket.Router = item["router"].ToString();
                Ticket.StatusExternal = item["statusexternal"].ToString();
                Ticket.StatusInternal = item["statusinternal"].ToString();
                Ticket.Udate = item["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(item["udate"]);
                Ticket.UdateExternal = item["udateexternal"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(item["udateexternal"]);
                Ticket.Uuser = item["uuser"].ToString();
                Ticket.Uuserexternal = item["uuserexternal"].ToString();
                Tickets.Add(Ticket);
            }
            return Tickets;
        }

        public Ticket TicketSingl(string id)
        {
            Ticket ticket = new Ticket();
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, (SELECT  c.calltype FROM CALLTYPE c where c.id= a.calltype) calltype,
                               a.ani, a.subscribername, a.servicenumber,
                               a.alternativenumber,
                               (SELECT  ad.calladdress FROM calladdress ad where ad.id=a.calladdress) calladdress, a.problemtype,
                               a.Ticketdetails,
                              (SELECT r.routername  FROM router r where r.id=a.router) router,
                                a.idate, a.iuser, a.udate, a.uuser,
                                a.agentudate, a.agentupdate, a.agentunote,
                               (SELECT  w.statusname  FROM Ticketstatus w where  w.id=a.statusinternal)statusinternal,
                                a.Ticketinternalaction,
                               (SELECT  b.statusname  FROM Ticketstatus b where b.id=a.statusexternal) statusexternal,
                               a.Ticketexternalaction, a.udateexternal, a.uuserexternal,
                               a.followup,a.gender 
                          FROM ticket a where a.id=:par1";
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("par1", id);
            da.open();
            dt = da.SelectData(query, par);
            da.close();
            ticket.Id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
            ticket.CallType = dt.Rows[0]["calltype"].ToString();
            ticket.Ani = dt.Rows[0]["ani"].ToString();
            ticket.SubscriberName = dt.Rows[0]["subscribername"].ToString();
            ticket.ServiceNumber = dt.Rows[0]["servicenumber"].ToString();
            ticket.AlternativeNumber = dt.Rows[0]["alternativenumber"].ToString();   
            ticket.TicketDetails = dt.Rows[0]["Ticketdetails"].ToString();
            ticket.Iuser = dt.Rows[0]["iuser"].ToString();
            ticket.AgentUdate = dt.Rows[0]["agentudate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["agentudate"]); ;
            ticket.AgentUpdate = dt.Rows[0]["agentupdate"].ToString();
            ticket.AgentUNote = dt.Rows[0]["agentunote"].ToString();
            ticket.TicketInternalAction = dt.Rows[0]["Ticketinternalaction"].ToString();
            ticket.TicketExternalAction = dt.Rows[0]["Ticketexternalaction"].ToString();
            ticket.FollowUp = dt.Rows[0]["followup"].ToString();

           // ticket.CallAddress = dt.Rows[0]["calladdress"].ToString();
            ticket.Idate = Convert.ToDateTime(dt.Rows[0]["idate"]);
            ticket.ProblemType = dt.Rows[0]["problemtype"].ToString();
            ticket.Router = dt.Rows[0]["router"].ToString();
            ticket.StatusExternal = dt.Rows[0]["statusexternal"].ToString();
            ticket.StatusInternal = dt.Rows[0]["statusinternal"].ToString();
            ticket.Udate = dt.Rows[0]["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udate"]);
            ticket.UdateExternal = dt.Rows[0]["udateexternal"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udateexternal"]);
            ticket.Uuser = dt.Rows[0]["uuser"].ToString();
            ticket.UuserExternal = dt.Rows[0]["uuserexternal"].ToString();
            ticket.Gender = dt.Rows[0]["gender"].ToString();


            return ticket;
        }

        public List<Ticket> TicketlistIntearnal(string status , string CallType)
        {
            List<Ticket> tickets = new List<Ticket>();
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, (SELECT  c.calltype FROM CALLTYPE c where c.id = a.calltype) calltype,
                               a.ani, a.subscribername, a.servicenumber,
                               a.alternativenumber,
                               (SELECT  ad.calladdress FROM calladdress ad where ad.id = a.calladdress) calladdress, a.problemtype,
                               a.Ticketdetails,
                               (SELECT r.routername FROM router r where r.id = a.router) router,
                                a.idate, a.iuser, a.udate, a.uuser,
                                a.agentudate, a.agentupdate, a.agentunote,
                               (SELECT  w.statusname FROM Ticketstatus w where w.id = a.statusinternal)statusinternal,
                                a.Ticketinternalaction,
                               (SELECT  b.statusname FROM Ticketstatus b where b.id = a.statusexternal) statusexternal,
                               a.Ticketexternalaction, a.udateexternal, a.uuserexternal,
                               a.followup
                          FROM ticket a where a.statusinternal =:par1 and a.calltype=:par2 order by idate asc"; ;
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("par1", status);
            par[1] = new OracleParameter("par2", CallType);

            da.open();
            dt = da.SelectData(query, par);
            da.close();

            foreach (DataRow item in dt.Rows)
            {
                Ticket ticket = new Ticket();
                ticket.Id = Convert.ToInt32(item["id"].ToString());
                ticket.CallType = item["calltype"].ToString();
                ticket.Ani = item["ani"].ToString();
                ticket.SubscriberName = item["subscribername"].ToString();
                ticket.ServiceNumber = item["servicenumber"].ToString();
                ticket.AlternativeNumber = item["alternativenumber"].ToString();
                ticket.TicketDetails = item["Ticketdetails"].ToString();
                ticket.Iuser = item["iuser"].ToString();
                ticket.AgentUdate =  dt.Rows[0]["agentudate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["agentudate"]);
                ticket.AgentUpdate = item["agentupdate"].ToString();
                ticket.AgentUNote = item["agentunote"].ToString();
                ticket.TicketInternalAction = item["Ticketinternalaction"].ToString();
                ticket.TicketExternalAction = item["Ticketexternalaction"].ToString();
                ticket.FollowUp = item["followup"].ToString();
                ticket.CallAddress = item["calladdress"].ToString();
                ticket.Idate = Convert.ToDateTime(item["idate"]);
                ticket.ProblemType = item["problemtype"].ToString();
                ticket.Router = item["router"].ToString();
                ticket.StatusExternal = item["statusexternal"].ToString();
                ticket.StatusInternal = item["statusinternal"].ToString();
                ticket.Udate =  dt.Rows[0]["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udate"]) ;
                ticket.UdateExternal = item["udateexternal"] == DBNull.Value ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udateexternal"]);
                ticket.Uuser = item["uuser"].ToString();
                ticket.UuserExternal = item["udateexternal"].ToString();
                tickets.Add(ticket);
            }
            return tickets;
        }


        public List<Ticket> TicketReport(Nullable<DateTime> from, Nullable<DateTime> to)
        {
            if (from == null || to == null)
            {
                from = DateTime.Now;
                to = DateTime.Now;
            }
            List<Ticket> tickets = new List<Ticket>();
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, (SELECT  c.calltype FROM CALLTYPE c where c.id = a.calltype) calltype,
                               a.ani, a.subscribername, a.servicenumber,
                               a.alternativenumber,
                               (SELECT  ad.calladdress FROM calladdress ad where ad.id = a.calladdress) calladdress, a.problemtype,
                               a.questiontext,a.Ticketdetails,
                               (SELECT r.routername FROM router r where r.id = a.router) router,
                                a.idate, a.iuser, a.udate, a.uuser,
                                a.agentudate, a.agentupdate, a.agentunote,
                               (SELECT  w.statusname FROM Ticketstatus w where w.id = a.statusinternal)statusinternal,
                                a.Ticketinternalaction,
                               (SELECT  b.statusname FROM Ticketstatus b where b.id = a.statusexternal) statusexternal,
                               a.Ticketexternalaction, a.udateexternal, a.uuserexternal,
                               a.followup
                          FROM ticket a " +
                   " where a.idate between TO_DATE('" + Convert.ToDateTime(from).ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS')"+
                    " AND TO_DATE('" + Convert.ToDateTime(to).ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS')"+
                   " order by idate desc"; 

            da.open();
            dt = da.SelectData(query, null);
            da.close();

            foreach (DataRow item in dt.Rows)
            {
                Ticket ticket = new Ticket();
                ticket.Id = Convert.ToInt32(item["id"].ToString());
                ticket.CallType = item["calltype"].ToString();
                ticket.Ani = item["ani"].ToString();
                ticket.SubscriberName = item["subscribername"].ToString();
                ticket.ServiceNumber = item["servicenumber"].ToString();
                ticket.AlternativeNumber = item["alternativenumber"].ToString();
                ticket.TicketDetails = item["Ticketdetails"].ToString();
                ticket.Iuser = item["iuser"].ToString();
                ticket.AgentUdate = dt.Rows[0]["agentudate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["agentudate"]);
                ticket.AgentUpdate = item["agentupdate"].ToString();
                ticket.AgentUNote = item["agentunote"].ToString();
                ticket.TicketInternalAction = item["Ticketinternalaction"].ToString();
                ticket.TicketExternalAction = item["Ticketexternalaction"].ToString();
                ticket.FollowUp = item["followup"].ToString();
                ticket.CallAddress = item["calladdress"].ToString();
                ticket.Idate = Convert.ToDateTime(item["idate"]);
                ticket.ProblemType = item["problemtype"].ToString();
                ticket.questiontext = item["questiontext"].ToString();
                ticket.Router = item["router"].ToString();
                ticket.StatusExternal = item["statusexternal"].ToString();
                ticket.StatusInternal = item["statusinternal"].ToString();
                ticket.Udate = dt.Rows[0]["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udate"]);
                ticket.UdateExternal = item["udateexternal"] == DBNull.Value ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(item["udateexternal"]);
                ticket.Uuser = item["uuser"].ToString();
                ticket.UuserExternal = item["uuserexternal"].ToString();
                tickets.Add(ticket);
            }

            return tickets;
        }


        public List<Ticket> TicketlistExtarnal( string CallType,string Status)
        {
            List<Ticket> tickets = new List<Ticket>();
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, (SELECT  c.calltype FROM CALLTYPE c where c.id = a.calltype) calltype,
                               a.ani, a.subscribername, a.servicenumber,
                               a.alternativenumber,
                               (SELECT  ad.calladdress FROM calladdress ad where ad.id = a.calladdress) calladdress, a.problemtype,
                               a.Ticketdetails,
                               (SELECT r.routername FROM router r where r.id = a.router) router,
                                a.idate, a.iuser, a.udate, a.uuser,
                                a.agentudate, a.agentupdate, a.agentunote,
                               (SELECT  w.statusname FROM Ticketstatus w where w.id = a.statusinternal)statusinternal,
                                a.Ticketinternalaction,
                               (SELECT  b.statusname FROM Ticketstatus b where b.id = a.statusexternal) statusexternal,
                               a.Ticketexternalaction, a.udateexternal, a.uuserexternal,
                               a.followup
                          FROM ticket a where  a.calltype=:par2 and a.uuser is null and (a.statusinternal=:par3 or a.statusexternal=:par3)
                                                and (a.iuser not in (select g.user_name from TBL_LOGIN_USERS g where g.PRIVILEGE_ID =:par1))
                                                order by idate asc";
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("par2", CallType);
            par[1] = new OracleParameter("par3", Status);
            par[2] = new OracleParameter("par1", "5");
            da.open();
            dt = da.SelectData(query, par);
            da.close();         

            foreach (DataRow item in dt.Rows)
            {
                Ticket ticket = new Ticket();
                ticket.Id = Convert.ToInt32(item["id"].ToString());
                ticket.CallType = item["calltype"].ToString();
                ticket.Ani = item["ani"].ToString();
                ticket.SubscriberName = item["subscribername"].ToString();
                ticket.ServiceNumber = item["servicenumber"].ToString();
                ticket.AlternativeNumber = item["alternativenumber"].ToString();
                ticket.TicketDetails = item["Ticketdetails"].ToString();
                ticket.Iuser = item["iuser"].ToString();
                ticket.AgentUdate = dt.Rows[0]["agentudate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["agentudate"]);
                ticket.AgentUpdate = item["agentupdate"].ToString();
                ticket.AgentUNote = item["agentunote"].ToString();
                ticket.TicketInternalAction = item["Ticketinternalaction"].ToString();
                ticket.TicketExternalAction = item["Ticketexternalaction"].ToString();
                ticket.FollowUp = item["followup"].ToString();
                ticket.CallAddress = item["calladdress"].ToString();
                ticket.Idate = Convert.ToDateTime(item["idate"]);
                ticket.ProblemType = item["problemtype"].ToString();
                ticket.Router = item["router"].ToString();
                ticket.StatusExternal = item["statusexternal"].ToString();
                ticket.StatusInternal = item["statusinternal"].ToString();
                ticket.Udate = dt.Rows[0]["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udate"]);
                ticket.UdateExternal = dt.Rows[0]["udateexternal"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udateexternal"]);
                ticket.Uuser = item["uuser"].ToString();
                ticket.UuserExternal = item["uuserexternal"].ToString();
                tickets.Add(ticket);
            }
            return tickets;
        }



        public List<Ticket> TicketlistExtarnaloption2(string CallType, string Status)
        {
            List<Ticket> tickets = new List<Ticket>();
            DataTable dt = new DataTable();
            string query = @"SELECT a.id, (SELECT  c.calltype FROM CALLTYPE c where c.id = a.calltype) calltype,
                               a.ani, a.subscribername, a.servicenumber,
                               a.alternativenumber,
                               (SELECT  ad.calladdress FROM calladdress ad where ad.id = a.calladdress) calladdress, a.problemtype,
                               a.Ticketdetails,
                               (SELECT r.routername FROM router r where r.id = a.router) router,
                                a.idate, a.iuser, a.udate, a.uuser,
                                a.agentudate, a.agentupdate, a.agentunote,
                               (SELECT  w.statusname FROM Ticketstatus w where w.id = a.statusinternal)statusinternal,
                                a.Ticketinternalaction,
                               (SELECT  b.statusname FROM Ticketstatus b where b.id = a.statusexternal) statusexternal,
                               a.Ticketexternalaction, a.udateexternal, a.uuserexternal,
                               a.followup
                          FROM ticket a where  a.calltype=:par2 and a.uuser is null and (a.statusinternal=:par3 or a.statusexternal=:par3)                                                
                                                order by idate asc";
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("par2", CallType);
            par[1] = new OracleParameter("par3", Status);
            da.open();
            dt = da.SelectData(query, par);
            da.close();

            foreach (DataRow item in dt.Rows)
            {
                Ticket ticket = new Ticket();
                ticket.Id = Convert.ToInt32(item["id"].ToString());
                ticket.CallType = item["calltype"].ToString();
                ticket.Ani = item["ani"].ToString();
                ticket.SubscriberName = item["subscribername"].ToString();
                ticket.ServiceNumber = item["servicenumber"].ToString();
                ticket.AlternativeNumber = item["alternativenumber"].ToString();
                ticket.TicketDetails = item["Ticketdetails"].ToString();
                ticket.Iuser = item["iuser"].ToString();
                ticket.AgentUdate = dt.Rows[0]["agentudate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["agentudate"]);
                ticket.AgentUpdate = item["agentupdate"].ToString();
                ticket.AgentUNote = item["agentunote"].ToString();
                ticket.TicketInternalAction = item["Ticketinternalaction"].ToString();
                ticket.TicketExternalAction = item["Ticketexternalaction"].ToString();
                ticket.FollowUp = item["followup"].ToString();
                ticket.CallAddress = item["calladdress"].ToString();
                ticket.Idate = Convert.ToDateTime(item["idate"]);
                ticket.ProblemType = item["problemtype"].ToString();
                ticket.Router = item["router"].ToString();
                ticket.StatusExternal = item["statusexternal"].ToString();
                ticket.StatusInternal = item["statusinternal"].ToString();
                ticket.Udate = dt.Rows[0]["udate"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udate"]);
                ticket.UdateExternal = dt.Rows[0]["udateexternal"].Equals(System.DBNull.Value) ? Convert.ToDateTime("01/01/2000") : Convert.ToDateTime(dt.Rows[0]["udateexternal"]);
                ticket.Uuser = item["uuser"].ToString();
                ticket.UuserExternal = item["uuserexternal"].ToString();
                tickets.Add(ticket);
            }
            return tickets;
        }

    }
}