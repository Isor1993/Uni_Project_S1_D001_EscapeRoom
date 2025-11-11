/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : InteractionManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all dependency references required by the <see cref="InteractionManager"/>.
* Connects the interaction system with all relevant game subsystems such as
* the UI, inventory, NPC, rules, and diagnostics for coordinated gameplay handling.
*
* Responsibilities:
* - Provide access to all managers needed for interaction processing
* - Maintain modular separation between logic, UI, and diagnostics
* - Ensure safe cross-system communication and runtime traceability
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines the dependency links required by the <see cref="InteractionManager"/>.
    /// Provides unified access to core game managers responsible for game logic,
    /// rendering, diagnostics, and progression handling during player interactions.
    /// </summary>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging all
    /// interaction events, warnings, and errors.
    /// </param>
    /// <param name="GameBoard">
    /// Provides access to the <see cref="GameBoardManager"/> that stores and manages
    /// all tile types and board state information.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObjectManager"/> responsible for object lookups,
    /// removals, and updates on the board.
    /// </param>
    /// <param name="Rule">
    /// Reference to the <see cref="RulesManager"/> that validates player actions
    /// and enforces gameplay constraints.
    /// </param>
    /// <param name="Inventory">
    /// Reference to the <see cref="InventoryManager"/> used to update player inventory
    /// and resource counts during interactions.
    /// </param>
    /// <param name="UI">
    /// Reference to the <see cref="UIManager"/> used to update HUD visuals and
    /// display feedback messages during player actions.
    /// </param>
    /// <param name="Npc">
    /// Reference to the <see cref="NpcManager"/> for retrieving NPC instances and dialogue data.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing graphical symbols
    /// for UI rendering and board updates.
    /// </param>
    /// <param name="Level">
    /// Reference to the <see cref="LevelManager"/> that coordinates level transitions
    /// and progression after successful interactions.
    /// </param>
    /// <param name="Print">
    /// Reference to the <see cref="PrintManager"/> responsible for visual updates
    /// on the console board.
    /// </param>
    /// <param name="Random">
    /// Reference to the <see cref="RandomManager"/> used to randomize NPC answers
    /// or gameplay outcomes.
    /// </param>
    internal sealed record InteractionManagerDependencies
    (
        DiagnosticsManager Diagnostic,
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        RulesManager Rule,
        InventoryManager Inventory,
        UIManager UI,
        NpcManager Npc,
        SymbolsManager Symbol,
        LevelManager Level,
        PrintManager Print,
        RandomManager Random
    );
}