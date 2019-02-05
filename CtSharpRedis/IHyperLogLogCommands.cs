namespace CtSharpRedis
{
    public interface IHyperLogLogCommands
    {
        /// <summary>
        /// HyperLogLog添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/pfadd
        /// </remarks>
        /// <returns></returns>
        bool HyperLogLogAdd<T>(string key, T value);

        /// <summary>
        /// HyperLogLog添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <remarks>
        ///https://redis.io/commands/pfadd
        /// </remarks>
        /// <returns></returns>
        bool HyperLogLogAdd(string key, CtSharpRedisValue[] values);


        /// <summary>
        /// HyperLogLog长度计算
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/pfcount
        /// </remarks>
        /// <returns></returns>
        long HyperLogLogLength(string key);


        /// <summary>
        /// HyperLogLog长度计算
        /// </summary>
        /// <param name="keys"></param>
        /// <remarks>
        ///https://redis.io/commands/pfcount
        /// </remarks>
        /// <returns></returns>
        long HyperLogLogLength(string[] keys);

        /// <summary>
        /// HyperLogLog合并
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <remarks>
        ///https://redis.io/commands/pfmerge
        /// </remarks>
        void HyperLogLogMerge(string destinationKey, string firstKey, string secondKey);


        ///  <summary>
        ///  HyperLogLog合并
        ///  </summary>
        ///  <param name="destinationKey"></param>
        /// <param name="mergeKeys"></param>
        /// <remarks>
        /// https://redis.io/commands/pfmerge
        ///  </remarks>
        void HyperLogLogMerge(string destinationKey, string[] mergeKeys);
    }
}
