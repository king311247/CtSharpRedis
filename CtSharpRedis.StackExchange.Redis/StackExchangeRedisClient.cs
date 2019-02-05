#region

using System;
using System.Globalization;
using System.IO;
using System.Text;
using CtSharpRedis.Exceptions;
using CtSharpRedis.Utils;
using StackExchange.Redis;

#endregion

namespace CtSharpRedis.StackExchange.Redis
{
    internal class StackExchangeRedisClient :IAbstractRedisClient
    {
        private IConnectionMultiplexer connectionMultiplexer;

        private readonly IRedisValueSerializeSettings serializeSettings = new DefaultRedisValueSerializeSettings();


        public CtSharpRedisException CtSharpRedisException { get; private set; }

        public event EventHandler<CtRedisEvent> CtRedisEventNotify;


        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (connectionMultiplexer == null)
                {
                    CtSharpRedisException=new CtSharpRedisException("IConnectionMultiplexer 连接对象为空");
                    return false;
                }

                return connectionMultiplexer.IsConnected;
            }
        }

        public bool Connect(string connectionString)
        {
            var builder = new StringBuilder();
            using (var log = new StringWriter(builder, CultureInfo.InvariantCulture))
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString, log);
            }

            connectionMultiplexer.IncludeDetailInExceptions = true;
            connectionMultiplexer.ConfigurationChangedBroadcast += (sender, args) =>
            {
                EventNotify("ConfigurationChangedBroadcast," + args.EndPoint.Serialize());
            };
            connectionMultiplexer.HashSlotMoved += (sender, args) =>
            {
                EventNotify($"HashSlotMoved,HashSlot:{args.HashSlot}|OldEndPoint:{args.OldEndPoint.Serialize()}|OldEndPoint:{args.NewEndPoint.Serialize()}");
            };
            connectionMultiplexer.ConfigurationChanged += (sender, args) => { EventNotify($"ConfigurationChanged,{args.EndPoint.Serialize()}"); };
            connectionMultiplexer.ConnectionRestored += (sender, args) => { EventNotify($"ConnectionRestored,{args.ToString()}", args.Exception); };
            connectionMultiplexer.ConnectionFailed += (sender, args) => { EventNotify($"ConnectionFailed,{args.ToString()}", args.Exception); };
            connectionMultiplexer.InternalError += (sender, args) => { EventNotify($"InternalError,Origin:{args.Origin}|EndPoint:{args.EndPoint.Serialize()}", args.Exception); };

            if (!connectionMultiplexer.IsConnected)
            {
                EventNotify("Redis连接失败:" + builder);
            }

            return connectionMultiplexer.IsConnected;
        }

        /// <summary>
        /// 获取DB
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public IRedisDataBase GetDatabase(int dbIndex = 0)
        {
            if (connectionMultiplexer == null)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }
            IDatabase database= connectionMultiplexer.GetDatabase(dbIndex);
            return new RedisDataBase(database, serializeSettings);
        }

        #region Pub/Sub

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public long Publish(string channelName,object message)
        {
            if (connectionMultiplexer == null)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }
            if (!connectionMultiplexer.IsConnected)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }
            return connectionMultiplexer.GetSubscriber().Publish(channelName, serializeSettings.SerializeRedisValue(message));
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="action"></param>
        public void Subscribe(string channelName,Action<string,string> action)
        {
            if (connectionMultiplexer == null)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }
            if (!connectionMultiplexer.IsConnected)
            {
                throw new CtSharpRedisException("redis 未连接,请先执行 Connect方法");
            }
            connectionMultiplexer.GetSubscriber().Subscribe(channelName, (channel, message) =>
            {
                try
                {
                    string cname = channelName;
                    string msg = message;
                    action(cname, msg);
                }
                catch (Exception e)
                {
                    EventNotify($"Subscribe channel {channelName} error", e);
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
            try
            {
                if (connectionMultiplexer != null)
                {
                    if (connectionMultiplexer.IsConnected)
                    {
                        connectionMultiplexer.Close();
                    }
                    connectionMultiplexer.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new CtSharpRedisException("StackExchangeRedisClient 释放异常", e);
            }

        }
    }
}