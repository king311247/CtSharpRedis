using System.Globalization;
using CtSharpRedis.Enums;

namespace CtSharpRedis
{
    public interface IListsCommands
    {
        /// <summary>
        /// 获取一个元素，通过其索引列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <remarks>
        ///https://redis.io/commands/lindex
        /// </remarks>
        /// <returns></returns>
        T ListGetByIndex<T>(string key, long index);

        /// <summary>
        /// 在列表中的另一个元素之后插入一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/linsert
        /// </remarks>
        /// <returns></returns>
        long ListInsertAfter<TPivot,TValue>(string key, TPivot pivot, TValue value);


        /// <summary>
        /// 在列表中的另一个元素之前插入一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/linsert
        /// </remarks>
        /// <returns></returns>
        long ListInsertBefore<TPivot, TValue>(string key, TPivot pivot, TPivot value);


        /// <summary>
        /// 获得队列(List)的长度
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/llen
        /// </remarks>
        /// <returns></returns>
        long ListLength(string key);


        /// <summary>
        /// 从队列的左边出队一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/lpop
        /// </remarks>
        /// <returns></returns>
        T ListLeftPop<T>(string key);

        ///  <summary>
        ///  从队列的左边入队一个元素
        ///  </summary>
        ///  <param name="key"></param>
        ///  <param name="value"></param>
        /// <param name="when"></param>
        /// <remarks>
        /// when = CtSharpWhen.Alway:https://redis.io/commands/lpush
        /// when = CtSharpWhen.Exists:https://redis.io/commands/lpushx
        ///  </remarks>
        ///  <returns></returns>
        long ListLeftPush<T>(string key, T value, CtSharpWhen when = CtSharpWhen.Always);


        /// <summary>
        /// 从队列的左边入队多个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <remarks>
        /// https://redis.io/commands/lpush
        /// </remarks>
        /// <returns></returns>
        long ListLeftPush(string key, CtSharpRedisValue[] values);


        /// <summary>
        /// 从列表中获取指定返回的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        /// <remarks>
        ///https://redis.io/commands/lrange
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] ListRange(string key, long startIndex, long stopIndex);


        /// <summary>
        /// 从列表中删除元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <remarks>
        ///https://redis.io/commands/lrem
        /// </remarks>
        /// <returns></returns>
        long ListRemove<T>(string key, T value, long count);

        /// <summary>
        /// 设置队列里面一个元素的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/lset
        /// </remarks>
        void ListSetByIndex<T>(string key, long index, T value);


        /// <summary>
        /// 修剪到指定范围内的清单
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        /// <remarks>
        ///https://redis.io/commands/ltrim
        /// </remarks>
        void ListTrim(string key, long startIndex, long stopIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/rpop
        /// </remarks>
        /// <returns></returns>
        T ListRightPop<T>(string key);


        /// <summary>
        /// 删除列表中的最后一个元素，将其追加到另一个列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="destinationKey"></param>
        /// <remarks>
        ///https://redis.io/commands/rpoplpush
        /// </remarks>
        /// <returns></returns>
        T ListRightPopLeftPush<T>(string key, string destinationKey);


        /// <summary>
        /// 从队列的右边入队一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="when"></param>
        /// <remarks>
        /// when = CtSharpWhen.Alway:https://redis.io/commands/rpush
        /// when = CtSharpWhen.Exists:https://redis.io/commands/rpushx
        ///  </remarks>
        ///  <returns></returns>
        long ListRightPush<T>(string key, T value,CtSharpWhen when=CtSharpWhen.Always);


        /// <summary>
        /// 从队列的右边入队多个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        long ListRightPush(string key, CtSharpRedisValue[] values);
    }
}