using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Repositories;

public interface ITransferRepository : IRepository<Transfer>
{
    void UpdateStatus(int id, Transfer.TransferStatus status);
}