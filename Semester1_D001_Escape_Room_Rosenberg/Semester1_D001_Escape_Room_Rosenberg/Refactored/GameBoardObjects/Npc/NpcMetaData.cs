namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Represents the metadata of a non-player character (NPC).
    /// Contains the NPC's name, map position, and display symbol information.
    /// </summary>
    internal class NpcMetaData
    {
        // === Fields ===
        readonly SymbolsManager _symbolsManager;
        private string _name;
        private (int y, int x) _position;
        private char _symbol;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcMetaData"/> class.
        /// </summary>
        /// <param name="name">The name of the NPC.</param>
        /// <param name="position">The NPC's position on the game board, represented by (y, x) coordinates.</param>
        /// <param name="symbolsManager">The symbol manager responsible for providing NPC-related symbols.</param>
        public NpcMetaData(SymbolsManager symbolsManager, string name, (int y, int x) position)
        {
            this._symbolsManager = symbolsManager;
            this._name = name;
            this._position = position;
            _symbol = _symbolsManager.QuestSymbol;
        }

        /// <summary>
        /// Gets the name of the NPC.
        /// </summary>
        public string Name => _name;
        /// <summary>
        /// Gets the NPC’s current position on the game board.
        /// </summary>
        public (int y, int x) Position => _position;
        /// <summary>
        /// Gets the visual symbol representing the NPC on the board.
        /// </summary>
        public char Symbol => _symbol;
    }
}
