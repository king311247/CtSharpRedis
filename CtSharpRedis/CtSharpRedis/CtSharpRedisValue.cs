#region

using System;
using CtSharpRedis.Utils;

#endregion

namespace CtSharpRedis
{
    public struct CtSharpRedisValue
    {
        public string Value { get; set; }

        /// <summary>
        /// 转字符串数组
        /// </summary>
        /// <param name="ctSharpRedisValues"></param>
        /// <returns></returns>
        public static string[] ConvertToStringArrary(CtSharpRedisValue[] ctSharpRedisValues)
        {
            if (ctSharpRedisValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<string>();
            }

            string[] result = new string[ctSharpRedisValues.Length];
            for (int i = 0; i < ctSharpRedisValues.Length; i++)
            {
                result[i] = ctSharpRedisValues[i].Value;
            }

            return result;
        }

        /// <summary>
        /// 转CtSharpRedisValue数组
        /// </summary>
        /// <param name="stringValues"></param>
        /// <returns></returns>
        public static CtSharpRedisValue[] ConvertToRedisValueArrary(string[] stringValues)
        {
            if (stringValues.IsNullOrEmptyArrary())
            {
                return Array.Empty<CtSharpRedisValue>();
            }

            CtSharpRedisValue[] result = new CtSharpRedisValue[stringValues.Length];
            for (int i = 0; i < stringValues.Length; i++)
            {
                result[i] = new CtSharpRedisValue() {Value = stringValues[i]};
            }

            return result;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CtSharpRedisValue SerializeRedisValue<T>(T value)
        {
            return SerializeSettings.SerializeRedisValue(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static T DeserializeRedisValue<T>(CtSharpRedisValue redisValue)
        {
            return (T) SerializeSettings.DeserializeRedisValue<T>(redisValue.Value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static T DeserializeRedisValue<T>(string redisValue)
        {
            return (T)SerializeSettings.DeserializeRedisValue<T>(redisValue);
        }


        private static IRedisValueSerializeSettings SerializeSettings => new DefaultRedisValueSerializeSettings();

        public static implicit operator CtSharpRedisValue(byte[] value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(byte value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(string value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(char value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(int value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(long value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(double value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(float value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(bool value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(DateTime value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(DateTimeOffset value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(TimeSpan value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static implicit operator CtSharpRedisValue(Guid value)
        {
            return new CtSharpRedisValue
            {
                Value = SerializeSettings.SerializeRedisValue(value)
            };
        }

        public static explicit operator byte[] (CtSharpRedisValue redisValue)
        {
            return (byte[])SerializeSettings.DeserializeRedisValue<byte[]>(redisValue.Value);
        }

        public static explicit operator byte(CtSharpRedisValue redisValue)
        {
            return (byte)SerializeSettings.DeserializeRedisValue<byte>(redisValue.Value);
        }

        public static explicit operator char(CtSharpRedisValue redisValue)
        {
            return (char)SerializeSettings.DeserializeRedisValue<char>(redisValue.Value);
        }

        public static explicit operator string(CtSharpRedisValue redisValue)
        {
            return (string)SerializeSettings.DeserializeRedisValue<string>(redisValue.Value);
        }

        public static explicit operator int(CtSharpRedisValue redisValue)
        {
            return (int)SerializeSettings.DeserializeRedisValue<int>(redisValue.Value);
        }

        public static explicit operator long(CtSharpRedisValue redisValue)
        {
            return (long)SerializeSettings.DeserializeRedisValue<long>(redisValue.Value);
        }

        public static explicit operator float(CtSharpRedisValue redisValue)
        {
            return (float)SerializeSettings.DeserializeRedisValue<float>(redisValue.Value);
        }

        public static explicit operator double(CtSharpRedisValue redisValue)
        {
            return (double)SerializeSettings.DeserializeRedisValue<double>(redisValue.Value);
        }

        public static explicit operator bool(CtSharpRedisValue redisValue)
        {
            return (bool)SerializeSettings.DeserializeRedisValue<bool>(redisValue.Value);
        }

        public static explicit operator DateTime(CtSharpRedisValue redisValue)
        {
            return (DateTime)SerializeSettings.DeserializeRedisValue<DateTime>(redisValue.Value);
        }

        public static explicit operator DateTimeOffset(CtSharpRedisValue redisValue)
        {
            return (DateTimeOffset)SerializeSettings.DeserializeRedisValue<DateTimeOffset>(redisValue.Value);
        }

        public static explicit operator TimeSpan(CtSharpRedisValue redisValue)
        {
            return (TimeSpan)SerializeSettings.DeserializeRedisValue<TimeSpan>(redisValue.Value);
        }

        public static explicit operator Guid(CtSharpRedisValue redisValue)
        {
            return (Guid)SerializeSettings.DeserializeRedisValue<TimeSpan>(redisValue.Value);
        }
    }
}