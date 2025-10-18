using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    enum TileTyp
    {
        None = 0,
        Wall,
        Door,
        Npc,
        Key,
        Player,
    }
    internal class GameBoardBuilder
    {
        /// Used only to read from printer and symbols
        private readonly PrinterManager _printer;
        private readonly SymbolsManager _symbols;

        /// <summary>
        /// Property to get printer and symbols connection
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="symbols"></param>
        public GameBoardBuilder(PrinterManager printer, SymbolsManager symbols)
        {
            _printer = printer;
            _symbols = symbols;
        }
        // Variablen
        private int _arraySizeY;
        private int _arraySizeX;
        /// <summary>
        /// Array variables
        /// </summary>
        private TileTyp[,] _gameBoardArray;
        
        
        /// Property for the game board array
        /// </summary>
        public TileTyp[,] GameBoardArray => _gameBoardArray;

        /// <summary>
        /// Property for array X
        /// </summary>
        public int ArraySizeX
        {
            get => _arraySizeX;
            set => _arraySizeX = value;
        }
        /// <summary>
        /// Property for array Y
        /// </summary>
        public int ArraySizeY
        {
            get => _arraySizeY;
            set => _arraySizeY = value;
        }
        /// <summary>
        /// List for wall positions to spawn rnd door later
        /// </summary>
        private readonly List<(int y, int x)> _listWallPositions = new List<(int y, int x)>();
        /// <summary>
        /// List for empty positions
        /// </summary>
        private readonly List<(int y, int x)> _listEmptyPositions = new List<(int y, int x)>();

        /// <summary>
        /// Property for wall positions list
        /// </summary>
        public List<(int y, int x)> ListWallPositions => _listWallPositions;
        /// <summary>
        /// Property for empty positions list
        /// </summary>
        public List<(int y, int x)> ListEmptyPositions => _listEmptyPositions;
        /// <summary>


        /// <summary>
        /// Asks the player for the board size (with min and max) and saves it
        /// </summary>
        public void DecideArraySize()
        {
            _arraySizeX = _printer.AskForIntInRange("How wide should the game board be?", 30, 120);
            _arraySizeY = _printer.AskForIntInRange("How high should the game board be?", 15, 20);
            _gameBoardArray = new TileTyp[_arraySizeY, _arraySizeX];
        }
        /// <summary>
        /// Fills array with symbols (walls + empty spaces)
        /// </summary>
        public void FillWallsToBoard()
        {
            // Row of the array
            for (int y = 0; y < _arraySizeY; y++)
            {
                // Column of the array
                for (int x = 0; x < _arraySizeX; x++)
                {
                    // Top and bottom row
                    if (y == 0 || y == _arraySizeY - 1)
                    {
                        _gameBoardArray[y, x] = TileTyp.Wall;
                        // Save wall position in list
                        _listWallPositions.Add((y, x));
                    }
                    // Left and right column
                    else if (x == 0 || x == _arraySizeX - 1)
                    {
                        _gameBoardArray[y, x] = _symbols.WallSideSymbol;
                        _listWallPositions.Add((y, x));
                    }
                    // Everything else is empty space
                    else
                    {
                        _gameBoardArray[y, x] = _symbols.EmptySymbol;
                        // Add empty positions to list
                        _listEmptyPositions.Add((y, x));
                    }
                }
            }
            // Corners of the game board
            _gameBoardArray[0, 0] = _symbols.WallLeftTopCornerSymbol;
            _gameBoardArray[_arraySizeY - 1, _arraySizeX - 1] = _symbols.WallRightBottomCornerSymbol;
            _gameBoardArray[0, _arraySizeX - 1] = _symbols.WallRightTopCornerSymbol;
            _gameBoardArray[_arraySizeY - 1, 0] = _symbols.WallLeftBottomCornerSymbol;
        }
    }
}
