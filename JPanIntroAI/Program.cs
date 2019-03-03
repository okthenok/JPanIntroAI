using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPanIntroAI
{

    //AND Gate
    // 0 0 = 0 0
    // 0 1 = 0 0
    // 1 0 = 0 1
    // 1 1 = 1 1

    //Perceptron
    //Compute
    //Mutate: set a random weight to a random value (double)

    class Perceptron
    {
        double bias;
        double[] weights;

        public Perceptron(int inputSize)
        {
            weights = new double[inputSize];
        }

        public void Randomize(Random random)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = random.NextDouble() - 0.5;
            }
            bias = random.NextDouble() - 0.5;
        }

        public double Compute(double[] inputs)
        {
            double output = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * weights[i];
            }
            return output + bias > 0 ? 1 : 0;
        }

        // And Gate                                                                     
        // Inputs     |   Outputs                                                         
        // [0, 0]     |   0
        // [0, 1]     |   0
        // [1, 0]     |   0
        // [1, 1]     |   1
        public double MAE(double[][] inputs, double[] outputs)
        {
            double rawError = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                double output = Compute(inputs[i]);
                rawError += Math.Abs(outputs[i] - output);
            }
            return rawError / inputs.Length;
        }

        public void Train(double[] inputs, double desiredOutput)
        {
            double output = Compute(inputs);
            double error = desiredOutput - output;
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += error * inputs[i];
            }
            bias += error;
        }

        public void TrainAll(double[][] inputs, double[] desiredOutputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                Train(inputs[i], desiredOutputs[i]);
            }
        }
    }


    class Program
    {
        static Random rand = new Random();

        #region HillClimbing functions
        /*
        //mutate: return mutated string
        static StringBuilder Mutate(string input)
        {
            StringBuilder temp = new StringBuilder(input);
            int index = rand.Next(input.Length);

            int flip = rand.Next(2) * 2 - 1;

            temp[index] = (char)(temp[index] + flip);

            return temp;
        }
        //mae: return double
        static double MAE(StringBuilder current, string target)
        {
            double absError = 0;
            for (int i = 0; i < current.Length; i++)
            {
                absError += Math.Abs((int)target[i] - (int)current[i]);
            }
            return absError / current.Length;
        }
        //compute: calculate output of perceptron
        */
        #endregion

        static double Sigmoid(double input)
        {
            return 1.0 / (1 + Math.Exp(-input));
        }

        static void Main(string[] args)
        {
            #region HillClimbing
            //Console.Write("Target: ");
            //string target = Console.ReadLine();

            //StringBuilder current = new StringBuilder();

            //for (int i = 0; i < target.Length; i++)
            //{
            //    current.Append((char)rand.Next(32, 127));
            //}

            //double error = MAE(current, target);
            //while (error != 0)
            //{
            //    var test = Mutate(current.ToString());
            //    double tempError = MAE(test, target);

            //    if (tempError < error)
            //    {
            //        current = test;
            //        error = tempError;
            //        Console.WriteLine(current.ToString());
            //    }
            //}
            #endregion
            #region Perceptron
            /*
            Perceptron andGate = new Perceptron(2);

            double[][] inputs = new double[][]
            {
                new double[] {0,0},
                new double[] {0,1},
                new double[] {1,0},
                new double[] {1,1},
            };
            double[] andOutputs = new double[] { 0, 0, 0, 1 };

            Perceptron orGate = new Perceptron(2);
            double[] orOutputs = new double[] { 0, 1, 1, 1 };



            int epoch = 0;
            while (orGate.MAE(inputs, orOutputs) > 0)
            {
                orGate.TrainAll(inputs, orOutputs);
                Console.WriteLine($"{epoch}");
                for (int i = 0; i < inputs.Length; i++)
                {
                    Console.WriteLine($"{inputs[i][0]} & {inputs[i][1]} = {orGate.Compute(inputs[i])}");
                }
                Console.WriteLine();

                epoch++;
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                Console.WriteLine($"{inputs[i][0]} & {inputs[i][1]} = {orGate.Compute(inputs[i])}");
            }
            Console.ReadKey();
            */
            #endregion
            #region Feed Foward Neural Network
            Network[] population = new Network[1000];
            for (int i = 0; i < 1000; i++)
            {
                population[i] = new Network(a => a < 0 ? 0 : 1, 2, 2, 1);
                //population[i] = new Network(Sigmoid, 2, 2, 1);
                population[i].Randomize(rand);
            }
            double[][] inputs = new double[][]
            {
                new double[] {0, 0},
                new double[] {0, 1},
                new double[] {1, 0},
                new double[] {1, 1},
            };
            double[][] xorOutputs = new double[][]
            {
                new double[] {0},
                new double[] {1},
                new double[] {1},
                new double[] {0},
            };

            int epoch = 0;
            do
            {
                epoch++;

                int cut = (int)(population.Length * 0.90);
                for (int i = 1; i < cut; i++)
                {
                    population[i].Mutate(rand, 0.25);
                }
                for (int i = cut; i < population.Length; i++)
                {
                    population[i].Randomize(rand);
                }

                Array.Sort(population, (a, b) => a.MAE(inputs, xorOutputs).CompareTo(b.MAE(inputs, xorOutputs)));

                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < inputs.Length; i++)
                {
                    Console.Write("[ ");
                    for (int j = 0; j < inputs[i].Length; j++)
                    {
                        Console.Write($"{inputs[i][j]} ");
                    }
                    Console.Write("] = [ ");
                    double[] output = population[0].Compute(inputs[i]);
                    for (int j = 0; j < output.Length; j++)
                    {
                        Console.Write($"{output[j]:0.00} ");
                    }
                    Console.WriteLine("]");
                }
                Console.WriteLine($"Gen: {epoch}");

            } while (population[0].MAE(inputs, xorOutputs) > 0);

            //run the network to test it
            #endregion
        }
    }
}
