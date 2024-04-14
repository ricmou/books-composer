using BookAPIComposer.Domain.Fetching.APIClients.Clients;

namespace BookAPIComposer.Domain.Requests.Exemplar;

public class Exemplar
{
    public string ExemplarId { get; set; }
    
    public int BookState { get; set; }
    
    public string DateOfAcquisition { get; set; }
    
    public ClientDto Seller { get; set; }

    public Exemplar(string exemplarId, int bookState, string dateOfAcquisition, ClientDto seller)
    {
        ExemplarId = exemplarId;
        BookState = bookState;
        DateOfAcquisition = dateOfAcquisition;
        Seller = seller;
    }
}