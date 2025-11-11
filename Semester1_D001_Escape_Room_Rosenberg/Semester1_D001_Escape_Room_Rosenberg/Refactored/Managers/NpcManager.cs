/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Manages all non-player characters (NPCs) in the game world.
* Handles the loading, instantiation, and lookup of all NPC entities,
* using the <see cref="NpcDataLoader"/> and <see cref="SymbolsManager"/>.
* Logs every major action (loading, creation, lookup) via the DiagnosticsManager.
*
* Responsibilities:
* - Load NPC data through the NpcDataLoader
* - Instantiate NPC objects with dependencies and metadata
* - Provide lookup functionality for NPCs by board position
* - Log all events for debugging and runtime tracking
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored
{
    /// <summary>
    ///Central manager responsible for all non-player character (NPC) operations.
    /// Handles loading from data files, instantiation, and runtime access to NPC entities.
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcManager"/> interacts with the <see cref="NpcDataLoader"/>
    /// to read external configuration files, creates fully initialized
    /// <see cref="NpcInstance"/> objects, and provides lookup utilities
    /// for gameplay interactions.
    /// All operations are logged for transparency and debugging.
    /// </remarks>
    internal class NpcManager
    {
        // === Dependencies ===
        private readonly NpcManagerDependencies _deps;

        // === Fields ===
        private readonly List<NpcInstance> _npcList = new List<NpcInstance>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcManager"/> class.
        /// </summary>
        /// <param name="npcManagerDependencies">
        /// Provides references to the <see cref="NpcDataLoader"/>,
        /// <see cref="DiagnosticsManager"/>, and <see cref="SymbolsManager"/>
        /// required for NPC initialization and logging.
        /// </param>
        public NpcManager(NpcManagerDependencies npcManagerDependencies)
        {
            _deps = npcManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(NpcManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Gets a list containing all currently loaded <see cref="NpcInstance"/> objects.
        /// </summary>
        public List<NpcInstance> NpcList => _npcList;

        /// <summary>
        /// Loads and initializes all NPCs from the external data source.
        /// </summary>
        /// <remarks>
        /// This method:
        /// <list type="number">
        /// <item><description>Clears the current NPC list.</description></item>
        /// <item><description>Loads raw NPC data via the <see cref="NpcDataLoader"/>.</description></item>
        /// <item><description>Instantiates new <see cref="NpcInstance"/> objects with dependencies.</description></item>
        /// <item><description>Stores all initialized NPCs in the <see cref="NpcList"/>.</description></item>
        /// </list>
        /// After loading, a diagnostic entry logs the total number of NPCs created.
        /// </remarks>
        public void LoadAllNpcData()
        {
            List<NpcRawData> rawDataList = _deps.NpcDataLoader.LoadNpcDataFromFile();

            _npcList.Clear();

            foreach (NpcRawData rawData in rawDataList)
            {
                NpcInstanceDependencies npcInstanceDeps = new NpcInstanceDependencies(_deps.Diagnostic, _deps.Symbol);

                NpcInstance npc = new NpcInstance(npcInstanceDeps, rawData.Meta, rawData.Dialog, rawData.Reward);

                _npcList.Add(npc);
            }
            _deps.Diagnostic.AddCheck($"{nameof(NpcManager)}.{nameof(LoadAllNpcData)}: Loaded {_npcList.Count} NPC instances.");
        }

        /// <summary>
        /// Retrieves an NPC instance located at the specified board position.
        /// </summary>
        /// <param name="position">The target (Y, X) coordinates on the game board.</param>
        /// <returns>
        /// The <see cref="NpcInstance"/> found at the specified position,
        /// or <c>null</c> if no NPC exists at that location.
        /// </returns>
        /// <remarks>
        /// A diagnostic log entry is always created to track lookups for debugging purposes.
        /// </remarks>
        public NpcInstance? GetNpcAt((int y, int x) position)
        {
            foreach (NpcInstance npc in _npcList)
            {
                if (npc.Meta.Position == position)
                {
                    _deps.Diagnostic.AddCheck($"{nameof(NpcManager)}.{nameof(GetNpcAt)}: Got Npc {npc.Meta.Name} at position {npc.Meta.Position} from NPC instances.");
                    return npc;
                }
            }

            _deps.Diagnostic.AddCheck($"{nameof(NpcManager)}.{nameof(GetNpcAt)}: No NPC found at position {position}.");
            return null;
        }
    }
}