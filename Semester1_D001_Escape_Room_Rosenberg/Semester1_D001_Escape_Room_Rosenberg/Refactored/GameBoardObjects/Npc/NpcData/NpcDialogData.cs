using System.Collections.Generic;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents the dialog data of a non-player character (NPC).
    /// Stores the NPC’s question, the correct answer, and all available answer options
    /// used during player interaction or quiz-style conversations.
    /// </summary>
    internal class NpcDialogData
    {
        // === Fields ===
        private readonly string _question;
        private readonly string _correctAnswer;
        private readonly List<(string A, string B, string C)> _answerGroups = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDialogData"/> class.
        /// </summary>
        /// <param name="question">The main question presented by the NPC.</param>
        /// <param name="correctAnswer">The correct answer to the question.</param>
        /// <param name="answerGroups">A list of possible answer sets, each containing three options (A, B, and C).</param>
        public NpcDialogData(string question, string correctAnswer, List<(string A, string B, string C)> answerGroups)
        {
            _question = question;
            _correctAnswer = correctAnswer;
            _answerGroups = answerGroups;
        }

        /// <summary>
        /// Gets the question that the NPC presents to the player.
        /// </summary>
        public string Question => _question;

        /// <summary>
        /// Gets the list of answer groups containing the NPC’s available choices.
        /// Each tuple represents a group of three answer options (A, B, C).
        /// </summary>
        public List<(string A, string B, string C)> AnswerGroups => _answerGroups;

        /// <summary>
        /// Gets the correct answer for the NPC’s question.
        /// </summary>
        public string CorrectAnswer => _correctAnswer;
    }
}