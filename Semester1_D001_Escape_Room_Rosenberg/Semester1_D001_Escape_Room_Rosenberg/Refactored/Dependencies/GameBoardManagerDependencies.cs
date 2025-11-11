/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : GameBoardManagerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines dependency references required by the GameBoardManager.
* Provides access to the DiagnosticsManager for centralized logging.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Encapsulates all external dependencies required by the <see cref="GameBoardManager"/>.
    /// Enables clean dependency injection for centralized diagnostics logging.
    /// </summary>
    /// <param name="Diagnostics">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for error and event logging.
    /// </param>
    internal sealed record GameBoardManagerDependencies
    (
        DiagnosticsManager Diagnostic
    );
}