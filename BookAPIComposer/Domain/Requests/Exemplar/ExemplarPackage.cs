using System.Collections.Generic;
using BookAPIComposer.Domain.Requests.Books;

namespace BookAPIComposer.Domain.Requests.Exemplar;

public class ExemplarPackage
{
    public Book Book { get; set; }
    
    public List<Exemplar> Exemplars { get; set; }

    public ExemplarPackage(Book book, List<Exemplar> exemplars)
    {
        Book = book;
        Exemplars = exemplars;
    }

    public ExemplarPackage(Book book)
    {
        Book = book;
        Exemplars = new List<Exemplar>();
    }
    
    
}