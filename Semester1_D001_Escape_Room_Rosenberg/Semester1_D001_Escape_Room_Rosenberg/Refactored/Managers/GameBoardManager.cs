/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : GameBoardManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Manages creation, initialization, and tile access of the Escape Room board.
* Handles wall placement, corner setup, and boundary validation.
* Uses DiagnosticsManager for logging all operations.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Defines all available tile types used to represent visual and logical
    /// elements of the game board.
    /// </summary>
    internal enum TileType
    {
        None = 0,
        Empty,
        WallHorizontal,
        WallVertical,
        WallCornerTopLeft,
        WallCornerTopRight,
        WallCornerBottomLeft,
        WallCornerBottomRight,
        Door,
        Npc,
        Key,
        Player,
    }
    /// <summary>
    /// Controls creation, initialization, and manipulation of the Escape Room
    /// game board. Handles board setup, tile placement, and boundary checks.
    /// </summary>
    internal class GameBoardManager
    {
        // === Dependencies ===
        private readonly GameBoardManagerDependencies _deps;

        // === Fields ===
        private int _arraySizeY;
        private int _arraySizeX;
        private TileType[,]? _gameBoardArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBoardManager"/> class
        /// and registers a successful creation message with the diagnostics system.
        /// </summary>
        /// <param name="gameBoardManagerDependencies">
        /// Dependency record providing a <see cref="DiagnosticsManager"/> reference.
        /// </param>
        public GameBoardManager(GameBoardManagerDependencies gameBoardManagerDependencies)
        {
            _deps = gameBoardManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets the two-dimensional array representing the game board.
        /// </summary>
        public TileType[,]? GameBoardArray => _gameBoardArray;

        /// <summary>
        /// Gets the horizontal size (X dimension) of the board.
        /// </summary>
        public int ArraySizeX => _arraySizeX;

        /// <summary>
        /// Gets the vertical size (Y dimension) of the board.
        /// </summary>
        public int ArraySizeY => _arraySizeY;

        /// <summary>
        /// Initializes the entire game board. 
        /// Creates the array, fills wall tiles, and assigns corners.
        /// </summary>
        public void InitializeBoard()
        {
            DecideArraySize();
            FillWallTypesToBoard();
            _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}.{nameof(InitializeBoard)}: Board fully initialized.");
        }

        /// <summary>
        /// Defines the board dimensions and creates the underlying 2D array.
        /// Logs warnings if a previous board already exists.
        /// </summary>
        public void DecideArraySize()
        {

            if (_gameBoardArray != null)
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: GameBoardArray was recreated.");
            }           

            _arraySizeX = Program.ArraySizeX;
            _arraySizeY = Program.ArraySizeY;

            _gameBoardArray = new TileType[_arraySizeY, _arraySizeX];
            _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: Board successfully initialized ({_arraySizeX}x{_arraySizeY}).");
        }

        /// <summary>
        /// Sets a specific <see cref="TileType"/> at a given position on the board.
        /// Performs boundary and null checks before assignment.
        /// </summary>
        /// <param name="position">Tuple containing (y, x) grid coordinates.</param>
        /// <param name="tileType">The tile type to assign at the given position.</param>
        public void SetTile((int y, int x) position, TileType tileType)
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(SetTile)}: GameBoardArray is null!");

                return;
            }

            if (!IsInBounds(position))
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(SetTile)}: Position {position} out of bounds.");
                return;
            }
            _gameBoardArray[position.y, position.x] = tileType;
        }

        /// <summary>
        /// Retrieves the <see cref="TileType"/> from a specified grid position.
        /// Returns <see cref="TileType.None"/> if invalid.
        /// </summary>
        /// <param name="position">Tuple containing (y, x) grid coordinates.</param>
        /// <returns>The tile type at the specified position.</returns>
        public TileType GetTileType((int y, int x) position)
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(GetTileType)}: GameBoardArray is null");

                return TileType.None;
            }
            if (!IsInBounds(position))
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(GetTileType)}: Position {position} out of bounds.");
                return TileType.None;
            }
            return _gameBoardArray[position.y, position.x];
        }

        /// <summary>
        /// Sets the given board position to <see cref="TileType.Empty"/>.
        /// </summary>
        /// <param name="position">Tuple containing (y, x) grid coordinates.</param>
        public void SetTileToEmpty((int y, int x) position)
        {
            SetTile(position, TileType.Empty);
        }

        /// <summary>
        /// Fills the board with wall and empty tiles. 
        /// Top and bottom edges receive horizontal walls,
        /// left and right edges receive vertical walls,
        /// and all inner tiles are initialized as empty.
        /// </summary>
        private void FillWallTypesToBoard()
        {

            if (_gameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Cannot fill walls. Board has not been initialized.");
                _gameBoardArray = new TileType[_arraySizeY, _arraySizeX];
            }


            if (_gameBoardArray[0, 0] != TileType.None)
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Existing board data will be overwritten.");
            }

            for (int y = 0; y < _arraySizeY; y++)
            {

                for (int x = 0; x < _arraySizeX; x++)
                {

                    if (y == 0 || y == _arraySizeY - 1)
                    {
                        _gameBoardArray[y, x] = TileType.WallHorizontal;
                    }

                    else if (x == 0 || x == _arraySizeX - 1)
                    {
                        _gameBoardArray[y, x] = TileType.WallVertical;
                    }

                    else
                    {
                        _gameBoardArray[y, x] = TileType.Empty;
                    }
                }
            }

            SetCorners();
        }

        /// <summary>
        /// Assigns the correct wall corner tiles to the four board corners.
        /// </summary>
        private void SetCorners()
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(SetCorners)}: Cannot fill walls. Board has not been initialized.");
                _gameBoardArray = new TileType[_arraySizeY, _arraySizeX];
            }

            _gameBoardArray[0, 0] = TileType.WallCornerTopLeft;
            _gameBoardArray[0, _arraySizeX - 1] = TileType.WallCornerTopRight;
            _gameBoardArray[_arraySizeY - 1, 0] = TileType.WallCornerBottomLeft;
            _gameBoardArray[_arraySizeY - 1, _arraySizeX - 1] = TileType.WallCornerBottomRight;
        }

        /// <summary>
        /// Attempts to retrieve the tile type from a given position.
        /// Returns true if the operation succeeds.
        /// </summary>
        /// <param name="position">Tuple containing (y, x) grid coordinates.</param>
        /// <param name="tile">Outputs the found tile type.</param>
        /// <returns><c>true</c> if valid position and tile exist; otherwise false.</returns>
        private bool TryGetTile((int y, int x) position, out TileType tile)
        {
            tile = TileType.None;

            if (_gameBoardArray == null || !IsInBounds(position))
                return false;

            tile = _gameBoardArray[position.y, position.x];
            return true;
        }       

        /// <summary>
        /// Determines whether the given position lies inside the current board boundaries.
        /// </summary>
        /// <param name="position">Tuple containing (y, x) grid coordinates.</param>
        /// <returns><c>true</c> if position is inside the valid board range; otherwise false.</returns>
        private bool IsInBounds((int y, int x) position)
        {
            return position.y >= 0 && position.y < _arraySizeY &&
                   position.x >= 0 && position.x < _arraySizeX;
        }
    }
}