using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Script.Serialization;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
namespace PassionProject.Controllers
{
    public class StudentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StudentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44387/api/");
        }
        //GET: Student/List
        public ActionResult List()
        {
            string url = "studentdata/liststudents";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<StudentDto> Students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            return View(Students);
        }

        //GET: Student/Details/1
        public ActionResult Details(int id)
        {
            DetailsStudent ViewModel = new DetailsStudent();
            //objective: communication with our Student data api to retrieve one Student.
            //curl https://localhost:44387/api/Studentdata/findStudent/{id}
            string url = "studentdata/findStudent" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is: ");
            Debug.WriteLine(response.StatusCode);

            StudentDto SelectedStudent = response.Content.ReadAsAsync<StudentDto>().Result;
            Debug.WriteLine("Student received: ");
            Debug.WriteLine(SelectedStudent.student_fname);

            ViewModel.SelectedStudent = SelectedStudent;

            url = "StudentData/ListStudentsForDiary/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DiaryDto> KeptDiaries = response.Content.ReadAsAsync<IEnumerable<DiaryDto>>().Result;

            ViewModel.KeptDiaries = KeptDiaries;

            return View(ViewModel);

        }
        public ActionResult Error()
        {
            return View();
        }

        //GET: Student/New
        public ActionResult New()
        {
            return View();
        }

        //POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student Student)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(Student.student_fname);
            //Objective: add a new Student into our system using the API
            //curl -H "Content-Type:application/json" -d @Student.json Https://localhost:44387/api/StudentData/AddStudent
            string url = "StudentData/AddStudent";

            string jsonpayload = jss.Serialize(Student);
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

        //GET: Student/Edit/16
        public ActionResult Edit(int id)
        {
            string url = "studentdata/findstudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudentDto selectedStudent = response.Content.ReadAsAsync<StudentDto>().Result;
            return View(selectedStudent);
        }
        //POST: Student/Update/6
        [HttpPost]
        public ActionResult Update(int id, Student Student)
        {
            string url = "studentdata/updatestudent/" + id;
            string jsonpayload = jss.Serialize(Student);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Student/Delete/6
        public ActionResult DeleteConfirm(int id)
        {
            string url = "studentdata/findstudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudentDto selectedStudent = response.Content.ReadAsAsync<StudentDto>().Result;
            return View(selectedStudent);
        }

        //POST: Student/Delete/6
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "studentdata/deletestudent/" + id;
            HttpContent content = new StringContent("");
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
    }

}