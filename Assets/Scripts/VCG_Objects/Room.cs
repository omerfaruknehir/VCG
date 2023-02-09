using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VCG_Objects
{
    public class Room
    {
        public string Name;
        public int Key;
        public int MaxPlayers;
        public Player[] Players;
        public bool IsPublic;
        public bool IsStarted;
        public List<Card> UnusedCards;


        private Thread roomLoop;

        private Thread lobbyLoop;
        private Thread gameLoop;

        public Room(int key, string name, int maxPlayers, bool isPublic)
        {
            Key = key;
            Name = name;
            MaxPlayers = maxPlayers;
            IsPublic = isPublic;
        }
    }
}
