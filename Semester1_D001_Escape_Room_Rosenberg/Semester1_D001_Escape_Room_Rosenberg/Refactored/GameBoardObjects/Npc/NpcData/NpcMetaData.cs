using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{

    /// <summary>
    /// Represents the metadata of a non-player character (NPC).
    /// Stores the NPC’s name, map position, and visual symbol information.
    /// This data defines the NPC’s presence and placement on the game board.
    /// </summary>
    internal class NpcMetaData
    {
        // === Dependencies ===
        readonly SymbolsManager _symbols;

        // === Fields ===
        private string _name;        
        private char _symbol;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolsManager"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
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
        public (int y, int x) Position { get ; private set; }
        /// <summary>
        /// Gets the visual symbol representing the NPC on the board.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void AssignPosition((int y , int x) position)
        {
            Position= position;
        }
    }
}
