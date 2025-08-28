using GameServers.Domain.Models;
using HtmlAgilityPack;

namespace GameServers.Infrastructure.Parsers
{
    public class HtmlParser : IHtmlParser
    {
        private readonly HttpClient _httpClient;

        public HtmlParser(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(string, System.Net.HttpStatusCode)> GetHtmlAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return (content, response.StatusCode);
        }

        public List<GameServer> ParseServersList(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var servers = new List<GameServer>();
            var nodes = doc.DocumentNode.SelectNodes("//li[contains(@class, 'serversList-item')]");

            if (nodes == null) return servers;

            foreach (var node in nodes)
            {
                var nameNode = node.SelectSingleNode(".//span[contains(@class,'serversList-itemName')]/a");
                string name = nameNode?.InnerText.Trim();

                var ipNode = node.SelectSingleNode(".//span[contains(@class,'serversList-itemAddressTblCText')]");
                string ip = string.Empty;
                string port = string.Empty;

                if (ipNode != null)
                {
                    string[] parts = ipNode.InnerText.Split(':');
                    if (parts.Length == 2)
                    {
                        ip = parts[0].Trim();
                        port = parts[1].Trim();
                    }
                }

                var currentPlayersNode = node.SelectSingleNode(".//span[contains(@class,'serversList-itemPlayersCur')]");
                var maxPlayersNode = node.SelectSingleNode(".//span[contains(@class,'serversList-itemPlayersMax')]");

                int curentPlayers = Convert.ToInt32(currentPlayersNode?.InnerText);
                int maxPlayers = Convert.ToInt32(maxPlayersNode?.InnerText);

                var mapNode = node.SelectSingleNode(".//a[contains(@class,'serversList-itemMap')]");
                string map = mapNode?.InnerText.Trim();

                var countryNode = node.SelectSingleNode(".//a[contains(@class,'serversList-itemCountry')]");
                string country = countryNode?.InnerText.Trim();

                if (nameNode != null && ipNode != null)
                {
                    servers.Add(new GameServer
                    {
                        Name = name,
                        Ip = ip,
                        Port = port,
                        Map = map,
                        CurrentPlayers = curentPlayers,
                        MaxPlayers = maxPlayers,
                        Country = country,
                    });
                }
            }

            return servers;
        }

        public GameServer ParseServerIp(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var details = new GameServer();

            var nameNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-titleName')]");
            string name = nameNode?.InnerText.Trim();

            var IpNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-addrText')]");
            string fullIp = IpNode?.InnerText.Trim();

            string ip = fullIp.Split(":")[0];
            string port = fullIp.Split(":")[1];

            var curNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-countCur')]");
            var maxNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-countMax')]");

            int curPlayers = Convert.ToInt32(curNode?.InnerText);
            int maxPlayers = Convert.ToInt32(maxNode?.InnerText);

            var mapNode = doc.DocumentNode.SelectSingleNode("//a[contains(@class,'srvPage-mapLink')]");
            string map = mapNode?.InnerText.Trim();

            Console.WriteLine(map + curPlayers + maxPlayers);

            return new GameServer { 
                Name = name,
                Map = map,
                Ip = ip,
                Port = port,
                CurrentPlayers = curPlayers,
                MaxPlayers = maxPlayers,
            };
        }

        public List<Game> ParseGames(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var games = new List<Game>();
            var nodes = doc.DocumentNode.SelectNodes("//li[contains(@class, 'gamesList-item')]");

            if (nodes == null) return games;

            foreach (var node in nodes)
            {
                var linkNode = node.SelectSingleNode(".//a[contains(@class,'gamesList-itemImage')]");
                if (linkNode == null) continue;

                string title = linkNode.GetAttributeValue("title", "");
                string href = linkNode.GetAttributeValue("href", "").Split("/")[3];

                games.Add(new Game
                {
                    Name = title,
                    Id = href
                });
            }

            return games;
        }
    }
}