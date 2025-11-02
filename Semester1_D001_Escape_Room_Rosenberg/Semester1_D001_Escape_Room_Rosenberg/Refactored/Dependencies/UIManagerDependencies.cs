using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
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
    /// Provides all required dependencies for the <see cref="UIManager"/> system.
    /// </summary>
    /// <remarks>
    /// The <see cref="UIManagerDependencies"/> record acts as a central injection container for all systems  
    /// involved in rendering and updating the Heads-Up Display (HUD).  
    /// It grants the <see cref="UIManager"/> direct access to visual symbols, player data,  
    /// diagnostic logging, randomization tools, and board-related information  
    /// while maintaining a decoupled and modular architecture.
    /// </remarks>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> that provides board dimensions, layout,  
    /// and coordinate information used for rendering HUD alignment and scaling.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging HUD construction events,  
    /// errors, and runtime consistency checks.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing visual elements such as border walls,  
    /// keys, hearts, and door icons used across the HUD interface.
    /// </param>
    /// <param name="Print">
    /// Reference to the <see cref="PrintManager"/> used for direct console output of HUD content and formatting.
    /// </param>
    /// <param name="Random">
    /// Reference to the <see cref="RandomManager"/> responsible for randomizing NPC dialogue answers  
    /// or other variable HUD elements.
    /// </param>
    /// <param name="Inventory">
    /// Reference to the <see cref="InventoryManager"/> providing access to the player's collected key fragments  
    /// and score points displayed on the HUD.
    /// </param>
    /// <param name="Level">
    /// Reference to the <see cref="LevelManager"/> that provides current level information and progress tracking.
    /// </param>      
    internal sealed record UIManagerDependencies
    (
        GameBoardManager GameBoard,
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol,
        PrintManager Print,
        RandomManager Random,
        InventoryManager Inventory, 
        GameObjectManager GameObject,
        LevelManager Level        
    );
}
