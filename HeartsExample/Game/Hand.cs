using CardDeck.Models;
using HeartsExample.Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Extensions;
using HeartsExample.Game.Player;

namespace HeartsExample.Game
{
	public class Hand
	{
		public Game CurrentGame { get; private set; }

		public Trick CurrentTrick { get; private set; }

		public int HandCount { get; private set; }

		public bool FirstHand { get; private set; }

		public PassingMethod CardPassingMethod => (HandCount % 4).ToString().GetValueFromDescription<PassingMethod>();

		/// <summary>
		/// Initialize a new hearts hand, after initializing this class, the below 3 methods must be called in order to fully setup a hand
		/// </summary><br/>
		/// <br/>
		/// <see cref="DealStartOfHand"/><br/>
		/// <see cref="HandlePreHandCardPassing"/><br/>
		/// <see cref="DetermineStartingPlayer"/><br/>
		/// <exception cref="ArgumentException">Throws exception if <see cref="numPlayers"/> is greater than 4</exception>
		public Hand(Game game)
		{
			CurrentGame = game;
			FirstHand = true;
			HandCount = 1;
			CurrentTrick = new Trick(game);
		}

		public void StartNewHand()
		{
			HandCount++;
			CurrentTrick.SetupNextTrick();

			DealStartOfHand();
			HandlePreHandCardPassing();
			DetermineStartingPlayer();
		}

		public void ManageHand()
		{
			while (!CurrentGame.Players.Any(x => x.GamePoints >= 50))
			{
				if (FirstHand)
				{
					FirstHand = false;
				}
				else
				{
					StartNewHand();
				}

				Card startingCard = Card.StartingTrickCard;
				while (CurrentTrick.TrickNumber < 14)
				{
					for (int playerCount = 1; playerCount <= 4; playerCount++)
					{
						BasePlayer player = (CurrentGame.Players.Find(x => x.OrderInTrick == (TrickOrder)playerCount) ?? null) ?? throw new InvalidOperationException();

						Card playerCard = player.DetermineCardToPlay(CurrentTrick, startingCard);

						if (!CurrentTrick.CardIsValidForTrick(playerCard, player.PlayerCards, startingCard))
						{
							throw new Exception("Invalid card for trick! ");
						}

						if (playerCount == 1)
						{
							startingCard = playerCard;
						}

						player.PlayCard(playerCard);
						CurrentTrick.AddPlayerCardToTrick(player, playerCard);
					}

					//do some checks
					if (CurrentTrick.CardsInTrick.Count != 4)
					{
						throw new Exception("Not enough cards played in trick! ");
					}

					//determine who won trick and reset order
					Tuple<BasePlayer, Card> trickWinner = CurrentTrick.DetermineTrickWinner(startingCard);
					CurrentTrick.DetermineTrickPoints(trickWinner.Item1);
					CurrentTrick.SortPlayerTrickOrder(trickWinner.Item1);

					CurrentTrick.SetupNextTrick();
				}

				//print game scoreboard
				Console.WriteLine();
				Console.WriteLine("Current Trick Score");
				CurrentGame.Players.ForEach(x =>
				{
					Console.WriteLine($"{x.Name}: {x.TrickPoints}");
				});

				Console.WriteLine();

				ApplyTrickPointsToGamePoints();

				//reset hearts broken
				CurrentTrick.ResetHeartsBroken();

				//print game scoreboard
				Console.WriteLine();
				Console.WriteLine("Current Game Score");
				CurrentGame.Players.ForEach(x =>
				{
					Console.WriteLine($"{x.Name}: {x.GamePoints}");
				});

				Console.WriteLine();
			}
		}

		#region ApplyTrickPointsToGamePoints

		public void ApplyTrickPointsToGamePoints()
		{
			//hand has ended, apply player trick points to game points
			//first check for a moonshot
			if (CurrentGame.Players.Any(x => x.TrickPoints == 26))
			{
				CurrentGame.Players.Where(x => x.TrickPoints == 0).ToList().ForEach(y =>
				{
					y.GamePoints += 26;
				});
			}
			else
			{
				CurrentGame.Players.ForEach(x =>
				{
					x.GamePoints += x.TrickPoints;
				});
			}
		}

		#endregion

		#region DealStartOfHand

		/// <summary>
		/// Shuffle the deck and deal random cards to each player
		/// </summary>
		public void DealStartOfHand()
		{
			Deck deck = new();
			deck.ShuffleDeck();

			CurrentGame.Players.ForEach(x => x.ResetCards());

			for (int i = 0; i <= deck.Cards.Count - 1; i++)
			{
				CurrentGame.Players[i % 4].DealCard(deck.Cards[i]);
			}
		}

		#endregion

		#region DetermineStartingPlayer

		public BasePlayer DetermineStartingPlayer()
		{
			//find the player with the 2 of clubs, they start the Trick
			BasePlayer? startingPlayer = CurrentGame.Players.Find(x => x.PlayerHasStartingCardForTrick()) ?? null;

			if (startingPlayer == null)
			{
				throw new Exception("No player has 2 of clubs");
			}

			Console.WriteLine($"Player {startingPlayer.Name} has 2 of clubs and is starting the hand");

			CurrentTrick.SortPlayerTrickOrder(startingPlayer);

			return startingPlayer;
		}

		#endregion

		#region HandlePreHandCardPassing

		public void HandlePreHandCardPassing()
		{
			//handle card passing based on hand number
			if (CardPassingMethod.Equals(PassingMethod.Left) || CardPassingMethod.Equals(PassingMethod.Right) || CardPassingMethod.Equals(PassingMethod.Across))
			{
				//first determine cards being passed
				List<List<Card>> passedCards = new List<List<Card>>();
				for (int i = 0; i < 4; i++)
				{
					passedCards.Add(CurrentGame.Players[i].PassCards());
				}

				//now pass
				//if passing to left, just add 1 
				if (CardPassingMethod.Equals(PassingMethod.Left))
				{
					for (int i = 0; i < 4; i++)
					{
						int newIndex = i + 1;
						if (newIndex == 4)
						{
							newIndex = 0;
						}

						CurrentGame.Players[newIndex].AddPassedCards(passedCards[i]);
					}
				}
				//if passing to right, just subtract 1 
				else if (CardPassingMethod == PassingMethod.Right)
				{
					for (int i = 3; i >= 0; i--)
					{
						int newIndex = i - 1;
						if (newIndex == -1)
						{
							newIndex = 3;
						}

						CurrentGame.Players[newIndex].AddPassedCards(passedCards[i]);
					}
				}
				//if passing across, just add 2
				else if (CardPassingMethod == PassingMethod.Across)
				{
					for (int i = 0; i < 4; i++)
					{
						int newIndex = i + 2;
						if (newIndex >= 4)
						{
							newIndex -= 4;
						}

						CurrentGame.Players[newIndex].AddPassedCards(passedCards[i]);
					}
				}
			}

			if (CurrentGame.Players.Count(x => x.PlayerCards.Count != 13) > 0)
			{
				throw new Exception("Player cards corrupted. ");
			}
		}

		#endregion
	}
}