using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Core.Abstractions;

namespace Application
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _connection;
        public RedisService()
        {
            _connection = ConnectionMultiplexer.Connect("localhost");
        }
        public IDatabase Db => _connection.GetDatabase();
    }
}
