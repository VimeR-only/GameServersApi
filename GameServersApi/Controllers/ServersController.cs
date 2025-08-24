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

        [HttpGet("{game}")]
        public async Task<IActionResult> GetServers(string game, int page = 1)
        {
            var servers = await _serverService.GetServers(game, page);
            return Ok(servers);
        }
        
        [HttpGet("{game}/all")]
        public async Task<IActionResult> GetAllServers(string game)
        {
            var servers = await _serverService.GetAllServersAsync(game);
            return Ok(servers);
        }
    }
}
