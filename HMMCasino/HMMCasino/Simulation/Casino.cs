namespace HMMCasino.Simulation
{
    using System;
    using System.Collections.Generic;

    using Dice;

    /// <summary>
    /// The casino.
    /// </summary>
    public class Casino
    {
        /// <summary>
        /// The random.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// The fair dice.
        /// </summary>
        private readonly FairDice fairDice;

        /// <summary>
        /// The unfair dice.
        /// </summary>
        private readonly UnfairDice unfairDice;

        /// <summary>
        /// The switch to fair chance.
        /// </summary>
        private readonly double switchToFairChance;

        /// <summary>
        /// The swicth to unfair chance.
        /// </summary>
        private readonly double swicthToUnfairChance;

        /// <summary>
        /// The simulation steps.
        /// </summary>
        private readonly List<SimulationStep> SimulationSteps = new List<SimulationStep>();

        /// <summary>
        /// The current dice.
        /// </summary>
        private Dice.Dice currentDice;

        /// <summary>
        /// Initializes a new instance of the <see cref="Casino"/> class.
        /// </summary>
        /// <param name="chances">
        /// The chances.
        /// </param>
        /// <param name="switchToFairChance">
        /// The switch to fair chance.
        /// </param>
        /// <param name="swicthToUnfairChance">
        /// The swicth to unfair chance.
        /// </param>
        public Casino(List<DiceSideChance> chances, double switchToFairChance, double swicthToUnfairChance)
        {
            this.fairDice = new FairDice(chances);
            this.unfairDice = new UnfairDice(chances);
            this.switchToFairChance = switchToFairChance;
            this.swicthToUnfairChance = swicthToUnfairChance;
            this.currentDice = this.SetStartDice(chances);
        }

        private Dice.Dice SetStartDice(List<DiceSideChance> chances)
        {
            var sumOfChances = this.switchToFairChance + this.swicthToUnfairChance;

            var randomNumber = this.random.NextDouble();
            var unfairStart = this.swicthToUnfairChance / sumOfChances;

            if (randomNumber <= unfairStart)
            {
                return new UnfairDice(chances);
            }

            return new FairDice(chances);
        }

        /// <summary>
        /// The simulate.
        /// </summary>
        /// <param name="numberOfSteps">
        /// The number of steps.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>. 
        /// </returns>
        public List<SimulationStep> Simulate(int numberOfSteps)
        {
            for (var i = 0; i < numberOfSteps; ++i)
            {
                this.MaybeSwitchDice();
                var side = this.currentDice.Roll();
                this.SimulationSteps.Add(new SimulationStep(this.currentDice is FairDice ? DiceState.FairDice : DiceState.UnfairDice, side));
            }

            return this.SimulationSteps;
        }

        /// <summary>
        /// The maybe switch dice.
        /// </summary>
        private void MaybeSwitchDice()
        {
            var randomNumber = this.random.NextDouble();
            if (this.currentDice is UnfairDice && randomNumber <= this.switchToFairChance)
            {
                this.currentDice = this.fairDice;
                return;
            }

            if (this.currentDice is FairDice && randomNumber <= this.swicthToUnfairChance)
            {
                this.currentDice = this.unfairDice;
            }
        }
    }
}
