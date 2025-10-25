using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{

    internal sealed record InteractionManagerDependencies
    (
        DiagnosticsManager Diagnostic,
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        RulesManager Rule,
        InventoryManager Inventory,
        UIManager UI,
        NpcManager Npc,
        SymbolsManager Symbol,
        LevelManager Level,
        DoorInstance Door

    );
}
