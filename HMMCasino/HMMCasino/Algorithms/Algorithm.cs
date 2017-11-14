namespace HMMCasino.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Simulation;
    using Simulation.Dice;

    public abstract class Algorithm
    {
        // Liczba obserwacji
        protected int T;

        // Możliwe stany
        protected DiceState[] States;

        // Początkowe szanse na stan
        protected Dictionary<DiceState, double> StartPropabilities;

        // Szansa dla każdego stanu na wylosowanie każdej ze ścian
        protected Matrix<DiceState, int> EmissionMatrix;

        // Szansa na przejście pomiędzy stanami
        protected Matrix<DiceState, DiceState> TransitionMatrix;

        // Aktualne obserwacje
        protected List<int> Observations;

        protected DiceState[] Result;

        public Algorithm(List<SimulationStep> simulationsSteps, double switchToFairChance, double swicthToUnfairChance, List<DiceSideChance> sides)
        {
            this.T = simulationsSteps.Count;

            this.States = new[] {DiceState.FairDice, DiceState.UnfairDice };

            this.StartPropabilities = this.CalculateStartPossibilities(switchToFairChance, swicthToUnfairChance);

            var sideNumbers= new List<int>();
            for (int i = 0; i < sides.Count; ++i)
            {
                sideNumbers.Add(i);
            }
            this.EmissionMatrix = new Matrix<DiceState,int>(this.States, sideNumbers);

            for (var i = 0; i < sides.Count; ++i)
            {
                this.EmissionMatrix[DiceState.FairDice, i] = 1.0 / sides.Count;
                this.EmissionMatrix[DiceState.UnfairDice, i] = this.CalculateDicePossibilities(sides,i);
            }

            this.Observations = simulationsSteps.Select(step => step.NumberOnDice).ToList();

            this.TransitionMatrix = this.PrepareTransitionsMatrix(switchToFairChance, swicthToUnfairChance);

            this.Result = new DiceState[this.T];
        }

        private double CalculateDicePossibilities(List<DiceSideChance> sides, int index)
        {
            var sumOfChances = sides.Sum(diceSideChance => diceSideChance.Chance);

            return sides[index].Chance / sumOfChances;

        }

        private Matrix<DiceState, DiceState> PrepareTransitionsMatrix(double switchToFairChance, double swicthToUnfairChance)
        {
            var sumOfChances = switchToFairChance + swicthToUnfairChance;
            return new Matrix<DiceState, DiceState> (this.States, this.States)
                       {
                           [DiceState.FairDice, DiceState.FairDice] = 1- swicthToUnfairChance/ sumOfChances,
                           [DiceState.FairDice, DiceState.UnfairDice] = swicthToUnfairChance/ sumOfChances,
                           [DiceState.UnfairDice, DiceState.FairDice] = switchToFairChance / sumOfChances,
                           [DiceState.UnfairDice, DiceState.UnfairDice] =1- switchToFairChance/ sumOfChances
                       };
        }

        private Dictionary<DiceState, double> CalculateStartPossibilities(double switchToFairChance, double swicthToUnfairChance)
        {
            var sumOfChances = switchToFairChance + swicthToUnfairChance;
            return new Dictionary<DiceState, double>
            {
                { DiceState.FairDice, switchToFairChance/sumOfChances},
                { DiceState.UnfairDice, swicthToUnfairChance/sumOfChances},
            };
        }
    }
}
