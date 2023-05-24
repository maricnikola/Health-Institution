using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.SpecialistsRefferals;

namespace ZdravoCorp.Core.Repositories.SpecialistsRefferalsRepo;

public interface ISpecialistsRefferalRepository
{
    public void Insert(SpecialistsRefferal newSpecialistsRefferal);
    public void SaveChanges();
    public List<SpecialistsRefferal> GetAll();
    public void Delete(SpecialistsRefferal entity);
    public SpecialistsRefferal GetById(int id);
}
