using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PointController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                    select PointId, SubjectsName, StudentName, Midterm, Final, Total
                    from dbo.Point, dbo.Student, dbo.Subjects where Point.Status = 1 and Point.StudentId = Student.StudentId
                    and Subjects.SubjectsId = Point.SubjectsId
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

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Point point)
        {
            float Total = (point.Final + point.Midterm) / 2;
            string query = @"
                    insert into dbo.Point
                    (SubjectsId, StudentId, Midterm ,Final, Total ,Status)
                    values 
                    (
                    '" + point.SubjectsId + @"'
                    ,'" + point.StudentId + @"'
                    ,'" + point.Midterm + @"'
                    ,'" + point.Final + @"'
                    ,'"+ Total + @"'
                    ,1
                    )
                    ";
            if (KiemTra(point.SubjectsId,point.StudentId) == 0)
            {
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
                return new JsonResult("Added Failed! Duplicate Data ");
        }


        [HttpPut]
        public JsonResult Put(Point point)
        {
            float Total = (point.Final + point.Midterm) / 2;
            string query = @"
                    update dbo.Point set
                    Final = '" + point.Final + @"'
                    ,Midterm = '" + point.Midterm + @"'
                    ,Total = '" + Total + @"'
                    where PointId = " + point.PointId + @"
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


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                    update dbo.Point set
                    Status = 0
                    where PointId = " + id + @" 
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


        [Route("GetAllStudentId")]
        public JsonResult GetAllStudentId()
        {
            string query = @"
                    select StudentId from dbo.Student
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

            return new JsonResult(table);
        }

        [Route("GetAllSubjectsId")]
        public JsonResult GetAllSubjectsId()
        {
            string query = @"
                    select SubjectsId from dbo.Subjects
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

            return new JsonResult(table);
        }
        public int KiemTra(int a,int b)
        {
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlConnection myCon = new SqlConnection(sqlDataSource);
            myCon.Open();
            string sql = "select * from Point where SubjectsId='" + a + @"' and StudentId='" + b + @"'";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myCon;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return 1;
            }
            else
            {
                return 0;
            }
            myCon.Close();
        }
    }
}
