using NUnit.Framework;

namespace HeartsExampleTest.Game
{
    public class HeartsExampleGameTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void CreateTestGame(int numPlayers)
        {
            try
            {
                HeartsExample.Game.Game game = new(numPlayers);
                Assert.That(game.Players.Count(x => x.IsHuman), Is.EqualTo(numPlayers), "Invalid Human player count. ");
            }
            catch (ArgumentException e)
            {
                Assert.That(numPlayers > 4, "Invalid parameter. ");
            }
        }
    }
}