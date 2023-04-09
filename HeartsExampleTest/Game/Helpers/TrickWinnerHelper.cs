using CardDeck.Enums;
using CardDeck.Models;
using HeartsExample.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeartsExample.Game;

namespace HeartsExampleTest.Game.Helpers
{
    public static class TrickWinnerHelper
    {
        public static Tuple<BasePlayer, Card> DetermineTrickWinner(List<Tuple<BasePlayer, Card>> cardsInTrick, Card startingCard)
        {
            Tuple<BasePlayer, Card> trickWinner = null;

            //if the starting card is the only one of this suit, they were the winner
            if (cardsInTrick.Count(x => x.Item2.CardSuit.Equals(startingCard.CardSuit)) == 1)
            {
                trickWinner = cardsInTrick.First(x => x.Item2.CardSuit.Equals(startingCard.CardSuit));
            }
            //if all cards are same suit, find largest one, that player won the trick
            else if (cardsInTrick.Count(x => x.Item2.CardSuit.Equals(startingCard.CardSuit)) == 4)
            {
                trickWinner = cardsInTrick.OrderByDescending(x => x.Item2.CardFaceValue).First();
            }
            else
            {
                trickWinner = cardsInTrick.Where(x => x.Item2.CardSuit.Equals(startingCard.CardSuit)).OrderByDescending(x => x.Item2.CardFaceValue).First();
            }

            if (trickWinner == null)
            {
                throw new Exception("Unable to determine trick winner");
            }

            return trickWinner;
        }

        /// <summary>
        /// Helper method to determine if a card is valid to be played for a trick, as a test helper method that computes a tests expected result this
        /// method should be 100 percent fool proof implementation of hearts logic
        /// </summary>
        /// <param name="currentTrick"></param>
        /// <param name="cardsInTrick"></param>
        /// <param name="playerCard"></param>
        /// <param name="otherCardsForPlayer"></param>
        /// <param name="startingCard"></param>
        /// <returns></returns>
        public static bool CardIsValidForTrick(Trick currentTrick, List<Tuple<BasePlayer, Card>> cardsInTrick, Card playerCard, List<Card> otherCardsForPlayer, Card startingCard)
        {
            //if its first trick and no cards have been played, player must have played 2 of clubs
            if (currentTrick.TrickNumber == 1 && cardsInTrick.Count == 0)
            {
                return playerCard.Equals(Card.StartingTrickCard);
            }

            //check that we lead the trick off correctly
            if (cardsInTrick.Count == 0)
            {
                //if player starts trick with a heart, and hearts have not been broken, and player has other cards in hand not hearts that's invalid
                if (playerCard.CardSuit == Suit.Hearts && !currentTrick.HeartsBroken && otherCardsForPlayer.Count(x => x.CardSuit != Suit.Hearts) > 0)
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
    }
}