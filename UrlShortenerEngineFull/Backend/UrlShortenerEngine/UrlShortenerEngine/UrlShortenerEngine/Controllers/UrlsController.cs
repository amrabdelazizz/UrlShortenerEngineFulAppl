using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerEngine.Entities;
using UrlShortenerEngine.Models;
using UrlShortenerEngine.Services;

namespace UrlShortenerEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        //private readonly ShortenUrlRequest _shortenUrlRequest;
        private readonly ShortenUrlService _shortenUrlService;
        private readonly ApplicationDbContext _dbcontext;

        public UrlsController( ShortenUrlService shortenUrlService, ApplicationDbContext context)
        {
           // _shortenUrlRequest = shortenUrlRequest;
            _shortenUrlService = shortenUrlService;
            _dbcontext = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateShortUrl([FromBody] ShortenUrlRequest shortenUrlRequest)
        {
            if (!Uri.TryCreate(shortenUrlRequest.URL, UriKind.Absolute, out _))
            {
                return BadRequest("Invalid Url, Please Enter a Valid One .");
            }

            var urlExist = await _dbcontext.shortenedUrls.FirstOrDefaultAsync(
                    u => u.LongUrl == shortenUrlRequest.URL
                ); 
            if (urlExist is not  null)
            {
                return Ok(new { ShortUrl = urlExist.ShortUrl });
            }

            var uniqueCode = await _shortenUrlService.GenerateUniqueCodeForShortUrl();

            var shortenendUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = shortenUrlRequest.URL,
                ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/Urls/{uniqueCode}",
                CodeAdded = uniqueCode,
                DateCreated = DateTime.Now
            };

            _dbcontext.shortenedUrls.Add(shortenendUrl);
            await _dbcontext.SaveChangesAsync();

            return Ok(new {ShortUrl = shortenendUrl.ShortUrl});

        }
        [HttpGet("{code}")]
        public async Task<IActionResult> getOrginalUrl(string code)
        {
            var shortenedUrl = await _dbcontext.shortenedUrls.
                FirstOrDefaultAsync(c => c.CodeAdded == code);

            if(shortenedUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortenedUrl.LongUrl);
        }
    }
}
