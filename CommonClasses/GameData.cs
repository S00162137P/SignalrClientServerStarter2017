using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameData
{

    public class Position
    {
        public int X;
        public int Y;
    }

    public class PlayerData
    {
        public string header = string.Empty;
        public string playerID;
        public string imageName = string.Empty; // could be the name of character
        public string GamerTag = string.Empty;
        public string PlayerName = string.Empty;
        public int XP;
        public Position playerPosition = new Position { };
        public string Password;

        public PlayerData() { }
        public PlayerData(string messageHeader, string ImgName, string id, string tag, int x, int y)
        {
            header = messageHeader;
            playerID = id;
            imageName = ImgName;
            playerPosition = new Position { };
            playerPosition.X = x;
            playerPosition.Y = y;
            GamerTag = tag;
        }

        public string PlayerMessage(string header)
        {
            return header + ":" + playerID + ":" + playerPosition.X.ToString() + ":" + playerPosition.Y.ToString();
        }


    }
    public class LeavingData
    {
        public string playerID;
        public string Tag;
    }
    public class CollectableData
    {
        public int ID;
        public Position position;
        public int worth; 
        public CollectableData(int id,Position p,int val){
            ID = id;
            position = p;
            worth = val;
        }

    }
    public class WorldSize
    {
        public int X;
        public int y;

    }

    public class JoinRequestMessage
    {
        public string TagName;
        public string Password;

    }

    public class TestMess
    {
        public string message = "Hello there";
    }

    public class ErrorMess
    {
        public string message = "Error --> ";
    }
    
    



}
