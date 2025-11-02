using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
// RSK Kontrolle ok
namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class PrintManager
    {

        // === Dependencies ===
        private readonly PrintManagerDependencies _deps;

        // === Fields ===
       
        // 2 HUD-Zeilen + 1 Trennlinie
        public const int BOARD_TOP_OFFSET = 3;
       


        public PrintManager(PrintManagerDependencies printManagerDependencies)
        {
            _deps = printManagerDependencies;
        }


        /// <summary>
        /// Read-only list for error messages
        /// </summary>
        readonly List<string> _listErrorMessages = new List<string>();
        /// <summary>
        /// Property for accessing the error list
        /// </summary>
        public List<string> ListErrorMessages => _listErrorMessages;
        /// <summary>
        /// Default message strings used for user prompts
        /// </summary>
        public string arraySizeXQuestion = "How long should the game board be?";
        public string arraySizeYQuestion = "How wide should the game board be?";

      
        public void PrintTile((int y,int x)position,char symbol)
        {

            Console.SetCursorPosition(position.x, position.y);
            Console.Write(symbol);
        }

        /// <summary>
        /// Method for printing a string message
        /// </summary>
        /// <param name="message"></param>
        public void PrintLine(string message)
        {
            Console.WriteLine(message);
        }
        /// <summary>
        /// Method for printing all collected messages from the list
        /// </summary>
        public void PrintList(List<string> list)
        {
            foreach (string message in list)
            {
                Console.WriteLine($"Error: {message}");
            }
        }
        /// <summary>
        /// Method for saving all error messages to a list
        /// </summary>
        /// <param name="errorMessage"></param>
        public void GetErrorMessage(string errorMessage)
        {
            _listErrorMessages.Add(errorMessage);
        }
        /// <summary>
        /// Method for getting a number from the player within a specified range
        /// </summary>
        /// <param name="question"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int AskForIntInRange(string question, int min, int max)
        {
            // variable for result value
            int result;
            while (true)
            {
                    // Output the question text and range (min and max)
                PrintLine(question + $"(between {min} and {max}):");
                // Read input from the player
                string input = Console.ReadLine();
                // Clear console for the next input attempt
                Console.Clear();
                
                
                // Try to convert the input to int and check if successful
                if (int.TryParse(input, out result))
                {
                    // If conversion succeeded, check if value is within range
                    if (result >= min && result <= max)
                        // Valid input -> return result and exit method
                        return result;
                    // Input was a number but outside the allowed range
                    else
                        PrintLine($"Invalid input! The number was not between {min} and {max}!");
                }
                // Conversion failed (input was not a number)
                else
                { PrintLine("Invalid input! Please enter only whole numbers!"); }
            }
        }
        /// <summary>
        /// Prints the current game board based on the objects stored in the GameObjectManager.
        /// </summary>
        public void PrintBoard()
        {
            // Access managers via dependencies
            
            int height = _deps.GameBoard.ArraySizeY;
            int width = _deps.GameBoard.ArraySizeX;

            // Optional: clear console or reset cursor
            
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(Program.CursorPosX, y + Program.CursorPosYGamBoardStart);

                for (int x = 0; x < width; x++)
                {
                    // 1️⃣ Versuche, ein Objekt an der Position zu finden
                    if (_deps.GameObject.TryGetObject((y, x), out var obj))
                    {
                        switch (obj)
                        {
                            case PlayerInstance player:
                                Console.Write(player.Symbol);
                                break;
                            case DoorInstance door:
                                Console.Write(door.Symbol);
                                break;
                            case NpcInstance npc:
                                Console.Write(npc.Meta.Symbol);
                                break;
                            case KeyFragmentInstance key:
                                Console.Write(key.Symbol);
                                break;
                            case WallInstance wall:
                                Console.Write(wall.Symbol);
                                break;
                            default:
                                Console.Write(_deps.Symbol.EmptySymbol);
                                break;
                        }
                    }
                    // 2️⃣ Wenn kein Objekt vorhanden → Empty-Tile
                    else
                    {
                        TileType tile =_deps.GameBoard.GetTileTyp((y, x));
                        Console.Write(tile == TileType.Empty ? _deps.Symbol.EmptySymbol : _deps.Symbol.DeathSymbol);
                    }
                }
                Console.WriteLine();
            }

            Console.ResetColor();
            _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}.{nameof(PrintBoard)}: Board successfully printed.");
        }

        
    }
}
