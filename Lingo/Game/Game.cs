using System;
namespace Lingo.Game
{
	public interface IGame
	{
		public string Title { get; }
		public string TitleShort { get; }

		public IGameAction? Start();
		public IGameAction? Resume(IGameAction gameAction);
	}
	public interface IGameAction
    {
        public string Title { get; }
        public string TitleShort { get; }
		public string Description { get; }

        // public enum Type { get; }

		public Action? Action { get; }
	}

	public enum Type
	{
		Prompt,
		Finished,
	}

	public abstract class GameBase : IGame
	{
		public abstract string Title { get; }
        public abstract string TitleShort { get; }

        public abstract IGameAction? Start();
        public abstract IGameAction? Resume(IGameAction gameAction);
    }
}

