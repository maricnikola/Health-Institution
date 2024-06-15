using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Presriptions.Models;
using ZdravoCorp.Core.PatientFiles.Presriptions.Repositories;

namespace ZdravoCorp.Core.PatientFiles.Presriptions.Services;

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
