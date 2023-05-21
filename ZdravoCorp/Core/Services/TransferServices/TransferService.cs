﻿using System.Collections.Generic;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.TransfersRepo;

namespace ZdravoCorp.Core.Services.TransferServices;

public class TransferService : ITransferService
{
    private ITransferRepository _transferRepository;
    public TransferService(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }
    public List<Transfer>? GetAll()
    {
        return _transferRepository.GetAll() as List<Transfer>;
    }

    public Transfer? GetById(int id)
    {
        return _transferRepository.GetById(id);
    }

    public void AddTransfer(TransferDTO transferDto)
    {
        _transferRepository.Insert(new Transfer(transferDto));
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
    }

    public void Delete(int id)
    {
        _transferRepository.Delete(_transferRepository.GetById(id));
    }
}