using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DiagnosticsManager"></param>
    /// <param name="RandomManager"></param>
    /// <param name="RulesManager"></param>
    /// <param name="GameBoardBuilder"></param>
    /// <param name="SymbolManager"></param>
    /// <param name="NpcDialogData"></param>
    /// <param name="NpcMetaData"></param>
    /// <param name="NpcRewardData"></param>
    internal sealed record SpawnManagerDependencies
    (
        RulesManager RulesManager,
        DiagnosticsManager DiagnosticsManager,
        RandomManager RandomManager,
        GameBoardManager GameBoardManager,
        SymbolsManager SymbolsManager,
        DoorInstanceDependencies DoorInstanceDependencies,
        KeyFragmentInstanceDependencies KeyFragmentInstanceDependencies,
        NpcInstanceDependencies NpcInstanceDependencies,
        PlayerInstanceDependencies PlayerInstanceDependencies,
        WallInstanceDependencies WallInstanceDependencies
    );
}