using System;
using System.Collections.Generic;
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
        readonly GameBoardBuilder _boardBuilder;
        readonly SymbolsManager _symbols;
        public UIManager(GameBoardBuilder boardBuilder, SymbolsManager symbols)
        {
            _boardBuilder = boardBuilder;
            _symbols = symbols;
        }

        private DateTime _startTime;

        public void StartTimer()
        {
            _startTime = DateTime.Now;
        }
        /// <summary>
        /// build UI Hud over Gameboard with Stats
        /// </summary>
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
        































    }
}
