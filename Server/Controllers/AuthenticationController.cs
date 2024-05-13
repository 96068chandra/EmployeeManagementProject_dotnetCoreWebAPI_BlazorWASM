using BaseLibrary.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repository.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync(Register register)
        {
            if (register == null)
            {
                return BadRequest("Model is empty");
            }
            var result=await userAccount.CreateAsync(register);
            return Ok(result);
            

        }

        [HttpPost("Login")]
        public async Task<IActionResult> SingInAsync(Login user)
        {
            if(user == null)  return BadRequest("Model is Empty");
            var result=await userAccount.SingInAsync(user);
            return Ok(result);

            
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken token)
        {
            if(token == null)
            {
                return BadRequest("Model is empty");
            }
            var result= await userAccount.RefreshTokenAsync(token);
            return Ok(result);
        }
        

    }
}
