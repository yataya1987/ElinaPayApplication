using System;
using System.Collections.Generic;
using System.Linq;
using BnetApplication.Data_Access_Layer;
using System.Data;
using System.Data.OracleClient;
using Logger;
using System.Diagnostics;
using System.Configuration;
using System.Web.Security;
using System.Web;

namespace BnetApplication.Models
{
    public class Opiration
    {
        DataAccessLayer dal = new DataAccessLayer();

        public DataTable SelectUserType()
        {
            DataTable dt = new DataTable();
            string query = "SELECT PRIVILEGE_ID, PRIVILEGE_NAME FROM TBL_LOGIN_PRIVILEGES WHERE (PRIVILEGE_STATUS = 'Enabled')";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;

        }


        /****************************************************************************************************/
        #region
        public DataTable SelectPRIVILEGE()
        {
            DataTable dt = new DataTable();
            string query = "SELECT PRIVILEGE_ID, PRIVILEGE_NAME FROM TBL_LOGIN_PRIVILEGES WHERE (PRIVILEGE_STATUS = 'Enabled')";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;

        }
        public string CountAccount()
        {
            DataTable dt = new DataTable();
            string count;
            string query = @"SELECT count(*) counter
                              FROM tbl_login_accounts  a
                            where  a.account_status='Enabled'";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;

        }
        public DataTable SelectAccount()
        {
            DataTable dt = new DataTable();
            string query = "SELECT ACCOUNT_ID, ACCOUNT_NAME, ACCOUNT_STATUS FROM TBL_LOGIN_ACCOUNTS ORDER BY ACCOUNT_ID";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;

        }

        public DataTable SelectAccount(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT ACCOUNT_ID, ACCOUNT_NAME, ACCOUNT_STATUS FROM TBL_LOGIN_ACCOUNTS where ACCOUNT_ID =:par1";
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("par1", id);
            dal.open();
            dt = dal.SelectData(query, par);
            dal.close();
            return dt;

        }


        public string CreateAccount(string AccountName)
        {
            string accountName = string.Empty;

            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/
                accountName = AccountName;
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parAccountName", accountName);
                cmd.Parameters.AddWithValue("parAddedUser", HttpContext.Current.Session["UserName"].ToString());
                /***************************************************************************************/
                string sql = "insert into TBL_LOGIN_ACCOUNTS" +
                             "( ACCOUNT_ID, ACCOUNT_NAME," +
                             "  IDATE, IUSER," +
                             "  ACCOUNT_STATUS, ACCOUNT_STATUS_ACTION, ACCOUNT_STATUS_DATE)" +
                             " Values " +
                             "( Login_Accounts_AccountID_seq.nextval, :parAccountName," +
                             "  sysdate, :parAddedUser," +
                             "  'Enabled', :parAddedUser, sysdate)";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();

                ErrorLogs.WriteLogDB("Inserted Account", "This user has inserted Account Name: " + accountName, HttpContext.Current.Session["UserName"].ToString(), "BNet", "NablusMConnection");

                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("AccountsManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("AccountsManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;

            }
            finally
            {
                conn.Close();
            }
        }


        public string EditAccount(string accountID, string accountName, string Status)
        {

            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();

            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /****************************************************************************/
                cmd.Parameters.AddWithValue("parAccountID", accountID);
                cmd.Parameters.AddWithValue("parAccountName", accountName);
                cmd.Parameters.AddWithValue("parAccountStatus", Status);
                cmd.Parameters.AddWithValue("parUpdatedUser", HttpContext.Current.Session["UserName"].ToString());
                /****************************************************************************/
                cmd.CommandText = "UPDATE TBL_LOGIN_ACCOUNTS" +
                                  " SET" +
                                  " ACCOUNT_NAME = :parAccountName," +
                                  " ACCOUNT_STATUS = :parAccountStatus," +
                                  " UDATE = sysdate," +
                                  " UUSER = :parUpdatedUser" +
                                  " WHERE (ACCOUNT_ID = :parAccountID)";

                cmd.ExecuteNonQuery();

                ErrorLogs.WriteLogDB("Updated Account", "This user has updated Account Name: " + accountID + " , " + accountName + " , " + Status, HttpContext.Current.Session["UserName"].ToString(), "BNet", "NablusMConnection");
                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("Opiration, EditAccount", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("Opiration, EditAccount", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }


        }
        #endregion
        /***************************************************************************************************/
        #region
        public string CountSkill()
        {
            DataTable dt = new DataTable();
            string count;
            string query = @"SELECT count(*)  counter
                              FROM tbl_login_skills a
                            where  a.skill_status='Enabled'";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;

        }
        public DataTable SelectSkill()
        {
            DataTable dt = new DataTable();
            string query = "SELECT  TBL_LOGIN_SKILLS.SKILL_ID, TBL_LOGIN_SKILLS.SKILL_NAME, TBL_LOGIN_SKILLS.SKILL_STATUS, TBL_LOGIN_ACCOUNTS.ACCOUNT_NAME, TBL_LOGIN_ACCOUNTS.ACCOUNT_ID FROM     TBL_LOGIN_ACCOUNTS, TBL_LOGIN_SKILLS WHERE  TBL_LOGIN_ACCOUNTS.ACCOUNT_ID = TBL_LOGIN_SKILLS.ACCOUNT_ID  ORDER BY TBL_LOGIN_SKILLS.SKILL_ID ";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;

        }

        public DataTable SelectSkill(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT  TBL_LOGIN_SKILLS.SKILL_ID, TBL_LOGIN_SKILLS.SKILL_NAME, TBL_LOGIN_SKILLS.SKILL_STATUS, TBL_LOGIN_SKILLS.account_id, TBL_LOGIN_ACCOUNTS.ACCOUNT_ID FROM     TBL_LOGIN_ACCOUNTS, TBL_LOGIN_SKILLS WHERE  TBL_LOGIN_ACCOUNTS.ACCOUNT_ID = TBL_LOGIN_SKILLS.ACCOUNT_ID and TBL_LOGIN_SKILLS.SKILL_ID=:par1 ";
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("par1", id);
            dal.open();
            dt = dal.SelectData(query, par);
            dal.close();
            return dt;

        }

        public string CreateSkill(string accountID, string SkillName)
        {

            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parAccountID", accountID);
                cmd.Parameters.AddWithValue("parSkillName", SkillName);
                cmd.Parameters.AddWithValue("parAddedUser", HttpContext.Current.Session["UserName"].ToString());
                /***************************************************************************************/
                string sql = "insert into TBL_LOGIN_SKILLS" +
                             "( SKILL_ID, SKILL_NAME," +
                             "  IDATE, IUSER," +
                             "  SKILL_STATUS, SKILL_STATUS_ACTION, SKILL_STATUS_DATE," +
                             "  ACCOUNT_ID)" +
                             " Values " +
                             "( Login_Skills_SkillID_seq.nextval, :parSkillName," +
                             "  sysdate, :parAddedUser," +
                             "  'Enabled', :parAddedUser, sysdate," +
                             "  :parAccountID)";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();

                ErrorLogs.WriteLogDB("Inserted Skill", "This user has inserted Skill Name: " + SkillName + " , " + accountID, HttpContext.Current.Session["UserName"].ToString(), "BNet", "NablusMConnection");
                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("SkillsManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("SkillsManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public string EditSkill(string skillID, string accountID, string SkillName, string status)
        {

            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();

            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /****************************************************************************/
                cmd.Parameters.AddWithValue("parSkillID", skillID);
                cmd.Parameters.AddWithValue("parSkillName", SkillName);
                cmd.Parameters.AddWithValue("parSkillStatus", status);
                cmd.Parameters.AddWithValue("parAccountName", accountID);
                cmd.Parameters.AddWithValue("parUpdatedUser", HttpContext.Current.Session["UserName"].ToString());
                /****************************************************************************/
                cmd.CommandText = "UPDATE TBL_LOGIN_SKILLS" +
                                  " SET" +
                                  " SKILL_NAME = :parSkillName," +
                                  " SKILL_STATUS = :parSkillStatus," +
                                  " ACCOUNT_ID = :parAccountName," +
                                  " UDATE = sysdate," +
                                  " UUSER = :parUpdatedUser" +
                                  " WHERE (SKILL_ID = :parSkillID)";

                cmd.ExecuteNonQuery();

                ErrorLogs.WriteLogDB("Updated Skill", "This user has updated Skill Name: " + skillID + " , " + SkillName + " , " + accountID + " , " + accountID, HttpContext.Current.Session["UserName"].ToString(), "BNet", "NablusMConnection");
                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("SkillsManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("SkillsManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion
        /***************************************************************************************************/
        public DataTable SelectProblemType()
        {
            DataTable dt = new DataTable();
            string query = "SELECT  PROBLEMTYPE.ID, PROBLEMTYPE.PROBLEMNAME, PROBLEMTYPE.STATUS FROM    PROBLEMTYPE";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;
        }
         public DataTable FindProblemType(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT PROBLEMTYPE.ID, PROBLEMTYPE.PROBLEMNAME, PROBLEMTYPE.STATUS FROM  PROBLEMTYPE where PROBLEMTYPE.ID ="+id;
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;
        }
        public string EditProblemType(ProblemType problemType)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/

                /***************************************************************************************/
                string sql = "update PROBLEMTYPE set PROBLEMNAME = '" + problemType.ProblemName+"' where id= " + problemType.ProblemTypeID;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string DeleteProblemType(string id)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/

                /***************************************************************************************/
                string sql = "delete from problemtype where id=" + id;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string CreatProblemType(ProblemType problemType)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parProblemName", problemType.ProblemName);
                cmd.Parameters.AddWithValue("parProblemStatus", 1);
                /***************************************************************************************/
                string sql = "insert into PROBLEMTYPE" +
                             "( ID, PROBLEMNAME,STATUS)" +
                             " Values " +
                             "( PROBLEMTYPE_SEQ.nextval, :parProblemName,:parProblemStatus)";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string CountProblemTypes()
        {
            DataTable dt = new DataTable();
            string count;
            string query = @"SELECT count(*) counter FROM PROBLEMTYPE ";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;
        }

        /***************************************************************************************************/
        public DataTable SelectRouterType()
        {
            DataTable dt = new DataTable();
            string query = "SELECT  ROUTER.ID, ROUTER.ROUTERNAME FROM  ROUTER";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;
        }
        public DataTable FindRouterType(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT router.ID, router.ROUTERNAME FROM  router where ROUTER.ID =" + id;
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;
        }
        public string CreatRouterType(RouterType routerType)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parRouterName", routerType.RouterName);
                /***************************************************************************************/
                string sql = "insert into ROUTER" +
                             "( ID, ROUTERNAME)" +
                             " Values " +
                             "( ROUTER_SEQ.nextval, :parRouterName)";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string EditRouterType(RouterType routerType)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/

                /***************************************************************************************/
                string sql = "update ROUTER set ROUTERNAME = '" + routerType.RouterName + "' where id= " + routerType.ID;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string DeleteRouterType(string id)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/

                /***************************************************************************************/
                string sql = "delete from router where id=" + id;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string CountRouterTypes()
        {
            DataTable dt = new DataTable();
            string count;
            string query = @"SELECT count(*) counter FROM ROUTER ";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;
        }
        /***************************************************************************************************/
        public DataTable SelectQuestionType()
        {
            DataTable dt = new DataTable();
            string query = "SELECT  QUESTIONTYPES.ID, QUESTIONTYPES.QUESTIONTEXT FROM  QUESTIONTYPES";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;
        }
        public DataTable FindQuestionType(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT QUESTIONTYPES.ID, QUESTIONTYPES.QUESTIONTEXT FROM  QUESTIONTYPES where QUESTIONTYPES.ID =" + id;
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;
        }
        public string CreatQuestionType(QuestionType questionType)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parQuestionText", questionType.QuestionText);
                /***************************************************************************************/
                string sql = "insert into QUESTIONTYPES" +
                             "( ID, QUESTIONTEXT)" +
                             " Values " +
                             "( QUESTIONTYPES_SEQ.nextval, :parQuestionText)";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string EditQuestionType(QuestionType questionType)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/

                /***************************************************************************************/
                string sql = "update QUESTIONTYPES set QUESTIONTEXT = '" + questionType.QuestionText + "' where id= " + questionType.ID;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string DeleteQuestionType(string id)
        {
            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************************************************************************/

                /***************************************************************************************/
                string sql = "delete from QUESTIONTYPES where id=" + id;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return "done";
        }
        public string CountQuestionTypes()
        {
            DataTable dt = new DataTable();
            string count;
            string query = @"SELECT count(*) counter FROM QUESTIONTYPES ";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;
        }
        /***************************************************************************************************/
        #region
        public string CountGROUPS()
        {
            DataTable dt = new DataTable();
            string count;
            string query = @"SELECT count(*) counter
                              FROM tbl_login_groups  a
                            where  a.group_status='Enabled'";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;

        }
        

        public DataTable SelectGROUPS(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT a.group_id, a.group_name, a.idate, a.iuser, a.udate, a.uuser,  a.group_status, a.group_status_action, a.group_status_date, a.privilege_id, a.skill_id, a.account_id  FROM tbl_login_groups a where a.GROUP_ID = :par1 ";
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("par1", id);
            dal.open();
            dt = dal.SelectData(query, par);
            dal.close();
            return dt;

        }

        public DataTable SelectGROUPS()
        {
            DataTable dt = new DataTable();
            string query = "SELECT TBL_LOGIN_GROUPS.GROUP_ID, TBL_LOGIN_GROUPS.GROUP_NAME, TBL_LOGIN_GROUPS.GROUP_STATUS, TBL_LOGIN_PRIVILEGES.PRIVILEGE_NAME, TBL_LOGIN_ACCOUNTS.ACCOUNT_NAME FROM TBL_LOGIN_PRIVILEGES, TBL_LOGIN_GROUPS, TBL_LOGIN_ACCOUNTS WHERE TBL_LOGIN_PRIVILEGES.PRIVILEGE_ID = TBL_LOGIN_GROUPS.PRIVILEGE_ID AND TBL_LOGIN_GROUPS.ACCOUNT_ID = TBL_LOGIN_ACCOUNTS.ACCOUNT_ID ORDER BY TBL_LOGIN_GROUPS.GROUP_ID";
            dal.open();
            dt = dal.SelectData(query, null);
            dal.close();
            return dt;

        }

        public string CreateGROUPS(string groupName, string groupPrivilege, string groupAccount, string groupSkill)
        {

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            OracleCommand cmd = new OracleCommand();

            try
            {
                cmd.Connection = conn;
                conn.Open();

                /***************************************************************************************/

                //groupSkill = ddlSkill_Insert.SelectedValue;           
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parGroupName", groupName);
                cmd.Parameters.AddWithValue("parGroupPrivilege", groupPrivilege);
                cmd.Parameters.AddWithValue("parGroupAccount", groupAccount);
                //cmd.Parameters.AddWithValue("parGroupSkill", groupSkill);
                cmd.Parameters.AddWithValue("parAddedUser", HttpContext.Current.Session["UserName"].ToString());
                /***************************************************************************************/
                string sql = "insert into TBL_LOGIN_GROUPS" +
                             "( GROUP_ID, GROUP_NAME," +
                             "  IDATE, IUSER," +
                             "  GROUP_STATUS, GROUP_STATUS_ACTION, GROUP_STATUS_DATE," +
                             "  PRIVILEGE_ID, ACCOUNT_ID)" +
                             " Values " +
                             "( Login_Groups_GroupID_seq.nextval, :parGroupName," +
                             "  sysdate, :parAddedUser," +
                             "  'Enabled', :parAddedUser, sysdate," +
                             "  :parGroupPrivilege, :parGroupAccount)";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();

                ErrorLogs.WriteLogDB("Inserted Group", "This user has inserted Group Name: " + groupName + " , " + groupPrivilege + " , " + groupAccount, HttpContext.Current.Session["UserName"].ToString(), "NablusM", "ConnJSC");

                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("Admin_GroupsManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "NablusM", ex);
                ErrorLogs.WriteErrorLogsXML("Admin_GroupsManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "NablusM", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public string EditGROUPS(string groupID, string groupName, string groupPrivilege, string groupAccount, string groupSkill, string GroupStatus)
        {


            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();

            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /**********************************************************************************/
                cmd.Parameters.AddWithValue("parGroupID", groupID);
                cmd.Parameters.AddWithValue("parGroupName", groupName);
                cmd.Parameters.AddWithValue("parGroupStatus", GroupStatus);
                cmd.Parameters.AddWithValue("parGroupPrivilege", groupPrivilege);
                // cmd.Parameters.AddWithValue("parGroupSkill", ddlGroupSkill.SelectedValue);             
                cmd.Parameters.AddWithValue("parUpdatedUser", HttpContext.Current.Session["UserName"].ToString());
                /**********************************************************************************/
                cmd.CommandText = "UPDATE TBL_LOGIN_GROUPS" +
                                  " SET" +
                                  " GROUP_NAME = :parGroupName," +
                                  " GROUP_STATUS = :parGroupStatus," +
                                  " GROUP_STATUS_ACTION = :parUpdatedUser, " +
                                  " GROUP_STATUS_DATE = sysdate, " +
                                  " UDATE = sysdate," +
                                  " UUSER = :parUpdatedUser," +
                                  " PRIVILEGE_ID = :parGroupPrivilege" +
                                  // " SKILL_ID = :parGroupSkill" +
                                  " WHERE (GROUP_ID = :parGroupID)";

                cmd.ExecuteNonQuery();

                ErrorLogs.WriteLogDB("Updated Group", "This user has updated Group Name: " + groupID + " , " + groupName + " , " + groupPrivilege + " , " + GroupStatus, HttpContext.Current.Session["UserName"].ToString(), "JSC_System", "ConnJSC");
                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("GroupsManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "JSC_System", ex);
                ErrorLogs.WriteErrorLogsXML("GroupsManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "JSC_System", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion
        /***************************************************************************************************/
        #region
        public string EnableorDisableUsers(string UserID, string status)
        {

            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();

            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();

                cmd.Parameters.AddWithValue("parUserID", UserID);
                cmd.Parameters.AddWithValue("parUpdatedUser", HttpContext.Current.Session["UserName"].ToString());

                if (status == "1")
                {
                    cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                 " SET" +
                                 " user_status = 'Enabled' ,uuser=:parUpdatedUser,udate=sysdate" +
                                 "  WHERE (USER_ID = :parUserID)";
                }
                else
                {
                    cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                " SET" +
                                " user_status = 'Locked',uuser=:parUpdatedUser,udate=sysdate" +
                                " WHERE (USER_ID = :parUserID)";
                }

                cmd.ExecuteNonQuery();
                return "done";

                ErrorLogs.WriteLogDB("Updated User", "This user has Enable User Name: " + UserID, HttpContext.Current.Session["UserName"].ToString(), "BNet", "NablusMConnection");
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("UsersManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("UsersManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }
        public string CountUser()
        {
            DataTable dt = new DataTable();
            string count;
            string query = "SELECT count(*) counter FROM TBL_LOGIN_USERS WHERE (user_status = 'Enabled')";
            dal.open();
            dt = dal.SelectData(query, null);

            dal.close();
            count = dt.Rows[0]["counter"].ToString();
            return count;

        }

        public string CreateUsers(string UserName, string UserPass, string UserPrivilege, string UserAuthType, string UserSkill, string UserEmail, string UserMobile, string UserIP)
        {

            string userName, userPass, userPrivilege, userAuthType, userAccount, userSkill, userEmail, userMobile, userIP;


            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();

            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                if (UserAuthType == "LDAP")
                {
                    userPass = "0";
                    userAuthType = UserAuthType;

                }
                else
                {
                    string hashedpass =FormsAuthentication.HashPasswordForStoringInConfigFile(UserPass, "sha1");
                    userPass = hashedpass;
                    userAuthType = UserAuthType;
                }
                userName = UserName.Trim().ToLower();
                userPrivilege = UserPrivilege;
           
                userSkill = UserSkill;
                userEmail = UserEmail;
                userMobile = UserMobile;
                userIP = UserIP;
                /***************************************************************************************/
                cmd.Parameters.AddWithValue("parUserName", userName);
                cmd.Parameters.AddWithValue("parPassword", userPass);
                cmd.Parameters.AddWithValue("parAuthType", userAuthType);
                cmd.Parameters.AddWithValue("parUserPrivilege", userPrivilege);
                // cmd.Parameters.AddWithValue("parUserAccount", userAccount);
                cmd.Parameters.AddWithValue("parUserSkill", userSkill);
                cmd.Parameters.AddWithValue("parUserEmail", userEmail);
                cmd.Parameters.AddWithValue("parUserMobile", userMobile);
                cmd.Parameters.AddWithValue("parUserIP", userIP);

                cmd.Parameters.AddWithValue("parAddedUser",HttpContext.Current. Session["UserName"].ToString());
                /***************************************************************************************/
                string sql = "insert into TBL_LOGIN_USERS" +
                             "( USER_ID, USER_NAME, USER_PASS, USER_AUTH," +
                             "  USER_IP, USER_EMAIL, USER_MOBILE," +
                             "  IDATE, IUSER," +
                             "  USER_STATUS, USER_STATUS_ACTION, USER_STATUS_DATE," +
                             "  PRIVILEGE_ID, SKILL_ID," +
                             "  LAST_PASSWORD_CHANGE, LOGIN_ATTEMPT_DATE , LAST_LOGIN_DATE)" +
                             " Values " +
                             "( Login_Users_UserID_seq.nextval, :parUserName, :parPassword, :parAuthType," +
                             "  :parUserIP, :parUserEmail, :parUserMobile," +
                             "  sysdate, :parAddedUser," +
                             "  'Enabled', :parAddedUser, sysdate," +
                             "  :parUserPrivilege, :parUserSkill," +
                             "  sysdate-200, sysdate-200 , sysdate)";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd.Parameters.Clear();

                ErrorLogs.WriteLogDB("Inserted User", "This user has inserted User Name: " + userName + " , Auth Type: " + userAuthType + " , Privilege: " + userPrivilege + " , Skill: " + userSkill, HttpContext.Current.Session["UserName"].ToString(), "BNet", "NablusMConnection");

                //Response.Redirect("~/Admin/UsersManagment.aspx", false);
                return "done";
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("UsersManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("UsersManagment Page, btnInsert_Click", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, HttpContext.Current.Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable SelectUsers(string username)
        {
            if (username == null)
            {
                DataTable dt = new DataTable();
                string query = "SELECT        TBL_LOGIN_USERS.USER_ID, TBL_LOGIN_USERS.USER_NAME, TBL_LOGIN_USERS.USER_PASS, TBL_LOGIN_USERS.USER_AUTH, " +
                                            " TBL_LOGIN_USERS.USER_IP, TBL_LOGIN_USERS.USER_EMAIL, TBL_LOGIN_USERS.USER_MOBILE, TBL_LOGIN_USERS.USER_STATUS, " +
                                            " TBL_LOGIN_PRIVILEGES.PRIVILEGE_NAME " +
                            "    FROM            TBL_LOGIN_USERS, TBL_LOGIN_PRIVILEGES " +
                            "    WHERE        TBL_LOGIN_USERS.PRIVILEGE_ID = TBL_LOGIN_PRIVILEGES.PRIVILEGE_ID ";
               
                dal.open();
                dt = dal.SelectData(query, null);
                dal.close();
                return dt;
            }
            else
            {
                DataTable dt = new DataTable();
                string query = "SELECT        TBL_LOGIN_USERS.USER_ID, TBL_LOGIN_USERS.USER_NAME, TBL_LOGIN_USERS.USER_PASS, TBL_LOGIN_USERS.USER_AUTH, " +
                                            " TBL_LOGIN_USERS.USER_IP, TBL_LOGIN_USERS.USER_EMAIL, TBL_LOGIN_USERS.USER_MOBILE, TBL_LOGIN_USERS.USER_STATUS, " +
                                            " TBL_LOGIN_PRIVILEGES.PRIVILEGE_NAME " +
                            "    FROM            TBL_LOGIN_USERS, TBL_LOGIN_PRIVILEGES " +
                            "    WHERE        TBL_LOGIN_USERS.PRIVILEGE_ID = TBL_LOGIN_PRIVILEGES.PRIVILEGE_ID and TBL_LOGIN_USERS.USER_NAME like '%'||:par1||'%'";
                OracleParameter[] par = new OracleParameter[1];
                par[0] = new OracleParameter("par1", username);
                dal.open();
                dt = dal.SelectData(query, par);
                dal.close();
                return dt;
            }
        
        }

        public DataTable SelectLastUsers()
        {
                DataTable dt = new DataTable();
                string query = @"SELECT        TBL_LOGIN_USERS.USER_ID, TBL_LOGIN_USERS.USER_NAME, TBL_LOGIN_USERS.idate, TBL_LOGIN_USERS.USER_AUTH,
                                            TBL_LOGIN_USERS.USER_IP, TBL_LOGIN_USERS.USER_EMAIL, TBL_LOGIN_USERS.USER_MOBILE, TBL_LOGIN_USERS.USER_STATUS, 
                                            TBL_LOGIN_PRIVILEGES.PRIVILEGE_NAME 
                              FROM            TBL_LOGIN_USERS, TBL_LOGIN_PRIVILEGES 
                             WHERE        TBL_LOGIN_USERS.PRIVILEGE_ID = TBL_LOGIN_PRIVILEGES.PRIVILEGE_ID   
                          order by TBL_LOGIN_USERS .idate desc ";

                dal.open();
                dt = dal.SelectData(query, null);
                dal.close();
                return dt;
            

        }
        public DataTable SelectUser(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT        TBL_LOGIN_USERS.USER_ID, TBL_LOGIN_USERS.USER_NAME, TBL_LOGIN_USERS.USER_PASS, TBL_LOGIN_USERS.USER_AUTH, " +
                                        " TBL_LOGIN_USERS.USER_IP, TBL_LOGIN_USERS.USER_EMAIL, TBL_LOGIN_USERS.USER_MOBILE, TBL_LOGIN_USERS.USER_STATUS, " +
                                        " TBL_LOGIN_PRIVILEGES.PRIVILEGE_NAME,TBL_LOGIN_USERS.SKILL_ID ,TBL_LOGIN_USERS.PRIVILEGE_ID" +
                        "    FROM            TBL_LOGIN_USERS, TBL_LOGIN_PRIVILEGES " +
                        "    WHERE        TBL_LOGIN_USERS.PRIVILEGE_ID = TBL_LOGIN_PRIVILEGES.PRIVILEGE_ID and TBL_LOGIN_USERS.USER_ID=:par1";
            OracleParameter[] par = new OracleParameter[1];
            par[0] = new OracleParameter("par1", id);
            dal.open();
            dt = dal.SelectData(query, par);
            dal.close();
            return dt;

        }

        public string UpdaeUsers(string UserID, string UserName, string UserPass, string UserPrivilege, string UserAuthType, string UserSkill, string UserEmail, string UserMobile, string UserIP,string Status)
        {
            string userName = UserName.ToLower();
            string userPassHash = "0";
            if (UserAuthType == "NOT_LDAP")
            {
                userPassHash = FormsAuthentication.HashPasswordForStoringInConfigFile(UserPass, "sha1");
            }

            OracleConnection conn = new OracleConnection();
            OracleCommand cmd = new OracleCommand();

            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
                cmd.Connection = conn;
                conn.Open();

                cmd.Parameters.AddWithValue("parUserID", UserID);
                cmd.Parameters.AddWithValue("parUserName", userName);
                cmd.Parameters.AddWithValue("parUserPass", userPassHash);
                cmd.Parameters.AddWithValue("parAuthType", UserAuthType);
                cmd.Parameters.AddWithValue("parUserIP", UserIP);
                cmd.Parameters.AddWithValue("parUserEmail", UserEmail);
                cmd.Parameters.AddWithValue("parUserMobile", UserMobile);
                cmd.Parameters.AddWithValue("parUserStatus", Status);
                cmd.Parameters.AddWithValue("parUserPrivilege", UserPrivilege);
                cmd.Parameters.AddWithValue("parUpdatedUser",HttpContext.Current.Session["UserName"].ToString());

                cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                  " SET" +
                                  " USER_NAME = :parUserName," +
                                  " USER_PASS = :parUserPass," +
                                  " USER_AUTH = :parAuthType," +
                                  " USER_IP = :parUserIP," +
                                  " USER_EMAIL = :parUserEmail," +
                                  " USER_MOBILE = :parUserMobile," +
                                  " USER_STATUS = :parUserStatus," +
                                  " PRIVILEGE_ID = :parUserPrivilege," +
                                  " UDATE = sysdate," +
                                  " UUSER = :parUpdatedUser," +
                                  " LAST_PASSWORD_CHANGE = sysdate-200 " +
                                  " WHERE (USER_ID = :parUserID)";

                cmd.ExecuteNonQuery();
                return "done";

                ErrorLogs.WriteLogDB("Updated User", "This user has updated User Name: " + UserID + " , " + userName + " , Auth Type: " + UserAuthType+ " , Status: " + Status + " , Privilege: " +UserPrivilege + " , Email: " + UserEmail + " , Mobile: " + UserMobile+ " , IP: " +UserIP,HttpContext.Current. Session["UserName"].ToString(), "BNet", "NablusMConnection");
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("UsersManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error,HttpContext.Current. Session["UserName"].ToString(), "BNet", ex);
                ErrorLogs.WriteErrorLogsXML("UsersManagment Page, GridView1_RowCommand, Update", PriorityType.Medium, TraceEventType.Error, CategoryType.Error,HttpContext.Current. Session["UserName"].ToString(), "BNet", ex);
                return ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
    }


}