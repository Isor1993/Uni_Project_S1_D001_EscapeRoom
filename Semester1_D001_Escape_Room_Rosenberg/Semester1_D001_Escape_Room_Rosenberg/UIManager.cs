using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class UIManager
    {
        private readonly GameBoardManager _boardBuilder;
        private readonly SymbolsManager _symbols;
        private readonly PrintManager _printer;
        private readonly SpawnManager _spawner;
        private readonly RandomManager _random;
       
        public UIManager(RandomManager random ,GameBoardManager boardBuilder, SymbolsManager symbols, PrintManager printer,SpawnManager spawner)
        {
            _boardBuilder = boardBuilder;
            _symbols = symbols;
            _printer = printer;
            _spawner=spawner;
            _random=random;
        }
       

        //TODO Später in richtige klasse machen
        string _playerName = "Joschi";
        int _lives = 3;
        int _keys = 3;
        bool _isDoorOpen = false;



        // maximal 28 x3 message lenght
        // Beispiel: Teil deines NPC- oder Dialogue-Systems
        // using System;
        // using System.Collections.Generic;

        private readonly List<string> _npcMessages = new List<string>();

        public void InitializeNpcMessages()
        {
            // Eine Test-Quizfrage (genau 72 Zeichen)
            _npcMessages.Add("Wie viele Speicherbits hat ein Byte und welche Werte kann es darstellen?");

            // Optional: weitere vorbefüllte Fragen kannst du hier anhängen
            // _npcMessages.Add("...");
        }

        // Holt eine zufällige Frage aus der Liste
        public string GetRandomNpcMessage()
        {
            if (_npcMessages.Count == 0)
                return "Keine Fragen verfügbar.";

            // Variante A (reines C# / Console):
            
            int idx = _random.Random.Next(0, _npcMessages.Count); // obere Grenze EXKLUSIV → kein Count-1 nötig
            return _npcMessages[idx];

            // Variante B (Unity):
            // int idx = UnityEngine.Random.Range(0, _npcMessages.Count); // int: obere Grenze EXKLUSIV
            // return _npcMessages[idx];
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
            string doorString = _isDoorOpen ? $"{_symbols.OpenDoorVerticalSymbol} = Open" : $"{_symbols.ClosedDoorVerticalSymbol} = Closed";
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
        /// <summary>
        /// Bottom Hud  line 1 anfang
        /// </summary>
        /// <param name="leftCornerTopSymbol"></param>
        /// <param name="rightCornerTopSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="midTDownSymbol"></param>
        /// <param name="TopSymbol"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_1(char leftCornerTopSymbol, char rightCornerTopSymbol, int lineMaxLenght, char midTDownSymbol, char TopSymbol)
        {
            string line = string.Empty;
            line += leftCornerTopSymbol + new string(TopSymbol, 15 - 1) + midTDownSymbol + new string(TopSymbol, (-17 + lineMaxLenght)) + rightCornerTopSymbol;
            return line;
        }
        /// <summary>
        /// Bottom Hud Line 2 with Message name and info
        /// </summary>
        /// <param name="leftZone"></param>
        /// <param name="rightZone"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="verticalHudSymbol"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Bottom Hud Line 3 trennlinie
        /// </summary>
        /// <param name="leftConnectSymbol"></param>
        /// <param name="rightConnectSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="midTUpSymbol"></param>
        /// <param name="TopSymbol"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_3(char leftConnectSymbol, char rightConnectSymbol, int lineMaxLenght, char midTUpSymbol, char TopSymbol)
        {
            string line = string.Empty;
            line += leftConnectSymbol + new string(TopSymbol, 15 - 1) + midTUpSymbol + new string(TopSymbol, (-17 + lineMaxLenght)) + rightConnectSymbol;
            return line;
        }
        /// <summary>
        /// Bottom Hud Line 4 with  message for npc first row
        /// </summary>
        /// <param name="SideSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_4(char SideSymbol, int lineMaxLenght, string text)
        {
            string line = string.Empty;
            line += SideSymbol + text + new string(' ', lineMaxLenght - 2 - text.Length) + SideSymbol;
            return line;
        }
        /// <summary>
        /// Bottom Hud Line 5 with message for npc second row
        /// </summary>
        /// <param name="SideSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_5(char SideSymbol, int lineMaxLenght, string text)
        {
            string line = string.Empty;
            line += SideSymbol + text + new string(' ', lineMaxLenght - 2 - text.Length) + SideSymbol;
            return line;
        }
        /// <summary>
        /// Bottom Hud Line 6 with message for npc third row
        /// </summary>
        /// <param name="SideSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_6(char SideSymbol, int lineMaxLenght, string text)
        {
            string line = string.Empty;
            line += SideSymbol + text + new string(' ', lineMaxLenght - 2 - text.Length) + SideSymbol;
            return line;
        }
        /// <summary>
        /// Bottom Hud Line 7 trennlinie
        /// </summary>
        /// <param name="leftConnectSymbol"></param>
        /// <param name="rightConnectSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="TopSymbol"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_7(char leftConnectSymbol, char rightConnectSymbol, int lineMaxLenght, char TopSymbol)
        {
            string line = string.Empty;
            line += leftConnectSymbol + new string(TopSymbol, lineMaxLenght - 2) + rightConnectSymbol;
            return line;
        }
        //TODO noch bauen richtig aktuell nur rahmen
        /// <summary>
        /// Bottom Hud Line 8 for player input answer npc
        /// </summary>
        /// <param name="SideSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_8(char SideSymbol, int lineMaxLenght)
        {
            string line = string.Empty;
            line += SideSymbol + new string(' ', lineMaxLenght - 2) + SideSymbol;
            return line;
        }
        /// <summary>
        /// Bottom Hud Line 9 end
        /// </summary>
        /// <param name="leftCornerBottomSymbol"></param>
        /// <param name="rightCornerBottomSymbol"></param>
        /// <param name="lineMaxLenght"></param>
        /// <param name="TopSymbol"></param>
        /// <returns></returns>
        public string BuildBottomHudLine_9(char leftCornerBottomSymbol, char rightCornerBottomSymbol, int lineMaxLenght, char TopSymbol)
        {
            string line = string.Empty;
            line += leftCornerBottomSymbol + new string(TopSymbol, lineMaxLenght - 2) + rightCornerBottomSymbol;
            return line;
        }

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
            // nur zum testen
            InitializeNpcMessages();
            


            string infoBoxSymbol = $"{symbol} x{count}";
            string infoBox = infoBoxstring + infoBoxSymbol;
            BuildBottomHudLine_1(_symbols.WallCornerTopLeftSymbol, _symbols.WallCornerTopRightSymbol, _boardBuilder.ArraySizeX, _symbols.WallTDownSymbol, _symbols.WallHorizontalSymbol);
            BuildBottomHudLine_2(messangerName, infoBox, _boardBuilder.ArraySizeY, _symbols.WallVerticalSymbol);
            BuildBottomHudLine_3(_symbols.WallCornerBottomLeftSymbol, _symbols.WallCornerBottomRightSymbol, _boardBuilder.ArraySizeX, _symbols.WallTUpSymbol, _symbols.WallHorizontalSymbol);

            int innerWidth = _boardBuilder.ArraySizeX - 2;
            List<string> cutTextLines = CutNpcMessageIntoLines(GetRandomNpcMessage(), innerWidth);

          




            BuildBottomHudLine_4(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX, cutTextLines[0]);
            BuildBottomHudLine_5(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX, cutTextLines.Count > 1 ? cutTextLines[1] : "");
            BuildBottomHudLine_6(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX, cutTextLines.Count > 2 ? cutTextLines[2] : "");

            BuildBottomHudLine_7(_symbols.WallTRightSymbol, _symbols.WallTLeftSymbol, _boardBuilder.ArraySizeX, _symbols.WallHorizontalSymbol);

            //TODO Umschreiben das readline funktioniert
            BuildBottomHudLine_8(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX);
            BuildBottomHudLine_9(_symbols.WallCornerBottomLeftSymbol, _symbols.WallCornerBottomRightSymbol, _boardBuilder.ArraySizeX, _symbols.WallHorizontalSymbol);


            //TODO nur fürs testen kommt noch in printer
            Console.WriteLine(BuildBottomHudLine_1(_symbols.WallCornerTopLeftSymbol, _symbols.WallCornerTopRightSymbol, _boardBuilder.ArraySizeX, _symbols.WallTDownSymbol, _symbols.WallHorizontalSymbol));
            Console.WriteLine(BuildBottomHudLine_2(messangerName, infoBox, _boardBuilder.ArraySizeY, _symbols.WallVerticalSymbol));
            Console.WriteLine(BuildBottomHudLine_3(_symbols.WallTRightSymbol, _symbols.WallTLeftSymbol, _boardBuilder.ArraySizeX, _symbols.WallTUpSymbol, _symbols.WallHorizontalSymbol));
            Console.WriteLine(BuildBottomHudLine_4(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX, cutTextLines[0]));
            Console.WriteLine(BuildBottomHudLine_5(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX, cutTextLines.Count > 1 ? cutTextLines[1] : ""));
            Console.WriteLine(BuildBottomHudLine_6(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX, cutTextLines.Count > 2 ? cutTextLines[2] : ""));
            Console.WriteLine(BuildBottomHudLine_7(_symbols.WallTRightSymbol, _symbols.WallTLeftSymbol, _boardBuilder.ArraySizeX, _symbols.WallHorizontalSymbol));
            Console.WriteLine(BuildBottomHudLine_8(_symbols.WallVerticalSymbol, _boardBuilder.ArraySizeX));
            Console.WriteLine(BuildBottomHudLine_9(_symbols.WallCornerBottomLeftSymbol, _symbols.WallCornerBottomRightSymbol, _boardBuilder.ArraySizeX, _symbols.WallHorizontalSymbol));
        }





























    }
}
