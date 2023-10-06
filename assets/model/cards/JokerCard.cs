namespace Fold.Motor.Model.Cards;

public class JokerCard : Card {
    public JokerCard(CardColor color) : base(color, 0) { }

    public override CardDefense Defend(List<Card> cardsAttacking, List<BoardPosition> from, BoardPosition to) {
        Result result = Result.won;
        Card topCard = this;
        Dictionary<string, CardState> cardValues = new();

        cardValues.Add(to.ToString(), State);

        Card cardWithHighestValue = cardsAttacking[0];
        for (int i = 0; i < cardsAttacking.Count; i++) {
            Card cardAttacking = cardsAttacking[i];

            if(cardAttacking.CombatValue == CombatValue)
                result = Result.lost;

            if(cardAttacking.CombatValue > cardWithHighestValue.CombatValue)
                cardWithHighestValue = cardAttacking;

            cardValues.Add(from[i].ToString(), cardAttacking.State);
        }

        if(result == Result.won)
            topCard = cardWithHighestValue;

        return new CardDefense {
            Result = result,
            TopCard = topCard,
            CardValues = cardValues
        };
    }
}