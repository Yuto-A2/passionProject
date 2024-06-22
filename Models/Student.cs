using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Student
    {

        [Key]
        public int studentId { get; set; }

        public string student_fname { get; set; }

        public string student_lname { get; set; }
    }
    public class StudentDto
    {
        public int studentId { get; set; }
        public string student_fname { get; set; }
        public string student_lname { get; set; }
    }
}