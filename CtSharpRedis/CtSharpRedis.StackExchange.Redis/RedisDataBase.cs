using System;
using System.Collections.Generic;
using CtSharpRedis.Enums;
using CtSharpRedis.Utils;
using StackExchange.Redis;

namespace CtSharpRedis.StackExchange.Redis
{
    internal class RedisDataBase : IRedisDataBase
    {
        private readonly IDatabase database;
        private readonly IRedisValueSerializeSettings serializeSettings;

        public RedisDataBase(IDatabase database, IRedisValueSerializeSettings serializeSettings)
        {
            this.database = database;
            this.serializeSettings = serializeSettings;
        }

        #region Keys

        public TimeSpan? KeyIdleTime(string key)
        {
            return database.KeyIdleTime(key);
        }

        public TimeSpan? KeyTimeToLive(string key)
        {
            return database.KeyTimeToLive(key);
        }

        public bool KeyDelete(string key)
        {
            return database.KeyDelete(key);
        }

        public long KeyDelete(string[] keys)
        {
            if (keys.IsNullOrEmptyArrary())
            {
                return 0;
            }

            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            return database.KeyDelete(redisKeys);
        }

        public byte[] KeyDump(string key)
        {
            return database.KeyDump(key);
        }

        public bool KeyExists(string key)
        {
            return database.KeyExists(key);
        }

        public bool KeyExpire(string key,TimeSpan expire)
        {
            return database.KeyExpire(key, expire);
        }

        public bool KeyExpire(string key, DateTime expire)
        {
            return database.KeyExpire(key, expire);
        }

        public bool KeyRename(string key, string newKey, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.KeyRename(key, newKey,When.NotExists);
            }

            return database.KeyRename(key, newKey, When.Always);
        }

        public bool KeyPersist(string key)
        {
            return database.KeyPersist(key);
        }

        public bool KeyMove(string key, int destinationDbIndex)
        {
            return database.KeyMove(key, destinationDbIndex);
        }

        #endregion

        #region Hash

        public bool HashDelete(string key, CtSharpRedisValue field)
        {
            return database.HashDelete(key, field.Value);
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

            List<RedisValue> hashFields = new List<RedisValue>();
            foreach (var field in fields)
            {
                hashFields.Add(field.Value);
            }

            return database.HashDelete(key, hashFields.ToArray());
        }

        public bool HashExists(string key, CtSharpRedisValue field)
        {
            return database.HashExists(key, field.Value);
        }

        public T HashGet<T>(string key, CtSharpRedisValue field)
        {
            RedisValue redisValue = database.HashGet(key, field.Value);
            if (redisValue.HasValue)
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
            }
            return default(T);
        }

        public CtSharpRedisValue[] HashGet(string key, CtSharpRedisValue[] fields)
        {
            if (fields==null)
            {
                throw new ArgumentNullException(nameof(fields));
            }
            if (fields.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            List<RedisValue> hfields = new List<RedisValue>();
            foreach (var field in fields)
            {
                hfields.Add(field.Value);
            }

            RedisValue[] redisValues = database.HashGet(key, hfields.ToArray());
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] HashGetAll(string key)
        {
            HashEntry[] entries = database.HashGetAll(key);
            if (entries.IsNullOrEmptyArrary())
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>>();
            }
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] result=new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[entries.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                result[i]=new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>(entries[i].Name.ToString(), entries[i].Value.ToString());
            }

            return result;
        }

        public long HashIncrement(string key, CtSharpRedisValue field, long value)
        {
            return database.HashIncrement(key, field.Value, value);
        }

        public double HashIncrement(string key, CtSharpRedisValue field, double value)
        {
            return database.HashIncrement(key, field.Value, value);
        }

        public long HashDecrement(string key, CtSharpRedisValue field, long value)
        {
            return database.HashDecrement(key, field.Value, value);
        }

        public double HashDecrement(string key, CtSharpRedisValue field, double value)
        {
            return database.HashDecrement(key, field.Value, value);
        }

        public CtSharpRedisValue[] HashKeys(string key)
        {
            var hashKeys = database.HashKeys(key);
            if (hashKeys.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[hashKeys.Length];
            for (int i = 0; i < hashKeys.Length; i++)
            {
                result[i] = hashKeys[i].ToString();
            }

            return result;
        }

        public bool HashSet<TValue>(string key, CtSharpRedisValue field, TValue value, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when == CtSharpWhen.NotExists)
            {
                return database.HashSet(key, field.Value, CtSharpRedisValue.SerializeRedisValue(value).Value, When.NotExists);
            }
            return database.HashSet(key, field.Value, CtSharpRedisValue.SerializeRedisValue(value).Value,When.Always);
        }

        public void HashSet(string key, KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] fieldValues)
        {
            if (fieldValues.IsNullOrEmptyArrary())
            {
                throw new ArgumentNullException(nameof(fieldValues));
            }


            HashEntry[] hashEntries = new HashEntry[fieldValues.Length];
            for (int i = 0; i < hashEntries.Length; i++)
            {
                hashEntries[i]=new HashEntry(fieldValues[i].Key.Value, fieldValues[i].Value.Value);
            }

            database.HashSet(key, hashEntries);
        }

        public long HashLength(string key)
        {
            return database.HashLength(key);
        }

        #endregion

        #region HyperLogLog


        public bool HyperLogLogAdd<T>(string key, T value)
        {
            return database.HyperLogLogAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public bool HyperLogLogAdd(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.HyperLogLogAdd(key, Array.Empty<RedisValue>());
            }

            RedisValue[] redisValues = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }

            return database.HyperLogLogAdd(key, redisValues);
        }

        public long HyperLogLogLength(string key)
        {
            return database.HyperLogLogLength(key);
        }

        public long HyperLogLogLength(string[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (keys.IsNullOrEmptyArrary())
            {
                return 0;
            }

            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }

            return database.HyperLogLogLength(redisKeys);
        }

        public void HyperLogLogMerge(string destinationKey, string firstKey, string secondKey)
        {
            database.HyperLogLogMerge(destinationKey, firstKey, secondKey);
        }

        public void HyperLogLogMerge(string destinationKey, string[] mergeKeys)
        {
            if (mergeKeys == null)
            {
                throw new ArgumentNullException(nameof(mergeKeys));
            }

            if (mergeKeys.IsNullOrEmptyArrary())
            {
                return;
            }
            RedisKey[] redisKeys = new RedisKey[mergeKeys.Length];
            for (int i = 0; i < mergeKeys.Length; i++)
            {
                redisKeys[i] = mergeKeys[i];
            }

            database.HyperLogLogMerge(destinationKey, redisKeys);
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
            return CtSharpRedisValue.DeserializeRedisValue<T>(database.ListGetByIndex(key, index));
        }

        public long ListInsertAfter<TPivot, TValue>(string key, TPivot pivot, TValue value)
        {
            return database.ListInsertAfter(key, CtSharpRedisValue.SerializeRedisValue(pivot).Value, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long ListInsertBefore<TPivot, TValue>(string key, TPivot pivot, TPivot value)
        {
            return database.ListInsertBefore(key, CtSharpRedisValue.SerializeRedisValue(pivot).Value, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }


        /// <summary>
        /// 获得队列(List)的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return database.ListLength(key);
        }

        /// <summary>
        /// 从队列的左边出队一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            return CtSharpRedisValue.DeserializeRedisValue<T>(database.ListLeftPop(key));
        }

        public long ListLeftPush<T>(string key, T value, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.Exists))
            {
                return database.ListLeftPush(key, CtSharpRedisValue.SerializeRedisValue(value).Value, When.Exists);
            }
            return database.ListLeftPush(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
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
            RedisValue[] redisValues = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }

            return database.ListLeftPush(key, redisValues);
        }

        public CtSharpRedisValue[] ListRange(string key, long startIndex, long stopIndex)
        {
            RedisValue[] redisValues = database.ListRange(key, startIndex, stopIndex);

            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public long ListRemove<T>(string key, T value, long count)
        {
            return database.ListRemove(key, CtSharpRedisValue.SerializeRedisValue(value).Value, count);
        }

        public void ListSetByIndex<T>(string key, long index, T value)
        {
            database.ListSetByIndex(key, index, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }


        /// <summary>
        /// 修剪到指定范围内的清单
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        public void ListTrim(string key, long startIndex, long stopIndex)
        {
            database.ListTrim(key, startIndex, stopIndex);
        }

        public T ListRightPop<T>(string key)
        {
            return CtSharpRedisValue.DeserializeRedisValue<T>(database.ListRightPop(key));
        }

        /// <summary>
        /// 删除列表中的最后一个元素，将其追加到另一个列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="destinationKey"></param>
        /// <returns></returns>
        public T ListRightPopLeftPush<T>(string key, string destinationKey)
        {
            return CtSharpRedisValue.DeserializeRedisValue<T>(database.ListRightPopLeftPush(key, destinationKey));
        }

        public long ListRightPush<T>(string key, T value, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when == CtSharpWhen.Exists)
            {
                return database.ListRightPush(key, CtSharpRedisValue.SerializeRedisValue(value).Value,When.Exists);
            }
            return database.ListRightPush(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long ListRightPush(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.ListRightPush(key, Array.Empty<RedisValue>());
            }
            RedisValue[] redisValues = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }

            return database.ListRightPush(key, redisValues);
        }


        #endregion

        #region Sets

        public bool SetAdd<T>(string key, T value)
        {
            return database.SetAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long SetAdd(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return database.SetAdd(key, Array.Empty<RedisValue>());
            }
            RedisValue[] redisValues = new RedisValue[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }

            return database.SetAdd(key, redisValues);
        }

        /// <summary>
        /// 获取集合里面的元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SetLength(string key)
        {
            return database.SetLength(key);
        }

        public CtSharpRedisValue[] SetCombineDiff(string firstKey, string secondKey)
        {
            RedisValue[] redisValues = database.SetCombine(SetOperation.Difference, firstKey, secondKey);

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public CtSharpRedisValue[] SetCombineDiff(string[] keys)
        {
            if (keys.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < redisKeys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            RedisValue[] redisValues = database.SetCombine(SetOperation.Difference, redisKeys);

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public CtSharpRedisValue[] SetCombineInter(string firstKey, string secondKey)
        {
            RedisValue[] redisValues = database.SetCombine(SetOperation.Intersect, firstKey, secondKey);

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public CtSharpRedisValue[] SetCombineInter(string[] keys)
        {
            if (keys.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < redisKeys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            RedisValue[] redisValues = database.SetCombine(SetOperation.Intersect, redisKeys);

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public CtSharpRedisValue[] SetCombineUnion(string firstKey, string secondKey)
        {
            RedisValue[] redisValues = database.SetCombine(SetOperation.Union, firstKey, secondKey);

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public CtSharpRedisValue[] SetCombineUnion(string[] keys)
        {
            if (keys.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < redisKeys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            RedisValue[] redisValues = database.SetCombine(SetOperation.Union, redisKeys);

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        ///  <summary>
        ///  类似于 SDIFF, 不同之处在于该命令不返回结果集，而是将结果存放在destination集合中
        ///  </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        ///  <param name="secondKey"></param>
        ///  <returns>结果集元素的个数</returns>
        public long SetCombineAndStoreDiff(string destinationKey, string firstKey, string secondKey)
        {
            return database.SetCombineAndStore(SetOperation.Difference, destinationKey, firstKey, secondKey);

        }

        /// <summary>
        /// 类似于 SDIFF, 不同之处在于该命令不返回结果集，而是将结果存放在destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <returns>结果集元素的个数</returns>
        public long SetCombineAndStoreDiff(string destinationKey, string[] keys)
        {
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < redisKeys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }

            return database.SetCombineAndStore(SetOperation.Difference, destinationKey, redisKeys);
        }

        /// <summary>
        /// 与SINTER命令类似, 但是它并不是直接返回结果集,而是将结果保存在 destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <returns>结果集中成员的个数</returns>
        public long SetCombineAndStoreInter(string destinationKey, string firstKey, string secondKey)
        {
            return database.SetCombineAndStore(SetOperation.Intersect, destinationKey, firstKey, secondKey);
        }

        /// <summary>
        /// 与SINTER命令类似, 但是它并不是直接返回结果集,而是将结果保存在 destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <returns>结果集中成员的个数</returns>
        public long SetCombineAndStoreInter(string destinationKey, string[] keys)
        {
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < redisKeys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            return database.SetCombineAndStore(SetOperation.Intersect, destinationKey, redisKeys);
        }

        /// <summary>
        /// 类似于SUNION命令,不同的是它并不返回结果集,而是将结果存储在destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <returns>结果集中元素的个数</returns>
        public long SetCombineAndStoreUnion(string destinationKey, string firstKey, string secondKey)
        {
            return database.SetCombineAndStore(SetOperation.Union, destinationKey, firstKey, secondKey);
        }

        /// <summary>
        /// 类似于SUNION命令,不同的是它并不返回结果集,而是将结果存储在destination集合中
        /// </summary>
        /// <param name="destinationKey"></param>
        /// <param name="keys"></param>
        /// <returns>结果集中元素的个数</returns>
        public long SetCombineAndStoreUnion(string destinationKey, string[] keys)
        {
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < redisKeys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }

            return database.SetCombineAndStore(SetOperation.Union, destinationKey, redisKeys);
        }

        public CtSharpRedisValue[] SetMembers(string key)
        {
            RedisValue[] redisValues = database.SetMembers(key);
            if (redisValues == null || redisValues.Length <= 0)
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public bool SetContains<T>(string key, T value)
        {
            return database.SetContains(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public bool SetMove<T>(string sourceKey, string destinationKey, T value)
        {
            return database.SetMove(sourceKey, destinationKey, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }


        public T SetPop<T>(string key)
        {
            var redisValue = database.SetPop(key);

            if (redisValue.HasValue)
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
            }

            return default(T);
        }

        public CtSharpRedisValue[] SetPop(string key, long count)
        {
            RedisValue[] redisValues = database.SetPop(key, count);
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }


        public T SetRandomMember<T>(string key)
        {

            RedisValue redisValue = database.SetRandomMember(key);
            if (redisValue.HasValue)
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue);
            }

            return default(T);
        }

        public CtSharpRedisValue[] SetRandomMembers(string key, long count)
        {
            RedisValue[] redisValues = database.SetRandomMembers(key, count);
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }

            return result;
        }

        public bool SetRemove<T>(string key, T value)
        {
            return database.SetRemove(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long SetRemove(string key, CtSharpRedisValue[] values)
        {
            if (values.IsNullOrEmptyArrary())
            {
                return 0;
            }
            RedisValue[] redisValues = new RedisValue[values.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                redisValues[i] = values[i].Value;
            }
            return database.SetRemove(key, redisValues);
        }

        #endregion

        #region SortedSet


        public bool SortedSetAdd<T>(string key, T value, double score, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.Exists))
            {
                return database.SortedSetAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value, score, When.Exists);
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.SortedSetAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value, score, When.NotExists);
            }
            return database.SortedSetAdd(key, CtSharpRedisValue.SerializeRedisValue(value).Value, score);
        }

        public long SortedSetAdd(string key, KeyValuePair<CtSharpRedisValue, double>[] scoreValues, CtSharpWhen when = CtSharpWhen.Always)
        {
            SortedSetEntry[] sortedSetEntries = null;
            if (scoreValues.IsNullOrEmptyArrary())
            {
                sortedSetEntries = Array.Empty<SortedSetEntry>();
            }
            else
            {
                sortedSetEntries = new SortedSetEntry[scoreValues.Length];
                for (int i = 0; i < scoreValues.Length; i++)
                {
                    sortedSetEntries[i] = new SortedSetEntry(scoreValues[i].Key.Value, scoreValues[i].Value);
                }
            }

            if (when.Equals(CtSharpWhen.Exists))
            {
                return database.SortedSetAdd(key, sortedSetEntries, When.Exists);
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.SortedSetAdd(key, sortedSetEntries, When.NotExists);
            }
            return database.SortedSetAdd(key, sortedSetEntries);
        }

        /// <summary>
        /// 返回key的有序集元素个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            return database.SortedSetLength(key);
        }

        public long SortedSetLength(string key, double minScore, double maxScore, Exclusive exclusive = Exclusive.None)
        {
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.SortedSetLength(key, minScore, maxScore, Exclude.Both);
            }

            if (exclusive.Equals(Exclusive.Start))
            {
                return database.SortedSetLength(key, minScore, maxScore, Exclude.Start);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.SortedSetLength(key, minScore, maxScore, Exclude.Stop);
            }
            return database.SortedSetLength(key, minScore, maxScore);
        }

        public double SortedSetIncrement<T>(string key, T value, double incScore)
        {
            return database.SortedSetIncrement(key, CtSharpRedisValue.SerializeRedisValue(value).Value, incScore);
        }

        public double SortedSetDecrement<T>(string key, T value, double incScore)
        {
            return database.SortedSetDecrement(key, CtSharpRedisValue.SerializeRedisValue(value).Value, incScore);
        }


        public long SortedSetCombineAndStoreInter(string destinationKey, string firstKey, string secondKey, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum)
        {
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate)|| aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, firstKey, secondKey, Aggregate.Sum);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, firstKey, secondKey, Aggregate.Min);
            }
            return database.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, firstKey, secondKey, Aggregate.Max);
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
        public long SortedSetCombineAndStoreInter(string destinationKey, string[] keys, double[] weights,CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum)
        {
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, redisKeys, weights, Aggregate.Sum);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, redisKeys, weights, Aggregate.Min);
            }
            return database.SortedSetCombineAndStore(SetOperation.Intersect, destinationKey, redisKeys, weights, Aggregate.Max);
        }


        public long SortedSetCombineAndStoreUnion(string destinationKey, string firstKey, string secondKey, CtSharpRedisAggregate aggregate = CtSharpRedisAggregate.Sum)
        {
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, firstKey, secondKey, Aggregate.Sum);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, firstKey, secondKey, Aggregate.Min);
            }
            return database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, firstKey, secondKey, Aggregate.Max);
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
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            if (Enum.IsDefined(typeof(CtSharpRedisAggregate), aggregate) || aggregate.Equals(CtSharpRedisAggregate.Sum))
            {
                return database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, redisKeys, weights, Aggregate.Sum);
            }

            if (aggregate.Equals(CtSharpRedisAggregate.Min))
            {
                return database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, redisKeys, weights, Aggregate.Min);
            }
            return database.SortedSetCombineAndStore(SetOperation.Union, destinationKey, redisKeys, weights, Aggregate.Max);
        }

        public long SortedSetLengthByValue<TMinMember, TMaxMember>(string key, TMinMember minValue, TMaxMember maxValue, Exclusive exclusive = Exclusive.None)
        {
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.SortedSetLengthByValue(key, CtSharpRedisValue.SerializeRedisValue(minValue).Value, CtSharpRedisValue.SerializeRedisValue(maxValue).Value,Exclude.Both);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.SortedSetLengthByValue(key, CtSharpRedisValue.SerializeRedisValue(minValue).Value, CtSharpRedisValue.SerializeRedisValue(maxValue).Value, Exclude.Start);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.SortedSetLengthByValue(key, CtSharpRedisValue.SerializeRedisValue(minValue).Value, CtSharpRedisValue.SerializeRedisValue(maxValue).Value, Exclude.Stop);
            }
            return database.SortedSetLengthByValue(key, CtSharpRedisValue.SerializeRedisValue(minValue).Value, CtSharpRedisValue.SerializeRedisValue(maxValue).Value, Exclude.None);
        }

        public CtSharpRedisValue[] SortedSetRangeByRankAsc(string key, long start, long stop)
        {
            var redisValues = database.SortedSetRangeByRank(key, start, stop, Order.Ascending);
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            return ToCtSharpRedisValue(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByRankDesc(string key, long start, long stop)
        {
            var redisValues = database.SortedSetRangeByRank(key, start, stop, Order.Descending);
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }
            return ToCtSharpRedisValue(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByValueAsc<TMinMember, TMaxMember>(string key, TMinMember minMember, TMaxMember maxMember, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            var min = CtSharpRedisValue.SerializeRedisValue(minMember).Value;
            var max = CtSharpRedisValue.SerializeRedisValue(maxMember).Value;

            RedisValue[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.Both, Order.Ascending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.Start, Order.Ascending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.Stop, Order.Ascending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }

            redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.None, Order.Ascending, offset, count);
            return ToCtSharpRedisValue(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByValueDesc<TMinMember, TMaxMember>(string key, TMinMember maxMember, TMaxMember minMember, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            var min = CtSharpRedisValue.SerializeRedisValue(maxMember).Value;
            var max = CtSharpRedisValue.SerializeRedisValue(minMember).Value;
            RedisValue[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.Both, Order.Descending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.Start, Order.Descending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.Stop, Order.Descending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            redisValues = database.SortedSetRangeByValue(key, min, max, Exclude.None, Order.Descending, offset, count);
            return ToCtSharpRedisValue(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByScoreAsc(string key, double minScore, double maxScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            RedisValue[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.SortedSetRangeByScore(key, minScore, maxScore, Exclude.Both, Order.Ascending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.SortedSetRangeByScore(key, minScore, maxScore, Exclude.Start, Order.Ascending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.SortedSetRangeByScore(key, minScore, maxScore, Exclude.Stop, Order.Ascending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            redisValues = database.SortedSetRangeByScore(key, minScore, maxScore, Exclude.None, Order.Ascending, offset, count);
            return ToCtSharpRedisValue(redisValues);
        }

        public CtSharpRedisValue[] SortedSetRangeByScoreDesc(string key, double maxScore, double minScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            RedisValue[] redisValues = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                redisValues = database.SortedSetRangeByScore(key, maxScore, minScore, Exclude.Both, Order.Descending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                redisValues = database.SortedSetRangeByScore(key, maxScore, minScore, Exclude.Start, Order.Descending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                redisValues = database.SortedSetRangeByScore(key, maxScore, minScore, Exclude.Stop, Order.Descending, offset, count);
                return ToCtSharpRedisValue(redisValues);
            }
            redisValues = database.SortedSetRangeByScore(key, maxScore, minScore, Exclude.None, Order.Descending, offset, count);
            return ToCtSharpRedisValue(redisValues);
        }

        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByScoreWithScoresAsc(string key, double minScore, double maxScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            SortedSetEntry[] entries = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                entries = database.SortedSetRangeByScoreWithScores(key, minScore, maxScore, Exclude.Both, Order.Ascending, offset, count);
                return ToCtSharpSortedSetRedisValue(entries);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                entries = database.SortedSetRangeByScoreWithScores(key, minScore, maxScore, Exclude.Start, Order.Ascending, offset, count);
                return ToCtSharpSortedSetRedisValue(entries);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                entries = database.SortedSetRangeByScoreWithScores(key, minScore, maxScore, Exclude.Stop, Order.Ascending, offset, count);
                return ToCtSharpSortedSetRedisValue(entries);
            }
            entries = database.SortedSetRangeByScoreWithScores(key, minScore, maxScore, Exclude.None, Order.Ascending, offset, count);
            return ToCtSharpSortedSetRedisValue(entries);
        }

        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByScoreWithScoresDesc(string key, double maxScore, double minScore, long offset, long count, Exclusive exclusive = Exclusive.None)
        {
            SortedSetEntry[] entries = null;
            if (exclusive.Equals(Exclusive.Both))
            {
                entries = database.SortedSetRangeByScoreWithScores(key, maxScore, minScore, Exclude.Both, Order.Descending, offset, count);
                return ToCtSharpSortedSetRedisValue(entries);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                entries = database.SortedSetRangeByScoreWithScores(key, maxScore, minScore, Exclude.Start, Order.Descending, offset, count);
                return ToCtSharpSortedSetRedisValue(entries);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                entries = database.SortedSetRangeByScoreWithScores(key, maxScore, minScore, Exclude.Stop, Order.Descending, offset, count);
                return ToCtSharpSortedSetRedisValue(entries);
            }
            entries = database.SortedSetRangeByScoreWithScores(key, maxScore, minScore, Exclude.None, Order.Descending, offset, count);
            return ToCtSharpSortedSetRedisValue(entries);
        }


        private CtSharpRedisValue[] ToCtSharpRedisValue(RedisValue[] redisValues)
        {
            if (redisValues .IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = redisValues[i].ToString();
            }
            return result;
        }

        private KeyValuePair<CtSharpRedisValue,double>[] ToCtSharpSortedSetRedisValue(SortedSetEntry[] redisValues)
        {
            if (redisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, double>>();
            }

            KeyValuePair<CtSharpRedisValue, double>[] result = new KeyValuePair<CtSharpRedisValue, double>[redisValues.Length];
            for (int i = 0; i < redisValues.Length; i++)
            {
                result[i] = new KeyValuePair<CtSharpRedisValue, double>(redisValues[i].Element.ToString(), redisValues[i].Score);
            }
            return result;
        }


        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从小到大排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByRankWithScoresAsc(string key, long start, long stop)
        {
            var redisValue = database.SortedSetRangeByRankWithScores(key, start, stop, Order.Ascending);
            if (redisValue == null || redisValue.Length <= 0)
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, double>>();
            }

            KeyValuePair<CtSharpRedisValue, double>[] result = new KeyValuePair<CtSharpRedisValue, double>[redisValue.Length];
            for (int i = 0; i < redisValue.Length; i++)
            {
                result[i] = new KeyValuePair<CtSharpRedisValue, double>(redisValue[i].Element.ToString(), redisValue[i].Score);
            }

            return result;
        }

        /// <summary>
        /// 返回指定范围的元素,返回元素按分值从小到大排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public KeyValuePair<CtSharpRedisValue, double>[] SortedSetRangeByRankWithScoresDesc(string key, long start, long stop)
        {
            var redisValue = database.SortedSetRangeByRankWithScores(key, start, stop, Order.Descending);
            if (redisValue == null || redisValue.Length <= 0)
            {
                return Array.Empty<KeyValuePair<CtSharpRedisValue, double>>();
            }

            KeyValuePair<CtSharpRedisValue, double>[] result = new KeyValuePair<CtSharpRedisValue, double>[redisValue.Length];
            for (int i = 0; i < redisValue.Length; i++)
            {
                result[i] = new KeyValuePair<CtSharpRedisValue, double>(redisValue[i].Element.ToString(), redisValue[i].Score);
            }

            return result;
        }

        public long? SortedSetRankAsc<T>(string key, T value)
        {
            return database.SortedSetRank(key, CtSharpRedisValue.SerializeRedisValue(value).Value, Order.Ascending);
        }

        public long? SortedSetRankDesc<T>(string key, T value)
        {
            return database.SortedSetRank(key, CtSharpRedisValue.SerializeRedisValue(value).Value, Order.Descending);
        }

        public bool SortedSetRemove<T>(string key, T value)
        {
            return database.SortedSetRemove(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long SortedSetRemoveRangeByValue<TMinMember, TMaxMember>(string key, TMinMember minMember, TMaxMember maxMember, Exclusive exclusive = Exclusive.None)
        {
            var min = CtSharpRedisValue.SerializeRedisValue(minMember).Value;
            var max = CtSharpRedisValue.SerializeRedisValue(maxMember).Value;
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.SortedSetRemoveRangeByValue(key, min, max, Exclude.Both);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.SortedSetRemoveRangeByValue(key, min, max, Exclude.Start);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.SortedSetRemoveRangeByValue(key, min, max, Exclude.Stop);
            }
            return database.SortedSetRemoveRangeByValue(key, min, max, Exclude.None);
        }


        public long SortedSetRemoveRangeByRank(string key, long start, long stop)
        {
            return database.SortedSetRemoveRangeByRank(key, start, stop);
        }

        public long SortedSetRemoveRangeByScore(string key, double minScore, double maxScore, Exclusive exclusive = Exclusive.None)
        {
            if (exclusive.Equals(Exclusive.Both))
            {
                return database.SortedSetRemoveRangeByScore(key, minScore, maxScore, Exclude.Both);
            }
            if (exclusive.Equals(Exclusive.Start))
            {
                return database.SortedSetRemoveRangeByScore(key, minScore, maxScore, Exclude.Start);
            }
            if (exclusive.Equals(Exclusive.Stop))
            {
                return database.SortedSetRemoveRangeByScore(key, minScore, maxScore, Exclude.Stop);
            }
            return database.SortedSetRemoveRangeByScore(key, minScore, maxScore, Exclude.None);
        }

        public double? SortedSetScore<T>(string key, T member)
        {
            return database.SortedSetScore(key, CtSharpRedisValue.SerializeRedisValue(member).Value);
        }

        #endregion

        #region Strings

        public long StringAppend<T>(string key,T value)
        {
            return database.StringAppend(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
        }

        public long StringBitCount(string key, long startIndex,long stopIndex)
        {
            return database.StringBitCount(key, startIndex, stopIndex);
        }

        public long StringBitOperation(string destinationKey, string firstKey, string secondKey, RedisBitwise bitwise= RedisBitwise.And)
        {
            if (bitwise.Equals(RedisBitwise.Not))
            {
                return database.StringBitOperation(Bitwise.Not, destinationKey, firstKey, secondKey);
            }
            if (bitwise.Equals(RedisBitwise.Or))
            {
                return database.StringBitOperation(Bitwise.Or, destinationKey, firstKey, secondKey);
            }
            if (bitwise.Equals(RedisBitwise.Xor))
            {
                return database.StringBitOperation(Bitwise.Xor, destinationKey, firstKey, secondKey);
            }

            return database.StringBitOperation(Bitwise.And, destinationKey, firstKey,secondKey);
        }

        public long StringBitOperation(string destinationKey, string[] keys, RedisBitwise bitwise = RedisBitwise.And)
        {
            RedisKey[] redisKeys = null;
            if (keys.IsNullOrEmptyArrary())
            {
                redisKeys = Array.Empty<RedisKey>();
            }
            else
            {
                redisKeys = new RedisKey[keys.Length];
                for (int i = 0; i < keys.Length; i++)
                {
                    redisKeys[i] = keys[i];
                }
            }
            if (bitwise.Equals(RedisBitwise.Not))
            {
                return database.StringBitOperation(Bitwise.Not, destinationKey, redisKeys);
            }
            if (bitwise.Equals(RedisBitwise.Or))
            {
                return database.StringBitOperation(Bitwise.Or, destinationKey, redisKeys);
            }
            if (bitwise.Equals(RedisBitwise.Xor))
            {
                return database.StringBitOperation(Bitwise.Xor, destinationKey, redisKeys);
            }

            return database.StringBitOperation(Bitwise.And, destinationKey, redisKeys);
        }

        public long StringBitPosition(string key, bool bit, long start = 0, long end = -1)
        {
            return database.StringBitPosition(key, bit, start, end);
        }

        public long StringDecrement(string key,long value=1)
        {
            return database.StringDecrement(key, value);
        }

        public double StringDecrement(string key, double value)
        {
            return database.StringDecrement(key, value);
        }

        public T StringGet<T>(string key)
        {
            RedisValue redisValue = database.StringGet(key);
            if (redisValue.HasValue)
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue.ToString());
            }

            return default(T);
        }

        public CtSharpRedisValue[] StringGet(string[] keys)
        {
            if (keys.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                redisKeys[i] = keys[i];
            }
            RedisValue[] redisValues = database.StringGet(redisKeys);
            return ToCtSharpRedisValue(redisValues);
        }

        public bool StringGetBit(string key, long offset)
        {
            return database.StringGetBit(key, offset);
        }

        public T StringGetRange<T>(string key, long start, long end)
        {
            RedisValue redisValue = database.StringGetRange(key, start, end);
            if (redisValue.HasValue)
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue.ToString());
            }

            return default(T);
        }

        public T StringGetSet<T>(string key, T value)
        {
            RedisValue redisValue = database.StringGetSet(key, CtSharpRedisValue.SerializeRedisValue(value).Value);
            if (redisValue.HasValue)
            {
                return CtSharpRedisValue.DeserializeRedisValue<T>(redisValue.ToString());
            }

            return default(T);
        }

        public long StringIncrement(string key, long value = 1)
        {
            return database.StringIncrement(key, value);
        }

        public double StringIncrement(string key, double value = 1)
        {
            return database.StringIncrement(key, value);
        }

        public long StringLength(string key)
        {
            return database.StringLength(key);
        }

        public bool StringSet<T>(string key, T value, TimeSpan? expiry = null, CtSharpWhen when = CtSharpWhen.Always)
        {
            if (when.Equals(CtSharpWhen.Exists))
            {
                return database.StringSet(key, CtSharpRedisValue.SerializeRedisValue(value).Value, expiry,When.Exists);
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.StringSet(key, CtSharpRedisValue.SerializeRedisValue(value).Value, expiry, When.NotExists);
            }

            return database.StringSet(key, CtSharpRedisValue.SerializeRedisValue(value).Value, expiry, When.Always);
        }


        public bool StringSet(KeyValuePair<string, CtSharpRedisValue>[] keyValues, CtSharpWhen when = CtSharpWhen.Always)
        {
            KeyValuePair<RedisKey, RedisValue>[] redisValues = null;
            if (keyValues.IsNullOrEmptyArrary())
            {
                redisValues = Array.Empty<KeyValuePair<RedisKey, RedisValue>>();
            }
            else
            {
                redisValues=new KeyValuePair<RedisKey, RedisValue>[keyValues.Length];
                for (int i = 0; i < keyValues.Length; i++)
                {
                    redisValues[i] = new KeyValuePair<RedisKey, RedisValue>(keyValues[i].Key, keyValues[i].Value.Value);
                }
            }

            if (when.Equals(CtSharpWhen.Exists))
            {
                return database.StringSet(redisValues, When.Exists);
            }
            if (when.Equals(CtSharpWhen.NotExists))
            {
                return database.StringSet(redisValues, When.NotExists);
            }

            return database.StringSet(redisValues, When.Always);
        }

        public bool StringSetBit(string key, long offset, bool bit)
        {
            return database.StringSetBit(key, offset, bit);
        }

        public long StringSetRange<T>(string key, long offset, T value)
        {
            var redisValue = database.StringSetRange(key, offset, CtSharpRedisValue.SerializeRedisValue(value).Value);
            if (redisValue.HasValue)
            {
                return (long) redisValue;
            }

            return 0;
        }

        #endregion
    }
}
