using System;

namespace WarGameProject.DataModels
{
    /// <summary>
    /// Information about the player
    /// </summary>
    public class PlayerInfo
    {
        public string PlayerName { get; set; } = "Unknown Player";


        public static PlayerInfo Create(string playerName)
        {
            return new PlayerInfo()
            {
                PlayerName = playerName
            };
        }

        public override string ToString()
        {
            return PlayerName;
        }



    }
}

