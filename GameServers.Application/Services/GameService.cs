using GameServers.Application.Interfaces;
using GameServers.Domain.Models;
using GameServers.Infrastructure.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameServers.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IHtmlParser _parser;

        public GameService(IHtmlParser parser)
        {
            _parser = parser;
        }

        public async Task<List<Game>> GetGames()
        {
            var (content, statusCode) = await _parser.GetHtmlAsync($"https://tsarvar.com/en/games");

            if (statusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"[ERROR] Failed to load main page");

                throw new Exception("[ERROR] Failed to load main page.");
            }

            var games = _parser.ParseGames(content);

            return games;
        }
    }
}
