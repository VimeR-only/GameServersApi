namespace GameServers.Domain.Models
{
    public class GameServer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Map { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string FullCountry { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int RankByGame { get; set; }
        public int RankByServers { get; set; }
    }
}
