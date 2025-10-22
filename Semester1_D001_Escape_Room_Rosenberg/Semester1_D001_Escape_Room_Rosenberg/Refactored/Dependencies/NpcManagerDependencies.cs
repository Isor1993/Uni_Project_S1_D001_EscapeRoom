using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="NpcManager"/> class.
    /// Provides access to the <see cref="PrintManager"/> for console output,
    /// the <see cref="DiagnosticsManager"/> for logging and validation,
    /// and the <see cref="NpcDataLoader"/> for loading NPC-related data.
    /// </summary>
    /// <param name="Printer">
    /// Reference to the <see cref="PrintManager"/> responsible for displaying
    /// NPC-related information and debug output on the console.
    /// </param>
    /// <param name="Diagnostics">
    /// Reference to the <see cref="DiagnosticsManager"/> used for tracking logs,
    /// warnings, and diagnostic checks during NPC operations.
    /// </param>
    /// <param name="NpcDataLoader">
    /// Reference to the <see cref="NpcDataLoader"/> responsible for reading and
    /// providing NPC data such as dialogues, rewards, and metadata.
    /// </param>
    internal sealed record NpcManagerDependencies
    (
        PrintManager Printer,
        DiagnosticsManager Diagnostics,
        NpcDataLoader NpcDataLoader
    );
}
