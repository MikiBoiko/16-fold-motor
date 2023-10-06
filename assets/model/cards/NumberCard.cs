namespace Fold.Motor.Model.Cards;

public class NumberCard : Card {
    public NumberCard(CardColor color, int value) : base(color, value) { }

    public override CardDefense Defend(List<Card> cardsAttacking, List<BoardPosition> from, BoardPosition to) {
        Result result = Result.won;
        Card topCard = this;
        Dictionary<string, CardState> cardValues = new();

        cardValues.Add(to.ToString(), State);

        int attackValue = 0;
        for (int i = 0; i < cardsAttacking.Count; i++) {
            Card cardAttacking = cardsAttacking[i];

            attackValue += cardAttacking.CombatValue;

            cardValues.Add(from[i].ToString(), cardAttacking.State);
        }

        if(attackValue == CombatValue)
            result = Result.draw;
        if(attackValue > CombatValue) {
            result = Result.lost;
        }

        return new CardDefense {
            Result = result,
            TopCard = topCard,
            CardValues = cardValues
        };
    }
}