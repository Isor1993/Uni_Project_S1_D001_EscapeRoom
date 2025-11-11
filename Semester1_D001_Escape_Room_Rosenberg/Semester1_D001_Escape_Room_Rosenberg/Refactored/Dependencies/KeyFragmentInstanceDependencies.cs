/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : KeyFragmentInstanceDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all external system references required by the <see cref="GameBoardObjects.Key.KeyFragmentInstance"/>.
* Provides symbol configuration and diagnostic logging through dependency injection,
* ensuring modularity and separation of concerns.
*
* Responsibilities:
* - Supply access to <see cref="SymbolsManager"/> for symbol configuration
* - Supply access to <see cref="DiagnosticsManager"/> for runtime validation and logging
* - Support dependency injection for <see cref="GameBoardObjects.Key.KeyFragmentInstance"/>
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Represents the dependency record required to initialize and manage a
    /// <see cref="GameBoardObjects.Key.KeyFragmentInstance"/>.
    /// </summary>
    /// <remarks>
    /// This record provides references to both the <see cref="SymbolsManager"/> and
    /// the <see cref="DiagnosticsManager"/>.
    /// It is injected into each key fragment instance to ensure clean architecture
    /// without direct access to global systems.
    /// </remarks>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> responsible for retrieving
    /// the correct visual symbol representing a key fragment.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging initialization,
    /// symbol assignment, and position tracking during gameplay.
    /// </param>
    internal sealed record KeyFragmentInstanceDependencies
    (
        SymbolsManager Symbol,
        DiagnosticsManager Diagnostic
    );
}