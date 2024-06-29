using TravelAPI.DTOs;
using TravelAPI.Models;

public interface ITripService
{
    Task<IEnumerable<Trip>> GetTrips(int page, int pageSize);
    Task<bool> AssignClientToTrip(int idTrip, ClientDto clientDto);
}
