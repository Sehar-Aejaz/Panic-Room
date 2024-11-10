using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace PuzzleGame
{
    public class Room
    {
        // Room properties
        public string Name { get; set; }
        public Puzzle Puzzle { get; set; }
        public Window roomWindow;

        // Method to display start window
        public void StartWindow()
        {
            roomWindow = new Window("start", 700, 700);
            Bitmap roomImage = new Bitmap("start", "start.jpg");
            roomWindow.DrawBitmap(roomImage, 0, 0);
            roomWindow.Refresh();
        }

        // Method to display win window when player completes all riddles
        public void WinWindow()
        {
            roomWindow = new Window("win", 700, 700);
            Bitmap roomImage = new Bitmap("win", "win.jpg");
            roomWindow.DrawBitmap(roomImage, 0, 0);
            roomWindow.Refresh();
        }

        // Method to display room
        public void DisplayRoom(int roomNumber)
        {
            const int WINDOW_WIDTH = 700;
            const int WINDOW_HEIGHT = 700;

            roomWindow = new Window("Room", WINDOW_WIDTH, WINDOW_HEIGHT);
            // Write puzzle on the terminal
            Console.WriteLine("Roon: " + Name);
            Console.WriteLine("Puzzle: " + Puzzle.Question);
            string roomStr1 = $"i{roomNumber+1}";
            string roomStr2 = $"i{roomNumber+1}.jpg";
            Bitmap roomImage = new Bitmap(roomStr1, roomStr2);
            roomWindow.DrawBitmap(roomImage, 0, 0);
            roomWindow.Refresh(60);

        }

    }
}


