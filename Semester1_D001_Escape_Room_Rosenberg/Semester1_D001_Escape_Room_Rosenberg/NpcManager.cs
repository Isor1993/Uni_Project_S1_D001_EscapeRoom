using System;
using System.Collections.Generic;
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


        // List where all loaded NPC objects are stored for later access.
        public List<NpcData> NpcList { get; private set; } = new List<NpcData>();

        /// <summary>
        /// Initializes a new instance of the NpcManager class.
        /// Automatically loads NPC data from the file "npc_questions.txt" upon creation.
        /// </summary>
        /// <param name="printer">Reference to the PrinterManager instance.</param>
        /// <param name="diagnostics">Reference to the DiagnosticsManager instance.</param>
        public NpcManager(PrinterManager printer,DiagnosticsManager diagnostics)
        {
            _printer= printer;
            _diagnostics= diagnostics;

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
                _diagnostics.AddError("[NpcManager]: File 'npc_questions.txt' was not found!");
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
            _diagnostics.AddCheck($"[NpcManager] Successfully loaded {NpcList.Count} Npc records.");
        }
    }

   
}
    

