using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Optimization
{
    public class Optimizer
    {
        struct PopulationItem
        {
            public double error { get; set; }
            public Layer model { get; set; }
            public PopulationItem(Layer model)
            {
                this.model = model;
                error = 0;
            }
        }

        PopulationItem[] population;

        Random rand;

        /// <summary>
        /// The initial model to optimize.
        /// </summary>
        public Layer model { get; set; }

        /// <summary>
        /// The rate at which to modify the model.
        /// </summary>
        public double learningRate { get; set; }

        /// <summary>
        /// The current generation.
        /// </summary>
        public int generation { get; set; }

        /// <summary>
        /// The  lowest error after the last generation.
        /// </summary>
        public Layer bestModel => population[0].model;

        /// <summary>
        /// The lowest error after the last generation.
        /// </summary>
        public double bestError => population[0].error;

        /// <summary>
        /// The current population size of the optimizer.
        /// </summary>
        public int populationSize => population.Length;

        /// <summary>
        /// Creates a new optimizer.
        /// </summary>
        /// <param name="model">The initial model to optimize.</param>
        /// <param name="populationSize">The initial population size of the optimizer.</param>
        /// <param name="learningRate">The rate at which to modify the model.</param>
        public Optimizer(Layer model, int populationSize, double learningRate)
        {
            this.model = model;
            this.learningRate = learningRate;

            rand = new Random();

            population = new PopulationItem[populationSize];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new PopulationItem(model.CreateChild(learningRate, rand));
            }
        }

        /// <summary>
        /// Returns the error of a model after the last generation.
        /// </summary>
        /// <param name="index">The index of the model.</param>
        public double GetError(int index)
        {
            return population[index].error;
        }

        /// <summary>
        /// Sets the current error of a model.
        /// </summary>
        /// <param name="index">The index of the model.</param>
        /// <param name="error">The new error of the model.</param>
        public void SetError(int index, double error)
        {
            population[index].error = error;
        }

        /// <summary>
        /// Returns a model from the population.
        /// </summary>
        /// <param name="index">The index of the model.</param>
        public Layer GetModel(int index)
        {
            return population[index].model;
        }

        /// <summary>
        /// Sets a model in the population.
        /// </summary>
        /// <param name="index">The index of the model.</param>
        /// <param name="model">The new model.</param>
        public void SetModel(int index, Layer model)
        {
            population[index].model = model;
        }
        
        double GetMultiplier(double error)
        {
            return Math.Pow(error, 2) * learningRate;
        }

        /// <summary>
        /// Advances the optimizer to the next generation.
        /// </summary>
        public void NextGeneration()
        {
            PopulationItem[] nextPopulation = population.OrderBy(item => item.error).ToArray();
            double multiplier = GetMultiplier(nextPopulation[0].error);
            for (int i = nextPopulation.Length / 2; i < nextPopulation.Length; i++)
            {
                Layer nextModel = nextPopulation[0].model.CreateChild(multiplier, rand);
                nextPopulation[i] = new PopulationItem(nextModel);
            }
            population = nextPopulation;
            generation++;
        }
    }
}
