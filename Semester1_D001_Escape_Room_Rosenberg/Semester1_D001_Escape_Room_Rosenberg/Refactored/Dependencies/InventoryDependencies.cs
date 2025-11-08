/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : InventoryDependencies.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines the single dependency reference required by the <see cref="InventoryManager"/>.
* Connects the inventory tracking system to the diagnostics subsystem for
* runtime validation and event logging.
*
* Responsibilities:
* - Provide <see cref="InventoryManager"/> access to diagnostics logging
* - Ensure consistent tracking of all inventory changes
* - Maintain modular separation between logic and monitoring
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using System;
namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines the dependency link required by the <see cref="InventoryManager"/>.
    /// Provides access to the diagnostics subsystem for logging all inventory
    /// operations such as additions, removals, and resets.
    /// </summary>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for
    /// recording all inventory-related events and ensuring consistent
    /// runtime traceability.
    /// </param>
    internal sealed record InventoryDependencies
    (
        DiagnosticsManager Diagnostic
    );
}