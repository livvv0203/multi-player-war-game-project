using System;
using System.Collections.Generic;
using WarGameProject.RuntimeModels;

namespace WarGameProject.GameProcess
{
    public class GameRound
    {

        public enum RoundResult
        {
            Init,
            HasWinner,
            InWar,
        }

        private GameRound() { }

        public int RoundIndex { get; set; }

        // Stores current result of current game round
        public RoundResult CurrentRoundResult { get; private set; } = RoundResult.Init;

        // Playerszones contains player info and corresponded zone
        public Dictionary<Player, PlayerGameRoundZone> PlayerZones { get; } = new Dictionary<Player, PlayerGameRoundZone>();

        public PlayerGameRoundZone GetPlayerZone(Player player)
        {
            return PlayerZones.ContainsKey(player) ? PlayerZones[player] : null;
        }

        /// <summary>
        /// To Create a new Game round, each of the round contains players and their zones(on Table)
        /// </summary>
        /// <param name="gameRound"></param>
        /// <param name="allPlayers"></param>
        /// <returns></returns>
        public static GameRound CreateNewGameRound(int gameRound, List<Player> allPlayers)
        {
            var gameround = new GameRound();
            gameround.RoundIndex = gameRound;

            for (int i = 0; i < allPlayers.Count; i++)
            {
                gameround.PlayerZones.Add(allPlayers[i], PlayerGameRoundZone.Create(allPlayers[i]));
            }

            return gameround;
        }
        
        public void UpdateState()
        {
            switch (CurrentRoundResult)
            {
                case RoundResult.Init:
                    CurrentRoundResult = UpdateStateOnInit();
                    break;

                default:
                case RoundResult.HasWinner:
                    return;

                case RoundResult.InWar:
                    CurrentRoundResult = UpdateStateOnInWar();
                    break;
            }
        }

        // Gets the latest winning zone, if Round Result is hasWinner
        public PlayerGameRoundZone LatestWinningZone { get; private set; }

        private RoundResult UpdateStateOnInit()
        {
            // max is used to store the maximum number during comparison of iteration 
            int max = int.MinValue; // or -1 
            PlayerGameRoundZone maxZone = null;

            // <Play, PlayGameRoundZone>
            foreach (var zone in PlayerZones.Values)
            {
                // 1) Force all players to place a battle card
                zone.Owner.PlaceBattleCard(zone);

                // 2) Basically, Find Unique MAX among all play round zones, after comparison
                if (zone.BattleCard.Number > max)
                {
                    max = zone.BattleCard.Number;
                    maxZone = zone;
                }

                else if (zone.BattleCard.Number == max) {
                    // Max number is not Unique, reset pointers
                    max = int.MinValue; 
                    maxZone = null; 
                }
            }

            if (maxZone == null)
            {
                // No Winner -> Enter 'WAR' Mode
                return RoundResult.InWar;
            }
            else
            {
                // We have a unique max -> hence winner exist -> terminate the round
                LatestWinningZone = maxZone;
                return RoundResult.HasWinner;
            }
        }

        private RoundResult UpdateStateOnInWar()
        {
            // Triggers a init of NEW WAR
            int max = int.MinValue;
            PlayerGameRoundZone maxZone = null;

            foreach(var zone in PlayerZones.Values)
            {
                // 1) Force all Players to create a new War Deck
                zone.Owner.PlaceWarDeck(zone);

                // 2) Handle the case where player has no card to play
                int handlingCardNumber;
                if (zone.WarDecks.Count > 0)
                {
                    // Has a War Deck - Use one at the top of Stack
                    handlingCardNumber = zone.WarDecks.Peek().FlagCard.Number;
                }
                else
                {
                    // No War Deck - Use battle card instead
                    handlingCardNumber = zone.BattleCard.Number;
                }

                // 3) Compare the War Decks
                if (handlingCardNumber > max)
                {
                    max = handlingCardNumber;
                    maxZone = zone;
                }
                else if (handlingCardNumber == max)
                {
                    max = int.MinValue;
                    maxZone = null;
                } 
            }

            // 4) Similarly, return the Round Result
            if (maxZone == null)
            {
                return RoundResult.InWar;
            }
            else
            {
                LatestWinningZone = maxZone;
                return RoundResult.HasWinner;
            }
        }

        // Add all cards among all zones to the winner's discard deck in zone
        public void RewardAllToPlayer(Player winner)
        {
            foreach (var zone in PlayerZones.Values)
            {
                // Add all battle cards of all players(winner and losers)
                winner.AddToDiscardedDeck(zone.BattleCard);

                // Add all War decks of all players
                while (zone.WarDecks.Count > 0)
                {
                    winner.AddToDiscardedDeck(zone.WarDecks.Pop().GetAllCards());
                }
                zone.ClearZone();
            }
        }
    }
}







