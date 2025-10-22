using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
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
    /// <param name="Meta"></param>
    /// <param name="Dialog"></param>
    /// <param name="Reward"></param>
    internal sealed record NpcInstanceDependencies
    ( 
        DiagnosticsManager DiagnosticsManager,
        NpcMetaData Meta,
        NpcDialogData Dialog,
        NpcRewardData Reward
    );
}
