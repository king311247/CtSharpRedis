using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CtSharpRedis.StackExchange.Redis
{
    public class StackExchangeRedisManager: IRedisManager
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private static readonly IDictionary<string, StackExchangeRedisClient> ConnectionDic = new ConcurrentDictionary<string, StackExchangeRedisClient>();

        /// <summary>
        /// 同步锁对象
        /// </summary>
        private static readonly object ConnectLock = new object();
        public IAbstractRedisClient Connect(string connectionString,Action<CtRedisEvent> action=null)
        {
            lock (ConnectLock)
            {
                StackExchangeRedisClient connection;
                if (!ConnectionDic.TryGetValue(connectionString, out connection))
                {
                    connection=new StackExchangeRedisClient();
                    if (action != null)
                    {
                        connection.CtRedisEventNotify += (sender, args) => { action(args); };
                    }
                    connection.Connect(connectionString);
                    ConnectionDic.Add(connectionString, connection);
                }

                return connection;
            }
        }
    }
}
