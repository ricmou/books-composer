using System;

namespace BookAPIComposer.Services.Shared;

public class NullApiException : Exception
{
    public string Details { get; }

    public NullApiException(string message) : base(message)
    {
            
    }

    public NullApiException(string message, string details) : base(message)
    {
        this.Details = details;
    }
}