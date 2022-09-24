using System;
using WarCardGame.RuntimeModels;
using WarGameProject.DataModels;
using WarGameProject.GameProcess;
using WarGameProject.RuntimeModels;

namespace WarGameTests
{
    [TestClass]
    public class WarDeckTest
    {
        private Player player;
        private PlayerInfo playerInfo;
        private PlayerGameRoundZone zone;

        [ClassInitialize]
        public void TestFixtureSetup()
        {
            playerInfo =  PlayerInfo.Create("Test Player");
        }
        [TestInitialize]
        public void Setup()
        {
            // Runs before each test.
            player =  new ComputerPlayer(playerInfo);

            player.InHandDeck.PushCardsIntoDeck(
                Deck.CreateFullDeck().ShuffleDeck().Cards
                ) ;
            zone = new PlayerGameRoundZone();
        }


        [TestMethod]
        [DataRow(2, 3)]
        [DataRow(3, 1)]
        public void WarDeckCreation(int flagCard, int facingDownCardCount)
        {
            var f = player.InHandDeck.Cards[0];
            List<Card> facingDownCards = player.InHandDeck.Cards.GetRange(1, 3);

            player.PlaceWarDeck(zone);
            var wd = zone.WarDecks.Pop();

            Assert.AreEqual(f, wd.FlagCard);

        }
    }
}

