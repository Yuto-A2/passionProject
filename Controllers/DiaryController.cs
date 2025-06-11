using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using System.Web.Script.Serialization;
using PassionProject.Migrations;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Security.Provider;

namespace PassionProject.Controllers
{
    public class DiaryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DiaryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }

        // GET: Diary/List
        public ActionResult List()
        {
            //curl localhost:44387/api/DiaryData/ListDiaries 
            //objective: communicate with our content data api to retrieve a list of contents

            string url = "DiaryData/ListDiaries";

            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode); 

            IEnumerable<DiaryDto> diaries = response.Content.ReadAsAsync<IEnumerable<DiaryDto>>().Result;
            //Views/Content/List.cshtml

            return View(diaries);
        }

        //GET: Content/Details/3
        public ActionResult Details(int id)
        {
            DetailsDiary ViewModel = new DetailsDiary();
            //curl Https://localhost:44302/api/DiaryData/FindDiary/{id}
            //This command works.

            string url = "DiaryData/FindDiary/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DiaryDto SelectedContent = response.Content.ReadAsAsync<DiaryDto>().Result;
            Debug.WriteLine("Diary received ");
            Debug.WriteLine(SelectedContent.teacher_lname);
            //Views/Content/Show.cshtml


            ViewModel.SelectedDiary = SelectedContent;

            url = "StudentData/ListDiariesForstudent" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> ResponsibleStudents = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;

            ViewModel.ResponsibleStudents = ResponsibleStudents;

            url = "TeacherData/ListDiariesForTeacher" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TeacherDto> ResponsibleTeachers = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;

            ViewModel.ResponsibleTeachers = ResponsibleTeachers;

            return View(ViewModel);
        }

        //GET: Diary/New
       public ActionResult New()
        {
            //information about all diaries in the system.
            //GET api/studentsdata/liststudents

            string url = "studentdata/liststudents";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> StudentOptions = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            return View(StudentOptions);
        }

        //POST: Diary/Create
        [HttpPost]
        public ActionResult Create(Diary diary)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(diary.title);
            //objective: add a new diary into our system using the API
            //curl -H "Content-Type:application/json" -d @diary.json http://localhost:44302/api/diarydata/adddiary

            string url = "diarydata/adddiary";

            string jsonpayload = jss.Serialize(diary);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Diary/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDiary ViewModel = new UpdateDiary();

            //the existing diary information
            string url = "diarydata/finddiary/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DiaryDto SelectedContent = response.Content.ReadAsAsync<DiaryDto>().Result;
            ViewModel.SelectedContent = SelectedContent;

            //all students to choose from when updating this diary
            //the exsisiting diary information
            url = "studentdata/liststudents/";
            response = client.GetAsync(url).Result;
            IEnumerable<DiaryDto> StudentsOptions = response.Content.ReadAsAsync<IEnumerable<DiaryDto>>().Result; 
            ViewModel.StudentsOptions = StudentsOptions;
            return View(ViewModel);
        }

        // GET: Diary/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "diarydata/finddiary" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DiaryDto selecteddiary = response.Content.ReadAsAsync<DiaryDto>().Result;
            return View(selecteddiary);
        }

        //POST: Diary/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "diarydata/deletediary" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if ( response.IsSuccessStatusCode )
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
          
        }
    }
}