using System;

namespace Lingo.Game
{
    public class WordGame : IGame
    {
        public string Title { get; } = "Lingo - Word Game";
        public string TitleShort { get; } = "Word Game";

        public string Word { get; private set; }
        public int CurrentAttempt { get; private set; } = 0;
        public int MaxAttempts { get; private set; } = 4;
        public bool Finished { get => TestFinished(); }
        public bool Started { get; private set; } = false;

        public IGameState? LastGameState { get; private set; }

        public List<WordAttempt> Attempts { get; private set; } = new List<WordAttempt>();

        public WordGame(string word, int maxAttempts)
        {
            Word = word.ToLower();
            MaxAttempts = maxAttempts;
        }

        public IGameState? Start()
        {
            if (Finished)
                return null;
            if (Started && LastGameState != null)
                return LastGameState;

            IGameState gameState = new WordGameState(Title, TitleShort, "", Response.Text, Resume);

            return null;
        }

        public IGameState? Resume(IGameState gameState)
        {
            throw new NotImplementedException();
        }
        public IGameState? Resume(IGameState gameState, object gameResponse)
        {
            throw new NotImplementedException();
        }

        public void TryWord(string word)
        {
            if (Finished)
                return;

            word = word.ToLower();

            WordAttempt currentWordAttempt = new WordAttempt(this);
            Attempts.Add(currentWordAttempt);

            currentWordAttempt.TryWord(word);

            CurrentAttempt++;
        }

        

        private bool TestFinished()
        {
            if (CurrentAttempt > MaxAttempts)
                 return true;

            return false;
        }
    }

    public class WordGameState : IGameState
    {
        public string Title { get; private set; }
        public string TitleShort { get; private set; }
        public string Description { get; private set; }

        public Response Response { get; private set; }
        public string? Prompt { get; private set; }
        public Func<IGameState, object, IGameState?> ResponseAction { get; private set; }

        public WordGameState(string title, string titleShort, string description, Response response, Func<IGameState, object, IGameState?> responseAction)
        {
            Title = title;
            TitleShort = titleShort;
            Description = description;
            Response = response;

            ResponseAction = responseAction;
        }
    }

    public class WordAttempt
    {
        public WordAttempt(WordGame wordGame)
        {
            WordGame = wordGame;

            for (int i = 0; i < CorrectWord.Length; i++)
                LetterAttempts.Add(new LetterAttempt());
        }

        public bool Finished { get; private set; } = false;
        public WordGame WordGame { get; private set; }
        public string CorrectWord { get => WordGame.Word; }

        public List<LetterAttempt> LetterAttempts { get; private set; } = new List<LetterAttempt>();

        public void TryWord(string word)
        {
            List<char?> correctLettersProcessing = CorrectWord.Select(c => (char?)c).ToList();

            FillLetterAttempt(word);

            TestForCorrectPlace(ref correctLettersProcessing);
            TestForWrongPlace(ref correctLettersProcessing);
            TestForNotInWord(ref correctLettersProcessing);
        }

        internal void FillLetterAttempt(string word)
        {
            List<char?> wordLetters = word.Select(c => (char?)c).ToList();

            for (int i = 0; i < LetterAttempts.Count; i++)
            {
                if (!(i < wordLetters.Count))
                    break;

                LetterAttempt letterAttempt = LetterAttempts[i];
                char? wordLetter = wordLetters[i];

                letterAttempt.Letter = wordLetter;
            }
        }

        internal void TestForCorrectPlace(ref List<char?> correctLettersProcessing)
        {
            for (int i = 0; i < LetterAttempts.Count; i++)
            {
                if (!(i < correctLettersProcessing.Count))
                    break;

                LetterAttempt letterAttempt = LetterAttempts[i];
                char? letterToProcess = correctLettersProcessing[i];

                if (letterAttempt.Letter == letterToProcess)
                {
                    letterAttempt.Status = LetterStatus.CorrectPlace;
                    correctLettersProcessing[i] = null;
                };
            }
        }
        internal void TestForWrongPlace(ref List<char?> correctLettersToProcess)
        {
            for (int i = 0; i < LetterAttempts.Count; i++)
            {
                LetterAttempt letterAttempt = LetterAttempts[i];

                if (correctLettersToProcess.Contains(letterAttempt.Letter))
                {
                    letterAttempt.Status = LetterStatus.WrongPlace;

                    int matchedIndex = correctLettersToProcess.FindIndex(item => item == letterAttempt.Letter);
                    correctLettersToProcess[matchedIndex] = null;
                };
            }
        }
        internal void TestForNotInWord(ref List<char?> correctLettersToProcess)
        {
            for (int i = 0; i < LetterAttempts.Count; i++)
            {
                LetterAttempt letterAttempt = LetterAttempts[i];

                if (letterAttempt.Letter == null)
                    break;

                if (!WordGame.Word.ToList().Contains((char)letterAttempt.Letter))
                {
                    letterAttempt.Status = LetterStatus.NotInWord;
                };
            }
        }
    }
    public class LetterAttempt
    {
        public char? Letter { get; internal set; }
        public LetterStatus Status { get; internal set; } = LetterStatus.NotProcesed;
    }
    public enum LetterStatus
    {
        NotProcesed,
        CorrectPlace,
        WrongPlace,
        NotInWord,
    }
}

