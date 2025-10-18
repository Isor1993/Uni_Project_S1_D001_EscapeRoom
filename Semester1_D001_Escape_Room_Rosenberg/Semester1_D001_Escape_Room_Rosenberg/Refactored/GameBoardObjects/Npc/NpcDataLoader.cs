using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    internal class NpcDataLoader
    {
        // Reference to the diagnostics manager for logging messages and errors.
        private readonly DiagnosticsManager _diagnostics;
        public NpcDataLoader(DiagnosticsManager diagnostics)
        {
            _diagnostics = diagnostics;
        }
            List<NpcData> tempNpcList;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadNpcDataFromFile(string filePath)
        {
            //
            try
            {
                //

                //
                foreach (string line in File.ReadAllLines(filePath))
                {
                    // Skip empty or whitespace-only lines.
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split the line into parts separated by ';'.
                    string[] parts = line.Split(';');
                    // Validate that the line contains all 5 required parts.
                    if (parts.Length < 7) continue;

                    // Create a new NPC data object using the parsed values.
                    //
                    NpcMetaData npcMeta = new NpcMetaData(parts[0], (0, 0));
                    //
                    NpcDialogData npcDialog = new NpcDialogData(parts[1], parts[2], new List<(string A, string B, string C)> { (parts[2], parts[3], parts[4]) });
                    //
                    NpcRewardData npcReward = new NpcRewardData(Convert.ToInt32(parts[5]), Convert.ToInt32(parts[6]));

                    // Add the NPC object to the list.
                    tempNpcList.Add(npcDialog,);
                }
                return tempNpcList;

            }         
            catch (FileNotFoundException ex)
            {
                // Datei nicht gefunden
                _diagnostics.AddEception ($"{nameof(NpcDataLoader)}: Datei nicht gefunden");
            }
            catch (DirectoryNotFoundException ex)
            {
                // Ordner existiert nicht
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Ordner existiert nicht");
            }
            catch (PathTooLongException ex)
            {
                // Pfad zu lang
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Pfad zu lang");
            }
            catch (UnauthorizedAccessException ex)
            {
                // Keine Berechtigung
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Keine Berechtigung");
            }
            catch (SecurityException ex)
            {
                // Sicherheitsfehler
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Sicherheitsfehler");
            }
            catch (IOException ex)
            {
                // Allgemeine IO-Fehler (z. B. Stream bereits geöffnet)
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Allgemeine IO-Fehler (z. B. Stream bereits geöffnet)");
            }
            catch (ArgumentNullException ex)
            {
                // Parameter null übergeben
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Parameter null übergeben");
            }
            catch (ArgumentException ex)
            {
                // Falsches Argumentformat
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Falsches Argumentformat");
            }
            catch (FormatException ex)
            {
                // Formatfehler beim Parsen etc.
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Formatfehler beim Parsen etc.");
            }
            catch (NotSupportedException ex)
            {
                // Feature/Format nicht unterstützt
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Feature/Format nicht unterstützt");
            }
            catch (OverflowException ex)
            {
                // Zahlenüberlauf o. Ä.
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: Zahlenüberlauf o. Ä.");
            }
            catch (Exception ex)
            {
                // Fallback: alle anderen
                _diagnostics.AddEception($"{nameof(NpcDataLoader)}: alle anderen");
            }



        }








        /// <summary>
        /// Loads all NPC data from an external text file into the internal list.
        /// Expected format per line: Name;Question;Answer;RewardKey;RewardPoints
        /// </summary>
        /// <param name="filePath">The path to the NPC data file.</param>
        private void LoadNpcDataFromFile_2(string filePath)
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
}
