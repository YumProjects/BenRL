using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Layers.Activation
{
    public class ReLULayer : Layer
    {
        public override Vector Init(Vector inputSize)
        {
            return inputSize;
        }

        public override Tensor Run(Tensor input)
        {
            return input.Apply(x => x > 0 ? x : 0);
        }

        public override Layer CreateChild(double multiplier, Random rand)
        {
            return new ReLULayer();
        }
    }
}
