using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides all required dependency references for the <see cref="InventoryManager"/> system.
    /// </summary>
    /// <remarks>
    /// The <see cref="InventoryDependencies"/> record serves as a centralized container  
    /// for dependency injection into the <see cref="InventoryManager"/>.  
    /// It ensures that the manager has access to shared diagnostic tools and  
    /// interconnected systems without creating tight coupling between components.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the global <see cref="DiagnosticsManager"/> used for logging, debugging,  
    /// and validation messages within the inventory system.
    /// </param>
    internal sealed record InventoryDependencies
    (
        DiagnosticsManager Diagnostic
    );
}
