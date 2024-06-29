using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs;
using TravelAPI.Models;

namespace TravelAPI.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext _context;

        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetTrips(int page, int pageSize)
        {
            return await _context.Trips
                .Include(t => t.CountryTrips).ThenInclude(ct => ct.Country)
                .Include(t => t.ClientTrips).ThenInclude(ct => ct.Client)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> AssignClientToTrip(int idTrip, ClientDto clientDto)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now)
            {
                return false;
            }

            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);

            if (existingClient != null)
            {
                return false;
            }

            var clientInTrip = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);

            if (clientInTrip)
            {
                return false;
            }

            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
