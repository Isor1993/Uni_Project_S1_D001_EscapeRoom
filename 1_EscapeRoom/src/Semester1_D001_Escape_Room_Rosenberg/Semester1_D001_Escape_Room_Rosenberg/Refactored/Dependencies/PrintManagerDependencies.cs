/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : PrintManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all dependency references required by the <see cref="PrintManager"/>.
* Connects the console output system with core gameplay data such as
* board structure, object positions, symbols, and diagnostics.
*
* Responsibilities:
* - Provides access to the game board array and dimensions
* - Grants access to registered game objects for visual rendering
* - Supplies all visual symbols (player, NPCs, walls, empty tiles, etc.)
* - Enables diagnostic logging for print and rendering operations
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines the dependency links required by the <see cref="PrintManager"/>.
    /// This record provides the essential managers for rendering the game board,
    /// accessing game object instances, and handling output diagnostics.
    /// </summary>
    /// <param name="GameBoard">
    /// Provides access to the <see cref="GameBoardManager"/>, including
    /// the board layout, tile types, and size definitions.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObjectManager"/> containing all
    /// active game entities (player, NPCs, keys, walls, etc.).
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing all visual
    /// characters used during board rendering (icons, walls, hearts, empty space, etc.).
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used to log print checks,
    /// warnings, and rendering exceptions during runtime.
    /// </param>
    internal sealed record PrintManagerDependencies
    (
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        SymbolsManager Symbol,
        DiagnosticsManager Diagnostic
    );
}