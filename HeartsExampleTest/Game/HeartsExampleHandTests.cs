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
using Newtonsoft.Json.Linq;
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
					Assert.That(player.PlayerCards, Has.Count.EqualTo(13), "Player does not have 13 cards. ");
				}
			}
		}

		[Test]
		[Order(1)]
		public void TestHandlePreHandCardPassingLeft()
		{
			HeartsExample.Game.Game game = new(0);

			//since its first hand should be left passing
			game.CurrentHand.DealStartOfHand();

			Assert.That(game.CurrentHand.CardPassingMethod, Is.EqualTo(PassingMethod.Left), "Passing method not left.");

			//first determine cards being passed
			List<List<Card>> passedCards = game.Players.Select(player => (BasePlayer)player.Clone()).Select(temp => temp.PassCards()).ToList();

			//make sure each player passed 3 cards
			foreach (List<Card> cards in passedCards)
			{
				Assert.That(cards, Has.Count.EqualTo(3), "Player did not pass 3 cards");
			}

			//handle passing cards, no human players so should be passed automatically
			game.CurrentHand.HandlePreHandCardPassing();

			for (int i = 0; i < 4; i++)
			{
				int newIndex = i + 1;
				if (newIndex == 4)
				{
					newIndex = 0;
				}

				Assert.That(passedCards[i].All(x => game.Players[newIndex].PlayerCards.Any(y => y.Equals(x))), "Card passing was unsuccessful.");
			}
		}

		[Order(2)]
		[Test]
		public void TestDetermineStartingPlayer()
		{
			HeartsExample.Game.Game game = new(0);
			game.CurrentHand.DealStartOfHand();

			//all assertions before this were ran in OneTimeSetUp so we're fine to not check

			//assert card passing method
			switch (game.CurrentHand.HandCount % 4)
			{
				//assert hand count method
				case 0:
					Assert.That(game.CurrentHand.CardPassingMethod, Is.EqualTo(PassingMethod.None), "Passing method should be none but is not. ");
					break;
				case 1:
					Assert.That(game.CurrentHand.CardPassingMethod, Is.EqualTo(PassingMethod.Left), "Passing method should be left but is not. ");
					break;
				case 2:
					Assert.That(game.CurrentHand.CardPassingMethod, Is.EqualTo(PassingMethod.Right), "Passing method should be right but is not. ");
					break;
				case 3:
					Assert.That(game.CurrentHand.CardPassingMethod, Is.EqualTo(PassingMethod.Across), "Passing method should be across but is not. ");
					break;
			}

			//handle passing cards, no human players so should be passed automatically
			game.CurrentHand.HandlePreHandCardPassing();

			//make sure each player has 13 cards
			foreach (BasePlayer player in game.Players)
			{
				Assert.That(player.PlayerCards, Has.Count.EqualTo(13), "Player does not have 13 cards. ");
			}

			//find expected player that has 2 of clubs
			BasePlayer expectedStartingPlayer = game.Players.First(x => x.PlayerHasStartingCardForTrick());

			BasePlayer actualStartingPlayer = game.CurrentHand.DetermineStartingPlayer();

			Assert.That(expectedStartingPlayer.Name, Is.EqualTo(actualStartingPlayer.Name), "Expected player does not have the 2 of clubs. ");
		}

		[Order(3)]
		[Test]
		public void TestMustPlayTwoOfClubsForFirstTrick()
		{
			//By now we've tested initialize and dealing hand so we can just focus on testing first trick 
			//Arrange
			HeartsExample.Game.Game game = new(0);

			game.CurrentHand.DealStartOfHand();

			game.CurrentHand.HandlePreHandCardPassing();

			BasePlayer actualStartingPlayer = game.CurrentHand.DetermineStartingPlayer();

			//Act
			Card playerCard = actualStartingPlayer.DetermineCardToPlay(game.CurrentHand.CurrentTrick, Card.StartingTrickCard);

			//Assert
			Assert.That(playerCard, Is.EqualTo(Card.StartingTrickCard), "First trick did not start with two of clubs. ");
		}

		[Order(4)]
		[Test]
		public void TestRandomFirstHand()
		{
			//By now we've tested initialize, dealing hand, and first trick, so we want to go through entire hand
			//Arrange
			HeartsExample.Game.Game game = new(0);
			game.StartNewGame();

			//Act
			Card? startingCard = Card.StartingTrickCard;
			while (game.CurrentHand.CurrentTrick.TrickNumber < 14)
			{
				for (int playerCount = 1; playerCount <= 4; playerCount++)
				{
					BasePlayer player = (game.Players.Find(x => x.OrderInTrick == (TrickOrder)playerCount) ?? null) ?? throw new InvalidOperationException();

					Card playerCard = player.DetermineCardToPlay(game.CurrentHand.CurrentTrick, startingCard);

					if (!game.CurrentHand.CurrentTrick.CardIsValidForTrick(playerCard, player.PlayerCards, startingCard))
					{
						throw new Exception("Invalid card for trick! ");
					}

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

				//every player must have 2 less cards than trick number
				foreach (BasePlayer player in game.Players)
				{
					Assert.That(player.PlayerCards, Has.Count.EqualTo(14 - game.CurrentHand.CurrentTrick.TrickNumber - 1), $"Player does not have {game.CurrentHand.CurrentTrick.TrickNumber - 2} cards. ");
				}

				//determine who won trick and reset order
				Tuple<BasePlayer, Card> expectedTrickWinner = TrickWinnerHelper.DetermineTrickWinner(game.CurrentHand.CurrentTrick.CardsInTrick, startingCard);
				Tuple<BasePlayer, Card> actualTrickWinner = game.CurrentHand.CurrentTrick.DetermineTrickWinner(startingCard);
				Assert.That(expectedTrickWinner.Item1.Name, Is.EqualTo(actualTrickWinner.Item1.Name), "Expected player did not win trick.");

				game.CurrentHand.CurrentTrick.DetermineTrickPoints(actualTrickWinner.Item1);

				Assert.That(expectedTrickWinner.Item1.TrickPoints, Is.EqualTo(actualTrickWinner.Item1.TrickPoints), "Trick points are not correct.");

				game.CurrentHand.CurrentTrick.SortPlayerTrickOrder(actualTrickWinner.Item1);
				game.CurrentHand.CurrentTrick.SetupNextTrick();
			}

			//make sure all cards were played
			Assert.That(game.Players.Count(x => x.PlayerCards.Count > 0), Is.EqualTo(0), "Not all 52 cards were played in hand. ");

			//make sure trick points equal 26
			Assert.That(game.Players.Sum(x => x.TrickPoints), Is.EqualTo(26), "Player trick points do not sum to 26. ");

			//hand asserts
			game.CurrentHand.ApplyTrickPointsToGamePoints();

			//make sure game points equal 26
			Assert.That(game.Players.Sum(x => x.GamePoints), Is.EqualTo(26), "Player game points do not sum to 26. ");
		}

		[Order(5)]
		[Test]
		public void TestMoonshotHandNoPassing()
		{
			//Arrange
			HeartsExample.Game.Game game = new(0);

			Deck deck = new();
			deck.ShuffleDeck();
			game.Players.ForEach(x => x.ResetCards());

			List<Card> cards = new List<Card>();

			//we are taking some liberties here, to make sure we have a player moonshot the first player we are going to deal all clubs except the 2,
			//so they will have 3-A of clubs and A of hearts to make sure they get all the points
			foreach (FaceValue value in Enum.GetValues(typeof(FaceValue)))
			{
				if (value != FaceValue.Two)
				{
					cards.Add(new Card()
					{
						CardSuit = Suit.Clubs,
						CardFaceValue = value
					});
				}
			}

			cards.Add(new Card()
			{
				CardSuit = Suit.Hearts,
				CardFaceValue = FaceValue.Ace
			});

			Assert.That(cards, Has.Count.EqualTo(13), "Not enough cards dealt to moonshot player.");

			game.Players[0].AddPassedCards(cards);

			deck.Cards.RemoveAll(x => cards.Any(y => y.Equals(x)));

			for (int i = 0; i <= deck.Cards.Count - 1; i++)
			{
				game.Players[(i % 3) + 1].DealCard(deck.Cards[i]);
			}

			game.CurrentHand.DetermineStartingPlayer();

			//Act
			Card? startingCard = Card.StartingTrickCard;
			while (game.CurrentHand.CurrentTrick.TrickNumber < 14)
			{
				for (int playerCount = 1; playerCount <= 4; playerCount++)
				{
					BasePlayer player = (game.Players.Find(x => x.OrderInTrick == (TrickOrder)playerCount) ?? null) ?? throw new InvalidOperationException();

					Card playerCard = player.DetermineCardToPlay(game.CurrentHand.CurrentTrick, startingCard);

					if (!game.CurrentHand.CurrentTrick.CardIsValidForTrick(playerCard, player.PlayerCards, startingCard))
					{
						throw new Exception("Invalid card for trick! ");
					}

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

				//every player must have 2 less cards than trick number
				foreach (BasePlayer player in game.Players)
				{
					Assert.That(player.PlayerCards, Has.Count.EqualTo(14 - game.CurrentHand.CurrentTrick.TrickNumber - 1), $"Player does not have {game.CurrentHand.CurrentTrick.TrickNumber - 2} cards. ");
				}

				//determine who won trick and reset order
				Tuple<BasePlayer, Card> expectedTrickWinner = TrickWinnerHelper.DetermineTrickWinner(game.CurrentHand.CurrentTrick.CardsInTrick, startingCard);
				Tuple<BasePlayer, Card> actualTrickWinner = game.CurrentHand.CurrentTrick.DetermineTrickWinner(startingCard);
				Assert.That(expectedTrickWinner.Item1.Name, Is.EqualTo(actualTrickWinner.Item1.Name), "Expected player did not win trick.");

				game.CurrentHand.CurrentTrick.DetermineTrickPoints(actualTrickWinner.Item1);

				Assert.That(expectedTrickWinner.Item1.TrickPoints, Is.EqualTo(actualTrickWinner.Item1.TrickPoints), "Trick points are not correct.");

				game.CurrentHand.CurrentTrick.SortPlayerTrickOrder(actualTrickWinner.Item1);
				game.CurrentHand.CurrentTrick.SetupNextTrick();
			}

			//make sure all cards were played
			Assert.That(game.Players.Count(x => x.PlayerCards.Count > 0), Is.EqualTo(0), "Not all 52 cards were played in hand. ");

			//make sure trick points equal 26
			Assert.That(game.Players.Sum(x => x.TrickPoints), Is.EqualTo(26), "Player trick points do not sum to 26.");

			//make sure 1 player has the 26 points
			Assert.That(game.Players.Count(x => x.TrickPoints == 26), Is.EqualTo(1), "Player 1 did not shoot the moon.");

			game.CurrentHand.ApplyTrickPointsToGamePoints();

			//make sure 3 players have 26 
			Assert.That(game.Players.Count(x => x.GamePoints == 26), Is.EqualTo(3), "Other players do not have 26 points");

			//make sure 1 player has 0 
			Assert.That(game.Players[0].GamePoints, Is.EqualTo(0), "Player 1 does not have 0 game points.");
		}
	}
}