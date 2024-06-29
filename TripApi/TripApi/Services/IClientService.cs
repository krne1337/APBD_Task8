using TravelAPI.Models;

public interface IClientService
{
    Task<bool> DeleteClient(int idClient);
}
