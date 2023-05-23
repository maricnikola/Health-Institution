using ZdravoCorp.Core.Models.Transfers;

namespace ZdravoCorp.Core.Repositories.TransfersRepo;

public interface ITransferRepository : IRepository<Transfer>
{
    void UpdateStatus(int id, Transfer.TransferStatus status);
}