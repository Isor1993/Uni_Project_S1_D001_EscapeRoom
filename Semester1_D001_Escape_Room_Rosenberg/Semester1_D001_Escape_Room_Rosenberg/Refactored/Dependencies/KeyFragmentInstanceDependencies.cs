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
    /// <param name="SymbolsManager"></param>
    /// <param name="DiagnosticsManager"></param>
    internal sealed record KeyFragmentInstanceDependencies
    (
        SymbolsManager SymbolsManager,
        DiagnosticsManager DiagnosticsManager
    );    
}
