using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// 
    /// </summary>
    internal class NpcInstance
    {
        private NpcMetaData _meta;
        private NpcDialogData _dialog;
        private NpcRewardData _reward;

        private bool _isActive;
        private bool _hasInteracted;

        public NpcInstance(NpcMetaData meta, NpcDialogData dialog, NpcRewardData reward)
        {
            _meta = meta;
            _dialog = dialog;
            _reward = reward;
            _isActive = false;
            _hasInteracted = false;
        }

        public NpcMetaData Meta => _meta;
        public NpcDialogData Dialog => _dialog;
        public NpcRewardData Reward => _reward;

        public bool IsActive => _isActive;
        public bool HasInteracted => _hasInteracted;
    }












}










}
