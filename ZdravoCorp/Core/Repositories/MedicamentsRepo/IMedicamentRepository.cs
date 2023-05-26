using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Medicaments;
using ZdravoCorp.Core.Models.SpecialistsRefferals;

namespace ZdravoCorp.Core.Repositories.MedicamentsRepo;

public interface IMedicamentRepository
{
    public void Insert(Medicament newMedicament);
    public void SaveChanges();
    public List<Medicament> GetAll();
    public void Delete(Medicament entity);
    public Medicament GetById(string name);
}

