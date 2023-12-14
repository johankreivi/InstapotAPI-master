using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InstapotAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileRepository _profileRepo;
    public ProfileController(IProfileRepository profileRepo)
    {
        _profileRepo = profileRepo;
    }

    [HttpGet]
    [Route("ProfileName/{id}")]
    public async Task<ActionResult<String>> GetName(int id)
    {
        var profile = await _profileRepo.Profile(id);
        if (profile == null) return NotFound();
        return Ok(profile.Username);
    }
}
