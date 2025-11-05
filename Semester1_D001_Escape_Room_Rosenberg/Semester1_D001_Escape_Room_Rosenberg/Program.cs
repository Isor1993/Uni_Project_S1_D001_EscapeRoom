// RSK Kontrolle ok
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
        private static int _newArraySizeX;
        private static int _npcAmount = 5;
        private static int _keyAmount = 3;

        public static int NpcAmount { get => _npcAmount; set => _npcAmount = value; }
        public static int KeyAmount { get => _keyAmount; set => _keyAmount = value; }
        public static int ArraySizeY { get => _arraySizeY; set => _arraySizeY = value; }
        public static int NewArraySizeX { get => _newArraySizeX; set => _newArraySizeX = value; }
        public static int ArraySizeX { get => _arraySizeX; set => _arraySizeX = value; }

        // === LEVEL ===
        private static readonly int _currentLevel = 1;
        public static int CurrentLevel => _currentLevel;


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

            // === BOARD SETUP ===
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
            SpawnManager spawn = new SpawnManager(new SpawnManagerDependencies(rules, diagnostics, random,
                gameBoard, symbols, doorInstanceDeps, keyFragmentInstanceDeps, npcInstanceDeps,
                playerInstanceDeps, wallInstanceDeps, npcManager, gameObject
                ));

            LevelManager level = new LevelManager(new LevelManagerDependencies(diagnostics, gameBoard, gameObject, spawn));

            // === Interaction MANAGER ===
            UIManager ui = new UIManager(new UIManagerDependencies(gameBoard, diagnostics, symbols, print, random, inventory, gameObject, level));
            InteractionManager interaction = new InteractionManager(new InteractionManagerDependencies(
              diagnostics, gameBoard, gameObject, rules, inventory, ui, npcManager, symbols, level, print, random

           ));



            // === INITIAL WORLD SETUP ===
            diagnostics.AddCheck("=== Starting world setup ===");

            // 1️ Spielfeldgröße zuerst bestimmen
            DecideArraySize(print);

            // 2️ Board initialisieren
            gameBoard.InitializeBoard();

            // 3️ NPC-Daten laden
            npcManager.LoadAllNpcData();

            // 4️ Welt befüllen (Spawns)
            spawn.SpawnAll(_npcAmount, _keyAmount);

            // 5️ Ausgabe vorbereiten
            print.PrintBoard();

            // 6️ HUD aufbauen
            ui.BuildTopHud();
            ui.BuildEmptyBottomHud();
            ui.PrintBottomHud();

            // 7️⃣ Player Controller erzeugen
            PlayerController playerController = new PlayerController(new PlayerControllerDependencies(
                gameBoard, rules, diagnostics, interaction, print, symbols, gameObject
            ));
            playerController.SetStartPosition();

            // 8️ Abschlusslog & Diagnoseausgabe

            // diagnostics.AddCheck("=== World setup complete ===");
            // diagnostics.PrintChronologicalLogs();

            //TODO player.SetName("Player");

            // === GAME LOOP ===

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                // 🔹 Bewegung steuern
                playerController.MovePlayer(key);

                // 🔹 ESC zum Beenden
                if (key == ConsoleKey.Escape)
                {
                    diagnostics.AddCheck("[Program] ESC pressed – exiting game.");
                    break;
                }

                // 🔹 Interaktion nur bei E-Taste
                if (key == ConsoleKey.E)
                {
                    PlayerInstance? player = gameObject.Player;
                    if (player == null)
                    {
                        diagnostics.AddError($"{nameof(Program)}: No Player");
                        break;
                    }

                    interaction.InteractionHandler(player.Position);
                }
                if (key == ConsoleKey.I)
                {
                    diagnostics.AddCheck("=== World setup complete ===");
                    diagnostics.PrintChronologicalLogs();
                }
                if (key == ConsoleKey.K)
                {
                    inventory.AddKeyFragment(100);
                    Console.WriteLine("key added");
                }
                if (NewArraySizeX >= 120)
                { break; }
                if ( level.IsNextLvl)
                {
                    diagnostics.AddCheck("=== Starting world setup NEW LEVEL ===");

                    // 1️ Spielfeldgröße zuerst bestimmen
                    DecideArraySize(15,NewArraySizeX);

                    // 2️ Board initialisieren
                    gameBoard.InitializeBoard();

                    // 3️ NPC-Daten laden
                    npcManager.LoadAllNpcData();

                    // 4️ Welt befüllen (Spawns)
                    spawn.SpawnAll(_npcAmount, _keyAmount);

                    // 5️ Ausgabe vorbereiten
                    print.PrintBoard();

                    // 6️ HUD aufbauen
                    ui.BuildTopHud();
                    ui.BuildEmptyBottomHud();
                    ui.PrintBottomHud();
                    level.IsNextLvl= false;

                }
            }
            Console.WriteLine("\n[Program] Game ended. Press any key to close.");
            Console.ReadKey(true);



        }
        static void DecideArraySize(PrintManager print)
        {
            // Ask for dimensions within predefined limits.
            _arraySizeX = print.AskForIntInRange("How wide should the game board be?", 45, 120);
            _arraySizeY = print.AskForIntInRange("How high should the game board be?", 15, 20);

        }

        static void DecideArraySize(int arraySizeY, int arraySizeX)
        {
            _arraySizeX = arraySizeX;
            _arraySizeY = arraySizeY;
        }
    }
}