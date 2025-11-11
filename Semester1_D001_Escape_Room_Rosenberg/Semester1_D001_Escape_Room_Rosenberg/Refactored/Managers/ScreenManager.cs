/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : ScreenManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles all visual console screen outputs, including tutorial, start, win,
* and game over screens.
* Uses ASCII-based UI elements to create immersive feedback sequences during gameplay.
* Integrates with inventory, level, and symbol systems for dynamic data display.
*
* Responsibilities:
* - Render title, tutorial, win, and game over screens
* - Display dynamic score and symbols from dependencies
* - Provide transitional pauses and animations for immersive console output
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Handles all static and animated screen displays such as tutorial,
    /// start menu, victory, and game over scenes.
    /// </summary>
    /// <remarks>
    /// The <see cref="ScreenManager"/> uses ASCII-based layout strings
    /// to render immersive visual transitions.
    /// It integrates with <see cref="ScreenManagerDependencies"/>
    /// for access to <see cref="SymbolsManager"/>, <see cref="InventoryManager"/>,
    /// and <see cref="LevelManager"/> data.
    /// Each screen supports animation timing and visual spacing
    /// to enhance player experience within the console window.
    /// </remarks>
    internal class ScreenManager
    {
        // === Dependencies ===
        private readonly ScreenManagerDependencies _deps;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        /// <param name="screenManagerDependencies">
        /// Provides access to symbol, inventory, level, and diagnostics managers
        /// for runtime visual rendering.
        /// </param>
        public ScreenManager(ScreenManagerDependencies screenManagerDependencies)
        {
            _deps = screenManagerDependencies;
        }

        // === Fields ===
        // Tutorial screen symbols and layout strings.
        private char _playerSymbol = ' ';

        private char _keyFragmentSymbol = ' ';
        private char _npcSymbol = ' ';
        private char _openDoor = ' ';

        // Static tutorial ASCII layout strings
        private static string _tutorial_1 = "                                               ░▀█▀░█░█░▀█▀░█▀█░█▀▄░▀█▀░█▀█░█░░";

        private static string _tutorial_2 = "                                               ░░█░░█░█░░█░░█░█░█▀▄░░█░░█▀█░█░░";
        private static string _tutorial_3 = "                                               ░░▀░░▀▀▀░░▀░░▀▀▀░▀░▀░▀▀▀░▀░▀░▀▀▀";

        // Tutorial ASCII layout strings
        private string _tutorialBoxSpace = "                         ║                                                                         ║";

        private string _tutorialBoxLineTop = "                         ╔═════════════════════════════════════════════════════════════════════════╗";
        private string _tutorialIntro = "                         ║ Welcome to the Escape Room. Your goal is to escape room through the door║";
        private string _tutorial_4 = "                         ║ Movement:             => Move your player with the following keys       ║";
        private string _tutorial_5 = "                         ║    [W]                => Up                                             ║";
        private string _tutorial_6 = "                         ║ [A][S][D]             => Left|Down|Right                                ║";
        private string _tutorial_7 = "                         ║ [E]                   => Interact with game objects                     ║";
        private string _tutorial_8 = "                         ║ [Esc]                 => Quit the game                                  ║";
        private string _tutorial_9 = "                         ║ [1]                   => Choose answer 1 with key [1]                   ║";
        private string _tutorial_10 = "                         ║ [2]                   => Choose answer 2 with key [2]                   ║";
        private string _tutorial_11 = "                         ║ [3]                   => Choose answer 3 with key [3]                   ║";
        private string? _tutorial_12 = ": Player symbol      => Thats you :)                                   ║";
        private string? _tutorial_13 = ": Keyfragment symbol => collect these to open the door                 ║";
        private string? _tutorial_14 = ": Door symbol        => To open it you need enough keyfragments        ║";
        private string? _tutorial_15 = ": Npc symbol         => Interact with them to get a quiz with a reward ║";
        private string _tutorialBoxLineBottom = "                         ╚═════════════════════════════════════════════════════════════════════════╝";
        private List<string> _tutorial = new List<string> { _tutorial_1, _tutorial_2, _tutorial_3 };

        // === ScreenStart ===

        private static string _escapeRoom_1 = "       ██████████                                                  ███████████";
        private static string _escapeRoom_2 = "      ▒▒███▒▒▒▒▒█                                                 ▒▒███▒▒▒▒▒███";
        private static string _escapeRoom_3 = "       ▒███  █ ▒   █████   ██████   ██████   ████████   ██████     ▒███    ▒███   ██████   ██████  █████████████";
        private static string _escapeRoom_4 = "       ▒██████    ███▒▒   ███▒▒███ ▒▒▒▒▒███ ▒▒███▒▒███ ███▒▒███    ▒██████████   ███▒▒███ ███▒▒███▒▒███▒▒███▒▒███";
        private static string _escapeRoom_5 = "       ▒███▒▒█   ▒▒█████ ▒███ ▒▒▒   ███████  ▒███ ▒███▒███████     ▒███▒▒▒▒▒███ ▒███ ▒███▒███ ▒███ ▒███ ▒███ ▒███";
        private static string _escapeRoom_6 = "       ▒███ ▒   █ ▒▒▒▒███▒███  ███ ███▒▒███  ▒███ ▒███▒███▒▒▒      ▒███    ▒███ ▒███ ▒███▒███ ▒███ ▒███ ▒███ ▒███";
        private static string _escapeRoom_7 = "       ██████████ ██████ ▒▒██████ ▒▒████████ ▒███████ ▒▒██████     █████   █████▒▒██████ ▒▒██████  █████▒███ █████";
        private static string _escapeRoom_8 = "      ▒▒▒▒▒▒▒▒▒▒ ▒▒▒▒▒▒   ▒▒▒▒▒▒   ▒▒▒▒▒▒▒▒  ▒███▒▒▒   ▒▒▒▒▒▒     ▒▒▒▒▒   ▒▒▒▒▒  ▒▒▒▒▒▒   ▒▒▒▒▒▒  ▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒";
        private static string _escapeRoom_9 = "                                             ▒███";
        private static string _escapeRoom_10 = "                                            █████";
        private List<string> _escapeRoom = new List<string> { _escapeRoom_1, _escapeRoom_2, _escapeRoom_3, _escapeRoom_4, _escapeRoom_5, _escapeRoom_6, _escapeRoom_7, _escapeRoom_8, _escapeRoom_9, _escapeRoom_10 };

        private static string _madeBy_1 = "                                                           █████             █████";
        private static string _madeBy_2 = "                                                           ▒▒███             ▒▒███";
        private static string _madeBy_3 = "                              █████████████    ██████    ███████   ██████     ▒███████  █████ ████";
        private static string _madeBy_4 = "                             ▒▒███▒▒███▒▒███  ▒▒▒▒▒███  ███▒▒███  ███▒▒███    ▒███▒▒███▒▒███ ▒███";
        private static string _madeBy_5 = "                              ▒███ ▒███ ▒███   ███████ ▒███ ▒███ ▒███████     ▒███ ▒███ ▒███ ▒███";
        private static string _madeBy_6 = "                              ▒███ ▒███ ▒███  ███▒▒███ ▒███ ▒███ ▒███▒▒▒      ▒███ ▒███ ▒███ ▒███";
        private static string _madeBy_7 = "                              █████▒███ █████▒▒████████▒▒████████▒▒██████     ████████  ▒▒███████";
        private static string _madeBy_8 = "                             ▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒  ▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒▒▒  ▒▒▒▒▒▒     ▒▒▒▒▒▒▒▒    ▒▒▒▒▒███";
        private static string _madeBy_9 = "                                                                                        ███ ▒███";
        private static string _madeBy_10 = "                                                                                       ▒▒██████";
        private List<string> _madeBy = new List<string> { _madeBy_1, _madeBy_2, _madeBy_3, _madeBy_4, _madeBy_5, _madeBy_6, _madeBy_7, _madeBy_8, _madeBy_9, _madeBy_10 };

        private List<string> _name = new List<string> { _name_1, _name_2, _name_3, _name_4, _name_5, _name_6, _name_7, _name_8 };

        private static string _name_1 = "                             █████                            ████   ████████   ████████   ████████";
        private static string _name_2 = "                            ▒▒███                            ▒▒███  ███▒▒▒▒███ ███▒▒▒▒███ ███▒▒▒▒███";
        private static string _name_3 = "                             ▒███   █████   ██████  ████████  ▒███ ▒███   ▒███▒███   ▒███▒▒▒    ▒███";
        private static string _name_4 = "                             ▒███  ███▒▒   ███▒▒███▒▒███▒▒███ ▒███ ▒▒█████████▒▒█████████   ██████▒";
        private static string _name_5 = "                             ▒███ ▒▒█████ ▒███ ▒███ ▒███ ▒▒▒  ▒███  ▒▒▒▒▒▒▒███ ▒▒▒▒▒▒▒███  ▒▒▒▒▒▒███";
        private static string _name_6 = "                             ▒███  ▒▒▒▒███▒███ ▒███ ▒███      ▒███  ███   ▒███ ███   ▒███ ███   ▒███";
        private static string _name_7 = "                             █████ ██████ ▒▒██████  █████     █████▒▒████████ ▒▒████████ ▒▒████████";
        private static string _name_8 = "                            ▒▒▒▒▒ ▒▒▒▒▒▒   ▒▒▒▒▒▒  ▒▒▒▒▒     ▒▒▒▒▒  ▒▒▒▒▒▒▒▒   ▒▒▒▒▒▒▒▒   ▒▒▒▒▒▒▒▒";

        // === ScreenWin ===

        private static string _youWon_1 = "                       █████ █████                        █████   ███   █████";
        private static string _youWon_2 = "                      ▒▒███ ▒▒███                        ▒▒███   ▒███  ▒▒███";
        private static string _youWon_3 = "                       ▒▒███ ███    ██████  █████ ████    ▒███   ▒███   ▒███   ██████  ████████";
        private static string _youWon_4 = "                        ▒▒█████    ███▒▒███▒▒███ ▒███     ▒███   ▒███   ▒███  ███▒▒███▒▒███▒▒███";
        private static string _youWon_5 = "                         ▒▒███    ▒███ ▒███ ▒███ ▒███     ▒▒███  █████  ███  ▒███ ▒███ ▒███ ▒███";
        private static string _youWon_6 = "                          ▒███    ▒███ ▒███ ▒███ ▒███      ▒▒▒█████▒█████▒   ▒███ ▒███ ▒███ ▒███";
        private static string _youWon_7 = "                          █████   ▒▒██████ ▒▒████████        ▒▒███ ▒▒███     ▒▒██████  ████ █████";
        private static string _youWon_8 = "                         ▒▒▒▒▒     ▒▒▒▒▒▒   ▒▒▒▒▒▒▒▒          ▒▒▒   ▒▒▒       ▒▒▒▒▒▒  ▒▒▒▒ ▒▒▒▒▒";

        private string _youWonBoxLineTop_9 = "                      ╔❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ╗";
        private string _youWonBoxmessage_10 = "                      ☠                Your mind withstood the tower’s trials.                ☠";
        private string _youWonBoxSpace_11 = "                      ☠                                                                       ☠";
        private string _youWonBoxmessage_12 = "                      ☠  I hope you had a little bit of fun! :)                               ☠";
        private string _youWonBoxmessage_13 = "                      ☠  This small project was created during my studies at                  ☠";
        private string _youWonBoxmessage_14 = "                      ☠  the university.In the ReadMe file, you’ll find information on how    ☠";
        private string _youWonBoxmessage_15 = "                      ☠  to change the code to create your own quiz questions and answers.    ☠";
        private string _youWonBoxContact_16 = "                      ☠  Contact info:                                                        ☠";
        private string _youWonBoxContact_17 = "                      ☠  Instagram: https://www.instagram.com/isor_gamedev                    ☠";
        private string _youWonBoxContact_18 = "                      ☠  GitHub: https://github.com/Isor1993                                  ☠";
        private string _youWonBoxContact_19 = "                      ☠  E-mail: IsorDev@email.de                                             ☠";
        private string _youWonBox_20 = "                      ☠                              Your Score                               ☠";
        private string _youWonBox_21 = "                      ☠                                  ||                                   ☠";
        private string _youWonBox_22 = "                      ☠                                  ||                                   ☠";
        private string _youWonBoxLineBottom_23 = "                      ╚❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ VV❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ❤ ╝";
        private string _youWonScore_24 = "                                                        ";
        private List<string> _youWon = new List<string> { _youWon_1, _youWon_2, _youWon_3, _youWon_4, _youWon_5, _youWon_6, _youWon_7, _youWon_8 };
        // === ScreenGameOver ===

        private static string _gameOver_1 = "              █████████                                           ███████";
        private static string _gameOver_2 = "             ███▒▒▒▒▒███                                        ███▒▒▒▒▒███";
        private static string _gameOver_3 = "            ███     ▒▒▒   ██████   █████████████    ██████     ███     ▒▒███ █████ █████  ██████  ████████";
        private static string _gameOver_4 = "           ▒███          ▒▒▒▒▒███ ▒▒███▒▒███▒▒███  ███▒▒███   ▒███      ▒███▒▒███ ▒▒███  ███▒▒███▒▒███▒▒███";
        private static string _gameOver_5 = "           ▒███    █████  ███████  ▒███ ▒███ ▒███ ▒███████    ▒███      ▒███ ▒███  ▒███ ▒███████  ▒███ ▒▒▒";
        private static string _gameOver_6 = "           ▒▒███  ▒▒███  ███▒▒███  ▒███ ▒███ ▒███ ▒███▒▒▒     ▒▒███     ███  ▒▒███ ███  ▒███▒▒▒   ▒███";
        private static string _gameOver_7 = "            ▒▒█████████ ▒▒████████ █████▒███ █████▒▒██████     ▒▒▒███████▒    ▒▒█████   ▒▒██████  █████";
        private static string _gameOver_8 = "             ▒▒▒▒▒▒▒▒▒   ▒▒▒▒▒▒▒▒ ▒▒▒▒▒ ▒▒▒ ▒▒▒▒▒  ▒▒▒▒▒▒        ▒▒▒▒▒▒▒       ▒▒▒▒▒     ▒▒▒▒▒▒  ▒▒▒▒▒";

        private string _GameOverBoxLineTop_9 = "                      ╔☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ╗";
        private string _gameOverBoxMessage_10 = "                      ☠                Your soul could not escape the Tower...                ☠";
        private string _gameOverBoxSpace_11 = "                      ☠                                                                       ☠";
        private string _gameOverBoxMessage_12 = "                      ☠  I hope you had a little bit of fun! :)                               ☠";
        private string _gameOverBoxMessage_13 = "                      ☠  This small project was created during my studies at the university.  ☠";
        private string _gameOverBoxMessage_14 = "                      ☠  In the ReadMe file, you’ll find information on how to change the code☠";
        private string _gameOverBoxMessage_15 = "                      ☠  to create your own quiz questions and answers.                       ☠";
        private string _gameOverBoxContact_16 = "                      ☠  Contact info:                                                        ☠";
        private string _gameOverBoxContact_17 = "                      ☠  Instagram: https://www.instagram.com/isor_gamedev                    ☠";
        private string _gameOverBoxContact_18 = "                      ☠  GitHub: https://github.com/Isor1993                                  ☠";
        private string _gameOverBoxContact_19 = "                      ☠  E-mail: IsorDev@email.de                                             ☠";
        private string _gameOverBox_20 = "                      ☠                               Your Score                              ☠";
        private string _gameOverBox_21 = "                      ☠                                   ||                                  ☠";
        private string _gameOverBox_22 = "                      ☠                                   ||                                  ☠";
        private string _gameOverBoxLineBottom_23 = "                      ╚☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠  VV☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ☠ ╝";
        private string _gameOverScore_24 = "                                                         ";
        private List<string> _gameOver = new List<string> { _gameOver_1, _gameOver_2, _gameOver_3, _gameOver_4, _gameOver_5, _gameOver_6, _gameOver_7, _gameOver_8 };

        /// <summary>
        /// Displays the tutorial screen and waits for player input to continue.
        /// </summary>
        public void ScreenTutorial()
        {
            ScreenTutorialHead();
            ScreenTutorialBuildBody();
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Displays the animated start screen with title and author text.
        /// </summary>
        public void ScreenStart()
        {
            Console.ForegroundColor = ConsoleColor.White;
            ScreenStart_1();
            ScreenStart_2();
            Console.WriteLine(" ");
            ScreenStart_3();
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Displays the victory screen after the player successfully escapes.
        /// </summary>
        public void ScreenWin()
        {
            ScreenWinHead();
            ScreenWinBuildBody();
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Displays the game over screen when the player dies or fails.
        /// </summary>
        public void ScreenGameOver()
        {
            ScreenGameOverHead();
            ScreenGameOverBuildBody();
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Prints 1. element with delay for animation feeling.
        /// </summary>
        private void ScreenStart_1()
        {
            for (int index = 0; index < _escapeRoom.Count; index++)
            {
                Console.WriteLine($"{_escapeRoom[index]}");
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// Prints 2. element with delay for animation feeling.
        /// </summary>
        private void ScreenStart_2()
        {
            for (int index = 0; index < _madeBy.Count; index++)
            {
                Console.WriteLine($"{_madeBy[index]}");
                Thread.Sleep(280);
            }
        }

        /// <summary>
        /// Prints 3. element with delay for animation feeling.
        /// </summary>
        private void ScreenStart_3()
        {
            for (int index = 0; index < _name.Count; index++)
            {
                Console.WriteLine($"{_name[index]}");
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Prints the top header lines of the tutorial ASCII title.
        /// </summary>
        private void ScreenTutorialHead()
        {
            for (int index = 0; index < _tutorial.Count; index++)
            {
                Console.WriteLine($"{_tutorial[index]}");
            }
        }

        /// <summary>
        /// Builds and prints the tutorial body section with controls and symbol legends.
        /// </summary>
        private void ScreenTutorialBuildBody()
        {
            if (_deps == null)
            {
                return;
            }
            _playerSymbol = _deps.Symbol.PlayerSymbol;
            _keyFragmentSymbol = _deps.Symbol.KeyFragmentSymbol;
            _openDoor = _deps.Symbol.OpenDoorVerticalSymbol;
            _npcSymbol = _deps.Symbol.QuestSymbol;
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine($"{_tutorialBoxLineTop}");
            Console.WriteLine($"{_tutorialIntro}");
            Console.WriteLine($"{_tutorialBoxSpace}");
            Console.WriteLine($"{_tutorial_4}");
            Console.WriteLine($"{_tutorialBoxSpace}");
            Console.WriteLine($"{_tutorial_5}");
            Console.WriteLine($"{_tutorial_6}");
            Console.WriteLine($"{_tutorialBoxSpace}");
            Console.WriteLine($"{_tutorial_7}");
            Console.WriteLine($"{_tutorial_8}");
            Console.WriteLine($"{_tutorial_9}");
            Console.WriteLine($"{_tutorial_10}");
            Console.WriteLine($"{_tutorial_11}");
            Console.WriteLine($"                         ║ {_playerSymbol}{_tutorial_12}");
            Console.WriteLine($"                         ║ {_keyFragmentSymbol}{_tutorial_13}");
            Console.WriteLine($"                         ║ {_openDoor}{_tutorial_14}");
            Console.WriteLine($"                         ║ {_npcSymbol}{_tutorial_15}");
            Console.WriteLine($"{_tutorialBoxLineBottom}");
        }

        /// <summary>
        /// Prints the ASCII "You Won" header.
        /// </summary>
        private void ScreenWinHead()
        {
            for (int index = 0; index < _youWon.Count; index++)
            {
                Console.WriteLine($"{_youWon[index]}");
            }
        }

        /// <summary>
        /// Builds and prints the victory screen body with contact info and final score.
        /// </summary>
        private void ScreenWinBuildBody()
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine($"{_youWonBoxLineTop_9}");
            Console.WriteLine($"{_youWonBoxmessage_10}");
            Console.WriteLine($"{_youWonBoxSpace_11}");
            Console.WriteLine($"{_youWonBoxmessage_12}");
            Console.WriteLine($"{_youWonBoxmessage_13}");
            Console.WriteLine($"{_youWonBoxmessage_14}");
            Console.WriteLine($"{_youWonBoxmessage_15}");
            Console.WriteLine($"{_youWonBoxSpace_11}");
            Console.WriteLine($"{_youWonBoxContact_16}");
            Console.WriteLine($"{_youWonBoxSpace_11}");
            Console.WriteLine($"{_youWonBoxContact_17}");
            Console.WriteLine($"{_youWonBoxContact_18}");
            Console.WriteLine($"{_youWonBoxContact_19}");
            Console.WriteLine($"{_youWonBox_20}");
            Console.WriteLine($"{_youWonBox_21}");
            Console.WriteLine($"{_youWonBox_22}");
            Console.WriteLine($"{_youWonBoxLineBottom_23}");
            Console.WriteLine($"{_youWonScore_24} {_deps.Inventory.Score}");
        }

        /// <summary>
        /// Prints the ASCII "Game Over" header.
        /// </summary>
        private void ScreenGameOverHead()
        {
            for (int index = 0; index < _gameOver.Count; index++)
            {
                Console.WriteLine($"{_gameOver[index]}");
            }
        }

        /// <summary>
        /// Builds and prints the game over screen body with messages and score.
        /// </summary>
        private void ScreenGameOverBuildBody()
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine($"{_GameOverBoxLineTop_9}");
            Console.WriteLine($"{_gameOverBoxMessage_10}");
            Console.WriteLine($"{_gameOverBoxSpace_11}");
            Console.WriteLine($"{_gameOverBoxMessage_12}");
            Console.WriteLine($"{_gameOverBoxMessage_13}");
            Console.WriteLine($"{_gameOverBoxMessage_14}");
            Console.WriteLine($"{_gameOverBoxMessage_15}");
            Console.WriteLine($"{_gameOverBoxSpace_11}");
            Console.WriteLine($"{_gameOverBoxContact_16}");
            Console.WriteLine($"{_gameOverBoxSpace_11}");
            Console.WriteLine($"{_gameOverBoxContact_17}");
            Console.WriteLine($"{_gameOverBoxContact_18}");
            Console.WriteLine($"{_gameOverBoxContact_19}");
            Console.WriteLine($"{_gameOverBox_20}");
            Console.WriteLine($"{_gameOverBox_21}");
            Console.WriteLine($"{_gameOverBox_22}");
            Console.WriteLine($"{_gameOverBoxLineBottom_23}");
            Console.WriteLine($"{_gameOverScore_24} {_deps.Inventory.Score}");
        }
    }
}