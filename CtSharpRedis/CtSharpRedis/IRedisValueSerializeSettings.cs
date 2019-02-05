using System;
using System.Collections.Generic;
using System.Text;

namespace CtSharpRedis
{
    /// <summary>
    /// 序列化工具
    /// </summary>
    public interface IRedisValueSerializeSettings
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string SerializeRedisValue(object value);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        object DeserializeRedisValue<T>(string value);
    }
}
