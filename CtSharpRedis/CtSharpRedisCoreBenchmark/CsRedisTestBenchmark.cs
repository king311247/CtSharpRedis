using System;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace CtSharpRedisCoreBenchmark
{
    public class CsRedisTestBenchmark: TestBase
    {
        public CsRedisTestBenchmark()
        {
            Init();
        }

        [Benchmark]
        public void CsRedisStringSet()
        {
            redisClient.GetDatabase(1).StringSet("CsRedisStringSet", "123");
        }

        //[Benchmark]
        //public void StackRedisStringSet()
        //{
        //    redisClient.GetDatabase(1).StringSet("StackRedisStringSet", "123");
        //}

        public void Sub()
        {
            redisClient.Subscribe("test", (channel, message) =>
            {
                Console.WriteLine(channel+message);
            });

            redisClient.Subscribe("AAAA2", (channel, message) =>
            {
                Console.WriteLine(channel+message);
            });
        }
    }
}
