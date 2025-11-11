/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : PlayerInstanceDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all external system references required by the <see cref="GameBoardObjects.Player.PlayerInstance"/>.
* Provides modular access to diagnostics and symbol management through dependency injection,
* ensuring the player instance operates independently from global manager references.
*
* Responsibilities:
* - Supply <see cref="DiagnosticsManager"/> for logging and runtime validation
* - Supply <see cref="SymbolsManager"/> for player symbol configuration
* - Maintain decoupling between the player instance and central management systems
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Represents the dependency record required to initialize and manage a
    /// <see cref="GameBoardObjects.Player.PlayerInstance"/>.
    /// </summary>
    /// <remarks>
    /// Provides structured access to both diagnostics and symbol management systems.
    /// This record is injected into the <see cref="GameBoardObjects.Player.PlayerInstance"/>
    /// to ensure clean architecture and consistent runtime behavior without direct global access.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for tracking
    /// player initialization, state changes, and runtime validations.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing the player’s visual representation
    /// and character configuration used during rendering.
    /// </param>
    internal sealed record PlayerInstanceDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}