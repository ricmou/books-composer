using System.Collections.Generic;
using BookAPIComposer.Domain.Fetching.APIBooks.Books;

namespace BookAPIComposer.Domain.Requests.Books;

public class CreatingBook
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string Language { get; set; }
    
    public List<BookDescriptionsDto> Description { get; set; } 
    
    public string PublisherId { get;  set; }
    
    public List<string> Authors { get; set; }
    
    public List<string> Categories { get; set; }

    public CreatingBook(string id, string title, string language, List<BookDescriptionsDto> description, string publisherId, List<string> authors, List<string> categories)
    {
        Id = id;
        Title = title; 
        Language = language;
        Description = description;
        PublisherId = publisherId;
        Authors = authors;
        Categories = categories;
    }
}