using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
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
    /// Defines the available tile types used on the game board.
    /// Each type represents a different element or object placed on the board.
    /// </summary>
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
    /// Responsible for creating and managing the structure of the game board.
    /// Handles board initialization, dimension selection, and wall placement logic.
    /// </summary>
    internal class GameBoardManager
    {
        // === Dependencies ===
        private readonly GameBoardBuilderDependencies _gameBoardBuilderDeps;

        // === Fields ===
        private int _arraySizeY;
        private int _arraySizeX;
        private TileTyp[,]? _gameBoardArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBoardManager"/> class.
        /// Sets up all required dependencies for board creation, printing, and diagnostics.
        /// </summary>
        /// <param name="gameBoardBuilderDependencies">
        /// Reference to the <see cref="GameBoardBuilderDependencies"/> object that provides
        /// the necessary managers for board rendering and diagnostic logging.
        /// </param>
        public GameBoardManager(GameBoardBuilderDependencies gameBoardBuilderDependencies)
        {
            _gameBoardBuilderDeps= gameBoardBuilderDependencies;
        }
        
        /// <summary>
        /// Gets the two-dimensional array that represents the game board.
        /// </summary>
        public TileTyp[,]? GameBoardArray => _gameBoardArray;

        /// <summary>
        /// Gets the width (X dimension) of the game board.
        /// </summary>
        public int ArraySizeX => _arraySizeX;

        /// <summary>
        /// Gets the height (Y dimension) of the game board.
        /// </summary>
        public int ArraySizeY => _arraySizeY;

        /// <summary>
        /// Prompts the player to choose the board dimensions and initializes the game board array.
        /// Logs a diagnostic warning if the board is recreated.
        /// </summary>
        public void DecideArraySize()
        {
            // If a previous board exists, warn that it will be overwritten.
            if (_gameBoardArray != null)
            {
                _gameBoardBuilderDeps.DiagnosticsManager.AddWarning($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: GameBoardArray was recreated.");
            }

            // Ask for dimensions within predefined limits.
            _arraySizeX = _gameBoardBuilderDeps.PrintManager.AskForIntInRange("How wide should the game board be?", 30, 120);        
            _arraySizeY = _gameBoardBuilderDeps.PrintManager.AskForIntInRange("How high should the game board be?", 15, 20);

            // Initialize a new 2D array based on user input.
            _gameBoardArray = new TileTyp[_arraySizeY, _arraySizeX];
            _gameBoardBuilderDeps.DiagnosticsManager.AddCheck($"{nameof(GameBoardManager)}.{nameof(DecideArraySize)}: Board successfully initialized ({_arraySizeX}x{_arraySizeY}).");
        }
        /// <summary>
        /// Fills the game board array with tile data.
        /// Wall tiles are placed around the borders, while the inner area is left empty.
        /// </summary>
        public void FillWallTypesToBoard()
        {           
            // Check if the board exists and recreate before trying to fill it.
            if (_gameBoardArray == null)
            {
                _gameBoardBuilderDeps.DiagnosticsManager.AddError($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Cannot fill walls. Board has not been initialized.");
                _gameBoardArray = new TileTyp[_arraySizeY, _arraySizeX];
            }

            // Optional: warn if it’s already filled with data (not empty anymore)
            if (_gameBoardArray[0, 0] != TileTyp.None)
            {
                _gameBoardBuilderDeps.DiagnosticsManager.AddWarning($"{nameof(GameBoardManager)}.{nameof(FillWallTypesToBoard)}: Existing board data will be overwritten.");
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
                        _gameBoardArray[y, x] = TileTyp.WallHorizontal;
                    }
                    // Assign vertical walls on the left and right edges.
                    else if (x == 0 || x == _arraySizeX - 1)
                    {
                        _gameBoardArray[y, x] = TileTyp.WallVertical;
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
        /// Assigns corner tiles to the four corners of the game board array.
        /// Each corner is initialized with its corresponding <see cref="TileTyp"/> value:
        /// top-left, top-right, bottom-left, and bottom-right.
        /// Logs a diagnostic message if the game board array is not initialized.
        /// </summary>
        private void SetCorners()
        {
            if (_gameBoardArray == null)
            {

                _gameBoardBuilderDeps.DiagnosticsManager.AddCheck($"{nameof(GameBoardManager)}.{nameof(SetCorners)}: ");

                return;
            }
            _gameBoardArray[0, 0] = TileTyp.WallCornerTopLeft;
            _gameBoardArray[_arraySizeY - 1, _arraySizeX - 1] = TileTyp.WallCornerBottomRight;
            _gameBoardArray[0, _arraySizeX - 1] = TileTyp.WallCornerTopRight;
            _gameBoardArray[_arraySizeY - 1, 0] = TileTyp.WallCornerBottomLeft;
        }
    }
}
