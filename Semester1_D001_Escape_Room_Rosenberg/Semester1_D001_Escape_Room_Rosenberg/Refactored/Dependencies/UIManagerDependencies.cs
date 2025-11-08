/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : UIManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Dependency record defining all required manager references for the UIManager.
* Provides a structured link between visual console output and core gameplay data.
* 
* Responsibilities:
* - Supplies access to GameBoard dimensions for HUD rendering
* - Enables centralized diagnostic logging
* - Delivers symbol data (icons, walls, hearts, etc.) for HUD drawing
* - Grants access to game object states (player, door, NPC)
* - Provides inventory and score data for top HUD visualization
* - Handles randomized behavior (e.g., text or layout randomization)
* - Gives level progression data for display and feedback
*
* History :
* 09.11.2025 ER Created
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;


namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all dependency links required by <see cref="UIManager"/>.
    /// This record injects references to all core managers that contribute to
    /// rendering, symbol data, diagnostics, and state visualization.
    /// </summary>
    /// <param name="GameBoard">Provides array size, layout, and cursor offsets for HUD rendering.</param>
    /// <param name="Diagnostic">Handles runtime checks, errors, and warnings for output validation.</param>
    /// <param name="Symbol">Contains all visual symbols used in the HUD (walls, keys, hearts, etc.).</param>
    /// <param name="Print">Offers printing utilities and output helpers used by the HUD manager.</param>
    /// <param name="Random">Provides randomization logic, e.g., shuffling or variable layout spacing.</param>
    /// <param name="Inventory">Grants access to score, key fragments, and collected items for display.</param>
    /// <param name="GameObject">Links active game object instances (Player, Door, NPC) to the HUD layer.</param>
    /// <param name="Level">Delivers current level data for display in the top HUD area.</param>
    internal sealed record UIManagerDependencies
    (
        GameBoardManager GameBoard,
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol,
        PrintManager Print,
        RandomManager Random,
        InventoryManager Inventory, 
        GameObjectManager GameObject,
        LevelManager Level        
    );
}