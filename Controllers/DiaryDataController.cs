using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject.Models;
using System.Diagnostics;
using System.Web.UI.WebControls;


namespace PassionProject.Controllers
{
    public class DiaryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Lists the diaries in the database
        /// </summary>
        /// <returns>
        /// An array of contents dtos.
        /// </returns>
        /// curl localhost:44387/api/DiaryData/ListDiaries
        //GET: api/DiaryData/ListDiaries->
        //[{"contentId":1, "content": Fui a Mexio., "Date": May 30, "comment": Goode job, }],
        [HttpGet]
        [Route("api/DiaryData/ListDiaries")]
        [ResponseType(typeof(DiaryDto))]
        public IHttpActionResult ListDiaries()
        {
            List<Diary> Diaries = db.Diaries.ToList();

            List<DiaryDto> DiaryDtos = new List<DiaryDto>();

            Diaries.ForEach(diary => DiaryDtos.Add(new DiaryDto()
            {
                content_Id = diary.content_Id,
                title = diary.title,
                diary_body = diary.diary_body,
                Post_date = diary.Post_date,
                comment = diary.comment,
                student_fname = diary.Student.student_fname,
                student_lname = diary.Student.student_lname,
                teacher_fname = diary.Teacher.teacher_fname,
                teacher_lname = diary.Teacher.teacher_lname
            }));


            return Ok(DiaryDtos);
        }
        /// <summary>
        /// Gathers information about all contents related to a particular teachers Id
        /// </summary>
        /// <returns>
        /// Content: all contents in the database, including their associated teachers matched with a particular teachers ID. 
        /// </returns>
        /// <param name="id">teachers ID.</param>
        /// The id of the content.
        /// </param>
        //GET: api/DiaryData/ListDiariesForTeachers/1->
        //[{"contentId":1, "content": Fui a Mexio., "Date": May 30, "comment": Goode job, }],
        [HttpGet]
        [Route("api/TeacherData/ListDiariesForTeacher/{id}")]
        [ResponseType(typeof(DiaryDto))]
        public IHttpActionResult ListDiariesForTeachers(int id)
        {
            //SQL equivalent:
            //SELECT title, diary_body, comment, teacher_fname, teacher_lname FROM Diaries JOIN Teachers on Diaries.content_id = Teachers.teacherId;
            List<Diary> Diary = db.Diaries.ToList();
            List<DiaryDto> DiaryDtos = new List<DiaryDto>();

            Diary.ForEach(d => DiaryDtos.Add(new DiaryDto()
            {
                content_Id = id,
                title = d.title,
                diary_body = d.diary_body,
                Post_date = d.Post_date,
                comment = d.comment,
                student_fname = d.Student.student_fname,
                student_lname = d.Student.student_lname,
                teacher_fname = d.Teacher.teacher_fname,
                teacher_lname = d.Teacher.teacher_lname
            }));

            return Ok(DiaryDtos);
        }

        [HttpGet]
        [Route("api/StudentData/ListDiariesForstudent/{id}")]
        [ResponseType(typeof(DiaryDto))]
        public IHttpActionResult ListDiariesForStudent(int id)
        {
            List<Diary> Diary = db.Diaries.ToList();
            List<DiaryDto> DiaryDtos = new List<DiaryDto>();

            Diary.ForEach(d => DiaryDtos.Add(new DiaryDto()
            {
                content_Id = id,
                title = d.title,
                diary_body = d.diary_body,
                Post_date = d.Post_date,
                comment = d.comment,
                student_fname = d.Student.student_fname,
                student_lname = d.Student.student_lname,
                teacher_fname = d.Teacher.teacher_fname,
                teacher_lname = d.Teacher.teacher_lname

            }));
            return Ok(DiaryDtos);
        }
        [ResponseType(typeof(DiaryDto))]
        [Route("api/DiaryData/FindDiary/{id}")]
        [HttpGet]
        public IHttpActionResult FindDiary(int id)
        {
            Diary Diary = db.Diaries.Find(id);
            DiaryDto DiaryDto = new DiaryDto()
            {
                content_Id = id,
                diary_body = Diary.diary_body,
                title = Diary.title,
                Post_date = Diary.Post_date,
                comment = Diary.comment,
                teacherId = Diary.teacherId,
                teacher_fname = Diary.Teacher.teacher_fname,
                teacher_lname = Diary.Teacher.teacher_lname,
                studentId = Diary.studentId,
                student_fname = Diary.Student.student_fname,
                student_lname = Diary.Student.student_lname
            };
            if (Diary == null)
            {
                return NotFound();
            }
            return Ok(DiaryDto);
        }

        /// <summary>
        /// Adds a diary to the system
        /// </summary>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Diary ID, Diary Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DiaryData/AddDiary  
        /// FORM DATA: Diary JSON Object
        /// </example>

        [ResponseType(typeof(Diary))]
        [Route("api/DiaryData/AddDiary/")]
        [HttpPost]
        public IHttpActionResult AddDiary(Diary diary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Diaries.Add(diary);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = diary.content_Id }, diary);
        }

        /// <summary>
        /// Updates a particular diary to the system
        /// </summary>
        /// <param name="id">Represents the Student Id primary key
        /// </param>
        /// <param name="diary">JSON FORM DATA of a diary</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or 
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DiaryData/UpdateDiary/5 
        /// FORM DATA: Diary JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDiary(int id, Diary diary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != diary.content_Id)
            {
                return BadRequest();
            }

            db.Entry(diary).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;

                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete a particular diary from the system
        /// </summary>
        /// <param name="id">The primary key of the diary
        /// </param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DiaryData/DeleteDiary/5 
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Diary))]
        [HttpPost]
        public IHttpActionResult DeleteDiary(int id)
        {
            Diary diary = db.Diaries.Find(id);
            if (diary == null)
            {
                return NotFound();
            }
            db.Diaries.Remove(diary);
            db.SaveChanges();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
          if (disposing)
            {
                db.Dispose();
            }
          base.Dispose(disposing);
        }

        private bool DiaryExists (int id)
        {
            return db.Diaries.Count(e => e.content_Id == id) > 0;
        }
    }
}
