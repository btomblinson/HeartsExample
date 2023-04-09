using CardDeck.Enums;
using CardDeck.Models;
using NUnit.Framework;

namespace CardDeckTest
{
    public class DeckTests
    {
        [Test]
        public void TestDeckCreation()
        {
            Deck deck = new();

            Assert.That(deck.Cards.Count, Is.EqualTo(52), "Deck does not have 52 cards");

            deck.ShuffleDeck();

            //check all clubs
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Two }), Is.EqualTo(1), $"{FaceValue.Two} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Three }), Is.EqualTo(1), $"{FaceValue.Three} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Four }), Is.EqualTo(1), $"{FaceValue.Four} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Five }), Is.EqualTo(1), $"{FaceValue.Five} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Six }), Is.EqualTo(1), $"{FaceValue.Six} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Seven }), Is.EqualTo(1), $"{FaceValue.Seven} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Eight }), Is.EqualTo(1), $"{FaceValue.Eight} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Nine }), Is.EqualTo(1), $"{FaceValue.Nine} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Ten }), Is.EqualTo(1), $"{FaceValue.Ten} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Jack }), Is.EqualTo(1), $"{FaceValue.Jack} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Queen }), Is.EqualTo(1), $"{FaceValue.Queen} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.King }), Is.EqualTo(1), $"{FaceValue.King} of {Suit.Clubs} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Clubs, CardFaceValue: FaceValue.Ace }), Is.EqualTo(1), $"{FaceValue.Ace} of {Suit.Clubs} is missing");

            //check all spades
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Two }), Is.EqualTo(1), $"{FaceValue.Two} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Three }), Is.EqualTo(1), $"{FaceValue.Three} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Four }), Is.EqualTo(1), $"{FaceValue.Four} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Five }), Is.EqualTo(1), $"{FaceValue.Five} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Six }), Is.EqualTo(1), $"{FaceValue.Six} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Seven }), Is.EqualTo(1), $"{FaceValue.Seven} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Eight }), Is.EqualTo(1), $"{FaceValue.Eight} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Nine }), Is.EqualTo(1), $"{FaceValue.Nine} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Ten }), Is.EqualTo(1), $"{FaceValue.Ten} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Jack }), Is.EqualTo(1), $"{FaceValue.Jack} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Queen }), Is.EqualTo(1), $"{FaceValue.Queen} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.King }), Is.EqualTo(1), $"{FaceValue.King} of {Suit.Spades} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Spades, CardFaceValue: FaceValue.Ace }), Is.EqualTo(1), $"{FaceValue.Ace} of {Suit.Spades} is missing");

            //check all diamonds
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Two }), Is.EqualTo(1), $"{FaceValue.Two} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Three }), Is.EqualTo(1), $"{FaceValue.Three} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Four }), Is.EqualTo(1), $"{FaceValue.Four} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Five }), Is.EqualTo(1), $"{FaceValue.Five} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Six }), Is.EqualTo(1), $"{FaceValue.Six} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Seven }), Is.EqualTo(1), $"{FaceValue.Seven} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Eight }), Is.EqualTo(1), $"{FaceValue.Eight} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Nine }), Is.EqualTo(1), $"{FaceValue.Nine} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Ten }), Is.EqualTo(1), $"{FaceValue.Ten} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Jack }), Is.EqualTo(1), $"{FaceValue.Jack} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Queen }), Is.EqualTo(1), $"{FaceValue.Queen} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.King }), Is.EqualTo(1), $"{FaceValue.King} of {Suit.Diamonds} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Diamonds, CardFaceValue: FaceValue.Ace }), Is.EqualTo(1), $"{FaceValue.Ace} of {Suit.Diamonds} is missing");

            //check all hearts
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Two }), Is.EqualTo(1), $"{FaceValue.Two} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Three }), Is.EqualTo(1), $"{FaceValue.Three} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Four }), Is.EqualTo(1), $"{FaceValue.Four} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Five }), Is.EqualTo(1), $"{FaceValue.Five} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Six }), Is.EqualTo(1), $"{FaceValue.Six} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Seven }), Is.EqualTo(1), $"{FaceValue.Seven} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Eight }), Is.EqualTo(1), $"{FaceValue.Eight} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Nine }), Is.EqualTo(1), $"{FaceValue.Nine} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Ten }), Is.EqualTo(1), $"{FaceValue.Ten} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Jack }), Is.EqualTo(1), $"{FaceValue.Jack} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Queen }), Is.EqualTo(1), $"{FaceValue.Queen} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.King }), Is.EqualTo(1), $"{FaceValue.King} of {Suit.Hearts} is missing");
            Assert.That(deck.Cards.Count(x => x is { CardSuit: Suit.Hearts, CardFaceValue: FaceValue.Ace }), Is.EqualTo(1), $"{FaceValue.Ace} of {Suit.Hearts} is missing");
        }
    }
}