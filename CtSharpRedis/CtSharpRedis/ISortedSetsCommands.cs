using System.Collections.Generic;
using CtSharpRedis.Enums;

namespace CtSharpRedis
{
    public interface ISortedSetsCommands
    {
        /// <summary>
        /// 将所有指定成员添加到键为key有序集合（sorted set）里面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <param name="when"></param>
        /// <remarks>
        /// https://redis.io/commands/sadd
        ///when = CtSharpWhen.Always 更改的元素是新添加的成员，已经存在的成员更新分数
        /// when = CtSharpWhen.Exists 仅仅更新存在的成员，不添加新成员
        /// when = CtSharpWhen.NotExists 不更新存在的成员。只添加新成员
        /// </remarks>
        /// <returns></returns>
        bool SortedSetAdd<T>(string key, T value, double score, CtSharpWhen when = CtSharpWhen.Always);


        ///  <summary>
        ///  将所有指定成员添加到键为key有序集合（sorted set）里面
        ///  </summary>
        ///  <typeparam name="T"></typeparam>
        ///  <param name="key"></param>
        ///  <param name="score"></param>
        /// <param name="scoreValues"></param>
        /// <param name="when"></param>
        /// <remarks>
        /// https://redis.io/commands/sadd
        ///when = CtSharpWhen.Always 更改的元素是新添加的成员，已经存在的成员更新分数
        /// when = CtSharpWhen.Exists 仅仅更新存在的成员，不添加新成员
        /// when = CtSharpWhen.NotExists 不更新存在的成员。只添加新成员
        /// </remarks>
        /// <returns></returns>
        long SortedSetAdd(string key, KeyValuePair<CtSharpRedisValue, double>[] scoreValues, CtSharpWhen when = CtSharpWhen.Always);


        /// <summary>
        /// 返回key的有序集元素个数
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        ///https://redis.io/commands/zcard
        /// </remarks>
        /// <returns></returns>
        long SortedSetLength(string key);


        /// <summary>
        /// 返回有序集key中，score值在min和max之间(默认包括score值等于min或max)的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <param name="exclusive"></param>
        /// <remarks>
        ///https://redis.io/commands/zcount
        /// </remarks>
        /// <returns></returns>
        long SortedSetLength(string key, double minScore, double maxScore, Exclusive exclusive=Exclusive.None);


        /// <summary>
        /// 为有序集key的成员member的score值加上增量increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="incScore"></param>
        /// <remarks>
        ///https://redis.io/commands/zincrby
        /// </remarks>
        /// <returns></returns>
        double SortedSetIncrement<T>(string key, T value, double incScore);


        /// <summary>
        /// 为有序集key的成员member的score值加上增量increment
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="incScore"></param>
        /// <remarks>
        ///https://redis.io/commands/zincrby
        /// </remarks>
        /// <returns></returns>
        double SortedSetDecrement<T>(string key, T value, double incScore);


        /// <summary>
        /// 计算给定的numkeys个有序集合的交集，并且把结果放到destination中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <param name="aggregate"></param>
        /// <remarks>
        ///https://redis.io/commands/zinterstore
        /// </remarks>
        /// <returns></returns>
        long SortedSetCombineAndStoreInter(string destinationKey, string firstKey, string secondKey, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum);


        /// <summary>
        /// 计算给定的numkeys个有序集合的交集，并且把结果放到destination中
        /// 使用WEIGHTS选项，你可以为每个给定的有序集指定一个乘法因子，意思就是，每个给定有序集的所有成员的score值在传递给聚合函数之前都要先乘以该因子。如果WEIGHTS没有给定，默认就是1
        ///默认使用的参数SUM，可以将所有集合中某个成员的score值之和作为结果集中该成员的score值。如果使用参数MIN或者MAX，结果集就是所有集合中元素最小或最大的元素。
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <remarks>
        ///https://redis.io/commands/zinterstore
        /// </remarks>
        /// <returns></returns>
        long SortedSetCombineAndStoreInter(string destinationKey, string[] keys, double[] weights, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum);


        /// <summary>
        /// 计算给定的numkeys个有序集合的并集，并且把结果放到destination中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <param name="aggregate"></param>
        /// <remarks>
        ///https://redis.io/commands/zunionstore
        /// </remarks>
        /// <returns></returns>
        long SortedSetCombineAndStoreUnion(string destinationKey, string firstKey, string secondKey, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum);


        /// <summary>
        /// 计算给定的numkeys个有序集合的并集，并且把结果放到destination中
        /// 使用WEIGHTS选项，你可以为每个给定的有序集指定一个乘法因子，意思就是，每个给定有序集的所有成员的score值在传递给聚合函数之前都要先乘以该因子。如果WEIGHTS没有给定，默认就是1
        ///默认使用的参数SUM，可以将所有集合中某个成员的score值之和作为结果集中该成员的score值。如果使用参数MIN或者MAX，结果集就是所有集合中元素最小或最大的元素。
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <remarks>
        ///https://redis.io/commands/zunionstore
        /// </remarks>
        /// <returns></returns>
        long SortedSetCombineAndStoreUnion(string destinationKey, string[] keys, double[] weights, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum);


        ///  <summary>
        ///  用于计算有序集合中指定成员之间的成员数量
        ///  </summary>
        ///  <param name="key"></param>
        ///  <param name="minValue"></param>
        ///  <param name="maxValue"></param>
        /// <param name="exclusive"></param>
        /// <remarks>
        /// https://redis.io/commands/zlexcount
        ///  </remarks>
        ///  <returns></returns>
        long SortedSetLengthByValue<TMinMember, TMaxMember>(string key, TMinMember minValue, TMaxMember maxValue, Exclusive exclusive = Exclusive.None);


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从小到大排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <remarks>
        ///https://redis.io/commands/zrange
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SortedSetRangeByRankAsc(string key, long start, long stop);


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从大到小排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <remarks>
        ///https://redis.io/commands/zrevrange
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SortedSetRangeByRankDesc(string key, long start, long stop);


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从大到小排序(ZRANGEBYLEX)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minMember"></param>
        /// <param name="maxMember"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="exclusive"></param>
        /// <remarks>
        ///https://redis.io/commands/zrangebylex
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SortedSetRangeByValueAsc<TMinMember,TMaxMember>(string key, TMinMember minMember, TMaxMember maxMember, long offset, long count, Exclusive exclusive=Exclusive.None);


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从大到小排序(ZRANGEBYLEX)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="maxMember"></param>
        /// <param name="minMember"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="exclusive"></param>
        /// <remarks>
        ///https://redis.io/commands/zrevrangebylex
        /// </remarks>
        /// <returns></returns>
        CtSharpRedisValue[] SortedSetRangeByValueDesc<TMaxMember, TMinMember>(string key, TMaxMember maxMember, TMinMember minMember, long offset, long count , Exclusive exclusive = Exclusive.None);

        ///  <summary>
        ///  返回指定范围的元素,返回元素按分值从大到小排序(ZRANGEBYLEX)
        ///  </summary>
        ///  <param name="key"></param>
        /// <param name="maxScore"></param>
        /// <param name="offset"></param>
        ///  <param name="count"></param>
        ///  <param name="exclusive"></param>
        /// <param name="minScore"></param>
        /// <remarks>
        /// https://redis.io/commands/zrangebylex
        ///  </remarks>
        ///  <returns></returns>
        CtSharpRedisValue[] SortedSetRangeByScoreAsc(string key, double minScore, double maxScore, long offset, long count, Exclusive exclusive = Exclusive.None);


        ///  <summary>
        ///  返回指定范围的元素,返回元素按分值从大到小排序(ZRANGEBYLEX)
        ///  </summary>
        ///  <param name="key"></param>

        /// <param name="minScore"></param>
        /// <param name="offset"></param>
        ///  <param name="count"></param>
        ///  <param name="exclusive"></param>
        /// <param name="maxScore"></param>
        /// <remarks>
        /// https://redis.io/commands/zrevrangebylex
        ///  </remarks>
        ///  <returns></returns>
        CtSharpRedisValue[] SortedSetRangeByScoreDesc(string key, double maxScore, double minScore, long offset, long count, Exclusive exclusive = Exclusive.None);

        ///  <summary>
        ///  返回指定范围的元素,返回元素按分值从大到小排序(ZRANGEBYLEX)
        ///  </summary>
        ///  <param name="key"></param>
        /// <param name="maxScore"></param>
        /// <param name="offset"></param>
        ///  <param name="count"></param>
        ///  <param name="exclusive"></param>
        /// <param name="minScore"></param>
        /// <remarks>
        /// https://redis.io/commands/zrangebylex
        ///  </remarks>
        ///  <returns></returns>
        KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByScoreWithScoresAsc(string key, double minScore, double maxScore, long offset, long count, Exclusive exclusive = Exclusive.None);


        ///  <summary>
        ///  返回指定范围的元素,返回元素按分值从大到小排序(ZRANGEBYLEX)
        ///  </summary>
        ///  <param name="key"></param>
        /// <param name="minScore"></param>
        /// <param name="offset"></param>
        ///  <param name="count"></param>
        ///  <param name="exclusive"></param>
        /// <param name="maxScore"></param>
        /// <remarks>
        /// https://redis.io/commands/zrevrangebylex
        ///  </remarks>
        ///  <returns></returns>
        KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByScoreWithScoresDesc(string key, double maxScore, double minScore, long offset, long count, Exclusive exclusive = Exclusive.None);


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从小到大排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <remarks>
        ///https://redis.io/commands/zrange
        /// </remarks>
        /// <returns></returns>
        KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByRankWithScoresAsc(string key, long start, long stop);


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从大到小排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <remarks>
        ///https://redis.io/commands/zrange
        /// </remarks>
        /// <returns></returns>
        KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByRankWithScoresDesc(string key, long start, long stop);



        /// <summary>
        /// 返回有序集key中成员member的排名。其中有序集成员按score值递增(从小到大)顺序排列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/zrank
        /// </remarks>
        /// <returns></returns>
        long? SortedSetRankAsc<T>(string key, T value);


        /// <summary>
        /// 返回有序集key中成员member的排名，其中有序集成员按score值从大到小排列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/zrevrank
        /// </remarks>
        /// <returns></returns>
        long? SortedSetRankDesc<T>(string key, T value);


        /// <summary>
        /// 删除指定成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>
        ///https://redis.io/commands/zrem
        /// </remarks>
        /// <returns></returns>
        bool SortedSetRemove<T>(string key, T value);


        /// <summary>
        /// 删除名称按字典由低到高排序成员之间所有成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minMember"></param>
        /// <param name="maxMember"></param>
        /// <param name="exclusive"></param>
        /// <remarks>
        ///https://redis.io/commands/zremrangebylex
        /// </remarks>
        /// <returns></returns>
        long SortedSetRemoveRangeByValue<TMinMember, TMaxMember>(string key, TMinMember minMember, TMaxMember maxMember, Exclusive exclusive = Exclusive.None);


        /// <summary>
        /// 移除有序集key中，指定排名(rank)区间内的所有成员。下标参数start和stop都以0为底，0处是分数最小的那个元素。这些索引也可是负数，表示位移从最高分处开始数。例如，-1是分数最高的元素，-2是分数第二高的，依次类推。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <remarks>
        ///https://redis.io/commands/zremrangebyrank
        /// </remarks>
        /// <returns></returns>
        long SortedSetRemoveRangeByRank(string key, long start, long stop);


        /// <summary>
        /// 移除有序集key中，所有score值介于min和max之间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <param name="exclusive"></param>
        /// <remarks>
        ///https://redis.io/commands/zremrangebyscore
        /// </remarks>
        /// <returns></returns>
        long SortedSetRemoveRangeByScore(string key, double minScore, double maxScore,  Exclusive exclusive = Exclusive.None);

        /// <summary>
        /// 返回有序集key中，成员member的score值,如果member元素不是有序集key的成员，或key不存在，返回nil
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <remarks>
        ///https://redis.io/commands/zscore
        /// </remarks>
        /// <returns></returns>
        double? SortedSetScore<T>(string key, T member);

    }
}
