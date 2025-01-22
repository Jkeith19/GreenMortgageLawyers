using BusinessServices.Services;
using Entity.DTOs;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LVR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LVRController : Controller
    {
        private readonly ILvrService _service;

        public LVRController(ILvrService service)
        {
            _service = service;
        }

        // POST: LVRController/Create
        [HttpPost]
        [ProducesResponseType(typeof(LVRDto), 201)]
        [ProducesResponseType(typeof(LVRDto), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CalculateAndSaveAsync([FromBody] InputLVR input)
        {
            try
            {
                var result = await _service.CalculateLVRAsync(input);
                return StatusCode(result.StatusCode, result);
            }
            catch 
            { 
                return StatusCode(500, "Internal Service Error!");
            }
        }
    }
}
