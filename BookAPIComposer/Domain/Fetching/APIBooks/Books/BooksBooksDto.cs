﻿using System.Collections.Generic;

namespace BookAPIComposer.Domain.Fetching.APIBooks.Books;

public class BooksBooksDto
{
    public string isbn { get; set; }
    
    public string title { get; set; }
    
    public string language { get; set; }

    public List<BookDescriptionsDto> descriptions { get; set; }

    public BooksBooksDto(string isbn, string title, string language, List<BookDescriptionsDto> descriptions)
    {
        this.isbn = isbn;
        this.title = title;
        this.language = language;
        this.descriptions = descriptions;
    }
}