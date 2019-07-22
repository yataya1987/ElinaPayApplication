using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
/// <summary>
/// Summary description for DataAccessLayer
/// </summary>

namespace BnetApplication.Data_Access_Layer
{
    public class DataAccessLayer
    {

        public OracleConnection Myconnection;

        // This Constructor Inisialize the connection object
        public DataAccessLayer()
        {
            Myconnection = new OracleConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
        }

        // Method to open connection
        public void open()
        {
            try
            {
                Myconnection.Open();
            }
            catch
            {
                return;
            }

        }
        // Method to close connection
        public void close()
        {
            try
            {
                Myconnection.Close();
            }
            catch
            {
                return;
            }


        }

        // Method to read data from DataBase 
        public DataTable SelectData(string Query, OracleParameter[] param)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Query;
            cmd.Connection = Myconnection;

            if (param != null)
            {
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
            }


            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }

        // Method to Insert, Update, and Delete Data from DataBase
        public void ExicuteCommand(string Query, OracleParameter[] param)
        {
            OracleCommand cmd = new OracleCommand(Query, Myconnection);
            cmd.CommandType = CommandType.Text;
            if (param != null)
            {
                //for (int i =0; i <= param.Length; i++)
                //{
                //    sqlcmd.Parameters.Add(param[i]);
                //}

                cmd.Parameters.AddRange(param);
                // We used AddRange insted for loop, it do the same work but less code.       
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }



}