using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored
{
    /// <summary>
    /// 
    /// </summary>
    internal class NpcManager
    {

        private readonly NpcManagerDependencies _deps;

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
            _deps = npcManagerDependencies;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<NpcInstance> _npcList = new List<NpcInstance>();
        /// <summary>
        /// 
        /// </summary>
        public List<NpcInstance> NpcList => _npcList;

       /// <summary>
       /// 
       /// </summary>
       /// <param name="filePath"></param>
        public void LoadAllNpcData(string filePath)
        {
            
            List<NpcRawData>rawDataList = _deps.NpcDataLoader.LoadNpcDataFromFile(filePath);

            _npcList = new List<NpcInstance>();
            foreach (NpcRawData rawData in rawDataList)
            {
                NpcInstanceDependencies npcInstanceDeps = new NpcInstanceDependencies(_deps.Diagnostic, rawData.NpcMetaData, rawData.NpcDialogData, rawData.NpcRewardData);

                NpcInstance npcInstance = new NpcInstance(npcInstanceDeps);
                _npcList.Add(npcInstance);
            }
            _deps.Diagnostic.AddCheck($"{nameof(NpcManager)}: Successfully created {_npcList.Count} NPC instances from raw data.");
        }

    }
}


