using System;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Configuration;
using Logger;
using System.Web.Security;
using System.Web;
using System.DirectoryServices;
using LogInLibrary.WebRef.SendSMS;
using LogInLibrary.WebRef.SendEmail;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace BnetApplication.Data_Access_Layer
{
    /// <summary>
    /// LogIn Library is used for managing the Login Page.
    /// </summary>
    /// <Author>Ayman Daqa</Author>
    /// <DateWritten>27th August 2017</DateWritten>
    /// <DateModified>27th August 2017</DateModified>
    public class LogInClass : Page
    {
        private static string AllGrp = string.Empty;
        /// <summary>
        /// this method to authonticate users login 
        /// </summary>
        /// <param name="userName1">login user name</param>
        /// <param name="userPass1">login user password</param>
        /// <param name="userType">login user type as admin </param>
        /// <param name="loggerFolder">Folder name to insert logs </param>
        /// <param name="connectionString">database connection string in wepconfig</param>
        /// <returns>true if success else false</returns>
        public static int LogInMethod(string userName1, string userPass1, string userType, string loggerFolder, string connectionString)
        {
            string userName = userName1.Trim().ToLower();

            string userPass = userPass1.Trim();
            /****************************************************************/
            // To check that the password is less than 30 characters ..   
            if (userPass.Length >= 30)
            {

                ErrorLogs.WriteInformationLogs("Login Failed - LogInMethod - Password More than 30 characters", PriorityType.Urgent, TraceEventType.Critical, CategoryType.Information, userName, loggerFolder);
                ErrorLogs.WriteInformationLogsXML("Login Failed - LogInMethod - Password More than 30 characters", PriorityType.Urgent, TraceEventType.Critical, CategoryType.Information, userName, loggerFolder);
                HttpContext.Current.Session["ErrorMessage"] = "عدد الاحرف اكثر من المتوقع !!";
                return 0;
            }
            /****************************************************************/
            string userIdDB = string.Empty;
            string userNameDB = string.Empty;
            string userPassDB = string.Empty;
            string userAuthDB = string.Empty;
            string userEmailDB = string.Empty;
            string userMobileDB = string.Empty;
            string userLastPassDB = string.Empty;
            string userStatusDB = string.Empty;

            string userPrivilegeIdDB = string.Empty;
            string userPrivilegeNameDB = string.Empty;
            string userRedirectPageDB = string.Empty;

            string hashedLoginUserPass = string.Empty;
            string userAccountDBID = string.Empty;
            string userAccountDB = string.Empty;
            string userSkillDB = string.Empty;

            string numberLoginAttemptDB = "0";
            string DatelastLoginAttemptDB = string.Empty;
            string LastLoginTimeDB = string.Empty;
            /****************************************************************/
            LogInClass classObj = new LogInClass();
            bool changePasswordFlag = true;
            /****************************************************************/
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection();
            OracleDataReader rdr = null;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /***************************يفحص داخل اليوزر [Table Login useres] اذا يوجد حساب *************************************/
                cmd.Parameters.AddWithValue("parUserName", userName);
                cmd.Parameters.AddWithValue("parUserType", userType);

                cmd.CommandText = "SELECT" +
                                  " users.USER_ID, users.USER_NAME, users.USER_PASS, users.USER_AUTH, users.USER_EMAIL, users.USER_MOBILE," +
                                  " users.LAST_PASSWORD_CHANGE, users.USER_STATUS, users.LOGIN_ATTEMPT, users.LOGIN_ATTEMPT_DATE, users.LAST_LOGIN_DATE," +
                                  " privilege.PRIVILEGE_ID, privilege.PRIVILEGE_NAME, privilege.REDIRECT_PAGE," +
                                  " skill.SKILL_NAME , account.ACCOUNT_NAME , account.ACCOUNT_ID" +
                                  " FROM" +
                                  " TBL_LOGIN_USERS users, TBL_LOGIN_PRIVILEGES privilege, TBL_LOGIN_SKILLS skill, TBL_LOGIN_ACCOUNTS account" +
                                  " WHERE" +
                                  " users.PRIVILEGE_ID = privilege.PRIVILEGE_ID" +
                                  " AND users.SKILL_ID = skill.SKILL_ID" +
                                  " AND skill.ACCOUNT_ID = account.ACCOUNT_ID" +
                                  " AND ( (users.USER_STATUS = 'Enabled') OR (users.USER_STATUS = 'Locked') OR (users.User_STATUS ='Disabled') )" +
                                  " AND (LOWER(users.USER_NAME) = :parUserName)" +
                                  " AND (users.PRIVILEGE_ID = :parUserType)";
                /****************************************************************/

                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    userIdDB = rdr["USER_ID"].ToString();
                    userNameDB = rdr["USER_NAME"].ToString();
                    userPassDB = rdr["USER_PASS"].ToString();
                    userAuthDB = rdr["USER_AUTH"].ToString();
                    userEmailDB = rdr["USER_EMAIL"].ToString();
                    userMobileDB = rdr["USER_MOBILE"].ToString();
                    userLastPassDB = rdr["LAST_PASSWORD_CHANGE"].ToString();
                    userStatusDB = rdr["USER_STATUS"].ToString();

                    userPrivilegeIdDB = rdr["PRIVILEGE_ID"].ToString();
                    userPrivilegeNameDB = rdr["PRIVILEGE_NAME"].ToString();
                    userRedirectPageDB = rdr["REDIRECT_PAGE"].ToString();

                    userAccountDB = rdr["ACCOUNT_NAME"].ToString();
                    userAccountDBID = rdr["ACCOUNT_ID"].ToString();

                    userSkillDB = rdr["SKILL_NAME"].ToString();

                    numberLoginAttemptDB = rdr["LOGIN_ATTEMPT"].ToString();
                    DatelastLoginAttemptDB = rdr["LOGIN_ATTEMPT_DATE"].ToString();
                    LastLoginTimeDB = rdr["LAST_LOGIN_DATE"].ToString();

                    hashedLoginUserPass = FormsAuthentication.HashPasswordForStoringInConfigFile(userPass, "SHA1");

                    /***********************************************************************************/
                    //فحص الحساب اذا كان مفلق 
                    TimeSpan dateLoginAttempt = new TimeSpan();
                    dateLoginAttempt = DateTime.Now - DateTime.Parse(DatelastLoginAttemptDB);

                    if (userStatusDB.Equals("Locked") && (dateLoginAttempt.Minutes < 15 && dateLoginAttempt.Hours <= 0 && dateLoginAttempt.Days <= 0) && userAuthDB.Equals("NOT_LDAP"))
                    {
                        ErrorLogs.WriteInformationLogs("Login Failed - " + userPrivilegeNameDB + " - " + userAuthDB + " - The Account is Locked", PriorityType.High, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        ErrorLogs.WriteInformationLogsXML("Login Failed - " + userPrivilegeNameDB + " - " + userAuthDB + " - The Account is Locked", PriorityType.High, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        HttpContext.Current.Session["ErrorMessage"] = "تم قفل الحساب بشكل مؤقت ";
                        return 0;
                    }
                    /***********************************************************************************/
                    TimeSpan dateLoginDate = new TimeSpan();
                    dateLoginDate = DateTime.Now - DateTime.Parse(LastLoginTimeDB);

                    if (dateLoginDate.Days > 60 && userAuthDB.Equals("NOT_LDAP"))
                    {
                        cmd.Parameters.Clear();

                        int numloginAttempt = 0;

                        cmd.Parameters.AddWithValue("parUserID", userIdDB);
                        cmd.Parameters.AddWithValue("parNumLoginAttempt", numloginAttempt);
                        cmd.Parameters.AddWithValue("parStatus", "Disabled");

                        cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                          " SET" +
                                          " USER_STATUS = :parStatus," +
                                          " LOGIN_ATTEMPT = :parNumLoginAttempt," +
                                          " LOGIN_ATTEMPT_DATE = sysdate" +
                                          " WHERE" +
                                          " USER_ID = :parUserID";

                        cmd.ExecuteNonQuery();

                        ErrorLogs.WriteInformationLogs("Login Failed - " + userPrivilegeNameDB + " - " + userAuthDB + " - The Account is disabled", PriorityType.High, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        ErrorLogs.WriteInformationLogsXML("Login Failed - " + userPrivilegeNameDB + " - " + userAuthDB + " - The Account is disabled", PriorityType.High, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        HttpContext.Current.Session["ErrorMessage"] = "حسابك غير فعال الرجاء مراجعة المدير";
                        return 0;
                    }

                    /***********************************************************************************/
                    if (userAuthDB.Equals("LDAP"))
                    {
                        if (classObj.AutenticateUser("reach", userName, userPass, loggerFolder))
                        {
                            /*****************************************************************/
                            HttpContext.Current.Session["UserName"] = userName;

                            HttpContext.Current.Session["UserType"] = userPrivilegeNameDB;
                            HttpContext.Current.Session["UserTypeID"] = userPrivilegeIdDB;

                            HttpContext.Current.Session["UserAccount"] = userAccountDB;
                            HttpContext.Current.Session["UserAccountID"] = userAccountDBID;

                            HttpContext.Current.Session["UserSkill"] = userSkillDB;
                            /*****************************************************************/


                            HttpContext.Current.Session["RedirectPage"] = userRedirectPageDB;


                            ErrorLogs.WriteInformationLogs("Login Successfully - " + userPrivilegeNameDB + " - LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            ErrorLogs.WriteInformationLogsXML("Login Successfully - " + userPrivilegeNameDB + " - LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            return 1;
                        }
                        else
                        {
                            ErrorLogs.WriteInformationLogs("Login Failed - Wrong in UserName or Password - LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            ErrorLogs.WriteInformationLogsXML("Login Failed - Wrong in UserName or Password - LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            return 0;
                        }
                    }
                    else
                    {
                        if (userPassDB.Equals(hashedLoginUserPass))
                        {
                            /*****************************************************************/
                            HttpContext.Current.Session["UserName"] = userName;
                            HttpContext.Current.Session["userIdDB"] = userIdDB;

                            HttpContext.Current.Session["UserType"] = userPrivilegeNameDB;
                            HttpContext.Current.Session["UserTypeID"] = userPrivilegeIdDB;

                            HttpContext.Current.Session["UserAccount"] = userAccountDB;
                            HttpContext.Current.Session["UserAccountID"] = userAccountDBID;

                            HttpContext.Current.Session["UserSkill"] = userSkillDB;
                            /*****************************************************************/
                            HttpContext.Current.Session["RedirectPage"] = userRedirectPageDB;
                            //HttpContext.Current.Response.Redirect(userRedirectPageDB, false);
                            /*****************************************************************/
                            //change password Force---------------
                            if (userLastPassDB == string.Empty)
                            {
                                HttpContext.Current.Session["CurrentPass"] = userPassDB;
                                changePasswordFlag = false;
                                //HttpContext.Current.Response.Redirect("~/ChangePasswordPage.aspx");
                                return 2;
                            }
                            else
                            {
                                TimeSpan sp = new TimeSpan();
                                sp = DateTime.Now.Date - DateTime.Parse(userLastPassDB);

                                if (sp.Days > 60)
                                {
                                    HttpContext.Current.Session["CurrentPass"] = userPassDB;
                                    changePasswordFlag = false;
                                    //HttpContext.Current.Response.Redirect("~/ChangePasswordPage.aspx");
                                    return 2;
                                }
                            }
                            /***************************************************************************/
                            // Redirect to desired page
                            //HttpContext.Current.Response.Redirect(userRedirectPageDB, false);
                            /***************************************************************************/
                            // Update the number of Attempt ...
                            try
                            {
                                cmd.Parameters.Clear();

                                int numloginAttempt = 0;

                                cmd.Parameters.AddWithValue("parUserID", userIdDB);
                                cmd.Parameters.AddWithValue("parNumLoginAttempt", numloginAttempt);
                                cmd.Parameters.AddWithValue("parStatus", "Enabled");

                                cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                                  " SET" +
                                                  " USER_STATUS = :parStatus," +
                                                  " LOGIN_ATTEMPT = :parNumLoginAttempt," +
                                                  " LOGIN_ATTEMPT_DATE = sysdate," +
                                                  " LAST_LOGIN_DATE = sysdate" +
                                                  " WHERE" +
                                                  " USER_ID = :parUserID";

                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ErrorLogs.WriteErrorLogs("LogInLibrary, LogInMethod - Update The Number Of Login Attempt, 1", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                                ErrorLogs.WriteErrorLogsXML("LogInLibrary, LogInMethod - Update The Number Of Login Attempt, 1", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                            }
                            /***************************************************************************************/

                            ErrorLogs.WriteInformationLogs("Login Successfully - " + userPrivilegeNameDB + " - NOT LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            ErrorLogs.WriteInformationLogsXML("Login Successfully - " + userPrivilegeNameDB + " - NOT LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            return 1;
                        }
                        else
                        {
                            // Update the number of Attempt ...
                            try
                            {
                                cmd.Parameters.Clear();

                                int numloginAttempt = Convert.ToInt32(numberLoginAttemptDB) + 1;

                                if ((numloginAttempt % 5) == 0)
                                {
                                    cmd.Parameters.AddWithValue("parStatus", "Locked");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("parStatus", "Enabled");
                                }

                                cmd.Parameters.AddWithValue("parUserID", userIdDB);
                                cmd.Parameters.AddWithValue("parNumLoginAttempt", numloginAttempt);

                                cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                                  " SET" +
                                                  " USER_STATUS = :parStatus," +
                                                  " LOGIN_ATTEMPT = :parNumLoginAttempt," +
                                                  " LOGIN_ATTEMPT_DATE = sysdate" +
                                                  " WHERE" +
                                                  " USER_ID = :parUserID";

                                cmd.ExecuteNonQuery();
                                /***************************************************************/
                                HttpContext.Current.Session["ErrorMessage"] = "مشكلة في الدخول, حاول مرة اخرى";
                                /***************************************************************/

                            }
                            catch (Exception ex)
                            {
                                ErrorLogs.WriteErrorLogs("LogInLibrary, LogInMethod - Update The Number Of Login Attempt, 2", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                                ErrorLogs.WriteErrorLogsXML("LogInLibrary, LogInMethod - Update The Number Of Login Attempt, 2", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                            }
                            /***************************************************************************************/
                            ErrorLogs.WriteInformationLogs("Login Failed - Wrong in UserName or Password - NOT LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            ErrorLogs.WriteInformationLogsXML("Login Failed - Wrong in UserName or Password - NOT LDAP", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                            return 0;
                        }
                    }
                }




                else if (classObj.AutenticateUserByGroups("reach", userName, userPass, loggerFolder))
                {
                    cmd.Parameters.Clear();

                    //cmd.Parameters.AddWithValue("parAllGroups", AllGrp);
                    cmd.Parameters.AddWithValue("parUserType", userType);

                    cmd.CommandText = "SELECT" +
                                  " privilege.PRIVILEGE_ID, privilege.PRIVILEGE_NAME, privilege.PRIVILEGE_STATUS, privilege.REDIRECT_PAGE," +
                                  " grps.GROUP_ID, grps.GROUP_NAME, grps.GROUP_STATUS," +
                                  " account.ACCOUNT_NAME, account.ACCOUNT_ID" +
                                  " FROM" +
                                  " TBL_LOGIN_GROUPS grps, TBL_LOGIN_PRIVILEGES privilege, TBL_LOGIN_ACCOUNTS account" +
                                  " WHERE" +
                                  " grps.PRIVILEGE_ID = privilege.PRIVILEGE_ID" +
                                  " AND grps.ACCOUNT_ID = account.ACCOUNT_ID" +
                                  " AND (grps.GROUP_STATUS = 'Enabled')" +
                                  " AND (privilege.PRIVILEGE_STATUS = 'Enabled')" +
                                  " AND (grps.PRIVILEGE_ID = :parUserType)" +
                                  " AND (grps.GROUP_NAME in " + AllGrp + ")";
                    //   " AND (grps.GROUP_NAME in '(' || :parAllGroups || ')' )";

                    /****************************************************************/

                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        userPrivilegeIdDB = rdr["PRIVILEGE_ID"].ToString();
                        userPrivilegeNameDB = rdr["PRIVILEGE_NAME"].ToString();
                        userRedirectPageDB = rdr["REDIRECT_PAGE"].ToString();

                        userAccountDBID = rdr["ACCOUNT_ID"].ToString();
                        userAccountDB = rdr["ACCOUNT_NAME"].ToString();
                        //userSkillDB = rdr["SKILL_NAME"].ToString();
                        /*****************************************************************/
                        HttpContext.Current.Session["UserName"] = userName;

                        HttpContext.Current.Session["UserTypeID"] = userPrivilegeIdDB;
                        HttpContext.Current.Session["UserType"] = userPrivilegeNameDB;

                        HttpContext.Current.Session["UserAccountID"] = userAccountDBID;
                        HttpContext.Current.Session["UserAccount"] = userAccountDB;
                        //HttpContext.Current.Session["UserSkill"] = userSkillDB;
                        /*****************************************************************/

                        //TODO: Active all 
                        HttpContext.Current.Session["RedirectPage"] = userRedirectPageDB;
                        //HttpContext.Current.Response.Redirect(userRedirectPageDB, false);

                        ErrorLogs.WriteInformationLogs("Login Successfully - " + userPrivilegeNameDB + " - Domain UserName and Password", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        ErrorLogs.WriteInformationLogsXML("Login Successfully - " + userPrivilegeNameDB + " - Domain UserName and Password", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        return 1;

                    }
                    else
                    {
                        ErrorLogs.WriteInformationLogs("Login Failed - Not in the Groups - Domain UserName and Password", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        ErrorLogs.WriteInformationLogsXML("Login Failed - Not in the Groups - Domain UserName and Password", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                        return 0;
                    }
                }
                else
                {
                    ErrorLogs.WriteInformationLogs("Login Failed - Wrong in UserName or Password - Domain UserName and Password", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                    ErrorLogs.WriteInformationLogsXML("Login Failed - Wrong in UserName or Password - Domain UserName and Password", PriorityType.Low, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                if (changePasswordFlag)
                {
                    ErrorLogs.WriteErrorLogs("LogInLibrary, LogInMethod", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                    ErrorLogs.WriteErrorLogsXML("LogInLibrary, LogInMethod", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                    return 0;
                }
                else
                {
                    ErrorLogs.WriteErrorLogs("LogInLibrary, LogInMethod - No Problem - Redirect To ChangePassowrdPage", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                    ErrorLogs.WriteErrorLogsXML("LogInLibrary, LogInMethod - No Problem - Redirect To ChangePassowrdPage", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                    return 1;
                }

            }
            finally
            {
                cmd.Parameters.Clear();
                conn.Close();
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        private bool AutenticateUserByGroups(string domainName, string userName, string password, string loggerFolder)
        {

            // Path to you LDAP directory server.
            // Contact your network administrator to obtain a valid path.
            //string adPath = "LDAP://" + System.Configuration.ConfigurationSettings.AppSettings["DefaultActiveDirectoryServer"];

            string adPath = ConfigurationManager.AppSettings["LDAPIP"].ToString();
            try
            {
                LogInClass classObj = new LogInClass();
                if (classObj.IsAuthenticated(adPath, domainName, userName, password, loggerFolder))
                {
                    AllGrp = classObj.FindTheGroups(adPath, domainName, userName, password, loggerFolder);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("LogInLibrary, AutenticateUserByGroups", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, AutenticateUserByGroups", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                return false;
            }
        }

        private bool AutenticateUser(string domainName, string userName, string password, string loggerFolder)
        {
            // Path to you LDAP directory server.
            // Contact your network administrator to obtain a valid path.
            //string adPath = "LDAP://" + System.Configuration.ConfigurationSettings.AppSettings["DefaultActiveDirectoryServer"];

            string adPath = ConfigurationManager.AppSettings["LDAPIP"].ToString();
            try
            {
                LogInClass classObj = new LogInClass();
                if (classObj.IsAuthenticated(adPath, domainName, userName, password, loggerFolder))
                {
                    classObj.EncryptionMethod(userName, loggerFolder);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("LogInLibrary, AutenticateUser", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, AutenticateUser", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                return false;
            }
        }

        private bool IsAuthenticated(string _path, string domainName, string userName, string password, string loggerFolder)
        {
            string _filterAttribute;
            string domainAndUsername = domainName + @"\" + userName;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, password); //Follow
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + userName + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("LogInLibrary, IsAuthenticated", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, IsAuthenticated", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                HttpContext.Current.Session["ErrorMessage"] = "مشكلة في اسم المستخدم او كلمة المرور ";
                return false;
            }
        }

        private string FindTheGroups(string _path, string domainName, string userName, string password, string loggerFolder)
        {
            try
            {
                DirectoryEntry Entry = new DirectoryEntry(_path, userName, password);
                DirectorySearcher Searcher = new DirectorySearcher(Entry);
                Searcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                Searcher.Filter = ("(&(objectcategory=user)(SAMAccountName=" + (userName + "))"));
                SearchResult res = Searcher.FindOne();
                int i;
                DirectoryEntry de;
                string begin = " ( ";
                string end = " '') ";
                string AllGrp = string.Empty;
                for (i = 0; (i <= (res.Properties["memberOf"].Count - 1)); i++)
                {
                    de = new DirectoryEntry("LDAP://" + res.Properties["memberOf"][i].ToString());
                    AllGrp = (AllGrp + "'" + de.Properties["name"].Value.ToString() + "' , ");
                }
                AllGrp = begin + AllGrp + end;
                EncryptionMethod(userName, loggerFolder);
                return AllGrp;
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("LogInLibrary, FindTheGroups", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, FindTheGroups", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                return (string.Empty);
            }
        }

        private void EncryptionMethod(string userName, string loggerFolder)
        {
            try
            {
                // Create the authetication ticket
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(60), false, "");
                // Now encrypt the ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                // Create a cookie and add the encrypted ticket to the
                // cookie as data.
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                // Add the cookie to the outgoing cookies collection.
                HttpContext.Current.Response.Cookies.Add(authCookie);
                // Redirect the user to the originally requested page
                //HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(userName, false ));
            }
            catch (Exception ex)
            {
                ErrorLogs.WriteErrorLogs("LogInLibrary, EncryptionMethod", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, EncryptionMethod", PriorityType.Medium, TraceEventType.Warning, CategoryType.Error, userName, loggerFolder, ex);
            }
        }

    }

    public class ChangePasswordClass
    {
        public static bool ChangePasswordMethod(string currentPass, string newPass, string newPassConfirm, string loggerFolder, string connectionString)
        {
            string userName = string.Empty;
            try
            {
                string currentPassword = currentPass.Trim();
                string newPassword = newPass.Trim();
                string newPasswordConfirm = newPassConfirm.Trim();
                /****************************************************************/
                userName = HttpContext.Current.Session["UserName"].ToString();
                string userID = HttpContext.Current.Session["userIdDB"].ToString();
                string currentPass_DB = HttpContext.Current.Session["CurrentPass"].ToString();
                string hashedCurrentPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(currentPassword, "sha1");
                /***********************************************************************************/
                // To check that the password is less than 30 characters ..
                if (newPassword.Length >= 30)
                {
                    ErrorLogs.WriteInformationLogs("ChangePassword Failed - ChangePasswordMethod - Password More than 30 characters", PriorityType.Urgent, TraceEventType.Critical, CategoryType.Information, userName, loggerFolder);
                    ErrorLogs.WriteInformationLogsXML("ChangePassword Failed - ChangePasswordMethod - Password More than 30 characters", PriorityType.Urgent, TraceEventType.Critical, CategoryType.Information, userName, loggerFolder);
                    HttpContext.Current.Session["ErrorMessage"] = "يجب ان لا يتجاوز 30 حرف ";
                    return false;
                }
                /***********************************************************************************/

                if ((currentPass_DB == hashedCurrentPassword) && (newPassword == newPasswordConfirm) && (currentPassword != newPassword))
                {
                    ChangePasswordClass classObj = new ChangePasswordClass();
                    if (classObj.ValidatePassword(newPassword, 8, 1, 1, 1, 1)) // call function 'ValidatePassword'
                    {
                        string hashedpass = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1");

                        OracleConnection conn = new OracleConnection();
                        OracleCommand cmd = new OracleCommand();

                        try
                        {
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
                            cmd.Connection = conn;
                            conn.Open();
                            /*****************************************************************************/
                            int numloginAttempt = 0;

                            cmd.Parameters.AddWithValue("parUserID", userID);
                            cmd.Parameters.AddWithValue("parPass", hashedpass);
                            cmd.Parameters.AddWithValue("parNumLoginAttempt", numloginAttempt);
                            cmd.Parameters.AddWithValue("parStatus", "Enabled");

                            cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                              " SET" +
                                              " USER_PASS = :parPass," +
                                              " LAST_PASSWORD_CHANGE = sysdate," +
                                              " USER_STATUS = :parStatus," +
                                              " LOGIN_ATTEMPT = :parNumLoginAttempt," +
                                              " LOGIN_ATTEMPT_DATE = sysdate" +
                                              " WHERE" +
                                              " USER_ID = :parUserID";

                            cmd.ExecuteNonQuery();

                            ErrorLogs.WriteLogDB("Changed Password", "This User has changed his/her Password From: " + currentPassword + " , to: " + newPassword, userName, loggerFolder, connectionString);

                            //HttpContext.Current.Response.Redirect("Login/index", false);
                            return true;
                        }
                        catch (Exception ex)
                        {
                         HttpContext.Current.Session["ErrorMessage"]= ex;
                            ErrorLogs.WriteErrorLogs("LogInLibrary, ChangePasswordMethod, User ID = " + userID, PriorityType.Medium, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                            ErrorLogs.WriteErrorLogsXML("LogInLibrary, ChangePasswordMethod, User ID = " + userID, PriorityType.Medium, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                            HttpContext.Current.Session["ErrorMessage"] = "خطأ في التحديث راجع القسم المختص";
                            return false;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    HttpContext.Current.Session["ErrorMessage"] = "تأكد من المدخلات مرة اخرى";
                    return false;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["ErrorMessage"] = ex;
                ErrorLogs.WriteErrorLogs("LogInLibrary, ChangePasswordMethod", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, ChangePasswordMethod", PriorityType.Medium, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                return false;
            }

        }
        /// <summary> 
        /// Every password must contain at least one character, one symbol, and one number
        /// The min length of the password is 8 characters.
        /// </summary>
        /// <param name="pwd">the new password</param>
        /// <param name="minLength"> min length of the password</param>
        /// <param name="numUpper">count of the upper characters used in the password</param>
        /// <param name="numLower">count of the lower characters used in the password</param>
        /// <param name="numNumbers">count of the numbers used in the password</param>
        /// <param name="numSpecial">count of the special symbol used in the password</param>
        /// <returns></returns>
        private bool ValidatePassword(string pwd, int minLength, int numUpper, int numLower, int numNumbers, int numSpecial)
        {

            // Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            System.Text.RegularExpressions.Regex upper = new System.Text.RegularExpressions.Regex("[A-Z]");
            System.Text.RegularExpressions.Regex lower = new System.Text.RegularExpressions.Regex("[a-z]");
            System.Text.RegularExpressions.Regex number = new System.Text.RegularExpressions.Regex("[0-9]");
            // Special is "none of the above".
            System.Text.RegularExpressions.Regex special = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");

            int i = 0;

            // Check the length.
            if (pwd.Length < minLength)
                return false;
            // Check for minimum number of occurrences.
            if (upper.Matches(pwd).Count >= numUpper)
                i = i + 1;
            if (lower.Matches(pwd).Count >= numLower)
                i = i + 1;
            if (number.Matches(pwd).Count >= numNumbers)
                i = i + 1;
            if (special.Matches(pwd).Count >= numSpecial)
                i = i + 1;

            // Passed all checks.
            if (i >= 3)
            {
                return true;
            }
            else
            {
                HttpContext.Current.Session["ErrorMessage"] = "الرجاء ادخال احرف وارقام ورموز";
                return false;
            }
        }
    }

    public class GetPasswordClass
    {
        public static bool GetPasswordMethod(string userName1, string userType, string loggerFolder, string connectionString)
        {
            string userName = userName1.Trim().ToLower();

            string userIdDB = string.Empty;
            string userNameDB = string.Empty;
            string userPassDB = string.Empty;
            string userAuthDB = string.Empty;
            string userEmailDB = string.Empty;
            string userMobileDB = string.Empty;
            string userLastPassDB = string.Empty;
            string userStatusDB = string.Empty;

            string userPrivilegeIdDB = string.Empty;
            string userPrivilegeNameDB = string.Empty;
            string userRedirectPageDB = string.Empty;

            string hashedLoginUserPass = string.Empty;
            string userAccountDB = string.Empty;
            string userSkillDB = string.Empty;

            /****************************************************************/
            SendSMS sendSMS = new SendSMS();
            Service sendEmail = new Service();
            /****************************************************************/
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection();
            OracleDataReader rdr = null;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
                cmd.Connection = conn;
                conn.Open();
                /****************************************************************/
                cmd.Parameters.AddWithValue("parUserName", userName);
                cmd.Parameters.AddWithValue("parUserType", userType);

                cmd.CommandText = "SELECT" +
                                  " users.USER_ID, users.USER_NAME, users.USER_PASS, users.USER_AUTH, users.USER_EMAIL, users.USER_MOBILE," +
                                  " users.LAST_PASSWORD_CHANGE, users.USER_STATUS," +
                                  " privilege.PRIVILEGE_ID, privilege.PRIVILEGE_NAME" +
                                  " FROM" +
                                  " TBL_LOGIN_USERS users, TBL_LOGIN_PRIVILEGES privilege" +
                                  " WHERE" +
                                  " users.PRIVILEGE_ID = privilege.PRIVILEGE_ID" +
                                  " AND (users.USER_STATUS = 'Enabled')" +
                                  " AND (users.USER_AUTH = 'NOT_LDAP')" +
                                  " AND (LOWER(users.USER_NAME) = :parUserName)" +
                                  " AND (users.PRIVILEGE_ID = :parUserType)";
                /****************************************************************/

                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    userIdDB = rdr["USER_ID"].ToString();
                    userNameDB = rdr["USER_NAME"].ToString();
                    userPassDB = rdr["USER_PASS"].ToString();
                    userAuthDB = rdr["USER_AUTH"].ToString();
                    userEmailDB = rdr["USER_EMAIL"].ToString();
                    userMobileDB = rdr["USER_MOBILE"].ToString();
                    userLastPassDB = rdr["LAST_PASSWORD_CHANGE"].ToString();
                    userStatusDB = rdr["USER_STATUS"].ToString();

                    userPrivilegeIdDB = rdr["PRIVILEGE_ID"].ToString();
                    userPrivilegeNameDB = rdr["PRIVILEGE_NAME"].ToString();
                    /****************************************************************/
                    string newPassRandom = Membership.GeneratePassword(8, 1);
                    newPassRandom = Regex.Replace(newPassRandom, @"[^a-zA-Z0-9]", m => "9");
                    //int length = 8;

                    //string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    //string newPassRandom = string.Empty;
                    //Random rnd = new Random();
                    //while (0 < length--)
                    //    newPassRandom += valid[rnd.Next(valid.Length)];

                    /****************************************************************/
                    if (userMobileDB != string.Empty && newPassRandom != string.Empty)
                    {
                        /***********************************************************/
                        string hashedpass = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassRandom, "sha1");
                        try
                        {
                            cmd.Parameters.Clear();

                            cmd.Parameters.AddWithValue("parUserID", userIdDB);
                            cmd.Parameters.AddWithValue("parPass", hashedpass);

                            cmd.CommandText = "UPDATE TBL_LOGIN_USERS" +
                                              " SET" +
                                              " USER_PASS = :parPass," +
                                              " LAST_PASSWORD_CHANGE = (sysdate-200)" +
                                              " WHERE" +
                                              " USER_ID = :parUserID";

                            cmd.ExecuteNonQuery();
                            /**************************************************************************************/
                            sendSMS.SendSms(userMobileDB, "your password is " + newPassRandom, "REACH");
                            /**************************************************************************************/
                            ErrorLogs.WriteLogDB("Get Password Method", " This User has used the ForgetPassword function to retrieve the password ", userName, loggerFolder, connectionString);

                            HttpContext.Current.Response.Redirect("~/LoginPage.aspx", false);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            HttpContext.Current.Session["ErrorMessage"] = ex;
                            ErrorLogs.WriteErrorLogs("LogInLibrary, GetPasswordMethod, User ID = " + userIdDB, PriorityType.Medium, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                            ErrorLogs.WriteErrorLogsXML("LogInLibrary, GetPasswordMethod, User ID = " + userIdDB, PriorityType.Medium, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                            return false;
                        }
                    }
                    else
                    {
                        // Send To ME .... 
                        // string emailBody = "User ID: " + userIdDB + " , User Name: " + userName + " , User Type: " + userType + " , New Pass: Need Jawwal Number";
                        string emailBody = "This User, ID: " + userIdDB + " , User Name: " + userName + " , User Type: " + userType + ", is trying to retrieve the forgotten password, but unfortunately, the mobile number is not inserted into the database.";
                        // TODO : Change the sedn email To New API
                        sendEmail.SendEmail("Notification@reach.ps", "Abcd@1234", "SoftWare Developers & Supports@reach.ps", "AhliBank System New Password  ", emailBody, "reachex.reach.ps");
                        return true;
                    }
                }
                else
                {
                    ErrorLogs.WriteInformationLogs("Get Password Failed - " + userType, PriorityType.High, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                    ErrorLogs.WriteInformationLogsXML("Get Password Failed - " + userType, PriorityType.High, TraceEventType.Information, CategoryType.Information, userName, loggerFolder);
                    HttpContext.Current.Session["ErrorMessage"] = "حسابك لا ينطبق عليه اليه الارسال ";
                    return false;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["ErrorMessage"] = ex;
                ErrorLogs.WriteErrorLogs("LogInLibrary, GetPasswordMethod - " + userType, PriorityType.High, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                ErrorLogs.WriteErrorLogsXML("LogInLibrary, GetPasswordMethod - " + userType, PriorityType.High, TraceEventType.Error, CategoryType.Error, userName, loggerFolder, ex);
                return false;
            }
            finally
            {
                cmd.Parameters.Clear();
                if (rdr != null)
                {
                    rdr.Close();
                }
                conn.Close();
            }
        }

    }

}