
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassId { get; set; }
        public string DateOfBirth { get; set; }
        public string PhotoFileName { get; set; }

    }
}
