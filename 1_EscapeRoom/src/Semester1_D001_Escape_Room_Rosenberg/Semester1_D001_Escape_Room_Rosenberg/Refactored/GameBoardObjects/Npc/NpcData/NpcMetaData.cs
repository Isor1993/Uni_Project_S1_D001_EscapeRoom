/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcMetaData.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines the metadata of a non-player character (NPC), including its name,
* position on the board, and associated visual symbol.
* Serves as the foundational identity component for all NPC instances.
*
* Responsibilities:
* - Store and manage NPC name and position data
* - Provide access to the symbol used for board rendering
* - Maintain independence from dialogue and reward logic
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents the metadata of a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcMetaData"/> class contains static identity data for an NPC,
    /// including its name, grid position, and the visual symbol used for rendering
    /// on the game board.
    /// It does not handle any dialogue or reward logic — those are defined separately
    /// in <see cref="NpcDialogData"/> and <see cref="NpcRewardData"/>.
    /// </remarks>
    internal class NpcMetaData
    {
        // === Dependencies ===
        private readonly SymbolsManager _symbols;

        // === Fields ===
        private string _name;

        private char _symbol;

        /// <summary>
        ///Initializes a new instance of the <see cref="NpcMetaData"/> class.
        /// </summary>
        /// <param name="symbolsManager">
        /// Reference to the <see cref="SymbolsManager"/> used to assign the NPC’s visual symbol.
        /// </param>
        /// <param name="name">The display name of the NPC.</param>
        /// <param name="position">
        /// The (Y, X) grid coordinates representing the NPC’s initial position on the board.
        /// </param>
        /// <remarks>
        /// Upon initialization, the NPC symbol is automatically assigned from
        /// <see cref="SymbolsManager.QuestSymbol"/> for consistent visual representation.
        /// </remarks>
        public NpcMetaData(SymbolsManager symbolsManager, string name, (int y, int x) position)
        {
            _symbols = symbolsManager;
            _name = name;
            Position = position;
            _symbol = _symbols.QuestSymbol;
        }

        /// <summary>
        /// Gets the display name of the NPC.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the current board position of the NPC.
        /// </summary>
        public (int y, int x) Position { get; private set; }

        /// <summary>
        /// Gets the visual symbol representing the NPC on the game board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Updates the NPC’s position on the game board.
        /// </summary>
        /// <param name="position">The new (Y, X) coordinates to assign.</param>
        /// <remarks>
        /// Typically called during spawning, board resets, or level transitions
        /// to synchronize logical and visual positioning.
        /// </remarks>
        public void AssignPosition((int y, int x) position)
        {
            Position = position;
        }
    }
}