using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Models;
using HeartsExample.Game;
using HeartsExample.Game.Player;
using NUnit.Framework;

namespace HeartsExampleTest.Game
{
    public class HeartsExampleTrickTests
    {
        List<BasePlayer> Players = new List<BasePlayer>();

        [OneTimeSetUp]
        public void CreatePlayers()
        {
            Players.Add(new ComputerPlayer($"Test{1}"));
            Players.Add(new ComputerPlayer($"Test{2}"));
            Players.Add(new ComputerPlayer($"Test{3}"));
            Players.Add(new ComputerPlayer($"Test{4}"));
        }

        //basic tests
        [TestCase(1, false, "2C", "3C", "4C", "5C", "2C", "Test4")]
        [TestCase(1, false, "KC", "2C", "AC", "QC", "2C", "Test3")]
        [TestCase(1, false, "1C", "AC", "9C", "2C", "2C", "Test2")]
        [TestCase(1, false, "AC", "KC", "QC", "2C", "2C", "Test1")]

        //regular tests
        [TestCase(6, false, "AS", "KS", "4S", "AH", "AS", "Test1")]
        public void TestDetermineTrickWinner(int trickNumber, bool heartsBroken, string player1Card, string player2Card, string player3Card, string player4Card, string startingCard, string winningPlayerName)
        {
            //Arrange
            List<Tuple<BasePlayer, Card>> cardsInTrick = new List<Tuple<BasePlayer, Card>>();
            Trick trick = new Trick(trickNumber, heartsBroken);

            Card startingCardObject = Card.ParsePlayerInput(startingCard);

            cardsInTrick.Add(new Tuple<BasePlayer, Card>(Players[0], Card.ParsePlayerInput(player1Card)));
            cardsInTrick.Add(new Tuple<BasePlayer, Card>(Players[1], Card.ParsePlayerInput(player2Card)));
            cardsInTrick.Add(new Tuple<BasePlayer, Card>(Players[2], Card.ParsePlayerInput(player3Card)));
            cardsInTrick.Add(new Tuple<BasePlayer, Card>(Players[3], Card.ParsePlayerInput(player4Card)));

            //Act 
            Tuple<BasePlayer, Card> computedResult = trick.DetermineTrickWinner(cardsInTrick, startingCardObject);
            BasePlayer actualResult = Players.First(x => x.Name.Equals(winningPlayerName));

            //Assert
            Assert.That(computedResult.Item1.Name.Equals(actualResult.Name), "Wrong player won! ");
        }
    }
}