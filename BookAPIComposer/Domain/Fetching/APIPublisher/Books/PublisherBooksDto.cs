using BookAPIComposer.Domain.Fetching.APIPublisher.Publishers;

namespace BookAPIComposer.Domain.Fetching.APIPublisher.Books
{
    public class PublisherBooksDto
    {
        public string Id { get; set; }
        
        public PublisherDto Publisher { get; set; }

        public PublisherBooksDto(string id, PublisherDto publisher)
        {
            this.Id = id;
            this.Publisher = publisher;
        }

    }
}