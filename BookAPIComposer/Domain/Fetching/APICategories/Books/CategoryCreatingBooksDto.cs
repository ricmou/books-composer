using System.Collections.Generic;

namespace BookAPIComposer.Domain.Fetching.APICategories.Books
{
    public class CategoryCreatingBooksDto
    {
        public string Id { get; set; }

        public List<string> Categories { get; set; }
        
        public CategoryCreatingBooksDto(string id, List<string> categories)
        {
            this.Id = id;
            this.Categories = categories;
        }
    }
}