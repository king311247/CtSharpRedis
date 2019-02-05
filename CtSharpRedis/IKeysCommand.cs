using System;
using CtSharpRedis.Enums;

namespace CtSharpRedis
{
    public interface IKeysCommand
    {
        /// <summary>
        /// 返回指定key对应的value自被存储之后空闲的时间，以秒为单位(
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/object
        /// </remarks>
        /// <returns></returns>
        TimeSpan? KeyIdleTime(string key);

        /// <summary>
        /// 返回key剩余的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/ttl
        /// </remarks>
        /// <returns></returns>
        TimeSpan? KeyTimeToLive(string key);

        /// <summary>
        /// 删除指定的一批keys，如果删除中的某些key不存在，则直接忽略
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/del
        /// </remarks>
        /// <returns></returns>
        bool KeyDelete(string key);

        /// <summary>
        /// 删除指定的一批keys，如果删除中的某些key不存在，则直接忽略
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks>
        ///https://redis.io/commands/del
        /// </remarks>
        /// <returns></returns>
        long KeyDelete(string[] keys);

        /// <summary>
        /// 序列化给定 key ，并返回被序列化的值
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/dump
        /// </remarks>
        /// <returns></returns>
        byte[] KeyDump(string key);

        /// <summary>
        /// 返回key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/exists
        /// </remarks>
        /// <returns></returns>
        bool KeyExists(string key);

        /// <summary>
        /// 设置key的过期时间，超过时间后，将会自动删除该key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <remarks>
        ///https://redis.io/commands/expire
        /// </remarks>
        /// <returns></returns>
        bool KeyExpire(string key, TimeSpan expire);

        /// <summary>
        /// 设置key的过期时间，超过时间后，将会自动删除该key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <remarks>
        ///https://redis.io/commands/expireat
        /// </remarks>
        /// <returns></returns>
        bool KeyExpire(string key, DateTime expire);

        ///  <summary>
        ///  将key重命名为newkey，如果key与newkey相同，将返回一个错误
        ///  </summary>
        ///  <param name="key"></param>
        ///  <param name="newKey"></param>
        /// <param name="when"></param>
        /// <remarks>
        /// https://redis.io/commands/rename
        ///  </remarks>
        ///  <returns></returns>
        bool KeyRename(string key, string newKey, CtSharpWhen when = CtSharpWhen.Always);

        /// <summary>
        /// 移除给定key的生存时间
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/persist
        /// </remarks>
        /// <returns></returns>
        bool KeyPersist(string key);

        /// <summary>
        /// 将当前数据库的 key 移动到给定的数据库 db 当中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="destinationDbIndex"></param>
        /// <remarks>
        ///https://redis.io/commands/move
        /// </remarks>
        /// <returns></returns>
        bool KeyMove(string key, int destinationDbIndex);
    }
}
