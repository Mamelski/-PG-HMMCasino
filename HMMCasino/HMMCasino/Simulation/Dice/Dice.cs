namespace HMMCasino.Simulation.Dice
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The dice.
    /// </summary>
    public abstract class Dice
    {
        /// <summary>
        /// The sidesChances.
        /// </summary>
        public List<DiceSideChance> sidesChances;

        protected Random random = new Random(0912830912);

        public Dice(List<DiceSideChance> sideChancesChances )
        {
            this.sidesChances = sideChancesChances;
        }

        public abstract int Roll();
    }
}
