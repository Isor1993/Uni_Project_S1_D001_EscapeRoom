/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : DoorInstanceDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all external system references required by the <see cref="GameBoardObjects.Door.DoorInstance"/>.  
* Ensures dependency injection for symbol configuration and diagnostic logging,  
* maintaining modularity and testability within the game architecture.
*
* Responsibilities:
* - Provide references to the <see cref="SymbolsManager"/> for door symbol configuration
* - Provide references to the <see cref="DiagnosticsManager"/> for runtime logging
* - Ensure <see cref="DoorInstance"/> remains independent from global manager access
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Represents the dependency record required to initialize and manage a <see cref="GameBoardObjects.Door.DoorInstance"/>.
    /// </summary>
    /// <remarks>
    /// Provides modular access to core systems such as symbol configuration and diagnostics.  
    /// This record supports dependency injection to ensure the door instance operates independently  
    /// of global static references, maintaining clean architecture boundaries.
    /// </remarks>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> responsible for door symbol configuration  
    /// (open/closed, vertical/horizontal variations).
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging initialization,  
    /// symbol assignment, and door state changes.
    /// </param>
    internal sealed record DoorInstanceDependencies
    (
        SymbolsManager Symbol,
        DiagnosticsManager Diagnostic
    );
}