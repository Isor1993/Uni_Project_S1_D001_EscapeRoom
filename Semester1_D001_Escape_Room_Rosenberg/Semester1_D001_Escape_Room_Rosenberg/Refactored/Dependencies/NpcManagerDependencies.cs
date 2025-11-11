/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all dependency references required by the <see cref="NpcManager"/>.
* Connects the NPC management system with data loading, symbol rendering,
* and diagnostics functionality for full runtime traceability.
*
* Responsibilities:
* - Provide <see cref="NpcManager"/> with access to NPC data loading
* - Supply symbol definitions for visual NPC representation
* - Enable diagnostic logging for load, instantiation, and lookup events
* - Maintain modular communication between game systems
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines the dependency links required by the <see cref="NpcManager"/>.
    /// Provides access to all subsystems responsible for loading, symbol rendering,
    /// and diagnostic monitoring of NPC operations.
    /// </summary>
    /// <param name="NpcDataLoader">
    /// Reference to the <see cref="NpcDataLoader"/> used to read and parse
    /// external NPC configuration files containing dialogue, metadata, and rewards.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging
    /// all NPC load operations, instantiations, and lookup results.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing the visual symbols
    /// used to represent NPCs in the game world during rendering.
    /// </param>
    internal sealed record NpcManagerDependencies
    (
        NpcDataLoader NpcDataLoader,
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}