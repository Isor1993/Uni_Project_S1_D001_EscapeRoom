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
* 09.11.2025 ER Created / Documentation fully updated
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
            // === Screen ===
            ScreenManager screen = new ScreenManager(new ScreenManagerDependencies(symbols, inventory, level, diagnostics));

            // === GAME STATE CONTROL ===
            GameState currentState = GameState.StartScreen;
            bool running = true;

            while (running)
            {
                switch (currentState)
                {
                    case GameState.StartScreen:
                        Console.Clear();
                        screen.ScreenStart();
                        currentState = GameState.Tutorial;
                        break;

                    case GameState.Tutorial:
                        Console.Clear();
                        screen.ScreenTutorial();
                        currentState = GameState.Playing;
                        break;

                    case GameState.Playing:
                        bool gameWon = RunGameLoop(diagnostics, level, gameBoard, spawn, print, ui, inventory, interaction, npcManager, rules, symbols, gameObject);
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
        /// <param name="diagnostics">Provides diagnostic logging for setup progress and potential errors.</param>
        /// <param name="level">Controls level-specific states and conditions (e.g., next level flag).</param>
        /// <param name="gameBoard">Manages the logical grid used for positioning and object placement.</param>
        /// <param name="spawn">Responsible for spawning player, NPCs, key fragments, and doors on the board.</param>
        /// <param name="print">Handles console-based visual rendering of the game board and HUD elements.</param>
        /// <param name="ui">Controls the Heads-Up Display for showing stats, symbols, and interaction feedback.</param>
        /// <param name="npcManager">Manages all NPC entities, including data loading and dialogue handling.</param>
        /// <param name="rules">Contains core gameplay rules and validation for movement and collisions.</param>
        /// <param name="interaction">Handles player interactions with tiles and objects (e.g., doors, NPCs, items).</param>
        /// <param name="symbols">Provides ASCII symbols for visual representation of in-game entities.</param>
        /// <param name="gameObject">Stores and organizes runtime references to all spawned objects on the board.</param>
        /// <returns>Returns a fully initialized <see cref="PlayerController"/> ready for use in the main loop.</returns>
        private static PlayerController InitializeLevel(
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

        /// <summary>
        ///  Initializes the next level after a successful completion or transition.
        /// Resets board data, respawns entities, and refreshes HUD visuals without
        /// reinitializing the entire game session.
        /// </summary>
        /// <param name="diagnostics">Provides logging and validation output for the next-level setup.</param>
        /// <param name="level">Tracks and updates the current level state during transitions.</param>
        /// <param name="gameBoard">Clears and rebuilds the grid for the new level layout.</param>
        /// <param name="spawn">Handles all new entity spawns (NPCs, keys, doors, player) for the upcoming level.</param>
        /// <param name="print">Renders the new board layout and HUD after the level reset.</param>
        /// <param name="ui">Rebuilds top and bottom HUD sections for the next level.</param>
        /// <param name="npcManager">Reloads NPC data and resets interaction states for the new environment.</param>
        /// <param name="rules">Ensures gameplay logic and movement validation are consistent in the new level.</param>
        /// <param name="interaction">Maintains interaction logic and reconnects it with new instances.</param>
        /// <param name="symbols">Provides consistent visual symbols across levels for entity display.</param>
        /// <param name="gameObject">Holds updated object references for all entities in the new level.</param>
        private static void InitializeNextLevel(
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

        /// <summary>
        /// Runs the core game loop for the Escape Room.
        /// Continuously processes user input, updates player movement, manages
        /// interactions, and checks win/loss conditions or level transitions.
        /// </summary>
        /// <param name="diagnostics">Logs checks, warnings, and errors occurring during the loop.</param>
        /// <param name="level">Tracks whether a new level should start or if the game has ended.</param>
        /// <param name="gameBoard">Provides grid-based spatial data for player and object updates.</param>
        /// <param name="spawn">Spawns and resets entities when a new level is triggered.</param>
        /// <param name="print">Handles visual updates to the board and HUD during gameplay.</param>
        /// <param name="ui">Displays real-time player stats, NPC dialogues, and symbol updates.</param>
        /// <param name="inventory">Tracks player key fragments and rewards throughout gameplay.</param>
        /// <param name="interaction">Manages interaction events when the player uses the interact key.</param>
        /// <param name="npcManager">Controls NPC dialogues and logic for question/answer sequences.</param>
        /// <param name="rules">Validates actions, movements, and conditions that define gameplay rules.</param>
        /// <param name="symbols">Provides the character symbols for consistent visual output.</param>
        /// <param name="gameObject">Stores active in-game objects and updates their state on interaction.</param>
        /// <returns>
        /// Returns <c>true</c> if the player successfully completes all levels or wins the game;
        /// returns <c>false</c> if the player dies, quits, or the game ends in failure.
        /// </returns>
        private static bool RunGameLoop(
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
                    InitializeNextLevel(diagnostics, level, gameBoard, spawn, print, ui, npcManager, rules, interaction, symbols, gameObject);
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

                if (level.CurrentLvl > maxLevel)
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