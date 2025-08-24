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

        [HttpGet("garrys-mod")]
        public async Task<IActionResult> GetServers(int page = 1)
        {
            var servers = await _serverService.GetServers(page);
            return Ok(servers);
        }
        
        [HttpGet("garrys-mod/all")]
        public async Task<IActionResult> GetAllServers()
        {
            var servers = await _serverService.GetAllServersAsync();
            return Ok(servers);
        }
    }
}
