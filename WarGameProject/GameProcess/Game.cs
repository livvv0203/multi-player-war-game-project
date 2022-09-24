using System;
using System.Collections.Generic;
using System.Linq;
using WarGameProject.DataModels;
using WarGameProject.RuntimeModels;

namespace WarGameProject.GameProcess
{
    /// <summary>
    /// Game Loop with combinations of Game Rounds
    /// </summary>
    public class Game
    {
        private readonly List<Player> players;

        // Each Game contains a list of Players
        public Game(List<Player> gamePlayers)
        {
            this.players = new List<Player>(gamePlayers);
        }

        private bool CheckGameWinner()
        {
            if (players.Count == 1) return true;
            // Any of player in Game wins all (52) cards
            return players.Any(s => s.TotalCards() >= Deck.TOTAL_CARD_IN_FULL_DECK);  
        }

        private bool inited = false; // flag

        /// <summary>
        /// To Initialize the Game Loop
        /// </summary>
        public void Initialize()
        {
            if (inited) return;
            inited = true;

            // Init Deck in Total number -> shuffle deck -> split deck
            Deck gameDeck = Deck.CreateFullDeck();
            gameDeck.ShuffleDeck();

            var decks = gameDeck.SplitDeck(players.Count);

            // Decks are split amont the players, hence cards are added to InHandDeck of each players
            for (var i = 0; i < players.Count; i++)
            { 
                players[i].InHandDeck.PushCardsIntoDeck(decks[i].Cards);
            }
        }

        public void MainLoop()
        {
            // Initialize Zone and GameRound 
            PlayerGameRoundZone roundWinnerZone = null;
            int gameRound = 0;

            // Keep Looping until we got the [WINNER]
            while (!CheckGameWinner())
            {
                Console.WriteLine($"\nNew Round Started: -----ROUND {gameRound}-----\n");

                var newRound = GameRound.CreateNewGameRound(gameRound, players);

                // Check game winner inside each of the [Game Round]
                while (newRound.CurrentRoundResult != GameRound.RoundResult.HasWinner)
                {

                    newRound.UpdateState(); 

                    Console.WriteLine($"\nRound {newRound.RoundIndex} Round State: {newRound.CurrentRoundResult}");

                }
                // Here, the round has filled with a winner, exit while loop
                roundWinnerZone = newRound.LatestWinningZone;

                // Collect Rewards/Cards
                newRound.RewardAllToPlayer(roundWinnerZone.Owner);

                Console.WriteLine($"Round WINNER [----- {roundWinnerZone.Owner.Info} -----]\n");

                players.RemoveAll(p =>
                {
                    if (p.IsPlayerOutOfCard())
                    {
                        Console.WriteLine($"{p.Info} is out of card and kicked out from the game.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(p.GenerateAllInfoWithCardsString(
                            (
                                p is HumanPlayer
                                    ? Config.SHOW_ALL_CARDS_AFTER_EACH_ROUND_FOR_HUMAN_PLAYER
                                    : Config.SHOW_ALL_CARDS_AFTER_EACH_ROUND_FOR_COM_PLAYER
                            )
                        ));
                    }
                    return false;
                });

                gameRound++;

                Console.WriteLine("\n**************************************************************************\n");
            }

            // Here, Game has finished with a winner, which is the roundWinner
            var winner = players.FirstOrDefault(s => s.TotalCards() >= Deck.TOTAL_CARD_IN_FULL_DECK);

            if (winner == null)
            {
                Console.WriteLine("Game finished without having a winner with full deck in hand, check logic");
            }
            else
            {
                Console.WriteLine($"Winner is {winner.Info}");
            }
        }

    }
}


















