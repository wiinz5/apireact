using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Point
    {
        public int PointId { get; set; }
        public int SubjectsId { get; set; }
        public int StudentId { get; set; }
        public float Midterm { get; set; }
        public float Final { get; set; }
    }
}
