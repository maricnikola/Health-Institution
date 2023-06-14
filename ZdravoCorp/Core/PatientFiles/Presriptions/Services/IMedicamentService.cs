using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Presriptions.Models;

namespace ZdravoCorp.Core.PatientFiles.Presriptions.Services;

public interface IMedicamentService
{
    public List<Medicament>? GetAll();
    public Medicament? GetByName(string name);
    public void AddMedicament(Medicament medicament);
    public void Delete(string name);
    public List<string> GetAllMedicamentNames();
}
