using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL
{
    public abstract class Layer
    {
        /// <summary>
        /// Initializes the <see cref="Layer"/>.
        /// </summary>
        /// <param name="inputSize">An array representing the input size <see cref="Vector"/>
        /// of the <see cref="Layer"/>.</param>
        /// <returns>The output size <see cref="Vector"/> of the <see cref="Layer"/>.</returns>
        public Vector Init(params double[] inputSize)
        {
            return Init(new Vector(inputSize));
        }

        /// <summary>
        /// Runs the <see cref="Layer"/>.
        /// </summary>
        /// <param name="inputs">An array representing the input <see cref="Tensor"/>
        /// of the <see cref="Layer"/>.</param>
        /// <returns>The output <see cref="Tensor"/> of the <see cref="Layer"/>.</returns>
        public Tensor Run(params double[] inputs)
        {
            return Run(Tensor.FromArray(inputs));
        }

        public abstract Vector Init(Vector inputSize);
        public abstract Tensor Run(Tensor input);
        public abstract Layer CreateChild(double multiplier, Random rand);
    }
}