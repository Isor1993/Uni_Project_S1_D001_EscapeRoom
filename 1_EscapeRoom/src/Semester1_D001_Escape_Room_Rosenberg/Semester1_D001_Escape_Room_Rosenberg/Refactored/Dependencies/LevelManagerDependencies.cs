/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : LevelManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all dependency references required by the <see cref="LevelManager"/>.
* Connects the level progression system with game state management, diagnostics,
* and object/actor spawning functionality.
*
* Responsibilities:
* - Provide <see cref="LevelManager"/> access to diagnostic logging
* - Enable cleanup of game board and objects for new levels
* - Supply spawning and inventory systems for level transitions
* - Maintain modular separation between gameplay layers
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/


using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines the dependency links required by the <see cref="LevelManager"/>.
    /// Provides access to all subsystems needed for level initialization,
    /// cleanup, inventory adjustments, and diagnostics reporting.
    /// </summary>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used to log
    /// all level initialization steps, cleanup operations, and progression events.
    /// </param>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> handling the board’s structure,
    /// tile data, and reset functionality during level transitions.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObjectManager"/> managing all active
    /// entities and providing cleanup routines for level restarts.
    /// </param>
    /// <param name="Spawn">
    /// Reference to the <see cref="SpawnManager"/> used to clear and
    /// reinitialize all entities when generating new levels.
    /// </param>
    /// <param name="Inventory">
    /// Reference to the <see cref="InventoryManager"/> responsible for
    /// managing key fragments and resetting item data between levels.
    /// </param>
    internal sealed record LevelManagerDependencies
    (
        DiagnosticsManager Diagnostic,
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        SpawnManager Spawn,
        InventoryManager Inventory
    );
}