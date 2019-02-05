using System;
using System.Collections.Concurrent;
using CSRedis;

namespace CtSharpRedis.CsRedis
{
    public class RedisClientFactory
    {
        internal enum ClientType
        {
            Common,

            Sub
        }

        private static readonly object SynObject = new object();


        static readonly ConcurrentDictionary<string, RedisClient> RedisPool = new ConcurrentDictionary<string, RedisClient>();

        internal static RedisClient GetDatabaseRedisClient(ClientType clientType, RedisConnectionOptions options, string channelName="",int dbIndex=-1, Action<string,Exception> eventNotify=null)
        {
            string clientName;
            if (clientType.Equals(ClientType.Sub))
            {
                clientName = ClientType.Sub + "_" + channelName;
            }
            else
            {
                clientName = ClientType.Common + "_" + dbIndex;
            }
            RedisClient client = null;
            if (RedisPool.TryGetValue(clientName, out client))
            {
                return client;
            }
            lock (SynObject)
            {
                client = new RedisClient(options.Host, options.Port);
                client.Connect(options);
                if (clientType.Equals(ClientType.Common))
                {
                    client.Select(dbIndex);
                }
                if (eventNotify != null)
                {
                    client.Connected += (sender, args) => { eventNotify($"CsRedis Connected,Host:{options.Host}|Port:{options.Port}", null); };
                }

                RedisPool.TryAdd(clientName, client);

                return client;
            }
        }

        internal static void ClearAllClient()
        {
            if (RedisPool.Count <= 0)
            {
                return;
            }

            foreach (var item in RedisPool)
            {
                item.Value.Dispose();
            }
        }
    }

}
