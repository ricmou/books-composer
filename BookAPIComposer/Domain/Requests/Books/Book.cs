using System.Collections.Generic;
using BookAPIComposer.Domain.Fetching.APIBooks.Books;
using BookAPIComposer.Domain.Requests.Authors;
using BookAPIComposer.Domain.Requests.Categories;
using BookAPIComposer.Domain.Requests.Publishers;

namespace BookAPIComposer.Domain.Requests.Books;

public class Book
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string Language { get; set; }
    
    public IList<BookDescriptionsDto> Description { get; set; } 
    
    public Publisher Publisher { get;  set; }
    
    public IList<Author> Authors { get; set; }
    
    public IList<Category> Categories { get; set; }

    public Book(string id, string title, string language, IList<BookDescriptionsDto> description, Publisher publisher, IList<Author> authors, IList<Category> categories)
    {
        Id = id;
        Title = title;
        Language = language;
        Description = description;
        Publisher = publisher;
        Authors = authors;
        Categories = categories;
    }
}