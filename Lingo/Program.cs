using Lingo.Game;


internal class Program
{
    private static void Main(string[] args)
    {
        string NEWLINE = Environment.NewLine;
        string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
        string GREEN = Console.IsOutputRedirected ? "" : "\x1b[32m";
        string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[33m";
        // https://stackoverflow.com/a/74807043

        Console.WriteLine("Hello, World!");
        IGame wordGame = new WordGame("tnetennba", 4);

        IGameState? gameState = wordGame.Start();
        while (true)
        {
            if (gameState == null)
                break;

            if (gameState is WordGameState wordGameState && wordGameState.Attempts.Count > 0)
            {
                string formatedWord = "";
                foreach (WordAttempt wordAttempt in wordGameState.Attempts)
                {
                    if (formatedWord != "")
                        formatedWord += NEWLINE;

                    foreach (LetterAttempt letter in wordAttempt.LetterAttempts)
                    {
                        if (letter.Status == LetterStatus.CorrectPlace)
                            formatedWord += $"{GREEN}{letter.Letter}{NORMAL}";
                        else if (letter.Status == LetterStatus.WrongPlace)
                            formatedWord += $"{YELLOW}{letter.Letter}{NORMAL}";
                        else
                            formatedWord += $"{letter.Letter}";
                    }
                }
                Console.WriteLine("Tried words:");
                Console.WriteLine(formatedWord);
            }

            Console.WriteLine(gameState.Prompt);
            if (gameState.Response == Response.ResponseText)
            {
                string? response = Console.ReadLine();
                gameState = gameState.ResponseAction(gameState, response);
            }
            else if (gameState.Game.Finished)
            {
                gameState = null;
            }
        }

        Console.WriteLine("Ended...");
        Console.ReadLine();
    }
}