using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    internal class NpcManager
    {
        private readonly PrinterManager _printer;
        private readonly DiagnosticsManager _diagnostics;

        
        // List wo die neuen Npc objekte gespeichert werden mit deren Daten für einmalig laden später
        public List<NpcData> NpcList { get; private set; } = new List<NpcData>();

        /// <summary>
        /// Lädt beim erstellen automatisch datei namens npc_question.txt
        /// </summary>
        public NpcManager(PrinterManager printer,DiagnosticsManager diagnostics)
        {
            _printer= printer;
            _diagnostics= diagnostics;

            LoadNpcDataFromFile("npc_questions.txt");
        }
                
        /// <summary>
        /// Lädt alle NPC-Daten aus einer externen Textdatei in die interne Liste.
        /// Format pro Zeile: Name;Frage;Antwort;Belohnung Key;Belohnung Points
        /// </summary>
        /// <param name="filePath">Enter file Path</param>
        private void LoadNpcDataFromFile(string filePath)
        {
            // wenn datei nicht exists fehler
            if (!File.Exists(filePath))
            {
                _diagnostics.AddError("[NpcManager]: File 'npc_questions.txt' was not found!");
                return;
            }
            // Opens file and read all lines and close it again
            foreach (string line in File.ReadAllLines(filePath))
            {
                // ignore empty spaces
                if (string.IsNullOrWhiteSpace(line)) continue;
                // splits the string in parts after ; and saves it in array parts
                string[] parts = line.Split(';');
                // do it till all 5 parts are reached
                if (parts.Length < 5) continue;
                // erstellt neues NpcData Object mit den jeweiligen parts
                NpcData npc = new NpcData(parts[0], parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]));
                // speicher das Object in liste
                NpcList.Add(npc);
            }

            _diagnostics.AddCheck($"[NpcManager] Successfully loaded {NpcList.Count} Npc records.");
        }
    }

   
}
    

