using System;
using System.Collections.Generic;
using CtSharpRedis.Enums;

namespace CtSharpRedis
{
    public interface IStringsCommands
    {
        /// <summary>
        /// 如果 key 已经存在，并且值为字符串，那么这个命令会把 value 追加到原来值（value）的结尾。 如果 key 不存在，那么它将首先创建一个空字符串的key，再执行追加操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/append
        /// </remarks>
        /// <returns></returns>
        long StringAppend<T>(string key, T value);

        /// <summary>
        /// 统计字符串被设置为1的bit数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        /// <remarks>
        ///https://redis.io/commands/bitcount
        /// </remarks>
        /// <returns></returns>
        long StringBitCount(string key, long startIndex, long stopIndex);

        /// <summary>
        /// 对一个或多个保存二进制位的字符串 key 进行位元操作，并将结果保存到 destkey 上
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <param name="bitwise"></param>
        /// <remarks>
        ///https://redis.io/commands/bitop
        /// </remarks>
        /// <returns></returns>
        long StringBitOperation(string destinationKey, string firstKey, string secondKey, RedisBitwise bitwise = RedisBitwise.And);


        /// <summary>
        /// 对一个或多个保存二进制位的字符串 key 进行位元操作，并将结果保存到 destkey 上
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <param name="bitwise"></param>
        /// <remarks>
        ///https://redis.io/commands/bitop
        /// </remarks>
        /// <returns></returns>
        long StringBitOperation(string destinationKey, string[] keys, RedisBitwise bitwise = RedisBitwise.And);

        /// <summary>
        /// 返回字符串里面第一个被设置为1或者0的bit位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <remarks>
        ///https://redis.io/commands/bitpos
        /// </remarks>
        /// <returns></returns>
        long StringBitPosition(string key, bool bit, long start = 0, long end = -1);

        /// <summary>
        /// 将key对应的数字减decrement
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/decrby
        /// </remarks>
        long StringDecrement(string key, long value = 1);

        /// <summary>
        /// 将key对应的数字减decrement
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/incrbyfloat
        /// </remarks>
        double StringDecrement(string key, double value);

        /// <summary>
        /// 返回key的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <remarks>
        /// https://redis.io/commands/get
        /// </remarks>
        /// <returns></returns>
        T StringGet<T>(string key);

        /// <summary>
        ///批量返回key的value
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks>
        ///https://redis.io/commands/mget
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] StringGet(string[] keys);

        /// <summary>
        /// 返回key对应的string在offset处的bit值 当offset超出了字符串长度的时候，这个字符串就被假定为由0比特填充的连续空间。当key不存在的时候，它就认为是一个空字符串，所以offset总是超出范围，然后value也被认为是由0比特填充的连续空间。到内存分配
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <remarks>
        ///https://redis.io/commands/getbit
        /// </remarks>
        /// <returns></returns>
        bool StringGetBit(string key, long offset);

        /// <summary>
        /// 通过下标获取字符串内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <remarks>
        ///https://redis.io/commands/getrange
        /// </remarks>
        /// <returns></returns>
        T StringGetRange<T>(string key, long start, long end);

        /// <summary>
        /// 自动将key对应到value并且返回原来key对应的value。如果key存在但是对应的value不是字符串，就返回错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/getset
        /// </remarks>
        /// <returns></returns>
        T StringGetSet<T>(string key, T value);

        /// <summary>
        /// 将key对应的数字加decrement
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/incrby
        /// </remarks>
        /// <returns></returns>
        long StringIncrement(string key, long value = 1);

        /// <summary>
        /// 将key对应的数字加decrement
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/incrbyfloat
        /// </remarks>
        /// <returns></returns>
        double StringIncrement(string key, double value = 1);

        /// <summary>
        /// 返回内容长度
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/strlen
        /// </remarks>
        /// <returns></returns>
        long StringLength(string key);

        /// <summary>
        /// 将键key设定为指定的“字符串”值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <remarks>
        ///https://redis.io/commands/set
        /// </remarks>
        /// <returns></returns>

        bool StringSet<T>(string key, T value, TimeSpan? expiry = null, CtSharpWhen when = CtSharpWhen.Always);

        /// <summary>
        /// 批量将键key设定为指定的“字符串”值
        /// </summary>
        /// <param name="keyValues"></param>
        /// <param name="when"></param>
        /// <remarks>
        ///https://redis.io/commands/mset
        /// </remarks>
        /// <returns></returns>
        bool StringSet(KeyValuePair<string, CtSharpRedisValue>[] keyValues, CtSharpWhen when = CtSharpWhen.Always);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        bool StringSetBit(string key, long offset, bool bit);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long StringSetRange<T>(string key, long offset, T value);
    }
}
