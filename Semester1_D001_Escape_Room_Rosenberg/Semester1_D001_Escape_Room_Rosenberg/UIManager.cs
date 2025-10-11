using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class UIManager
    {

        //TODO Später in richtige klasse machen
        string _playerName = "Joschi";
        int _lives = 3;
        int _keys = 3;
        bool _isDoorOpen = false;
        List<string> _npcMessages;
        
        public List<string> NPCMessages
        {
            get => _npcMessages;
        }




        readonly GameBoardBuilder _boardBuilder;
        readonly SymbolsManager _symbols;
        readonly PrinterManager _printer;
        public UIManager(GameBoardBuilder boardBuilder, SymbolsManager symbols, PrinterManager printer)
        {
            _boardBuilder = boardBuilder;
            _symbols = symbols;
            _printer = printer;
        }

        private DateTime _startTime;

        public void StartTimer()
        {
            _startTime = DateTime.Now;
        }
        /// <summary>
        /// build UI Hud over Gameboard with Stats
        /// </summary>

        //TODO Print zu printer klasse schieben
        public void BuildTopHud()
        {
            // getting max boardWidth
            int boardWidth = _boardBuilder.GameBoardArray.GetLength(1);
            // Get time for Hud
            TimeSpan elapsed = DateTime.Now - _startTime;
            // Output string for Time with Symbol
            string timeString = $"{_symbols.TimeWatchSymbol} {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            // creating zones so that it gets always the same balanced layout size
            // First row with _playerName with Symbol and Time with Symbole
            string namePart = $"{_symbols.PlayerSymbol} {_playerName}";
            string lvlPart = $"Lvl = {Program.CurrentLevel}";
            string timePart = timeString;

            // print it on console
            Console.SetCursorPosition(0, 0);
            Console.Write(BuildCentredLine(namePart, lvlPart, timePart, boardWidth));
            // Second Row with lives keys and door open or closed with Operator

            string liveString = $"{_symbols.HearthSymbol}  x{_lives}";
            string keyString = $"{_symbols.KeyFragmentSymbol}  x{_keys}";
            string doorString = _isDoorOpen ? $"{_symbols.OpenDoorSideWallSymbol} = Open" : $"{_symbols.ClosedDoorSideWallSymbol} = Closed";
            // print second row on console
            Console.SetCursorPosition(0, 1);
            Console.Write(BuildCentredLine(liveString, doorString, keyString, boardWidth));
            // Test extraline
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new string('─', boardWidth));
        }


        private string BuildCentredLine(string leftZone, string midZone, string rightZone, int lineMaxLenght)
        {
            // Start position from midZone
            int midZoneStartPosition = (lineMaxLenght / 2) - (midZone.Length / 2);

            // line = LeftZone
            string line = leftZone;
            // calculate empty spaces betweeen leftZone and midZoneStartPosition
            int leftZoneSpace = Math.Max(1, (midZoneStartPosition - leftZone.Length));
            // add empty spaces to line
            line += new string(' ', leftZoneSpace);
            // add midZone to line
            line += midZone;
            // calculate empty spaces between M´midZoneSTartPosition and rightZone
            int rightZoneSpace = Math.Max(1, (lineMaxLenght - line.Length - rightZone.Length));
            // add empty spaces to line
            line += new string(' ', rightZoneSpace);
            // add rightZone to line
            line += rightZone;
            // return line
            return line;
        }
        public string BuildBottomHudLine_1(char leftCornerTopSymbol, char rightCornerTopSymbol, int lineMaxLenght, char midTDownSymbol, char TopSymbol)
        {
            string line = string.Empty;
            line += leftCornerTopSymbol + new string(TopSymbol, 15 - 1) + midTDownSymbol + new string(TopSymbol, (-17 + lineMaxLenght)) + rightCornerTopSymbol;
            return line;
        }
        public string BuildBottomHudLine_2(string leftZone, string rightZone, int lineMaxLenght, char verticalHudSymbol)
        {
            int charSymbol = 1;
            lineMaxLenght = _boardBuilder.GameBoardArray.GetLength(1);
            int leftZoneMax = 14;
            int rightZoneMax = _boardBuilder.ArraySizeX - (leftZoneMax + 1);
            string line = string.Empty;
            if (leftZone.Length > leftZoneMax)
            {
                leftZone = string.Empty;
                _printer.GetErrorMessage("Dev Error: BuildBottomHudline_1: string leftZone is too long!");
            }
            else
            {
                line = verticalHudSymbol + leftZone;
            }
            int leftZoneSpace = Math.Max(1, (15 - leftZone.Length - charSymbol));
            line += new string(' ', leftZoneSpace);
            line += verticalHudSymbol + rightZone;
            int rightZoneSpace = Math.Max(1, (rightZoneMax - rightZone.Length - charSymbol - charSymbol));
            line += new string(' ', rightZoneSpace);
            line += verticalHudSymbol;
            return line;
        }
        public string BuildBottomHudLine_3(char leftConnectSymbol, char rightConnectSymbol, int lineMaxLenght, char midTUpSymbol, char TopSymbol)
        {
            string line = string.Empty;
            line += leftConnectSymbol + new string(TopSymbol, 15 - 1) + midTUpSymbol + new string(TopSymbol, (-17 + lineMaxLenght)) + rightConnectSymbol;
            return line;
        }
        public string BuildBottomHudLine_4_5_6_8(char SideSymbol,int lineMaxLenght)
        {
            string line =string.Empty;
            line += SideSymbol + new string(' ', lineMaxLenght - 2)+SideSymbol;
            return line;
        }
        public string BuildBottomHudLine_7(char leftConnectSymbol, char rightConnectSymbol, int lineMaxLenght,char TopSymbol)
        {
            string line = string.Empty;            
            line += leftConnectSymbol + new string(TopSymbol, lineMaxLenght - 2) + rightConnectSymbol;
            return line;
        }
        public string BuildBottomHudLine_9(char leftCornerBottomSymbol, char rightCornerBottomSymbol, int lineMaxLenght, char TopSymbol)
        {
            string line = string.Empty;
            line += leftCornerBottomSymbol + new string(TopSymbol, lineMaxLenght - 2) + rightCornerBottomSymbol;
            return line;
        }
        /*
        public string BuildBottomHudLine_7()
        {

        }
        public string BuildBottomHudLine_8()
        {

        }
        public string BuildBottomHudLine_9()
        {

        }
        */
        /// <summary>
        /// Takes a full NPC message and splits it into multiple shorter lines.
        /// </summary>
        /// <param name="npcMessage">The original message string that will be split.</param>
        /// <param name="maxWidth">The maximum number of characters allowed per line.</param>
        /// <returns>A list of strings, each representing one formatted line of text.</returns>
        public List<string> CutNpcMessageIntoLines(string npcMessage, int maxWidth)
        {
            // Create a list to store the final message lines.
            List<string> lines = new List<string>();
            // Holds the current line being built; starts empty.
            string currentLine = string.Empty;

            // Split the NPC message into words using spaces as separators.
            foreach (string word in npcMessage.Split(' '))
            {
                // If adding the next word would exceed the maximum width... .
                if ((currentLine + word).Length > maxWidth)
                {
                    //Save the current line to the list.
                    lines.Add(currentLine);
                    // Start a new line.
                    currentLine = string.Empty;
                }
                // If there’s already content in the current line... . 
                if (currentLine.Length > 0)
                {
                    // Add a space before the next word.
                    currentLine += " ";
                }
                // Append the current word to the active line.
                currentLine += word;
            }
            // if it's not empty... .
            if (currentLine.Length > 0)
            {
                // Add the final line to the list
                lines.Add(currentLine);
            }
            
            // Return the formatted lines.
            return lines;
        }
        public void FillUpBottomHud(string messangerName, string infoBoxstring, char symbol, int count)
        {
            string infoBoxSymbol = $"{symbol} x{count}";
            string infoBox = infoBoxstring + infoBoxSymbol;
            BuildBottomHudLine_1(_symbols.WallLeftTopCornerSymbol, _symbols.WallRightTopCornerSymbol, _boardBuilder.ArraySizeX, _symbols.WallTDownSymbol, _symbols.WallTopSymbol);
            BuildBottomHudLine_2(messangerName, infoBox, _boardBuilder.ArraySizeY, _symbols.WallSideSymbol);
            BuildBottomHudLine_3(_symbols.WallLeftBottomCornerSymbol, _symbols.WallRightBottomCornerSymbol, _boardBuilder.ArraySizeX, _symbols.WallTUpSymbol, _symbols.WallTopSymbol);
            BuildBottomHudLine_4_5_6_8(_symbols.WallSideSymbol, _boardBuilder.ArraySizeX);
            BuildBottomHudLine_7(_symbols.WallTRightSymbol,_symbols.WallTLeftSymbol,_boardBuilder.ArraySizeX,_symbols.WallTopSymbol);
            BuildBottomHudLine_9(_symbols.WallLeftBottomCornerSymbol, _symbols.WallRightBottomCornerSymbol, _boardBuilder.ArraySizeX, _symbols.WallTopSymbol);
            Console.WriteLine(BuildBottomHudLine_1(_symbols.WallLeftTopCornerSymbol, _symbols.WallRightTopCornerSymbol, _boardBuilder.ArraySizeX, _symbols.WallTDownSymbol, _symbols.WallTopSymbol));
            Console.WriteLine(BuildBottomHudLine_2(messangerName, infoBox, _boardBuilder.ArraySizeY, _symbols.WallSideSymbol));
            Console.WriteLine(BuildBottomHudLine_3(_symbols.WallTRightSymbol, _symbols.WallTLeftSymbol, _boardBuilder.ArraySizeX, _symbols.WallTUpSymbol, _symbols.WallTopSymbol));
            for (int i = 0; i < 3; i++)
            { 
            Console.WriteLine(BuildBottomHudLine_4_5_6_8(_symbols.WallSideSymbol, _boardBuilder.ArraySizeX));
            }
            Console.WriteLine(BuildBottomHudLine_7(_symbols.WallTRightSymbol, _symbols.WallTLeftSymbol, _boardBuilder.ArraySizeX, _symbols.WallTopSymbol));
            Console.WriteLine(BuildBottomHudLine_4_5_6_8(_symbols.WallSideSymbol, _boardBuilder.ArraySizeX));
            Console.WriteLine(BuildBottomHudLine_9(_symbols.WallLeftBottomCornerSymbol, _symbols.WallRightBottomCornerSymbol, _boardBuilder.ArraySizeX, _symbols.WallTopSymbol));
        }





























    }
}
