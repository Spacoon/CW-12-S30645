using System.ComponentModel.DataAnnotations;

namespace Cwiczenia12.DTOs;

public class AssignClientToTripDto
{
    [Required]
    public string FirstName { get; set; } = null!;
    
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Telephone { get; set; } = null!;
    
    [Required]
    public string Pesel { get; set; } = null!;
    
    [Required]
    public int IdTrip { get; set; }
    
    [Required]
    public string TripName { get; set; } = null!;
    
    public DateTime? PaymentDate { get; set; }
}