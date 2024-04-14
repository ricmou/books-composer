namespace BookAPIComposer.Domain.Requests.Authors;

public class Author
{
    public string AuthorId { get; private set; }
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string BirthDate { get; private set; }
    
    public string Country { get; private set; }

    public Author(string authorId, string firstName, string lastName, string birthDate, string country)
    {
        this.AuthorId = authorId;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.BirthDate = birthDate;
        this.Country = country;
    }
}