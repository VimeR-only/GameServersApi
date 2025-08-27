using GameServers.Application.Interfaces;
using GameServers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameServers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameServer;

        public GameController(IGameService gameServer)
        {
            _gameServer = gameServer;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetGames()
        {
            var games = await _gameServer.GetGames();

            return Ok(games);
        }
    }
}
