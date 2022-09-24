using System;
using System.Collections.Generic;
using WarCardGame.RuntimeModels;
using WarGameProject.DataModels;
using WarGameProject.RuntimeModels;

namespace WarGameProject.GameProcess
{
    /// <summary>
    /// Represents Gameround Zone among each of the players 
    /// </summary>
    public class PlayerGameRoundZone
    {
        public static PlayerGameRoundZone Create(Player player)
        {
            return new PlayerGameRoundZone()
            {
                Owner = player,
            };
        }

        public Player Owner { get; set; }

        public Card BattleCard { get; set; }

        public Stack<WarDeck> WarDecks = new Stack<WarDeck>();

        public void ClearZone()
        {
            BattleCard = null;
            WarDecks.Clear();
        }
    }
}

