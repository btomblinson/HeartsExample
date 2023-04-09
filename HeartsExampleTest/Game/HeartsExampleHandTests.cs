using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;
using CardDeck.Models;
using HeartsExample.Game;
using HeartsExample.Game.Enums;
using HeartsExample.Game.Player;
using HeartsExampleTest.Game.Helpers;
using NUnit.Framework;

namespace HeartsExampleTest.Game
{
    [TestFixture]
    public class HeartsExampleHandTests
    {
        [OneTimeSetUp]
        public void CreateAndDealCardsForTestStartingHand()
        {
            for (int numPlayers = 0; numPlayers < 5; numPlayers++)
            {
                HeartsExample.Game.Game game = new(numPlayers);

                try
                {
                    //do some initialize asserts for first hand
                    Assert.That(game.CurrentHand.HandCount, Is.EqualTo(1), "Hand count does not equal 1. ");
                    Assert.That(game.CurrentHand.FirstHand, Is.EqualTo(true), "First hand is not equal to true. ");
                    Assert.That(game.CurrentHand.CurrentTrick.TrickNumber, Is.EqualTo(1), "First trick number is not equal to 1. ");
                }
                catch (ArgumentException e)
                {
                    Assert.Fail("Invalid number of play. ");
                    return;
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                    return;
                }

                //hand has been initialized, now deal the hand
                game.CurrentHand.DealStartOfHand();

                //make sure each player has 13 cards
                foreach (BasePlayer player in game.Players)
                {
                    Assert.That(player.PlayerCards.Count == 13, "Player does not have 13 cards. ");
                }
            }
        }

        [Order(1)]
        [Test]
        public void TestDetermineStartingPlayerForFirstHandNoHumanPlayers()
        {
            HeartsExample.Game.Game game = new(0);
            game.CurrentHand.DealStartOfHand();

            //all assertions before this were ran in OneTimeSetUp so we're fine to not check

            //assert card passing method
            switch (game.CurrentHand.HandCount % 4)
            {
                //assert hand count method
                case 0:
                    Assert.That(game.CurrentHand.CardPassingMethod == PassingMethod.None, "Passing method should be none but is not. ");
                    break;
                case 1:
                    Assert.That(game.CurrentHand.CardPassingMethod == PassingMethod.Left, "Passing method should be left but is not. ");
                    break;
                case 2:
                    Assert.That(game.CurrentHand.CardPassingMethod == PassingMethod.Right, "Passing method should be right but is not. ");
                    break;
                case 3:
                    Assert.That(game.CurrentHand.CardPassingMethod == PassingMethod.Across, "Passing method should be across but is not. ");
                    break;
            }

            //handle passing cards, no human players so should be passed automatically
            game.CurrentHand.HandlePreHandCardPassing();

            //make sure each player has 13 cards
            foreach (BasePlayer player in game.Players)
            {
                Assert.That(player.PlayerCards.Count == 13, "Player does not have 13 cards. ");
            }

            //find expected player that has 2 of clubs
            BasePlayer expectedStartingPlayer = game.Players.First(x => x.PlayerHasStartingCardForTrick());

            BasePlayer actualStartingPlayer = game.CurrentHand.DetermineStartingPlayer();

            Assert.That(expectedStartingPlayer.Name.Equals(actualStartingPlayer.Name), "Expected player does not have the 2 of clubs. ");
        }

        [Order(2)]
        [Test]
        public void TestPlayerMustPlayerTwoOfClubsForFirstTrickForFirstHandNoHumanPlayers()
        {
            //By now we've tested initialize and dealing hand so we can just focus on testing first trick 
            //Arrange
            List<Tuple<BasePlayer, Card>> cardsInTrick = new List<Tuple<BasePlayer, Card>>();

            HeartsExample.Game.Game game = new(0);

            game.CurrentHand.DealStartOfHand();

            game.CurrentHand.HandlePreHandCardPassing();

            BasePlayer actualStartingPlayer = game.CurrentHand.DetermineStartingPlayer();

            //Act
            Card playerCard = actualStartingPlayer.DetermineCardToPlay(game.CurrentHand.CurrentTrick, cardsInTrick, Card.StartingTrickCard);

            //Assert
            Assert.That(playerCard.Equals(Card.StartingTrickCard), "First trick did not start with two of clubs. ");
        }

        [Order(3)]
        [Test]
        public void TestPlayersPlayFirstTrickForFirstHandNoHumanPlayers()
        {
            //By now we've tested initialize and dealing hand so we can just focus on testing first trick 
            //Arrange
            List<Tuple<BasePlayer, Card>> cardsInTrick = new List<Tuple<BasePlayer, Card>>();

            HeartsExample.Game.Game game = new(0);
            game.StartNewGame();

            //Act
            Card startingCard = Card.StartingTrickCard;
            for (int playerCount = 1; playerCount <= 4; playerCount++)
            {
                BasePlayer player = game.Players.First(x => x.OrderInTrick == (TrickOrder)playerCount);

                Card playerCard = player.DetermineCardToPlay(game.CurrentHand.CurrentTrick, cardsInTrick, startingCard);

                if (!game.CurrentHand.CurrentTrick.CardIsValidForTrick(cardsInTrick, playerCard, player.PlayerCards, startingCard))
                {
                    throw new Exception("Invalid card for trick! ");
                }

                if (playerCount == 1)
                {
                    startingCard = playerCard;
                }

                player.PlayCard(playerCard);
                cardsInTrick.Add(new Tuple<BasePlayer, Card>(player, playerCard));
            }

            //Assert
            //must be 4 cards in trick
            Assert.That(cardsInTrick.Count == 4, "Not enough cards played in trick! ");

            //every player must have 12 cards
            foreach (BasePlayer player in game.Players)
            {
                Assert.That(player.PlayerCards.Count == 12, "Player does not have 12 cards. ");
            }

            Tuple<BasePlayer, Card> expectedTrickWinner = TrickWinnerHelper.DetermineTrickWinner(cardsInTrick, startingCard);
            Tuple<BasePlayer, Card> actualTrickWinner = game.CurrentHand.CurrentTrick.DetermineTrickWinner(cardsInTrick, startingCard);
            Assert.That(expectedTrickWinner.Item1.Name.Equals(actualTrickWinner.Item1.Name), "Expected player did not win trick. ");
        }

        [Order(4)]
        [Test]
        public void TestPlayersEntireFirstHandNoHumanPlayers()
        {
            //By now we've tested initialize, dealing hand, and first trick, so we want to go through entire hand
            //Arrange
            List<Tuple<BasePlayer, Card>> cardsInTrick = new List<Tuple<BasePlayer, Card>>();
            HeartsExample.Game.Game game = new(0);
            game.StartNewGame();

            //Act
            Card? startingCard = Card.StartingTrickCard;
            while (game.CurrentHand.CurrentTrick.TrickNumber < 14)
            {
                for (int playerCount = 1; playerCount <= 4; playerCount++)
                {
                    BasePlayer player = (game.Players.Find(x => x.OrderInTrick == (TrickOrder)playerCount) ?? null) ?? throw new InvalidOperationException();

                    Card playerCard = player.DetermineCardToPlay(game.CurrentHand.CurrentTrick, cardsInTrick, startingCard);

                    if (!game.CurrentHand.CurrentTrick.CardIsValidForTrick(cardsInTrick, playerCard, player.PlayerCards, startingCard))
                    {
                        throw new Exception("Invalid card for trick! ");
                    }

                    if (playerCount == 1)
                    {
                        startingCard = playerCard;
                    }

                    player.PlayCard(playerCard);
                    cardsInTrick.Add(new Tuple<BasePlayer, Card>(player, playerCard));
                }

                //Assert
                //must be 4 cards in trick
                Assert.That(cardsInTrick.Count == 4, "Not enough cards played in trick! ");

                //every player must have 2 less cards than trick number
                foreach (BasePlayer player in game.Players)
                {
                    Assert.That(player.PlayerCards.Count == 14 - game.CurrentHand.CurrentTrick.TrickNumber - 1, $"Player does not have {game.CurrentHand.CurrentTrick.TrickNumber - 2} cards. ");
                }

                //determine who won trick and reset order
                Tuple<BasePlayer, Card> expectedTrickWinner = TrickWinnerHelper.DetermineTrickWinner(cardsInTrick, startingCard);
                Tuple<BasePlayer, Card> actualTrickWinner = game.CurrentHand.CurrentTrick.DetermineTrickWinner(cardsInTrick, startingCard);
                Assert.That(expectedTrickWinner.Item1.Name.Equals(actualTrickWinner.Item1.Name), "Expected player did not win trick. ");

                //TODO: Trick points
                game.CurrentHand.CurrentTrick.DetermineTrickPoints(actualTrickWinner.Item1, cardsInTrick);
                game.CurrentHand.CurrentTrick.SortPlayerTrickOrder(actualTrickWinner.Item1);
                cardsInTrick.Clear();
                startingCard = null;

                game.CurrentHand.CurrentTrick.TrickNumber++;
            }

            //hand asserts

            //make sure all cards were played
            Assert.That(game.Players.Count(x => x.PlayerCards.Count > 0) == 0, "Not all 52 cards were played in hand. ");

            //make sure trick points equal 26
            Assert.That(game.Players.Sum(x => x.TrickPoints) == 26, "Player trick points do not sum to 26. ");
        }

     
    }
}