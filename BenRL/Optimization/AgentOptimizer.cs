using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Optimization
{
    public class AgentOptimizer
    {
        /// <summary>
        /// The number of ticks in every round.
        /// </summary>
        public int ticksPerRound { get; private set; }

        /// <summary>
        /// The number of rounds in each generation.
        /// </summary>
        public int roundsPerGeneration { get; private set; }

        /// <summary>
        /// The set of agents to test the model on.
        /// </summary>
        public Agent[] agents { get; private set; }

        /// <summary>
        /// The current tick.
        /// </summary>
        public int currentTick { get; private set; }

        /// <summary>
        /// The current round.
        /// </summary>
        public int currentRound { get; private set; }

        /// <summary>
        /// The underlying basic optimizer.
        /// </summary>
        public Optimizer optimizer { get; set; }


        /// <summary>
        /// Creates a new agent optimizer.
        /// </summary>
        /// <param name="model">The initial model to optimize</param>
        /// <param name="agents">A set of agents to test the model on.</param>
        /// <param name="ticksPerRound">The number of ticks in every round.</param>
        /// <param name="roundsPerGeneration">The number of rounds in every generation.</param>
        /// <param name="learningRate">The rate at which to modify the model.</param>
        public AgentOptimizer(Layer model, Agent[] agents, int ticksPerRound,
            int roundsPerGeneration, double learningRate)
        {
            this.ticksPerRound = ticksPerRound;
            this.roundsPerGeneration = roundsPerGeneration;
            this.agents = agents;

            currentTick = 0;
            currentRound = 0;

            optimizer = new Optimizer(model, roundsPerGeneration * agents.Length, learningRate);

            ResetAgents();
        }
        
        /// <summary>
        /// Resets all agents.
        /// </summary>
        public void ResetAgents()
        {
            for(int i = 0; i < agents.Length; i++)
            {
                agents[i].Reset();
            }
        }

        /// <summary>
        /// Runs one tick of the optimizer on all agents.
        /// </summary>
        public void Tick()
        {
            for(int i = 0; i < agents.Length; i++)
            {
                int index = currentRound * agents.Length + i;
                Tensor outputs = agents[i].ProduceOutputs();
                Tensor inputs = optimizer.GetModel(index).Run(outputs);
                agents[i].ConsumeInputs(inputs);
                optimizer.SetError(index, agents[i].GetError());
            }

            currentTick++;
            if (currentTick >= ticksPerRound)
            {
                currentTick = 0;
                currentRound++;
                if (currentRound >= roundsPerGeneration)
                {
                    optimizer.NextGeneration();
                    currentRound = 0;
                }
                ResetAgents();
            }
        }
    }
}