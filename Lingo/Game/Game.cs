using System;
namespace Lingo.Game
{
	public interface IGame
	{
		public string Title { get; }
		public string TitleShort { get; }

		public bool Finished { get; }
        public bool Started { get; }

        public IGameState? LastGameState { get; }

        public IGameState? Start();
		public IGameState? Resume(IGameState gameState);
        public IGameState? Resume(IGameState gameState, object response);

    }
	public interface IGameState
    {
        public string Title { get; }
        public string TitleShort { get; }
        public string Description { get; }

        public Response Response { get; }
        public string? Prompt { get; }
        public Func<IGameState, object, IGameState?> ResponseAction { get; }
    }

    public enum Response
    {
        False,
        Text,
    }
}