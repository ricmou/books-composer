using System.Collections.Generic;
using BookAPIComposer.Domain.Fetching.APICategories.Categories;

namespace BookAPIComposer.Domain.Fetching.APICategories.Books;

public class CategoryBooksDto
{
    public string Id { get; set; }
        
    public List<CategoryDto> Categories { get; set; }

    public CategoryBooksDto(string id, List<CategoryDto> categories)
    {
        this.Id = id;
        this.Categories = categories;
    }

}