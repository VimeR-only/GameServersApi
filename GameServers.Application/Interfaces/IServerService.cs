using GameServers.Domain.Models;

namespace GameServers.Application.Interfaces
{
    public interface IServerService
    {
        Task<List<GameServer>> GetServersAsync(string? game, int page = 1);
        Task<GameServer> GetServerIpAsync(string game, string ip);
        Task<List<GameServer>> GetAllServersAsync(string? game);
    }
}
