# Game Servers Api

**Game Servers Api** is an ASP.NET Core Web API that provides information about game servers and games from [tsarvar.com](https://tsarvar.com). The API parses the website's HTML pages and returns structured data about servers and games via HTTP requests.


This is a personal project I created for fun and as part of my portfolio.  

- It's still a work in progress — roughly 20% complete.  
- You are free to use it for learning, experimenting, or any personal purposes.  
- Not all features are fully implemented yet, so some endpoints or functionality may not work as expected.

Note: This README does not cover every aspect of the project.

---

## Project Structure

- **Controllers** — API controllers that handle HTTP requests:
  - `ServersController` — fetches the list of servers for a game, all servers, or detailed info about a specific server.

- **Services** — services containing business logic:
  - `ServerService` — retrieves server and game data through the parser.

- **Parsers** — HTML parser:
  - `HtmlParser` implements `IHtmlParser` and extracts server and game data from HTML pages.

- **Models** — data models:
  - `GameServer` — represents a game server (name, IP, port, map, country, number of players).
  - `Game` — represents a game (name, ID).

---

## Features

The API allows you to:

- Get a **list of all games** available on tsarvar.com.
- Get a **list of servers** for a specific game (paginated or all servers at once).
- Get **detailed information about a server** by IP.
