namespace UrlShortenerEngine.Entities
{
    public class ShortenedUrl
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public string CodeAdded { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

    }
}
