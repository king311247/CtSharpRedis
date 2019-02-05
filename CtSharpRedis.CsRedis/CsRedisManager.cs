using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CtSharpRedis.CsRedis
{
    public class CsRedisManager: IRedisManager
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private static readonly ConcurrentDictionary<string, CsRedisClient> ConnectionDic = new ConcurrentDictionary<string, CsRedisClient>();

        /// <summary>
        /// 同步锁对象
        /// </summary>
        private static readonly object ConnectLock = new object();

        public IAbstractRedisClient Connect(string connectionString, Action<CtRedisEvent> action=null)
        {
            lock (ConnectLock)
            {
                CsRedisClient connection;
                if (!ConnectionDic.TryGetValue(connectionString, out connection))
                {
                    connection = new CsRedisClient();
                    if (action != null)
                    {
                        connection.CtRedisEventNotify += (sender, args) => { action(args); };
                    }

                    connection.Connect(connectionString);
                    ConnectionDic.TryAdd(connectionString, connection);
                }

                return connection;
            }
        }
    }
}
