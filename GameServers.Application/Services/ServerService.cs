using GameServers.Application.Interfaces;
using GameServers.Domain.Models;
using GameServers.Infrastructure.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServers.Application.Services
{
    public class ServerService : IServerService
    {
        private readonly IHtmlParser _parser;

        public ServerService(IHtmlParser parser)
        {
            _parser = parser;
        }
        public async Task<List<GameServer>> GetServers()
        {
            var html = await _parser.GetHtmlAsync("https://tsarvar.com/en/servers/garrys-mod?page=1");
            return _parser.ParseServers(html);
        }
    }
}
