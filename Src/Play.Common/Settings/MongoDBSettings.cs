using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace play.common.settings
{
    public class MongoDBSettings
    {
        public string? Host { get; init; }
        public int Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}