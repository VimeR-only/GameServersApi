using GameServers.Domain.Models;

namespace GameServers.Infrastructure.Parsers
{
    public interface IHtmlParser
    {
        Task<(string, System.Net.HttpStatusCode)> GetHtmlAsync(string url);
        List<GameServer> ParseServersList(string html);
        GameServer ParseServerIp(string html);
        List<Game> ParseGames(string html);
    }
}
