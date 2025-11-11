/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : RulesManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines dependency references required by the RulesManager.
* This record ensures clean dependency injection for diagnostics and board data.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides dependency references for the <see cref="RulesManager"/>.
    /// Includes diagnostics access and the current game board instance.
    /// </summary>
    /// <param name="Diagnostic">Reference to the DiagnosticsManager for logging.</param>
    /// <param name="GameBoard">Reference to the GameBoardManager providing board data.</param>
    internal sealed record RulesManagerDependencies
    (
        DiagnosticsManager Diagnostic,
        GameBoardManager GameBoard
    );
}