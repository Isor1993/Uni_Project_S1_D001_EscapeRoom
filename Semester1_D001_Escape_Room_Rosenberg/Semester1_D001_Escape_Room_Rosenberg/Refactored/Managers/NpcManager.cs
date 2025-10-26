using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored
{
    /// <summary>
    /// Manages all non-player characters (NPCs) within the game.
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcManager"/> is responsible for loading, storing, and retrieving 
    /// all <see cref="NpcInstance"/> objects.  
    /// It interacts with the <see cref="NpcDataLoader"/> to read NPC data files 
    /// and instantiate NPCs with their associated metadata, dialogue, and rewards.  
    /// Diagnostics messages are logged for every major step (loading, instantiation, lookup).
    /// </remarks>
    internal class NpcManager
    {
        // === Dependencies ===

        private readonly NpcManagerDependencies _deps;

        // === Fields ===

        private List<NpcInstance> _npcList = new List<NpcInstance>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcManager"/> class.
        /// </summary>
        /// <param name="npcManagerDependencies">
        /// Provides references to all required dependencies for managing NPC data and instantiation, 
        /// including <see cref="NpcDataLoader"/>, <see cref="DiagnosticsManager"/>, and <see cref="SymbolsManager"/>.
        /// </param>
        public NpcManager(NpcManagerDependencies npcManagerDependencies)
        {
            _deps = npcManagerDependencies;
        }

        /// <summary>
        /// Gets a list containing all currently loaded <see cref="NpcInstance"/> objects.
        /// </summary>
        public List<NpcInstance> NpcList => _npcList;

        /// <summary>
        /// Loads and initializes all NPCs from an external data source.
        /// </summary>
        /// <param name="filePath">The path to the NPC data file to be loaded.</param>
        /// <remarks>
        /// This method clears the current NPC list, then:
        /// <list type="number">
        /// <item><description>Loads raw NPC data from the <see cref="NpcDataLoader"/>.</description></item>
        /// <item><description>Creates a new <see cref="NpcInstance"/> for each dataset.</description></item>
        /// <item><description>Stores all instances in the <see cref="NpcList"/>.</description></item>
        /// </list>
        /// After loading, a diagnostic entry logs the total number of NPCs initialized.
        /// </remarks>
        public void LoadAllNpcData(string filePath)
        {

            List<NpcRawData> rawDataList = _deps.NpcDataLoader.LoadNpcDataFromFile();

            _npcList.Clear();

            foreach (NpcRawData rawData in rawDataList)
            {
                NpcInstanceDependencies npcInstanceDeps = new NpcInstanceDependencies(_deps.Diagnostic, _deps.Symbol);

                NpcInstance npc = new NpcInstance(npcInstanceDeps, rawData.Meta, rawData.Dialog, rawData.Reward);

                _npcList.Add(npc);
            }
            _deps.Diagnostic.AddCheck($"{nameof(NpcManager)}: Loaded {_npcList.Count} NPC instances.");
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
        /// A diagnostic entry is created whether an NPC is found or not, 
        /// to help with debugging board-based interactions.
        /// </remarks>
        public NpcInstance? GetNpcAt((int y, int x) position)
        {
            foreach (var npc in _npcList)
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


