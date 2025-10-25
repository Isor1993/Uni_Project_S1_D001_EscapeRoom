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
    /// <param name="Rule">
    /// Reference to the <see cref="Rule"/> responsible for validating spawn rules
    /// and checking positional availability on the board.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="Diagnostic"/> used for logging checks,
    /// warnings, and error messages during spawn operations.
    /// </param>
    /// <param name="Random">
    /// Reference to the <see cref="Random"/> responsible for generating random positions
    /// and random element selections during spawning.
    /// </param>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoard"/> handling board-related updates and data access.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="Symbol"/> that provides all visual and symbolic representations
    /// for tiles, NPCs, and items.
    /// </param>
    /// <param name="DoorInstance">
    /// Reference to the <see cref="DoorInstance"/> that defines the managers and data
    /// required for initializing and spawning door instances.
    /// </param>
    /// <param name="KeyFragmentInstance">
    /// Reference to the <see cref="KeyFragmentInstance"/> providing managers and symbols
    /// used when spawning key fragment objects.
    /// </param>
    /// <param name="NpcInstance">
    /// Reference to the <see cref="NpcInstance"/> used for setting up and initializing NPC instances.
    /// </param>
    /// <param name="PlayerInstance">
    /// Reference to the <see cref="PlayerInstance"/> providing managers and configuration data
    /// for player initialization and spawning.
    /// </param>
    /// <param name="WallInstance">
    /// Reference to the <see cref="WallInstance"/> that defines dependencies used for
    /// building and managing wall elements.
    /// </param>
    /// <param name="Npc">
    /// Reference to the <see cref="Npc"/> responsible for controlling and managing all NPC-related logic.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObject"/> used for tracking, registering,
    /// and retrieving objects placed on the board.
    /// </param>
    internal sealed record SpawnManagerDependencies
    (
        RulesManager Rule,
        DiagnosticsManager Diagnostic,
        RandomManager Random,
        GameBoardManager GameBoard,
        SymbolsManager Symbol,
        DoorInstanceDependencies DoorInstance,
        KeyFragmentInstanceDependencies KeyFragmentInstance,
        NpcInstanceDependencies NpcInstance,
        PlayerInstanceDependencies PlayerInstance,
        WallInstanceDependencies WallInstance,
        NpcManager Npc,
        GameObjectManager GameObject
    );
}