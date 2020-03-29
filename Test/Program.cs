using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BenRL;
using BenRL.Layers.Connection;
using BenRL.Layers.Activation;
using BenRL.Optimization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TestTrainingSet();
            TestAgents();

            Console.ReadLine();
        }

        static void TestTrainingSet()
        {
            Tensor[] inputSet = new Tensor[]
            {
                Tensor.FromArray(0, 0, 0, 0),
                Tensor.FromArray(1, 1, 1, 1),
                Tensor.FromArray(1, 1, 0, 0),
                Tensor.FromArray(0, 0, 1, 1),
                Tensor.FromArray(0, 1, 0, 1),
                Tensor.FromArray(1, 0, 1, 0),

                Tensor.FromArray(1, 0, 0, 1),
                Tensor.FromArray(0, 1, 1, 0)
            };
            Tensor[] outputSet = new Tensor[]
            {
                Tensor.FromArray(0, 1),
                Tensor.FromArray(0, 1),
                Tensor.FromArray(0, 1),
                Tensor.FromArray(0, 1),
                Tensor.FromArray(0, 1),
                Tensor.FromArray(0, 1),

                Tensor.FromArray(1, 0),
                Tensor.FromArray(1, 0)
            };

            LayerModel model = new LayerModel();
            model.layers.Add(new FullyConnectedLayer(10));
            model.layers.Add(new ReLULayer());
            model.layers.Add(new BiasLayer());
            model.layers.Add(new FullyConnectedLayer(2));
            model.layers.Add(new SigmoidLayer());
            model.Init(4);

            TrainingSetOptimizer optimizer = new TrainingSetOptimizer(inputSet, outputSet, model, 100, 1.5);
            for(int g = 0; g < 300; g++)
            {
                optimizer.NextGeneration();
                Console.WriteLine("Generation " + g + " error: " + optimizer.optimizer.bestError);
            }
        }

        static void TestAgents()
        {
            LayerModel model = new LayerModel();

            model.layers.Add(new FullyConnectedLayer(4));
            model.layers.Add(new ReLULayer());
            model.layers.Add(new BiasLayer());
            model.layers.Add(new FullyConnectedLayer(2));

            model.Init(2);

            Agent[] agents = new Agent[] { new TestAgent(), new TestAgent(), new TestAgent() };

            AgentOptimizer optimizer = new AgentOptimizer(model, agents, 2, 2, 0.1);

            for (int g = 0; g < 300; g++)
            {
                optimizer.Tick();
                Console.WriteLine("Generation " + g + " error: " + optimizer.optimizer.bestError);
                Tensor testOutput = optimizer.optimizer.bestModel.Run(1, 1);
                Console.WriteLine("Output: " + testOutput[0] + ", " + testOutput[1]);
            }
        }
    }
}