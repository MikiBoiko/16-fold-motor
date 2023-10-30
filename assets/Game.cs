using Fold.Motor.Constructors.Builders;
using Fold.Motor.Resources.Request;
using Fold.Motor.Resources.Response;
using Fold.Motor.Model;
using Fold.Motor.Resources.Resolution;

namespace Fold.Motor;

public enum CardColor
{
    red = 0,
    black = 1,
    none = 2,
    both = 3
};

public class Game
{
    #region Attributes and constructor
    public static readonly int PLAYER_COUNT = 2;

    // Players (red and black)
    public readonly Player[] players;

    // Board (contains the cards)
    private Board _board;

    // Current state
    private int _turnCount;
    private int _turn;
    public int GameMove => _turnCount / 2;
    public Player CurrentPlayer => players[_turn];
    public Player WaitingPlayer => players[(_turn + 1) % 2];

    private GameStartedResponse? _startedResponse;
    public bool GameStarted => _startedResponse != null;
    public delegate void OnStartGame(GameStartedResponse initializationResolution);
    public OnStartGame OnGameStarted;

    private GameEndedResponse? _endedResponse;
    public bool GameEnded => _endedResponse != null;
    public delegate void OnEndGame(GameEndedResponse resolution);
    private OnEndGame OnGameEnded;

    public Game(
        double interval,
        double increment,
        OnStartGame onGameStarted,
        OnEndGame onGameEnded
    )
    {
        players = new Player[PLAYER_COUNT];

        players[0] = new Player(
            CardColor.red,
            "Red Player",
            interval,
            increment,
            () => SetGameEnded(
                new GameEndedResponse
                {
                    Way = GameEndedResponse.Reason.TIME,
                    Result = CardColor.black
                }
            )
        );

        players[1] = new Player(
            CardColor.black,
            "Black Player",
            interval,
            increment,
            () => SetGameEnded(
                new GameEndedResponse
                {
                    Way = GameEndedResponse.Reason.TIME,
                    Result = CardColor.red
                }
            )
        );

        _board = new Board();

        OnGameStarted = onGameStarted;
        OnGameEnded = onGameEnded;
    }
    #endregion

    #region Initialization and finalization
    // After this method is done, the game is ready to begin.
    // 1. Uses the game seed to generate a pseudo-random class.
    // 2. Pick who starts.
    // 3. Initializes the board with random initial values and places them in the
    //    board player's starting position.
    public void Start()
    {
        // All random events only happens in the initialization 
        Random random = new Random();

        // Pick who starts
        _turnCount = 0;
        _turn = random.Next(PLAYER_COUNT);

        // Sets up the board with a deck. 
        List<int> cardValues = _board.InitializeBoard(players[0], players[1], random.Next(), new Model.Decks.FrenchDeck(random.Next()));

        SetGameStarted(
            new GameStartedResponse
            {
                Turn = (CardColor)_turn,
                PickedCards = cardValues
            }
        );
    }

    // Restarts an already finished game
    public void Restart(bool sameCards)
    {
        if (!GameStarted)
            throw new GameNotStartedException();

        if (!GameEnded)
            throw new GameDidNotEndException();

        // unset resolution
        _endedResponse = null;

        // calculate starting turn
        _turn = _turn - _turnCount; // TODO check with tests
        _turnCount = 0;

        // restart players
        players[0].Restart();
        players[1].Restart();

        // reinitialize board
        Random random = new Random();
        List<int> cardValues = _board.InitializeBoard(
            players[0],
            players[1],
            positionShuffleSeed: random.Next(),
            deck: sameCards ? null : new Model.Decks.FrenchDeck(random.Next())
        );

        SetGameStarted(
            new GameStartedResponse
            {
                Turn = (CardColor)_turn,
                PickedCards = cardValues
            }
        );
    }

    public void SetGameStarted(GameStartedResponse response)
    {
        if (GameEnded)
            throw new GameEndedException();

        _startedResponse = response;
        OnGameStarted.Invoke(response);
    }

    public void SetGameEnded(GameEndedResponse response)
    {
        if (GameEnded)
            throw new GameEndedException();

        _endedResponse = response;
        OnGameEnded.Invoke(response);
    }
    #endregion

    #region Actions and decisions
    public ActionResolution DoAction(CardColor playerColor, ActionRequest actionRequest)
    {
        if (!GameStarted)
            throw new GameNotStartedException();

        if (GameEnded)
            throw new GameEndedException();

        Player turnPlayer = players[_turn];

        // Check that it's the players turn
        if (turnPlayer.color != playerColor)
            throw new NotPlayersTurnException();

        Model.Action action = ActionBuilder.Build(actionRequest);

        ActionResolution resolution = action.DoAction(turnPlayer, _board);
        resolution.Request = actionRequest;

        if (resolution.GameEndedResponse != null)
            SetGameEnded(resolution.GameEndedResponse);
        else resolution.AllResponse.TimeLeft = NextTurn(resolution);

        return resolution;
    }

    public DecisionResolution DoDecision(CardColor playerColor, DecisionRequest decisionRequest)
    {
        if (!GameStarted)
            throw new GameNotStartedException();

        if (GameEnded)
            throw new GameEndedException();

        Decision decision = DecisionBuilder.Build(decisionRequest);

        DecisionResolution resolution = decision.DoDecision(players[(int)playerColor], this);
        resolution.Request = decisionRequest;

        return resolution;
    }
    #endregion

    #region Class methods
    public double NextTurn(ActionResolution lastMoveActionResolution)
    {
        double timeLeft = players[_turn].EndTurn(lastMoveActionResolution);
        _turnCount++;
        _turn++;
        _turn %= 2;
        players[_turn].StartTurn();
        return timeLeft;
    }

    public Player OtherPlayer(Player player) => players[((int)player.color + 1) % 2];
    #endregion

    #region State
    public class State
    {
        public int Turn { set; get; }
        public int TurnCount { set; get; }
        public GameStartedResponse? StartedResponse { set; get; }
        public GameEndedResponse? EndedResponse { set; get; }
        public Player.State[]? PlayerStates { set; get; }
        public Dictionary<string, Card.CardState?>? BoardState { set; get; }
        public DateTime TimeStamp { get; set; }
        public ActionResponse? LastActionResponse { get; set; }
    }

    public State GetState()
    {
        return new State
        {
            Turn = _turn,
            TurnCount = _turnCount,
            StartedResponse = _startedResponse ?? null,
            EndedResponse = _endedResponse ?? null,
            PlayerStates = new List<Player.State>
            {
                players[0].GetState(),
                players[1].GetState()
            }.ToArray(),
            BoardState = _board.GetState(),
            TimeStamp = DateTime.Now,
            LastActionResponse = WaitingPlayer.LastActionResolution == null
                ? null
                : WaitingPlayer.LastActionResolution.AllResponse ?? null
        };
    }
    #endregion
}

#region Game exceptions
public class NotPlayersTurnException : Exception { }
public class PlayerIdNotFoundException : Exception { }
public class GameNotStartedException : Exception { }
public class GameEndedException : Exception { }
public class GameDidNotEndException : Exception { }
#endregion

#region Board Exceptions
public class NotAdjecentException : Exception { }
public class NoCardFoundException : Exception { }
public class CardNotHiddenException : Exception { }
public class PositionOccupiedException : Exception { }
public class OutOfBoardPositionException : Exception { }
public class CardNotCloseToWinningException : Exception { } // TODO : probably refactor
#endregion

