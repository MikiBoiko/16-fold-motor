namespace Fold.Motor.Model; 

public abstract class Deck {
    protected readonly int randomSeed;

    public Deck(int randomSeed) {
        this.randomSeed = randomSeed;
    }

    public abstract List<int> GenerateInitialValues(bool hasJokers, int playerStartingCardCount);
}