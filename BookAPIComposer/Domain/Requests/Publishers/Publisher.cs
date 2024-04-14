namespace BookAPIComposer.Domain.Requests.Publishers;

public class Publisher
{
    public string PublisherId { get; set; }
    
    public string Name { get; set; }
    
    public string Country { get; set; }

    public Publisher(string publisherId, string name, string country)
    {
        PublisherId = publisherId;
        Name = name;
        Country = country;
    }
}