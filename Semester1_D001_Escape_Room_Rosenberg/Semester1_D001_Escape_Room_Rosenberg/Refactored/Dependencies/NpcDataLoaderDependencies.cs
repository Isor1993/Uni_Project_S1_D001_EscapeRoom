/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcDataLoaderDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines the dependency references required by the <see cref="NpcDataLoader"/>.
* Connects the NPC data loading process with diagnostic logging and symbol
* management for consistent NPC initialization and validation.
*
* Responsibilities:
* - Provide <see cref="NpcDataLoader"/> access to diagnostics for runtime logging
* - Supply symbol references for NPC data verification and visualization
* - Maintain clear separation between data loading logic and global systems
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    ///Defines the dependency links required by the <see cref="NpcDataLoader"/>.
    /// Provides unified access to diagnostic and symbol systems, ensuring
    /// traceable and consistent NPC data parsing and initialization.
    /// </summary>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging
    /// all NPC data loading events, warnings, and errors during file parsing.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> that supplies visual
    /// symbol definitions used when validating or assigning NPC icons on the board.
    /// </param>
    internal sealed record NpcDataLoaderDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}