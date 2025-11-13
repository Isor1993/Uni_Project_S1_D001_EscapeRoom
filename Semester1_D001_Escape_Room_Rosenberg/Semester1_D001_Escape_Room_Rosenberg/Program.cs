/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : Program.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Entry point of the Escape Room console game.
* Manages initialization of all systems, dependency injection, game state control,
* and high-level loop flow between start, tutorial, main gameplay, and end screens.
* Coordinates interactions between all managers, systems, and instances.
*
* Responsibilities:
* - Initialize and inject all game dependencies
* - Control game state flow (Start → Tutorial → Playing → Win/GameOver)
* - Manage global static data (board size, player instance, etc.)
* - Handle main game loop and level transitions
*
* History :
* 11.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// The main entry class controlling initialization, dependency setup,
    /// and the global game loop of the Escape Room project.
    /// </summary>
    internal class Program
    {
        // === Global Player Reference ===
        private static PlayerInstance? _playerInstance;
        // === CORE SYSTEMS ===
        private static DiagnosticsManager _diagnostics = null!;
        private static SymbolsManager _symbols = null!;
        // === BOARD SYSTEMS ===
        private static GameBoardManager _gameBoard = null!;
        private static GameObjectManager _gameObject = null!;
        private static RulesManager _rules = null!;
        private static SpawnManager _spawn = null!;
        // === GAME FLOW SYSTEMS ===
        private static LevelManager _level = null!;
        private static InteractionManager _interaction = null!;
        private static NpcManager _npcManager = null!;
        private static InventoryManager _inventory = null!;
        // === VISUALS ===
        private static PrintManager _print = null!;
        private static UIManager _ui = null!;


        // === CURSOR AND HUD COORDINATES ===
        public const int CursorPosX = 0;

        public const int CursorPosYGamBoardStart = 0;
        public const int maxLevel = 5;
        public const int minLive = -1;



        // Dynamische Property: abhängig von ArraySizeY
        /// <summary>
        /// Gets the Y-coordinate where the bottom HUD section begins.
        /// </summary>
        public static int CursorPosYBottomHudStart => ArraySizeY + 3;

        /// <summary>
        /// Gets the Y-coordinate for the top HUD’s first line.
        /// </summary>
        public static int CursorPosYTopHudStart => ArraySizeY;

        /// <summary>
        /// Gets the Y-coordinate for the top HUD’s second line.
        /// </summary>
        public static int CursorPosYTopHud_2 => ArraySizeY + 1;

        /// <summary>
        /// Gets the Y-coordinate for the top HUD’s third line.
        /// </summary>
        public static int CursorPosYTopHud_3 => ArraySizeY + 2;

        // === BOARD SIZE ===
        private static int _arraySizeY;

        private static int _arraySizeX;
        private static int _newArraySizeX = 45;
        private static int _npcAmount = 5;
        private static int _keyAmount = 3;

        /// <summary>
        /// Gets or sets the current player instance.
        /// </summary>
        public static PlayerInstance? PlayerInstance { get => _playerInstance; set => _playerInstance = value; }

        /// <summary>
        /// Gets or sets the number of NPCs to spawn on the board.
        /// </summary>
        public static int NpcAmount { get => _npcAmount; set => _npcAmount = value; }

        /// <summary>
        /// Gets or sets the number of key fragments to spawn on the board.
        /// </summary>
        public static int KeyAmount { get => _keyAmount; set => _keyAmount = value; }

        /// <summary>
        /// Gets or sets the current board height.
        /// </summary>
        public static int ArraySizeY { get => _arraySizeY; set => _arraySizeY = value; }

        /// <summary>
        /// Gets or sets the default width for new levels.
        /// </summary>
        public static int NewArraySizeX { get => _newArraySizeX; set => _newArraySizeX = value; }

        /// <summary>
        /// Gets or sets the current board width.
        /// </summary>
        public static int ArraySizeX { get => _arraySizeX; set => _arraySizeX = value; }

        /// <summary>
        /// Represents the current global state of the game.
        /// </summary>
        private enum GameState
        {
            StartScreen,
            Tutorial,
            Playing,
            GameOver,
            Win
        }

        /// <summary>
        /// The main entry method — sets up all dependencies, systems,
        /// and manages the game state loop.
        /// </summary>
        /// <param name="args">Command line arguments (not used).</param>
        private static void Main(string[] args)
        {
            // === CONSOLE SETUP ===
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.SetBufferSize(120, 200);
            Console.SetWindowSize(120, 40);

            // === CORE SYSTEMS ===
            _diagnostics = new DiagnosticsManager();
            _symbols = new SymbolsManager();
            RandomManager random = new RandomManager(_diagnostics);

            // === BOARD SYSTEMS ===
            _gameBoard = new GameBoardManager(new GameBoardManagerDependencies(_diagnostics));
            _gameObject = new GameObjectManager(new GameObjectManagerDependencies(_gameBoard, _diagnostics));
            _rules = new RulesManager(new RulesManagerDependencies(_diagnostics, _gameBoard));

            // === DATA & INVENTORY ===
            NpcDataLoader npcDataLoader = new NpcDataLoader(new NpcDataLoaderDependencies(_diagnostics, _symbols));
            _npcManager = new NpcManager(new NpcManagerDependencies(npcDataLoader, _diagnostics, _symbols));
            _inventory = new InventoryManager(new InventoryDependencies(_diagnostics));

            // === VISUALS ===
            _print = new PrintManager(new PrintManagerDependencies(_gameBoard, _gameObject, _symbols, _diagnostics));

            // === INSTANCE DEPENDENCIES ===
            WallInstanceDependencies wallInstanceDeps = new WallInstanceDependencies(_diagnostics, _symbols);
            KeyFragmentInstanceDependencies keyFragmentInstanceDeps = new KeyFragmentInstanceDependencies(_symbols, _diagnostics);
            DoorInstanceDependencies doorInstanceDeps = new DoorInstanceDependencies(_symbols, _diagnostics);
            PlayerInstanceDependencies playerInstanceDeps = new PlayerInstanceDependencies(_diagnostics, _symbols);
            NpcInstanceDependencies npcInstanceDeps = new NpcInstanceDependencies(_diagnostics, _symbols);

            // === SPAWN MANAGER ===
            _spawn = new SpawnManager(new SpawnManagerDependencies(
                _rules, _diagnostics, random, _gameBoard, _symbols,
                doorInstanceDeps, keyFragmentInstanceDeps, npcInstanceDeps,
                playerInstanceDeps, wallInstanceDeps, _npcManager, _gameObject
            ));

            // === LEVEL + INTERACTION ===
            _level = new LevelManager(new LevelManagerDependencies(_diagnostics, _gameBoard, _gameObject, _spawn, _inventory));
            _ui = new UIManager(new UIManagerDependencies(_gameBoard, _diagnostics, _symbols, _print, random, _inventory, _gameObject, _level));
            _interaction = new InteractionManager(new InteractionManagerDependencies(
                _diagnostics, _gameBoard, _gameObject, _rules, _inventory, _ui, _npcManager, _symbols, _level, _print, random
            ));
            // === Screen ===
            ScreenManager screen = new ScreenManager(new ScreenManagerDependencies(_symbols, _inventory, _level, _diagnostics));

            // === GAME STATE CONTROL ===
            GameState currentState = GameState.StartScreen;
            bool running = true;

            while (running)
            {
                switch (currentState)
                {
                    case GameState.StartScreen:
                        Console.Clear();
                        //screen.ScreenStart();
                        currentState = GameState.Tutorial;
                        break;

                    case GameState.Tutorial:
                        Console.Clear();
                        //screen.ScreenTutorial();
                        currentState = GameState.Playing;
                        break;

                    case GameState.Playing:
                        bool gameWon = RunGameLoop();
                        if (gameWon)
                            currentState = GameState.Win;
                        else if (PlayerInstance == null || gameWon == false)
                            currentState = GameState.GameOver;
                        break;

                    case GameState.GameOver:
                        Console.Clear();
                        screen.ScreenGameOver();
                        running = false;
                        break;

                    case GameState.Win:
                        Console.Clear();
                        screen.ScreenWin();
                        running = false;
                        break;
                }
            }

            Console.WriteLine("\n[Program] Game ended. Press any key to close.");
            Console.ReadKey(true);
        }

        // === LEVEL INITIALIZATION ===

        /// <summary>
        /// Initializes and builds the first game level.
        /// Handles dependency wiring, board setup, NPC loading, object spawning,
        /// and the creation of the player controller for active gameplay.
        /// </summary>        
        /// <returns>Returns a fully initialized <see cref="PlayerController"/> ready for use in the main loop.</returns>
        private static PlayerController InitializeLevel()
        {
            _diagnostics.AddCheck("=== Starting world setup ===");

            DecideArraySize(_print);

            _gameBoard.InitializeBoard();
            _newArraySizeX = _arraySizeX;
            _npcManager.LoadAllNpcData();
            _spawn.SpawnAll(NpcAmount, KeyAmount);
            // Needs to be Initializede here
            PlayerController playerController = new PlayerController(new PlayerControllerDependencies(
                _gameBoard, _rules, _diagnostics, _interaction, _print, _symbols, _gameObject
            ));
            PlayerInstance = _spawn.GetPlayer;

            _print.PrintBoard();
            _ui.BuildTopHud();
            _ui.BuildEmptyBottomHud();
            _ui.PrintBottomHud();
            return playerController;
        }

        /// <summary>
        ///  Initializes the next level after a successful completion or transition.
        /// Resets board data, respawns entities, and refreshes HUD visuals without
        /// reinitializing the entire game session.
        /// </summary>        
        private static void InitializeNextLevel()
        {
            _diagnostics.AddCheck("=== Starting world setup ===");

            DecideArraySize(15, NewArraySizeX);

            _gameBoard.InitializeBoard();
            _npcManager.LoadAllNpcData();
            _spawn.SpawnAllNewLvl(NpcAmount, KeyAmount);

            PlayerInstance = _spawn.GetPlayer;

            _print.PrintBoard();
            _ui.BuildTopHud();
            _ui.BuildEmptyBottomHud();
            _ui.PrintBottomHud();
        }

        // === MAIN GAME LOOP ===

        /// <summary>
        /// Runs the core game loop for the Escape Room.
        /// Continuously processes user input, updates player movement, manages
        /// interactions, and checks win/loss conditions or level transitions.
        /// </summary>        
        /// <returns>
        /// Returns <c>true</c> if the player successfully completes all levels or wins the game;
        /// returns <c>false</c> if the player dies, quits, or the game ends in failure.
        /// </returns>
        private static bool RunGameLoop()
        {
            PlayerController playerController = InitializeLevel();

            while (true)
            {
                // === NEXT LEVEL HANDLING ===
                if (_level.IsNextLvl)
                {
                    _diagnostics.AddCheck("=== Starting world setup NEW LEVEL ===");
                    InitializeNextLevel();
                    _level.IsNextLvl = false;
                    continue;
                }

                // === INPUT ===
                ConsoleKey key = Console.ReadKey(true).Key;
                playerController.MovePlayer(key);

                if (key == ConsoleKey.I)
                {
                    _diagnostics.PrintChronologicalLogs();
                }
                if (key == ConsoleKey.Escape)
                    return false;

                if (key == ConsoleKey.K)
                {
                    _inventory.AddKeyFragment(100);
                }
                if (key == ConsoleKey.E && PlayerInstance != null)
                    _interaction.InteractionHandler(PlayerInstance.Position);

                if (_level.CurrentLvl > maxLevel)
                {
                    return true;
                }
                if (PlayerInstance != null && PlayerInstance.Lives <= minLive)
                    return false;
            }
        }

        // === ASK FOR SIZE ===

        /// <summary>
        /// Asks the player to define the board size interactively via the console.
        /// Ensures the entered dimensions fall within the allowed range.
        /// </summary>
        /// <param name="print">Provides the console input/output methods used for requesting values.</param>
        private static void DecideArraySize(PrintManager print)
        {
            _arraySizeX = print.AskForIntInRange("How wide should the game board be?", 45, 120);
            _arraySizeY = print.AskForIntInRange("How high should the game board be?", 15, 17);
        }

        /// <summary>
        /// Sets the board size directly with given Y and X dimensions
        /// (used for automated level transitions without user input).
        /// </summary>
        /// <param name="arraySizeY">Defines the height (rows) of the board grid.</param>
        /// <param name="arraySizeX">Defines the width (columns) of the board grid.</param>
        private static void DecideArraySize(int arraySizeY, int arraySizeX)
        {
            _arraySizeX = arraySizeX;
            _arraySizeY = arraySizeY;
        }
    }
}