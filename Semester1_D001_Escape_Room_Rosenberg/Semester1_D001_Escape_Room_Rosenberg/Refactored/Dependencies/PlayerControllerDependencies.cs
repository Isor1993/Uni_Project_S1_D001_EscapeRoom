using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides all required dependencies for the <see cref="PlayerController"/> system.
    /// </summary>
    /// <remarks>
    /// This dependency record bundles all external systems necessary for handling  
    /// player movement, interaction, and validation logic.  
    /// It ensures a clean separation of concerns through dependency injection.
    /// </remarks>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> controlling tile updates and player position tracking.
    /// </param>
    /// <param name="Rule">
    /// Reference to the <see cref="RulesManager"/> used for validating allowed movements on the board.
    /// </param>
    /// <param name="Player">
    /// Reference to the <see cref="PlayerInstance"/> representing the player's data and position.
    /// </param>
    /// <param name="Diagnostics">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging all player-related actions and warnings.
    /// </param>
    /// <param name="Interaction">
    /// Reference to the <see cref="InteractionManager"/> used for processing object interactions on adjacent tiles.
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
