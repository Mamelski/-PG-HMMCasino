namespace HMMCasino.Simulation
{
    using HMMCasino.Simulation.Dice;

    public class SimulationStep
    {
        public SimulationStep(DiceState diceState, int numberOnDice)
        {
            this.DiceState = diceState;
            this.NumberOnDice = numberOnDice;
        }

        public DiceState DiceState { get; set; }

        public int NumberOnDice { get; set; }
    }
}
