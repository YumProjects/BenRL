using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Layers.Connection
{
    public class FullyConnectedLayer : Layer
    {
        /// <summary>
        /// The output size of the <see cref="FullyConnectedLayer"/>.
        /// </summary>
        public int outputSize { get; private set; }

        /// <summary>
        /// The weights of the connections.
        /// </summary>
        public Tensor weights { get; set; }

        private FullyConnectedLayer() { }

        /// <summary>
        /// Creates a new <see cref="FullyConnectedLayer"/>
        /// </summary>
        /// <param name="outputSize"></param>
        public FullyConnectedLayer(int outputSize)
        {
            this.outputSize = outputSize;
        }

        public override Vector Init(Vector inputSize)
        {
            if (inputSize.dimentions != 1)
                throw new Exception("Fully connected layer input size must be 1 dimentional.");

            weights = new Tensor(new Vector(inputSize[0], outputSize));
            return new Vector(outputSize);
        }

        public override Tensor Run(Tensor input)
        {
            Tensor output = new Tensor(outputSize);
            weights.IterateArrayPosition(pos => output[pos[1]] += input[pos[0]] * weights[pos]);
            return output;
        }

        public override Layer CreateChild(double multiplier, Random rand)
        {
            Tensor newWeights = new Tensor(weights.GetSizeArray());
            //double weightMultiplier = multiplier / weights.GetSizeVector().GetArea();
            newWeights.IterateArrayPosition(pos => 
            {
                double change = (rand.NextDouble() - 0.5) * 2 * multiplier;
                newWeights[pos] = weights[pos] + change;
            });
            return new FullyConnectedLayer() 
            { 
                outputSize = outputSize, 
                weights = newWeights
            };
        }
    }
}