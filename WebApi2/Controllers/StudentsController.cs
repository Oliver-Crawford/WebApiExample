using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Helpers;
using System.Web.Http;
using WebApi2.Models;


namespace WebApi2.Controllers
{
    public class StudentsController : ApiController
    {
        string connectionString = @"Data Source = localhost\SQLEXPRESS; Initial Catalog = WebApi2; Integrated Security = true;";
        // GET api/values
        public IEnumerable<People> Get()
        {
            List<People> people = new List<People>();
            string query = "Select * from People";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using(SqlDataAdapter ad = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if(dt != null)
                    {
                        foreach(DataRow row in dt.Rows)
                        {
                            people.Add(new People
                            {
                                Id = (int)row["Id"],
                                FirstName = (string)row["FirstName"],
                                LastName = (string)row["LastName"],
                                Email = (string)row["Email"],
                                Phone = (string)row["Phone"]
                        });
                        }
                    }
                    dt.Dispose();
                    
                }
            }

            return people;
        }
        public People Get(int id)
        {
            People people = new People();
            string query = $"Select * from People where Id = {id}";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter ad = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        people.Id = (int)dt.Rows[0]["Id"];
                        people.FirstName = (string)dt.Rows[0]["FirstName"];
                        people.LastName = (string)dt.Rows[0]["LastName"];
                        people.Email = (string)dt.Rows[0]["Email"];
                        people.Phone = (string)dt.Rows[0]["Phone"];
                    }
                    dt.Dispose();

                }
            }

            return people;
        }
        public IHttpActionResult Post([FromBody] People people)
        {
            string query = $"insert into People(FirstName, LastName, Email, Phone) VALUES(@FirstName, @LastName, @Email, @Phone);";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FirstName", people.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", people.LastName);
                    cmd.Parameters.AddWithValue("@Email", people.Email);
                    cmd.Parameters.AddWithValue("@Phone", people.Phone);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Ok();
        }
        public IHttpActionResult Put([FromBody] People people)
        {
            string query = $"Update People set FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone where Id = @Id;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", people.Id);
                    cmd.Parameters.AddWithValue("@FirstName", people.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", people.LastName);
                    cmd.Parameters.AddWithValue("@Email", people.Email);
                    cmd.Parameters.AddWithValue("@Phone", people.Phone);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Ok();
        }
        public IHttpActionResult Delete(int id)
        {
            string query = $"Delete People where Id = @Id;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Ok();
        }
    }
}
