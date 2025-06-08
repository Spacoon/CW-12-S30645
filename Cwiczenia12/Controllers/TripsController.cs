using Cwiczenia12.DTOs;
using Cwiczenia12.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            return BadRequest("Page number must be at least 1");
        }

        if (pageSize < 1)
        {
            return BadRequest("Page size must be at least 1");
        }

        var result = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, AssignClientToTripDto dto)
    {
        try
        {
            if (dto.IdTrip != idTrip)
            {
                return BadRequest("Trip ID in the URL and request body do not match");
            }

            await _tripService.AssignClientToTripAsync(idTrip, dto);
            return Ok("Client successfully assigned to the trip");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
