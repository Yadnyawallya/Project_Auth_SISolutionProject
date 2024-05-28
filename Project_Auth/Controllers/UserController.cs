using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Auth.context;
using Project_Auth.helpers;
using Project_Auth.models;

namespace Project_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            var user = await _dbContext.users
                .FirstOrDefaultAsync(x => x.UserName == userObj.UserName 
                && x.Password ==userObj.Password);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User Is Not Found"
                });
            }
            return Ok(new
            {
                Message= " Login Sucessful!!..."
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userObj)
        {
            if(userObj == null)
            {
                return BadRequest();
            }
            userObj.Password = PasswordHasher.HashPasword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _dbContext.users.AddAsync(userObj);
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                Message=" User Register Sucessfully!!... "
            });
        }



    }
}
