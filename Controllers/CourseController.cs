using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearning.Application.Dtos;
using OnlineLearning.Application.Interfaces.ServiceInterfaces;
using OnlineLearning.Domain.Models;
using System.Data;
//using System.Web.Http;
using System.Web.Http.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineLearning.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService= courseService;  
        }
        // GET: api/<CourseController>
        
        [HttpGet("courses")]
        
        public async Task<ActionResult<IEnumerable<Course>>> GetAll()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses); 
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Course>> Get(Guid id)
        {
            if (ModelState.IsValid)
            {
                var course = await _courseService.GetCourse(id);
                return Ok(course);
            }
            return BadRequest();
        }

        // POST api/<CourseController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Course>> Post([FromBody] CourseDto coursedto)
        {
            //if(_courseService.GetAllCourses().FirstOrDefaultAsync())
           
                var course = new Course()
                {
                    //CourseId = new Guid(),
                    CourseName = coursedto.CourseName,
                    CourseCategory = coursedto.CourseCategory,
                    CourseFee = coursedto.CourseFee
                };
                var _course = await _courseService.AddCourse(course);
                if (_course != null)
                {
                    return Ok(course);
                }
             
            return BadRequest("Course Name already exists");
        }

        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Course>> Put(Guid id, [FromBody] CourseDto coursedto)
        {
            //var _course = await _courseService.GetCourse(id);
                var course = new Course()
                {
                    CourseName = coursedto.CourseName,
                    CourseCategory = coursedto.CourseCategory,
                    CourseFee = coursedto.CourseFee 

                };
             course =   await _courseService.UpdateCourse(course, id);
            if (course != null) return Ok($"Updated Course Successfully");     
            
            return BadRequest("No Course found");
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Course>> Delete(Guid id)
        {
            var course = await _courseService.DeleteCourse(id);
            if (course != null)
            {
                return Ok(course);
            }
            return NotFound();

        }
    }
}
