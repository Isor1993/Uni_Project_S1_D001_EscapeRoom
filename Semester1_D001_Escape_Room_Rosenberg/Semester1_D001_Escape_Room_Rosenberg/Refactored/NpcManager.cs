using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored
{
    /// <summary>
    /// Handles the management of all NPC (Non-Player Character) data within the game.
    /// Responsible for loading NPCs from an external data file and storing their dialogue,
    /// rewards, and related attributes for in-game interaction.
    internal class NpcManager
    {
        // Reference to the PrinterManager, used for console or UI output.
        private readonly PrinterManager _printer;
        // Reference to the DiagnosticsManager, used for logging errors, warnings, and checks.
        private readonly DiagnosticsManager _diagnostics;
        // Reference to the NpcDataLoader, responsible for reading NPC data from external files.
        private readonly NpcDataLoader _npcDataLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcManager"/> class with required dependencies.
        /// </summary>
        /// <param name="printerManager">The PrinterManager instance responsible for displaying output.</param>
        /// <param name="diagnosticsManager">The DiagnosticsManager instance for logging and diagnostics.</param>
        /// <param name="npcDataLoader">The NpcDataLoader used to load NPC data from text files.</param>
        public NpcManager(PrinterManager printerManager, DiagnosticsManager diagnosticsManager, NpcDataLoader npcDataLoader)
        {
            _printer = printerManager;
            _diagnostics = diagnosticsManager;
            _npcDataLoader = npcDataLoader;
        }
        /// <summary>
        /// Internal list that stores all loaded NPC instances.
        /// </summary>
        private List<NpcInstance> _npcList = new List<NpcInstance>();
        /// <summary>
        /// Gets the list of all loaded NPC instances.
        /// Each instance contains its dialogue, quest information, and reward data.
        /// </summary>
        public List<NpcInstance> NpcList => _npcList;

        /// <summary>
        /// Loads all NPC data from the specified file path and stores it in memory.
        /// </summary>
        /// <param name="filePath">The path to the NPC data file.</param>
        public void LoadAllNpcData(string filePath)
        {
            _npcList = _npcDataLoader.LoadNpcDataFromFile(filePath);
            _diagnostics.AddCheck($"{nameof(NpcManager)}: Successfully loaded {_npcList.Count} NPC records.");
        }

    }
}


