namespace HMMCasino.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Simulation;
    using Simulation.Dice;


    public class AlfaBeta : Algorithm
    {
        private double[] c;

        private Dictionary<DiceState, double[]> alpha;

        private Dictionary<DiceState, double[]> beta;

        private double chanceSufix;

        private double chancePrefix;

        private double logChance = 0;

        private double chance = 0;

        public DiceState[] Calculate()
        {
            this.AlfaPass();
            this.BetaPass();
            this.CalculateGamma();
            return this.Result;
        }

        

        // sanjose-HMM
        private void AlfaPass()
        {
            // compute a0(1)
            foreach (var state in this.States)
            {
                this.alpha[state][0] = this.StartPropabilities[state] * this.EmissionMatrix[state, this.Observations[0]];
                this.c[0] += this.alpha[state][0];
            }

            // scale the a0(i)
            this.c[0] = 1 / this.c[0];
            foreach (var state in this.States)
            {
                this.alpha[state][0] *= this.c[0];
            }

            // compute at(i)
            for (var t = 1; t < this.T; t++)
            {
                this.c[t] = 0;
                foreach (var iState in this.States)
                {
                    this.alpha[iState][t] = 0;
                    foreach (var jState in this.States)
                    {
                        this.alpha[iState][t] += this.alpha[jState][t - 1] * this.TransitionMatrix[jState, iState];
                    }

                    this.alpha[iState][t] *= this.EmissionMatrix[iState, this.Observations[t]];
                    this.c[t] += this.alpha[iState][t];

                }

                // scale at(i)
                this.c[t] = 1 / this.c[t];
                foreach (var iState in this.States)
                {
                    this.alpha[iState][t] *= this.c[t];
                }
            }
        }

        // sanjose-HMM
        private void BetaPass()
        {
            // Let b[T-1](i) scaled by c[T-1]
            foreach (var iState in this.States)
            {
                this.beta[iState][this.T - 1] = this.c[this.T - 1];
            }

            // β-pass
            for (var t = this.T - 2; t >= 0; --t)
            {
                foreach (var iState in this.States)
                {
                    this.beta[iState][t] = 0;
                    foreach (var jState in this.States)
                    {
                        this.beta[iState][t] += this.TransitionMatrix[iState, jState]
                                           * this.EmissionMatrix[jState, this.Observations[t + 1]] * this.beta[jState][t + 1];
                    }
                    // scale b[t](i) with same scale factor as at(i)

                    this.beta[iState][t] *= this.c[t];
                }
            }
        }

        private void CalculateGamma()
        {
            this.chanceSufix =  this.States.Sum(state => this.StartPropabilities[state] * this.EmissionMatrix[state, this.Observations[0]] * this.beta[state][0]);
            this.chancePrefix = this.States.Sum(state => this.alpha[state][this.T - 1]);

            for (int i = 0; i < this.T; i++)
            {
                this.logChance -= Math.Log(this.c[i]);
            }

            this.chance = Math.Exp(this.logChance);

            for (var i = 0; i < this.T; i++)
            {
                var calculatedState = this.States.Aggregate( (s1, s2) => 
                this.alpha[s1][i] * this.beta[s1][i] > this.alpha[s2][i] * this.beta[s2][i]
                            ? s1
                            : s2);
                this.Result[i] = calculatedState;
            }
        }

        public AlfaBeta(List<SimulationStep> simulationsSteps, double switchToFairChance, double swicthToUnfairChance, List<DiceSideChance> sides)
            : base(simulationsSteps, switchToFairChance, swicthToUnfairChance, sides)
        {
            this.c = new double[this.T];

            this.alpha = new Dictionary<DiceState, double[]>
            {
                { DiceState.FairDice, new double[this.T]},
                { DiceState.UnfairDice, new double[this.T]},
            };

            this.beta = new Dictionary<DiceState, double[]>
            {
                { DiceState.FairDice, new double[this.T]},
                { DiceState.UnfairDice, new double[this.T]},
            };
        }
    }
}
