namespace Fold.Motor.Model.Decks;

public class FrenchDeck : Deck {
    public FrenchDeck(int randomSeed) : base(randomSeed) { }

    public override List<int> GenerateInitialValues(bool hasJokers, int playerStartingCardCount) {
        Random random = new Random(randomSeed);

        List<int> initial = new List<int>() {
            4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29
        };

        List<int> initialValues = new List<int>();

        int cardsToGenerateCount = hasJokers ? playerStartingCardCount - 1 : playerStartingCardCount;
        for (int i = 0; i < cardsToGenerateCount; i++)
        {
            int randomIndex = random.Next(initial.Count);
            initialValues.Add(initial[randomIndex]);
            initial.RemoveAt(randomIndex);
        }

        return initialValues;
    }
}