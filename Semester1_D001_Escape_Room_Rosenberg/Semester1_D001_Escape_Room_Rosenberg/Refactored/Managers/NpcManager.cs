using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
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

        private readonly NpcManagerDependencies _npcManagerDeps;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcManager"/> class.
        /// Sets up all required dependencies for managing NPC initialization,
        /// data handling, and interactions on the game board.
        /// </summary>
        /// <param name="npcManagerDependencies">
        /// Reference to the <see cref="NpcManagerDependencies"/> object that provides
        /// the necessary managers and configuration data for NPC management and system integration.
        /// </param>
        public NpcManager(NpcManagerDependencies npcManagerDependencies)
        {
            _npcManagerDeps = npcManagerDependencies;
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
            _npcList = _npcManagerDeps.NpcDataLoader.LoadNpcDataFromFile(filePath);
            _npcManagerDeps.Diagnostics.AddCheck($"{nameof(NpcManager)}: Successfully loaded {_npcList.Count} NPC records.");
        }

    }
}


