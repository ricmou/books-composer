﻿using System;
using Newtonsoft.Json;

namespace BookAPIComposer.Domain.Shared;

public class ExemplarId : EntityId
{
    [JsonConstructor]
    public ExemplarId(Guid value) : base(value)
    {
    }

    public ExemplarId(String value) : base(value)
    {
    }
    
    protected override Object createFromString(String text)
    {
        return new Guid(text);
    }
    
    public override String AsString()
    {
        Guid obj = (Guid)base.ObjValue;
        return obj.ToString();
    }

    public Guid AsGuid()
    {
        return (Guid)base.ObjValue;
    }
}