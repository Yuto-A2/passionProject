using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class DetailsStudent
    {
        public StudentDto SelectedStudent { get; set; }
        public IEnumerable<DiaryDto> KeptDiaries { get; set; }

    }
}