/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : ScreenManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all external system references required by the <see cref="Managers.ScreenManager"/>.  
* Provides access to symbol, inventory, level, and diagnostic systems through dependency injection,  
* ensuring modular, testable, and maintainable UI rendering logic.
*
* Responsibilities:
* - Supply <see cref="SymbolsManager"/> for visual symbol configuration  
* - Supply <see cref="InventoryManager"/> for inventory-related HUD updates  
* - Supply <see cref="LevelManager"/> for level state and progression tracking  
* - Supply <see cref="DiagnosticsManager"/> for UI logging and error validation  
* - Maintain separation between visual logic and core game systems
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Represents the dependency record required to initialize and manage the  
    /// <see cref="Managers.ScreenManager"/>.
    /// </summary>
    /// <remarks>
    /// Provides structured access to core systems responsible for HUD rendering,  
    /// player inventory visualization, level display, and diagnostic tracking.  
    /// Enables dependency injection for clean, modular screen management without  
    /// direct coupling to global instances.
    /// </remarks>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> responsible for defining  
    /// visual symbols used in HUD and screen rendering.
    /// </param>
    /// <param name="Inventory">
    /// Reference to the <see cref="InventoryManager"/> that provides item data  
    /// and player inventory information for display.
    /// </param>
    /// <param name="Level">
    /// Reference to the <see cref="LevelManager"/> managing level state and progression  
    /// data shown on the HUD or other UI sections.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for tracking and logging  
    /// rendering operations, display updates, and system validation.
    /// </param>
    internal sealed record ScreenManagerDependencies
    (
        SymbolsManager Symbol,
        InventoryManager Inventory,
        LevelManager Level,
        DiagnosticsManager Diagnostic
    );
}