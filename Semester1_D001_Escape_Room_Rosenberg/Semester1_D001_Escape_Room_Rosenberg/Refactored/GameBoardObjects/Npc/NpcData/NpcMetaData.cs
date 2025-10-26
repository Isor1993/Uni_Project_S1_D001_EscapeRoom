using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{

    /// <summary>
    /// Represents the metadata of a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// Stores the NPC’s name, board position, and visual symbol used for rendering.  
    /// This data defines the NPC’s identity and placement on the game board.  
    /// It does not include dialogue or reward logic — those are handled separately 
    /// by <see cref="NpcDialogData"/> and <see cref="NpcRewardData"/>.
    /// </remarks>
    internal class NpcMetaData
    {
        // === Dependencies ===
        private readonly SymbolsManager _symbols;

        // === Fields ===
        private string _name;
        private char _symbol;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcMetaData"/> class.
        /// </summary>
        /// <param name="symbolsManager">Reference to the <see cref="SymbolsManager"/> used for retrieving NPC symbols.</param>
        /// <param name="name">The name of the NPC.</param>
        /// <param name="position">The (Y, X) grid coordinates representing the NPC’s position on the board.</param>
        /// <remarks>
        /// When initialized, the NPC’s symbol is automatically assigned from <see cref="SymbolsManager.QuestSymbol"/>.
        /// </remarks>
        public NpcMetaData(SymbolsManager symbolsManager, string name, (int y, int x) position)
        {
            _symbols = symbolsManager;
            _name = name;
            Position = position;
            _symbol = _symbols.QuestSymbol;
        }

        /// <summary>
        /// Gets the name of the NPC.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the NPC’s current position on the game board.
        /// </summary>
        public (int y, int x) Position { get; private set; }

        /// <summary>
        /// Gets the visual symbol representing the NPC on the board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Updates the NPC’s position on the game board.
        /// </summary>
        /// <param name="position">The new (Y, X) coordinates to assign.</param>
        /// <remarks>
        /// This method is typically called during spawning, movement, or data reloading.
        /// </remarks>
        public void AssignPosition((int y, int x) position)
        {
            Position = position;
        }
    }
}
