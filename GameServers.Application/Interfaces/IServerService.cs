using GameServers.Domain.Models;

namespace GameServers.Application.Interfaces
{
    public interface IServerService
    {
        Task<List<GameServer>> GetServers(int page = 1);
        Task<List<GameServer>> GetAllServersAsync();
    }
}
