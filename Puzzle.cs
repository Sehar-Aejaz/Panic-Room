/// <summary>
/// Puzzle.cs
/// </summary>
using System;
using System.Collections.Generic;
using SplashKitSDK;
namespace  PuzzleGame
{
    public class Puzzle
    {
        // Puzzle properties
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Clue { get; set; }
    }
}

/// <summary>
/// Room.cs
/// </summary>
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

/// <summary>
/// Player.cs
/// </summary>
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

/// <summary>
/// Program.cs
/// </summary>
using System;
using System.Collections.Generic;
using SplashKitSDK;
namespace PuzzleGame
{
    public class Game
    {
        static void Main(string[] args)
        {
            // Define the path to your CSV file
            string filePath = "puzzle.tsv";

            // Sound effects
            SoundEffect bg = new SoundEffect("bg", "bg.mp3");
            SoundEffect correct = new SoundEffect("yay", "yay.mp3");
            SoundEffect wrong = new SoundEffect("wrong", "wrong_answer.mp3");
            SoundEffect win = new SoundEffect("win", "win.mp3");
            SoundEffect start = new SoundEffect("start", "start.mp3");

            // Bitmaps
            Bitmap yay = new Bitmap("yay", "yay.png");
            Bitmap ohNo = new Bitmap("ohNo", "oh-no.png");
            
            
            // Create a list to store the data
            List<string> serialNumber = new List<string>();
            List<string> roomName = new List<string>();
            List<string> descriptions = new List<string>();
            List<string> questions = new List<string>();
            List<string> answers = new List<string>();
            List<string> clues = new List<string>();


            // Open the TSV file
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Read the file line by line until the end
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split the line into fields using tab space as the delimiter
                        string[] fields = line.Split('\t');

                        int x = 0;

                        // Add each field to the respective list
                        foreach (string field in fields)
                        {
                            if (x == 0) serialNumber.Add(field);
                            if (x == 1) roomName.Add(field);
                            if (x == 2) descriptions.Add(field);
                            if (x == 3) questions.Add(field);
                            if (x == 4) answers.Add(field);
                            if (x == 5) clues.Add(field);
                            x++;
                        }

                    }
                }

            // Create a list of rooms
            List<Room> rooms = new List<Room>();
            
            // Create rooms with puzzles
            for (int i = 1; i < questions.Count; i++)
            {
                Puzzle puzzle = new Puzzle
                {
                    Question = questions[i],
                    Answer = answers[i],
                    Clue = clues[i]
                };

                Room room = new Room
                {
                    Name = roomName[i],
                    Puzzle = puzzle
                };

                rooms.Add(room);
            }

            // Create player
            Player player = new Player
            {
                CurrentRoom = rooms[0],
            };

            // Displa starting window
            Console.WriteLine("WELCOME TO PUZZLE PANIC! Answer the following riddles to GET OUT of here.\n");
            player.CurrentRoom.StartWindow();
            start.Play();
            SplashKit.Delay(7000);
            player.CurrentRoom.roomWindow.Close();

            // Start the game loop
            int currentRoomIndex = 0;
            player.CurrentRoom.DisplayRoom(currentRoomIndex);
            while (currentRoomIndex < rooms.Count)
            {
                bg.Play();

                // Ask for the answer directly
                Console.WriteLine("Enter your answer:");
                string answer = Console.ReadLine();

                // Check if given answer is the correct answer
                if (answer.Equals(player.CurrentRoom.Puzzle.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("You have successfully escaped the room!");
                    player.CurrentRoom.roomWindow.DrawBitmap(yay, 150, 150);
                    player.CurrentRoom.roomWindow.Refresh();
                    bg.Stop();
                    correct.Play();
                    SplashKit.Delay(1000);


                    // Move to the next room if available
                    currentRoomIndex++;
                    if (currentRoomIndex < rooms.Count)
                    {
                        player.MoveRooms(rooms[currentRoomIndex-1], rooms[currentRoomIndex], currentRoomIndex);
                        
                    }
                    else
                    {
                        // Display win message
                        bg.Stop();
                        Console.WriteLine("Congratulations! You solved PUZZLE PANIC!");
                        player.CurrentRoom.WinWindow();
                        win.Play();
                        SplashKit.Delay(10000);
                        player.CurrentRoom.roomWindow.Close();
                        break;
                    }
                }
                else
                {
                    // Message if answer is wrong
                    player.CurrentRoom.roomWindow.DrawBitmap(ohNo, 150, 150);
                    player.CurrentRoom.roomWindow.Refresh();
                    bg.Stop();
                    Console.WriteLine("Incorrect answer. Try again with the help of a clue.");
                    wrong.Play();
                    SplashKit.Delay(6000);
                    Console.WriteLine(player.CurrentRoom.Puzzle.Clue);
                    
                }
            }

        }
    }
}