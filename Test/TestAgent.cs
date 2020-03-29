using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenRL;
using BenRL.Optimization;

namespace Test
{
    public class TestAgent : Agent
    {
        double guess1;
        double guess2;

        public void ConsumeInputs(Tensor input)
        {
            guess1 = input[0];
            guess2 = input[1];
        }

        public double GetError()
        {
            return Math.Abs(5 - guess1) + Math.Abs(3 - guess2);
        }

        public Tensor ProduceOutputs()
        {
            return Tensor.FromArray(1, 1);
        }

        public void Reset()
        {
            guess1 = 0;
            guess2 = 0;
        }
    }
}
