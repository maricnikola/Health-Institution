using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Medicaments;
using ZdravoCorp.Core.Models.SpecialistsRefferals;

namespace ZdravoCorp.Core.Services.MedicamentServices;

public interface IMedicamentService
{
    public List<Medicament>? GetAll();
    public Medicament? GetByName(string name);
    public void AddMedicament(Medicament medicament);
    public void Delete(string name);
    public List<string> GetAllMedicamentNames();
}
