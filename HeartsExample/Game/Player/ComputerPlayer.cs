using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;
using CardDeck.Models;

namespace HeartsExample.Game.Player
{
    public class ComputerPlayer : BasePlayer
    {
        public ComputerPlayer(string name) : base(name)
        {
            IsHuman = false;
        }

        public override Card DetermineCardToPlay(Trick currentTrick, Card? startingCard = null)
        {
            //if its first trick has to be a club and not hearts or queen of spades
            if (currentTrick.TrickNumber == 1)
            {
                if (currentTrick.CardsInTrick.Count == 0)
                {
                    //we have to have the 2 of clubs
                    return PlayerCards.First(x => x.Equals(Card.StartingTrickCard));
                }

                if (PlayerCards.Any(x => x.CardSuit == startingCard?.CardSuit))
                {
                    return PlayerCards.Where(x => x.CardSuit == Suit.Clubs).OrderByDescending(y => y.CardFaceValue).First();
                }

                return PlayerCards.Where(x => x.CardSuit != Suit.Hearts && x.CardSuit != Card.QueenOfSpades.CardSuit && x.CardFaceValue != Card.QueenOfSpades.CardFaceValue).OrderByDescending(y => y.CardFaceValue).First();
            }

            //this if for leading off a trick after the first one
            if (startingCard == null)
            {
                //if hearts have been broken, just lead with lowest card regardless of suit
                if (currentTrick.HeartsBroken)
                {
                    return PlayerCards.OrderBy(y => y.CardFaceValue).First();
                }

                //if hearts has not been broken but the player doesn't have any non hearts to start with, then play lowest heart
                if (PlayerCards.Count(x => x.CardSuit != Suit.Hearts) == 0)
                {
                    return PlayerCards.Where(x => x.CardSuit == Suit.Hearts).OrderBy(y => y.CardFaceValue).First();
                }

                //else play your lowest card that isn't a heart
                return PlayerCards.Where(x => x.CardSuit != Suit.Hearts).OrderBy(y => y.CardFaceValue).First();
            }

            //this is if the player is not leading the trick, this is where it gets complicated

            //if the player does not have any cards of the starting card suit and has the queen of spades, play it
            if (PlayerCards.All(x => x.CardSuit != startingCard.CardSuit) && PlayerCards.Any(x => x.CardSuit == Card.QueenOfSpades.CardSuit && x.CardFaceValue == Card.QueenOfSpades.CardFaceValue))
            {
                return PlayerCards.First(x => x.CardSuit == Card.QueenOfSpades.CardSuit && x.CardFaceValue == Card.QueenOfSpades.CardFaceValue);
            }

            //if the player does not have any cards of the starting card suit and they have hearts, play them
            if (PlayerCards.All(x => x.CardSuit != startingCard.CardSuit) && PlayerCards.Any(x => x is { CardSuit: Suit.Hearts }))
            {
                return PlayerCards.Where(x => x.CardSuit == Suit.Hearts).OrderByDescending(y => y.CardFaceValue).First();
            }

            //check if the player has cards of the starting suit
            if (PlayerCards.Any(x => x.CardSuit == startingCard.CardSuit))
            {
                //check if there are hearts or queen of spades played
                if (currentTrick.CardsInTrick.Any(x => x.Item2.CardSuit.Equals(Suit.Hearts)) || currentTrick.CardsInTrick.Any(x => x.Item2.CardSuit.Equals(Card.QueenOfSpades.CardSuit) && x.Item2.CardFaceValue.Equals(Card.QueenOfSpades.CardFaceValue)))
                {
                    //if the player has a card that does not beat what is out there play it
                    if (PlayerCards.Any(x => x.CardSuit == startingCard.CardSuit && x.CardFaceValue < startingCard.CardFaceValue))
                    {
                        return PlayerCards.Where(x => x.CardSuit == startingCard.CardSuit && x.CardFaceValue < startingCard.CardFaceValue).OrderByDescending(y => y.CardFaceValue).First();
                    }

                    //the player must play a higher card than what is out there

                    //check and see if they are the last player to play, if they are just play their highest card

                    if (currentTrick.CardsInTrick.Count() == 3)
                    {
                        return PlayerCards.Where(x => x.CardSuit == startingCard.CardSuit).OrderByDescending(y => y.CardFaceValue).First();
                    }

                    //if they are not, play the lower card
                    return PlayerCards.Where(x => x.CardSuit == startingCard.CardSuit).OrderBy(y => y.CardFaceValue).First();
                }

                //the player has cards of the same suit, but there are no hearts or queen of spades currently played, so just play highest card of 
                //the starting suit making sure you don't throw the queen on yourself unless you have to
                Card cardToPlay = PlayerCards.Where(x => x.CardSuit == startingCard.CardSuit).OrderByDescending(y => y.CardFaceValue).First();
                if (cardToPlay.CardSuit == Card.QueenOfSpades.CardSuit && cardToPlay.CardFaceValue == Card.QueenOfSpades.CardFaceValue)
                {
                    //if the queen is the only spade we have to play it
                    if (PlayerCards.Count(x => x.CardSuit == startingCard.CardSuit) == 1)
                    {
                        return cardToPlay;
                    }

                    //if not play our next highest spade
                    return PlayerCards.Where(x => x.CardSuit == startingCard.CardSuit).OrderByDescending(y => y.CardFaceValue).Skip(1).First();
                }
                else
                {
                    return cardToPlay;
                }
            }

            //the player does not have any cards of the starting suit, nor do they have the queen of spades or any hearts, just play the largest card
            return PlayerCards.OrderByDescending(y => y.CardFaceValue).First();
        }

        public override List<Card> PassCards()
        {
            List<Card> cards = new List<Card>();

            //determine worst 3 cards to pass

            while (cards.Count < 3)
            {
                Card card;
                //if you have starting card pass it
                if (PlayerCards.Any(x => x.Equals(Card.StartingTrickCard)))
                {
                    RemoveCard(Card.StartingTrickCard);
                    cards.Add(Card.StartingTrickCard);
                    continue;
                }

                //if you have queen of spades pass it
                if (PlayerCards.Any(x => x.Equals(Card.QueenOfSpades)))
                {
                    RemoveCard(Card.QueenOfSpades);
                    cards.Add(Card.QueenOfSpades);
                    continue;
                }

                //if you have Ace of spades pass it
                if (PlayerCards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Ace }) == 1)
                {
                    card = new Card()
                    {
                        CardSuit = Suit.Spades,
                        CardFaceValue = FaceValue.Ace
                    };
                    RemoveCard(card);
                    cards.Add(card);
                    continue;
                }

                //if you have King of spades pass it
                if (PlayerCards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.King }) == 1)
                {
                    card = new Card()
                    {
                        CardSuit = Suit.Spades,
                        CardFaceValue = FaceValue.King
                    };
                    RemoveCard(card);
                    cards.Add(card);
                    continue;
                }

                //else just add highest card left
                card = PlayerCards.OrderByDescending(y => y.CardFaceValue).First();
                RemoveCard(card);
                cards.Add(card);
            }

            return cards;
        }
    }
}