using GameServers.Domain.Models;
using HtmlAgilityPack;
using System;

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
    }
}
