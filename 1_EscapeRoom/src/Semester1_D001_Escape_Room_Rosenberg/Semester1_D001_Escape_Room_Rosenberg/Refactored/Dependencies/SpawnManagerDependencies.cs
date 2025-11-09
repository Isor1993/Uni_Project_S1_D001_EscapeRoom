/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : SpawnManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Dependency record defining all manager and instance references required by 
* the <see cref="SpawnManager"/>. 
* Provides centralized access to systems responsible for board setup, spawning, 
* diagnostics, and runtime validation.
*
* Responsibilities:
* - Supplies all core systems used during the spawn process
* - Links to all instance dependency records for dynamic game objects
* - Ensures unified communication between managers and the spawn logic
* - Supports diagnostics and randomized spawning via dependency injection
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all dependency links required by the <see cref="SpawnManager"/>.
    /// Provides access to core gameplay managers and instance dependency records 
    /// for all spawnable entities (Player, NPC, Door, Wall, KeyFragment).
    /// </summary>
    /// <param name="Rule">
    /// Reference to the <see cref="RulesManager"/> handling position validation, 
    /// spawn restrictions, and environment collision logic.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging 
    /// all runtime messages, warnings, and errors during spawn operations.
    /// </param>
    /// <param name="Random">
    /// Provides access to the <see cref="RandomManager"/>, used for randomized 
    /// position selection, shuffling, and variation in spawn patterns.
    /// </param>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> providing the board array, 
    /// dimensions, and tile accessors for spatial positioning.
    /// </param>
    /// <param name="Symbol">
    /// Links to the <see cref="SymbolsManager"/>, providing tile icons and 
    /// visual elements used during spawn visualization and board setup.
    /// </param>
    /// <param name="DoorInstanceDeps">
    /// Dependency record providing initialization data and logic access 
    /// for <see cref="DoorInstance"/> creation.
    /// </param>
    /// <param name="KeyFragmentInstancedeps">
    /// Dependency record providing initialization data for 
    /// <see cref="KeyFragmentInstance"/> objects, including symbol and state linkage.
    /// </param>
    /// <param name="NpcInstanceDeps">
    /// Dependency record defining initialization and behavior configuration 
    /// for each <see cref="NpcInstance"/>.
    /// </param>
    /// <param name="PlayerInstanceDeps">
    /// Dependency record supplying initialization logic, symbol data, and rule access 
    /// for the <see cref="PlayerInstance"/>.
    /// </param>
    /// <param name="WallInstanceDeps">
    /// Dependency record defining wall creation logic, corner conversion, 
    /// and symbol mapping for <see cref="WallInstance"/>.
    /// </param>
    /// <param name="Npc">
    /// Reference to the <see cref="NpcManager"/> that maintains a list of 
    /// available NPCs and related logic for random selection.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObjectManager"/> managing registration 
    /// and lookup of all active in-game objects during spawn operations.
    /// </param>
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