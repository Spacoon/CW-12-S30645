using Microsoft.EntityFrameworkCore;
using Cwiczenia12.Data;
using Cwiczenia12.DTOs;
using Cwiczenia12.Models;

namespace Cwiczenia12.Services;

public class TripService : ITripService
{
    private readonly TripContext _context;

    public TripService(TripContext context)
    {
        _context = context;
    }

    public async Task<TripsResponseDto> GetTripsAsync(int page = 1, int pageSize = 10)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips
            .Include(t => t.IdCountries)  
            .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();

        return new TripsResponseDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        };
    }

    public async Task<bool> AssignClientToTripAsync(int idTrip, AssignClientToTripDto dto)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            throw new ArgumentException("Trip not found");
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new InvalidOperationException("Cannot register for a trip that has already started");
        }

        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (existingClient != null)
        {
            var existingRegistration = await _context.ClientTrips
                .FirstOrDefaultAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);
            
            if (existingRegistration != null)
            {
                throw new InvalidOperationException("Client is already registered for this trip");
            }

            var clientTrip = new ClientTrip
            {
                IdClient = existingClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
        }
        else
        {
            var newClient = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            var clientTrip = new ClientTrip
            {
                IdClient = newClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
        }

        await _context.SaveChangesAsync();
        return true;
    }
}

