namespace BookAPIComposer.Domain.Requests.Books;

public class BookDeletionDto
{
    public bool Author { get; set; }
    public bool Book { get; set; }
    public bool Category { get; set; }
    public bool Publisher { get; set; }

    public BookDeletionDto(bool author, bool book, bool category, bool publisher)
    {
        Author = author;
        Book = book;
        Category = category;
        Publisher = publisher;
    }
}