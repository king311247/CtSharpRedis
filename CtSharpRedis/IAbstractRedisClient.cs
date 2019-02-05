using System;
using CtSharpRedis.Exceptions;

namespace CtSharpRedis
{
    public interface IAbstractRedisClient : IDisposable
    {
        CtSharpRedisException CtSharpRedisException { get; }

        /// <summary>
        /// 日志通知事件
        /// </summary>
        event EventHandler<CtRedisEvent> CtRedisEventNotify;

        /// <summary>
        /// 连接状态
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        bool Connect(string connectionString);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        long Publish(string channelName, object message);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="action"></param>
        void Subscribe(string channelName, Action<string, string> action);

        /// <summary>
        /// 获取DB
        /// </summary>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        IRedisDataBase GetDatabase(int dbIndex = 0);
    }
}
