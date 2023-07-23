using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;
using CardDeck.Extensions;

namespace CardDeck.Models
{
    /// <summary>
    /// Represents a playing card that has both a suit and a face value
    /// </summary>
    public class Card : IEquatable<Card>, IComparable<Card>, ICloneable
    {
        public Suit CardSuit { get; set; }

        public FaceValue CardFaceValue { get; set; }

        public static Card StartingTrickCard => new Card { CardSuit = Suit.Clubs, CardFaceValue = FaceValue.Two };

        public static Card QueenOfSpades => new Card { CardSuit = Suit.Spades, CardFaceValue = FaceValue.Queen };

        public override string ToString()
        {
            return $"{CardFaceValue} of {CardSuit}";
        }

        public bool Equals(Card? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CardSuit == other.CardSuit && CardFaceValue == other.CardFaceValue;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Card)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)CardSuit, (int)CardFaceValue);
        }

        public int CompareTo(Card? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            int cardSuitComparison = CardSuit.CompareTo(other.CardSuit);
            if (cardSuitComparison != 0)
            {
                return cardSuitComparison;
            }

            return CardFaceValue.CompareTo(other.CardFaceValue);
        }

        public static Card ParsePlayerInput(string response)
        {
            string face = response.Substring(0, 1).ToUpper();
            string suit = response.Substring(1, 1).ToUpper();

            return new Card()
            {
                CardFaceValue = face.GetValueFromDescription<FaceValue>(),
                CardSuit = suit.GetValueFromDescription<Suit>()
            };
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}