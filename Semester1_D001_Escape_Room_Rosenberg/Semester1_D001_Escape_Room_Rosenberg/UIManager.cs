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
        public void BuildHud()
        {
            // getting max boardWidth
            int boardWidth = _boardBuilder.GameBoardArray.GetLength(1);
            // Get time for Hud
            TimeSpan elapsed = DateTime.Now - _startTime;
            // Output string for Time with Symbol
            string timeString = $"{_symbols.TimeWatchSymbol} {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            // creating zones so that it gets always the same balanced layout size
            int zone_1 = (int)(boardWidth * 0.33);
            int zone_2 = (int)(boardWidth * 0.34);
            int zone_3 = boardWidth - zone_1 - zone_2;
            // First row with _playerName with Symbol and Time with Symbole
            string namePart = $"{_symbols.PlayerSymbol} {_playerName}";
            string timePart = timeString.PadLeft(zone_3);
            string line_1 = namePart.PadRight(zone_1 + zone_2) + timePart;
            // print it on console
            Console.SetCursorPosition(0, 0);
            Console.Write(line_1.PadRight(boardWidth));
            // Second Row with lives keys and door open or closed with Operator
            string liveString = new string(_symbols.HearthSymbol, _lives);
            string keyString = $"{_symbols.KeyFragmentSymbol} x{_keys}";
            string doorString = _isDoorOpen ? $"{_symbols.OpenDoorSideWallSymbol} = Open" : $"{_symbols.ClosedDoorSideWallSymbol} = Closed";
            // print second row on console
            string line2 = $"{_symbols.HearthSymbol}{_keys}".PadRight(zone_1 + zone_2) + doorString.PadLeft(zone_3);
            Console.SetCursorPosition(0, 1);
            Console.Write(line2.PadRight(boardWidth));
            // Test extraline
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new string('─', boardWidth));









        }









    }
}
