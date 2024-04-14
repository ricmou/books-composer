namespace BookAPIComposer.Domain.Fetching.APIPublisher.Books
{
    public class PublisherCreatingBooksDto
    {
        public string Id { get; set; }

        public string PublisherId { get; set; }
        
        public PublisherCreatingBooksDto(string id, string publisher)
        {
            this.Id = id;
            this.PublisherId = publisher;
        }
    }
}