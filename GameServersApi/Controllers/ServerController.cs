using GameServers.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameServers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _serverService;

        public ServerController(IServerService serverService)
        {
            _serverService = serverService;
        }


        [HttpGet("{game}/{ip}")]
        public async Task<IActionResult> GetServerIp(string game, string ip)
        {
            var server = await _serverService.GetServerIpAsync(game, ip);

            return Ok(server);
        }
    }
}
