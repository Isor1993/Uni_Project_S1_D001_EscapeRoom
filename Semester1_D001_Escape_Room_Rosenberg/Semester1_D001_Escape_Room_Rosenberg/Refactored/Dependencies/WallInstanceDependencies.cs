/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : WallInstanceDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all external system references required by the <see cref="GameBoardObjects.Wall.WallInstance"/>.  
* Provides modular access to diagnostics and symbol management through dependency injection,  
* ensuring clean architecture and traceable initialization.
*
* Responsibilities:
* - Supply <see cref="DiagnosticsManager"/> for wall initialization and validation logs  
* - Supply <see cref="SymbolsManager"/> for wall symbol configuration  
* - Maintain strict decoupling between wall instances and global managers
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;


namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Represents the dependency record required to initialize and manage a  
    /// <see cref="GameBoardObjects.Wall.WallInstance"/>.
    /// </summary>
    /// <remarks>
    /// Provides access to both the <see cref="DiagnosticsManager"/> and  
    /// the <see cref="SymbolsManager"/> used for visual configuration and runtime logging.  
    /// Enables dependency injection for modular and testable wall initialization.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging  
    /// initialization events, position assignments, and wall symbol validations.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> that provides  
    /// the character symbols used to visually represent different wall types.
    /// </param>
    internal sealed record WallInstanceDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}