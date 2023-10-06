using Fold.Motor.Resources.Resolution;

namespace Fold.Motor.Model;

public class Player
{
	public readonly CardColor color;
	public readonly string username;
	public PlayerTimer Timer { private set; get; }
	public int ActionCount { private set; get; }
	public ActionResolution? LastActionResolution { private set; get; }
	public bool DidAnIllegalAction => LastActionResolution == null
			? false
			: LastActionResolution.Color != color;

	public Player(CardColor cardColor, string username, double interval, double increment, PlayerTimer.OnTimeLost onTimeLostGameResolution)
	{
		this.color = cardColor;
		this.username = username;
		this.Timer = new PlayerTimer(interval, increment, onTimeLostGameResolution);

		Restart();
	}

	public void Restart()
	{
		ActionCount = 0;
		Timer.Restart();
	}

	public void StartTurn()
	{
		if (ActionCount == 0)
			return;

		Timer.Enable();
	}

	public double EndTurn(ActionResolution lastActionResolution)
	{
		LastActionResolution = lastActionResolution;


		double timeLeft = (ActionCount > 0)
				? Timer.Disable()
				: Timer.interval;

		ActionCount++;

		return timeLeft;
	}

	public double GetTimeLeft()
	{
		return Timer.TimeLeft;
	}

	#region State
	public class State
	{
		public required string Username { get; set; }
		public required double TimeLeft { get; set; }
	}

	public State GetState()
	{
		return new State
		{
			Username = username,
			TimeLeft = Timer.GetState(ActionCount > 0)
		};
	}

	#endregion
}