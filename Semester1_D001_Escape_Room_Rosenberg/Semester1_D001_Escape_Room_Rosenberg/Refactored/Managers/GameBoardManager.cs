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
    /// Each <see cref="TileTyp"/> represents a specific element that can appear on the board, 
    /// such as walls, doors, NPCs, or items.  
    /// The enumeration is used by the <see cref="GameBoardManager"/> to store and update board state.
    /// </remarks>
    enum TileTyp
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
        private readonly GameBoardBuilderDependencies _deps;

        // === Fields ===
        private int _arraySizeY;
        private int _arraySizeX;
        private TileTyp[,]? _gameBoardArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBoardManager"/> class with dependency injection.
        /// </summary>
        /// <param name="gameBoardBuilderDependencies">
        /// Provides access to diagnostics, print utilities, and the game object system
        /// for building and validating the board.
        /// </param>
        public GameBoardManager(GameBoardBuilderDependencies gameBoardBuilderDependencies)
        {
            _deps = gameBoardBuilderDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}: successfully loaded");
        }

        /// <summary>
        /// Gets the underlying 2D array representing the game board.
        /// </summary>
        public TileTyp[,]? GameBoardArray => _gameBoardArray;

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
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: GameBoardArray was recreated.");
            }

            // Ask for dimensions within predefined limits.
            _arraySizeX = _deps.Print.AskForIntInRange("How wide should the game board be?", 30, 120);
            _arraySizeY = _deps.Print.AskForIntInRange("How high should the game board be?", 15, 20);

            // Initialize a new 2D array based on user input.
            _gameBoardArray = new TileTyp[_arraySizeY, _arraySizeX];
            _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: Board successfully initialized ({_arraySizeX}x{_arraySizeY}).");
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
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Cannot fill walls. Board has not been initialized.");
                _gameBoardArray = new TileTyp[_arraySizeY, _arraySizeX];
            }

            // Optional: warn if it’s already filled with data (not empty anymore)
            if (_gameBoardArray[0, 0] != TileTyp.None)
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Existing board data will be overwritten.");
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
                        //TODO löschen wenn funktioniert :_gameBoardArray[y, x] = TileTyp.WallHorizontal;
                        WallInstance wall = new WallInstance(_deps.WallInstanceDeps);
                        wall.Initialize(TileTyp.WallHorizontal, (y, x));
                        _deps.GameObject.RegisterObject((y, x), wall);
                    }
                    // Assign vertical walls on the left and right edges.
                    else if (x == 0 || x == _arraySizeX - 1)
                    {
                        //TODO löschen wenn funktioniert : _gameBoardArray[y, x] = TileTyp.WallVertical;
                        WallInstance wall = new WallInstance(_deps.WallInstanceDeps);
                        wall.Initialize(TileTyp.WallVertical, (y, x));
                        _deps.GameObject.RegisterObject((y, x), wall);
                    }
                    // Everything else is empty space.
                    else
                    {
                        _gameBoardArray[y, x] = TileTyp.Empty;
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
        /// <item><description>Top-Left → <see cref="TileTyp.WallCornerTopLeft"/></description></item>
        /// <item><description>Top-Right → <see cref="TileTyp.WallCornerTopRight"/></description></item>
        /// <item><description>Bottom-Left → <see cref="TileTyp.WallCornerBottomLeft"/></description></item>
        /// <item><description>Bottom-Right → <see cref="TileTyp.WallCornerBottomRight"/></description></item>
        /// </list>
        /// This ensures visual consistency and structural closure of the game board perimeter.
        /// </remarks>
        private void SetCorners()
        {
            UpdateCorner((0, 0), TileTyp.WallCornerTopLeft);
            UpdateCorner((0, _arraySizeX - 1), TileTyp.WallCornerTopRight);
            UpdateCorner((_arraySizeY - 1, 0), TileTyp.WallCornerBottomLeft);
            UpdateCorner((_arraySizeY - 1, _arraySizeX - 1), TileTyp.WallCornerBottomRight);
        }

        /// <summary>
        /// Updates a specific board corner tile with the corresponding wall corner type.
        /// </summary>
        /// <remarks>
        /// This method verifies that the <see cref="_gameBoardArray"/> is initialized and that 
        /// a <see cref="WallInstance"/> exists at the specified position.  
        /// If found, it re-initializes the wall with the given corner type and updates the tile entry.  
        /// If no valid wall is present, an error is logged to the diagnostics system.
        /// </remarks>
        /// <param name="position">
        /// The Y/X grid position of the corner tile to update.
        /// </param>
        /// <param name="newType">
        /// The <see cref="TileTyp"/> that represents the new wall corner type to assign.
        /// </param>
        private void UpdateCorner((int y, int x) position, TileTyp newType)
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(UpdateCorner)}: GameBoardArray is null!");
                return;
            }
            if (_deps.GameObject.TryGetObject(position, out object? obj) && obj is WallInstance wall)
            {
                wall.Initialize(newType, position);
                SetTile(position, newType);
                //TODO löschen wenn funktioniert : _gameBoardArray[position.y, position.x] = newType;
                _deps.Diagnostic.AddCheck($"{nameof(GameBoardManager)}: Updated existing wall at {position} to {newType}.");
            }
            else
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}: No existing wall found at {position} to convert to corner.");
            }
        }

        /// <summary>
        /// Assigns a specific <see cref="TileTyp"/> to a given board position.
        /// </summary>
        /// <param name="position">The Y/X grid position of the target tile.</param>
        /// <param name="tileTyp">The new tile type to assign.</param>
        public void SetTile((int y, int x) position, TileTyp tileTyp)
        {
            if (_gameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(SetTile)}:GameBoardArray is null!");

                return;
            }

            if (!IsInBounds(position))
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameBoardManager)}.{nameof(SetTile)}: Position {position} out of bounds.");
                return;
            }
            _gameBoardArray[position.y, position.x] = tileTyp;
        }

        /// <summary>
        /// Retrieves the tile type from a specific board position.
        /// </summary>
        /// <param name="position">The Y/X grid position of the tile.</param>
        /// <returns>The <see cref="TileTyp"/> at the given position.</returns>
        public TileTyp GetTileTyp((int y, int x) position)
        {
            if (_gameBoardArray == null)
            {

                _deps.Diagnostic.AddError($"{nameof(GameBoardManager)}.{nameof(GetTileTyp)}: GameBoardArray is null");

                return TileTyp.None;
            }
            return _gameBoardArray[position.y, position.x];


        }

        /// <summary>
        /// Attempts to retrieve the tile type from a specific position.
        /// </summary>
        /// <param name="position">The target grid position.</param>
        /// <param name="tile">Outputs the found tile type.</param>
        /// <returns><c>true</c> if the position is valid and contains a tile; otherwise, <c>false</c>.</returns>
        public bool TryGetTile((int y, int x) position, out TileTyp tile)
        {
            tile = TileTyp.None;

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
            SetTile(position, TileTyp.Empty);
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
            FillWallTypesToBoard();
        }
    }
}
