using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Services;

public interface ISpecialistsRefferalService
{
    public List<SpecialistsRefferal>? GetAll();
    public SpecialistsRefferal? GetById(int id);
    public void AddRefferal(SpecialistsRefferal specialistsRefferal);
    public void Delete(int id);
}
