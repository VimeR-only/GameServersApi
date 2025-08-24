using GameServers.Domain.Models;

namespace GameServers.Application.Interfaces
{
    public interface IServerService
    {
        Task<List<GameServer>> GetServers(string? game, int page = 1);
        Task<List<GameServer>> GetAllServersAsync(string? game);
    }
}
