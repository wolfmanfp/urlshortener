using Base62;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Wolfman.UrlShortener
{

    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

        public class PostRequest {
            public string Url { get; set; }
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<string>> GetUrl(string hash) {
            IDatabase db = redis.GetDatabase();
            string url = await db.StringGetAsync(hash);
            return Ok(new {url = url});
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] PostRequest req) {
            IDatabase db = redis.GetDatabase();
            Base62Converter base62 = new Base62Converter();
            Random random = new Random();

            string hash = base62.Encode(random.Next(10000, 99999).ToString());
            await db.StringSetAsync(hash, req.Url);
            return CreatedAtAction(nameof(GetUrl), new {hash = hash}, new {hash = hash});
        }
    }
}