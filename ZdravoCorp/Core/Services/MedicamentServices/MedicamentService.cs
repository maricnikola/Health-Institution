using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Medicaments;
using ZdravoCorp.Core.Repositories.MedicamentsRepo;

namespace ZdravoCorp.Core.Services.MedicamentServices;

public class MedicamentService : IMedicamentService
{
    private IMedicamentRepository _medicamentRepository;

    public MedicamentService(IMedicamentRepository medicamentRepository)
    {
        _medicamentRepository = medicamentRepository;
    }

    public List<Medicament>? GetAll()
    {
        return _medicamentRepository.GetAll() as List<Medicament>;
    }

    public Medicament? GetByName(string name)
    {
        return _medicamentRepository.GetById(name);
    }

    public void AddMedicament(Medicament newMedicament)
    {
        _medicamentRepository.Insert(newMedicament);
    }

    public void Delete(string name)
    {
        _medicamentRepository.Delete(GetByName(name));
    }
    public List<string> GetAllMedicamentNames()
    {
        List<string> medicaments = new List<string>();
        foreach(Medicament medicament in _medicamentRepository.GetAll())
        {
            medicaments.Add(medicament.Name);
        }
        return medicaments;
    }
}
