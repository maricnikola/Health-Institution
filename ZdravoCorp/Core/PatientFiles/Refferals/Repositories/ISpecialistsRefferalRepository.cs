using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Repositories;

public interface ISpecialistsRefferalRepository
{
    public void Insert(SpecialistsRefferal newSpecialistsRefferal);
    public void SaveChanges();
    public List<SpecialistsRefferal> GetAll();
    public void Delete(SpecialistsRefferal entity);
    public SpecialistsRefferal GetById(int id);
}
