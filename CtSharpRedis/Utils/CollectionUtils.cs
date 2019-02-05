using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CtSharpRedis.Utils
{
    public static class CollectionUtils
    {
        /// <summary>
        /// 是否空数组
        /// </summary>
        /// <param name="valueArrary"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyArrary<T>(this T[] valueArrary)
        {
            if (valueArrary == null)
            {
                return true;
            }

            if (valueArrary.Length <= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否空字典
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyDictionary<TKey,TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                return true;
            }

            if (dictionary.Count <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
