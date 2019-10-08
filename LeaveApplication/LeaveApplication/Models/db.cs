using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LeaveApplication.Models
{
    public class db
    {
        SqlConnection con;
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        SqlCommand cmd;
        SqlDataReader reader;
        SqlDataAdapter da;
        DataSet ds;
        /// <summary>
        /// Exceute Insert,Update,Delete Queries
        /// </summary>
        /// <param name="Querry"></param>
        public void ExecuteQuerry(string Querry)
        {
            con = new SqlConnection(connection);
            con.Open();
            cmd = new SqlCommand(Querry, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void ExecuteQuerry(string Querry,SqlParameter sqlParameter)
        {
            con = new SqlConnection(connection);
            con.Open();
            cmd = new SqlCommand(Querry, con);
            cmd.Parameters.Add(sqlParameter);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public DataSet Read(string Querry)
        {
            con = new SqlConnection(connection);
            con.Open();
            da = new SqlDataAdapter(Querry, con);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;

        }
        public object ExecuteScalar(string Querry)
        {
            con = new SqlConnection(connection);
            con.Open();
            cmd = new SqlCommand(Querry, con);
            object x = cmd.ExecuteScalar();
            con.Close();
            return x;
        }
    }
}