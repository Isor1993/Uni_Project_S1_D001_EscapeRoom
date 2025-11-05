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
    /// Provides all required dependencies for the <see cref="InteractionManager"/> system.
    /// </summary>
    /// <remarks>
    /// The <see cref="InteractionManagerDependencies"/> record functions as an injection container  
    /// for all external systems required to process interactions such as NPC dialogues,  
    /// key fragment collection, and door unlocking.  
    /// It maintains clean separation between logic, rendering, and game state management  
    /// while ensuring the <see cref="InteractionManager"/> can access every necessary subsystem.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used to log interactions, warnings, and errors.
    /// </param>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> providing the board layout, tile data,  
    /// and access to specific tile types for validation and lookup.
    /// </param>
    /// <param name="GameObject">
    /// Reference to the <see cref="GameObjectManager"/> responsible for managing and retrieving  
    /// interactable objects on the board, such as NPCs, keys, and doors.
    /// </param>
    /// <param name="Rule">
    /// Reference to the <see cref="RulesManager"/> defining movement and interaction restrictions.
    /// </param>
    /// <param name="Inventory">
    /// Reference to the <see cref="InventoryManager"/> providing access to the player’s collected  
    /// key fragments and score, updated during interactions.
    /// </param>
    /// <param name="UI">
    /// Reference to the <see cref="UIManager"/> controlling the Heads-Up Display (HUD) and dialogue windows.
    /// </param>
    /// <param name="Npc">
    /// Reference to the <see cref="NpcManager"/> managing all NPCs, including their metadata, dialogue, and rewards.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> defining visual symbols (e.g., walls, keys, hearts)  
    /// used within HUD rendering and printed messages.
    /// </param>
    /// <param name="Level">
    /// Reference to the <see cref="LevelManager"/> used to track and progress through levels,  
    /// including door-opening logic and win conditions.
    /// </param>
    /// <param name="Door">
    /// Reference to the <see cref="DoorInstance"/> representing the door entity on the board,  
    /// used for open/close operations and level completion.    
    /// </param>
    /// <param name="Print">
    /// Reference to the <see cref="PrintManager"/> responsible for visual output to the console display.
    /// </param>
    /// <param name="Player">
    /// Reference to the <see cref="PlayerInstance"/> providing the player’s current position, lives,  
    /// and symbol data required for interactions and HUD updates.
    /// </param>
    internal sealed record InteractionManagerDependencies
    (
        DiagnosticsManager Diagnostic,
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        RulesManager Rule,
        InventoryManager Inventory,
        UIManager UI,
        NpcManager Npc,
        SymbolsManager Symbol,
        LevelManager Level,               
        PrintManager Print,
        RandomManager Random

        
    );
}
