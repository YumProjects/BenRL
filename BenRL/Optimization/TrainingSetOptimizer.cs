using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Optimization
{
    public class TrainingSetOptimizer
    {
        /// <summary>
        /// The input training set.
        /// </summary>
        public Tensor[] inputSet { get; set; }

        /// <summary>
        /// The output training set.
        /// </summary>
        public Tensor[] outputSet { get; set; }

        /// <summary>
        /// The underling <see cref="Optimizer"/>.
        /// </summary>
        public Optimizer optimizer { get; set; }

        /// <summary>
        /// Creates a new <see cref="TrainingSetOptimizer"/>.
        /// </summary>
        /// <param name="inputSet">The input training set.</param>
        /// <param name="outputSet">The output training set.</param>
        /// <param name="model">The initial model to optimize.</param>
        /// <param name="populationSize">The population size.</param>
        /// <param name="learningRate">The rate at which to modify the model.</param>
        public TrainingSetOptimizer(Tensor[] inputSet, Tensor[] outputSet, Layer model,
            int populationSize, double learningRate)
        {
            if (inputSet.Length != outputSet.Length)
                throw new Exception("The length of the input and output training sets did not match.");

            this.inputSet = inputSet;
            this.outputSet = outputSet;
            optimizer = new Optimizer(model, populationSize, learningRate);
        }

        /// <summary>
        /// Advances the optimizer to the next generation.
        /// </summary>
        public void NextGeneration()
        {
            for(int p = 0; p < optimizer.populationSize; p++)
            {
                double error = 0;
                for(int t = 0; t < inputSet.Length; t++)
                {
                    Tensor output = optimizer.GetModel(p).Run(inputSet[t]);
                    output.IterateArrayPosition(pos =>
                    {
                        error += Math.Abs(output[pos] - outputSet[t][pos]);
                    });
                }
                error /= inputSet.Length;
                optimizer.SetError(p, error);
            }
            optimizer.NextGeneration();
        }
    }
}