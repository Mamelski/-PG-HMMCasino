namespace HMMCasino.Algorithms
{
    using System;
    using System.Collections.Generic;

    using Simulation;
    using Simulation.Dice;

    public class Viterbi : Algorithm
    {
        private Dictionary<DiceState, StateWithChance[]> chances;

        public Viterbi(List<SimulationStep> simulationsSteps, double switchToFairChance, double swicthToUnfairChance, List<DiceSideChance> sides)
            : base(simulationsSteps, switchToFairChance, swicthToUnfairChance, sides)
        {
        }

        public DiceState[] Calculate()
        {
            this.chances = new Dictionary<DiceState, StateWithChance[]>();

            // inicjalizacja
            foreach (var iState in this.States)
            {
                this.chances[iState] = new StateWithChance[this.T];
                this.chances[iState][0] = new StateWithChance(iState, Math.Log(this.StartPropabilities[iState]));
            }

            // iteracja
            for (var t = 1; t < this.T; t++)
            {
                foreach (var iState in this.States)
                {
                    this.chances[iState][t] = this.MaxhChanceToBeInIState(t, iState);
                }
            }

            // Ostatni krok
            if (this.chances[DiceState.FairDice][this.T - 1].Chance > this.chances[DiceState.UnfairDice][this.T - 1].Chance)
            {
                this.Result[this.T - 1] = DiceState.FairDice;
            }
            else
            {
                this.Result[this.T-1] = DiceState.UnfairDice;
            }

            // Cofamy się
            for (var t = this.T - 1; t > 0; --t)
            {
                this.Result[t - 1] = this.chances[this.Result[t]][t].State;
            }

            return this.Result;
        }

        // szansa, że w kroku t będzie stan iState
        private StateWithChance MaxhChanceToBeInIState(int t, DiceState iState)
        {
            var maxChance = new StateWithChance(default(DiceState), double.MinValue);

            foreach (var jState in this.States)
            {
                var value = this.chances[jState][t - 1].Chance +
                    Math.Log(this.TransitionMatrix[jState, iState]) +
                    Math.Log(this.EmissionMatrix[iState, this.Observations[t]]);

                if (value > maxChance.Chance)
                {
                    maxChance = new StateWithChance(jState, value);
                }
            }
            return maxChance;
        }
    }
}
