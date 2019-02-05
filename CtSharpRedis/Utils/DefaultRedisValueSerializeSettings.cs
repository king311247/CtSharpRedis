using System;
using System.Text;
using Newtonsoft.Json;

namespace CtSharpRedis.Utils
{
    public class DefaultRedisValueSerializeSettings: IRedisValueSerializeSettings
    {
        private static readonly Type ByteArrayType = typeof(byte[]);

        private static readonly Type ByteType = typeof(byte);

        private static readonly Type CharType = typeof(char);

        private static readonly Type StringType = typeof(string);

        private static readonly Type IntType = typeof(int);

        private static readonly Type DoubleType = typeof(double);

        private static readonly Type FloatType = typeof(float);

        private static readonly Type BoolType = typeof(bool);

        private static readonly Type LongType = typeof(long);

        private static readonly Type DateTimeType = typeof(DateTime);

        private static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);

        private static readonly Type TimeSpanType = typeof(TimeSpan);

        private static readonly Type GuidType = typeof(Guid);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SerializeRedisValue(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var valueType = value.GetType();
            if (valueType == ByteArrayType)
            {
                var byteValue = (byte[])value;
                return Encoding.UTF8.GetString(byteValue);
            }
            if (valueType == ByteType)
            {
                var byteValue = (byte)value;
                return byteValue.ToString();
            }
            if (valueType == CharType)
            {
                return (string)value;
            }
            if (valueType == StringType)
            {
                return (string)value;
            }
            if (valueType == IntType)
            {
                return value.ToString();
            }
            if (valueType == DoubleType)
            {
                return value.ToString();
            }
            if (valueType == FloatType)
            {
                return value.ToString();
            }
            if (valueType == BoolType)
            {
                return (bool) value ? "1" : "0";
            }
            if (valueType == LongType)
            {
                return value.ToString();
            }

            if (valueType == DateTimeType)
            {
                return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:sszzzz", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }

            if (valueType == DateTimeOffsetType)
            {
                return value.ToString();
            }

            if (valueType == TimeSpanType)
            {
                return ((TimeSpan)value).Ticks.ToString();
            }

            if (valueType == GuidType)
            {
                return value.ToString();
            }

            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public object DeserializeRedisValue<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            var valueType = typeof(T);
            if (valueType == ByteArrayType)
            {
                return Encoding.UTF8.GetBytes(value);
            }
            if (valueType == ByteType)
            {
                return Convert.ChangeType(value, valueType);
            }
            if (valueType == CharType)
            {
                return Convert.ChangeType(value, valueType);
            }
            if (valueType == StringType)
            {
                return value;
            }
            if (valueType == IntType)
            {
                return Convert.ChangeType(value, valueType);
            }
            if (valueType == DoubleType)
            {
                return Convert.ChangeType(value, valueType);
            }
            if (valueType == FloatType)
            {
                return Convert.ChangeType(value, valueType);
            }
            if (valueType == BoolType)
            {
                if (value == "1")
                {
                    return true;
                }
                if (value == "0")
                {
                    return false;
                }
                if (value.ToLower() == "true")
                {
                    return true;
                }
                if (value.ToLower() == "false")
                {
                    return true;
                }
                throw new ArgumentException("value can't convert to bool");
            }
            if (valueType == LongType)
            {
                return Convert.ChangeType(value, valueType);
            }

            if (valueType == DateTimeType)
            {
                return DateTime.Parse(value);
            }

            if (valueType == DateTimeOffsetType)
            {
                if (DateTimeOffset.TryParse(value, out var timeOffset))
                {
                    return timeOffset;
                }

                throw new ArgumentException("value can't convert to DateTimeOffset");
            }

            if (valueType == TimeSpanType)
            {
                if (Int64.TryParse(value, out var intResult))
                {
                    return new TimeSpan(intResult);
                }
                throw new ArgumentException("value can't convert to TimeSpan");

            }

            if (valueType == GuidType)
            {
                if (Guid.TryParse(value, out var guid))
                {
                    return guid;
                }
                throw new ArgumentException("value can't convert to Guid");
            }

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
