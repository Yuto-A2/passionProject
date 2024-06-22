using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PassionProject.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PassionProject.Controllers
{
    public class StudentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Return all students in the database
        /// </summary>
        /// <returns>
        /// CONTENT: all Students in the database, including their associated teachers.
        /// </returns>
        //GET: api/StudentData/ListStudents->
        //[{"studentId":1, "student_fname": John, "student_lname": Lennon}],
        //[{"studentId":2, "student_fname": Paul, "student_lname": McCartney}]
        [HttpGet]
        [Route("api/StudentData/ListStudents")]
        [ResponseType(typeof(StudentDto))]
        public IHttpActionResult ListStudents()
        {
            List<Student> Students = db.Students.ToList();

            List<StudentDto> StudentDtos = new List<StudentDto>();

            Students.ForEach(s => StudentDtos.Add(new StudentDto()
            {
                studentId = s.studentId,
                student_fname = s.student_fname,
                student_lname = s.student_lname
            }));

            return Ok(StudentDtos);
        }
        ///<summary>
        ///Returns Studnents in the system not caring for a particular student.
        /// </summary>
        /// <returns>
        /// CONTENT: all students in the database writing a diary
        /// </returns>
        /// <param name="id">Content Primary Key</param>
        /// <example>
        /// GET: api/StudentData/ListDiariesForStudent/1
        /// </example>
        [HttpGet]
        [Route("api/StudentData/ListStudentsForDiary/{id}")]
        [ResponseType(typeof(StudentDto))]
        public IHttpActionResult ListStudentsForDiary(int id)
        {
            List<Student> Students = db.Students.ToList();

            List<StudentDto> StudentDtos = new List<StudentDto>();
            Students.ForEach(s => StudentDtos.Add(new StudentDto()
            {
                studentId = s.studentId,
                student_fname = s.student_fname,
                student_lname = s.student_lname,
            }));
            return Ok(StudentDtos);
        }

        /// <summary>
        /// Return all Students in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200(OK)
        /// CONTENT: A Student in the system matching up to the Student ID primary Key
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <param name="id">The primary key of the student</param>
        /// <example>
        /// GET: api/StudentData/FindStudent/1
        ///<studentId>1</studentId>
        ///<student_fname>John</student_fname>
        ///<student_lname>Lennon</student_lname>
        /// </example>
        [ResponseType(typeof(StudentDto))]
        [Route("api/StudentData/FindStudent/{id}")]
        [HttpGet]
        public IHttpActionResult FindStudent(int id)
        {
            Student Student = db.Students.Find(id);
            StudentDto StudentDto = new StudentDto()
            {
                studentId = Student.studentId,
                student_fname = Student.student_fname,
                student_lname = Student.student_lname
            };
            if (Student == null)
            {
                return NotFound();
            }
            return Ok(StudentDto);
        }
        /// <summary>
        /// Adds a particular Student in the system with POST Data input.
        /// </summary>
        /// <returns>
        /// HEADER: 201(Created)
        /// CONTENT: A Student data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/StudentData/AddStudent/
        ///FORM DATA: Student JSON Object
        ///</example>

        [ResponseType(typeof(Student))]
        [HttpPost]
        public IHttpActionResult AddStudent(Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(Student);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = Student.studentId }, Student);
        }

        /// <summary>
        /// Updates a particular Student in the system with POST Data input.
        /// </summary>
        /// <returns>
        /// HEADER: 204(Success, No Content Response)
        /// CONTENT: A Student in the system matching up to the Student ID primary Key
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <param name="id">Represents the Student ID primary key</param>
        /// <param name="Student">JSON FROM DATA of a student</param>
        /// <example>
        /// POST: api/StudentData/UpdateStudent/1
        ///FORM DATA: Student JSON Object

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStudent(int id, Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Student.studentId)
            {
                return BadRequest();
            }

            db.Entry(Student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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
        /// Delete a Student from the database by its ID.
        /// </summary>
        /// <returns>
        /// HEADER: 200(Ok)
        /// CONTENT: A Student in the system matching up to the Student ID primary Key
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <param name="id">The primary key of the Student</param>
        /// <example>
        /// POST: api/StudentData/DeleteStudent/6
        ///FORM DATA: (empty)

        [ResponseType(typeof(Student))]
        [HttpPost]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            db.Students.Remove(student);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.studentId == id) > 0;
        }
    }
}
