using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.HospitalAssets.Equipment.Repositories;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;

public class TransferService : ITransferService
{
    private ITransferRepository _transferRepository;
    public TransferService(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }

    public event EventHandler? DataChanged;

    public List<Transfer> GetAll()
    {
        return _transferRepository.GetAll() as List<Transfer> ?? throw new InvalidOperationException();
    }

    public Transfer? GetById(int id)
    {
        return _transferRepository.GetById(id);
    }

    public void AddTransfer(TransferDTO transferDto)
    {
        _transferRepository.Insert(new Transfer(transferDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Update(int id, TransferDTO transferDto)
    {
        var oldTransfer = _transferRepository.GetById(id);
        if (oldTransfer == null)
        {
            throw new KeyNotFoundException();
        }
        _transferRepository.Delete(oldTransfer);
        _transferRepository.Insert(new Transfer(transferDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void UpdateStatus(int id, Transfer.TransferStatus status)
    {
        _transferRepository.UpdateStatus(id, status);
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Delete(int id)
    {
        _transferRepository.Delete(_transferRepository.GetById(id));
        DataChanged?.Invoke(this, new EventArgs());
    }
}