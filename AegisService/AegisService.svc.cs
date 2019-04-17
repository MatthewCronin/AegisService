using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace AegisService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AegisService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AegisService.svc or AegisService.svc.cs at the Solution Explorer and start debugging.
    public class AegisService : IAegisService
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString);

        //Users
       
        public string PostUser(User u)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 1);
            cmd.Parameters.AddWithValue("@UserName", u.UserName);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            cmd.Parameters.AddWithValue("@SecQuestion1", u.SecQuestion1);
            cmd.Parameters.AddWithValue("@SecQuestion2", u.SecQuestion2);
            cmd.Parameters.AddWithValue("@SecAnswer1", u.SecAnswer1);
            cmd.Parameters.AddWithValue("@SecAnswer2", u.SecAnswer2);
            cmd.Parameters.AddWithValue("@Disabled", false);
            int ret = cmd.ExecuteNonQuery();
            SqlCommand cmd2 = new SqlCommand("Select UserID From tblUsers WHERE UserName = '" + u.UserName + "' AND SecAnswer1 = '" + u.SecAnswer1 + "'", con);
            int id = Convert.ToInt32(cmd2.ExecuteScalar().ToString());
            con.Close();
            con.Dispose();
            cmd.Dispose();
            cmd2.Dispose();
            return id.ToString();
        }
        
        public int ValidateUser(string uName, string pword)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spValidateUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 1);
            cmd.Parameters.AddWithValue("@UserName", uName);
            cmd.Parameters.AddWithValue("@Password", pword);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }        
        //////END USER
        /////// ////////////////////////////////
        //////ToDo
        public int PostToDo(ToDo todo)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spToDo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 1);
            cmd.Parameters.AddWithValue("@UserID", todo.CreatedBy);
            cmd.Parameters.AddWithValue("@Title", todo.Title);
            cmd.Parameters.AddWithValue("@Description", todo.Description);
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(todo.StartDate));
            if (todo.EndDate == null)
            {
                todo.EndDate = "01/01/1900";
            }
            if(todo.EndDate == null)
            {
                cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(todo.EndDate));
            }
            cmd.Parameters.AddWithValue("@CreatedBy", todo.CreatedBy);
            if (todo.UpdatedBy == null)
            {
                cmd.Parameters.AddWithValue("@UpdatedBY", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UpdatedBY", todo.DisabledBy);
            }
            if (todo.DisabledBy == null)
            {
                cmd.Parameters.AddWithValue("@DisabledBY", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DisabledBY", todo.DisabledBy);
            }
            cmd.Parameters.AddWithValue("@Disabled", 0);
            cmd.Parameters.AddWithValue("@Completed", 0);
            int ret = cmd.ExecuteNonQuery();
            con.Close();
            return ret;
        }
        public ToDo GetToDo(string ToDoID)
        {
            ToDo todo = new ToDo();
            con.Open();
            SqlCommand cmd = new SqlCommand("spToDo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 2);
            cmd.Parameters.AddWithValue("@ToDoID", Convert.ToInt32(ToDoID));
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                todo.ToDoID = Convert.ToInt32(dt.Rows[0]["ToDoID"]);
                todo.Title = dt.Rows[0]["Title"].ToString();
                todo.Description = dt.Rows[0]["Description"].ToString();
                todo.StartDate = dt.Rows[0]["StartDate"].ToString();
                todo.EndDate = dt.Rows[0]["EndDate"].ToString();
                todo.Completed = Convert.ToBoolean(dt.Rows[0]["Completed"]);
                todo.Disabled = Convert.ToBoolean(dt.Rows[0]["Disabled"]);
            }
            return todo;
        }
        public List<ToDo> GetToDos(string UserID)
        {
            List<ToDo> todo = null;
            con.Open();
            SqlCommand cmd = new SqlCommand("spToDo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 2);
            cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(UserID));
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                todo = (from DataRow dr in dt.Rows
                        select new ToDo()
                        {
                            Title = dr["Title"].ToString(),
                            Description = dr["Description"].ToString(),
                            ToDoID = Convert.ToInt32(dr["ToDoID"]),
                            StartDate = dr["StartDate"].ToString(),
                            EndDate = dr["EndDate"].ToString(),
                            Disabled = Convert.ToBoolean(dr["Disabled"]),
                            Completed = Convert.ToBoolean(dr["Completed"])
                        }).ToList();

                return todo;
            }
            else
            {
                return null;
            }
        }
        public int PutToDo(ToDo todo)
        {
            //Updates a current ToDo Item
            con.Open();
            SqlCommand cmd = new SqlCommand("spToDo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 3);
            cmd.Parameters.AddWithValue("@ToDoID", todo.ToDoID);
            cmd.Parameters.AddWithValue("@Title", todo.Title);
            cmd.Parameters.AddWithValue("@Description", todo.Description);
            cmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(todo.StartDate));
            if (todo.EndDate == null)
            {
                cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(todo.EndDate));
            }
            if (todo.UpdatedBy == null)
            {
                cmd.Parameters.AddWithValue("@UpdatedBY", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UpdatedBY", todo.DisabledBy);
            }
            if (todo.DisabledBy == null)
            {
                cmd.Parameters.AddWithValue("@DisabledBY", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DisabledBY", todo.DisabledBy);
            }
            cmd.Parameters.AddWithValue("@Disabled", todo.Disabled);
            cmd.Parameters.AddWithValue("@Completed", todo.Completed);
            int ret = cmd.ExecuteNonQuery();
            con.Close();
            return ret;
        }
        public int DisableToDo(ToDo todo)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spToDo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 4);
            cmd.Parameters.AddWithValue("@ToDoID", todo.ToDoID);
            cmd.Parameters.AddWithValue("@DisabledBy", todo.DisabledBy);
            cmd.Parameters.AddWithValue("@Disabled", todo.Disabled);
            return cmd.ExecuteNonQuery();
        }
        public int CompleteToDo(ToDo todo)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("spToDo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Operation", 3);
            cmd.Parameters.AddWithValue("@ToDoID", todo.ToDoID);
            cmd.Parameters.AddWithValue("@UpdatedBy", todo.UpdatedBy);
            cmd.Parameters.AddWithValue("@Completed", todo.Completed);
            return cmd.ExecuteNonQuery();
        }
        //End ToDo
        //////Security Questions
        public List<SecurityQuestions> GetSecQuestions()
        {
            List<SecurityQuestions> lstSecQuestions = null;
            con.Open();
            SqlCommand cmd = new SqlCommand("spSecQuestions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                lstSecQuestions = (from DataRow dr in dt.Rows
                                   select new SecurityQuestions()
                                   {
                                       SecQuestion = dr["SecQuestion"].ToString(),
                                       SecQuestionID = Convert.ToInt32(dr["SecQuestionID"])
                                   }).ToList();

                return lstSecQuestions;
            }
            else
            {
                return null;
            }
        }
        //End Security Questions
    }
}
