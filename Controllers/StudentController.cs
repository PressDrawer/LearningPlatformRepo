using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineLearning.Application.Dtos;
using OnlineLearning.Application.Interfaces.ServiceInterfaces;
using OnlineLearning.Application.Services;
using OnlineLearning.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace OnlineLearning.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService) 
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _studentService.GetAllStudents();  
            return Ok(students);
        }

        // GET api/<CourseController>/5      
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Student>> Get(Guid id)
        {
            if (ModelState.IsValid)
            {
                var student = await _studentService.GetStudent(id);      
                return Ok(student);
            }
            return BadRequest();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Student>> Post([FromBody] StudentDto studentDto)
        {
            //if(_courseService.GetAllCourses().FirstOrDefaultAsync())

            var student = new Student()
            {
                Name = studentDto.Name,
                Age = studentDto.Age,
                Gender = studentDto.Gender,
                City = studentDto.City,
                Mobile=studentDto.Mobile,
                Email=studentDto.Email,
                Password = studentDto.Password

            };
            var _student = await _studentService.AddStudent(student);
            if (_student != null)
            {
                return Ok(studentDto);
            }

            return BadRequest("Student already exists");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> Put(Guid id, [FromBody] StudentDto studentDto)
        {
            //var _course = await _courseService.GetCourse(id);
            var student = new Student()
            {
                Name = studentDto.Name,
                Age = studentDto.Age,
                Gender = studentDto.Gender,
                City = studentDto.City,
                Mobile = studentDto.Mobile,
                Email = studentDto.Email,
                Password = studentDto.Password
            };
            student = await _studentService.UpdateStudent(student,id);
            if (student != null) return Ok($"Updated Student Successfully");

            return BadRequest("No Student found");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Student>> Delete(Guid id)
        {
            var student = await _studentService.DeleteStudent(id);
            if (student != null)
            {
                return Ok(student + "Successfully deleted");
            }
            return NotFound("No student found");

        }
        [HttpGet("EnrolledCourses")]
        public async Task<ActionResult<ICollection<CourseView>>> GetEnrolledCourse(Guid studentId)
        {
            var courses = await _studentService.GetEnrolledCources(studentId);
            return Ok(courses);
        }

        [HttpPost("Enroll")]
        //[Authorize]
        public async Task<ActionResult<EnrollmentDto>> EnrollonCourse([FromQuery] EnrollmentDto enroll)
        {
            enroll = await _studentService.EnrollCourses(enroll);
                if (enroll != null) return Ok(enroll);
            return BadRequest(); 
        }
    }
}
