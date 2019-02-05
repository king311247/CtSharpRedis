using System;
using CtSharpRedis;
using CtSharpRedis.CsRedis;
using CtSharpRedis.StackExchange.Redis;
using CtSharpRedis.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CtSharpRedisCoreBenchmark
{
    public class TestBase
    {
        protected IServiceCollection serviceCollection;

        protected IAbstractRedisClient redisClient;



        public class TestInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreateTime { get; set; }

            public int[] TagId { get; set; }

            public override string ToString()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }
        }

        public void Init()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton<IRedisManager, CsRedisManager>();
            //serviceCollection.AddSingleton<IRedisManager, StackExchangeRedisManager>();
            var redisManager = serviceCollection.BuildServiceProvider().GetService<IRedisManager>();
            redisClient = redisManager.Connect("192.168.1.209:10062,Password=gjn2ijj98jg745g10062,ConnectTimeout=5000,WriteBuffer=512,KeepAlive=120,ConnectRetry=2");
        }
    }
}
