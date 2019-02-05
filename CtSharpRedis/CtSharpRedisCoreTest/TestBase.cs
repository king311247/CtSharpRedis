using System;
using System.Text;
using CtSharpRedis;
using CtSharpRedis.CsRedis;
using CtSharpRedis.StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CtSharpRedisCoreTest
{
    public class TestBase
    {
        protected IServiceCollection serviceCollection;
        protected IAbstractRedisClient redisClient;

        protected IRedisDataBase defaultRds
        {
            get { return redisClient.GetDatabase(); }
        }

        protected IRedisDataBase defaultRds1
        {
            get { return redisClient.GetDatabase(1); }
        }

        protected readonly object Null = null;
        protected readonly string String = "我是中国人";
        protected readonly byte[] Bytes = Encoding.UTF8.GetBytes("这是一个byte字节");
        protected readonly TestInfo Info = new TestInfo { Id = 1, Name = "Class名称", CreateTime = DateTime.Now, TagId = new[] { 1, 3, 3, 3, 3 } };


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

        [TestInitialize]
        public void Init()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            //serviceCollection.AddSingleton<IRedisManager, StackExchangeRedisManager>();
            serviceCollection.AddSingleton<IRedisManager, CsRedisManager>();
            var redisManager = serviceCollection.BuildServiceProvider().GetService<IRedisManager>();
            redisClient=redisManager.Connect("192.168.1.209:10062,Password=gjn2ijj98jg745g10062,ConnectTimeout=5000,WriteBuffer=512,KeepAlive=120,ConnectRetry=2", redisEvent => { LogNotify(this, redisEvent); });
            //serviceCollection.AddSingleton<IAbstractRedisClient, CsRedisClient>();
            //redisClient = serviceCollection.BuildServiceProvider().GetService<IAbstractRedisClient>();
            //redisClient.CtRedisEventNotify += LogNotify;
            //redisClient.Connect("192.168.1.209:10062,Password=gjn2ijj98jg745g10062,ConnectTimeout=5000,WriteBuffer=512,KeepAlive=120,ConnectRetry=2");
        }

        public void LogNotify(object sender, CtRedisEvent @event)
        {
            Console.WriteLine("aa");
        }
    }
}
