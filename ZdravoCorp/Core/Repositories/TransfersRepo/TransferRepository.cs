using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.TransfersRepo;

public class TransferRepository : ISerializable, ITransferRepository
{
    private readonly string _fileName = @".\..\..\..\Data\transfers.json";
    private List<Transfer>? _transfers;


    public EventHandler OnRequestUpdate;

    public TransferRepository()
    {
        _transfers = new List<Transfer>();
        Serializer.Load(this);
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

    public IEnumerable<Transfer> GetAll()
    {
        return _transfers;
    }

    public void Insert(Transfer transfer)
    {
        _transfers.Add(transfer);
    }

    public void Delete(Transfer transfer)
    {
        _transfers.Remove(transfer);
        Serializer.Save(this);
    }

    public Transfer GetById(int id)
    {
        return _transfers.FirstOrDefault(transfer => transfer.Id == id);
    }
}