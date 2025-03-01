using StackExchange.Redis;

namespace Core.Abstractions
{
    public interface IRedisService
    {
        IDatabase Db { get; }
    }
}