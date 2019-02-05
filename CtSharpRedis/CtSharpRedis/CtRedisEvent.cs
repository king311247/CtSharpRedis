using System;
using System.Collections.Generic;
using System.Text;

namespace CtSharpRedis
{
    /// <summary>
    /// 日志事件
    /// </summary>
    public class CtRedisEvent
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
