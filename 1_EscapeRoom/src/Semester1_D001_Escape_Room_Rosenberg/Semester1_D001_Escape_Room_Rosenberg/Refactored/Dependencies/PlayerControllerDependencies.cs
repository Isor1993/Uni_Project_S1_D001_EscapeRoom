/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : PlayerControllerDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines all required dependency references for the <see cref="PlayerController"/>.
* Links the player input and movement logic with core gameplay systems such as
* the game board, rule validation, interactions, rendering, and diagnostics.
*
* Responsibilities:
* - Provide the <see cref="PlayerController"/> with access to all relevant managers
* - Connect input handling with movement validation and visual output
* - Ensure modular communication without direct cross-references between systems
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines the dependency links required by the <see cref="PlayerController"/>.
    /// Provides all connected systems that the controller interacts with,
    /// ensuring proper separation of concerns and centralized access.
    /// </summary>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/>, used for accessing
    /// the board array, dimensions, and validating tile positions.
    /// </param>
    /// <param name="Rule">
    /// Reference to the <see cref="RulesManager"/>, responsible for validating
    /// whether the player can move to a given position.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging
    /// all player actions, movement events, and potential errors.
    /// </param>
    /// <param name="Interaction">
    /// Reference to the <see cref="InteractionManager"/>, handling interactions
    /// with NPCs, doors, keys, and other interactive objects.
    /// </param>
    /// <param name="Print">
    /// Reference to the <see cref="PrintManager"/> responsible for rendering
    /// player movement and updates on the console.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing visual symbols
    /// for the player and environment during console rendering.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObjectManager"/> managing all
    /// active instances (Player, NPCs, Keys, Walls, etc.) within the game world.
    /// </param>
    internal sealed record PlayerControllerDependencies
    (
        GameBoardManager GameBoard,
        RulesManager Rule,
        DiagnosticsManager Diagnostic,
        InteractionManager Interaction,
        PrintManager Print,
        SymbolsManager Symbol,
        GameObjectManager GameObject
    );
}