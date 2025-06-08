using Cwiczenia12.DTOs;

namespace Cwiczenia12.Services;

public interface ITripService
{
    Task<TripsResponseDto> GetTripsAsync(int page = 1, int pageSize = 10);
    Task<bool> AssignClientToTripAsync(int idTrip, AssignClientToTripDto dto);
}