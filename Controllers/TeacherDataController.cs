using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject.Models;
using System.Diagnostics;
using PassionProject.Migrations;

namespace PassionProject.Controllers
{
    public class TeacherDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Lists the teachers in the database
        /// </summary>
        /// <returns>
        /// An array of teachers dtos.
        /// </returns>
        //GET: api/TeacherData/ListTeachers->
        //[{"teacherId":1, "student_fname": Angela, "student_lname": Smith}],
        //[{"teacherId":2, "student_fname": Daniel, "student_lname": Joe}]
        [HttpGet]
        [Route("api/TeacherData/ListTeachers")]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult ListTeachers()
        {
            List<Teacher> Teachers = db.Teachers.ToList();

            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(teacher => TeacherDtos.Add(new TeacherDto()
            {
                teacherId = teacher.teacherId,
                teacher_fname = teacher.teacher_fname,
                teacher_lname = teacher.teacher_lname,
            }));

            return Ok(TeacherDtos);
        }
        [ResponseType(typeof(TeacherDto))]
        [Route("api/TeacherData/ListTeachersForDiary/{id}")]
        [HttpGet]
        public IHttpActionResult ListTeachersForDiary(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            TeacherDto TeacherDto = new TeacherDto()
            {
                teacherId = id,
                teacher_fname = teacher.teacher_fname,
                teacher_lname = teacher.teacher_lname,
            };
            if (teacher == null)
            {
                return NotFound();
            }
            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return Ok();
        }
        [ResponseType(typeof(TeacherDto))]
        [Route("api/TeacherData/FindTeacher/{id}")]
        [HttpGet]
        public IHttpActionResult FindTeacher(int id)
        {
            Teacher Teacher = db.Teachers.Find(id);
            TeacherDto TeacherDto = new TeacherDto()
            {
                teacherId = id,
                teacher_fname = Teacher.teacher_fname,
                teacher_lname = Teacher.teacher_lname
            };
            if (Teacher == null)
            {
                return NotFound();
            }
            return Ok(TeacherDto);
        }

        /// <summary>
        /// Adds a teacher in the database
        /// </summary>
        /// <param name="Studnet">JSON FROM DATA of an Teacher</param>
        /// <returns>
        /// HEADER: 201(Created)
        /// CONTENT: Teacher ID, Teacher Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TeacherData/AddTeacher
        /// FROM DATA: Teacher JSON Object
        /// </example>

        [ResponseType(typeof(Teacher))]
        [HttpPost]
        public IHttpActionResult AddTeacher(Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(Teacher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Teacher.teacherId }, Teacher);
        }
        /// <summary>
        /// Updates a particular teacher in the database
        /// </summary>
        /// <param name="Teacher">JSON FROM DATA of a Teacher</param>
        /// <returns>
        /// HEADER: 204(Success, No Content Response)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TeacherData/UpdateTeacher/5
        /// FROM DATA: Teacher JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTeacher(int id, Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Teacher.teacherId)
            {
                return BadRequest();
            }
            db.Entry(Teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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
        /// Delete a teacher from the database
        /// </summary>
        /// <param name="id">The primary key of the teacher</param>
        /// <returns>
        /// HEADER: 200(Ok)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TeacherData/DeleteTeacher/5
        /// FROM DATA: Teacher JSON Object
        /// </example>

        [ResponseType (typeof(teacher))]
        [HttpPost]
        public IHttpActionResult DeleteTeacher(int id)
        {
            Teacher Teacher = db.Teachers.Find(id);
            if (Teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(Teacher);
            db.SaveChanges();

            return Ok();
        }

        protected　override void Dispose (bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose (disposing);
        }

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.teacherId == id) > 0;
        }
    }
}

