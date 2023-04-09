using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;

namespace CardDeck.Models
{
    /// <summary>
    /// A deck is a list of 52 cards 
    /// </summary>
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>(52);

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (FaceValue value in Enum.GetValues(typeof(FaceValue)))
                {
                    Cards.Add(new Card()
                    {
                        CardSuit = suit,
                        CardFaceValue = value
                    });
                }
            }
        }

        public void ShuffleDeck()
        {
            Random random = new Random();
            int n = Cards.Count;

            while (n > 1)
            {
                n--;
                int randomValue = random.Next(n + 1);
                (Cards[randomValue], Cards[n]) = (Cards[n], Cards[randomValue]);
            }
        }
    }
}