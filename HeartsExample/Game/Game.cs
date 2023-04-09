using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardDeck.Enums;
using CardDeck.Models;
using HeartsExample.Game.Enums;
using HeartsExample.Game.Player;

namespace HeartsExample.Game
{
    public class Game
    {
        /// <summary>
        /// List of players
        /// </summary>
        public List<BasePlayer> Players { get; private set; }

        public Hand CurrentHand { get; private set; }

        public Game(int numPlayers)
        {
            Players = new List<BasePlayer>();
            if (numPlayers > 4)
            {
                throw new ArgumentException("Only handles 4 players. ");
            }

            for (int i = 1; i <= 4; i++)
            {
                if (i <= numPlayers)
                {
                    Players.Add(new HumanPlayer($"Test{i}"));
                }
                else
                {
                    Players.Add(new ComputerPlayer($"Test{i}"));
                }
            }

            CurrentHand = new Hand(this);
        }

        public void StartNewGame()
        {
            CurrentHand.DealStartOfHand();
            CurrentHand.HandlePreHandCardPassing();
            CurrentHand.DetermineStartingPlayer();
        }
    }
}