using System;
using System.Collections.Generic;
using SplashKitSDK;
namespace PuzzleGame
{
    public class Player
    {
        // Player class property
        public Room CurrentRoom { get; set; }

        // Method to move to the next room
        public void MoveRooms(Room lastRoom, Room nextRoom, int roomNum)
        {
            lastRoom.roomWindow.Close();
            Console.WriteLine("You moved to the next room.");
            CurrentRoom = nextRoom;
            CurrentRoom.DisplayRoom(roomNum);
        }
    }
}

