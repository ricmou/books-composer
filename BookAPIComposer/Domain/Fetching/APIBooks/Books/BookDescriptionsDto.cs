namespace BookAPIComposer.Domain.Fetching.APIBooks.Books;

public class BookDescriptionsDto
{
    public string text { get; set; }
    public string language { get; set; }

    public BookDescriptionsDto(string text, string language)
    {
        this.text = text;
        this.language = language;
    }
}