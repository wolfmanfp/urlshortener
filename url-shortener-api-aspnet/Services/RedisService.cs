using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Wolfman.UrlShortener {
    public class RedisService {
        private readonly string host;
        private readonly int port;
        private readonly int database;
        private readonly string password;
        private readonly bool ssl;
        private ConnectionMultiplexer redis;

        public RedisService(IConfiguration config) {
            host = config["Redis:Host"];
            port = Convert.ToInt32(config["Redis:Port"]);
            database = Convert.ToInt32(config["Redis:Database"]);
            ssl = Convert.ToBoolean(config["Redis:Ssl"]);

            string pwString = config["Redis:Password"];
            if (string.IsNullOrEmpty(pwString)) {
                password = "null";
            }
            else {
                password = pwString;
            }
        }

        public void Connect() {
            try
            {
                redis = ConnectionMultiplexer.Connect(new ConfigurationOptions {
                    EndPoints = {{host, port}},
                    DefaultDatabase = database,
                    Password = password,
                    Ssl = ssl,
                    ConnectRetry = 5
                });
            }
            catch (RedisConnectionException ex)
            {
                throw ex;
            }
        }

        public async Task<string> Get(string key) {
            IDatabase db = redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task Set(string key, string value) {
            IDatabase db = redis.GetDatabase();
            await db.StringSetAsync(key, value);
        }
    }
}
