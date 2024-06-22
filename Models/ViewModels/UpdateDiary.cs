using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class UpdateDiary
    {
        public DiaryDto SelectedContent { get; set; }
        public IEnumerable<DiaryDto> StudentsOptions { get; set; }
    }
}