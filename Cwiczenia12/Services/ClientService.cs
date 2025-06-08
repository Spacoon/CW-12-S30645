using Microsoft.EntityFrameworkCore;
using Cwiczenia12.Data;

namespace Cwiczenia12.Services;

public class ClientService : IClientService
{
    private readonly TripContext _context;

    public ClientService(TripContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            throw new ArgumentException("Client not found");
        }

        if (client.ClientTrips.Any())
        {
            throw new InvalidOperationException("Cannot delete client who has assigned trips");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}