namespace BookAPIComposer.Domain.Requests.Categories;

public class Category
{
    public string CategoryId { get; set; }
    
    public string Name { get; set; }

    public Category(string categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }
}