using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    internal class NpcMetaData
    {
        readonly SymbolsManager _symbolsManager;
        public NpcMetaData(string name, (int y, int x) position, SymbolsManager symbolsManager)
        {
            _name = name;
            _position = position;
            _symbolsManager = symbolsManager;
            _symbol = Symbol;
        }
        private string _name;
        private (int y, int x) _position;
        private char _symbol;

        public string Name => _name;
        public (int y, int x) Position => _position;
        public char Symbol => _symbolsManager.QuestSymbol;
    }
}
