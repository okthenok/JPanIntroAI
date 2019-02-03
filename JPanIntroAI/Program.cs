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


    class Program
    {
        static Random rand = new Random();

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



        static void Main(string[] args)
        {
            #region HillClimbing
            Console.Write("Target: ");
            string target = Console.ReadLine();

            StringBuilder current = new StringBuilder();

            for (int i = 0; i < target.Length; i++)
            {
                current.Append((char)rand.Next(32, 127));
            }

            double error = MAE(current, target);
            while (error != 0)
            {
                var test = Mutate(current.ToString());
                double tempError = MAE(test, target);
               
                if (tempError < error)
                {
                    current = test;
                    error = tempError;
                    Console.WriteLine(current.ToString());
                }
            }
            #endregion
        }
    }
}
