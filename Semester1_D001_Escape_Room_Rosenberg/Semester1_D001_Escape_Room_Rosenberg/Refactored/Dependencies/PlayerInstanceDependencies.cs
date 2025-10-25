using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="PlayerInstance"/> class.
    /// Provides access to the diagnostic and symbol management systems used
    /// for initializing and representing the player on the game board.
    /// </summary>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="Diagnostic"/> responsible for logging
    /// checks, warnings, and debug messages related to player initialization and state.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="Symbol"/> that provides the player's
    /// symbolic representation used for rendering and board display.
    /// </param>
    internal sealed record PlayerInstanceDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );

}
