using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Defines all available tile types used to build the game board.
    /// </summary>
    /// <remarks>
    /// Each <see cref="TileType"/> represents a specific element that can appear on the board, 
    /// such as walls, doors, NPCs, or items.  
    /// The enumeration is used by the <see cref="GameBoardManager"/> to store and update board state.
    /// </remarks>
    enum TileType
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
    /// Manages the creation, initialization, and manipulation of the game board grid.
    /// </summary>
    /// <remarks>
    /// The <see cref="GameBoardManager"/> is responsible for constructing the 
    /// 2D array that represents the playable area of the Escape Room game.  
    /// It handles array setup, boundary validation, wall placement, 
    /// and provides utility methods for tile modification and lookups.  
    /// All actions are logged using the diagnostic manager for debugging and validation.
    /// </remarks>
    internal class GameBoardManager
    {
        // === Dependencies ===
        private readonly GameBoardManagerDependencies _deps;

        // === Fields ===
        private int _arraySizeY;
        private int _arraySizeX;
        private TileType[,]? _gameBoardArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBoardManager"/> class with dependency injection.
        /// </summary>
        /// <param name="gameBoardBuilderDependencies">
        /// Provides access to diagnostics, print utilities, and the game object system
        /// for building and validating the board.
        /// </param>
        public GameBoardManager(GameBoardManagerDependencies gameBoardBuilderDependencies)
        {
            _deps = gameBoardBuilderDependencies;
            _deps.Diagnostics.AddCheck($"{nameof(GameBoardManager)}: successfully loaded");
        }

        /// <summary>
        /// Gets the underlying 2D array representing the game board.
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
        /// Asks the player to choose the board dimensions and initializes the game board.
        /// </summary>
        /// <remarks>
        /// If a previous board exists, a warning is logged and the old board is overwritten.  
        /// The width and height are chosen within predefined value ranges.
        /// </remarks>
        public void DecideArraySize()
        {
            // If a previous board exists, warn that it will be overwritten.
            if (_gameBoardArray != null)
            {
                _deps.Diagnostics.AddWarning($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: GameBoardArray was recreated.");
            }
            
            // Ask for dimensions within predefined limits.
            _arraySizeX = Program.ArraySizeX;
            _arraySizeY = Program.ArraySizeY;

            // Initialize a new 2D array based on user input.
            _gameBoardArray = new TileType[_arraySizeY, _arraySizeX];
            _deps.Diagnostics.AddCheck($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: Board successfully initialized ({_arraySizeX}x{_arraySizeY}).");
        }

        /// <summary>
        /// Fills the entire board with default wall tiles and sets up corner tiles.
        /// </summary>
        /// <remarks>
        /// Adds horizontal and vertical wall objects to the border using <see cref="WallInstance"/>.
        /// Logs an error if the board is not initialized, and a warning if existing data will be overwritten.
        /// </remarks>
        public void FillWallTypesToBoard()
        {
            // Check if the board exists and recreate before trying to fill it.
            if (_gameBoardArray == null)
            {
                _deps.Diagnostics.AddError($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Cannot fill walls. Board has not been initialized.");
                _gameBoardArray = new TileType[_arraySizeY, _arraySizeX];
            }

            // Optional: warn if it’s already filled with data (not empty anymore)
            if (_gameBoardArray[0, 0] != TileType.None)
            {
                _deps.Diagnostics.AddWarning($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Existing board data will be overwritten.");
            }
            // Iterate through all rows.
            for (int y = 0; y < _arraySizeY; y++)
            {
                // Iterate through all columns.
                for (int x = 0; x < _arraySizeX; x++)
                {
                    // Assign horizontal walls at the top and bottom.
                    if (y == 0 || y == _arraySizeY - 1)
                    {
                        _gameBoardArray[y, x] = TileType.WallHorizontal;
                       
                        
                    }
                    // Assign vertical walls on the left and right edges.
                    else if (x == 0 || x == _arraySizeX - 1)
                    {
                         _gameBoardArray[y, x] = TileType.WallVertical;
                        
                    }
                    // Everything else is empty space.
                    else
                    {
                        _gameBoardArray[y, x] = TileType.Empty;
                    }
                }
            }
            // Assign the four corner symbols.
            SetCorners();
        }

        /// <summary>
        /// Assigns the correct corner tiles to each corner of the game board.
        /// </summary>
        /// <remarks>
        /// This method defines the four board corner positions and delegates 
        /// the actual update process to <see cref="UpdateCorner((int y, int x), TileTyp)"/>.  
        /// Each corner tile is replaced by its respective wall corner type:
        /// <list type="bullet">
        /// <item><description>Top-Left → <see cref="TileType.WallCornerTopLeft"/></description></item>
        /// <item><description>Top-Right → <see cref="TileType.WallCornerTopRight"/></description></item>
        /// <item><description>Bottom-Left → <see cref="TileType.WallCornerBottomLeft"/></description></item>
        /// <item><description>Bottom-Right → <see cref="TileType.WallCornerBottomRight"/></description></item>
        /// </list>
        /// This ensures visual consistency and structural closure of the game board perimeter.
        /// </remarks>
        private void SetCorners()
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostics.AddError($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Cannot fill walls. Board has not been initialized.");
                _gameBoardArray = new TileType[_arraySizeY, _arraySizeX];
            }

            _gameBoardArray[0, 0] = TileType.WallCornerTopLeft;
            _gameBoardArray[0, _arraySizeX - 1] = TileType.WallCornerTopRight;
            _gameBoardArray[_arraySizeY - 1, 0] = TileType.WallCornerBottomLeft;
            _gameBoardArray[_arraySizeY - 1, _arraySizeX - 1] = TileType.WallCornerBottomRight;            
        }       

        /// <summary>
        /// Assigns a specific <see cref="TileType"/> to a given board position.
        /// </summary>
        /// <param name="position">The Y/X grid position of the target tile.</param>
        /// <param name="tileTyp">The new tile type to assign.</param>
        public void SetTile((int y, int x) position, TileType tileTyp)
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostics.AddError($"{nameof(GameBoardManager)}.{nameof(SetTile)}:GameBoardArray is null!");

                return;
            }

            if (!IsInBounds(position))
            {
                _deps.Diagnostics.AddWarning($"{nameof(GameBoardManager)}.{nameof(SetTile)}: Position {position} out of bounds.");
                return;
            }
            _gameBoardArray[position.y, position.x] = tileTyp;
        }

        /// <summary>
        /// Retrieves the tile type from a specific board position.
        /// </summary>
        /// <param name="position">The Y/X grid position of the tile.</param>
        /// <returns>The <see cref="TileType"/> at the given position.</returns>
        public TileType GetTileTyp((int y, int x) position)
        {
            if (_gameBoardArray == null)
            {

                _deps.Diagnostics.AddError($"{nameof(GameBoardManager)}.{nameof(GetTileTyp)}: GameBoardArray is null");

                return TileType.None;
            }
            return _gameBoardArray[position.y, position.x];


        }

        /// <summary>
        /// Attempts to retrieve the tile type from a specific position.
        /// </summary>
        /// <param name="position">The target grid position.</param>
        /// <param name="tile">Outputs the found tile type.</param>
        /// <returns><c>true</c> if the position is valid and contains a tile; otherwise, <c>false</c>.</returns>
        public bool TryGetTile((int y, int x) position, out TileType tile)
        {
            tile = TileType.None;

            if (_gameBoardArray == null || !IsInBounds(position))
                return false;

            tile = _gameBoardArray[position.y, position.x];
            return true;
        }

        /// <summary>
        /// Sets the specified position on the board to an empty tile.
        /// </summary>
        /// <param name="position">The target Y/X grid position.</param>
        public void SetTileToEmpty((int y, int x) position)
        {
            SetTile(position, TileType.Empty);
        }

        /// <summary>
        /// Checks whether a given position lies within the bounds of the board.
        /// </summary>
        /// <param name="position">The Y/X position to check.</param>
        /// <returns><c>true</c> if the position is valid; otherwise, <c>false</c>.</returns>
        public bool IsInBounds((int y, int x) position)
        {
            return position.y >= 0 && position.y < _arraySizeY &&
                   position.x >= 0 && position.x < _arraySizeX;
        }

        /// <summary>
        /// Initializes and fills the board structure with base wall and corner tiles.
        /// </summary>
        public void InitializeBoard()
        {
            DecideArraySize();
            FillWallTypesToBoard();
        }
    }
}
