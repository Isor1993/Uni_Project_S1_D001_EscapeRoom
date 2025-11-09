/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcInstanceDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines the dependency references required by the <see cref="NpcInstance"/>.
* Connects each NPC entity with the diagnostics and symbol rendering systems
* for event logging and visual board representation.
*
* Responsibilities:
* - Provide <see cref="NpcInstance"/> access to diagnostics for runtime logging
* - Supply symbol definitions for consistent NPC visualization
* - Maintain modular separation between NPC logic and global systems
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;


namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{

    /// <summary>
    /// Defines the dependency links required by the <see cref="NpcInstance"/>.
    /// Provides access to core systems responsible for logging NPC actions
    /// and rendering NPC symbols on the game board.
    /// </summary>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging
    /// all NPC-related operations, including activation, deactivation,
    /// and dialogue interactions.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> used to retrieve
    /// graphical symbols representing NPCs within the board display.
    /// </param>
    internal sealed record NpcInstanceDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}