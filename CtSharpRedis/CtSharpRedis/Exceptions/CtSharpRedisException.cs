using System;

namespace CtSharpRedis.Exceptions
{
    /// <summary>
    /// redis异常
    /// </summary>
    public class CtSharpRedisException:Exception
    {
        //
        // 摘要:
        //     Initializes a new instance of the System.Exception class.
        public CtSharpRedisException():base()
        {

        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message.
        //
        // 参数:
        //   message:
        //     The message that describes the error.
        public CtSharpRedisException(string message) : base(message)
        {

        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // 参数:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public CtSharpRedisException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
