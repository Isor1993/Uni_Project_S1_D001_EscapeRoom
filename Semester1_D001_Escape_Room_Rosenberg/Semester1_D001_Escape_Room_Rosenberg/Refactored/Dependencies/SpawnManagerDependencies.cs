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
    /// Defines all required dependencies for the <see cref="SpawnManager"/> class.
    /// Provides centralized access to all core managers, instance dependencies,
    /// and helper components involved in the spawning process of the game board.
    /// </summary>
    /// <param name="RulesManager">
    /// Reference to the <see cref="RulesManager"/> responsible for validating spawn rules
    /// and checking positional availability on the board.
    /// </param>
    /// <param name="DiagnosticsManager">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging checks,
    /// warnings, and error messages during spawn operations.
    /// </param>
    /// <param name="RandomManager">
    /// Reference to the <see cref="RandomManager"/> responsible for generating random positions
    /// and random element selections during spawning.
    /// </param>
    /// <param name="GameBoardManager">
    /// Reference to the <see cref="GameBoardManager"/> handling board-related updates and data access.
    /// </param>
    /// <param name="SymbolsManager">
    /// Reference to the <see cref="SymbolsManager"/> that provides all visual and symbolic representations
    /// for tiles, NPCs, and items.
    /// </param>
    /// <param name="DoorInstanceDependencies">
    /// Reference to the <see cref="DoorInstanceDependencies"/> that defines the managers and data
    /// required for initializing and spawning door instances.
    /// </param>
    /// <param name="KeyFragmentInstanceDependencies">
    /// Reference to the <see cref="KeyFragmentInstanceDependencies"/> providing managers and symbols
    /// used when spawning key fragment objects.
    /// </param>
    /// <param name="NpcInstanceDependencies">
    /// Reference to the <see cref="NpcInstanceDependencies"/> used for setting up and initializing NPC instances.
    /// </param>
    /// <param name="PlayerInstanceDependencies">
    /// Reference to the <see cref="PlayerInstanceDependencies"/> providing managers and configuration data
    /// for player initialization and spawning.
    /// </param>
    /// <param name="WallInstanceDependencies">
    /// Reference to the <see cref="WallInstanceDependencies"/> that defines dependencies used for
    /// building and managing wall elements.
    /// </param>
    /// <param name="NpcManager">
    /// Reference to the <see cref="NpcManager"/> responsible for controlling and managing all NPC-related logic.
    /// </param>
    /// <param name="GameObjectManager">
    /// Reference to the <see cref="GameObjectManager"/> used for tracking, registering,
    /// and retrieving objects placed on the board.
    /// </param>
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
        WallInstanceDependencies WallInstanceDependencies,
        NpcManager NpcManager,
        GameObjectManager GameObjectManager
    );
}