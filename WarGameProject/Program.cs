using System;
using System.Collections.Generic;
using WarGameProject.DataModels;
using WarGameProject.GameProcess;
using WarGameProject.RuntimeModels;

namespace WarGameProject // Note: actual namespace depends on the project name.
{

    public static class Config
    {
        public const int TOTAL_HUMAN_PLAYERS = 1;

        public const int TOTAL_COM_PLAYERS = 1;

        public const bool SHOW_ALL_CARDS_AFTER_EACH_ROUND_FOR_HUMAN_PLAYER = true;

        public const bool SHOW_ALL_CARDS_AFTER_EACH_ROUND_FOR_COM_PLAYER = true;

    }

    public class Program
    {
        private static List<PlayerInfo> CreateAllPlayerInfo(int creationCount, bool isHumanPlayer)
        {
            List<PlayerInfo> createdPlayerInfo = new List<PlayerInfo>(creationCount);

            for (int i = 0; i < creationCount; i++)
            {
                string playerName = null;

                if (isHumanPlayer)
                {
                    // Prompt Human Players to enter their names
                    do
                    {
                        Console.WriteLine("Welcome to Jieqing's War Game!");

                        Console.WriteLine($"Enter Player {i} Name: \n");
                        playerName = Console.ReadLine();

                        // Validate user input
                        if (!string.IsNullOrWhiteSpace(playerName))
                        {
                            break;
                        }

                        Console.Write("Player name cannot be empty, please enter again.");

                    } while (true);
                }

                else
                {
                    playerName = $"COMPUTER - {i}";
                }

                createdPlayerInfo.Add(PlayerInfo.Create(playerName));
            }

            return createdPlayerInfo;
        }


        private const ConsoleKey EXIT_KEY = ConsoleKey.E;

        static void Main(string[] args)
        {

            var humanPlayerInfo = CreateAllPlayerInfo(Config.TOTAL_HUMAN_PLAYERS, true);

            var computerPlayerInfo = CreateAllPlayerInfo(Config.TOTAL_COM_PLAYERS, false);

            Console.WriteLine($"\nReady to Play, press ENTER to start, or {EXIT_KEY} to exit.");

            while(Console.ReadKey().Key != EXIT_KEY)
            {
                // To store the list of in-game players 
                var playerList = new List<Player>();
                playerList.AddRange(humanPlayerInfo.ConvertAll(s => new HumanPlayer(s)));
                playerList.AddRange(computerPlayerInfo.ConvertAll(s => new ComputerPlayer(s)));

                // Now prompt to ensure we have all the Players ready
                Console.WriteLine("\n-----------GAME BEGIN ---------\n");
                Console.WriteLine("Players Lined UP as: \n");

                for (var i = 0; i < playerList.Count; i++) { 
                    Console.WriteLine($"\nPlayer[{i}]: {playerList[i].Info.PlayerName} \n");
                }

                var game = new Game(playerList);
                game.Initialize();
                game.MainLoop();

                Console.Write($"\nGame Finished, press enter {EXIT_KEY} to exit, any other key to play again >O< ");
            }

            // EXIT GAME

            Console.WriteLine("\nThank you for playing, Bye.");


            
        }
    }
}