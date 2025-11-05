using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System.Reflection.Metadata.Ecma335;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// Handles all player interactions with objects on the game board, including NPCs, keys, and doors.
    /// </summary>
    /// <remarks>
    /// The <see cref="InteractionManager"/> processes player interactions depending on the type of object  
    /// located at a specific tile. It handles NPC dialogues, item collection, and door unlocking.  
    /// Additionally, it updates the HUD and logs all events to the <see cref="DiagnosticsManager"/>.
    /// </remarks>
    internal class InteractionManager
    {
        // === Dependencies ===
        readonly InteractionManagerDependencies _deps;

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
        /// Dependency container providing references to all required systems  
        /// (UI, Inventory, GameBoard, Diagnostics, Door, Level, etc.).
        /// </param>
        public InteractionManager(InteractionManagerDependencies interactionManagerDependencies)
        {
            _deps = interactionManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Reads the player's input choice from the console and determines whether it matches the correct answer.
        /// </summary>
        /// <param name="answer_1">The first selectable answer option.</param>
        /// <param name="answer_2">The second selectable answer option.</param>
        /// <param name="answer_3">The third selectable answer option.</param>
        /// <param name="correctAnswer">The correct answer string for validation.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the player’s selection matches the correct answer; otherwise, <see langword="false"/>.
        /// </returns>
        private bool PlayerChooseAnswer(string answer_1, string answer_2, string answer_3, string correctAnswer)
        {
            string chosen = string.Empty;
            while(true)
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
        /// Handles player interaction with an NPC at a specified position.
        /// </summary>
        /// <param name="targetPosition">
        /// The (Y, X) coordinates of the NPC on the game board.
        /// </param>
        /// <remarks>
        /// Displays the NPC dialogue, processes player answers, updates HUD feedback,  
        /// and grants rewards or penalties depending on the correctness of the answer.
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

            // TODO Audio Npc Greeting
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

                    _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: Correct answer by player at ({targetPosition.y},{targetPosition.x}).");
                }
                else
                {
                    PlayerInstance? player = _deps.GameObject.Player;

                    player?.LoseLife();

                    _deps.UI.BuildTopHud();

                    _deps.UI.FillUpBottomHud(_system, "You lost ", "Your last answer was wrong!", _deps.Symbol.HearthSymbol, 1, "", "", "");

                    _deps.UI.PrintBottomHud();

                    _deps.Diagnostic.AddWarning($"{nameof(InteractionManager)}.{nameof(HandleNpc)}: Wrong answer by player at ({targetPosition.y},{targetPosition.x}).");
                }
            }
            else
            {
                _deps.UI.FillUpBottomHud(_system, "", "This NPC doesn't want to talk to you", ' ', "", "", "");

                _deps.UI.PrintBottomHud();
            }
            npc.Deactivate();
            //TODO Audio Npc Goodbye
        }

        /// <summary>
        /// Handles player interaction with a key fragment located at the given board position.
        /// </summary>
        /// <param name="targetPosition">
        /// The (Y, X) coordinates of the key object on the game board.
        /// </param>
        /// <remarks>
        /// Removes the key object, updates the player’s inventory, refreshes the HUD,  
        /// and plays a collection sound to provide feedback.
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

            //TODO Play Audio Colected Key
            Console.Beep(1000, 100);
            Console.Beep(1300, 100);
            Console.Beep(1600, 150);
        }

        /// <summary>
        /// Handles interaction with a door object and checks whether the player has enough keys to unlock it.
        /// </summary>
        /// <param name="targetPosition">
        /// The (Y, X) coordinates of the door object on the game board.
        /// </param>
        /// <remarks>
        /// If the player lacks the required number of key fragments, a warning message is displayed.  
        /// Otherwise, the door is opened, victory feedback is shown, and a new level is generated.
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

            // TODO Audio
            Console.Beep(784, 150);
            Console.Beep(880, 150);
            Console.Beep(988, 150);
            Console.Beep(1046, 250);
            Console.Beep(988, 150);
            Console.Beep(880, 150);
            Console.Beep(1174, 400);

            //TODO new Level generating
            _deps.Level.NewLevel(_deps.Inventory.Score);
            Thread.Sleep(2000);

        }

        /// <summary>
        /// Determines the type of object at the specified position and triggers the correct interaction.
        /// </summary>
        /// <param name="targetPosition">
        /// The (Y, X) coordinates on the game board that the player interacts with.
        /// </param>
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
    }
}