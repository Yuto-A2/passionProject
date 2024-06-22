using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Teacher
    {
        [Key]
        public int teacherId { get; set; }
        public string teacher_fname { get; set; }
        public string teacher_lname { get; set; }

    }
    public class TeacherDto
    {
        public int teacherId { get; set; }
        public string teacher_fname { get; set; }
        public string teacher_lname { get; set; }
    }
}
