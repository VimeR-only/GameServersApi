using GameServers.Application.Interfaces;
using GameServers.Domain.Models;
using GameServers.Infrastructure.Parsers;
using System.Net;
// using System.Diagnostics;

namespace GameServers.Application.Services
{
    public class ServerService : IServerService
    {
        private readonly IHtmlParser _parser;

        public ServerService(IHtmlParser parser)
        {
            _parser = parser;
        }
        public async Task<List<GameServer>> GetServers(string? game, int page = 1)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }
            try
            {
                // var stopwatch = new Stopwatch();

                // stopwatch.Start();

                var (content, statusCode) = await _parser.GetHtmlAsync($"https://tsarvar.com/en/servers/{game}?page={page}");
                
                if (statusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"[ERROR] Failed to load page {page}: Status code {statusCode}");

                    return new List<GameServer>();
                }

                // stopwatch.Stop();

                // Console.WriteLine($"Request status: {statusCode}, Time: {stopwatch.Elapsed.TotalMilliseconds} ms");

                // var list = _parser.ParseServers(content);

                // Console.WriteLine($"[INFO] Page {list.Count} servers found.");
                return _parser.ParseServersList(content);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load page {page}: {ex.Message}");

                return new List<GameServer>();
            }
        }

        public async Task<GameServer> GetServerIpAsync(string game, string ip)
        {

            var (content, statusCode) = await _parser.GetHtmlAsync($"https://tsarvar.com/en/servers/{game}/{ip}");

            if (statusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"[ERROR] Failed to load server of {ip}: Status code {statusCode}");

                return new GameServer();
            }

            return _parser.ParseServerIp(content);
        }

        public async Task<List<GameServer>> GetAllServersAsync(string? game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var servers = new List<GameServer>();

            int currentPage = 1;
            int batchSize = 5;

            // var stopwatch = new Stopwatch();

            // stopwatch.Start();

            while (true)
            {
                var tasks = new List<Task<(string, System.Net.HttpStatusCode)>>();

                for (int i = 0; i < batchSize; i++)
                {
                    int pageNumber = currentPage + i;
                    tasks.Add(_parser.GetHtmlAsync(
                        $"https://tsarvar.com/en/servers/{game}?page={pageNumber}"
                    ));
                }

                var results = await Task.WhenAll(tasks);
                bool anyPageHasServers = false;

                foreach (var (html, statusCode) in results)
                {
                    var pageServers = _parser.ParseServersList(html);

                    if (pageServers.Count > 0)
                    {
                        servers.AddRange(pageServers);
                        anyPageHasServers = true;
                    }
                }

                if (!anyPageHasServers)
                    break;

                currentPage += batchSize;
            }

            // stopwatch.Stop();

            // Console.WriteLine($"[INFO] Total servers fetched: {servers.Count}");
            // Console.WriteLine($"[INFO] Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds");

            return servers;
        }
    }
}
