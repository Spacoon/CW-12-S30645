namespace Cwiczenia12.DTOs;

public class TripsResponseDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public IEnumerable<TripDto> Trips { get; set; } = new List<TripDto>();
}