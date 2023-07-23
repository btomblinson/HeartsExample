using CardDeck.Enums;
using CardDeck.Extensions;
using CardDeck.Models;
using HeartsExample.Game.Enums;
using System.Linq;

namespace HeartsExample.Game.Player
{
    public abstract class BasePlayer : ICloneable
    {
        #region Properties

        public bool IsHuman { get; set; }

        public string Name { get; set; }

        public TrickOrder OrderInTrick { get; set; }

        public int TrickPoints { get; set; }

        public int GamePoints { get; set; }

        public List<Card> PlayerCards;

        #endregion

        #region Constructors

        public BasePlayer(string name)
        {
            Name = name;
            PlayerCards = new List<Card>();
            GamePoints = 0;
        }

        #endregion

        #region Virtual Methods

        public virtual void ResetCards()
        {
            PlayerCards.Clear();
            TrickPoints = 0;
        }

        public virtual void DealCard(Card card)
        {
            PlayerCards.Add(card);
            PlayerCards.Sort();
        }

        public virtual void AddPassedCards(List<Card> cards)
        {
            PlayerCards.AddRange(cards);
            PlayerCards.Sort();
        }

        public Card GetRandomCard()
        {
            Random random = new Random();
            int index = random.Next(PlayerCards.Count);
            Card card = PlayerCards[index];
            PlayerCards.RemoveAt(index);
            return card;
        }

        public virtual void PlayCard(Card card)
        {
            if (!PlayerCards.Any(x => x.CardSuit == card.CardSuit && x.CardFaceValue == card.CardFaceValue))
            {
                throw new Exception("Player does not have card");
            }

            RemoveCard(card);

            Console.WriteLine($"Player {Name} played {card}");
        }

        public virtual void RemoveCard(Card card)
        {
            PlayerCards.RemoveAll(x => x.CardSuit == card.CardSuit && x.CardFaceValue == card.CardFaceValue);
        }

        public virtual bool PlayerHasStartingCardForTrick()
        {
            return PlayerCards.Count(x => x.CardFaceValue == Card.StartingTrickCard.CardFaceValue && x.CardSuit == Card.StartingTrickCard.CardSuit) > 0;
        }

        #endregion

        #region Abstract Methods

        public abstract Card DetermineCardToPlay(Trick currentTrick, List<Tuple<BasePlayer, Card>> cardsInTrick, Card? startingCard = null);

        public abstract List<Card> PassCards();

        #endregion

        #region Interface

        public object Clone()
        {
            BasePlayer clone = (BasePlayer)base.MemberwiseClone();
            clone.PlayerCards = PlayerCards.Select(x => (Card)x.Clone()).ToList();
            return clone;
        }

        #endregion
    }
}