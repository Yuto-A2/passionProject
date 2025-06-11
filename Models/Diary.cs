using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProject.Models
{
    public class Diary
    {
        [Key]
        public int content_Id { get; set; }
        public string title { get; set; }
        public string diary_body { get; set; }
        public DateTime Post_date { get; set; }
        public string comment { get; set; }

        [ForeignKey("Student")]
        public int studentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Teacher")]
        public int teacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Student> Students { get; set; }
    }
    public class DiaryDto
    {
        public int content_Id { get; set; }
        public string title { get; set; }
        public string diary_body { get; set; }
        public DateTime Post_date { get; set; }
        public string comment { get; set; }

        public int studentId { get; set; }
        public string student_fname { get; set; }
        public string student_lname { get; set; }

        public int teacherId { get; set; }
        public string teacher_fname { get; set; }
        public string teacher_lname { get; set; }
    }
}
