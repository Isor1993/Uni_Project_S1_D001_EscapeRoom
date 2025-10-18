using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    internal class NpcRewardData
    {
        public NpcRewardData(int keyFragment, int points)
        {
            _keyFragment = keyFragment;
            _points = points;
        }


        private int _keyFragment;
        private int _points;

        public int KeyFragment=> _keyFragment;
        public int Points => _points;
        
    }
}