using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CSRedis;
using CtSharpRedis.Exceptions;
using CtSharpRedis.Utils;

namespace CtSharpRedis.CsRedis
{
    internal class CsRedisClient:IAbstractRedisClient
    {
        private RedisClient redisClient;

        private IRedisValueSerializeSettings serializeSettings = new DefaultRedisValueSerializeSettings();

        public CtSharpRedisException CtSharpRedisException { get; private set; }

        public event EventHandler<CtRedisEvent> CtRedisEventNotify;

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (redisClient == null)
                {
                    CtSharpRedisException = new CtSharpRedisException("redisClient 连接对象为空");
                    return false;
                }

                return redisClient.IsConnected;
            }
        }

        private RedisConnectionOptions options;

        public bool Connect(string connectionString)
        {
            options = RedisConnectionOptions.Parse(connectionString);
            redisClient =new RedisClient(options.Host, options.Port);
            redisClient.Connect(options);

            redisClient.Connected += (sender, args) => { EventNotify($"CsRedis Connected,Host:{options.Host}|Port:{options.Port}"); };
            return redisClient.IsConnected;
        }


        /// <summary>
        /// 获取DB
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public IRedisDataBase GetDatabase(int dbIndex = 0)
        {
            if (redisClient == null)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }

            var client = RedisClientFactory.GetDatabaseRedisClient(RedisClientFactory.ClientType.Common, options, "", dbIndex, EventNotify);
            return new RedisDataBase(client, serializeSettings);
        }

        #region Pub/Sub

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish(string channelName, object message)
        {
            if (redisClient == null)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }
            if (!redisClient.IsConnected)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }

            return redisClient.Publish(channelName, serializeSettings.SerializeRedisValue(message));
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="action"></param>
        public void Subscribe(string channelName, Action<string, string> action)
        {
            Task.Run(() =>
            {
                RedisClient client = RedisClientFactory.GetDatabaseRedisClient(RedisClientFactory.ClientType.Sub, options, channelName, -1, EventNotify);
                if (!client.IsConnected)
                {
                    throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
                }

                client.SubscriptionReceived += (sender, eventArg) =>
                {
                    if (eventArg == null || eventArg.Message == null)
                    {
                        return;
                    }

                    var message = eventArg.Message;
                    string cname = message.Channel;
                    string msg = message.Body;
                    try
                    {
                        // 模糊匹配
                        if (cname.Contains("*"))
                        {
                            action(cname, msg);
                        }
                        else
                        {
                            if (cname.Equals(channelName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                action(cname, msg);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        EventNotify($"Subscribe channel {cname} error", e);
                    }
                };
                if (channelName.Contains("*"))
                {
                    client.PSubscribe(channelName);
                }
                else
                {
                    client.Subscribe(channelName);
                }
            });
        }

        #endregion

        private void EventNotify(string message, Exception exception = null)
        {
            if (CtRedisEventNotify != null)
            {
                CtRedisEvent redisEvent = new CtRedisEvent
                {
                    Message = message,
                    Exception = exception
                };
                CtRedisEventNotify(this, redisEvent);
            }
        }

        public void Dispose()
        {
            if (redisClient != null)
            {
                redisClient.Dispose();
            }
            RedisClientFactory.ClearAllClient();
        }
    }
}
