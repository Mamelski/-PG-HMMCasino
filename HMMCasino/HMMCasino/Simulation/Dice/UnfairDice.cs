namespace HMMCasino.Simulation.Dice
{
    using System.Collections.Generic;
    using System.Linq;

    public class UnfairDice : Dice
    {
        public UnfairDice(List<DiceSideChance> sideChancesChances)
            : base(sideChancesChances)
        {
        }

        public override int Roll()
        {
            var sumOfChances = this.sidesChances.Sum(diceSideChance => diceSideChance.Chance);

            var percentChances = this.sidesChances.Select(c => c.Chance / sumOfChances).ToList();

            var randomNumber = this.random.NextDouble();
            double subsum = 0;
            for (var index = 0; index < percentChances.Count; index++)
            {
                subsum += percentChances[index];
                if (randomNumber <= subsum)
                {
                    return index;
                }
            }

            return 0;
        }
    }
}
