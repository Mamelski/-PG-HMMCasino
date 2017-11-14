namespace HMMCasino.Simulation.Dice
{
    using System.Collections.Generic;

    /// <summary>
    /// The fair dice.
    /// </summary>
    public class FairDice : Dice
    {
        /// <summary>
        /// The roll.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int Roll()
        {
            return this.random.Next(0, this.sidesChances.Count);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FairDice"/> class.
        /// </summary>
        /// <param name="sideChancesChances">
        /// The side chances chances.
        /// </param>
        public FairDice(List<DiceSideChance> sideChancesChances)
            : base(sideChancesChances)
        {
        }
    }
}
