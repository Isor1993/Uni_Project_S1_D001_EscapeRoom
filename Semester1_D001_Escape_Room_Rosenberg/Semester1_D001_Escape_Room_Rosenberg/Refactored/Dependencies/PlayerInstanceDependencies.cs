using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DiagnosticsManager"></param>
    /// <param name="SymbolsManager"></param>
    internal sealed record PlayerInstanceDependencies
    (
        DiagnosticsManager DiagnosticsManager,
        SymbolsManager SymbolsManager
    );

}
