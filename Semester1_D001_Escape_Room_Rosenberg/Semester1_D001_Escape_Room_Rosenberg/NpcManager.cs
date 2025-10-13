using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// Manages the loading and storage of NPC data from an external text file.
    /// Each NPC record contains dialogue information and potential rewards.
    /// </summary>
    internal class NpcManager
    {
        // Reference to the printer manager for console output.
        private readonly PrinterManager _printer;
        // Reference to the diagnostics manager for logging messages and errors.
        private readonly DiagnosticsManager _diagnostics;
        //
        private readonly Random _random;
        NpcData _currentNpc;
        public NpcData CurrentNpc{ get => _currentNpc; }
        // List where all loaded NPC objects are stored for later access.
        public List<NpcData> NpcList { get; private set; } = new List<NpcData>();
        // List where curent loaded NPC objects are stored for later access.
        public List<NpcData> CurrentNpcList { get; private set; } = new List<NpcData>();

        /// <summary>
        /// Initializes a new instance of the NpcManager class.
        /// Automatically loads NPC data from the file "npc_questions.txt" upon creation.
        /// </summary>
        /// <param name="printer">Reference to the PrinterManager instance.</param>
        /// <param name="diagnostics">Reference to the DiagnosticsManager instance.</param>
        public NpcManager(PrinterManager printer, DiagnosticsManager diagnostics)
        {
            _printer = printer;
            _diagnostics = diagnostics;


            LoadNpcDataFromFile("npc_questions.txt");
        }

        /// <summary>
        /// Loads all NPC data from an external text file into the internal list.
        /// Expected format per line: Name;Question;Answer;RewardKey;RewardPoints
        /// </summary>
        /// <param name="filePath">The path to the NPC data file.</param>
        private void LoadNpcDataFromFile(string filePath)
        {
            // Check if the file exists; if not, log an error and stop loading.
            if (!File.Exists(filePath))
            {
                _diagnostics.AddError($"{nameof(NpcManager)}: File 'npc_questions.txt' was not found!");
                return;
            }
            // Open the file, read all lines, and process each one.
            foreach (string line in File.ReadAllLines(filePath))
            {
                // Skip empty or whitespace-only lines.
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Split the line into parts separated by ';'.
                string[] parts = line.Split(';');
                // Validate that the line contains all 5 required parts.
                if (parts.Length < 5) continue;

                // Create a new NPC data object using the parsed values.
                NpcData npc = new NpcData(parts[0], parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]));
                // Add the NPC object to the list.
                NpcList.Add(npc);
            }
            // Log a success message through the diagnostics manager.
            _diagnostics.AddCheck($"{nameof(NpcManager)}: Successfully loaded {NpcList.Count} Npc records.");
        }

        public NpcData? GetRandomNpc()
        {
            if (NpcList.Count == 0)
            {
                _diagnostics.AddWarning($"{nameof(NpcManager)}: Npc list empty -> No NPC data loaded");
                return null;
            }
            int index = _random.Next(0, NpcList.Count);
            return NpcList[index];

        }

        public NpcData? GetCurrentNpc()
        {
            
            NpcData? randomNpc = GetRandomNpc();
            if(randomNpc == null)
            {
                return null;
            }
            _currentNpc = randomNpc;
            return _currentNpc;
        }
        public string GetNameFromNpc(string name)
        {
            if (_currentNpc == null)
            {
                _diagnostics.AddWarning($"{nameof(NpcManager)}: No current NPC selected-> No name.");
                return string .Empty;
            }
            return _currentNpc.Name;
        }
        public string GetQuestionFromNpc(string question)
        {
            if (_currentNpc == null)
            {
                _diagnostics.AddWarning($"{nameof(NpcManager)}: No current NPC selected-> No question.");
                return string.Empty;
            }
            return _currentNpc.Question;
        }
        public string GetCorrectAnswerFromNpc(string answer)
        {
            if (_currentNpc == null)
            {
                _diagnostics.AddWarning($"{nameof(NpcManager)}: No current NPC selected-> No answer.");
                return string.Empty;
            }
            return _currentNpc.CorrectAnswer;

        }
        public int GetRewardKeyFromNpc(int rewardKeyfragment)
        {
            if (_currentNpc == null)
            {
                _diagnostics.AddWarning($"{nameof(NpcManager)}: No current NPC selected-> KeyFragment = 0.");
                return 0;
            }
            return _currentNpc.RewardKeyFragment;
        }

        public int GetRewardPointsFromNpc(int rewardPoints)
        {
            if (_currentNpc == null)
            {
                _diagnostics.AddWarning($"{nameof(NpcManager)}: No current NPC selected-> Points = 0.");
                return 0;
            }
            return _currentNpc.RewardPoints;
        }
    }
}


