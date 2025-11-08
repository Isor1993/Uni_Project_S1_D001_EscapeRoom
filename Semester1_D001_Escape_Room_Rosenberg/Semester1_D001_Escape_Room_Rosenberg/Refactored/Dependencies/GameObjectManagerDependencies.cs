/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : GameObjectManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all dependency references required by the <see cref="GameObjectManager"/>.
* Provides access to the GameBoardManager for tile synchronization
* and to the DiagnosticsManager for structured runtime logging.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Encapsulates all external dependencies required by the 
    /// <see cref="GameObjectManager"/>. Enables clean dependency injection for
    /// diagnostics and board interaction.
    /// </summary>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> used for tile updates and board synchronization.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for error,
    /// warning, and state logging.
    /// </param>
    internal sealed record GameObjectManagerDependencies
    (
        GameBoardManager GameBoard,
        DiagnosticsManager Diagnostic
    );
}