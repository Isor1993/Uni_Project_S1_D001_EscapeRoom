/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : PrintManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles all text and symbol rendering in the console.
* Responsible for displaying the game board, messages, and user prompts.
* Includes safe console output and user input handling with diagnostics logging.
*
* Responsibilities:
* - Render the complete game board (tiles, walls, NPCs, keys, player, door)
* - Display messages, warnings, and system logs
* - Manage user input for numeric questions with validation
* - Catch and log all console-related exceptions
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// Handles all visual console output for the Escape Room game.
    /// Prints the game board, NPCs, items, and handles player input safely.
    /// </summary>
    internal class PrintManager
    {
        // === Dependencies ===
        private readonly PrintManagerDependencies _deps;

        // === Fields ===               
        public const int BOARD_TOP_OFFSET = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintManager"/> class.
        /// </summary>
        /// <param name="printManagerDependencies">
        /// Reference container providing access to diagnostics, symbols,
        /// game board and object managers.
        /// </param>
        public PrintManager(PrintManagerDependencies printManagerDependencies)
        {
            _deps = printManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(PrintManager)}: Initialized successfully.");
        }


        /// <summary>
        /// Internal list for storing error messages during runtime.
        /// </summary>
        readonly List<string> _listErrorMessages = new List<string>();

        /// <summary>
        /// Exposes all stored error messages as a read-only property.
        /// </summary>
        public List<string> ListErrorMessages => _listErrorMessages;

        /// <summary>
        /// Default question texts used when asking the user for array dimensions.
        /// </summary>
        public string arraySizeXQuestion = "How long should the game board be?";
        public string arraySizeYQuestion = "How wide should the game board be?";

        /// <summary>
        /// Prints a single symbol on the console at the specified position.
        /// </summary>
        /// <param name="position">Tuple (y, x) indicating console coordinates.</param>
        /// <param name="symbol">Character symbol to display.</param>
        /// <remarks>
        /// Wrapped in a try-catch block to prevent <see cref="ArgumentOutOfRangeException"/>
        /// if coordinates exceed console bounds.
        /// </remarks>
        public void PrintTile((int y, int x) position, char symbol)
        {
            try
            {
                Console.SetCursorPosition(position.x, position.y);
                Console.Write(symbol);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _deps.Diagnostic.AddError($"{nameof(PrintManager)}.{nameof(PrintTile)}: Invalid cursor position {position}. Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Prints a single message line to the console.
        /// </summary>
        /// <param name="message">The text message to print.</param>
        public void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Prints all messages contained in the provided list with timestamp prefix.
        /// </summary>
        /// <param name="list">List of messages to output.</param>
        public void PrintList(List<string> list)
        {
            foreach (string message in list)
            {
                Console.WriteLine($" Error: {message}");
            }
        }

        /// <summary>
        /// Adds a single error message to the internal message list.
        /// </summary>
        /// <param name="errorMessage">The error text to store.</param>
        public void GetErrorMessage(string errorMessage)
        {
            _listErrorMessages.Add(errorMessage);
        }

        /// <summary>
        /// Requests a numeric value from the user within the specified range.
        /// </summary>
        /// <param name="question">The question to display.</param>
        /// <param name="min">Minimum valid number.</param>
        /// <param name="max">Maximum valid number.</param>
        /// <returns>The validated integer input by the user.</returns>
        /// <remarks>
        /// Includes exception handling for <see cref="IOException"/> and general errors.
        /// Clears the console after each attempt to keep the interface clean.
        /// </remarks>
        public int AskForIntInRange(string question, int min, int max)
        {
            // variable for result value
            int result;
            while (true)
            {
                try
                {
                    // Output the question text and range (min and max)
                    PrintLine(question + $"(between {min} and {max}):");
                    // Read input from the player
                    string? input = Console.ReadLine();
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
                catch (IOException ex)
                {
                    _deps.Diagnostic.AddException($"{nameof(PrintManager)}.{nameof(AskForIntInRange)}: IO error during input: {ex.Message}");
                    PrintLine("A console error occurred. Please try again.");
                }
                catch (Exception ex)
                {
                    _deps.Diagnostic.AddException($"{nameof(PrintManager)}.{nameof(AskForIntInRange)}: Unexpected error: {ex.Message}");
                    PrintLine("Unexpected error occurred. Please try again.");
                }
            }
        }

        /// <summary>
        /// Renders the current game board and all registered game objects.
        /// </summary>
        /// <remarks>
        /// Iterates through all grid positions and prints the corresponding
        /// symbol for each object. If no object exists at a position,
        /// prints the default empty symbol.
        /// Includes full exception safety to prevent crashes from console or data errors.
        /// </remarks>
        public void PrintBoard()
        {
            try
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
                            TileType tile = _deps.GameBoard.GetTileType((y, x));
                            Console.Write(tile == TileType.Empty ? _deps.Symbol.EmptySymbol : _deps.Symbol.DeathSymbol);
                        }
                    }
                    Console.WriteLine();
                }

                Console.ResetColor();
                _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}.{nameof(PrintBoard)}: Board successfully printed.");
            }
            catch (Exception ex)
            {
                _deps.Diagnostic.AddException($"{nameof(PrintManager)}.{nameof(PrintBoard)}: Unexpected error during rendering — {ex.Message}");
            }
        }
    }
}
