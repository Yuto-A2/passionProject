using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class DetailsTeacher
    {
        public TeacherDto SelectedTeachers { get; set; }
        public IEnumerable<DiaryDto> RelatedContents { get; set; }

    }
}