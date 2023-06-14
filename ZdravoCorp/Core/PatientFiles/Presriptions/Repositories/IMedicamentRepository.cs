using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Presriptions.Models;

namespace ZdravoCorp.Core.PatientFiles.Presriptions.Repositories;

public interface IMedicamentRepository
{
    public void Insert(Medicament newMedicament);
    public void SaveChanges();
    public List<Medicament> GetAll();
    public void Delete(Medicament entity);
    public Medicament GetById(string name);
}

