﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GameData;
using System.Timers;

namespace Week21112016
{
    
    public class GameHub : Hub
    {
 //       This is called in monogame cliant game1
        //list of players
        //static variables keep variables seperate in more than 1 hub is active
        static List<PlayerData> Players = new List<PlayerData>();
        static Random r = new Random();
        static List<CollectableData> Collectables = new List<CollectableData>()
        {
            new CollectableData(0,
            new Position { X= r.Next(100,1800), Y = r.Next(100,1800) }
            ,10),
            new CollectableData(1,
            new Position { X= r.Next(100,1800), Y = r.Next(100,1800) }
            ,20),
            new CollectableData(2,
            new Position { X= r.Next(100,1800), Y = r.Next(100,1800) }
            ,30),

        };

        public static int WorldX = 2000;
        public static int WorldY = 2000;
        static bool PLAYING = false;
        public static Timer _startTime;

        public void Hello()
        {
            Clients.All.hello();
        }

        public void join()
        {
            Clients.Caller.joined(WorldX,WorldY);
        }


        public PlayerData JoinPlayer(Position pos)
        {
            //New Patter
            //Passes position player joins at
            //Center of the View port
            //create new player objec,t unique ID
            //Add to list of player then returns player
            PlayerData player = new PlayerData("joined", string.Empty, Players.Count().ToString(),"Player", pos.X, pos.Y);
            Players.Add(player);
            return player;



        }

    }
}