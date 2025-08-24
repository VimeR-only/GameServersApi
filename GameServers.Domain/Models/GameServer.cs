namespace GameServers.Domain.Models
{
    public class GameServer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Map { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Country { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
    }
}
