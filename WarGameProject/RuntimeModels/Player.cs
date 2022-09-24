using System;
using System.Collections.Generic;
using System.Text;
using WarCardGame.RuntimeModels;
using WarGameProject.DataModels;
using WarGameProject.GameProcess;

namespace WarGameProject.RuntimeModels
{
    /// <summary>
    /// Represents a War Game Player
    /// </summary>
    public abstract class Player
    {
        public PlayerInfo Info { get; set; } = null;

        protected Player(PlayerInfo info)
        {
            Info = info;
            InHandDeck = Deck.CreateEmptyDeck();
            InHandDeck.Owner = this;
            DiscardedDeck = Deck.CreateEmptyDeck();
            DiscardedDeck.Owner = this;
        }

        public Deck InHandDeck { get; }

        public Deck DiscardedDeck { get; }

        public int TotalCards() { return InHandDeck.CardsCount() + DiscardedDeck.CardsCount(); }

        public bool IsPlayerOutOfCard()
        {
            return InHandDeck.IsEmpty() && DiscardedDeck.IsEmpty();
        }

        private void TakeAllCardsFromDiscardedDeck()
        {
            DiscardedDeck.ShuffleDeck();
            InHandDeck.PushCardsIntoDeck(DiscardedDeck.Cards);
            DiscardedDeck.ClearAll();
        }

        public virtual void PlaceBattleCard(PlayerGameRoundZone zone)
        {

            if (InHandDeck.IsEmpty())
            {
                TakeAllCardsFromDiscardedDeck();
                Console.WriteLine($"{this.Info} Collected the Discarded Deck.");
            }

            var card = InHandDeck.RemoveFirstHeadCard();
            
            zone.BattleCard = card;
            
            Console.WriteLine($"{this.Info} Places a [Battle Card] ---> [{card}]");
        }

        private const int WAR_DECK_FACING_DOWN_CARDS_REQUIREMENT = 3; // 3 Cards Face-Down
        private const int WAR_DECK_FULL_CARDS_COUNT = WAR_DECK_FACING_DOWN_CARDS_REQUIREMENT + 1; // 4

        public virtual void PlaceWarDeck(PlayerGameRoundZone zone)
        {
            
            if (IsPlayerOutOfCard())
            {
                return;
            }

            // Here, we have at least 1 cards in one of the deck
            if (InHandDeck.CardsCount() < WAR_DECK_FULL_CARDS_COUNT)
            {
                // We don't have cards in hand any more
                TakeAllCardsFromDiscardedDeck();
            }

            // Once reached here, at least 1 card in hand deck

            var newWarDeck = WarDeck.Create(
                player: this,
                flagCard: InHandDeck.RemoveFirstHeadCard(),
                faceDownCards: InHandDeck.RemoveHeadCards(
                    Math.Min(InHandDeck.CardsCount(), WAR_DECK_FACING_DOWN_CARDS_REQUIREMENT)
                    ));

            zone.WarDecks.Push(newWarDeck);

            Console.WriteLine($"{this.Info} Played War Deck: {newWarDeck}");

        }

        public void AddToDiscardedDeck(Card card)
        {
            this.DiscardedDeck.PushCardIntoDeck(card);
        }

        public void AddToDiscardedDeck(List<Card> cards)
        {
            this.DiscardedDeck.PushCardsIntoDeck(cards);
        }

        public string GenerateAllInfoWithCardsString(bool includeListOfCard)
        {

            StringBuilder sb = new StringBuilder($"{this.Info} has {this.InHandDeck.CardsCount()} cards in hand " +
                                                 $"and {this.DiscardedDeck.CardsCount()} in discarded deck");

            if (includeListOfCard)
            {
                sb.AppendLine()
                    .Append($"In Hand Deck ---> [Count={InHandDeck.CardsCount()}]: ")
                    .AppendLine(InHandDeck.ToString());
                sb
                    .Append($"Discarded Deck ---> [Count={DiscardedDeck.CardsCount()}]: ")
                    .AppendLine(DiscardedDeck.ToString());
            }

            return sb.ToString();
        }

    }

}

