using GameServers.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServers.Application.Interfaces
{
    public interface IGameService
    {
        Task<List<Game>> GetGames();


    }
}
