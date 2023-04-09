using HeartsExample.Game;

Console.WriteLine("How many players? ");
string numPlayers = Console.ReadLine() ?? "0";

if (!int.TryParse(numPlayers, out int players))
{
    throw new Exception("Invalid players");
}

if (players is < 0 or > 4)
{
    throw new Exception("Invalid players");
}

Game game = new Game(players);
game.StartNewGame();