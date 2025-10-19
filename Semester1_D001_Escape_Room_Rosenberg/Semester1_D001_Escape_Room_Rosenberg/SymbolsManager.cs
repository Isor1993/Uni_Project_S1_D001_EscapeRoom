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

        private char _wallTDownSymbol = '\u2566'; //                 ' ╦ '  

        private char _wallTUpSymbol = '\u2569'; //                   ' ╩ '  

        private char _wallTRightSymbol = '\u2560'; //                ' ╠ '  

        private char _wallTLeftSymbol = '\u2563'; //                 ' ╣ '  

        private char _wallCrossSymbol = '\u256C'; //                 ' ╬ '  

        private char _wallHorizontalSymbol = '\u2550';//             ' ═ '

        private char _wallVerticalSymbol = '\u2551';//               ' ║'

        private char _wallCornerTopLeftSymbol = '\u2554';//          ' ╔ '

        private char _wallCornerTopRightSymbol = '\u2557';//         ' ╗ '

        private char _wallCornerBottomLeftSymbol = '\u255A';//       ' ╚ '

        private char _wallCornerBottomRightSymbol = '\u255D';//      ' ╝ '

        private char _playerSymbol = '\u2659';//                     ' ♙ '   //\u2603 Schneeman  \u2659 bauer

        private char _trapSymbol = '\u2297';// ;                     ' ⊗ '

        private char _fogSymbol = '\u2592';// ;                      ' ▒ '

        private char _deathSymbol = '\u2620';  //                    ' ☠ '

        private char _keyFragmentSymbol = '\u26BF';  //              ' ⚿ '

        private char _questSymbol = '\u003f';//                      ' ? '

        private char _emptySymbol = ' ';//                           '   '

        private char _closedDoorVerticalSymbol = '\u25AE';//         ' ▮ '

        private char _openDoorVerticalSymbol = '\u25AF';//           ' ▯ '

        private char _closedDoorHorizontalSymbol = '\u25AC';//          ' ▬ '

        private char _openDoorHorizontalSymbol = '\u25AD';//            ' ▭ '

        private char _timeWatchSymbol = '\u23f1';//                  ' ⏱ '

        private char _hearthSymbol = '\u2764';//                      ' ❤ '

        /// <summary>
        /// Properties for accessing or optionally changing symbols later
        /// </summary>

        public char WallTDownSymbol { get => _wallTDownSymbol; set => _wallTDownSymbol = value; }
        public char WallTUpSymbol { get => _wallTUpSymbol; set => _wallTUpSymbol = value; }
        public char WallTRightSymbol { get => _wallTRightSymbol; set => _wallTRightSymbol = value; }
        public char WallTLeftSymbol { get => _wallTLeftSymbol; set => _wallTLeftSymbol = value; }
        public char WallCrossSymbol { get => _wallCrossSymbol; set => _wallCrossSymbol = value; }
        public char WallHorizontalSymbol { get => _wallHorizontalSymbol; set => _wallHorizontalSymbol = value; }
        public char WallVerticalSymbol { get => _wallVerticalSymbol; set => _wallVerticalSymbol = value; }
        public char WallCornerTopLeftSymbol { get => _wallCornerTopLeftSymbol; set => _wallCornerTopLeftSymbol = value; }
        public char WallCornerTopRightSymbol { get => _wallCornerTopRightSymbol; set => _wallCornerTopRightSymbol = value; }
        public char WallCornerBottomLeftSymbol { get => _wallCornerBottomLeftSymbol; set => _wallCornerBottomLeftSymbol = value; }
        public char WallCornerBottomRightSymbol { get => _wallCornerBottomRightSymbol; set => _wallCornerBottomRightSymbol = value; }
        public char PlayerSymbol { get => _playerSymbol; set => _playerSymbol = value; }
        public char TrapSymbol { get => _trapSymbol; set => _trapSymbol = value; }
        public char FogSymbol { get => _fogSymbol; set => _fogSymbol = value; }
        public char DeathSymbol { get => _deathSymbol; set => _deathSymbol = value; }
        public char KeyFragmentSymbol { get => _keyFragmentSymbol; set => _keyFragmentSymbol = value; }
        public char QuestSymbol { get => _questSymbol; set => _questSymbol = value; }
        public char EmptySymbol { get => _emptySymbol; set => _emptySymbol = value; }
        public char OpenDoorVerticalSymbol { get => _openDoorVerticalSymbol; set => _openDoorVerticalSymbol = value; }
        public char ClosedDoorVerticalSymbol { get => _closedDoorVerticalSymbol; set => _closedDoorVerticalSymbol = value; }
        public char OpenDoororizontalSymbol { get => _openDoorHorizontalSymbol; set => _openDoorHorizontalSymbol = value; }
        public char ClosedDoorHorizontalSymbol { get => _closedDoorHorizontalSymbol; set => _closedDoorHorizontalSymbol = value; }
        public char TimeWatchSymbol { get => _timeWatchSymbol; set => _timeWatchSymbol = value; }
        public char HearthSymbol { get => _hearthSymbol; set => _hearthSymbol = value; }




    }
}
