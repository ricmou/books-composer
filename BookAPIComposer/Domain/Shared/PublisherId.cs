using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BookAPIComposer.Domain.Shared;

public class PublisherId : EntityId
{
    [JsonConstructor]
    public PublisherId(String value) : base(IsValidPublisherID(value))
    {
    }


    protected override Object createFromString(String text)
    {
        return text;
    }


    public override String AsString()
    {
        var obj = (string)base.ObjValue;
        return obj;
    }


    //This is most definitely a hack...
    private static string IsValidPublisherID(string bookId)
    {
        if (string.IsNullOrEmpty(bookId) || !Regex.IsMatch(bookId, "[A-Za-z0-9]{3}"))
        {
            throw new BusinessRuleValidationException("Invalid Category ID");
        }

        return bookId;
    }
}