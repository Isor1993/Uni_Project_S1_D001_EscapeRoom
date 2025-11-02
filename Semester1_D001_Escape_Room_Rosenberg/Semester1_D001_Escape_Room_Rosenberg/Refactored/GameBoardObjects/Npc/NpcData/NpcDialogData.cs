using System.Collections.Generic;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents the dialog configuration of a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// Stores the question, the correct answer, and possible answer sets.  
    /// Each NPC can have one main question and multiple grouped answers (A, B, C) 
    /// used for interactive or quiz-style conversations with the player.
    /// </remarks>
    internal class NpcDialogData
    {
        // === Fields ===
        private readonly string _question;
        private readonly string _correctAnswer;
        private readonly List<(string A, string B, string C)> _answerGroups = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDialogData"/> class.
        /// </summary>
        /// <param name="question">The question posed by the NPC.</param>
        /// <param name="correctAnswer">The correct answer to the question.</param>
        /// <param name="answerGroups">A list of possible answer sets (A, B, C).</param>
        /// <remarks>
        /// Each answer group typically contains three options from which the player must select one.
        /// </remarks>
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
        /// Gets all answer groups available for the NPC’s question.
        /// </summary>
        /// <remarks>
        /// Each tuple represents a set of three multiple-choice options.
        /// </remarks>
        public List<(string A, string B, string C)> AnswerGroups => _answerGroups;

        /// <summary>
        /// Gets the correct answer for the NPC’s question.
        /// </summary>
        public string CorrectAnswer => _correctAnswer;
                
    }
}