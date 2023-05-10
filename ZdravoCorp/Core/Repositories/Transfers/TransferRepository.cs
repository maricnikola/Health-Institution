﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.Transfers;

public class TransferRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\transfers.json";
    private List<Transfer>? _transfers;

    public TransferRepository()
    {
        _transfers = new List<Transfer>();
        Serializer.Load(this);
        
    }

    public List<Transfer> GetAll()
    {
        return _transfers;
    }
    public void Add(Transfer transfer)
    {
        _transfers.Add(transfer);
    }
    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _transfers;
    }

    public void Import(JToken token)
    {
        _transfers = token.ToObject<List<Transfer>>();
    }
}