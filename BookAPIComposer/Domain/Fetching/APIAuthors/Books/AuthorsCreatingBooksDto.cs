using System.Collections.Generic;

namespace BookAPIComposer.Domain.Fetching.APIAuthors.Books
{
    public class AuthorsCreatingBooksDto
    {
        public string Id { get; set; }

        public List<string> Authors { get; set; }
        
        public AuthorsCreatingBooksDto(string id, List<string> authors)
        {
            this.Id = id;
            this.Authors = authors;
        }
    }
}