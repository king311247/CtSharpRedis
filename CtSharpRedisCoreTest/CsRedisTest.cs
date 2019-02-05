#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CtSharpRedis;
using CtSharpRedis.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

#endregion

namespace CtSharpRedisCoreTest
{
    [TestClass]
    public class CsRedisTest : TestBase
    {
        //#region PubSub

        //[TestMethod]
        //public void TestPub()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        redisClient.Publish("test", "测试一");

        //        redisClient.Publish("AAAA2", "测试二");
        //    }
        //}

        //public void TestSub()
        //{
        //    redisClient.Subscribe("test", (channel, body) =>
        //    {
        //        Console.WriteLine(body);
        //    });
        //}


        //#endregion

        #region Keys

        [TestMethod]
        public void Del()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_null1", string.Empty),
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_string1", String),
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_bytes1", Bytes),
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_class1", JsonConvert.SerializeObject(Info))
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));
            string[] keys = {"TestDel_null1", "TestDel_string1", "TestDel_bytes1", "TestDel_class1"};
            long delLen = defaultRds.KeyDelete(keys);
            Assert.IsTrue(4 == delLen);
        }

        [TestMethod]
        public void Exists()
        {
            Assert.IsFalse(defaultRds.KeyExists("TestExists_null1"));
            Assert.IsTrue(defaultRds.StringSet("TestExists_null1", 1));
            Assert.IsTrue(defaultRds.KeyExists("TestExists_null1"));
            Assert.IsTrue(defaultRds.KeyDelete("TestExists_null1"));
            Assert.IsFalse(defaultRds.KeyExists("TestExists_null1"));
        }

        [TestMethod]
        public void Expire()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestExpire_null1", string.Empty)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));

            Assert.IsTrue(defaultRds.KeyExpire("TestExpire_null1", new TimeSpan(0, 0, 10)));
            long ttl = defaultRds.KeyTimeToLive("TestExpire_null1").Value.Seconds;
            Assert.IsTrue(ttl > 5 && ttl <= 10);
        }

        [TestMethod]
        public void ExpireAt()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestExpireAt_null1", string.Empty)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));
            Assert.IsTrue(defaultRds.KeyExpire("TestExpireAt_null1", DateTime.UtcNow.AddSeconds(10)));
        }


        [TestMethod]
        public void Move()
        {
            redisClient.GetDatabase(1).KeyDelete("TestMove_string1");
            defaultRds.KeyDelete("TestMove_string1");
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =new KeyValuePair<string, CtSharpRedisValue>[]
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestMove_string1", String)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));
            Assert.IsTrue(defaultRds.KeyMove("TestMove_string1", 1));
            Assert.IsFalse(defaultRds.KeyExists("TestMove_string1"));

            Assert.IsTrue(redisClient.GetDatabase(1).KeyExists("TestMove_string1"));
        }

        [TestMethod]
        public void Persist()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestPersist_null1", String)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));
            Assert.IsTrue(defaultRds.KeyExpire("TestPersist_null1", TimeSpan.FromSeconds(100)));
            long ttl = defaultRds.KeyTimeToLive("TestPersist_null1").Value.Seconds;

            Assert.IsTrue(ttl > 0);
            Assert.IsTrue(ttl <= 100);
            Assert.IsTrue(defaultRds.KeyPersist("TestPersist_null1"));
            TimeSpan? exp = defaultRds.KeyTimeToLive("TestPersist_null1");

            Assert.IsFalse(exp.HasValue);
        }


        [TestMethod]
        public void Rename()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestRename_string1", String)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));
            Assert.IsTrue(String.Equals(defaultRds.StringGet<string>("TestRename_string1")));
            Assert.IsTrue(defaultRds.KeyRename("TestRename_string1", "TestRename_string11"));
            Assert.IsFalse(defaultRds.KeyExists("TestRename_string1"));
            Assert.IsTrue(String.Equals(defaultRds.StringGet<string>("TestRename_string11")));
        }

        [TestMethod]
        public void RenameNx()
        {
            defaultRds.KeyDelete("TestRenameNx_string1");
            defaultRds.KeyDelete("TestRenameNx_string11");
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestRenameNx_string1", String)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));
            Assert.IsTrue(String.Equals(defaultRds.StringGet<string>("TestRenameNx_string1")));
            Assert.IsTrue(defaultRds.KeyRename("TestRenameNx_string1", "TestRenameNx_string11", CtSharpWhen.NotExists));
            Assert.IsFalse(defaultRds.KeyExists("TestRenameNx_string1"));
            Assert.IsTrue(String.Equals(defaultRds.StringGet<string>("TestRenameNx_string11")));
        }


        [TestMethod]
        public void Ttl()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestTtl_null1", String)
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));

            Assert.IsTrue(defaultRds.KeyExpire("TestTtl_null1", TimeSpan.FromSeconds(10)));
            long ttl = defaultRds.KeyTimeToLive("TestTtl_null1").Value.Seconds;
            Assert.IsTrue(ttl > 5);
            Assert.IsTrue(ttl <= 10);
        }

        #endregion

        #region Strings

        [TestMethod]
        public void Append()
        {
            var key = "TestAppend_null";

            var aa = defaultRds;

            defaultRds1.StringSet(key, String);
            aa.StringSet(key+"33", String);

            defaultRds.StringSet(key, String);

            defaultRds1.StringSet(key, String);


            defaultRds.StringAppend(key, Null);
            Assert.IsTrue(String.Equals(defaultRds.StringGet<string>(key)));

            key = "TestAppend_string";
            defaultRds.StringSet(key, String);
            defaultRds.StringAppend(key, String);
            Assert.IsTrue((String + String).Equals(defaultRds.StringGet<string>(key)));

            key = "TestAppend_bytes";
            defaultRds.StringSet(key, Bytes);
            defaultRds.StringAppend(key, Bytes);
            Assert.IsTrue(Convert.ToBase64String(Bytes.Concat(Bytes).ToArray()).Equals(Convert.ToBase64String(defaultRds.StringGet<byte[]>(key))));
        }

        [TestMethod]
        public void BitCount()
        {
            var key = "TestBitCount";
            defaultRds.KeyDelete(key);
            defaultRds.StringSetBit(key, 100, true);
            defaultRds.StringSetBit(key, 90, true);
            defaultRds.StringSetBit(key, 80, true);
            Assert.AreEqual(3, defaultRds.StringBitCount(key, 0, 100));
            Assert.AreEqual(3, defaultRds.StringBitCount(key, 0, 99));
            Assert.AreEqual(3, defaultRds.StringBitCount(key, 0, 60));
        }

        [TestMethod]
        public void Get()
        {
            var key = "TestGet_null";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, string.Empty);
            Assert.IsTrue(string.IsNullOrEmpty(defaultRds.StringGet<string>(key)));

            key = "TestGet_string";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, String);
            Assert.AreEqual(String, defaultRds.StringGet<string>(key));

            key = "TestGet_bytes";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, Bytes);
            Assert.IsTrue(defaultRds.StringGet<byte[]>(key).SequenceEqual(Bytes));

            defaultRds.StringSet(key, Info);
            var info = defaultRds.StringGet<TestInfo>(key);
            Assert.AreEqual(info.CreateTime, Info.CreateTime);
            Assert.AreEqual(info.Id, Info.Id);
            Assert.AreEqual(info.Name, Info.Name);
            Assert.IsTrue(info.TagId.SequenceEqual(Info.TagId));

            key = "TestGet_classArray";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, new[] {Info, Info});
            Assert.AreEqual(2, defaultRds.StringGet<TestInfo[]>(key).Length);
        }

        [TestMethod]
        public void GetBit()
        {
            var key = "TestGetBit";
            defaultRds.StringSetBit(key, 100, true);
            defaultRds.StringSetBit(key, 90, true);
            defaultRds.StringSetBit(key, 80, true);

            Assert.IsTrue(defaultRds.StringGetBit(key, 100));
            Assert.IsTrue(defaultRds.StringGetBit(key, 90));
            Assert.IsTrue(defaultRds.StringGetBit(key, 80));
            Assert.IsFalse(defaultRds.StringGetBit(key, 79));
        }

        [TestMethod]
        public void GetRange()
        {
            var key = "TestGetRange_null";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, string.Empty);
            Assert.IsTrue(string.IsNullOrEmpty(defaultRds.StringGetRange<string>(key, 10, 20)));

            key = "TestGetRange_string";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, "abcdefg");
            Assert.IsTrue("cde".Equals(defaultRds.StringGetRange<string>(key, 2, 4)));
            Assert.IsTrue("abcdefg".Equals(defaultRds.StringGetRange<string>(key, 0, -1)));
        }

        [TestMethod]
        public void GetSet()
        {
            var key = "TestGetSet_null";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, string.Empty,TimeSpan.FromMilliseconds(1));
            Assert.IsTrue(string.IsNullOrEmpty(defaultRds.StringGetSet(key, string.Empty)));

            key = "TestGetSet_string";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, String);
            Assert.IsTrue(String.Equals(defaultRds.StringGetSet(key, "newvalue")));

            Assert.AreEqual("newvalue", defaultRds.StringGet<string>(key));
        }

        [TestMethod]
        public void IncrBy()
        {
            var key = "TestIncrBy_null";
            defaultRds.KeyDelete(key);
            defaultRds.StringSet(key, Null);
            Assert.AreEqual(1, defaultRds.StringIncrement(key, 1));


            key = "TestIncrBy";
            defaultRds.KeyDelete(key);
            Assert.AreEqual(1, defaultRds.StringIncrement(key, 1));
            Assert.AreEqual(11, defaultRds.StringIncrement(key, 10));
            Assert.AreEqual(21.5, defaultRds.StringIncrement(key, 10.5));
        }

        [TestMethod]
        public void MGet()
        {
            string[] keys = {"TestMGet_null1", "TestMGet_string1", "TestMGet_bytes1", "TestMGet_class1"};
            defaultRds.KeyDelete(keys);
            defaultRds.StringSet("TestMGet_null1", Null);
            defaultRds.StringSet("TestMGet_string1", String);
            defaultRds.StringSet("TestMGet_bytes1", Bytes);
            defaultRds.StringSet("TestMGet_class1", Info);

            CtSharpRedisValue[] redisValues = defaultRds.StringGet(keys);

            Assert.AreEqual(4, redisValues.Length);
            Assert.IsTrue(string.IsNullOrEmpty((string) redisValues[0]));
            Assert.AreEqual(String, (string) redisValues[1]);
            Assert.AreEqual(Encoding.UTF8.GetString(Bytes), (string) redisValues[2]);
            Assert.AreEqual(Info.CreateTime, CtSharpRedisValue.DeserializeRedisValue<TestInfo>(redisValues[3]).CreateTime);
        }

        [TestMethod]
        public void MSet()
        {
            string[] keys = {"TestMSet_null1", "TestMSet_string1", "TestMSet_bytes1", "TestMSet_class1"};
            defaultRds.KeyDelete(keys);
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestMSet_null1", string.Empty),
                new KeyValuePair<string, CtSharpRedisValue>("TestMSet_string1", String),
                new KeyValuePair<string, CtSharpRedisValue>("TestMSet_bytes1", Bytes),
                new KeyValuePair<string, CtSharpRedisValue>("TestMSet_class1", JsonConvert.SerializeObject(Info))
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));

            Assert.IsTrue(string.IsNullOrEmpty(defaultRds.StringGet<string>("TestMSet_null1")));
            Assert.AreEqual(String, defaultRds.StringGet<string>("TestMSet_string1"));
            Assert.IsTrue(Bytes.SequenceEqual(defaultRds.StringGet<byte[]>("TestMSet_bytes1")));

            var info = defaultRds.StringGet<TestInfo>("TestMSet_class1");
            Assert.AreEqual(Info.CreateTime, info.CreateTime);
        }

        [TestMethod]
        public void MSetNx()
        {
            KeyValuePair<string, CtSharpRedisValue>[] redisValue =
            {
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_null1", string.Empty),
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_string1", String),
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_bytes1", Bytes),
                new KeyValuePair<string, CtSharpRedisValue>("TestDel_class1", JsonConvert.SerializeObject(Info))
            };
            Assert.IsTrue(defaultRds.StringSet(redisValue));

            Assert.IsFalse(defaultRds.StringSet(redisValue, CtSharpWhen.NotExists));
        }

        [TestMethod]
        public void Set()
        {
            Assert.IsTrue(defaultRds.StringSet("TestSet_null", Null,TimeSpan.FromMinutes(1)));
            // 重复设置过期时间测试
            Assert.IsTrue(defaultRds.StringSet("TestSet_null", Null, TimeSpan.FromMinutes(1)));
            Assert.IsTrue(string.IsNullOrEmpty(defaultRds.StringGet<string>("TestSet_null")));
        }

        [TestMethod]
        public void SetBit()
        {
            var key = "TestSetBit";
            defaultRds.StringSetBit(key, 100, true);
            defaultRds.StringSetBit(key, 90, true);
            defaultRds.StringSetBit(key, 80, true);
            Assert.IsTrue(defaultRds.StringGetBit(key, 100));
            Assert.IsTrue(defaultRds.StringGetBit(key, 90));
            Assert.IsTrue(defaultRds.StringGetBit(key, 80));
            Assert.IsFalse(defaultRds.StringGetBit(key, 79));
        }

        [TestMethod]
        public void SetNx()
        {
            defaultRds.KeyDelete("TestSetNx_null");
            Assert.IsTrue(defaultRds.StringSet("TestSetNx_null", string.Empty, null, CtSharpWhen.NotExists));
            Assert.IsFalse(defaultRds.StringSet("TestSetNx_null", string.Empty, null, CtSharpWhen.NotExists));
        }

        [TestMethod]
        public void SetRange()
        {
            var key = "TestSetRange_null";
            defaultRds.KeyDelete("TestSetNx_null");
            Assert.IsTrue(defaultRds.StringSet("TestSetNx_null", string.Empty, null, CtSharpWhen.NotExists));
            defaultRds.StringSetRange(key, 10, String);
            Assert.AreEqual(String, defaultRds.StringGetRange<string>(key, 10, -1));
        }

        [TestMethod]
        public void StrLen()
        {
            var key = "TestStrLen_null";
            defaultRds.StringSet(key, Null);
            long len = defaultRds.StringLength(key);
            Assert.AreEqual(0, len);

            key = "TestStrLen_string";
            defaultRds.StringSet(key, "abcdefg");
            len = defaultRds.StringLength(key);
            Assert.AreEqual(7, len);

            key = "TestStrLen_bytes";
            defaultRds.StringSet(key, Bytes);
            len = defaultRds.StringLength(key);

            Assert.AreEqual(Bytes.Length, len);
        }

        #endregion

        #region Hashes

        [TestMethod]
        public void HDel()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHDel", redisValuePairs);
            CtSharpRedisValue[] fields =
            {
                "string1",
                "bytes1",
                "class1"
            };

            Assert.AreEqual(3, defaultRds.HashDelete("TestHDel", fields));
        }

        [TestMethod]
        public void HExists()
        {
            defaultRds.HashDelete("TestHExists", "null1");


            bool isOk = defaultRds.HashSet("TestHExists", "null1", 1);
            Assert.IsTrue(isOk);
            int value = defaultRds.HashGet<int>("TestHExists", "null1");
            Assert.AreEqual(1, value);
            Assert.IsTrue(defaultRds.HashExists("TestHExists", "null1"));
        }

        [TestMethod]
        public void HGet()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHGet", redisValuePairs);
            Assert.AreEqual(String, defaultRds.HashGet<string>("TestHGet", "string1"));
            Assert.IsTrue(Bytes.SequenceEqual(defaultRds.HashGet<byte[]>("TestHGet", "bytes1")));

            var info = defaultRds.HashGet<TestInfo>("TestHGet", "class1");
            Assert.AreEqual(info.CreateTime, Info.CreateTime);
        }

        [TestMethod]
        public void HGetAll()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHGetAll", redisValuePairs);
            var redisValues = defaultRds.HashGetAll("TestHGetAll");

            Assert.AreEqual(3, redisValues.Length);
            Assert.AreEqual(String, (string) redisValues[0].Value);
            Assert.AreEqual(Encoding.UTF8.GetString(Bytes), (string) redisValues[1].Value);
            var info = CtSharpRedisValue.DeserializeRedisValue<TestInfo>(redisValues[2].Value);
            Assert.AreEqual(Info.ToString(), info.ToString());
        }

        [TestMethod]
        public void HIncr()
        {
            string key = "TestHIncrBy";
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("long", 0),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("float", 0)
            };
            defaultRds.KeyDelete(key);
            defaultRds.HashSet(key, redisValuePairs);

            Assert.IsTrue(defaultRds.HashIncrement(key, "long", 1) == 1);
            Assert.IsTrue(defaultRds.HashIncrement(key, "long", 2) == 3);

            Assert.IsTrue(defaultRds.HashDecrement(key, "long", 1) == 2);
            Assert.IsTrue(defaultRds.HashDecrement(key, "long", 2) == 0);

            Assert.IsTrue(defaultRds.HashIncrement(key, "float", 1.1) >= 1.1);
            Assert.IsTrue(defaultRds.HashIncrement(key, "float", 2.2) >= 3.2);

            Assert.IsTrue(defaultRds.HashDecrement(key, "float", 1.1) <= 2.2);
            Assert.IsTrue(defaultRds.HashDecrement(key, "float", 2.2) >= 0);
        }


        [TestMethod]
        public void HKeys()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHKeys", redisValuePairs);

            Assert.AreEqual(3, defaultRds.HashKeys("TestHKeys").Length);
        }

        [TestMethod]
        public void HLen()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHLen", redisValuePairs);
            Assert.AreEqual(3, defaultRds.HashLength("TestHLen"));
        }

        [TestMethod]
        public void HMGet()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHMGet", redisValuePairs);

            var redisValue = defaultRds.HashGet("TestHMGet", redisValuePairs.Select(o => o.Key).ToArray());

            Assert.AreEqual(3, redisValue.Length);
            Assert.AreEqual(String, (string) redisValue[0]);
        }

        [TestMethod]
        public void HMSet()
        {
            KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>[] redisValuePairs =
            {
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("string1", String),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("bytes1", Bytes),
                new KeyValuePair<CtSharpRedisValue, CtSharpRedisValue>("class1", CtSharpRedisValue.SerializeRedisValue(Info))
            };
            defaultRds.HashSet("TestHMSet", redisValuePairs);
            Assert.AreEqual(3, defaultRds.HashKeys("TestHMSet").Length);
        }

        [TestMethod]
        public void HSet()
        {
            string key = "TestHSet";
            defaultRds.KeyDelete(key);
            defaultRds.HashSet(key, "test", "asdasd");

            Assert.AreEqual(defaultRds.HashGet<string>(key, "test"), "asdasd");
        }

        [TestMethod]
        public void HSetNx()
        {
            string key = "TestHSetNx";
            defaultRds.KeyDelete(key);
            defaultRds.HashSet(key, "test", "asdasd");
            Assert.AreEqual(defaultRds.HashGet<string>(key, "test"), "asdasd");

            Assert.IsFalse(defaultRds.HashSet(key, "test", "asdasd", CtSharpWhen.NotExists));
        }

        #endregion

        #region HyperLogLog

        [TestMethod, TestCategory("HyperLogLog")]
        public void PfAddTest()
        {
            string key = "HyperLogLogAdd";
            defaultRds.HyperLogLogAdd(key, "a");
            defaultRds.HyperLogLogAdd(key, "acc");
            var len = defaultRds.HyperLogLogLength(key);

            Assert.AreEqual(2, len);
        }

        [TestMethod, TestCategory("HyperLogLog")]
        public void MPfAddTest()
        {
            string key = "MHyperLogLogAdd";
            CtSharpRedisValue[] redisvalue =
            {
                "a",
                "b"
            };
            defaultRds.HyperLogLogAdd(key, redisvalue);
            var len = defaultRds.HyperLogLogLength(key);
            Assert.AreEqual(2, len);
        }

        [TestMethod, TestCategory("HyperLogLog")]
        public void PfCountTest()
        {
            string key = "HyperLogLogLength";
            defaultRds.HyperLogLogAdd(key, "a");
            defaultRds.HyperLogLogAdd(key, "acc");
            var len = defaultRds.HyperLogLogLength(key);

            Assert.AreEqual(2, len);
        }

        [TestMethod, TestCategory("HyperLogLog")]
        public void MPfCountTest()
        {
            string key = "MHyperLogLogAdd1";
            defaultRds.HyperLogLogAdd(key, "a");
            defaultRds.HyperLogLogAdd(key, "acc");

            key = "MHyperLogLogAdd2";
            defaultRds.HyperLogLogAdd(key, "a");
            defaultRds.HyperLogLogAdd(key, "accdd");
            string[] keys = {"MHyperLogLogAdd1", "MHyperLogLogAdd2"};

            var len = defaultRds.HyperLogLogLength(keys);
            Assert.AreEqual(3, len);
        }

        [TestMethod, TestCategory("HyperLogLog")]
        public void PfMergeTest()
        {
            string key = "HyperLogLogMerge1";
            defaultRds.HyperLogLogAdd(key, "a");
            defaultRds.HyperLogLogAdd(key, "acc");

            key = "HyperLogLogMerge2";
            defaultRds.HyperLogLogAdd(key, "a");
            defaultRds.HyperLogLogAdd(key, "accdd");
            string[] keys = {"HyperLogLogMerge1", "HyperLogLogMerge2"};
            defaultRds.HyperLogLogMerge("HyperLogLogMerge3", keys);

            var len = defaultRds.HyperLogLogLength("HyperLogLogMerge3");
            Assert.AreEqual(3, len);
        }

        #endregion

        #region Sets

        [TestMethod, TestCategory("Sets")]
        public void TestSAdd()
        {
            defaultRds.KeyDelete("TestSAdd");
            Assert.IsTrue(defaultRds.SetAdd("TestSAdd", "test1"));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSCard()
        {
            defaultRds.KeyDelete("TestSCard");
            Assert.IsTrue(defaultRds.SetAdd("TestSCard", "test1"));
            Assert.AreEqual(1,defaultRds.SetLength("TestSCard"));

        }

        [TestMethod, TestCategory("Sets")]
        public void TestSDiff()
        {
            defaultRds.KeyDelete("TestSDiff1");
            defaultRds.KeyDelete("TestSDiff2");
            Assert.IsTrue(defaultRds.SetAdd("TestSDiff1", "test1"));
            Assert.IsTrue(defaultRds.SetAdd("TestSDiff1", "test3"));
            Assert.IsTrue(defaultRds.SetAdd("TestSDiff2", "test2"));

            Assert.AreEqual(2, defaultRds.SetCombineDiff("TestSDiff1", "TestSDiff2").Length);
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSDiffStore()
        {
            defaultRds.KeyDelete("TestSDiffStore1");
            defaultRds.KeyDelete("TestSDiffStore2");
            defaultRds.KeyDelete("TestSDiffStore3");
            Assert.IsTrue(defaultRds.SetAdd("TestSDiffStore1", "test1"));
            Assert.IsTrue(defaultRds.SetAdd("TestSDiffStore2", "test2"));


            Assert.AreEqual(1, defaultRds.SetCombineAndStoreDiff("TestSDiffStore3", "TestSDiffStore1", "TestSDiffStore2"));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestInter()
        {
            defaultRds.KeyDelete("TestInter1");
            defaultRds.KeyDelete("TestInter2");
            Assert.IsTrue(defaultRds.SetAdd("TestInter1", "test1"));
            Assert.IsTrue(defaultRds.SetAdd("TestInter1", "test2"));

            Assert.IsTrue(defaultRds.SetAdd("TestInter2", "test2"));

            Assert.AreEqual(1, defaultRds.SetCombineInter("TestInter1", "TestInter2").Length);
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSInterStore()
        {
            defaultRds.KeyDelete("TestSInterStore1");
            defaultRds.KeyDelete("TestSInterStore2");
            defaultRds.KeyDelete("TestSInterStore3");
            Assert.IsTrue(defaultRds.SetAdd("TestSInterStore1", "test1"));
            Assert.IsTrue(defaultRds.SetAdd("TestSInterStore1", "test2"));

            Assert.IsTrue(defaultRds.SetAdd("TestSInterStore2", "test2"));

            Assert.AreEqual(1, defaultRds.SetCombineAndStoreInter("TestSInterStore3", "TestSInterStore1", "TestSInterStore2"));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSIsMember()
        {
            defaultRds.KeyDelete("TestSIsMember");
            Assert.IsTrue(defaultRds.SetAdd("TestSIsMember", "test1"));
            Assert.IsTrue(defaultRds.SetContains("TestSIsMember", "test1"));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSMembers()
        {
            defaultRds.KeyDelete("TestSMembers");
            Assert.IsTrue(defaultRds.SetAdd("TestSMembers", "test1"));
            Assert.AreEqual(1,defaultRds.SetMembers("TestSMembers").Length);
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSMove()
        {
            string key = "TestSMove";
            string targetKey = "TestSMove2";
            defaultRds.KeyDelete(key);
            defaultRds.KeyDelete(targetKey);
            Assert.IsTrue(defaultRds.SetAdd(key, "test1"));
            Assert.IsTrue(defaultRds.SetAdd(targetKey, "test2"));

            Assert.IsTrue(defaultRds.SetMove(key, targetKey, "test1"));

            Assert.IsFalse(defaultRds.SetContains(key, "test1"));
            Assert.IsTrue(defaultRds.SetContains(targetKey, "test1"));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSPop()
        {
            string key = "TestSPop";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SetAdd(key, "test1"));
            Assert.IsTrue(defaultRds.SetAdd(key, "test2"));
            string value = defaultRds.SetPop<string>(key);

            Assert.IsTrue(!string.IsNullOrEmpty(value));
            Assert.AreEqual(1,defaultRds.SetLength(key));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSRandMember()
        {
            string key = "TestSRandMember";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SetAdd(key, "test1"));
            Assert.IsTrue(defaultRds.SetAdd(key, "test2"));
            Assert.IsTrue(defaultRds.SetAdd(key, "test3"));
            Assert.IsTrue(defaultRds.SetAdd(key, "test4"));

            var value = defaultRds.SetRandomMember<string>(key);
            Assert.IsTrue(!string.IsNullOrEmpty(value));
            Assert.AreEqual(4, defaultRds.SetLength(key));

            var values = defaultRds.SetRandomMembers(key, 2);
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual(4, defaultRds.SetLength(key));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSRem()
        {
            string key = "TestSRem";
            defaultRds.KeyDelete(key);
            CtSharpRedisValue[] values=new CtSharpRedisValue[]
            {
                "test1",
                "test2",
                "test3",
                "test4",
            };
            Assert.IsTrue(defaultRds.SetAdd(key, values) > 0);
            defaultRds.SetRemove<string>(key, "test1");
            Assert.AreEqual(3, defaultRds.SetLength(key));

            defaultRds.SetRemove(key,new CtSharpRedisValue[]
            {
                "test2",
                "test3",
            });
            Assert.AreEqual(1, defaultRds.SetLength(key));
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSUnion()
        {
            defaultRds.KeyDelete("TestSUnion1");
            defaultRds.KeyDelete("TestSUnion2");
            Assert.IsTrue(defaultRds.SetAdd("TestSUnion1", "test1"));
            Assert.IsTrue(defaultRds.SetAdd("TestSUnion2", "test2"));

            Assert.AreEqual(2, defaultRds.SetCombineUnion("TestInter1", "TestInter2").Length);
        }

        [TestMethod, TestCategory("Sets")]
        public void TestSUnionStore()
        {
            defaultRds.KeyDelete("TestSUnionStore1");
            defaultRds.KeyDelete("TestSUnionStore2");
            Assert.IsTrue(defaultRds.SetAdd("TestSUnionStore1", "test1"));
            Assert.IsTrue(defaultRds.SetAdd("TestSUnionStore2", "test2"));

            Assert.AreEqual(2, defaultRds.SetCombineAndStoreUnion("TestSUnionStore3", "TestSUnionStore1", "TestSUnionStore2"));
            Assert.AreEqual(1, defaultRds.SetLength("TestSUnionStore1"));
            Assert.AreEqual(1, defaultRds.SetLength("TestSUnionStore2"));

            Assert.IsTrue(defaultRds.SetAdd("TestSUnionStore1", "test3"));
            Assert.IsTrue(defaultRds.SetAdd("TestSUnionStore2", "test4"));

            Assert.AreEqual(4, defaultRds.SetCombineAndStoreUnion("TestSUnionStore4", new[]
            {
                "TestSUnionStore1", "TestSUnionStore2"
            }));

        }


        #endregion

        #region Sortedsets

        [TestMethod, TestCategory("SortedSets")]
        public void TestZAdd()
        {
            
            string key = "TestZAdd_Array";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key,new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            })>0);

            Assert.AreEqual(4,defaultRds.SortedSetLength(key));
        }


        [TestMethod, TestCategory("SortedSets")]
        public void TestZCard()
        {
            string key = "TestZCard";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);

            Assert.AreEqual(4, defaultRds.SortedSetLength(key));
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZCount()
        {
            string key = "TestZCount";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);

            Assert.AreEqual(3, defaultRds.SortedSetLength(key,1,2));
            Assert.AreEqual(0, defaultRds.SortedSetLength(key, 1, 2,Exclusive.Both));
            Assert.AreEqual(2, defaultRds.SortedSetLength(key, 1, 2, Exclusive.Start));
            Assert.AreEqual(1, defaultRds.SortedSetLength(key, 1, 2, Exclusive.Stop));
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZIncrby()
        {
            string key = "TestZIncrby";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));
            Assert.IsTrue(defaultRds.SortedSetIncrement(key, "test1", 1) >= 2);

            Assert.IsTrue(defaultRds.SortedSetDecrement(key, "test1", 1) >= 1);
            Assert.IsTrue(defaultRds.SortedSetScore(key, "test1") >= 1);
            Assert.IsTrue(defaultRds.SortedSetScore(key, "test1") <2);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZInterStore()
        {
            string key = "TestZInterStore1";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);

            string key2 = "TestZInterStore2";
            defaultRds.KeyDelete(key2);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key2, "test2", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key2, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);

            Assert.IsTrue(defaultRds.SortedSetCombineAndStoreInter("TestZInterStore3", key, key2) == 3);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZLexCount()
        {
            string key = "TestZLexCount";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);
            var len = defaultRds.SortedSetLengthByValue(key, "test1", "test33");
            Assert.IsTrue(len == 3);
            len = defaultRds.SortedSetLengthByValue(key, "test1", "test4", Exclusive.Both);
            Assert.IsTrue( len== 2);
            len = defaultRds.SortedSetLengthByValue(key, "test1", "test33", Exclusive.Stop);
            Assert.IsTrue( len== 2);
            len = defaultRds.SortedSetLengthByValue(key, "test1", "test33", Exclusive.Start);
            Assert.IsTrue(len == 2);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void SortedSetRangeByRankAsc()
        {
            string key = "SortedSetRangeByRankAsc";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            var values = defaultRds.SortedSetRangeByRankAsc(key, 0, 2);
            Assert.AreEqual(3, values.Length);
            Assert.AreEqual("test1",values[0].Value);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void SortedSetRangeByRankDesc()
        {
            string key = "SortedSetRangeByRankDesc";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);

           var  values = defaultRds.SortedSetRangeByRankDesc(key, 0, -1);
            Assert.AreEqual(5, values.Length);
            Assert.AreEqual("test5", values[0].Value);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void SortedSetRangeByValueAsc()
        {
            string key = "SortedSetRangeByValueAsc";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);

            var values = defaultRds.SortedSetRangeByValueAsc(key, "test", "test2", 0, 2);
            Assert.AreEqual(2, values.Length);
            values = defaultRds.SortedSetRangeByValueAsc(key, "test1", "test2", 0, 2);
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual("test2", values[1].Value);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void SortedSetRangeByValueDesc()
        {
            string key = "SortedSetRangeByValueDesc";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);

            var values = defaultRds.SortedSetRangeByValueDesc(key, "test", "test2", 0, 2);
            Assert.AreEqual(2, values.Length);
            values = defaultRds.SortedSetRangeByValueDesc(key, "test1", "test2", 0, 2);
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual("test1", values[1].Value);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRangeByScore()
        {
            string key = "TestZRangeByScore";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);

            var values= defaultRds.SortedSetRangeByScoreAsc(key, 1, 2, 1, 1);
            Assert.IsTrue(values.Length == 1);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRangeByScoreWithScores()
        {
            string key = "TestZRangeByScoreWithScores";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            var values = defaultRds.SortedSetRangeByScoreWithScoresAsc(key, 1, 2, 1, 1);

            Assert.IsTrue(values.Length == 1);
            Assert.IsTrue(values[0].Value >= 2);

            values = defaultRds.SortedSetRangeByScoreWithScoresAsc(key, 1, 5, 0, 3);


            Assert.IsTrue(values.Length == 3);
            Assert.IsTrue(values[2].Value >= 3);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRangeByScoreWithScoresDesc()
        {
            string key = "TestZRangeByScoreWithScoresDesc";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            var values = defaultRds.SortedSetRangeByScoreWithScoresDesc(key, 2, 1, 1, 1);

            Assert.IsTrue(values.Length == 1);
            Assert.IsTrue(values[0].Value <=2);

            values = defaultRds.SortedSetRangeByScoreWithScoresDesc(key, 5, 1, 0, 3);


            Assert.IsTrue(values.Length == 3);
            Assert.IsTrue(values[2].Value >= 3);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRank()
        {
            string key = "TestZRank";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            long? rank= defaultRds.SortedSetRankAsc(key, "test");
            Assert.IsFalse(rank.HasValue);

            rank = defaultRds.SortedSetRankAsc(key, "test1");
            Assert.IsTrue(rank.HasValue);
            Assert.AreEqual(rank.Value,0);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRem()
        {
            string key = "TestZRem";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            Assert.IsTrue(defaultRds.SortedSetRankAsc(key, "test1").HasValue);
            Assert.IsTrue(defaultRds.SortedSetRemove(key, "test1"));

            Assert.IsFalse(defaultRds.SortedSetRankAsc(key,"test1").HasValue);

        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRemRangeByLex()
        {
            string key = "TestZRemRangeByLex";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            var removeLen = defaultRds.SortedSetRemoveRangeByValue(key, "test1", "test2");
            Assert.IsTrue( removeLen== 2);
            removeLen = defaultRds.SortedSetRemoveRangeByValue(key, "test33", "test4", Exclusive.Both);
            Assert.IsTrue(removeLen == 0);
            removeLen = defaultRds.SortedSetRemoveRangeByValue(key, "test33", "test4", Exclusive.Stop);
            Assert.IsTrue(removeLen == 1);
            removeLen = defaultRds.SortedSetRemoveRangeByValue(key, "test4", "test5", Exclusive.Start);
            Assert.IsTrue(removeLen== 1);
            var len = defaultRds.SortedSetLength(key);
            Assert.AreEqual(1, len);
            Assert.AreEqual("test4", defaultRds.SortedSetRangeByRankAsc(key,0,-1)[0].Value);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRemRangeByRank()
        {
            string key = "TestZRemRangeByRank";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);

            Assert.AreEqual(2, defaultRds.SortedSetRemoveRangeByRank(key, 0, 1));
            Assert.AreEqual(3, defaultRds.SortedSetLength(key));
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZRemRangeByScore()
        {
            string key = "TestZRemRangeByScore";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",3),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
                new KeyValuePair<CtSharpRedisValue, double>("test5",5),
            }) > 0);
            var len = defaultRds.SortedSetRemoveRangeByScore(key, 0, 1);
            Assert.AreEqual(1, len);
            len = defaultRds.SortedSetLength(key);
            Assert.AreEqual(4, len);
            len = defaultRds.SortedSetRemoveRangeByScore(key, 2, 3, Exclusive.Stop);
            Assert.AreEqual(1, len);
            len = defaultRds.SortedSetRemoveRangeByScore(key, 3, 5, Exclusive.Both);
            Assert.AreEqual(0, len);
            len = defaultRds.SortedSetRemoveRangeByScore(key, 3, 5, Exclusive.Start);
            Assert.AreEqual(1,len );
        }


        [TestMethod, TestCategory("SortedSets")]
        public void TestZScore()
        {
            string key = "TestZScore";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetScore(key, "test1") >= 1);
        }

        [TestMethod, TestCategory("SortedSets")]
        public void TestZUnionStore()
        {
            string key = "TestZUnionStore1";
            defaultRds.KeyDelete(key);
            Assert.IsTrue(defaultRds.SortedSetAdd<string>(key, "test1", 1));

            Assert.IsTrue(defaultRds.SortedSetAdd(key, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);

            string key2 = "TestZUnionStor2";
            defaultRds.KeyDelete(key2);

            Assert.IsTrue(defaultRds.SortedSetAdd(key2, new KeyValuePair<CtSharpRedisValue, double>[]
            {
                new KeyValuePair<CtSharpRedisValue, double>("test2",2),
                new KeyValuePair<CtSharpRedisValue, double>("test33",2),
                new KeyValuePair<CtSharpRedisValue, double>("test4",3),
            }) > 0);

            Assert.IsTrue(defaultRds.SortedSetCombineAndStoreUnion("TestZUnionStore3", key, key2) == 4);
        }


        #endregion

        /// <summary>
        ///     将字符串转成二进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Bianma(string s)
        {
            byte[] data = Encoding.Unicode.GetBytes(s);
            StringBuilder result = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return result.ToString();
        }
    }
}
