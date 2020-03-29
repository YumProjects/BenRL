using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL
{
    public class LayerModel : Layer
    {
        /// <summary>
        /// The set of <see cref="Layer"/>s which will run in sequence.
        /// </summary>
        public List<Layer> layers { get; private set; }

        Vector inputSize;

        /// <summary>
        /// Creates a new <see cref="LayerModel"/>.
        /// </summary>
        public LayerModel()
        {
            layers = new List<Layer>();
        }

        public override Vector Init(Vector inputSize)
        {
            this.inputSize = inputSize;
            Vector nextSize = inputSize.Copy();
            for(int i = 0; i < layers.Count; i++)
            {
                nextSize = layers[i].Init(nextSize);
            }
            return nextSize;
        }

        public override Tensor Run(Tensor input)
        {
            if (input.GetSizeVector() != inputSize)
                throw new Exception("Size of input did not match required input size of model.");

            Tensor nextTensor = input.Copy();
            for(int i = 0; i < layers.Count; i++)
            {
                nextTensor = layers[i].Run(nextTensor);
            }
            return nextTensor;
        }

        public override Layer CreateChild(double multiplier, Random rand)
        {
            LayerModel child = new LayerModel() { inputSize = inputSize };
            for (int i = 0; i < layers.Count; i++)
            {
                child.layers.Add(layers[i].CreateChild(multiplier, rand));
            }
            return child;
        }
    }
}
