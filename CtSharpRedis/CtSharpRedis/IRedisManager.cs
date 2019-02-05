using System;

namespace CtSharpRedis
{
    public interface IRedisManager
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IAbstractRedisClient Connect(string connectionString, Action<CtRedisEvent> action=null);
    }
}
