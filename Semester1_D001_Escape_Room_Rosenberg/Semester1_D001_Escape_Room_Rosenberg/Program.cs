// RSK Kontrolle ok
using Microsoft.VisualBasic;
using Semester1_D001_Escape_Room_Rosenberg.Refactored;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Security.AccessControl;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class Program
    {
        private static PlayerInstance _playerInstance;
        // === CURSOR- UND HUD-KOORDINATEN ===
        public const int CursorPosX = 0;
        public const int CursorPosYGamBoardStart = 0;

        // Dynamische Property: abhängig von ArraySizeY
        public static int CursorPosYBottomHudStart => ArraySizeY + 3;
        public static int CursorPosYTopHudStart => ArraySizeY;
        public static int CursorPosYTopHud_2 => ArraySizeY + 1;
        public static int CursorPosYTopHud_3 => ArraySizeY + 2;


        // === BOARD-GRÖSSE ===
        private static int _arraySizeY;
        private static int _arraySizeX;
        // Fields
        private static int _newArraySizeX = 45;
        private static int _npcAmount = 5;
        private static int _keyAmount = 3;

        public static PlayerInstance PlayerInstance { get => _playerInstance; set => _playerInstance = value; }
        public static int NpcAmount { get => _npcAmount; set => _npcAmount = value; }
        public static int KeyAmount { get => _keyAmount; set => _keyAmount = value; }
        public static int ArraySizeY { get => _arraySizeY; set => _arraySizeY = value; }
        public static int NewArraySizeX { get => _newArraySizeX; set => _newArraySizeX = value; }
        public static int ArraySizeX { get => _arraySizeX; set => _arraySizeX = value; }

        // === LEVEL ===
        private static readonly int _currentLevel = 1;
        public static int CurrentLevel => _currentLevel;


        // === GAME STATES ===
        enum GameState
        {
            StartScreen,
            Tutorial,
            Playing,
            GameOver,
            Win
        }

        static void Main(string[] args)
        {
            // === CONSOLE SETUP ===
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.SetBufferSize(120, 200);
            Console.SetWindowSize(120, 40);

            // === CORE SYSTEMS ===
            DiagnosticsManager diagnostics = new DiagnosticsManager();
            SymbolsManager symbols = new SymbolsManager();
            RandomManager random = new RandomManager(diagnostics);

            // === BOARD SYSTEMS ===
            GameBoardManager gameBoard = new GameBoardManager(new GameBoardManagerDependencies(diagnostics));
            GameObjectManager gameObject = new GameObjectManager(new GameObjectManagerDependencies(gameBoard, diagnostics));
            RulesManager rules = new RulesManager(new RulesManagerDependencies(diagnostics, gameBoard));

            // === DATA & INVENTORY ===
            NpcDataLoader npcDataLoader = new NpcDataLoader(new NpcDataLoaderDependencies(diagnostics, symbols));
            NpcManager npcManager = new NpcManager(new NpcManagerDependencies(npcDataLoader, diagnostics, symbols));
            InventoryManager inventory = new InventoryManager(new InventoryDependencies(diagnostics));

            // === VISUALS ===
            PrintManager print = new PrintManager(new PrintManagerDependencies(gameBoard, gameObject, symbols, diagnostics));

            // === INSTANCE DEPENDENCIES ===
            WallInstanceDependencies wallInstanceDeps = new WallInstanceDependencies(diagnostics, symbols);
            KeyFragmentInstanceDependencies keyFragmentInstanceDeps = new KeyFragmentInstanceDependencies(symbols, diagnostics);
            DoorInstanceDependencies doorInstanceDeps = new DoorInstanceDependencies(symbols, diagnostics);
            PlayerInstanceDependencies playerInstanceDeps = new PlayerInstanceDependencies(diagnostics, symbols);
            NpcInstanceDependencies npcInstanceDeps = new NpcInstanceDependencies(diagnostics, symbols);

            // === SPAWN MANAGER ===
            SpawnManager spawn = new SpawnManager(new SpawnManagerDependencies(
                rules, diagnostics, random, gameBoard, symbols,
                doorInstanceDeps, keyFragmentInstanceDeps, npcInstanceDeps,
                playerInstanceDeps, wallInstanceDeps, npcManager, gameObject
            ));

            // === LEVEL + INTERACTION ===
            LevelManager level = new LevelManager(new LevelManagerDependencies(diagnostics, gameBoard, gameObject, spawn, inventory));
            UIManager ui = new UIManager(new UIManagerDependencies(gameBoard, diagnostics, symbols, print, random, inventory, gameObject, level));
            InteractionManager interaction = new InteractionManager(new InteractionManagerDependencies(
                diagnostics, gameBoard, gameObject, rules, inventory, ui, npcManager, symbols, level, print, random
            ));


            // === GAME STATE CONTROL ===
            GameState currentState = GameState.StartScreen;
            bool running = true;

            while (running)
            {
                switch (currentState)
                {
                    case GameState.StartScreen:
                        ShowStartScreen();
                        currentState = GameState.Tutorial;
                        break;

                    case GameState.Tutorial:
                        ShowTutorialScreen();
                        currentState = GameState.Playing;
                        break;

                    case GameState.Playing:
                        bool gameWon = RunGameLoop(diagnostics, level, gameBoard, spawn, print, ui, inventory, interaction, npcManager, rules, symbols, gameObject);
                        if (gameWon)
                            currentState = GameState.Win;
                        else if (PlayerInstance == null || PlayerInstance.Heart <= 0)
                            currentState = GameState.GameOver;
                        break;

                    case GameState.GameOver:
                        ShowGameOverScreen();
                        running = false;
                        break;

                    case GameState.Win:
                        ShowWinScreen();
                        running = false;
                        break;
                }
            }

            Console.WriteLine("\n[Program] Game ended. Press any key to close.");
            Console.ReadKey(true);
        }

        // === LEVEL INITIALIZATION ===
        static PlayerController InitializeLevel(
            DiagnosticsManager diagnostics,
            LevelManager level,
            GameBoardManager gameBoard,
            SpawnManager spawn,
            PrintManager print,
            UIManager ui,
            NpcManager npcManager,
            RulesManager rules,
            InteractionManager interaction,
            SymbolsManager symbols,
            GameObjectManager gameObject
            )
        {
            diagnostics.AddCheck("=== Starting world setup ===");

            DecideArraySize(print);

            gameBoard.InitializeBoard();
            npcManager.LoadAllNpcData();
            spawn.SpawnAll(NpcAmount, KeyAmount);
            // Needs to be Initializede here
            PlayerController playerController = new PlayerController(new PlayerControllerDependencies(
                gameBoard, rules, diagnostics, interaction, print, symbols, gameObject
            ));
            PlayerInstance = spawn.GetPlayer;

            print.PrintBoard();
            ui.BuildTopHud();
            ui.BuildEmptyBottomHud();
            ui.PrintBottomHud();
            return playerController;
        }
        // === LEVEL INITIALIZATION ===
        static void InitializeNExtLevel(
            DiagnosticsManager diagnostics,
            LevelManager level,
            GameBoardManager gameBoard,
            SpawnManager spawn,
            PrintManager print,
            UIManager ui,
            NpcManager npcManager,
            RulesManager rules,
            InteractionManager interaction,
            SymbolsManager symbols,
            GameObjectManager gameObject
            )
        {
            diagnostics.AddCheck("=== Starting world setup ===");

            DecideArraySize(15, NewArraySizeX);

            gameBoard.InitializeBoard();
            npcManager.LoadAllNpcData();
            spawn.SpawnAllNewLvl(NpcAmount, KeyAmount);
            
            PlayerInstance = spawn.GetPlayer;

            print.PrintBoard();
            ui.BuildTopHud();
            ui.BuildEmptyBottomHud();
            ui.PrintBottomHud();

        }

        // === MAIN GAME LOOP ===
        static bool RunGameLoop(
            DiagnosticsManager diagnostics,
            LevelManager level,
            GameBoardManager gameBoard,
            SpawnManager spawn,
            PrintManager print,
            UIManager ui,
            InventoryManager inventory,
            InteractionManager interaction,
            NpcManager npcManager,
            RulesManager rules,
            SymbolsManager symbols,
            GameObjectManager gameObject)
        {
            PlayerController playerController = InitializeLevel(diagnostics, level, gameBoard, spawn, print, ui, npcManager, rules, interaction, symbols, gameObject);

            while (true)
            {

                // === NEXT LEVEL HANDLING ===
                if (level.IsNextLvl)
                {

                    diagnostics.AddCheck("=== Starting world setup NEW LEVEL ===");
                    InitializeNExtLevel(diagnostics, level, gameBoard, spawn, print, ui, npcManager, rules, interaction, symbols, gameObject);
                    level.IsNextLvl = false;
                    continue;
                }

                // === INPUT ===
                ConsoleKey key = Console.ReadKey(true).Key;
                playerController.MovePlayer(key);


                if (key == ConsoleKey.I)
                {
                    diagnostics.PrintChronologicalLogs();
                }
                if (key == ConsoleKey.Escape)
                    return false;

                if (key == ConsoleKey.K)
                {
                    inventory.AddKeyFragment(100);
                    
                }
                if (key == ConsoleKey.E && PlayerInstance != null)
                    interaction.InteractionHandler(PlayerInstance.Position);

                if (CurrentLevel > 10)
                    return true;

                if (PlayerInstance != null && PlayerInstance.Heart <= -1)
                    return false;

            }
        }

        // === ASK FOR SIZE ===
        static void DecideArraySize(PrintManager print)
        {
            _arraySizeX = print.AskForIntInRange("How wide should the game board be?", 45, 120);
            _arraySizeY = print.AskForIntInRange("How high should the game board be?", 15, 20);
        }

        static void DecideArraySize(int arraySizeY, int arraySizeX)
        {
            _arraySizeX = arraySizeX;
            _arraySizeY = arraySizeY;
        }

        // === UI SCREENS ===
        static void ShowStartScreen()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("              ESCAPE ROOM – ISOR TOWER");
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("           Press any key to begin...");
            Console.ReadKey(true);
        }

        static void ShowTutorialScreen()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("                    TUTORIAL");
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("Use arrow keys to move.");
            Console.WriteLine("Press 'E' to interact.");
            Console.WriteLine("Collect key fragments and avoid death!");
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("           Press any key to start...");
            Console.ReadKey(true);
        }

        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("                   GAME OVER 💀");
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("Your soul could not escape the Tower...");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }

        static void ShowWinScreen()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("                YOU ESCAPED! 🎉");
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("Your mind withstood the tower’s trials.");
            Console.WriteLine("Press any key to close.");
            Console.ReadKey(true);
        }
    }
}
