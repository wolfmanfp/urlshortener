using Base62;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wolfman.UrlShortener
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly RedisService redisService;

        public class PostRequest
        {
            public string Url { get; set; }
        }

        public UrlController(RedisService redisService)
        {
            this.redisService = redisService;
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<string>> GetUrl(string hash)
        {
            string url = await redisService.Get(hash);
            if (url == null)
            {
                return NotFound();
            }
            return Ok(new { url });
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] PostRequest req)
        {
            string hash = GetHash(req.Url);
            await redisService.Set(hash, req.Url);
            return CreatedAtAction(nameof(GetUrl), new { hash }, new { hash });
        }

        private static string GetHash(string url) 
        {
            Base62Converter base62 = new();
            string hash = base62.Encode(GetMd5Hash(url));
            return hash[..6];
        }

        private static string GetMd5Hash(string url)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(url);
            byte[] hashBytes = MD5.HashData(inputBytes);

            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}