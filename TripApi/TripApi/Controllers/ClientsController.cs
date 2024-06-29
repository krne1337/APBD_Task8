using Microsoft.AspNetCore.Mvc;
using TravelAPI.Services;

namespace TravelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var result = await _clientService.DeleteClient(idClient);
            if (!result)
            {
                return BadRequest("Client has assigned trips and cannot be deleted.");
            }

            return NoContent();
        }
    }
}
