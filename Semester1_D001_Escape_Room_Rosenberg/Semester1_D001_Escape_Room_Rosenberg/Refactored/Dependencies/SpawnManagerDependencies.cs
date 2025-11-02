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
    /// Provides all required dependencies for the <see cref="SpawnManager"/>.
    /// </summary>
    /// <remarks>
    /// This record acts as a dependency injection container for all systems  
    /// that the <see cref="SpawnManager"/> requires to perform object placement.  
    /// It includes references to rule validation, diagnostics logging, random generation,  
    /// board management, and all object-specific instance dependencies.
    /// </remarks>
    /// <param name="Rule">Reference to the <see cref="RulesManager"/> used for spawn validation.</param>
    /// <param name="Diagnostic">Reference to the <see cref="DiagnosticsManager"/> used for event logging.</param>
    /// <param name="Random">Reference to the <see cref="RandomManager"/> used for position randomization.</param>
    /// <param name="GameBoard">Reference to the <see cref="GameBoardManager"/> managing board layout and size.</param>
    /// <param name="Symbol">Reference to the <see cref="SymbolsManager"/> providing all object symbols.</param>
    /// <param name="DoorInstanceDeps">Dependencies for creating and initializing <see cref="DoorInstanceDeps"/> objects.</param>
    /// <param name="KeyFragmentInstancedeps">Dependencies for creating and initializing <see cref="KeyFragmentInstancedeps"/> objects.</param>
    /// <param name="NpcInstanceDeps">Dependencies for creating and initializing <see cref="NpcInstanceDeps"/> objects.</param>
    /// <param name="PlayerInstanceDeps">Dependencies for creating and initializing <see cref="PlayerInstanceDeps"/> objects.</param>
    /// <param name="WallInstance">Dependencies for creating and initializing <see cref="WallInstance"/> objects.</param>
    /// <param name="Npc">Reference to the <see cref="NpcManager"/> containing all available NPCs for spawning.</param>
    /// <param name="GameObject">Reference to the <see cref="GameObjectManager"/> used to register spawned objects on the board.</param>
    internal sealed record SpawnManagerDependencies
    (
        RulesManager Rule,
        DiagnosticsManager Diagnostic,
        RandomManager Random,
        GameBoardManager GameBoard,
        SymbolsManager Symbol,
        DoorInstanceDependencies DoorInstanceDeps,
        KeyFragmentInstanceDependencies KeyFragmentInstancedeps,
        NpcInstanceDependencies NpcInstanceDeps,
        PlayerInstanceDependencies PlayerInstanceDeps,
        WallInstanceDependencies WallInstanceDeps,
        NpcManager Npc,
        GameObjectManager GameObject
    );
}