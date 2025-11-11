/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : InteractionManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles all player interactions with in-game objects, such as NPCs, keys,
* and doors. Coordinates between game logic, UI, inventory, and level systems.
* Each interaction triggers feedback, sound, and diagnostics logging.
*
* Responsibilities:
* - Process player interactions (NPCs, keys, doors)
* - Update UI and inventory according to interaction outcomes
* - Log all results via DiagnosticsManager
* - Manage game progression (unlock doors, advance levels)
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using System.Threading.Tasks.Sources;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Manages all player interactions with objects on the game board,
    /// including NPC dialogues, key fragment pickups, and door unlocking.
    /// </summary>
    /// <remarks>
    /// The <see cref="InteractionManager"/> determines the type of object the player interacts with
    /// and delegates handling to specific methods. It updates the HUD, plays audio feedback,
    /// modifies the player inventory, and logs results to the diagnostics system.
    /// </remarks>
    internal class InteractionManager
    {
        // === Dependencies ===
        private readonly InteractionManagerDependencies _deps;

        // === Fields ===

        // === Handle Key ===
        private string _keyMessage = " You collected a Key Fragment";

        private string _keyInfobox = "Key Fragment ";
        private string _system = "System";

        // === Handle Door ===
        private string _doorNotOpenMessage = "You need more Key Fragments";

        private string _doorNotOpenInfobox = "Required Key Fragments ";
        private string _doorOpenMessage = " Door is open now. You won the Level";
        private string _doorOpenInfobox = "Door is open";

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionManager"/> class.
        /// </summary>
        /// <param name="interactionManagerDependencies">
        /// Container providing references to all required systems:
        /// UI, inventory, game board, diagnostics, door, and level management.
        /// </param>
        public InteractionManager(InteractionManagerDependencies interactionManagerDependencies)
        {
            _deps = interactionManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Determines the object type at the player’s target position and
        /// routes the call to the appropriate interaction method.
        /// </summary>
        /// <param name="targetPosition">Tuple containing Y and X coordinates to check.</param>
        public void InteractionHandler((int y, int x) targetPosition)
        {
            if (_deps.GameBoard == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(InteractionHandler)}: GameBoardArray reference missing!");
                return;
            }
            else if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(InteractionHandler)}: GameBoard reference missing!");
                return;
            }

            TileType tile = _deps.GameBoard.GameBoardArray[targetPosition.y, targetPosition.x];

            switch (tile)
            {
                case TileType.Key:
                    HandleKey(targetPosition);
                    break;

                case TileType.Npc:
                    HandleNpc(targetPosition);
                    break;

                case TileType.Door:
                    HandleDoor(targetPosition);
                    break;

                default:
                    _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(InteractionHandler)}: No interaction at ({targetPosition.y},{targetPosition.x}) [{tile}].");
                    break;
            }
        }

        /// <summary>
        /// Reads the player’s key input (1–3) and compares the selected answer
        /// to the correct one provided by the NPC dialogue.
        /// </summary>
        /// <param name="answer_1">First answer option.</param>
        /// <param name="answer_2">Second answer option.</param>
        /// <param name="answer_3">Third answer option.</param>
        /// <param name="correctAnswer">The correct answer text to validate against.</param>
        /// <returns>
        /// <see langword="true"/> if the player chose the correct answer;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private bool PlayerChooseAnswer(string answer_1, string answer_2, string answer_3, string correctAnswer)
        {
            string chosen = string.Empty;
            while (true)
            {
                char input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        chosen = answer_1;
                        break;

                    case '2':
                        chosen = answer_2;
                        break;

                    case '3':
                        chosen = answer_3;
                        break;

                    default:
                        continue;
                }
                break;
            }
            bool correct = chosen == correctAnswer;
            return correct;
        }

        /// <summary>
        /// Handles the player’s interaction with an NPC at the specified board position.
        /// </summary>
        /// <param name="targetPosition">Tuple containing Y and X coordinates of the NPC.</param>
        /// <remarks>
        /// Displays NPC dialogue, plays feedback sounds, processes the player’s answer,
        /// updates the HUD, modifies the inventory, and logs the outcome.
        /// </remarks>
        private void HandleNpc((int y, int x) targetPosition)
        {
            if (_deps.Npc == null || _deps.UI == null || _deps.Symbol == null || _deps.Inventory == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: Missing dependency.");
                return;
            }

            NpcInstance? npc = _deps.Npc.GetNpcAt(targetPosition);
            if (npc == null)
            {
                _deps.Diagnostic.AddWarning($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: No NPC found at ({targetPosition.y},{targetPosition.x}).");
                return;
            }

            npc.Activate();

            if (npc.Dialog == null || npc.Dialog.AnswerGroups == null || npc.Dialog.AnswerGroups.Count == 0)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: NPC '{npc.Meta.Name}' has no valid dialog data.");
                npc.Deactivate();
                return;
            }

            // TODO: Replace with multithreading maybe in future
            Console.Beep(400, 250);
            Console.Beep(450, 300);

            if (!npc.HasInteracted)
            {
                npc.MarkAsInteracted();

                (string answer_1, string answer_2, string answer_3) = npc.Dialog.AnswerGroups[0];

                npc.Dialog.CorrectAnswer = answer_1;

                List<string> answers = new List<string> { answer_1, answer_2, answer_3 };

                answers = _deps.Random.GetRandomElements(answers, answers.Count);

                _deps.UI.FillUpBottomHud(npc.Meta.Name, "", npc.Dialog.Question, _deps.Symbol.KeyFragmentSymbol, npc.Reward.KeyFragment, answers[0], answers[1], answers[2]);

                _deps.UI.PrintBottomHud();

                bool correct = PlayerChooseAnswer(answers[0], answers[1], answers[2], npc.Dialog.CorrectAnswer);

                if (correct)
                {
                    _deps.Inventory.AddScorePoints(npc.Reward.Points);

                    _deps.Inventory.AddKeyFragment(npc.Reward.KeyFragment);

                    _deps.UI.BuildTopHud();

                    _deps.UI.FillUpBottomHud(_system, "You received ", "Your last answer was correct!", _deps.Symbol.KeyFragmentSymbol, npc.Reward.KeyFragment, "", "", "");

                    _deps.UI.PrintBottomHud();

                    _deps.Print.PrintNpc(npc.Meta.Position,correct);

                    _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: Correct answer by player at ({targetPosition.y},{targetPosition.x}).");
                }
                else
                {
                    PlayerInstance? player = _deps.GameObject.Player;

                    player?.LoseLife();

                    _deps.UI.BuildTopHud();

                    _deps.UI.FillUpBottomHud(_system, "You lost ", "Your last answer was wrong!", _deps.Symbol.HearthSymbol, 1, "", "", "");

                    _deps.UI.PrintBottomHud();

                    _deps.Print.PrintNpc(npc.Meta.Position,correct);

                    _deps.Diagnostic.AddWarning($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: Wrong answer by player at ({targetPosition.y},{targetPosition.x}).");
                }
                
            }
            else
            {              
                _deps.UI.FillUpBottomHud(_system, "", "This NPC doesn't want to talk to you", ' ', "", "", "");

                _deps.UI.PrintBottomHud();
            }
            
            
            npc.Deactivate();
        }

        /// <summary>
        /// Handles the player’s interaction with a key fragment at the specified position.
        /// </summary>
        /// <param name="targetPosition">Tuple containing Y and X coordinates of the key object.</param>
        /// <remarks>
        /// Updates the player’s inventory and HUD, removes the key object from the board,
        /// and plays a pickup sound for feedback.
        /// </remarks>
        private void HandleKey((int y, int x) targetPosition)
        {
            if (_deps.GameObject == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleKey)}: Missing {nameof(GameObjectManager)} reference.");
                return;
            }
            if (_deps.Inventory == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleKey)}: Missing {nameof(InventoryManager)} reference.");
                return;
            }
            if (_deps.UI == null || _deps.Symbol == null)
            {
                _deps.Diagnostic.AddWarning($"{nameof(InteractionManager)}.{nameof(HandleKey)}: UI or SymbolsManager missing.");
            }

            // === RETRIEVE KEY OBJECT FROM POSITION ===
            KeyFragmentInstance? key = _deps.GameObject.GetObject<KeyFragmentInstance>(targetPosition);

            if (key == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleKey)}: No key object found at ({targetPosition.y},{targetPosition.x}).");
                return;
            }

            _deps.Inventory.AddKeyFragment(key.Amount);

            _deps.GameObject.RemoveObject(targetPosition);

            _deps.Print.PrintTile(targetPosition, _deps.Symbol.EmptySymbol);

            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(HandleKey)}: Picked up key at ({targetPosition.y},{targetPosition.x}).");

            _deps.UI?.BuildTopHud();

            _deps.UI?.FillUpBottomHud(_system, _keyInfobox, _keyMessage, key.Symbol, key.Amount, "", "", "");

            _deps.UI?.PrintBottomHud();

            // TODO: Replace with multithreading maybe in future
            Console.Beep(1000, 100);
            Console.Beep(1300, 100);
            Console.Beep(1600, 150);
        }

        /// <summary>
        /// Handles the player’s interaction with a door and checks if it can be opened.
        /// </summary>
        /// <param name="targetPosition">Tuple containing Y and X coordinates of the door object.</param>
        /// <remarks>
        /// Displays a message if not enough key fragments are available.
        /// If requirements are met, the door opens, victory feedback is shown,
        /// and the next level is initialized.
        /// </remarks>
        private void HandleDoor((int y, int x) targetPosition)
        {
            // === RETRIEVE DOOR OBJECT FROM POSITION ===
            DoorInstance? door = _deps.GameObject.GetObject<DoorInstance>(targetPosition);

            if (door == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleDoor)}: No door object found at ({targetPosition.y},{targetPosition.x}).");
                return;
            }
            if (_deps.UI == null || _deps.Symbol == null)
            {
                _deps.Diagnostic.AddError($"{nameof(HandleDoor)}: UI or SymbolsManager missing.");
                return;
            }

            int playerKeys = _deps.Inventory.KeyFragment;
            int requiredKeys = _deps.Level.RequiredKeys;

            if (playerKeys < requiredKeys)
            {
                _deps.UI.FillUpBottomHud(_system, _doorNotOpenInfobox, $"{_doorNotOpenMessage} {requiredKeys - _deps.Inventory.KeyFragment}", _deps.Symbol.KeyFragmentSymbol, requiredKeys, "", "", "");

                _deps.UI.PrintBottomHud();

                _deps.Diagnostic.AddWarning($"{nameof(InteractionManager)}.{nameof(HandleDoor)}: Door locked at ({targetPosition.y},{targetPosition.x}). Requires {requiredKeys}, player has {playerKeys}.");
                return;
            }

            door.OpenDoor();

            _deps.UI.BuildTopHud();

            _deps.UI.FillUpBottomHud(_system, _doorOpenInfobox, _doorOpenMessage, _deps.Symbol.OpenDoorVerticalSymbol, "", "", "");

            _deps.UI.PrintBottomHud();

            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(HandleDoor)}: Door opened at ({targetPosition.y},{targetPosition.x}). Used {requiredKeys} key fragments.");

            // TODO: Replace with multithreading maybe in future
            Console.Beep(880, 150);
            Console.Beep(988, 150);
            Console.Beep(1046, 250);
            Console.Beep(988, 150);
            Console.Beep(880, 150);
            Console.Beep(1174, 400);

            _deps.Level.NewLevel(_deps.Inventory.Score);
            // TODO: Replace with non-blocking delay system in future engine version
            Thread.Sleep(2000);
        }

       
    }
}