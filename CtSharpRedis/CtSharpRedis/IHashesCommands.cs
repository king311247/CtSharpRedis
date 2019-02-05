#region

using System.Collections.Generic;
using CtSharpRedis.Enums;

#endregion

namespace CtSharpRedis
{
    /// <summary>
    ///     哈希操作
    /// </summary>
    public interface IHashesCommands
    {
        /// <summary>
        /// 删除一个hash的field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <remarks>
        /// https://redis.io/commands/hdel
        /// </remarks>
        /// <returns></returns>
        bool HashDelete(string key, CtSharpRedisValue field);

        /// <summary>
        /// 删除多个hash的field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <remarks>
        /// https://redis.io/commands/hdel
        /// </remarks>
        /// <returns>返回从哈希集中成功移除的域的数量</returns>
        long HashDelete(string key, CtSharpRedisValue[] fields);

        /// <summary>
        /// 判断field是否存在于hash中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <remarks>
        /// https://redis.io/commands/hexists
        /// </remarks>
        /// <returns></returns>
        bool HashExists(string key, CtSharpRedisValue field);

        /// <summary>
        /// 获取指定的hash集中指定字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <remarks>
        ///https://redis.io/commands/hget
        /// </remarks>
        /// <returns></returns>
        T HashGet<T>(string key, CtSharpRedisValue field);

        /// <summary>
        /// 获取指定的hash集中指定字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <remarks>
        ///https://redis.io/commands/hmget
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] HashGet(string key, CtSharpRedisValue[] fields);

        /// <summary>
        /// 从hash中读取全部的域和值
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        /// https://redis.io/commands/hgetall
        /// </remarks>
        /// <returns></returns>
        KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] HashGetAll(string key);

        /// <summary>
        /// 将hash中指定域的值增加给定的数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/hincrby
        /// </remarks>
        /// <returns></returns>
        long HashIncrement(string key, CtSharpRedisValue field, long value);

        /// <summary>
        /// 将hash中指定域的值增加给定的浮点数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/hincrbyfloat
        /// </remarks>
        /// <returns></returns>
        double HashIncrement(string key, CtSharpRedisValue field, double value);

        /// <summary>
        /// 将hash中指定域的值增加给定的数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/hincrby
        /// </remarks>
        /// <returns></returns>
        long HashDecrement(string key, CtSharpRedisValue field, long value);

        /// <summary>
        /// 将hash中指定域的值增加给定的浮点数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/hincrbyfloat
        /// </remarks>
        /// <returns></returns>
        double HashDecrement(string key, CtSharpRedisValue field, double value);

        /// <summary>
        /// 获取hash的所有字段
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        /// https://redis.io/commands/hkeys
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] HashKeys(string key);

        /// <summary>
        /// 设置hash里面一个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="when"></param>
        /// <remarks>
        ///when = CtSharpWhen.Always:https://redis.io/commands/hset
        /// when = CtSharpWhen.NotExists:https://redis.io/commands/hsetnx
        /// </remarks>
        /// <returns></returns>
        bool HashSet<TValue>(string key, CtSharpRedisValue field, TValue value, CtSharpWhen when = CtSharpWhen.Always);

        /// <summary>
        /// 设置hash里面多个field的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldValues"></param>
        /// <remarks>
        ///https://redis.io/commands/hmset
        /// </remarks>
        void HashSet(string key, KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] fieldValues);

        /// <summary>
        /// 获取hash里所有字段的数量
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/hlen
        /// </remarks>
        /// <returns></returns>
        long HashLength(string key);
    }
}