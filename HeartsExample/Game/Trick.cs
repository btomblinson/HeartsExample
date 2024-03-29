﻿using CardDeck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;
using HeartsExample.Game.Enums;
using HeartsExample.Game.Player;

namespace HeartsExample.Game
{
	public class Trick
	{
		public Game CurrentGame { get; private set; }

		public int TrickNumber { get; private set; }

		public bool HeartsBroken { get; private set; }

		public List<Tuple<BasePlayer, Card>> CardsInTrick { get; private set; }

		public Trick(Game game)
		{
			TrickNumber = 1;
			CurrentGame = game;
			CardsInTrick = new List<Tuple<BasePlayer, Card>>();
		}

		public Trick(int trickNumber, bool heartsBroken)
		{
			TrickNumber = trickNumber;
			HeartsBroken = heartsBroken;
			CardsInTrick = new List<Tuple<BasePlayer, Card>>();
		}

		public void AddPlayerCardToTrick(BasePlayer player, Card card)
		{
			CardsInTrick.Add(new Tuple<BasePlayer, Card>(player, card));
		}

		public void SetupNextTrick()
		{
			CardsInTrick.Clear();
			TrickNumber++;
		}

        public void SetupFreshTrick()
        {
            CardsInTrick.Clear();
            TrickNumber =1;
        }

        public void SortPlayerTrickOrder(BasePlayer startingPlayer)
		{
			int startingIndex = CurrentGame.Players.IndexOf(startingPlayer);

			//set starter
			CurrentGame.Players[startingIndex].OrderInTrick = TrickOrder.First;

			//if the starting index is 0, then this is easy
			if (startingIndex == 0)
			{
				CurrentGame.Players[1].OrderInTrick = TrickOrder.Second;
				CurrentGame.Players[2].OrderInTrick = TrickOrder.Third;
				CurrentGame.Players[3].OrderInTrick = TrickOrder.Last;
			}

			else
			{
				int playersAssigned = 1;
				int index = startingIndex;

				while (playersAssigned < 4)
				{
					index++;
					playersAssigned++;
					if (index == CurrentGame.Players.Count)
					{
						index = 0;
					}

					if (playersAssigned == 2)
					{
						CurrentGame.Players[index].OrderInTrick = TrickOrder.Second;
					}

					if (playersAssigned == 3)
					{
						CurrentGame.Players[index].OrderInTrick = TrickOrder.Third;
					}

					if (playersAssigned == 4)
					{
						CurrentGame.Players[index].OrderInTrick = TrickOrder.Last;
					}
				}
			}
		}

		public Tuple<BasePlayer, Card> DetermineTrickWinner(Card startingCard)
		{
			Tuple<BasePlayer, Card> trickWinner = null;

			if (!HeartsBroken && CardsInTrick.Count(x => x.Item2.CardSuit.Equals(Suit.Hearts)) > 0)
			{
				HeartsBroken = true;
			}

			//if the starting card is the only one of this suit, they were the winner
			if (CardsInTrick.Count(x => x.Item2.CardSuit.Equals(startingCard.CardSuit)) == 1)
			{
				trickWinner = CardsInTrick.First(x => x.Item2.CardSuit.Equals(startingCard.CardSuit));
			}
			//if all cards are same suit, find largest one, that player won the trick
			else if (CardsInTrick.Count(x => x.Item2.CardSuit.Equals(startingCard.CardSuit)) == 4)
			{
				trickWinner = CardsInTrick.OrderByDescending(x => x.Item2.CardFaceValue).First();
			}
			else
			{
				trickWinner = CardsInTrick.Where(x => x.Item2.CardSuit.Equals(startingCard.CardSuit)).OrderByDescending(x => x.Item2.CardFaceValue).First();
			}

			if (trickWinner == null)
			{
				throw new ArgumentException("Unable to determine trick winner");
			}

			Console.WriteLine($"Player {trickWinner.Item1.Name} won trick with {trickWinner.Item2}");

			return trickWinner;
		}

		/// <summary>
		/// This method checks if a card is valid to be played in the trick, this logic is actually easy but it requires the current state of the trick
		/// and the players hand.
		/// </summary>
		/// <param name="startingCard"></param>
		/// <param name="playerCard"></param>
		/// <param name="otherCardsForPlayer"></param>
		/// <returns></returns>
		public bool CardIsValidForTrick(Card playerCard, List<Card> otherCardsForPlayer, Card? startingCard = null)
		{
			//if its first trick and no cards have been played, player must have played 2 of clubs
			if (TrickNumber == 1 && CardsInTrick.Count == 0)
			{
				return playerCard.Equals(Card.StartingTrickCard);
			}

			//check that we lead the trick off correctly
			if (CardsInTrick.Count == 0)
			{
				//if player starts trick with a heart, and hearts have not been broken, and player has other cards in hand not hearts that's invalid
				if (playerCard.CardSuit == Suit.Hearts && !HeartsBroken && otherCardsForPlayer.Count(x => x.CardSuit != Suit.Hearts) > 0)
				{
					return false;
				}

				//else let player start with whatever they want
				return true;
			}

			//if there is a starting card player must play the same suit if they have it
			if (startingCard != null)
			{
				//if suit of played card is same as starting card its fine
				if (startingCard.CardSuit == playerCard.CardSuit)
				{
					return true;
				}

				//if suit of played card does not match starting card but player has cards of starting suit error
				if (startingCard.CardSuit != playerCard.CardSuit && otherCardsForPlayer.Count(x => x.CardSuit == startingCard.CardSuit) > 0)
				{
					return false;
				}
			}

			//else player can play what they want
			return true;
		}

		public void DetermineTrickPoints(BasePlayer winningPlayer)
		{
			if (CardsInTrick.Any(x => x.Item2.CardSuit.Equals(Suit.Hearts)))
			{
				CurrentGame.Players[CurrentGame.Players.IndexOf(winningPlayer)].TrickPoints += CardsInTrick.Count(x => x.Item2.CardSuit.Equals(Suit.Hearts));
			}

			if (CardsInTrick.Any(x => x.Item2.CardSuit.Equals(Card.QueenOfSpades.CardSuit) && x.Item2.CardFaceValue.Equals(Card.QueenOfSpades.CardFaceValue)))
			{
				CurrentGame.Players[CurrentGame.Players.IndexOf(winningPlayer)].TrickPoints += 13;
			}
		}

		public void ResetHeartsBroken()
		{
			HeartsBroken = false;
		}
	}
}