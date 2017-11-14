namespace HMMCasino.Algorithms
{
    using Simulation.Dice;

    public class StateWithChance
    {
        public StateWithChance(DiceState state, double probability)
        {
            this.State = state;
            this.Chance = probability;
        }

        public double Chance { get; }

        public DiceState State { get; }
    }
}
