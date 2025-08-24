using GameServers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServers.Infrastructure.Parsers
{
    public interface IHtmlParser
    {
        Task<(string, System.Net.HttpStatusCode)> GetHtmlAsync(string url);
        List<GameServer> ParseServersList(string html);
    }
}
