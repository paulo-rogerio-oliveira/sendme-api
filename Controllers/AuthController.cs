using Microsoft.AspNetCore.Mvc;
using SendMe.Models;
using SendMe.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SendMe.Controllers
{
    public class LoginRequest {

        [Required(AllowEmptyStrings =false)]        
        public string? login { get; set; }
        [Required(AllowEmptyStrings =false)]
        public string? password { get; set; }
        public string? email { get; set; }


    }

    public class AuthResponse {
        
        public string? login { get; set; }
        public string? occupation { get; set; }

        public string? token { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IPersistence<AuthUser> _persistence;

        public AuthController(IPersistence<AuthUser> persistence) {

            _persistence = persistence;
        }

        // POST api/<AuthController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest usr)
        {

            if (String.IsNullOrEmpty(usr.login))
            {
                ModelState.AddModelError("login", "Inválid login.");
            }

            if (String.IsNullOrEmpty(usr.password))
            {
                ModelState.AddModelError("password", "Inválid password.");
            }

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            var result = await _persistence.FindAll();
            var resultUser = result.ToList().Find( u => u.Login == usr.login&& u.Password == usr.password);
            
            if (resultUser != null)
            {
                var generatedToken = TokenService.GenerateToken(new Models.APIConsumer { login = usr.login, password = usr.password, role = resultUser.Occupation});

                return Ok( new AuthResponse { login = resultUser.Login, occupation = resultUser.Occupation, token = generatedToken });
            }
            else {

                ModelState.AddModelError("errors", "Inválid login information.");
                return BadRequest(ModelState);
            }


        }

        [HttpPut]
        public async  Task<IActionResult> Register([FromBody] LoginRequest usr){

            if (String.IsNullOrEmpty(usr.login))
            {
                ModelState.AddModelError("login", "Inválid login.");

            }
            
            if (String.IsNullOrEmpty(usr.password))
            {
                ModelState.AddModelError("password", "Inválid password.");
      
            }

            if (String.IsNullOrEmpty(usr.email))
            {
                ModelState.AddModelError("email", "Inválid email.");
                
            }
            if(!ModelState.IsValid) return UnprocessableEntity(ModelState);

            var newUser = await _persistence.Save(new AuthUser { Login = usr.login, Occupation = "user", Password = usr.password });
            return StatusCode(StatusCodes.Status201Created, newUser);
        
        }

    }
}
