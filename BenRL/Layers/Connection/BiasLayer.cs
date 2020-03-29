using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Layers.Connection
{
    public class BiasLayer : Layer
    {
        public Tensor weights { get; set; }

        public override Vector Init(Vector inputSize)
        {
            weights = new Tensor(inputSize);
            return inputSize;
        }

        public override Tensor Run(Tensor input)
        {
            Tensor output = new Tensor(input.GetSizeArray());
            output.IterateArrayPosition(pos => output[pos] = input[pos] + weights[pos]);
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
            return new BiasLayer()
            {
                weights = newWeights
            };
        }
    }
}