using System;
using System.Collections.Generic;
using System.Linq;
using CSRedis;
using CtSharpRedis.Enums;
using CtSharpRedis.Utils;

namespace CtSharpRedis.CsRedis
{
    internal class RedisDataBase: IRedisDataBase
    {
        private readonly RedisClient database;
        //private readonly IRedisValueSerializeSettings serializeSettings;

        public RedisDataBase(RedisClient database, IRedisValueSerializeSettings serializeSettings)
        {
            this.database = database;
            //this.serializeSettings = serializeSettings;
        }

        #region Keys
        public TimeSpan? KeyIdleTime(string key)
        {
            long? result= database.Object(RedisObjectSubCommand.IdleTime,key);
            if (result.HasValue)
            {
                return TimeSpan.FromSeconds(result.Value);
            }

            return null;
        }

        public TimeSpan? KeyTimeToLive(string key)
        {
            long seconds = database.Ttl(key);
            if (seconds < 0)
            {
                return null;
            }

            return TimeSpan.FromSeconds(seconds);
        }

        public bool KeyDelete(string key)
        {
            return database.Del(key) > 0;
        }

        public long KeyDelete(string[] keys)
        {
            return database.Del(keys);
        }

        public byte[] KeyDump(string key)
        {
            return database.Dump(key);
        }

        public bool KeyExists(string key)
        {
            return database.Exists(key);
        }

        public bool KeyExpire(string key, TimeSpan expire)
        {
            return database.Expire(key, expire);
        }

        public bool KeyExpire(string key, DateTime expire)
        {
            return database.ExpireAt(key, expire);
        }

        public bool KeyRename(string key, string newKey, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.RenameNx(key, newKey);
            }

            string result = database.Rename(key, newKey);
            if (result.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public bool KeyPersist(string key)
        {
            return database.Persist(key);
        }

        public bool KeyMove(string key, int destinationDbIndex)
        {
            return database.Move(key, destinationDbIndex);
        }


        #endregion

        #region Hash

        public bool HashDelete(string key, CtSharpRedisValue field)
        {
            return database.HDel(key, field.Value) > 0;
        }

        public long HashDelete(string key, CtSharpRedisValue[] fields)
        {
            if (fields == null)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            if (fields.IsNullOrEmptyArrary())
            {
                return 0;
            }

            string[] hashFields = CtSharpRedisValue.ConvertToStringArrary(fields);
            return database.HDel(key, hashFields);
        }

        public bool HashExists(string key, CtSharpRedisValue field)
        {
            return database.HExists(key, field.Value);
        }

        public T HashGet<T>(string key, CtSharpRedisValue field)
        {
            string redisValue = database.HGet(key, field.Value);
            return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
        }

        public CtSharpRedisValue[] HashGet(string key, CtSharpRedisValue[] fields)
        {
            if (fields == null)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            if (fields.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }
            string[] hashFields = CtSharpRedisValue.ConvertToStringArrary(fields);
            var redisValues= database.HMGet(key, hashFields);

            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] HashGetAll(string key)
        {
            var dic = database.HGetAll(key);

            if (dic.IsNullOrEmptyDictionary())
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>>();
            }

            var arrary = dic.ToArray();
            var result = new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[arrary.Length];
            for (int i = 0; i < arrary.Length; i++)
            {
                result[i]=new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>(arrary[i].Key, arrary[i].Value);
            }

            return result;
        }

        public long HashIncrement(string key, CtSharpRedisValue field, long value)
        {
            return database.HIncrBy(key, field.Value, value);
        }

        public double HashIncrement(string key, CtSharpRedisValue field, double value)
        {
            return database.HIncrByFloat(key, field.Value, value);
        }

        public long HashDecrement(string key, CtSharpRedisValue field, long value)
        {
            return database.HIncrBy(key, field.Value, -value);
        }

        public double HashDecrement(string key, CtSharpRedisValue field, double value)
        {
            return database.HIncrByFloat(key, field.Value, -value);
        }

        public CtSharpRedisValue[] HashKeys(string key)
        {
            var redisValues= database.HKeys(key);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public bool HashSet<TValue>(string key, CtSharpRedisValue field, TValue value, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when == CtSharpWhen.NotExists)
            {
                return database.HSetNx(key, field.Value, CtSharpRedisValue.SerializeRedisValue(value).Value);
            }
            return database.HSet(key, field.Value, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public void HashSet(string key, KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] fieldValues)
        {
            if (fieldValues.IsNullOrEmptyArrary())
            {
                return;
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < fieldValues.Length; i++)
            {
                dictionary.Add(fieldValues[i].Key.Value, fieldValues[i].Value.Value);
            }


            database.HMSet(key, dictionary);
        }

        /// <summary>
        /// 获取hash里所有字段的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HashLength(string key)
        {
            return database.HLen(key);
        }


        #endregion

        #region HyperLogLog

        public bool HyperLogLogAdd<T>(string key, T value)
        {
            return database.PfAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public bool HyperLogLogAdd(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.PfAdd(key, Array.Empty<object>());
            }
            object[] redisValue = new object[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValue[i] = values[i].Value;
            }
            return database.PfAdd(key, redisValue);
        }

        /// <summary>
        /// HyperLogLog长度计算
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HyperLogLogLength(string key)
        {
            return database.PfCount(key);
        }

        /// <summary>
        /// HyperLogLog长度计算
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public long HyperLogLogLength(string[] keys)
        {
            return database.PfCount(keys);
        }

        /// <summary>
        /// HyperLogLog合并
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        public void HyperLogLogMerge(string destinationKey, string firstKey, string secondKey)
        {
            database.PfMerge(destinationKey, firstKey, secondKey);
        }

        /// <summary>
        /// HyperLogLog合并
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="mergeKeys"></param>
        public void HyperLogLogMerge(string destinationKey, string[] mergeKeys)
        {
            database.PfMerge(destinationKey, mergeKeys);
        }




        #endregion

        #region List

        /// <summary>
        /// 获取一个元素，通过其索引列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public T ListGetByIndex<T>(string key, long index)
        {
            string redisValue = database.LIndex(key, index);
            return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
        }

        public long ListInsertAfter<TPivot, TValue>(string key, TPivot pivot, TValue value)
        {
            return database.LInsert(key, RedisInsert.After, CtSharpRedisValue.SerializeRedisValue(pivot).Value, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long ListInsertBefore<TPivot, TValue>(string key, TPivot pivot, TPivot value)
        {
            return database.LInsert(key, RedisInsert.Before, CtSharpRedisValue.SerializeRedisValue(pivot).Value, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }


        /// <summary>
        /// 获得队列(List)的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return database.LLen(key);
        }

        /// <summary>
        /// 从队列的左边出队一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            string redisValue = database.LPop(key);
            return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
        }

        public long ListLeftPush<T>(string key, T value, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when == CtSharpWhen.Exists)
            {
                return database.LPushX(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
            }
            return database.LPush(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }


        public long ListLeftPush(string key, CtSharpRedisValue[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.IsNullOrEmptyArrary())
            {
                return 0;
            }
            object[] redisValues = new object[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }
            return database.LPush(key, redisValues);
        }

        public CtSharpRedisValue[] ListRange(string key, long startIndex, long stopIndex)
        {
            var redisValues = database.LRange(key, startIndex, stopIndex);

            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i];
            }

            return result;
        }

        public long ListRemove<T>(string key, T value, long count)
        {
            return database.LRem(key, count, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public void ListSetByIndex<T>(string key, long index, T value)
        {
            database.LSet(key, index, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public void ListTrim(string key, long startIndex, long stopIndex)
        {
            database.LTrim(key, startIndex, stopIndex);
        }

        public T ListRightPop<T>(string key)
        {
            return CtSharpRedisValue.DeserializeRedisValue<T>(database.RPop(key));
        }

        public T ListRightPopLeftPush<T>(string key, string destinationKey)
        {
            return CtSharpRedisValue.DeserializeRedisValue<T>(database.RPopLPush(key, destinationKey));
        }

        public long ListRightPush<T>(string key, T value, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.Exists))
            {
                return database.RPushX(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
            }
            return database.RPush(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long ListRightPush(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.RPush(key, Array.Empty<object>());
            }
            object[] redisValues = new object[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }
            return database.RPush(key, redisValues);
        }

        #endregion

        #region Sets

        public bool SetAdd<T>(string key, T value)
        {
            return database.SAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value) > 0;
        }

        public long SetAdd(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.SAdd(key, Array.Empty<object>());
            }
            object[] redisValues = new object[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }

            return database.SAdd(key, redisValues);
        }

        /// <summary>
        /// 获取集合里面的元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SetLength(string key)
        {
            return database.SCard(key);
        }

        public CtSharpRedisValue[] SetCombineDiff(string firstKey, string secondKey)
        {
            string[] redisValues = database.SDiff(firstKey, secondKey);

            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SetCombineDiff(string[] keys)
        {
            string[] redisValues = database.SDiff(keys);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SetCombineInter(string firstKey, string secondKey)
        {
            string[] redisValues = database.SInter(firstKey, secondKey);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SetCombineInter(string[] keys)
        {
            string[] redisValues = database.SInter(keys);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SetCombineUnion(string firstKey, string secondKey)
        {
            string[] redisValues = database.SUnion(firstKey, secondKey);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SetCombineUnion(string[] keys)
        {
            string[] redisValues = database.SUnion(keys);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public long SetCombineAndStoreDiff(string destinationKey, string firstKey, string secondKey)
        {
            return database.SDiffStore(destinationKey, firstKey, secondKey);

        }

        public long SetCombineAndStoreDiff(string destinationKey, string[] keys)
        {
            return database.SDiffStore(destinationKey, keys);
        }


        public long SetCombineAndStoreInter(string destinationKey, string firstKey, string secondKey)
        {
            return database.SInterStore(destinationKey, firstKey, secondKey);
        }


        public long SetCombineAndStoreInter(string destinationKey, string[] keys)
        {
            return database.SInterStore(destinationKey, keys);
        }

        public long SetCombineAndStoreUnion(string destinationKey, string firstKey, string secondKey)
        {
            return database.SUnionStore(destinationKey, firstKey, secondKey);
        }


        public long SetCombineAndStoreUnion(string destinationKey, string[] keys)
        {
            return database.SUnionStore(destinationKey, keys);
        }

        public CtSharpRedisValue[] SetMembers(string key)
        {
            var redisValue= database.SMembers(key);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValue);
        }

        public bool SetContains<T>(string key, T value)
        {
            return database.SIsMember(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public bool SetMove<T>(string sourceKey, string destinationKey, T value)
        {
            return database.SMove(sourceKey, destinationKey, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }


        /// <summary>
        /// 读取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SetPop<T>(string key)
        {
            var redisValue = database.SPop(key);

            if (!string.IsNullOrWhiteSpace(redisValue))
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
            }

            return default(T);
        }

        public CtSharpRedisValue[] SetPop(string key, long count)
        {
            throw new NotImplementedException("ctcsredis 不支持此命令");
        }


        /// <summary>
        /// 随机获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SetRandomMember<T>(string key)
        {

            var redisValue = database.SRandMember(key);
            if (!string.IsNullOrWhiteSpace(redisValue))
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
            }

            return default(T);
        }

        public CtSharpRedisValue[] SetRandomMembers(string key, long count)
        {
            var redisValues = database.SRandMember(key, count);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public bool SetRemove<T>(string key, T value)
        {
            return database.SRem(key, CtSharpRedisValue.SerializeRedisValue(value).Value) > 0;
        }

        public long SetRemove(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.SRem(key, Array.Empty<object>());
            }
            object[] redisValues = new object[values.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }
            return database.SRem(key, redisValues);
        }

        #endregion

        #region SortedSet

        public bool SortedSetAdd<T>(string key, T value, double score, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.Exists))
            {
                throw new NotImplementedException("ctcsredis 不支持此命令");
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                throw new NotImplementedException("ctcsredis 不支持此命令");
            }
            Tuple<double, string> tupleValue = Tuple.Create(score, CtSharpRedisValue.SerializeRedisValue(value).Value);
            return database.ZAdd(key, tupleValue) > 0;
        }


        public long SortedSetAdd(string key, KeyValuePair<CtSharpRedisValue, double>[] scoreValues, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.Exists))
            {
                throw new NotImplementedException("ctcsredis 不支持此命令");
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                throw new NotImplementedException("ctcsredis 不支持此命令");
            }
            Tuple<double, object>[] sortedSetEntries = new Tuple<double, object>[scoreValues.Length];
            if (scoreValues.IsNullOrEmptyArrary())
            {
                return database.ZAdd(key, sortedSetEntries);
            }
            
            for (int i = 0; i < scoreValues.Length; i++)
            {
                sortedSetEntries[i] = Tuple.Create(scoreValues[i].Value, (object)scoreValues[i].Key.Value);
            }
            return database.ZAdd(key, sortedSetEntries);
        }

        /// <summary>
        /// 返回key的有序集元素个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            return database.ZCard(key);
        }

        public long SortedSetLength(string key, double minScore, double maxScore, Exclusive exclusive = Exclusive.None)
        {
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.ZCount(key, minScore, maxScore, true, true);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.ZCount(key, minScore, maxScore, true, false);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.ZCount(key, minScore, maxScore, false, true);
            }
            return database.ZCount(key, minScore, maxScore, false, false);
        }

        public double SortedSetIncrement<T>(string key, T value, double incScore)
        {
            return database.ZIncrBy(key, incScore, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public double SortedSetDecrement<T>(string key, T value, double incScore)
        {
            return database.ZIncrBy(key, -incScore, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        /// <summary>
        /// 计算给定的numkeys个有序集合的交集，并且把结果放到destination中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public long SortedSetCombineAndStoreInter(string destinationKey, string firstKey, string secondKey,CtSharpRedisAggregate aggregate= CtSharpRedisAggregate.Sum)
        {
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.ZInterStore(destinationKey, null, RedisAggregate.Sum, firstKey, secondKey);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.ZInterStore(destinationKey, null, RedisAggregate.Min, firstKey, secondKey);
            }
            return database.ZInterStore(destinationKey, null, RedisAggregate.Max, firstKey, secondKey);
        }

        /// <summary>
        /// 计算给定的numkeys个有序集合的交集，并且把结果放到destination中
        /// 使用WEIGHTS选项，你可以为每个给定的有序集指定一个乘法因子，意思就是，每个给定有序集的所有成员的score值在传递给聚合函数之前都要先乘以该因子。如果WEIGHTS没有给定，默认就是1
        ///默认使用的参数SUM，可以将所有集合中某个成员的score值之和作为结果集中该成员的score值。如果使用参数MIN或者MAX，结果集就是所有集合中元素最小或最大的元素。
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public long SortedSetCombineAndStoreInter(string destinationKey, string[] keys, double[] weights, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum)
        {
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.ZInterStore(destinationKey, weights, RedisAggregate.Sum,keys);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.ZInterStore(destinationKey, weights, RedisAggregate.Min, keys);
            }
            return database.ZInterStore(destinationKey, weights, RedisAggregate.Max, keys);
        }

        /// <summary>
        /// 计算给定的numkeys个有序集合的并集，并且把结果放到destination中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public long SortedSetCombineAndStoreUnion(string destinationKey, string firstKey, string secondKey, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum)
        {
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.ZUnionStore(destinationKey, null, RedisAggregate.Sum, firstKey, secondKey);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.ZUnionStore(destinationKey, null, RedisAggregate.Min, firstKey, secondKey);
            }
            return database.ZUnionStore(destinationKey, null, RedisAggregate.Max, firstKey, secondKey);
        }

        /// <summary>
        /// 计算给定的numkeys个有序集合的并集，并且把结果放到destination中
        /// 使用WEIGHTS选项，你可以为每个给定的有序集指定一个乘法因子，意思就是，每个给定有序集的所有成员的score值在传递给聚合函数之前都要先乘以该因子。如果WEIGHTS没有给定，默认就是1
        ///默认使用的参数SUM，可以将所有集合中某个成员的score值之和作为结果集中该成员的score值。如果使用参数MIN或者MAX，结果集就是所有集合中元素最小或最大的元素。
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <param name="weights"></param>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        public long SortedSetCombineAndStoreUnion(string destinationKey, string[] keys, double[] weights, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum)
        {
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.ZUnionStore(destinationKey, weights, RedisAggregate.Sum, keys);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.ZUnionStore(destinationKey, weights, RedisAggregate.Min, keys);
            }
            return database.ZUnionStore(destinationKey, weights, RedisAggregate.Max, keys);
        }

        public long SortedSetLengthByValue<TMinMember, TMaxMember>(string key, TMinMember minValue, TMaxMember maxValue, Exclusive exclusive = Exclusive.None)
        {
            string min = CtSharpRedisValue.SerializeRedisValue(minValue).Value;
            string max = CtSharpRedisValue.SerializeRedisValue(maxValue).Value;
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.ZLexCount(key,$"({min}" , $"({max}");
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.ZLexCount(key, $"({min}", $"[{max}");
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.ZLexCount(key, $"[{min}", $"({max}");
            }

            return database.ZLexCount(key, $"[{min}", $"[{max}");
        }

        public CtSharpRedisValue[] SortedSetRangeByRankAsc(string key, long start, long stop)
        {
            var redisValues= database.ZRange(key, start, stop);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByRankDesc(string key, long start, long stop)
        {
            var redisValues= database.ZRevRange(key, start, stop);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByValueAsc<TMinMember, TMaxMember>(string key, TMinMember minMember, TMaxMember maxMember, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            var min = CtSharpRedisValue.SerializeRedisValue(minMember).Value.Trim();
            var max = CtSharpRedisValue.SerializeRedisValue(maxMember).Value.Trim();
            string[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues= database.ZRangeByLex(key, $"({min}", $"({max}", offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues= database.ZRangeByLex(key, $"({min}", $"[{max}", offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.ZRangeByLex(key, $"[{min}", $"({max}", offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            redisValues = database.ZRangeByLex(key, $"[{min}", $"[{max}", offset, count);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByValueDesc<TMinMember, TMaxMember>(string key, TMinMember maxMember, TMaxMember minMember, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            throw new NotImplementedException("ctcsredis 不支持此命令");
        }

        public CtSharpRedisValue[] SortedSetRangeByScoreAsc(string key, double minScore, double maxScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            string[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.ZRangeByScore(key, minScore, maxScore, false, true, true, offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.ZRangeByScore(key, minScore, maxScore, false, true, false, offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.ZRangeByScore(key, minScore, maxScore, false, false, true, offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            redisValues = database.ZRangeByScore(key, minScore, maxScore, false, false, false, offset, count);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByScoreDesc(string key, double maxScore, double minScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            string[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.ZRevRangeByScore(key, maxScore, minScore, false, true, true, offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.ZRevRangeByScore(key, maxScore, minScore, false, true, false, offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.ZRevRangeByScore(key, maxScore, minScore, false, false, true, offset, count);
                return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
            }
            redisValues = database.ZRevRangeByScore(key, maxScore, minScore, false, false, false, offset, count);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByScoreWithScoresAsc(string key, double minScore, double maxScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            string[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.ZRangeByScore(key, minScore, maxScore, true, true, true, offset, count);
                return ToCtSharpSortedSetRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.ZRangeByScore(key, minScore, maxScore, true, true, false, offset, count);
                return ToCtSharpSortedSetRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.ZRangeByScore(key, minScore, maxScore, true, false, true, offset, count);
                return ToCtSharpSortedSetRedisValue(redisValues);
            }
            redisValues = database.ZRangeByScore(key, minScore, maxScore, true, false, false, offset, count);
            return ToCtSharpSortedSetRedisValue(redisValues);
        }

        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByScoreWithScoresDesc(string key, double maxScore, double minScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            string[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.ZRevRangeByScore(key, maxScore, minScore, true, true, true, offset, count);
                return ToCtSharpSortedSetRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.ZRevRangeByScore(key, maxScore, minScore, true, true, false, offset, count);
                return ToCtSharpSortedSetRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.ZRevRangeByScore(key, maxScore, minScore, true, false, true, offset, count);
                return ToCtSharpSortedSetRedisValue(redisValues);
            }
            redisValues = database.ZRevRangeByScore(key, maxScore, minScore, true, false, false, offset, count);
            return ToCtSharpSortedSetRedisValue(redisValues);
        }

        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByRankWithScoresAsc(string key, long start, long stop)
        {
            var redisValues = database.ZRange(key, start, stop, true);
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, double>>();
            }
            return ToCtSharpSortedSetRedisValue(redisValues);
        }

        private KeyValuePair<CtSharpRedisValue, double>[] ToCtSharpSortedSetRedisValue(string[] redisValues)
        {
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, double>>();
            }
            int valueScoreSplitNum = 2;
            KeyValuePair<CtSharpRedisValue, double>[] result = new KeyValuePair<CtSharpRedisValue, double>[redisValues.Length / valueScoreSplitNum];
            long valueIndex = 0;
            long scoreIndex = 1;
            for (long i = 0; i < result.Length; i++)
            {
                result[i] = new KeyValuePair<CtSharpRedisValue, double>(redisValues[valueIndex], Convert.ToDouble(redisValues[scoreIndex]));
                valueIndex = valueIndex + valueScoreSplitNum;
                scoreIndex = scoreIndex + valueScoreSplitNum;
            }

            return result;
        }

        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByRankWithScoresDesc(string key, long start, long stop)
        {
            var redisValues = database.ZRevRange(key, start, stop, true);
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, double>>();
            }

            return ToCtSharpSortedSetRedisValue(redisValues);
        }

        public long? SortedSetRankAsc<T>(string key, T value)
        {
            return database.ZRank(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long? SortedSetRankDesc<T>(string key, T value)
        {
            return database.ZRevRank(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public bool SortedSetRemove<T>(string key, T value)
        {
            object redisValue = CtSharpRedisValue.SerializeRedisValue(value).Value;
            return database.ZRem(key, redisValue) > 0;
        }

        public long SortedSetRemoveRangeByValue<TMinMember, TMaxMember>(string key, TMinMember minMember, TMaxMember maxMember, Exclusive exclusive = Exclusive.None)
        {
            var min = CtSharpRedisValue.SerializeRedisValue(minMember).Value;
            var max = CtSharpRedisValue.SerializeRedisValue(maxMember).Value;
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.ZRemRangeByLex(key, $"({min}", $"({max}");
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.ZRemRangeByLex(key, $"({min}", $"[{max}");
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.ZRemRangeByLex(key, $"[{min}", $"({max}");
            }
            return database.ZRemRangeByLex(key, $"[{min}", $"[{max}");
        }

        /// <summary>
        /// 移除有序集key中，指定排名(rank)区间内的所有成员。下标参数start和stop都以0为底，0处是分数最小的那个元素。这些索引也可是负数，表示位移从最高分处开始数。例如，-1是分数最高的元素，-2是分数第二高的，依次类推。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public long SortedSetRemoveRangeByRank(string key, long start, long stop)
        {
            return database.ZRemRangeByRank(key, start, stop);
        }

        public long SortedSetRemoveRangeByScore(string key, double minScore, double maxScore, Exclusive exclusive = Exclusive.None)
        {
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.ZRemRangeByScore(key, minScore, maxScore, true, true);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.ZRemRangeByScore(key, minScore, maxScore, true, false);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.ZRemRangeByScore(key, minScore, maxScore, false, true);
            }
            return database.ZRemRangeByScore(key, minScore, maxScore, false, false);
        }

        public double? SortedSetScore<T>(string key, T member)
        {
            return database.ZScore(key, CtSharpRedisValue.SerializeRedisValue(member).Value);
        }


        #endregion

        #region Strings

        public long StringAppend<T>(string key, T value)
        {
            return database.Append(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long StringBitCount(string key, long startIndex, long stopIndex)
        {
            return database.BitCount(key, startIndex,stopIndex);
        }

        public long StringBitOperation(string destinationKey, string firstKey, string secondKey, RedisBitwise bitwise = RedisBitwise.And)
        {
            if (bitwise.Equals(RedisBitwise.Not))
            {
                return database.BitOp(RedisBitOp.Not, destinationKey, firstKey, secondKey);
            }
            if (bitwise.Equals(RedisBitwise.Or))
            {
                return database.BitOp(RedisBitOp.Or, destinationKey, firstKey, secondKey);
            }
            if (bitwise.Equals(RedisBitwise.Xor))
            {
                return database.BitOp(RedisBitOp.XOr, destinationKey, firstKey, secondKey);
            }

            return database.BitOp(RedisBitOp.And, destinationKey, firstKey, secondKey);
        }

        public long StringBitOperation(string destinationKey, string[] keys, RedisBitwise bitwise = RedisBitwise.And)
        {
            if (bitwise.Equals(RedisBitwise.Not))
            {
                return database.BitOp(RedisBitOp.Not, destinationKey, keys);
            }
            if (bitwise.Equals(RedisBitwise.Or))
            {
                return database.BitOp(RedisBitOp.Or, destinationKey, keys);
            }
            if (bitwise.Equals(RedisBitwise.Xor))
            {
                return database.BitOp(RedisBitOp.XOr, destinationKey, keys);
            }

            return database.BitOp(RedisBitOp.And, destinationKey, keys);
        }

        public long StringBitPosition(string key, bool bit, long start = 0, long end = -1)
        {
            return database.BitPos(key, bit, start, end);
        }

        public long StringDecrement(string key, long value = 1)
        {
            return database.DecrBy(key, value);
        }

        public double StringDecrement(string key, double value)
        {
            return database.IncrByFloat(key, -value);
        }

        public T StringGet<T>(string key)
        {
            string redisValue = database.Get(key);

            return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
        }

        public CtSharpRedisValue[] StringGet(string[] keys)
        {
            string[] redisValues = database.MGet(keys);
            return CtSharpRedisValue.ConvertToRedisValueArrary(redisValues);
        }

        public bool StringGetBit(string key, long offset)
        {
            return database.GetBit(key, (uint) offset);
        }

        public T StringGetRange<T>(string key, long start, long end)
        {
            string redisValue = database.GetRange(key, start, end);
            return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
        }

        public T StringGetSet<T>(string key, T value)
        {
            string redisValue = database.GetSet(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
            return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
        }

        public long StringIncrement(string key, long value = 1)
        {
            return database.IncrBy(key, value);
        }

        public double StringIncrement(string key, double value)
        {
            return database.IncrByFloat(key, value);
        }

        public long StringLength(string key)
        {
            return database.StrLen(key);
        }

        public bool StringSet<T>(string key, T value, TimeSpan? expiry = null, CtSharpWhen when = CtSharpWhen.Always)
        {
            object redisValue = CtSharpRedisValue.SerializeRedisValue(value).Value;
            string result = string.Empty;
            if (expiry.HasValue)
            {
                if (when.Equals(CtSharpWhen.NotExists))
                {
                    bool isSet = database.SetNx(key, redisValue);
                    if (isSet)
                    {
                        database.Expire(key, expiry.Value);
                    }
                    return isSet;

                }
                result = database.SetEx(key, (long)expiry.Value.TotalSeconds, redisValue);
                if (result.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.SetNx(key, redisValue);
            }

            result = database.Set(key, redisValue);
            if (result.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;

        }

        public bool StringSet(KeyValuePair<string, CtSharpRedisValue>[] keyValues, CtSharpWhen when = CtSharpWhen.Always)
        {
            Tuple<string,string>[] redisValues = null;
            if (keyValues.IsNullOrEmptyArrary())
            {
                redisValues = Array.Empty<Tuple<string, string>>();
            }
            else
            {
                redisValues = new Tuple<string, string>[keyValues.Length];
                for (int i = 0; i < keyValues.Length; i++)
                {
                    redisValues[i] = Tuple.Create(keyValues[i].Key, keyValues[i].Value.Value);
                }
            }

            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.MSetNx(redisValues);
            }
            string result = string.Empty;
            if (when.Equals(CtSharpWhen.Exists))
            {
                result = database.MSet(redisValues);
            }
            else
            {
                result = database.MSet(redisValues);
            }

            if (result.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public bool StringSetBit(string key, long offset, bool bit)
        {
            return database.SetBit(key, (uint)offset, bit);
        }

        public long StringSetRange<T>(string key, long offset, T value)
        {
            return database.SetRange(key, (uint) offset, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        #endregion
    }
}
