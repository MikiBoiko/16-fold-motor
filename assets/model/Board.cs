using Fold.Motor.Resources.Resolution;
using Fold.Motor.Resources.Response;

namespace Fold.Motor.Model;

public class Board
{
	// size count of the tiles in a board
	public static readonly int SIZE_X = 4, SIZE_Y = 7, PLAYER_STARTING_CARDS_COUNT = 8;

	// deck used to genereate initial values
	private Deck? _deck;

	// count for the cards
	public int[] PlayerCardStackCount { private set; get; } = new int[2];
	// map of the cards in the game where the key is letter(x) + y, BoardPosition format
	private Dictionary<BoardPosition, CardStack> _cards = new Dictionary<BoardPosition, CardStack>();

	// starting positions of both players
	private static readonly BoardPosition[] _playerRedStartingPositions = BoardPosition.FormatedPositionStringArrayToBoardPositionArray(
		new string[] {
			"a3", "b3", "c3", "d3",
			"a1", "b1", "c1", "d1"
		},
		PLAYER_STARTING_CARDS_COUNT
	);

	private static readonly BoardPosition[] _playerBlackStartingPositions = BoardPosition.FormatedPositionStringArrayToBoardPositionArray(
		new string[] {
			"a7", "b7", "c7", "d7",
			"a5", "b5", "c5", "d5"
		},
		PLAYER_STARTING_CARDS_COUNT
	);

	public List<int> InitializeBoard(Player playerRed, Player playerBlack, int positionShuffleSeed, Deck? deck = null, bool hasJokers = true)
	{
		_cards.Clear();

		_deck = deck ?? _deck ?? new Decks.FrenchDeck(new Random().Next());

		// generate new initial cards if a new deck is sent
		List<int> initialValues = _deck.GenerateInitialValues(hasJokers, PLAYER_STARTING_CARDS_COUNT);

		// Generate random instance with the provided seed
		Random random = new Random(positionShuffleSeed);

		// set up players
		SetUpPlayer(playerRed, _playerRedStartingPositions, new List<int>(initialValues), ref random);
		SetUpPlayer(playerBlack, _playerBlackStartingPositions, new List<int>(initialValues), ref random);

		return initialValues;
	}

	private void SetUpPlayer(Player player, BoardPosition[] positions, List<int> initialValues, ref Random random)
	{
		// set player card stack count
		PlayerCardStackCount[(int)player.color] = PLAYER_STARTING_CARDS_COUNT;

		// instantiate players
		List<CardStack> playerCards = new List<CardStack>();

		// for each initial value...
		foreach (int initialValue in initialValues)
		{
			// create a card for the player
			Cards.NumberCard playerCard = new Cards.NumberCard(player.color, initialValue);
			// add it to the cards
			playerCards.Add(new CardStack(player.color, playerCard, new List<Card>()));
		}

		// same with the joker
		Cards.JokerCard playerJokerCard = new Cards.JokerCard(player.color);
		playerCards.Add(new CardStack(player.color, playerJokerCard, new List<Card>()));

		// randomly initialize player cards into the starting positions...
		for (int i = 0; i < positions.Length; i++)
		{
			int randomIndex = random.Next(playerCards.Count);
			//Console.WriteLine(String.Format("{0} : {1}", positions[i], playerCards[randomIndex].Card.value));
			_cards.Add(positions[i], playerCards[randomIndex]);
			playerCards.RemoveAt(randomIndex);
		}
	}

	public bool IsInbound(BoardPosition at) => at.x >= 0 && at.x < SIZE_X && at.y >= 0 && at.y < SIZE_Y;

	public bool IsOccupied(BoardPosition at) => _cards.ContainsKey(at);

	public CardStack FindCardStack(BoardPosition at)
	{
		if (!IsOccupied(at))
			throw new NoCardFoundException();

		return _cards[at];
	}

	public void AddCardStack(BoardPosition at, CardStack cardStack)
	{
		if (IsOccupied(at))
			throw new PositionOccupiedException();

		PlayerCardStackCount[(int)cardStack.OwnerColor]++;
		_cards.Add(at, cardStack);
	}

	public CardStack RemoveCardStack(BoardPosition at)
	{
		if (!IsOccupied(at))
			throw new NoCardFoundException();

		PlayerCardStackCount[(int)FindCardStack(at).OwnerColor]--;
		CardStack stack = FindCardStack(at);
		_cards.Remove(at);
		return stack;
	}

	// returns a dictionary of the board positions, and returns null if the stack is hidden
	public Dictionary<BoardPosition, CardStack?> GetPosition()
	{
		Dictionary<BoardPosition, CardStack?> result = new Dictionary<BoardPosition, CardStack?>();
		foreach (KeyValuePair<BoardPosition, CardStack> cardStack in _cards)
		{
			result.Add(cardStack.Key, cardStack.Value.IsHidden ? null : cardStack.Value);
		}
		return result;
	}

	#region State
	public Dictionary<string, Card.CardState?> GetState()
	{
		Dictionary<string, Card.CardState?> cardStackStates = new();
		Console.WriteLine(_cards.Keys.Count);
		foreach (BoardPosition stackPosition in _cards.Keys)
		{
			cardStackStates.Add(stackPosition.ToString(), _cards[stackPosition].GetState());
			Console.WriteLine(stackPosition.ToString());
		}

		return cardStackStates;
	}
	#endregion
}
