using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;
using CardDeck.Extensions;
using CardDeck.Models;

namespace HeartsExample.Game.Player
{
    public class HumanPlayer : BasePlayer
    {
        public HumanPlayer(string name) : base(name)
        {
            IsHuman = true;
        }

        public override Card DetermineCardToPlay(Trick currentTrick, Card? startingCard = null)
        {
            Card card = null;
            bool validCard = false;
            while (!validCard)
            {
                Console.WriteLine($"Player {Name} what card do you want to play? ");
                string response = string.Empty;
                while (string.IsNullOrWhiteSpace(response))
                {
                    response = Console.ReadLine() ?? string.Empty;
                    switch (response)
                    {
                        //if 1 list all cards
                        case "1":
                        {
                            foreach (Card cards in PlayerCards)
                            {
                                Console.WriteLine(cards.ToString());
                            }

                            response = string.Empty;
                            break;
                        }
                        //if 2 list all valid
                        case "2":
                        {
                            foreach (Card cards in PlayerCards.Where(x => currentTrick.CardIsValidForTrick(x, PlayerCards, startingCard)).ToList())
                            {
                                Console.WriteLine(cards.ToString());
                            }

                            response = string.Empty;
                            break;
                        }
                        default:
                        {
                            if (response.Length == 2)
                            {
                                card = Card.ParsePlayerInput(response);

                                if (!PlayerCards.Any(x => x.CardSuit == card.CardSuit && x.CardFaceValue == card.CardFaceValue))
                                {
                                    Console.WriteLine("Player does not have card");
                                    response = string.Empty;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Couldn't read player input. ");
                                response = string.Empty;
                            }

                            break;
                        }
                    }
                }

                if (card != null && currentTrick.CardIsValidForTrick(card, PlayerCards, startingCard))
                {
                    validCard = true;
                }
            }

            if (card == null)
            {
                Console.WriteLine("Couldn't read player input. ");
            }

            return card;
        }

        public override List<Card> PassCards()
        {
            List<Card> cards = new List<Card>();

            //pick 3 cards to pass
            while (cards.Count < 3)
            {
                Console.WriteLine($"Player {Name} what card do you want to pass? ");
                string response = string.Empty;
                while (string.IsNullOrWhiteSpace(response))
                {
                    response = Console.ReadLine() ?? string.Empty;
                    switch (response)
                    {
                        //if 1 list all cards
                        case "1":
                        {
                            foreach (Card allCards in PlayerCards)
                            {
                                Console.WriteLine(allCards.ToString());
                            }

                            response = string.Empty;
                            break;
                        }
                        //if 2 list all selected cards
                        case "2":
                        {
                            foreach (Card selectedCards in cards)
                            {
                                Console.WriteLine(selectedCards.ToString());
                            }

                            response = string.Empty;
                            break;
                        }
                        default:
                        {
                            if (response.Length == 2)
                            {
                                Card card = Card.ParsePlayerInput(response);

                                if (PlayerCards.Any(x => x.CardSuit == card.CardSuit && x.CardFaceValue == card.CardFaceValue))
                                {
                                    RemoveCard(card);
                                    cards.Add(card);
                                    break;
                                }

                                response = string.Empty;
                                Console.WriteLine($"Player does not have card {card} ");
                            }
                            else
                            {
                                response = string.Empty;
                                Console.WriteLine("Couldn't read player input. ");
                            }

                            break;
                        }
                    }
                }
            }

            return cards;
        }
    }
}