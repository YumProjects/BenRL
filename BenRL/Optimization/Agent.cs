using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenRL.Optimization
{
    public interface Agent
    {
        void Reset();
        void ConsumeInputs(Tensor input);
        Tensor ProduceOutputs();
        double GetError();
    }
}
