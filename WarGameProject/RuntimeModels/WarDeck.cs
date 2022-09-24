using System;
using System.Collections.Generic;
using WarGameProject.DataModels;
using WarGameProject.RuntimeModels;

namespace WarCardGame.RuntimeModels
{
    /// <summary>
    ///     The war deck is a flag card that is used to compare to another war deck, and the deck
    ///     contains the 3 cards facing down.
    /// </summary>
    public class WarDeck : Deck, IComparable<WarDeck>
    {

        public static WarDeck Create(Player player, Card flagCard, List<Card> faceDownCards)
        {
            return new WarDeck()
            {
                FlagCard = flagCard,
                Cards = faceDownCards,
                Owner = player,
            };
        }


        public Card FlagCard { get; set; }

        public void ReplaceWarDeck(Card flagCard, List<Card> cards)
        {
            FlagCard = flagCard;
            this.Cards.Clear();
            this.Cards.AddRange(cards);
        }

        public List<Card> GetAllCards()
        {
            return new List<Card>(this.Cards)
            {
                FlagCard
            };
        }

        public override void ClearAll()
        {
            base.ClearAll();
            FlagCard = null;
        }


        #region Comparison

        public int CompareTo(WarDeck other)
        {
            return FlagCard.CompareTo(other.FlagCard);
        }
        public static bool operator <(WarDeck left, WarDeck right)
        {
            return left.FlagCard < right.FlagCard;
        }

        public static bool operator >(WarDeck left, WarDeck right)
        {
            return left.FlagCard > right.FlagCard;
        }

        public static bool operator <=(WarDeck left, WarDeck right)
        {
            return left.FlagCard <= right.FlagCard;
        }

        public static bool operator >=(WarDeck left, WarDeck right)
        {
            return left.FlagCard >= right.FlagCard;
        }

        #endregion


        public override string ToString()
        {
            return $"Flag Card: {FlagCard} with Facing Down Cards: {base.ToString()}";
        }
    }
}
