using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// RSK Kontrolle ok
namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class SymbolsManager
    {
        /// <summary>
        /// Symbols used throughout the game
        /// Each symbol can be accessed or modified via its corresponding property.
        /// </summary>
        private char _wallTopSymbol = '\u2550';//                    ' ═ '

        private char _wallSideSymbol = '\u2551';//                   ' ║'

        private char _wallLeftTopCornerSymbol = '\u2554';//          ' ╔ '

        private char _wallRightTopCornerSymbol = '\u2557';//         ' ╗ '

        private char _wallLeftBottomCornerSymbol = '\u255A';//       ' ╚ '

        private char _wallRightBottomCornerSymbol = '\u255D';//      ' ╝ '

        private char _playerSymbol = '\u2659';//                     ' ♙ '   //\u2603 Schneeman  \u2659 bauer

        private char _trapSymbol = '\u2297';// ;                     ' ⊗ '

        private char _fogSymbol = '\u2592';// ;                      ' ▒ '

        private char _deathSymbol = '\u2620';  //                    ' ☠ '

        private char _keyFragmentSymbol = '\u26BF';  //              ' ⚿ '

        private char _questSymbol = '\u003f';//                      ' ? '

        private char _emptySymbol = ' ';//                           '   '

        private char _closedDoorSideWallSymbol = '\u25AE';//         ' ▮ '

        private char _openDoorSideWallSymbol = '\u25AF';//           ' ▯ '

        private char _closedDoorTopWallSymbol = '\u25AC';//          ' ▬ '

        private char _openDoorTopWallSymbol = '\u25AD';//            ' ▭ '

        private char _timeWatchSymbol = '\u23f1';//                  ' ⏱ '

        private char _hearthSymbol = '\u2764';//                      ' ❤ '

        /// <summary>
        /// Properties for accessing or optionally changing symbols later
        /// </summary>
        public char WallTopSymbol { get => _wallTopSymbol; set => _wallTopSymbol = value; }
        public char WallSideSymbol { get => _wallSideSymbol; set => _wallSideSymbol = value; }
        public char WallLeftTopCornerSymbol { get => _wallLeftTopCornerSymbol; set => _wallLeftTopCornerSymbol = value; }
        public char WallRightTopCornerSymbol { get => _wallRightTopCornerSymbol; set => _wallRightTopCornerSymbol = value; }
        public char WallLeftBottomCornerSymbol { get => _wallLeftBottomCornerSymbol; set => _wallLeftBottomCornerSymbol = value; }
        public char WallRightBottomCornerSymbol { get => _wallRightBottomCornerSymbol; set => _wallRightBottomCornerSymbol = value; }
        public char PlayerSymbol { get => _playerSymbol; set => _playerSymbol = value; }
        public char TrapSymbol { get => _trapSymbol; set => _trapSymbol = value; }
        public char FogSymbol { get => _fogSymbol; set => _fogSymbol = value; }
        public char DeathSymbol { get => _deathSymbol; set => _deathSymbol = value; }
        public char KeyFragmentSymbol { get => _keyFragmentSymbol; set => _keyFragmentSymbol = value; }
        public char QuestSymbol { get => _questSymbol; set => _questSymbol = value; }
        public char EmptySymbol { get => _emptySymbol; set => _emptySymbol = value; }
        public char OpenDoorSideWallSymbol { get => _openDoorSideWallSymbol; set => _openDoorSideWallSymbol = value; }
        public char ClosedDoorSideWallSymbol { get => _closedDoorSideWallSymbol; set => _closedDoorSideWallSymbol = value; }
        public char OpenDoorTopWallSymbol { get => _openDoorTopWallSymbol; set => _openDoorTopWallSymbol = value; }
        public char ClosedDoorTopWallSymbol { get => _closedDoorTopWallSymbol; set => _closedDoorTopWallSymbol = value; }
        public char TimeWatchSymbol { get => _timeWatchSymbol; set => _timeWatchSymbol = value; }
        public char HearthSymbol { get => _hearthSymbol; set => _hearthSymbol = value; }




    }
}
