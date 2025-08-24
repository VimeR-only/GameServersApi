using GameServers.Application.Interfaces;
using GameServers.Application.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameServers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServersController : ControllerBase 
    {
        private readonly IServerService _serverService;

        public ServersController(IServerService serverService)
        {
            _serverService = serverService;
        }

        [HttpGet("servers")]
        public async Task<IActionResult> GetServers()
        {
            var servers = await _serverService.GetServers();
            return Ok(servers);
        }
    }
}
