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
                string name = nameNode?.InnerText.Trim() ?? string.Empty;

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
                string map = mapNode?.InnerText.Trim() ?? string.Empty;

                var countryNode = node.SelectSingleNode(".//a[contains(@class,'serversList-itemCountry')]");
                string country = countryNode?.InnerText.Trim() ?? string.Empty;

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

        private string ParseServerName(HtmlDocument doc)
        {
            var nameNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-titleName')]");
            return nameNode?.InnerText.Trim() ?? string.Empty;
        }

        private string ParseServerIp(HtmlDocument doc)
        {
            var IpNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-addrText')]");
            string fullIp = IpNode?.InnerText.Trim() ?? string.Empty;

            return fullIp;
        }   

        private (string ip, string port) ParseIpAndPort(string fullIp)
        {
            var parts = fullIp.Split(':');
            if (parts.Length == 2)
            {
                return (parts[0], parts[1]);
            }
            return (string.Empty, string.Empty);
        }

        private int ParseCurrentPlayers(HtmlDocument doc)
        {
            var curNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-countCur')]");
            return Convert.ToInt32(curNode?.InnerText);
        }

        private int ParseMaxPlayers(HtmlDocument doc)
        {
            var maxNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class,'srvPage-countMax')]");
            return Convert.ToInt32(maxNode?.InnerText);
        }

        private string ParseMap(HtmlDocument doc)
        {
            var mapNode = doc.DocumentNode.SelectSingleNode("//a[contains(@class,'srvPage-mapLink')]");
            return mapNode?.InnerText?.Trim() ?? string.Empty;
        }

        private string ParseCountryName(HtmlDocument doc)
        {
            var countryNode = doc.DocumentNode.SelectSingleNode("//a[@class='srvPage-countryLink']");
            return countryNode.InnerText.Trim();
        }

        private string ParseCountryCode(HtmlDocument doc)
        {
            var countryNode = doc.DocumentNode.SelectSingleNode("//a[@class='srvPage-countryLink']");
            return countryNode.GetAttributeValue("href", "").Split(':').Last();
        }

        private (int, int) ParseRankAndTotalServers(HtmlDocument doc)
        {
            var rankNode = doc.DocumentNode.SelectSingleNode("//span[@class='srvPage-rankValue']/b");
            int rank = int.Parse(rankNode.InnerText.Trim());

            var divNode = doc.DocumentNode.SelectSingleNode("//div[@class='srvPage-rank']");
            int totalServers = int.Parse(divNode.InnerText.Trim().Split('/')[1].Trim());

            return (rank, totalServers);
        }

        public GameServer ParseServerIp(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            string name = ParseServerName(doc);

            string fullIp = ParseServerIp(doc);
            (string ip, string port) = ParseIpAndPort(fullIp);

            int curPlayers = ParseCurrentPlayers(doc);
            int maxPlayers = ParseMaxPlayers(doc);

            string map = ParseMap(doc);

            string countryName = ParseCountryName(doc);
            string code = ParseCountryCode(doc);

            (int rank, int totalServers) = ParseRankAndTotalServers(doc);

            return new GameServer
            {
                Name = name,
                Map = map,
                Ip = ip,
                Port = port,
                FullCountry = countryName,
                Country = code,
                CurrentPlayers = curPlayers,
                MaxPlayers = maxPlayers,
                RankByGame = rank,
                RankByServers = totalServers
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