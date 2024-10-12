using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DotNetTrainingBatch5.RestApi.ViewModels;
using System.Data;

namespace DotNetTrainingBatch5.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsAdoDotNetControllers : ControllerBase
    {
        private readonly string _connectionString = "Data Source=.;Initial Catalog=DotNetTraining;User ID=sa;Password=sasa@123;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetBlogs()
        {

           List<BlogViewModel> lst = new List<BlogViewModel>();
            
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"SELECT [BlogId]
                            ,[BlogTitle]
                            ,[BlogAuthor]
                            ,[BlogContent]
                            ,[DeleteFlag]
                            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["BlogId"]);
                Console.WriteLine(reader["BlogTitle"]);
                Console.WriteLine(reader["BlogAuthor"]);
                Console.WriteLine(reader["BlogContent"]);
                //Console.WriteLine(dr["DeleteFlag"]);

                lst.Add(new BlogViewModel
                {
                    Id = Convert.ToInt32(reader["BlogId"]),
                    Title = Convert.ToString(reader["BlogTitle"]),
                    Author = Convert.ToString(reader["BlogAuthor"]),
                    Content = Convert.ToString(reader["BlogContent"]),
                    DeleteFlag = Convert.ToBoolean(reader["DeleteFlag"]),
                });
            }
            connection.Close(); 
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {

            List<BlogViewModel> lst = new List<BlogViewModel>();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"SELECT [BlogId]
                               ,[BlogTitle]
                               ,[BlogAuthor]
                               ,[BlogContent]
                               ,[DeleteFlag]
                            FROM [dbo].[Tbl_Blog] where BlogId = @BlogId";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            /* SqlDataAdapter adapter = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             adapter.Fill(dt);*/
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["BlogId"]);
                Console.WriteLine(reader["BlogTitle"]);
                Console.WriteLine(reader["BlogAuthor"]);
                Console.WriteLine(reader["BlogContent"]);
                //Console.WriteLine(dr["DeleteFlag"]);

                lst.Add(new BlogViewModel
                {
                    Id = Convert.ToInt32(reader["BlogId"]),
                    Title = Convert.ToString(reader["BlogTitle"]),
                    Author = Convert.ToString(reader["BlogAuthor"]),
                    Content = Convert.ToString(reader["BlogContent"]),
                    DeleteFlag = Convert.ToBoolean(reader["DeleteFlag"]),
                });
            }
            connection.Close();
            return Ok(lst);
        }

        [HttpPost]
        public IActionResult CreateBlogs(BlogViewModel Blog)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string query = $@"INSERT INTO [dbo].[Tbl_Blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent]
           ,[DeleteFlag])
           VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent
           ,0)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogTitle", Blog.Title);
            cmd.Parameters.AddWithValue("@BlogAuthor", Blog.Author);
            cmd.Parameters.AddWithValue("@BlogContent", Blog.Content);

            int result = cmd.ExecuteNonQuery();

            connection.Close();
             return Ok(result > 0 ? "Saving Successful." : "Saving Failed.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGlogs(int id, BlogViewModel Blog)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = $@"UPDATE [dbo].[Tbl_Blog]
                              SET [BlogTitle] = @BlogTitle
                              ,[BlogAuthor] = @BlogAuthor
                              ,[BlogContent] = @BlogContent
                              ,[DeleteFlag] = 0
                               WHERE  BlogId = @BlogId";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            cmd.Parameters.AddWithValue("@BlogTitle", Blog.Title);
            cmd.Parameters.AddWithValue("@BlogAuthor", Blog.Author);
            cmd.Parameters.AddWithValue("@BlogContent", Blog.Content);

            int result = cmd.ExecuteNonQuery();

            connection.Close();
            return Ok(result > 0 ? "Updating Successful." : "Updating Failed.");
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlogs(int id, BlogViewModel Blog)
        {
            string condition = "";
            if (!string.IsNullOrEmpty(Blog.Title))
            {
                condition += " [BlogTitle] = @BlogTitle, ";
            }
            if (!string.IsNullOrEmpty(Blog.Author))
            {
                condition += " [BlogAuthor] = @BlogAuthor, ";
            }
            if (!string.IsNullOrEmpty(Blog.Content))
            {
                condition += " [BlogContent] = @BlogContent, ";
            }
            if (condition.Length == 0)
            {
                return BadRequest("Invalid Parameter!");
            }
           condition = condition.Substring(0, condition.Length - 2);
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = $@"UPDATE [dbo].[Tbl_Blog] SET {condition} WHERE  BlogId = @BlogId";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            if (!string.IsNullOrEmpty(Blog.Title))
            {
                cmd.Parameters.AddWithValue("@BlogTitle", Blog.Title);
            }
            if (!string.IsNullOrEmpty(Blog.Author))
            {
                cmd.Parameters.AddWithValue("@BlogAuthor", Blog.Author);
            }
            if (!string.IsNullOrEmpty(Blog.Content))
            {
                cmd.Parameters.AddWithValue("@BlogContent", Blog.Content);
            }

            int result = cmd.ExecuteNonQuery();

            connection.Close();
            return Ok(result > 0 ? "Updating Successful." : "Updating Failed.");

        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBogs(int id)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"UPDATE [dbo].[Tbl_Blog]
               SET [DeleteFlag] = 1
             WHERE BlogId = @id";

            //to actually delete data from db
            //string query = @"DELETE FROM [dbo].[Tbl_Blog]
            //    WHERE BlogId = @id";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            int result = cmd.ExecuteNonQuery();

            connection.Close();
            return Ok(result > 0 ? "Successfully Deleted Blog!" : "Deleteing Blog Failed!");

        }

    }
}
