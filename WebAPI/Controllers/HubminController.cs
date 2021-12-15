using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HubminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                    select Username, Password, Status from dbo.Login";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Hubmin hubmin)
        {
            string q = @" select * from dbo.Login where Username = '" + hubmin.Username + @"'";

            DataTable tbl = new DataTable();
            string DataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader Reader;
            using (SqlConnection Con = new SqlConnection(DataSource))
            {
                Con.Open();
                using (SqlCommand Command = new SqlCommand(q, Con))
                {
                    Reader = Command.ExecuteReader();
                    tbl.Load(Reader); ;
                    if (tbl.Rows.Count < 1)
                    {
                        string query = @"
                    insert into dbo.Login
                    (Username,Password,Status)
                    values 
                    (
                    '" + hubmin.Username + @"'
                    ,'" + hubmin.Password + @"'
                    ,'" + hubmin.Status + @"'
                    )
                    ";
                        DataTable table = new DataTable();
                        string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                        SqlDataReader myReader;
                        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                        {
                            myCon.Open();
                            using (SqlCommand myCommand = new SqlCommand(query, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                table.Load(myReader); ;

                                myReader.Close();
                                myCon.Close();
                            }
                        }
                        return new JsonResult("Added Successfully");
                    }else
                        return new JsonResult("Add Error");
                    Reader.Close();
                    Con.Close();
                }
            }
        }


        [HttpPut]
        public JsonResult Put(Hubmin hubmin)
        {
            string query = @"
                    update dbo.Login set 
                    Password = '" + hubmin.Password + @"'
                    ,Status = '" + hubmin.Status +@"'
                    where Username = '" + hubmin.Username + @"'
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{user}")]
        public JsonResult Delete(string user)
        {
            string query = @"
                    Delete from dbo.Login
                    where Username = '" + user + @"'
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
