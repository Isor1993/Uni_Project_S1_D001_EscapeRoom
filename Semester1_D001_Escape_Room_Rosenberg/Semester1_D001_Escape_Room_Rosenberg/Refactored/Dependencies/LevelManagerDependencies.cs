using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
   
    internal sealed record LevelManagerDependencies
    (
        DiagnosticsManager Diagnostic,
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        SpawnManager Spawn       
    );
}
