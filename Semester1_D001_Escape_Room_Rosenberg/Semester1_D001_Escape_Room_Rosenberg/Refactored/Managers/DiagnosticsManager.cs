using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// Manages and stores all diagnostic messages such as errors, warnings, and system checks.
    /// Provides functionality to add, count, and print diagnostic results.
    /// </summary>
    internal class DiagnosticsManager
    {
        // Collection storing all critical error messages.
        private readonly List<string> _errors = new();
        // Collection storing all non-critical warning messages.
        private readonly List<string> _warnings = new();
        // Collection storing system or validation checks (informational messages).
        private readonly List<string> _checks = new();
        // Collection storing system or validation checks (informational messages).
        private readonly List<string> _exception = new();

        /// <summary>
        /// Adds an exception message to the error log.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        public void AddException(string message) => _exception.Add($"[Exception] {message}");
        /// <summary>
        /// Adds an error message to the error log.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        public void AddError(string message) => _errors.Add($"[ERROR] {message}");
        /// <summary>
        /// Adds a warning message to the warning log.
        /// </summary>
        /// <param name="message">The message describing the warning.</param>
        public void AddWarning(string message) => _warnings.Add($"[WARNING] {message}");
        /// <summary>
        /// Adds a check message to the system check log.
        /// </summary>
        /// <param name="message">The message describing the successful or verified check.</param>
        public void AddCheck(string message) => _checks.Add($"[CHECK] {message}");

        /// <summary>
        /// Prints all collected diagnostics to the console using the PrinterManager.
        /// </summary>
        /// <param name="printer">The PrinterManager instance responsible for console output.</param>
        public void PrintAll(PrintManager printer)
        {
            //  Print all error messages first.
            foreach (string msg in _errors) printer.PrintLine(msg);
            //// Finally, print all exception checks.
            foreach (string msg in _exception) printer.PrintLine(msg);
            // Then print all warnings.
            foreach (string msg in _warnings) printer.PrintLine(msg);
            // Finally, print all system checks.
            foreach (string msg in _checks) printer.PrintLine(msg);
            printer.PrintLine("--- END OF DIAGNOSTICS ---");
        }
        /// <summary>
        /// Gets the total number of recorded exception messages.
        /// </summary>
        public int ExceptionCount => _exception.Count;
        /// <summary>
        /// Gets the total number of recorded error messages.
        /// </summary>
        public int ErrorCount => _errors.Count;
        /// <summary>
        /// Gets the total number of recorded warning messages.
        /// </summary>
        public int WarningCount => _warnings.Count;
        /// <summary>
        /// Gets the total number of recorded system check messages.
        /// </summary>
        public int CheckCount => _checks.Count;
        /// <summary>
        /// Prints a summary of all diagnostic counts (errors, warnings, checks).
        /// </summary>
        /// <param name="printer">The PrinterManager instance responsible for console output.</param>
        public void PrintAllCount(PrintManager printer)
        {
            printer.PrintLine($"Total Errors: {ErrorCount}");
            printer.PrintLine($"Total Exceptions: {ExceptionCount}");
            printer.PrintLine($"Total Warnings: {WarningCount}");
            printer.PrintLine($"Total Checks: {CheckCount}");
        }

        /// <summary>
        /// Clears all stored diagnostic entries, including errors, warnings,
        /// checks, and exceptions, resetting the diagnostics state completely.
        /// </summary>
        /// <remarks>
        /// This method provides a full reset of the diagnostics system, removing
        /// all accumulated log entries. It should typically be called when starting
        /// a new test, scene, or game session to ensure a clean diagnostic state.
        /// </remarks>
        public void ClearAll()
        {
            _errors.Clear();
            _warnings.Clear();
            _checks.Clear();
            _exception.Clear();
        }
    }
}
