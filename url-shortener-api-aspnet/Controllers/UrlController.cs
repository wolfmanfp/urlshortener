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

        public UrlController(RedisService svc)
        {
            redisService = svc;
        }

        [HttpGet("{hash}")]
        public async Task<ActionResult<string>> GetUrl(string hash)
        {
            string url = await redisService.Get(hash);
            if (url == null)
            {
                return NotFound();
            }
            return Ok(new { url = url });
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostUrl([FromBody] PostRequest req)
        {
            string hash = GetHash(req.Url);
            await redisService.Set(hash, req.Url);
            return CreatedAtAction(nameof(GetUrl), new { hash = hash }, new { hash = hash });
        }

        private string GetHash(string url) {
            Base62Converter base62 = new Base62Converter();
            string hash = base62.Encode(GetMd5Hash(url));
            return hash.Substring(0, 6);
        }

        private string GetMd5Hash(string url)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(url);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}