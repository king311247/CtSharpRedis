namespace CtSharpRedis
{
    public interface ISetsCommands
    {
        /// <summary>
        /// 添加一个元素到集合(set)里
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sadd
        /// </remarks>
        /// <returns></returns>
        bool SetAdd<T>(string key, T value);


        /// <summary>
        /// 添加多个元素到集合(set)里
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sadd
        /// </remarks>
        /// <returns></returns>
        long SetAdd(string key, CtSharpRedisValue[] values);


        /// <summary>
        /// 获取集合里面的元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///http://www.redis.io/commands/scard
        /// </remarks>
        /// <returns></returns>
        long SetLength(string key);


        /// <summary>
        /// 返回一个集合与给定集合的差集的元素
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sdiff
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetCombineDiff(string firstKey, string secondKey);


        /// <summary>
        /// 返回一个集合与给定集合的差集的元素
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sdiff
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetCombineDiff(string[] keys);


        /// <summary>
        /// 返回指定所有的集合的成员的交集
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sinter
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetCombineInter(string firstKey, string secondKey);


        /// <summary>
        /// 返回指定所有的集合的成员的交集
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sinter
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetCombineInter(string[] keys);


        /// <summary>
        /// 返回给定的多个集合的并集中的所有成员
        /// </summary>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sunion
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetCombineUnion(string firstKey, string secondKey);


        /// <summary>
        /// 返回给定的多个集合的并集中的所有成员
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sunion
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetCombineUnion(string[] keys);


        ///  <summary>
        ///  类似于 SDIFF, 不同之处在于该命令不返回结果集，而是将结果存放在destination集合中
        ///  </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        ///  <param name="secondKey"></param>
        /// <remarks>
        /// http://www.redis.io/commands/sdiffstore
        /// </remarks>
        ///  <returns>结果集元素的个数</returns>
        long SetCombineAndStoreDiff(string destinationKey, string firstKey, string secondKey);


        /// <summary>
        /// 类似于 SDIFF, 不同之处在于该命令不返回结果集，而是将结果存放在destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <remarks>
        /// http://www.redis.io/commands/sdiffstore
        /// </remarks>
        ///  <returns>结果集元素的个数</returns>
        long SetCombineAndStoreDiff(string destinationKey, string[] keys);


        /// <summary>
        /// 与SINTER命令类似, 但是它并不是直接返回结果集,而是将结果保存在 destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <remarks>
        /// http://www.redis.io/commands/sinterstore
        /// </remarks>
        ///  <returns>结果集元素的个数</returns>
        long SetCombineAndStoreInter(string destinationKey, string firstKey, string secondKey);


        /// <summary>
        /// 与SINTER命令类似, 但是它并不是直接返回结果集,而是将结果保存在 destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <remarks>
        /// http://www.redis.io/commands/sinterstore
        /// </remarks>
        ///  <returns>结果集元素的个数</returns>
        long SetCombineAndStoreInter(string destinationKey, string[] keys);


        /// <summary>
        /// 类似于SUNION命令,不同的是它并不返回结果集,而是将结果存储在destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <remarks>
        /// http://www.redis.io/commands/sunionstore
        /// </remarks>
        ///  <returns>结果集元素的个数</returns>
        long SetCombineAndStoreUnion(string destinationKey, string firstKey, string secondKey);


        /// <summary>
        /// 类似于SUNION命令,不同的是它并不返回结果集,而是将结果存储在destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <remarks>
        /// http://www.redis.io/commands/sunionstore
        /// </remarks>
        ///  <returns>结果集元素的个数</returns>
        long SetCombineAndStoreUnion(string destinationKey, string[] keys);


        /// <summary>
        /// 返回key集合所有的元素
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///http://www.redis.io/commands/smembers
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetMembers(string key);


        /// <summary>
        /// value 是否是存储的集合 key的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///http://www.redis.io/commands/sismember
        /// </remarks>
        /// <returns></returns>
        bool SetContains<T>(string key, T value);

        /// <summary>
        /// 将指定value 从source集合移动到destination集合中
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <param name="destinationKey"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///http://www.redis.io/commands/smove
        /// </remarks>
        /// <returns>成功移除,返回true;元素不是 source集合成员,无任何操作,返还false</returns>
        bool SetMove<T>(string sourceKey, string destinationKey, T value);


        /// <summary>
        /// 读取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <remarks>
        ///http://www.redis.io/commands/spop
        /// </remarks>
        /// <returns></returns>
        T SetPop<T>(string key);


        /// <summary>
        /// 老版本csredis不支持取多个
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <remarks>
        ///http://www.redis.io/commands/spop
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetPop(string key, long count);


        /// <summary>
        /// 随机获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <remarks>
        ///http://www.redis.io/commands/srandmember
        /// </remarks>
        /// <returns></returns>
        T SetRandomMember<T>(string key);


        /// <summary>
        /// 随机获取多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <remarks>
        ///http://www.redis.io/commands/srandmember
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SetRandomMembers(string key, long count);


        /// <summary>
        /// 在key集合中移除指定的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///http://www.redis.io/commands/srem
        /// </remarks>
        /// <returns></returns>
        bool SetRemove<T>(string key, T value);


        /// <summary>
        /// 在key集合中移除指定的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <remarks>
        ///http://www.redis.io/commands/srem
        /// </remarks>
        /// <returns>返回移除个数</returns>
        long SetRemove(string key, CtSharpRedisValue[] values);
    }
}
