using System.Collections.Generic;
using BookAPIComposer.Domain.Fetching.APIAuthors.Authors;

namespace BookAPIComposer.Domain.Fetching.APIAuthors.Books;

public class AuthorsBooksDto
{
    public string Id { get; set; }
        
    public List<AuthorDto> Authors { get; set; }

    public AuthorsBooksDto(string id, List<AuthorDto> authors)
    {
        this.Id = id;
        this.Authors = authors;
    }

}