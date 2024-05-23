using Microsoft.EntityFrameworkCore;

namespace UrlShortenerEngine.Services
{
    public class ShortenUrlService
    {
        public const int NumberOfCharsInShortenLink = 6;
        private const string CharsUsedForCreateShortLink =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        private readonly Random _random = new Random();
        private readonly ApplicationDbContext _dbcontext;

        public ShortenUrlService(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<string> GenerateUniqueCodeForShortUrl()
        {
            var uniqueCode = new char[NumberOfCharsInShortenLink];
            while (true) 
            { 
                for (int i = 0; i < uniqueCode.Length; i++)
                {
                    var randomIndex = _random.Next(CharsUsedForCreateShortLink.Length - 1);
                    uniqueCode[i] = CharsUsedForCreateShortLink[randomIndex];
                }

                var Code = new string(uniqueCode);
                if( !await _dbcontext.shortenedUrls.AnyAsync( c => c.CodeAdded == Code))
                {
                    return Code;
                }
            }
        }

    }
}
