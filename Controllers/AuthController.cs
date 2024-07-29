using Microsoft.AspNetCore.Mvc;
using OnlineLearning.Application.Dtos;
using OnlineLearning.Application.Interfaces.ServiceInterfaces;
using OnlineLearning.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineLearning.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthenticateService _authService;
        IStudentService _studentService;
        public AuthController(IAuthenticateService authService,IStudentService studentService)
        {
            _authService= authService;  
            _studentService= studentService;    
        }
        // GET: api/<AuthController>
        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            var _user = await _authService.RegisterUser(user);
            if(_user!=null) return Ok(user);
            return BadRequest();
        }

        // GET api/<AuthController>/5
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponce>> LoginUser(UserLogin user)
        {
            var userResponce = await _authService.UserLogin(user);
            //var _users = await _studentService.GetAllStudents();
            //_users= _users.FirstOrDefault()
            
            if(userResponce != null) return Ok(userResponce);
            return Unauthorized("Please loged in");
        }

    
    }
}
