namespace CtSharpRedis.Enums
{
    //
    // 摘要:
    //     Indicates when this operation should be performed (only some variations are legal
    //     in a given context)
    public enum CtSharpWhen
    {
        //
        // 摘要:
        //     The operation should occur whether or not there is an existing value
        Always = 0,
        //
        // 摘要:
        //     The operation should only occur when there is an existing value
        Exists = 1,
        //
        // 摘要:
        //     The operation should only occur when there is not an existing value
        NotExists = 2
    }
}
