using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides all required dependencies for constructing and managing the game board.
    /// </summary>
    /// <remarks>
    /// This record is used for dependency injection in the <see cref="GameBoardManager"/>.  
    /// It bundles all necessary subsystems — printing utilities, diagnostics, 
    /// object registration, and wall instance configuration — into one container.
    /// </remarks>
    /// <param name="Print">
    /// Reference to the <see cref="PrintManager"/>, responsible for handling 
    /// user prompts and console output.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/>, used for 
    /// logging checks, warnings, and errors during board generation.
    /// </param>
    /// <param name="GameObject">
    /// Provides access to the <see cref="GameObjectManager"/>, allowing 
    /// dynamic registration and removal of wall, NPC, or item objects.
    /// </param>
    /// <param name="WallInstanceDeps">
    /// Provides the dependency configuration for initializing <see cref="WallInstance"/> objects.
    /// </param>
    internal sealed record GameBoardBuilderDependencies
    (
        PrintManager  Print,
        DiagnosticsManager  Diagnostic,
        GameObjectManager GameObject,
        WallInstanceDependencies WallInstanceDeps        
    );    
}
