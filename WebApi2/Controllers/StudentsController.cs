using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class StudentsController : ApiController
    {

        // GET api/values
        public IEnumerable<Student> Get()
        {
            //in models
            List<Student> students = new List<Student>();

            //connect to DB
            string connectionString = @"Data Source = localhost; Initial Catalog = WebApi2; Integrated Security = true;";
            string query = "Select * from Students";
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
                            students.Add(new Student
                            {
                                Id = (int)row["Id"],
                                Name = (string)row["Name"],
                                Age = (int)row["Age"]
                            });
                        }
                    }
                    dt.Dispose();
                    
                }
            }

            return students;
        }

        // GET api/values/5
        public Student Get(int id)
        {
            //in models
            Student student = new Student();

            //connect to DB
            string connectionString = @"Data Source = localhost; Initial Catalog = WebApi2; Integrated Security = true;";
            string query = $"Select * from Students where id = {id}";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter ad = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        student.Id = (int)dt.Rows[0]["Id"];
                        student.Name = (string)dt.Rows[0]["Name"];
                        student.Age = (int)dt.Rows[0]["Age"];
                    }
                    dt.Dispose();

                }
            }

            return student;
        }

        // POST api/values
        public IHttpActionResult Post([FromBody] Student student)
        {

            //connect to DB
            string connectionString = @"Data Source = localhost; Initial Catalog = WebApi2; Integrated Security = true;";
            string query = $"insert into Students(Name, Age) VALUES(@Name, @Age);";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Ok();
        }

        // PUT api/values/5
        public IHttpActionResult Put([FromBody] Student student)
        {
            //connect to DB
            string connectionString = @"Data Source = localhost; Initial Catalog = WebApi2; Integrated Security = true;";
            string query = $"Update Students set Name = @Name, Age = @Age where Id = @Id;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", student.Id);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Ok();
        }

        // DELETE api/values/5
        public IHttpActionResult Delete(int id)
        {
            //connect to DB
            string connectionString = @"Data Source = localhost; Initial Catalog = WebApi2; Integrated Security = true;";
            string query = $"Delete Students where Id = @Id;";
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
