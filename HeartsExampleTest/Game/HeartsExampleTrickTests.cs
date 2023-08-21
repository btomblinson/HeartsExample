using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Models;
using HeartsExample.Game;
using HeartsExample.Game.Enums;
using HeartsExample.Game.Player;
using HeartsExampleTest.Game.Helpers;
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

		[Test]
		public void TestDetermineTrickWinnerRandomHands()
		{
			//Arrange
			Random random = new Random();
			Trick trick;

			//run this test a ton
			for (int i = 0; i < 10000; i++)
			{
				//start a random trick
				trick = new Trick(random.Next(1, 14), random.Next(0, 1) == 1);

				//shuffle deck for more randomness
				Deck deck = new();
				Assert.That(deck.Cards, Has.Count.EqualTo(52), "Deck does not have 52 cards");
				deck.ShuffleDeck();

				//deal cards
				Players.ForEach(x => x.ResetCards());

				for (int j = 0; j <= deck.Cards.Count - 1; j++)
				{
					Players[j % 4].DealCard(deck.Cards[j]);
				}

				Card startingCard = new Card();

				//this runs through a 4 player trick using random cards and random game parameters for as many times as the
				//above loop is initialized, that should be a fairly large number(at least 10k) to simulate randomness,
				//the below comparison and assert must be absolutely fool proof
				//because we have to make sure the trick logic of validating a played card and the logic to determine a winner is correct 
				//100 percent of the time
				for (int playerCount = 1; playerCount <= 4; playerCount++)
				{
					BasePlayer player = Players[playerCount - 1];

					Card playerCard = player.GetRandomCard();

					if (playerCount == 1)
					{
						startingCard = playerCard;
					}

					bool expectedResult = TrickWinnerHelper.CardIsValidForTrick(trick, playerCard, player.PlayerCards, startingCard);
					bool actualResult = trick.CardIsValidForTrick(playerCard, player.PlayerCards, startingCard);

					if (actualResult == false)
					{
						Console.WriteLine("Test");
					}

					Assert.That(expectedResult, Is.EqualTo(actualResult), "Card is not valid for trick. ");

					trick.AddPlayerCardToTrick(player, playerCard);
				}

				Tuple<BasePlayer, Card> expectedTrickWinner = TrickWinnerHelper.DetermineTrickWinner(trick.CardsInTrick, startingCard);
				Tuple<BasePlayer, Card> actualTrickWinner = trick.DetermineTrickWinner(startingCard);

				Assert.That(expectedTrickWinner.Item1.Name, Is.EqualTo(actualTrickWinner.Item1.Name), "Expected player did not win trick. ");
			}
		}

		[Test]
		public void TestPlayFirstTrick()
		{
			//By now we've tested initialize and dealing hand so we can just focus on testing first trick 
			//Arrange
			HeartsExample.Game.Game game = new(0);
			game.StartNewGame();

			//Act
			Card startingCard = Card.StartingTrickCard;
			for (int playerCount = 1; playerCount <= 4; playerCount++)
			{
				BasePlayer player = game.Players.First(x => x.OrderInTrick == (TrickOrder)playerCount);

				Card playerCard = player.DetermineCardToPlay(game.CurrentHand.CurrentTrick, startingCard);

				Assert.That(game.CurrentHand.CurrentTrick.CardIsValidForTrick(playerCard, player.PlayerCards, startingCard), "Invalid card for trick!");

				if (playerCount == 1)
				{
					startingCard = playerCard;
				}

				player.PlayCard(playerCard);
				game.CurrentHand.CurrentTrick.AddPlayerCardToTrick(player, playerCard);
			}

			//Assert
			//must be 4 cards in trick
			Assert.That(game.CurrentHand.CurrentTrick.CardsInTrick, Has.Count.EqualTo(4), "Not enough cards played in trick! ");

			//every player must have 12 cards
			foreach (BasePlayer player in game.Players)
			{
				Assert.That(player.PlayerCards, Has.Count.EqualTo(12), "Player does not have 12 cards. ");
			}

			Tuple<BasePlayer, Card> expectedTrickWinner = TrickWinnerHelper.DetermineTrickWinner(game.CurrentHand.CurrentTrick.CardsInTrick, startingCard);
			Tuple<BasePlayer, Card> actualTrickWinner = game.CurrentHand.CurrentTrick.DetermineTrickWinner(startingCard);
			Assert.That(expectedTrickWinner.Item1.Name, Is.EqualTo(actualTrickWinner.Item1.Name), "Expected player did not win trick. ");
		}
	}
}