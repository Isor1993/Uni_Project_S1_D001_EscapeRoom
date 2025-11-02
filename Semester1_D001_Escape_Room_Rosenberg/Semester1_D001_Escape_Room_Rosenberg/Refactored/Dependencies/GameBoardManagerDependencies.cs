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
/// 
/// </summary>
/// <param name="Diagnostics"></param>
    internal sealed record GameBoardManagerDependencies
    (        
        DiagnosticsManager  Diagnostics             
    );    
}
