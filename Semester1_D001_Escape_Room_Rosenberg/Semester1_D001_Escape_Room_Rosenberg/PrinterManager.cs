using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
// RSK Kontrolle ok
namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class PrinterManager
    {
        // 2 HUD-Zeilen + 1 Trennlinie
        public const int BoardTopOffset = 3; 

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
        #region DEBUG METHODS
        // Used for debugging 2D board layouts
        //TODO Delete later
        /// <summary>
        /// Prints a 2D character array to the console (for testing/debugging purposes)
        /// </summary>
        /// <param name="Array"></param>
        public void PrintArray(char[,] Array)
        {

            // Iterate through the first dimension (rows)
            for (int y = 0; y < Array.GetLength(0); y++)
            {
                Console.SetCursorPosition(0, y + BoardTopOffset);
                // Iterate through the second dimension (columns)
                for (int x = 0; x < Array.GetLength(1); x++)
                {
                    Console.Write(Array[y, x]);
                }
                Console.WriteLine();
            }

        }
        // Public void PrintArray(char[,] Array) { ... }
        #endregion


        public void PrintUIHud()
        {

        }
    }
}
