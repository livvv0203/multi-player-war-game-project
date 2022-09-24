using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarGameProject.DataModels;

namespace WarGameProject.RuntimeModels
{
    /// <summary>
    /// A DECK contains a list of Cards
    /// </summary>
    public class Deck
    {
        public const int TOTAL_CARD_IN_FULL_DECK = (Card.MAX_CARD - Card.MIN_CARD + 1) * Card.TOTAL_SUITS; // 52

        // Owner of the DECK
        public Player Owner { get; set; }

        // List of Cards in DECK
        public List<Card> Cards { get; set; }

        // Constructor
        public Deck()
        {
            Cards = new List<Card>();
        }

        public static Deck CreateEmptyDeck() { return new Deck(); }

        /// <summary>
        /// Creates with given card, that contains a deep clone of every cards in the list
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public static Deck CreateWithGivenCards(List<Card> cardList)
        {
            var newDeck = new Deck();
            newDeck.Cards.AddRange(cardList.Select(s => s.DeepCopy()));
            return newDeck;
        }

        /// <summary>
        /// Create a FULL DECK of Cards containing TOTAL_CARD_IN_DECK numbers of cards 
        /// </summary>
        /// <returns></returns>
        public static Deck CreateFullDeck()
        {
            var newDeck = new Deck();
            // (2 -> 14) * 4 = 13 * 4 = 52 Cards
            for (int i = Card.MIN_CARD; i <= Card.MAX_CARD; i++)
            {
                for (int j = 0; j < Card.TOTAL_SUITS; j++)
                {
                    newDeck.Cards.Add(Card.Create(i));
                }
            }
            return newDeck;
        }

        /// <summary>
        /// Shuffle all cards in the deck with 'Fisher-Yates shuffle'
        /// </summary>
        /// <returns>A Deck after shuffling</returns>
        public Deck ShuffleDeck()
        {
            var rand = new Random();

            // Fisher-Yates Shuffle
            for (var i = Cards.Count - 1; i >= 0; i--)
            {
                var j = rand.Next(0, i);

                // SWAP i and j in Cards
                var temp = Cards[i];
                Cards[i] = Cards[j];
                Cards[j] = temp;
            }

            return this; // Deck
        }

        public Deck DeepCopy()
        {
            var copy = new Deck();
            for (var i = 0; i < this.Cards.Count; i++)
            {
                copy.Cards.Add(this.Cards[i].DeepCopy());
            }
            return copy;
        }

        public bool IsEmpty()
        {
            return CardsCount() == 0;
        }

        public int CardsCount() { return this.Cards.Count(); }

        /// <summary>
        /// Split a Deck in to "splitInto" decks, as "evenly" as possible
        /// </summary>
        /// <param name="splitInto"></param>
        /// <returns></returns>
        public List<Deck> SplitDeck(int splitInto)
        {
            if (splitInto > CardsCount())
            {
                splitInto = CardsCount();
            }

            // Treated as split into 1, return self's deep copy, same deck with Total Card number
            if (splitInto <= 1) return new List<Deck>() { DeepCopy() };

            var returnDeckList = new List<Deck>(splitInto);

            var perSplitCount = CardsCount() / splitInto;

            // We can only split divisible values of TOTAL_CARD_IN_FULL_DECK(52? 52 * 2?)
            // otherwise the last deck will have less count than others.
            if (CardsCount() % splitInto != 0)
            {
                perSplitCount++; 
            }

            for (int i = 0; i < splitInto - 1; i++)
            {
                var handlingDeck = new Deck();
                returnDeckList.Add(handlingDeck);

                for (int j = 0; j < perSplitCount; j++)
                {
                    handlingDeck.Cards.Add(this.Cards[perSplitCount * i + j]);
                }
            }

            // Now, we have the rest of the deck, causing incomplete set if splitinto is not a divisible number
            var lastDeck = new Deck();
            returnDeckList.Add(lastDeck); // Push the rest into the deck
            for (int j = perSplitCount * (splitInto - 1); j < CardsCount(); j++)
            {
                lastDeck.Cards.Add(this.Cards[j]);
            }

            return returnDeckList;

        }

        public Deck CombineIntoDeck(Deck other)
        {
            Cards.AddRange(other.Cards);
            return this;
        }

        public Deck CombineIntoDeck(List<Deck> others)
        {
            for (int i = 0; i < others.Count; i++)
            {
                CombineIntoDeck(others[i]);
            }
            return this;
        }

        // Pushes a card into deck
        public Deck PushCardIntoDeck(Card card)
        {
            Cards.Add(card);
            return this;
        }

        // Push a list of cards into deck
        public Deck PushCardsIntoDeck(List<Card> cards)
        {
            Cards.AddRange(cards);
            return this;
        }

        public virtual void ClearAll()
        {
            this.Cards.Clear();
        }

        // Removes the first head card
        public Card RemoveFirstHeadCard()
        {
            if (IsEmpty()) return null;

            return RemoveHeadCards(1)[0];
        }

        public List<Card> RemoveHeadCards(int count)
        {
            if (count > CardsCount()) count = CardsCount();
            if (count <= 0) return new List<Card>();

            List<Card> removedCards = Cards.GetRange(0, count);
            Cards.RemoveRange(0, count);
            return removedCards;
        }

        public override string ToString()
        {
            StringBuilder toStringBuilder = new StringBuilder();
            toStringBuilder.AppendJoin(",", this.Cards);
            return toStringBuilder.ToString();
        }

    }
}








