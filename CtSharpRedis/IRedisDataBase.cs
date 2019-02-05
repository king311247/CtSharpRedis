namespace CtSharpRedis
{
    public interface IRedisDataBase: IKeysCommand, IHashesCommands, IHyperLogLogCommands, IListsCommands, IPubSubCommands, ISetsCommands, ISortedSetsCommands, IStringsCommands
    {
    }
}
