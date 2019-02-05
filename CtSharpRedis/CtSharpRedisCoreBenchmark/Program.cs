using System;
using BenchmarkDotNet.Running;

namespace CtSharpRedisCoreBenchmark
{
    class Program
    {

        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CsRedisTestBenchmark>();

            //CsRedisTestBenchmark aa = new CsRedisTestBenchmark();

            //aa.CsRedisStringSet();
            //aa.CsRedisStringSet();
            //aa.CsRedisStringSet();
            //Console.WriteLine("sub start");
            //aa.Sub();
            Console.ReadLine();
        }
    }
}
