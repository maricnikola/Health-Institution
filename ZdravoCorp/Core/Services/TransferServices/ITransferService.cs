using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Transfers;

namespace ZdravoCorp.Core.Services.TransferServices;

public interface ITransferService
{
    public event EventHandler DataChanged;
    public List<Transfer> GetAll();
    public Transfer? GetById(int id);
    public void AddTransfer(TransferDTO transferDtoDto);

    public void Update(int id, TransferDTO transferDto);

    public void UpdateStatus(int id, Transfer.TransferStatus status);

    public void Delete(int id);
    
}