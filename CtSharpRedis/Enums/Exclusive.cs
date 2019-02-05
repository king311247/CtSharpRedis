namespace CtSharpRedis.Enums
{
    public enum Exclusive
    {
        //
        // 摘要:
        //     Both start and stop are inclusive
        None = 0,
        //
        // 摘要:
        //     Start is exclusive, stop is inclusive
        Start = 1,
        //
        // 摘要:
        //     Start is inclusive, stop is exclusive
        Stop = 2,
        //
        // 摘要:
        //     Both start and stop are exclusive
        Both = 3
    }
}
