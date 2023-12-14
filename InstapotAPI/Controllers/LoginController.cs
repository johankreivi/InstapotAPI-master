using InstapotAPI.Dtos.Profile;
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InstapotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IProfileRepository _profileReposetory;

        private readonly ILogger<LoginController> _logger;
        
        public LoginController(IProfileRepository profileReposetory, ILogger<LoginController> logger) 
        {
            _profileReposetory = profileReposetory; 
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Profile>> Create(Profile profile)
        {
            var createdProfile = await _profileReposetory.Create(profile);

            return CreatedAtAction("Get", new { id = createdProfile.Id }, createdProfile);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout(int id)
        {
            var profileUpdateLoginStatus = await _profileReposetory.SetLoginStatusToFalse(id);

            if (profileUpdateLoginStatus == null)
            {
                return NotFound(id);
            }

            return Ok(profileUpdateLoginStatus);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(int id)
        {
            var profileUpdateLoginStatus = await _profileReposetory.SetLoginStatusToTrue(id);

            if (profileUpdateLoginStatus == null)
            {
                return NotFound(id);
            }

            return Ok(profileUpdateLoginStatus);
        }


    }
}
