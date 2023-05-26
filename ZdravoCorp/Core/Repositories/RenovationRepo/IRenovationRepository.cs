using ZdravoCorp.Core.Models.Renovation;

namespace ZdravoCorp.Core.Repositories.RenovationRepo;

public interface IRenovationRepository: IRepository<Renovation>
{
    void UpdateStatus(int id, Renovation.RenovationStatus status);
}