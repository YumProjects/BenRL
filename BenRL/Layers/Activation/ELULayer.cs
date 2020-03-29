using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Layers.Activation
{
    public class ELULayer : Layer
    {
        public double a { get; set; }

        public ELULayer(double a)
        {
            this.a = a;
        }

        public override Vector Init(Vector inputSize)
        {
            return inputSize;
        }

        public override Tensor Run(Tensor input)
        {
            return input.Apply(x => x > 0 ? x : a * (Math.Pow(Math.E, x) - 1));
        }

        public override Layer CreateChild(double multiplier, Random rand)
        {
            return new ELULayer(a);
        }
    }
}
