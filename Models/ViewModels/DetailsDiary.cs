using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class DetailsDiary
    {
        public DiaryDto SelectedDiary { get; set; }
        public IEnumerable<StudentDto> ResponsibleStudents { get; set; }
        //public IEnumerable<StudentDto> AvailableStudents { get; set; }
        public IEnumerable<TeacherDto> ResponsibleTeachers { get; set; }
    }
}