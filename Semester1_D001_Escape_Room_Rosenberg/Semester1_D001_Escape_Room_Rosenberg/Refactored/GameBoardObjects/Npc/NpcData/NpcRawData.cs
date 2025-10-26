using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents a complete NPC dataset combining metadata, dialogue, and reward information.
    /// </summary>
    /// <remarks>
    /// This record acts as a container for all relevant NPC data.  
    /// It is typically constructed by a loader or factory class such as 
    /// <c>NpcDataLoader</c> and passed to the <c>NpcManager</c> for registration.
    /// </remarks>
    /// <param name="Meta">The general metadata of the NPC (name, position, symbol).</param>
    /// <param name="Dialog">The dialogue data containing question and answer sets.</param>
    /// <param name="Reward">The reward data defining what the player receives after interaction.</param>
    internal sealed record NpcRawData
    (
        NpcMetaData Meta,
        NpcDialogData Dialog,
        NpcRewardData Reward
    );

}
