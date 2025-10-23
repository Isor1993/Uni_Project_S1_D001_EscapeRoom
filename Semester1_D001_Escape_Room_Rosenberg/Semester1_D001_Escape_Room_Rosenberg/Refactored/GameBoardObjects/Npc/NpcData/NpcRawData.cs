using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="NpcMetaData"></param>
    /// <param name="NpcDialogData"></param>
    /// <param name="NpcRewardData"></param>
    internal sealed record NpcRawData
    (
        NpcMetaData NpcMetaData,
        NpcDialogData NpcDialogData,
        NpcRewardData NpcRewardData
    );
    
}
