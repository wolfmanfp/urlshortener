using Base62;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Wolfman.UrlShortener
{

    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly RedisService redisService;

        public class PostRequest {
            public string Url { get; set; }
        }

        public UrlController(RedisService svc) {
            redisService = svc;
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<string>> GetUrl(string hash) {
            string url = await redisService.Get(hash);
            return Ok(new {url = url});
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] PostRequest req) {
            Base62Converter base62 = new Base62Converter();
            Random random = new Random();

            string hash = base62.Encode(random.Next(10000, 99999).ToString());
            await redisService.Set(hash, req.Url);
            return CreatedAtAction(nameof(GetUrl), new {hash = hash}, new {hash = hash});
        }
    }
}