using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public int Post(Login login)
        {
            string query = @"
                    select * from dbo.Login where Username='" + login.Username + "'and Password = '" + login.Password + "'";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    if (table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        string num = (dr["Status"].ToString());
                        if (num == "1")
                            return 1;
                        else
                            return 2;
                    }
                    else
                        return 0;
                    myReader.Close();
                    myCon.Close();
                }
            }
        }
    }
}
