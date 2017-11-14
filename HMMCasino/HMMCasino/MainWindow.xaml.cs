using System.Windows;

namespace HMMCasino
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using HMMCasino.Algorithms;

    using Simulation;
    using Simulation.Dice;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// The steps.
        /// </summary>
        private List<SimulationStep> steps;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.InitilizeSides();
            this.dataGrid.ItemsSource = this.DiceSides;
        }

        public BindingList<DiceSideChance> DiceSides { get; set; }

        /// <summary>
        /// The initilize sidesChances.
        /// </summary>
        private void InitilizeSides()
        {
            this.DiceSides = new BindingList<DiceSideChance>
                                 {
                                     new DiceSideChance { Chance = 0.2 },
                                     new DiceSideChance { Chance = 0.4 },
                                     new DiceSideChance { Chance = 0.3 },
                                     new DiceSideChance { Chance = 0.2 },
                                     new DiceSideChance { Chance = 0.9 },
                                     new DiceSideChance { Chance = 0.5 }
                                 };
        }

        /// <summary>
        /// The start simulation_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void StartSimulation(object sender, RoutedEventArgs e)
        {
            double swicthToUnfairChance = (double)this.ChangeToSwitchToUnfair.Value;
            double switchToFairChance = (double)this.ChanceToSwitchToFair.Value;
            int numberOfRolls = (int)this.NumberOfRolls.Value;

            var casinoSimulation = new Casino(this.DiceSides.ToList(), switchToFairChance, swicthToUnfairChance);
            this.steps = casinoSimulation.Simulate(numberOfRolls);

            var alfaBeta = new AlfaBeta(this.steps,switchToFairChance, swicthToUnfairChance, this.DiceSides.ToList());
            var alfaBetaResult = alfaBeta.Calculate();

            double alfaPass = 0;

            for (int i = 0; i < alfaBetaResult.Length; ++i)
            {
                if (alfaBetaResult[i] == this.steps[i].DiceState)
                {
                    ++alfaPass;
                }
            }

            var viterbi = new Viterbi(this.steps, switchToFairChance, swicthToUnfairChance, this.DiceSides.ToList());
            var viterbiResult = viterbi.Calculate();

            double viterbiPass = 0;

            for (int i = 0; i < viterbiResult.Length; ++i)
            {
                if (viterbiResult[i] == this.steps[i].DiceState)
                {
                    ++viterbiPass;
                }
            }

            int a = 0;
        }

        /// <summary>
        /// The run algorithms_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RunAlgorithms_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
