// RSK Kontrolle ok
namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class Program
    {
        readonly static int _currentlevel = 10;
        public static int CurrentLevel { get => _currentlevel; }

        static void Main(string[] args)
        {
            // Console output encoding
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // damit es beim vollbild nicht verzehrt wird
            Console.SetWindowSize(120, 25);
            Console.SetBufferSize(120, 25);
            //
            RandomManager random= new RandomManager();
            // Initialize core objects
            PrinterManager printer = new PrinterManager();
            DiagnosticsManager diagnostics = new DiagnosticsManager();
            SymbolsManager symbols = new SymbolsManager();
            // GameBoardBuilder receives printer and symbols as read-only references
            GameBoardBuilder boardBuilder = new GameBoardBuilder(printer, symbols);
            /*RulesManager rules = new RulesManager(symbols, boardBuilder, printer);
            SpawnerManager spawner = new SpawnerManager(random,printer, symbols, rules, boardBuilder);
            MovePlayerManager move = new MovePlayerManager(spawner,boardBuilder,rules,symbols);
            UIManager uIBuilder=new UIManager(random,boardBuilder,symbols,printer,spawner);
            // Player sets board size
            boardBuilder.DecideArraySize();
            // Fills array with symbols (wall and empty)
            boardBuilder.FillWallsToBoard();
            // Fill array with a random door symbol
            spawner.SpawnDoor();
            // Fill array with a random player symbol
            spawner.SpawnPlayer();
            // Fill array with a key fragment
            spawner.SpawnKeyFragment();
            // Fill array with a quest Npc
            spawner.SpawnQuestNpc();
            // set cursor invisible
            Console.CursorVisible = false;
            // Print game board 
            // Start Timer
            uIBuilder.StartTimer();
            //build Hud
            uIBuilder.BuildTopHud();
            printer.PrintArray(boardBuilder.GameBoardArray);
            uIBuilder.FillUpBottomHud("System","Reward :",symbols.KeyFragmentSymbol,5);
            // get start porsition
            move.SetStartPosition(spawner.PlayerStartposition);
            // Move funktion
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                move.MovePlayer(key);
            }
            */
            // Keep console window open
            Console.ReadKey();
        }
    }
}
