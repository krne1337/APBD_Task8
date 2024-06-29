using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.Models;

namespace TravelAPI.Services
{
    public class ClientService : IClientService
    {
        private readonly DbContext _context;

        public ClientService(DbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null || client.ClientTrips.Any())
            {
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
